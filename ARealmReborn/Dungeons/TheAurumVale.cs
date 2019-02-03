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
    public class TheAurumVale : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 5;
        public override string Name => @"The Aurum Vale";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Locksmith", 1534, 0, 31.38f, new Vector3(27.51f, -9.26f, 2.82f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("Goldvine #1", 1538, 1, 23.12f, new Vector3(-37.58f, -17.20f, -95.66f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Coincounter", 1533, 2, 45.30f, new Vector3(-169.18f, -29.74f, -141.86f), () => ScriptHelper.IsTodoChecked(2)),
                new Boss("Goldvine #2", 1538, 3, 21.12f, new Vector3(-337.63f, -33.86f, -119.88f), () => DMDirectorManager.Instance.GetTodoArgs(3).Item1 > 0),
                new Boss("Miser's Mistress", 1532, 4, 34.43f, new Vector3(-411.46f, -33.30f, -126.85f), () => ScriptHelper.IsTodoChecked(4))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Path after Locksmith
            new OffMeshConnection(new Vector3(30.10f, -9.23f, -25.66f), new Vector3(30.07f, -9.13f, -30.56f), ConnectionMode.Bidirectional),
            // Vines after Locksmith
            new OffMeshConnection(new Vector3(-43.85f, -15.51f, -106.88f), new Vector3(-50.34f, -16.00f, -128.63f), ConnectionMode.Bidirectional),
            // Path after Coincounter
            new OffMeshConnection(new Vector3(-171.94f, -27.19f, -169.81f), new Vector3(-176.75f, -26.33f, -173.60f), ConnectionMode.Bidirectional),
            // Vines before Miser's Mistress
            new OffMeshConnection(new Vector3(-334.06f, -32.50f, -156.39f), new Vector3(-333.32f, -33.13f, -167.25f), ConnectionMode.Bidirectional)
        };

        private uint[] _100TonzeSwing = new uint[] { 12452, 10174, 6971, 6503, 2769, 2417, 2080, 628 };
        private uint[] _100TonzeSwipe = new uint[] { 10865, 6502, 2768, 2416, 2079, 1035, 627 };
        private uint[] _eyeOfTheBeholder = new uint[] { 10867, 6972, 2771, 2419, 2081, 631 };

        public override void OnEnter()
        {
            // Sulphuric Geyser
            AddAvoidObject<BattleCharacter>(2, 108);
            // 100-tonze Swing (Coincounter)
            AddAvoidObject<BattleCharacter>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _100TonzeSwing.Contains(i.CastingSpellId)),
                15,
                x => x.NpcId == 1533,
                x => x.Location
                );
            // 100-tonze Swipe (Coincounter)
            AddAvoidObject<BattleCharacter>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _100TonzeSwipe.Contains(i.CastingSpellId)),
                10,
                x => x.NpcId == 1533,
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
                // Poisoned Peasant
                if (obj.Object.NpcId == 1546)
                    obj.Score += 2000;
                // Morbol Fruit
                if (obj.Object.NpcId == 1536)
                    obj.Score += 2000;
                // Vale Banemite
                if (obj.Object.NpcId == 1544)
                    obj.Score += 1000;
                // Nether Nix
                if (obj.Object.NpcId == 1543)
                    obj.Score += 1000;
                // Morbol Seedlings
                if (obj.Object.NpcId == 1535)
                    obj.Score += 1000;
            }
        }

        //[EncounterHandler(1534, "Locksmith", 30, CallBehaviorMode.InCombat)]
        //public async Task<bool> LocksmithHandler(BattleCharacter c)
        //{
        //    if (Core.Me.HasAura(302))
        //    {
        //        if (Core.Me.GetAuraById(302).Value > 1)
        //        {
        //            var morbolFruit1Id = GameObjectManager.GetObjectByNPCId(2002649);
        //            var morbolFruit2Id = GameObjectManager.GetObjectByNPCId(2002648);
        //            var morbolFruit3Id = GameObjectManager.GetObjectByNPCId(2002647);
        //            var morbolFruit4Id = GameObjectManager.GetObjectByNPCId(2000778);

        //            if ((morbolFruit1Id != null && morbolFruit1Id.IsVisible) || (morbolFruit2Id != null && morbolFruit2Id.IsVisible) || (morbolFruit3Id != null && morbolFruit3Id.IsVisible) || (morbolFruit4Id != null && morbolFruit4Id.IsVisible))
        //            {
        //                var morbolFruit1Distance = Core.Me.Distance2D(morbolFruit1Id.Location);
        //                var morbolFruit2Distance = Core.Me.Distance2D(morbolFruit2Id.Location);
        //                var morbolFruit3Distance = Core.Me.Distance2D(morbolFruit3Id.Location);
        //                var morbolFruit4Distance = Core.Me.Distance2D(morbolFruit4Id.Location);

        //                if (morbolFruit1Distance > morbolFruit2Distance && morbolFruit1Distance > morbolFruit3Distance && morbolFruit1Distance > morbolFruit4Distance)
        //                {
        //                    if (Core.Me.Distance2D(morbolFruit1Id.Location) > 3f)
        //                    {
        //                        await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(morbolFruit1Id.Location, "Morbol Fruit #1"), 2f, true);
        //                        return true;
        //                    }
        //                    await CommonTasks.StopMoving();
        //                    return await ScriptHelper.ObjectInteraction(morbolFruit1Id);
        //                }
        //                if (morbolFruit2Distance > morbolFruit1Distance && morbolFruit2Distance > morbolFruit3Distance && morbolFruit2Distance > morbolFruit4Distance)
        //                {
        //                    if (Core.Me.Distance2D(morbolFruit2Id.Location) > 3f)
        //                    {
        //                        await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(morbolFruit2Id.Location, "Morbol Fruit #2"), 2f, true);
        //                        return true;
        //                    }
        //                    await CommonTasks.StopMoving();
        //                    return await ScriptHelper.ObjectInteraction(morbolFruit2Id);
        //                }
        //                if (morbolFruit3Distance > morbolFruit1Distance && morbolFruit3Distance > morbolFruit2Distance && morbolFruit3Distance > morbolFruit4Distance)
        //                {
        //                    if (Core.Me.Distance2D(morbolFruit3Id.Location) > 3f)
        //                    {
        //                        await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(morbolFruit3Id.Location, "Morbol Fruit #3"), 2f, true);
        //                        return true;
        //                    }
        //                    await CommonTasks.StopMoving();
        //                    return await ScriptHelper.ObjectInteraction(morbolFruit3Id);
        //                }
        //                if (morbolFruit4Distance > morbolFruit1Distance && morbolFruit4Distance > morbolFruit2Distance && morbolFruit4Distance > morbolFruit3Distance)
        //                {
        //                    if (Core.Me.Distance2D(morbolFruit4Id.Location) > 3f)
        //                    {
        //                        await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(morbolFruit4Id.Location, "Morbol Fruit #4"), 2f, true);
        //                        return true;
        //                    }
        //                    await CommonTasks.StopMoving();
        //                    return await ScriptHelper.ObjectInteraction(morbolFruit4Id);
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}
        [EncounterHandler(1533, "Coincounter", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> CoincounterHandler(BattleCharacter c)
        {
            if (c.IsCasting && _eyeOfTheBeholder.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 7f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Coincounter"), 7f, true);
                    return true;
                }
            }

            return false;
        }

        // Locksmith Tactics
        //
        // Core.Me.GetAuraById(302).Value > 1 => Use nearest Morbol Fruit
        //
        // Coincounter Tactics
        //
        // 10-Tonze Swipe => A frontal AoE attack. Tank boss away from group.
        // Glower => Avoid line from boss's heading. 3y width?
        //
        // Miser's Mistress Tactics
        //
        // Core.Me.GetAuraById(303).Value > 2 => Use nearest Morbol Fruit

    }
}
