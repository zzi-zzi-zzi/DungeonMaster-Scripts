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
using Clio.Utilities;
using DungeonMaster.Attributes;
using DungeonMaster.DungeonProfile;
using DungeonMaster.Helpers;
using DungeonMaster.Managers;
using DungeonMaster.TargetingSystems;
using ff14bot;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.ARealmReborn.Dungeons
{
    public class ThePraetorium : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 16;
        public override string Name => @"The Praetorium";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Magitek Terminal", 2001145, 0, 3f, new Vector3(196.35f, 186.28f, -3.98f), () => Core.Me.Y < 160f),
                new Boss("Magitek Transporter", 2001147, 1, 3f, new Vector3(204.42f, 120.00f, -60.01f), () => Core.Me.Y < 120f),
                new Boss("Mark II Magitek Colossus", 2134, 2, 22.83f, new Vector3(192.03f, 76.00f, -0.08f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Magitek Terminal", 2000803, 3, 3f, new Vector3(173.81f, 76.08f, 4.12f), () => Core.Me.Y < 50f),
                new Boss("Magitek Terminal", 2000810, 4, 3f, new Vector3(154.20f, 46.89f, -34.22f), () => (Core.Me.Y > 100f && Core.Me.Y < 110f) || ScriptHelper.IsTodoChecked(3)),
                new Boss("Magitek Lift", 2001205, 5, 1f, new Vector3(134.81f, 21.91f, 0.01f), () => Core.Me.Y < 0f),
                new Boss("Nero tol Scaeva", 2135, 6, 36.60f, new Vector3(-168.63f, -104.40f, -0.20f), () => ScriptHelper.IsTodoChecked(4)),
                new Boss("Magitek Terminal", 2000825, 7, 3f, new Vector3(-238.15f, -104.05f, -19.74f), () => Core.Me.Y < -200f || ScriptHelper.IsTodoChecked(5)),
                new Boss("Gaius van Baelsar", 2136, 8, 25.35f, new Vector3(-569.53f, -268.00f, 220.07f), () => ScriptHelper.IsTodoChecked(5)),
                new Boss("The Ultima Weapon", 2137, 9, 28.05f, new Vector3(-781.03f, -400.02f, -599.74f), () => ScriptHelper.IsTodoChecked(6)),
                new Boss("Lahabrea", 2143, 10, 18.38f, new Vector3(-703.97f, -185.66f, 479.98f), () => ScriptHelper.IsTodoChecked(7))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First door before Mark II Magitek Colossus
            new OffMeshConnection(new Vector3(228.39f, 155.80f, 4.06f), new Vector3(228.23f, 155.80f, -4.04f), ConnectionMode.Bidirectional),
            // Collapsed walkway before Mark II Magitek Colossus
            new OffMeshConnection(new Vector3(167.96f, 156.00f, -33.64f), new Vector3(165.40f, 154.45f, -31.19f), ConnectionMode.OneWay),
            // Second door before Mark II Magitek Colossus
            new OffMeshConnection(new Vector3(221.20f, 75.72f, -0.10f), new Vector3(214.70f, 76.00f, -0.07f), ConnectionMode.Bidirectional),
            // First door after Mark II Magitek Colossus
            new OffMeshConnection(new Vector3(191.68f, 46.00f, -44.88f), new Vector3(191.68f, 45.50f, -51.18f), ConnectionMode.Bidirectional),
            // Second door after Mark II Magitek Colossus
            new OffMeshConnection(new Vector3(145.51f, 46.00f, -49.61f), new Vector3(142.34f, 46.00f, -46.42f), ConnectionMode.Bidirectional),
            // First collapsed walkway after Magitek Armor
            new OffMeshConnection(new Vector3(142.01f, 84.89f, -93.03f), new Vector3(138.37f, 80.07f, -89.24f), ConnectionMode.OneWay),
            // Second collapsed walkway after Magitek Armor
            new OffMeshConnection(new Vector3(126.39f, 72.91f, -75.60f), new Vector3(122.38f, 66.38f, -68.37f), ConnectionMode.OneWay),
            // First Cermet Bulkhead
            new OffMeshConnection(new Vector3(206.59f, 38.13f, -0.10f), new Vector3(198.93f, 36.00f, -0.10f), ConnectionMode.Bidirectional),
            // Drop after Magitek Lift
            new OffMeshConnection(new Vector3(125.50f, -102.98f, -0.12f), new Vector3(121.54f, -104.00f, -0.16f), ConnectionMode.OneWay),
            // Second Cermet Bulkhead
            new OffMeshConnection(new Vector3(-67.61f, -103.81f, 0.11f), new Vector3(-75.30f, -103.99f, 0.20f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Proto Ultima Arm Unit
            AddAvoidObject<BattleCharacter>(15, 2133);
            // Hand of the Empire (Gaius van Baelsar)
            AddAvoidObject<EventObject>(12, 2001141);
            // Aetheroplasm (The Ultima Weapon)
            AddAvoidObject<BattleCharacter>(10, 2138);
            // Sea of Pitch (Lahabrea)
            AddAvoidObject<EventObject>(7, 2002601);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // 1st Cohort Medicus
                if (obj.Object.NpcId == 2129)
                    obj.Score += 3000;
                // 1st Cohort Signifer
                if (obj.Object.NpcId == 2127)
                    obj.Score += 2000;
                // Magitek Bit (The Ultima Weapon)
                if (obj.Object.NpcId == 2142)
                    obj.Score += 1000;
                // 1st Cohort Sagittarius
                if (obj.Object.NpcId == 2128)
                    obj.Score += 1000;
                // Magitek Death Claw (Nero tol Scaeva)
                if (obj.Object.NpcId == 2121)
                    obj.Score += 1000;
            }
        }

        // Magitek Terminal - NpcId2001145 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2001145, ObjectRange = 25)]
        public async Task<bool> MagitekTerminalHandler(GameObject context)
        {
            // if (ScriptHelper.InCombat())
                // return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);
        }
        // Magitek Terminal - NpcId2000792 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Terminal - NpcId2000841 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Terminal - NpcId2000857 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000792, ObjectRange = 25)]
        [ObjectHandler(2000841, ObjectRange = 25)]
        [ObjectHandler(2000857, ObjectRange = 25)]
        public async Task<bool> MagitekTerminalHandler2(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2001060))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Imperial Identification Key - NpcId2000837 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Imperial Identification Key - NpcId2000839 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Imperial Identification Key - NpcId2000840 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000837, ObjectRange = 25)]
        [ObjectHandler(2000839, ObjectRange = 25)]
        [ObjectHandler(2000840, ObjectRange = 25)]
        public async Task<bool> ImperialIdentificationKeyHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);
        }
        // Magitek Transporter - NpcId2001147 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2001147, ObjectRange = 25)]
        public async Task<bool> MagitekTransporterHandler(GameObject context)
        {
            // if (ScriptHelper.InCombat())
                // return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);
        }
        // Magitek Terminal - NpcId2000803 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000803, ObjectRange = 25)]
        public async Task<bool> MagitekTerminalHandler3(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.IsTodoChecked(1))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Identification Key Reader - NpcId2000808 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000808, ObjectRange = 25)]
        public async Task<bool> IdentificationKeyReaderHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);

        }
        // Magitek Terminal - NpcId2000810 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000810, ObjectRange = 25)]
        public async Task<bool> MagitekTerminalHandler4(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);

        }
        // Magitek Armor - NpcId2000872 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000872, ObjectRange = 55)]
        public async Task<bool> MagitekArmorHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!Core.Player.IsMounted)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        [LocationHandler(208.0468f, 38.76678f, 0.07358904f, 1f)]
        public async Task<bool> CermetBulkheadHandler(GameObject context)
        {
            var cermetBulkheadId = GameObjectManager.GetObjectByNPCId(2131);
            
            while (cermetBulkheadId != null && cermetBulkheadId.IsVisible)
            {
                cermetBulkheadId.Target();
                await Coroutine.Sleep(100);
                ActionManager.DoActionLocation(1128, cermetBulkheadId.Location);
                await Coroutine.Sleep(1500);
            }

            return false;
        }
        [LocationHandler(83.94897f, -107.9852f, -30.76188f, 1f)]
        public async Task<bool> ProtoUltimaArmUnitHandler(GameObject context)
        {
            var protoUltimaArmUnitId = GameObjectManager.GetObjectByNPCId(2133);

            while (protoUltimaArmUnitId != null && protoUltimaArmUnitId.IsVisible)
            {
                protoUltimaArmUnitId.Target();
                await Coroutine.Sleep(100);
                ActionManager.DoActionLocation(1128, protoUltimaArmUnitId.Location);
                await Coroutine.Sleep(1500);
            }

            return false;
        }
        [LocationHandler(52.79589f, -107.9864f, -71.94444f, 1f)]
        public async Task<bool> ProtoUltimaArmUnitHandler2(GameObject context)
        {
            var protoUltimaArmUnit2Id = GameObjectManager.GetObjectByNPCId(2133);

            while (protoUltimaArmUnit2Id != null && protoUltimaArmUnit2Id.IsVisible)
            {
                protoUltimaArmUnit2Id.Target();
                await Coroutine.Sleep(100);
                ActionManager.DoActionLocation(1128, protoUltimaArmUnit2Id.Location);
                await Coroutine.Sleep(1500);
            }

            return false;
        }
        [LocationHandler(-22.09423f, -104f, 0.02178545f, 1f)]
        public async Task<bool> ProtoUltimaArmUnitHandler3(GameObject context)
        {
            var protoUltimaArmUnit3Id = GameObjectManager.GetObjectByNPCId(2133);

            while (protoUltimaArmUnit3Id != null && protoUltimaArmUnit3Id.IsVisible)
            {
                protoUltimaArmUnit3Id.Target();
                await Coroutine.Sleep(100);
                ActionManager.DoActionLocation(1128, protoUltimaArmUnit3Id.Location);
                await Coroutine.Sleep(1500);
            }

            return false;
        }
        [LocationHandler(-60.36529f, -103.9064f, 0.07477038f, 1f)]
        public async Task<bool> CermetBlastDoorHandler(GameObject context)
        {
            var cermetBlastDoorId = GameObjectManager.GetObjectByNPCId(2132);

            while (cermetBlastDoorId != null && cermetBlastDoorId.IsVisible)
            {
                cermetBlastDoorId.Target();
                await Coroutine.Sleep(100);
                ActionManager.DoActionLocation(1128, cermetBlastDoorId.Location);
                await Coroutine.Sleep(1500);
            }

            return false;
        }
        // Magitek Terminal - NpcId2000825 - TODOStep: 5 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000825, ObjectRange = 100)]
        public async Task<bool> MagitekTerminalHandler5(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.IsTodoChecked(4))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Porta Decumana Entryway - NpcId2001045 - TODOStep: 6 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2001045, ObjectRange = 35)]
        public async Task<bool> PortaDecumanaEntrywayHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);
        }

        // TO DO
        //
        // Resolve navigation issues. Add more "bosses," if necessary.
        // Use until "You are now authorized to operate a suit of magitek armor!" is in chat:
        // --- Identification Key Reader => EventObject => 2000808
        // Use terminal to commence Gaius van Baelsar fight.
        // --- Magitek Terminal => EventObject => 2000825
        //
        // Mark II Magitek Colossus Tactics
        //
        // Grand Sword => Frontal attack. Tanks should keep the boss facing away.
        //
        // Nero tol Scaeva Tactics
        //
        // Iron Uprising => Frontal attack. Tanks should keep the boss facing away.
        // Sideswing => Frontal attack. Tanks should keep the boss facing away.
        // Adds:
        // --- Magitek Death Claw (2121)
        // ------ Tethers to a random player. If it reaches the player, it will knock them back. Run to opposite edge of area to give DPS max time to kill it.
        //
        // Gaius van Baelsar Tactics
        //
        // Innocence => Frontal attack. Tanks should keep the boss facing away.
        // Terminus Est => Blue "X" frontal column attack.
        //
        // The Ultima Weapon Tactics
        //
        // "Green Lasers" => 3 frontal column attacks on random players. Cast in succession.
        // Freefire => Gunship falls into the area on a proximity marker. Avoid by 20y.
        // Primal attacks:
        // --- Eye of the Storm(?) => Garuda attacks outer edge of area. Move to 10y within center of area.
        // --- Geocrush => Titan attacks in middle of area. Move to 10y away from center of area.
        // --- Radiant Plume => Ifrit attacks outer edge of area. Move to 10y within center of area.
        // ------ Can be reversed with Weight of the Land positioning.
        // --- Weight of the Land => Titan attacks in middle of area. Move to 15y away from center of area.
        // ------ Can be reversed with Radiant Plume positioning.
        // --- Eruption => Ifrit attacks and casts on a random player. Avoid by 10y(?). Attacks same target 2 or 3 times in succession.

    }
}
