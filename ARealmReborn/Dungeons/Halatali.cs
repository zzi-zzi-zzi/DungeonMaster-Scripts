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
using System.Threading;
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
    public class Halatali : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 7;
        public override string Name => @"Halatali";
        public override Version Version => new Version(0, 9, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Firemane", 1194, 0, 35.09f, new Vector3(21.63f, 1.30f, 135.97f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("Thunderclap Guivre", 1196, 1, 29.21f, new Vector3(-179.70f, -15.31f, -135.82f), () => ScriptHelper.IsTodoChecked(2)),
                new Boss("Tangata", 1197, 2, 39.35f, new Vector3(-271.13f, 17.23f, 19.96f), () => ScriptHelper.IsTodoChecked(3))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Entrance
            // new OffMeshConnection(new Vector3(239.51f, 9.90f, -1.09f), new Vector3(234.02f, 9.86f, -20.53f), ConnectionMode.Bidirectional),
            // Gate after entrance
            // new OffMeshConnection(new Vector3(228.91f, 9.77f, -29.29f), new Vector3(222.74f, 9.44f, -33.56f), ConnectionMode.Bidirectional),
            // Door before Firemane
            new OffMeshConnection(new Vector3(131.53f, -0.64f, 2.69f), new Vector3(126.92f, -1.07f, 10.29f), ConnectionMode.Bidirectional),
            // Walkway jump to lower level
            new OffMeshConnection(new Vector3(-29.74f, 0.12f, -132.81f), new Vector3(-34.57f, -11.08f, -128.54f), ConnectionMode.OneWay),
            // Gate before Thunderclap Guivre
            new OffMeshConnection(new Vector3(-95.87f, -9.74f, -102.11f), new Vector3(-102.22f, -9.07f, -104.25f), ConnectionMode.Bidirectional),
            // Door before Tangata
            new OffMeshConnection(new Vector3(-170.66f, 12.50f, 12.32f), new Vector3(-177.41f, 12.08f, 13.14f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Scorched Earth (Tangata)
            AddAvoidObject<EventObject>(10, 2000210);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Fire Sprite (Tangata)
                if (obj.Object.NpcId == 116)
                    obj.Score += 3000;
                // Noxius (Firemane & Tangata)
                if (obj.Object.NpcId == 1195)
                    obj.Score += 2000;
                // Gas Bomb
                if (obj.Object.NpcId == 1193)
                    obj.Score += 1000;
                // Rudius Beak
                if (obj.Object.NpcId == 1191)
                    obj.Score += 1000;
                // Damantus (Firemane & Tangata)
                if (obj.Object.NpcId == 1187)
                    obj.Score += 1000;
                // Lightning Sprite (Thunderclap Guivre)
                if (obj.Object.NpcId == 117)
                    obj.Score += 1000;
            }
        }

        [LocationHandler(239.51f, 9.90f, -1.09f, 3f)]
        public async Task<bool> EntranceDoorHandler(Vector3 context)
        {
            var MidPoint = new Vector3(229.94f, 9.79f, -27.41f);
            var EndPoint = new Vector3(221.94f, 9.35f, -33.74f);

            if (ScriptHelper.InCombat())
                return false;
            while (!Navigator.InPosition(MidPoint, Core.Me.Location, 3f))
            {
                Navigator.PlayerMover.MoveTowards(MidPoint);
                await Coroutine.Yield();
            }
            while (!Navigator.InPosition(EndPoint, Core.Me.Location, 1f))
            {
                Navigator.PlayerMover.MoveTowards(EndPoint);
                await Coroutine.Yield();
            }
            await CommonTasks.StopMoving();

            return false;
        }
        // Aetherial Flow - NpcId2001619 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  5\n
		[ObjectHandler(2001619, ObjectRange = 25)]
        public async Task<bool> AetherialFlowHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            while (!Navigator.InPosition(context.Location, Core.Me.Location, 3f) && ScriptHelper.IsTodoChecked(0))
            {
                Navigator.PlayerMover.MoveTowards(context.Location);
                MovementManager.Jump();
                return true;
            }
            if (ScriptHelper.IsTodoChecked(0))
                if (MovementManager.IsMoving)
                {
                    await CommonTasks.StopMoving();
                    return true;
                }
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Chain Winch - NpcId2001624 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  5
        // Chain Winch - NpcId2001625 - TODOStep: 1 - TODOValue (Before Pickup): 1 /  5
        // Chain Winch - NpcId2001626 - TODOStep: 1 - TODOValue (Before Pickup): 2 /  5
        // Chain Winch - NpcId2001627 - TODOStep: 1 - TODOValue (Before Pickup): 3 /  5
        // Chain Winch - NpcId2001628 - TODOStep: 1 - TODOValue (Before Pickup): 4 /  5\n
        [ObjectHandler(2001624, ObjectRange = 25)]
        [ObjectHandler(2001625, ObjectRange = 25)]
        [ObjectHandler(2001626, ObjectRange = 25)]
        [ObjectHandler(2001627, ObjectRange = 25)]
        [ObjectHandler(2001628, ObjectRange = 25)]
        public async Task<bool> ChainWinchHandler(GameObject context)
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
        [EncounterHandler(1196, "Thunderclap Guivre", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> ThunderclapGuivreHandler(BattleCharacter c)
        {
            var electrifiedWaterId = GameObjectManager.GetObjectByNPCId(2001648);

            if (!electrifiedWaterId.IsVisible)
            {
                var lightningSpriteId = GameObjectManager.GetObjectByNPCId(117);

                if (Core.Me.HasAura(288))
                {
                    var safeSpotLeft = new Vector3(-161.57f, -7.15f, -102.57f);
                    var safeSpotRight = new Vector3(-149.98f, -9.59f, -152.10f);

                    // Find both points, but go to the closest one, or the one the group is going to/at currently.
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(safeSpotLeft, "Left Safe Spot"), 1f, true);
                    // await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(new Vector3(safeSpotRight), "Right Safe Spot"), 1f, true);
                    return true;
                }
                if (lightningSpriteId.IsVisible)
                    return false;
                return true;
            }

            return false;
        }
        // Aetherial Flow - NpcId2001647 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2001647, ObjectRange = 25)]
        public async Task<bool> AetherialFlowHandler2(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.IsTodoChecked(2))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Ludus Door - NpcId2001623 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2001623, ObjectRange = 25)]
        public async Task<bool> LudusDoorHandler(GameObject context)
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

    }
}
