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
using Buddy.Coroutines;
using Clio.Common;
using Clio.Utilities;
using DungeonMaster.Attributes;
using DungeonMaster.DungeonProfile;
using DungeonMaster.Enumeration;
using DungeonMaster.Helpers;
using DungeonMaster.Managers;
using DungeonMaster.TargetingSystems;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using ff14bot.Pathing;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class TheDrownedCityOfSkalla : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 58;
        public override string Name => @"The Drowned City of Skalla";
        public override Version Version => new Version(0, 8, 0, 1);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Kelpie", 6907, 0, 27.13f, new Vector3(-217.70f, -1.90f, 10.31f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("The Old One", 6908, 1, 25.62f, new Vector3(121.73f, 9.00f, 3.99f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Hrodric Poisontongue", 6910, 2, 26.32f, new Vector3(487.63f, -13.95f, 3.98f), () => ScriptHelper.IsTodoChecked(5))
            };
        }
        
        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Larvae wall
            new OffMeshConnection(new Vector3(-380.1175f, 5.25499f, -14.57886f), new Vector3(-380.1322f, 5.255368f, -21.52924f), ConnectionMode.Bidirectional),
            // Waterfall before Kelpie
            new OffMeshConnection(new Vector3(-275.2009f, 1.281889f, 3.234623f), new Vector3(-266.4566f, 0.8641517f, 3.303339f), ConnectionMode.Bidirectional),
            // Door before The Old One
            new OffMeshConnection(new Vector3(67.74905f, 7.633199f, 3.927771f), new Vector3(76.59614f, 7.5f, 3.872761f), ConnectionMode.Bidirectional),
            // First Transfiguration gap
            new OffMeshConnection(new Vector3(275.8991f, -12f, 23.3275f), new Vector3(275.8878f, -12f, -11.9604f), ConnectionMode.OneWay),
            // Second Transfiguration gap
            new OffMeshConnection(new Vector3(292.7512f, -12f, -35.09476f), new Vector3(319.716f, -12f, -35.40932f), ConnectionMode.OneWay),
            // Wall before Hrodric Poisontongue
            new OffMeshConnection(new Vector3(434.0036f, -15.9375f, 3.762431f), new Vector3(441.0499f, -15.9f, 3.900358f), ConnectionMode.Bidirectional)
        };
        
        private uint[] _eyeOfTheFire = new uint[] { 11922, 9829 };
        private uint[] _hydroPull = new uint[] { 9809 };
        private uint[] _hydroPush = new uint[] { 9810 };
        private uint[] _mysticFlame = new uint[] { 9817, 9816 };

        public override void OnEnter()
        {
            // Hydrosphere (Kelpie)
            AddAvoidObject<BattleCharacter>(8, 6949);
            // Mystic Flame (The Old One)
            AddAvoidObject<GameObject>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _mysticFlame.Contains(i.CastingSpellId)),
                5,
                x => (x.Type == ff14bot.Enums.GameObjectType.Pc && !x.IsMe),
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
                // Subservient (The Old One)
                if (obj.Object.NpcId == 6909)
                    obj.Score += 1000;
            }
        }

        [EncounterHandler(6910, "Hrodric Poisontongue", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> HrodricPoisontongueHandler(BattleCharacter c)
        {
            while (c.IsCasting && _eyeOfTheFire.Contains(c.CastingSpellId))
            {
                ScriptHelper.LookAway(c);
                await Coroutine.Yield();
            }

            return false;
        }
        [EncounterHandler(6908, "The Old One", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> TheOldOneHandler(BattleCharacter c)
        {
            if (Core.Me.HasAura(1448))
            {
                var subservientId = GameObjectManager.GetObjectByNPCId(6909);

                if (Core.Me.Location.Distance(subservientId.Location) > 3f)
                {
                    var subservientPos = GameObjectManager.GetObjectsByNPCId(6909).OrderByDescending(i => i.Distance2D()).First().Location;

                    await CommonTasks.MoveTo(subservientPos);
                    ActionManager.DoAction(9823, subservientId);
                    return true;
                }
            }

            return false;
        }
        [EncounterHandler(6907, "Kelpie", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> KelpieHandler(BattleCharacter c)
        {
            if (c.IsCasting && _hydroPull.Contains(c.CastingSpellId))
            {
                if (Core.Me.Location.Distance(c.CalculatePointInFront(8f)) > 1f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.CalculatePointInFront(8f), "Kelpie"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }
            if (c.IsCasting && _hydroPush.Contains(c.CastingSpellId))
            {
                if (Core.Me.Location.Distance(c.CalculatePointInFront(30f)) > 1f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.CalculatePointInFront(30f), "Kelpie"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }

            return false;
        }
        // Get Transfiguration aura (565) @ 276.3167, -12, -47.89919 (Transfiguration glyph) before crossing second Transfiguration gap OffMeshConnection.
        [LocationHandler(275.8991f, -12f, 23.3275f, 5f)]
        public async Task<bool> TransfigurationHandler(GameObject context)
        {
            if (!Core.Me.HasAura(565))
            {
                if (Core.Me.Location.Distance(new Vector3(276.3167f, -12f, -47.89919f)) > 1f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(new Vector3(276.3167f, -12f, -47.89919f), "Transfiguration Glyph"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }

            return false;
        }

        // Hrodric Poisontongue Tactics
        //
        // Raise Claws => Move behind boss.
        // Raise Tail => Move in front of boss.
        // If boss is facing you, move out of X yalm cone to avoid untelegraphed attack.
        // --- Need more complex logic to avoid tanks from trying to move during the entire fight.
        // Avoid "donut" and "cross" AE (cast on player) telegraphed attacks. Move "donut" to boss. Move "cross" to edge of area.

    }
}
