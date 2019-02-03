//
// LICENSE:
// This work is licensed under the
//     Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// also known as CC-BY-NC-SA.  To view a copy of this license, visit
//      http://creativecommons.org/licenses/by-nc-sa/3.0/
// or send a letter to
//      Creative Commons // 171 Second Street, Suite 300 // San Francisco, California, 94105, USA.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clio.Utilities;
using DungeonMaster.Attributes;
using DungeonMaster.DungeonProfile;
using DungeonMaster.Enumeration;
using DungeonMaster.Helpers;
using DungeonMaster.Managers;
using DungeonMaster.TargetingSystems;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class AlaMhigo : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 56;
        public override string Name => @"Ala Mhigo";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Magitek Scorpion", 6037, 0, 23.47f, new Vector3(-185.00f, 34.87f, 72.00f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Aulus mal Asina", 6038, 1, 28.27f, new Vector3(250.00f, 106.45f, -80.00f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Zenos yae Galvus", 6039, 2, 34.09f, new Vector3(249.99f, 122.00f, -365.01f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Wall before Magitek Scorpion
            new OffMeshConnection(new Vector3(-260.742f, 27f, 97.70065f), new Vector3(-254.6938f, 27.00001f, 97.64568f), ConnectionMode.Bidirectional),
            // Transition line before/after Magitek Scorpion
            new OffMeshConnection(new Vector3(-157.1929f, 34.86707f, 71.85528f), new Vector3(130.1971f, 108.4f, 72.49117f), ConnectionMode.OneWay)
        };

        private uint[] _artOfTheSword = new uint[] { 9609, 9608, 8993, 8992, 8297, 8296 };
        private uint[] _artOfTheStorm = new uint[] { 9607, 8294 };
        private uint[] _artOfTheSwell = new uint[] { 9606, 8293 };

        public override void OnEnter()
        {
            //2008685 - "Target Search"
            AddAvoidObject<EventObject>(3, 2008685);

            //2002735 ??
            //TODO not sure how this spell is done. this is fore the forward and back lazer
            AddAvoidLine(
                () => CurrentBoss.NpcId == 6037 && GameObjectManager.GetObjectByNPCId(108) != null && GameObjectManager.GetObjectByNPCId(108).IsVisible,
                x => x.Location,
                x => x.Heading,
                x => 60f,
                x => 20f,
                () => GameObjectManager.GetObjectsByNPCId(6037).Where(i => i.IsVisible), //can be created more than once?
                () => GameObjectManager.GetObjectByNPCId(6037).Location,
                40f
                );
            //for the lazer going the other direction
            AddAvoidLine(
                () => CurrentBoss.NpcId == 6037 && GameObjectManager.GetObjectByNPCId(108) != null && GameObjectManager.GetObjectByNPCId(108).IsVisible,
                x => x.Location,
                x => x.Heading - (float)Math.PI,
                x => 60f,
                x => 20f,
                () => GameObjectManager.GetObjectsByNPCId(6037).Where(i => i.IsVisible), //can be created more than once?
                () => GameObjectManager.GetObjectByNPCId(6037).Location,
                40f
                );

            //todo avoids on the way to boss 2. need to verify the EObj ids for them
            AddAvoidObject<EventObject>(10, 2007457); //cannon shot telegraphs

            //boss 2
            AddAvoidObject<BattleCharacter>(5, 6390); //prototype death claw

            //boss 3
            //Art of the Sword(9609 or 9608 or 8993 or 8992 or 8297 or 8296 ?) => Move 10 - 15y away from others (and the boss).
            AddAvoidObject<GameObject>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _artOfTheSword.Contains(i.CastingSpellId)),
                10,
                x => x.NpcId == 6039 || (x.Type == ff14bot.Enums.GameObjectType.Pc && !x.IsMe),
                x => x.Location
                );
            //Art of the Storm (9607 or 8294?) => Move 20y from boss.
            AddAvoidObject<BattleCharacter>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _artOfTheStorm.Contains(i.CastingSpellId)),
                10,
                x => x.NpcId == 6039,
                x => x.Location
                );
            //TODO: add vein splitter clones? not sure if they are bnpcs or not. Ignore if they are bnpcs
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // The Swell
                if (obj.Object.NpcId == 6041)
                    obj.Score += 3000;
                // Ame-no-Habakiri
                if (obj.Object.NpcId == 6040)
                    obj.Score += 2000;
                // The Storm
                if (obj.Object.NpcId == 6042)
                    obj.Score += 1000;
                // Magitek Laserfield
                if (obj.Object.NpcId == 6036)
                    obj.Score += 1000;
            }
        }

        private Vector3 _mindJackPos = Vector3.Zero;

        [EncounterHandler(6038, "Aulus mal Asina", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> AulusMalAsinaHandler(BattleCharacter c)
        {
            if(c.IsCasting && (c.CastingSpellId == 8954 || c.CastingSpellId == 8270))
            {
                _mindJackPos = Core.Me.Location;

                //we can't do combat while he is casting MindJack.
                return true;
            }

            if(Core.Me.HasAura(779))
            {
                if(_mindJackPos != Vector3.Zero)
                    await CommonTasks.MoveTo(_mindJackPos);
                else
                {
                    Logger.Warning("We don't know where our body is");
                    _mindJackPos = GameObjectManager.GetObjectsByNPCId(6666).OrderByDescending(i => i.Distance2D()).First().Location;
                    await CommonTasks.MoveTo(_mindJackPos);
                }
                return true; //can't do combat here
            }

            return false;
        }
        [EncounterHandler(6039, "Zenos yae Galvus", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> ZenosYaeGalvusHandler(BattleCharacter c)
        {
            if(c.IsCasting && _artOfTheSwell.Contains(c.CastingSpellId))
            {
                if(Core.Me.Distance2D(c) > 5f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Zenos yae Galvus"), 5f, true);
                    return true;
                }
            }

            //todo detect if we have the tether
            //if(HAVE TETHER){
            //249.99f, 122.00f, -365.01

            //var headingInRad = MoveResultExtensions.CalculateNeededFacing(CurrentBoss.Location, c.Location);
            //var pointClosestToWall = MathEx.GetPointAt(c.Location, 3, headingInRad);
            //Move toward that point
            //await CommonTasks.MoveTo(pointClosestToWall);
            //return true
            //}

            return false;
        }

        // Zenos Yae Galvus Tactics
        //
        // Obtain Aura for when you are tethered. (Vein Splitter?)

    }
}
