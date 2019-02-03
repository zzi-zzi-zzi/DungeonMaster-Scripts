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
using DungeonMaster.Enumeration;
using DungeonMaster.Helpers;
using DungeonMaster.Managers;
using DungeonMaster.TargetingSystems;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.ARealmReborn.Dungeons
{
    public class TheSunkenTempleOfQarn : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 9;
        public override string Name => @"The Sunken Temple of Qarn";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Teratotaur", 1567, 0, 23.93f, new Vector3(-70.00f, -12.00f, -62.00f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("Avoirdupois", 1572, 1, 4.72f, new Vector3(-14.77f, -18.00f, -0.02f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Avoirdupois", 1572, 2, 4.84f, new Vector3(8.96f, -18.68f, -14.88f), () => (GameObjectManager.GetObjectByNPCId(2000433) == null || !GameObjectManager.GetObjectByNPCId(2000433).IsVisible) || ScriptHelper.IsTodoChecked(2)),
                new Boss("Avoirdupois", 1572, 3, 4.37f, new Vector3(7.52f, -18.77f, 15.40f), () => (GameObjectManager.GetObjectByNPCId(2000432) == null || !GameObjectManager.GetObjectByNPCId(2000432).IsVisible) || ScriptHelper.IsTodoChecked(2)),
                new Boss("Temple Guardian", 1569, 4, 36.95f, new Vector3(53.52f, -49.57f, 1.22f), () => ScriptHelper.IsTodoChecked(2)),
                new Boss("Avoirdupois", 1572, 5, 4.45f, new Vector3(49.76f, -18.71f, 1.11f), () => (GameObjectManager.GetObjectByNPCId(2000434) == null || !GameObjectManager.GetObjectByNPCId(2000434).IsVisible) || ScriptHelper.IsTodoChecked(3)),
                new Boss("Avoirdupois", 1572, 6, 4.89f, new Vector3(124.38f, -3.68f, -0.05f), () => (GameObjectManager.GetObjectByNPCId(2000435) == null || !GameObjectManager.GetObjectByNPCId(2000435).IsVisible) || ScriptHelper.IsTodoChecked(3)),
                new Boss("Adjudicator", 1570, 7, 33.00f, new Vector3(243.00f, -4.00f, 0.00f), () => ScriptHelper.IsTodoChecked(4))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First door after Teratotaur
            new OffMeshConnection(new Vector3(-62.85f, -12.00f, -46.23f), new Vector3(-62.78f, -12.00f, -39.83f), ConnectionMode.Bidirectional),
            // Second door after Teratotaur
            new OffMeshConnection(new Vector3(-11.68f, -18.00f, -0.29f), new Vector3(-5.98f, -18.00f, -0.32f), ConnectionMode.Bidirectional),
            // Door near The Flame of Magic
            new OffMeshConnection(new Vector3(8.93f, -19.00f, 26.26f), new Vector3(9.01f, -19.00f, 31.97f), ConnectionMode.Bidirectional),
            // Door near The Fruit of Knowledge
            new OffMeshConnection(new Vector3(9.22f, -19.00f, -26.15f), new Vector3(9.20f, -19.00f, -32.45f), ConnectionMode.Bidirectional),
            // Door after Temple Guardian
            new OffMeshConnection(new Vector3(47.92f, -48.21f, 17.43f), new Vector3(47.90f, -47.86f, 22.45f), ConnectionMode.Bidirectional),
            // Door to the Vault of Wealth
            new OffMeshConnection(new Vector3(69.77f, -18.00f, 19.25f), new Vector3(72.21f, -18.00f, 23.73f), ConnectionMode.Bidirectional),
            // Door to the Vault of Steel
            new OffMeshConnection(new Vector3(69.35f, -18.00f, -19.54f), new Vector3(72.10f, -18.00f, -23.96f), ConnectionMode.Bidirectional),
            // Entryway before The Rosarium of Lalafuto III
            new OffMeshConnection(new Vector3(74.17f, -11.00f, -2.71f), new Vector3(78.27f, -11.00f, -2.72f), ConnectionMode.Bidirectional),
            // Door to the Vault of Secrets
            new OffMeshConnection(new Vector3(124.04f, -4.00f, 18.80f), new Vector3(127.13f, -4.00f, 21.93f), ConnectionMode.Bidirectional),
            // Door to the Vault of Aether
            new OffMeshConnection(new Vector3(123.85f, -4.00f, -18.13f), new Vector3(127.38f, -4.00f, -21.58f), ConnectionMode.Bidirectional),
            // Door before the Scales of Judgment
            new OffMeshConnection(new Vector3(130.66f, -4.00f, -0.25f), new Vector3(135.96f, -4.00f, -0.14f), ConnectionMode.Bidirectional),
            // False wall to The Oratory of Memeto the Meek
            new OffMeshConnection(new Vector3(190.93f, -4.00f, -11.20f), new Vector3(190.91f, -4.00f, -18.50f), ConnectionMode.Bidirectional)
        };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Mythril Verge (Adjudicator)
                if (obj.Object.NpcId == 1798)
                    obj.Score += 2000;
                // The Condemned
                if (obj.Object.NpcId == 1580)
                    obj.Score += 1000;
                // Temple Bee
                if (obj.Object.NpcId == 1575)
                    obj.Score += 1000;
                // Sun Juror (Adjudicator)
                if (obj.Object.NpcId == 1571)
                    obj.Score += 1000;
                // Dump Wespe (Teratotaur)
                if (obj.Object.NpcId == 1568)
                    obj.Score += 1000;
                // Golem Soulstone (Temple Guardian) 
                if (obj.Object.NpcId == 1490)
                    obj.Score += 1000;
            }
        }

        // The Helm of Might - NpcId2000418 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000418, ObjectRange = 25)]
        public async Task<bool> TheHelmOfMightHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible && !ScriptHelper.HasKeyItem(2000568))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // The Flame of Magic - NpcId2000415 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000415, ObjectRange = 25)]
        public async Task<bool> TheFlameOfMagicHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible && !ScriptHelper.HasKeyItem(2000565))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        [EncounterHandler(1567, "Teratotaur", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> TeratotaurHandler(BattleCharacter c)
        {
            if (Core.Me.HasAura(210))
            {
                var tile1Id = GameObjectManager.GetObjectByNPCId(2000866);
                var tile2Id = GameObjectManager.GetObjectByNPCId(2000867);
                var tile3Id = GameObjectManager.GetObjectByNPCId(2000868);

                if (tile1Id.IsVisible)
                {
                    while (!Navigator.InPosition(tile1Id.Location, Core.Me.Location, 1f))
                    {
                        Navigator.PlayerMover.MoveTowards(tile1Id.Location);
                        await Coroutine.Yield();
                    }
                    await CommonTasks.StopMoving();
                }
                if (tile2Id.IsVisible)
                {
                    while (!Navigator.InPosition(tile2Id.Location, Core.Me.Location, 1f))
                    {
                        Navigator.PlayerMover.MoveTowards(tile2Id.Location);
                        await Coroutine.Yield();
                    }
                    await CommonTasks.StopMoving();
                }
                if (tile3Id.IsVisible)
                {
                    while (!Navigator.InPosition(tile3Id.Location, Core.Me.Location, 1f))
                    {
                        Navigator.PlayerMover.MoveTowards(tile3Id.Location);
                        await Coroutine.Yield();
                    }
                    await CommonTasks.StopMoving();
                }
            }

            return false;
        }
        // The Fruit of Knowledge - NpcId2000416 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000416, ObjectRange = 35)]
        public async Task<bool> TheFruitOfKnowledgeHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible && !ScriptHelper.HasKeyItem(2000566))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // The Gem of Affluence - NpcId2000417 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000417, ObjectRange = 35)]
        public async Task<bool> TheGemOfAffluenceHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible && !ScriptHelper.HasKeyItem(2000567))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Stone Pedestal - NpcId2000423 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000423, ObjectRange = 30)]
        public async Task<bool> StonePedestalHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectString.IsOpen)
            {
                SelectString.ClickLineContains("The Gem of Affluence");
                return true;
            }
            if (context.IsVisible && ScriptHelper.HasKeyItem(2000567))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Stone Pedestal - NpcId2000425 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000425, ObjectRange = 30)]
        public async Task<bool> StonePedestalHandler2(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectString.IsOpen)
            {
                SelectString.ClickLineContains("The Helm of Might");
                return true;
            }
            if (context.IsVisible && ScriptHelper.HasKeyItem(2000568))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Stone Pedestal - NpcId2000421 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000421, ObjectRange = 30)]
        public async Task<bool> StonePedestalHandler3(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectString.IsOpen)
            {
                SelectString.ClickLineContains("The Fruit of Knowledge");
                return true;
            }
            if (context.IsVisible && ScriptHelper.HasKeyItem(2000566))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Stone Pedestal - NpcId2000419 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000419, ObjectRange = 30)]
        public async Task<bool> StonePedestalHandler4(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectString.IsOpen)
            {
                SelectString.ClickLineContains("The Flame of Magic");
                return true;
            }
            if (context.IsVisible && ScriptHelper.HasKeyItem(2000565))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Left Pan - NpcId2000427 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000427, ObjectRange = 25)]
        public async Task<bool> LeftPanHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectString.IsOpen)
            {
                SelectString.ClickLineContains("The Flame of Magic");
                return true;
            }
            if (context.IsVisible && ScriptHelper.HasKeyItem(2000565))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Right Pan - NpcId2000428 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000428, ObjectRange = 25)]
        public async Task<bool> RightPanHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectString.IsOpen)
            {
                SelectString.ClickLineContains("The Fruit of Knowledge");
                return true;
            }
            if (context.IsVisible && ScriptHelper.HasKeyItem(2000566))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // The Scales of Judgment - NpcId2000658 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000658, ObjectRange = 25)]
        public async Task<bool> TheScalesOfJudgmentHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible && !ScriptHelper.HasKeyItem(2000565) && !ScriptHelper.HasKeyItem(2000566))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }

        // TO DO
        //
        // Kill Avoirdupoises (1572) on top of glowing tiles.
        // --- Name: 0x262979B3DE0, Type:ff14bot.Objects.EventObject, ID:2000650, Obj:1074347194
        // --- Name: 0x262979B41E0, Type:ff14bot.Objects.EventObject, ID:2000651, Obj:1074347193
        // --- Name: 0x262979B4FE0, Type:ff14bot.Objects.EventObject, ID:2000652, Obj:1074347192
        // --- Name: 0x262979B59E0, Type:ff14bot.Objects.EventObject, ID:2000653, Obj:1074347191
        // --- Name: 0x262979B5BE0, Type:ff14bot.Objects.EventObject, ID:2000654, Obj:1074347190
        //
        // Adjudicator Tactics
        //
        // Adds:
        // --- Sun Juror (1571) => +1000
        // ------ Kill these on top of glowing tiles.
        // --------- Name: 0x262979B39E0, Type:ff14bot.Objects.EventObject, ID:2000656, Obj:1074347199
        // --------- Name: 0x262979B5FE0, Type:ff14bot.Objects.EventObject, ID:2000655, Obj:1074347200
        // --------- Name: 0x262979B3DE0, Type:ff14bot.Objects.EventObject, ID:2000657, Obj:1074347198

    }
}
