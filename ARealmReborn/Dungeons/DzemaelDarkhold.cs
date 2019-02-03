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
// using Buddy.Coroutines;
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
// using ff14bot.Navigation;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.ARealmReborn.Dungeons
{
    public class DzemaelDarkhold : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 13;
        public override string Name => @"Dzemael Darkhold";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Grand Hall Gate", 2000438, 0, 0.00f, new Vector3(102.74f, -8.26f, 112.01f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("All-seeing Eye", 1397, 1, 41.19f, new Vector3(49.86f, -14.59f, 75.21f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Taulurd", 1415, 2, 25.33f, new Vector3(-87.69f, -29.27f, -35.90f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Batraal", 1396, 3, 37.50f, new Vector3(86.63f, -39.04f, -176.69f), () => ScriptHelper.IsTodoChecked(3))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Gate near Chocobo Stables
            new OffMeshConnection(new Vector3(79.97f, 1.27f, 162.21f), new Vector3(80.04f, 1.27f, 157.90f), ConnectionMode.Bidirectional),
            // First gate before All-seeing Eye
            new OffMeshConnection(new Vector3(144.10f, -6.83f, 162.12f), new Vector3(144.13f, -6.83f, 158.02f), ConnectionMode.Bidirectional),
            // Second gate before All-seeing Eye
            new OffMeshConnection(new Vector3(98.06f, -6.83f, 111.85f), new Vector3(93.88f, -6.83f, 111.83f), ConnectionMode.Bidirectional),
            // Barrier before Batraal
            new OffMeshConnection(new Vector3(14.11f, -15.14f, -142.82f), new Vector3(16.86f, -15.53f, -138.02f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Void Pitch (Batraal)
            AddAvoidObject<BattleCharacter>(8, 2153);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Lava Drake
                if (obj.Object.NpcId == 1652)
                    obj.Score += 2000;
                // Corrupted Crystal (Batraal)
                if (obj.Object.NpcId == 2154)
                    obj.Score += 1000;
                // Alpgrot Orobon
                if (obj.Object.NpcId == 1655)
                    obj.Score += 1000;
                // Forsaken Soul
                if (obj.Object.NpcId == 1654)
                    obj.Score += 1000;
                // Bone Nix
                if (obj.Object.NpcId == 1653)
                    obj.Score += 1000;
                // Amanuensis (All-seeing Eye)
                if (obj.Object.NpcId == 1498)
                    obj.Score += 1000;
                // Mouche Volante(All-seeing Eye)
                if (obj.Object.NpcId == 1497)
                    obj.Score += 1000;
            }
        }

        [LocationHandler(35.73392f, 8.087571f, 206.8964f, 15f)]
        public async Task<bool> CrystalSafeSpotHandler(GameObject context)
        {
            // Crystal Safe Spot #1
            return false;
        }
        //[LocationHandler(81.01265f, 1.358971f, 173.3143f, 15f)]
        //public async Task<bool> MagitekTerminalHandler(GameObject context)
        //{
        //    // Magitek Terminal I Loc
        //    return false;
        //}
        //[LocationHandler(80.85289f, 1.192093E-07f, 192.9843f, 15f)]
        //public async Task<bool> MagitekTerminalHandler2(GameObject context)
        //{
        //    var MagitekTerminalI = GameObjectManager.GetObjectByNPCId(2000476);

        //    if (ScriptHelper.InCombat())
        //        return false;
        //    if (MagitekTerminalI.IsVisible && Core.Me.Distance2D(MagitekTerminalI.Location) > 1f)
        //    {
        //        await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(MagitekTerminalI.Location, "Magitek Terminal I"), 0.5f, true);
        //        while (MagitekTerminalI.IsVisible)
        //        {
        //            await CommonTasks.StopMoving();
        //        }
        //        return true;
        //    }

        //    return false;
        //}
        [LocationHandler(111.5706f, -3.624766f, 176.3287f, 15f)]
        public async Task<bool> CrystalSafeSpotHandler2(GameObject context)
        {
            // Crystal Safe Spot #2
            return false;
        }
        //[LocationHandler(124.1592f, -13.81359f, 123.3193f, 15f)]
        //public async Task<bool> MagitekTerminalHandler3(GameObject context)
        //{
        //    // Magitek Terminal III Loc
        //    return false;
        //}
        //[LocationHandler(140.7879f, -11.96489f, 113.2706f, 15f)]
        //public async Task<bool> MagitekTerminalHandler4(GameObject context)
        //{
        //    // Magitek Terminal IV Loc
        //    return false;
        //}
        [LocationHandler(142.7961f, -7.184865f, 145.9116f, 15f)]
        public async Task<bool> MagitekTerminalHandler5(GameObject context)
        {
            var MagitekTerminalIII = GameObjectManager.GetObjectByNPCId(2000478);
            var MagitekTerminalIV = GameObjectManager.GetObjectByNPCId(2000479);

            if (ScriptHelper.InCombat())
                return false;
            if (MagitekTerminalIII.IsVisible && Core.Me.Distance2D(MagitekTerminalIII.Location) > 1f)
            {
                await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(MagitekTerminalIII.Location, "Magitek Terminal III"), 0.5f, true);
                while (MagitekTerminalIII.IsVisible)
                {
                    await CommonTasks.StopMoving();
                }
                return true;
            }
            if (MagitekTerminalIV.IsVisible && Core.Me.Distance2D(MagitekTerminalIV.Location) > 1f)
            {
                await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(MagitekTerminalIV.Location, "Magitek Terminal IV"), 0.5f, true);
                while (MagitekTerminalIV.IsVisible)
                {
                    await CommonTasks.StopMoving();
                }
                return true;
            }

            return false;
        }
        [LocationHandler(139.205f, -10.70804f, 109.5027f, 15f)]
        public async Task<bool> CrystalSafeSpotHandler3(GameObject context)
        {
            // Crystal Safe Spot #3
            return false;
        }
        //[EncounterHandler(1397, "All-seeing Eye", 30, CallBehaviorMode.InCombat)]
        //public async Task<bool> AllseeingEyeHandler(BattleCharacter c)
        //{
        //    var crystalClusterId = GameObjectManager.GetObjectByNPCId(2000276);
        //    // var crystalClusterId = GameObjectManager.GetObjectByNPCId(2000279);

        //    if (!Core.Me.HasAura(322) && crystalClusterId.IsVisible)
        //    {
        //        if (Core.Me.Location.Distance(crystalClusterId.Location) > 1f)
        //        {
        //            await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(crystalClusterId.Location, "Crystal Cluster"), 0.5f, true);
        //            return true;
        //        }
        //        //while (!Navigator.InPosition(crystalClusterId.Location, Core.Me.Location, 1f))
        //        //{
        //        //    Navigator.PlayerMover.MoveTowards(crystalClusterId.Location);
        //        //    await Coroutine.Yield();
        //        //}
        //        await CommonTasks.StopMoving();
        //        return true;
        //    }

        //    return false;
        //}
        //[LocationHandler(68.21272f, -14.30186f, 83.95959f, 15f)]
        //public async Task<bool> CrystalSafeSpotHandler4(GameObject context)
        //{
        //    // Crystal Safe Spot #1 (All-seeing Eye)
        //    return false;
        //}
        //[LocationHandler(51.87904f, -11.82386f, 111.1436f, 15f)]
        //public async Task<bool> CrystalSafeSpotHandler5(GameObject context)
        //{
        //    // Crystal Safe Spot #2 (All-seeing Eye)
        //    return false;
        //}
        //[LocationHandler(18.58518f, -14.01604f, 88.06983f, 15f)]
        //public async Task<bool> CrystalSafeSpotHandler6(GameObject context)
        //{
        //    // Crystal Safe Spot #3 (All-seeing Eye)
        //    return false;
        //}
        //[LocationHandler(20.16036f, -11.14092f, 51.55822f, 15f)]
        //public async Task<bool> CrystalSafeSpotHandler7(GameObject context)
        //{
        //    // Crystal Safe Spot #4 (All-seeing Eye)
        //    return false;
        //}
        // Magitek Transporter - NpcId2000458 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
        // Magitek Transporter - NpcId2000474 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000458, ObjectRange = 25)]
        [ObjectHandler(2000474, ObjectRange = 25)]
        public async Task<bool> MagitekTransporterHandler6(GameObject context)
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
        [LocationHandler(-40.23561f, -22.03471f, -161.9476f, 15f)]
        public async Task<bool> MagitekTerminalHandler7(GameObject context)
        {
            var MagitekTerminalVIII = GameObjectManager.GetObjectByNPCId(2000483);
            var MagitekTerminalIX = GameObjectManager.GetObjectByNPCId(2000484);

            if (ScriptHelper.InCombat())
                return false;
            if (MagitekTerminalVIII.IsVisible && Core.Me.Distance2D(MagitekTerminalVIII.Location) > 1f)
            {
                await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(MagitekTerminalVIII.Location, "Magitek Terminal VIII"), 0.5f, true);
                while (MagitekTerminalVIII.IsVisible)
                {
                    await CommonTasks.StopMoving();
                }
                return true;
            }
            if (MagitekTerminalIX.IsVisible && Core.Me.Distance2D(MagitekTerminalIX.Location) > 1f)
            {
                await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(MagitekTerminalIX.Location, "Magitek Terminal IX"), 0.5f, true);
                while (MagitekTerminalIX.IsVisible)
                {
                    await CommonTasks.StopMoving();
                }
                return true;
            }

            return false;
        }

        // All-seeing Eye Tactics
        //
        // Fight the All-seeing Eye near the crystal clusters to gain the Crystal Veil buff.
        // --- If All-seeing Eye (1397) < 20y && !Core.Me.HasAura(322) (Crystal Veil) => MoveAndStop to nearest safe spot.
        // Objs @ safe spots:
        // --- EventObject; 2000276
        // --- EventObject; 2000279
        // "The power of the crystal begins to dim."
        // --- Move to the nearest safe spot when the current one "dims."

    }
}
