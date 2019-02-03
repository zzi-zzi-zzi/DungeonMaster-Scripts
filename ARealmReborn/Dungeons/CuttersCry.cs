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

namespace DungeonMasterScripts.ARealmReborn.Dungeons
{
    public class CuttersCry : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 12;
        public override string Name => @"Cutter's Cry";
        public override Version Version => new Version(0, 9, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                // The Dry Sands - Room 1
                new Boss("Shifting Sands #1", 2000460, 0, 5.08f, new Vector3(259.50f, -3.50f, 88.09f), () => ScriptHelper.IsTodoChecked(0)),
                // The Dry Sands - Room 2
                new Boss("Shifting Sands #2", 2000461, 1, 12.51f, new Vector3(79.09f, 0.29f, 152.21f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Myrmidon Princess", 1585, 2, 40.70f, new Vector3(-17.88f, -9.59f, 206.32f), () => ScriptHelper.IsTodoChecked(2)),
                // The Wet Sands - Room 1
                new Boss("Shifting Sands #4", 2000465, 3, 10.64f, new Vector3(301.84f, -0.72f, -111.04f), () => (ScriptHelper.IsTodoChecked(2) && Core.Me.Z < -140) || ScriptHelper.IsTodoChecked(3)),
                // The Wet Sands - Room 2
                new Boss("Shifting Sands #5", 2000466, 4, 2.31f, new Vector3(319.02f, 0.26f, -232.65f), () => (ScriptHelper.IsTodoChecked(2) && Core.Me.X < -110) || ScriptHelper.IsTodoChecked(3)),
                new Boss("Giant Tunnel Worm", 1589, 5, 28.08f, new Vector3(-144.51f, -4.90f, 153.31f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Chimera", 1590, 6, 35.20f, new Vector3(-180.91f, -4.90f, -210.64f), () => ScriptHelper.IsTodoChecked(4))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Jump before Giant Tunnel Worm
            new OffMeshConnection(new Vector3(-144.46f, -0.47f, 186.18f), new Vector3(-144.21f, -3.03f, 180.56f), ConnectionMode.OneWay)
        };

        private uint[] _theDragonsVoice = new uint[] { 12432, 10965, 7079, 3344, 2604, 2145, 1442, 1338, 1105 };
        private uint[] _theRamsVoice = new uint[] { 12431, 7078, 3343, 2603, 2144, 1285, 1104 };

        public override void OnEnter()
        {
            // Sand Pillar
            AddAvoidObject<EventObject>(3, 2001133);
            // Ceruleum Spring
            AddAvoidObject<BattleCharacter>(2, 1806);
            // Cacophony (Chimera)
            AddAvoidObject<BattleCharacter>(10, 1848);
            // The Ram's Keeper (Chimera)
            AddAvoidObject<EventObject>(8, 2000659);
            // The Ram's Voice (Chimera)
            AddAvoidObject<BattleCharacter>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _theRamsVoice.Contains(i.CastingSpellId)),
                20,
                x => x.NpcId == 1590,
                x => x.Location
                );
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Myrmidon Marshal (Myrmidon Princess)
                if (obj.Object.NpcId == 1586)
                    obj.Score += 3000;
                // Myrmidon Guard (Myrmidon Princess)
                if (obj.Object.NpcId == 1587)
                    obj.Score += 2000;
                // Shrapnel
                if (obj.Object.NpcId == 1799)
                    obj.Score += 1000;
                // Schorl Doblyn
                if (obj.Object.NpcId == 1598)
                    obj.Score += 1000;
                // Myrmidon Soldier (Myrmidon Princess)
                if (obj.Object.NpcId == 1588)
                    obj.Score += 1000;
            }
        }

        // Shifting Sands - NpcId2000460 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  8\n
        [ObjectHandler(2000460, ObjectRange = 25)]
        public async Task<bool> ShiftingSandsHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (DMDirectorManager.Instance.GetTodoArgs(0).Item1 == 8)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Shifting Sands - NpcId2000461 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000461, ObjectRange = 25)]
        public async Task<bool> ShiftingSandsHandler2(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (DMDirectorManager.Instance.GetTodoArgs(1).Item1 == 8)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Shifting Sands - NpcId2000464 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000464, ObjectRange = 25)]
        public async Task<bool> ShiftingSandsHandler3(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Shifting Sands - NpcId2000465 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000465, ObjectRange = 25)]
        public async Task<bool> ShiftingSandsHandler4(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Shifting Sands - NpcId2000466 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000466, ObjectRange = 25)]
        public async Task<bool> ShiftingSandsHandler5(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Shifting Sands - NpcId2000469 - TODOStep: 4 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000469, ObjectRange = 35)]
        public async Task<bool> ShiftingSandsHandler6(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.IsTodoChecked(3))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        [EncounterHandler(1590, "Chimera", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> ChimeraHandler(BattleCharacter c)
        {
            if (c.IsCasting && _theDragonsVoice.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 7f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Chimera"), 7f, true);
                    return true;
                }
            }

            return false;
        }

        // Giant Tunnel Worm Tactics
        //
        // Avoid -141.1913, -4.900002, 155.9331 by 15y while boss is not targetable.
        //
        // Chimera Tactics
        //
        // Lion's Breath => Frontal AoE attack. Tank away from group.

    }
}
