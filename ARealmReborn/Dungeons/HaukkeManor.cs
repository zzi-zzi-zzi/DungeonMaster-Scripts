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
    public class HaukkeManor : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 6;
        public override string Name => @"Haukke Manor";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Tiny Key #1", 2000302, 0, 3f, new Vector3(71.40f, 0.00f, 28.64f), () => DMDirectorManager.Instance.GetTodoArgs(0).Item1 > 0 || ScriptHelper.IsTodoChecked(1)),
                new Boss("Locked Door #1", 2000329, 1, 3f, new Vector3(0.51f, 0.14f, 56.98f), () => DMDirectorManager.Instance.GetTodoArgs(0).Item1 < 1 || ScriptHelper.IsTodoChecked(1)),
                new Boss("Tiny Key #2", 2000303, 2, 3f, new Vector3(-26.32f, -0.01f, 50.72f), () => DMDirectorManager.Instance.GetTodoArgs(0).Item1 > 0 || ScriptHelper.IsTodoChecked(1)),
                new Boss("Manor Claviger", 423, 3, 31.65f, new Vector3(10.50f, 0.00f, 0.00f), () => ScriptHelper.IsTodoChecked(1)),
                // new Boss("Ivy Door", 2000355, 4, 3f, new Vector3(-48.38f, 0.43f, -0.02f), () => ScriptHelper.IsTodoChecked(2)),
                // new Boss("Locked Door #2", 2000343, 5, 3f, new Vector3(-1.90f, -18.62f, 34.62f), () => ScriptHelper.IsTodoChecked(2)),
                new Boss("Yellow Key", 2000325, 4, 3f, new Vector3(-12.35f, -18.80f, 52.29f), () => ScriptHelper.IsTodoChecked(2)),
                // new Boss("Carnation Door", 2000356, 7, 3f, new Vector3(-25.56f, -18.57f, -0.08f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Manor Jester & Manor Steward", 426, 5, 39.51f, new Vector3(17.23f, -18.80f, 4.00f), () => ScriptHelper.IsTodoChecked(3)),
                // new Boss("Sealed Barrier", 2001233, 9, 3f, new Vector3(46.70f, 9.88f, -0.25f), () => ScriptHelper.IsTodoChecked(4)),
                // new Boss("Manor Sentry", 428, 10, 17.88f, new Vector3(30.29f, 17.00f, 3.59f), () => ScriptHelper.IsTodoChecked(4)),
                new Boss("Lady Amandine", 422, 6, 37.32f, new Vector3(-17.55f, 17.00f, -0.02f), () => ScriptHelper.IsTodoChecked(4))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Rubble after Yellow Key
            new OffMeshConnection(new Vector3(-10.23f, -18.80f, 27.58f), new Vector3(-23.74f, -18.80f, 27.39f), ConnectionMode.OneWay)
        };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Lady's Candle (Lady Amandine)
                if (obj.Object.NpcId == 425)
                    obj.Score += 3000;
                // Manor Sentry (Lady Amandine)
                if (obj.Object.NpcId == 428)
                    obj.Score += 2000;
                // Manor Jester (Manor Jester & Manor Steward)
                if (obj.Object.NpcId == 426)
                    obj.Score += 1000;
                // Lady's Handmaiden (Lady Amandine)
                if (obj.Object.NpcId == 424)
                    obj.Score += 1000;
            }
        }

        // Tiny Key - NpcId2000302 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Tiny Key - NpcId2000303 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Tiny Key - NpcId2000304 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Tiny Key - NpcId2000305 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1
        // Tiny Key - NpcId2000306 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1
        // Tiny Key - NpcId2000308 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1
        // Tiny Key - NpcId2000307 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000302, ObjectRange = 10)]
        [ObjectHandler(2000303, ObjectRange = 10)]
        [ObjectHandler(2000304, ObjectRange = 10)]
        [ObjectHandler(2000305, ObjectRange = 8)]
        [ObjectHandler(2000306, ObjectRange = 10)]
        [ObjectHandler(2000308, ObjectRange = 10)]
        [ObjectHandler(2000307, ObjectRange = 10)]
        public async Task<bool> TinyKeyHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!ScriptHelper.HasKeyItem(2000260))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Locked Door - NpcId2000329 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Locked Door - NpcId2000343 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000329, ObjectRange = 25)]
        [ObjectHandler(2000343, ObjectRange = 25)]
        public async Task<bool> LockedDoorHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2000260))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Green Key - NpcId2000324 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000324, ObjectRange = 25)]
        public async Task<bool> GreenKeyHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!ScriptHelper.HasKeyItem(2000263))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Ivy Door - NpcId2000355 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000355, ObjectRange = 25)]
        public async Task<bool> IvyDoorHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2000263) && context != null && context.IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Yellow Key - NpcId2000325 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000325, ObjectRange = 25)]
        public async Task<bool> YellowKeyHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!ScriptHelper.HasKeyItem(2000264) && Core.Me.Y < 0f)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Carnation Door - NpcId2000356 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000356, ObjectRange = 25)]
        public async Task<bool> CarnationDoorHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2000264) && Core.Me.Y < 0f)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Bloody Parchment - NpcId2001235 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2001235, ObjectRange = 25)]
        public async Task<bool> BloodyParchmentHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!ScriptHelper.HasKeyItem(2000259))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Sealed Barrier - NpcId2001233 - TODOStep: 4 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2001233, ObjectRange = 25)]
        public async Task<bool> SealedBarrierHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2000259))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        [EncounterHandler(422, "Lady Amandine", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> LadyAmandineHandler(BattleCharacter c)
        {
            var voidLamp1Id = GameObjectManager.GetObjectByNPCId(2000366);
            var voidLamp2Id = GameObjectManager.GetObjectByNPCId(2000367);
            var voidLamp3Id = GameObjectManager.GetObjectByNPCId(2000368);
            var voidLamp4Id = GameObjectManager.GetObjectByNPCId(2000369);

            if ((voidLamp1Id != null && voidLamp1Id.IsVisible) && (voidLamp2Id != null && voidLamp2Id.IsVisible) && (voidLamp3Id != null && voidLamp3Id.IsVisible) && (voidLamp4Id != null && voidLamp4Id.IsVisible))
            {
                var voidLamp1Distance = Core.Me.Distance2D(voidLamp1Id.Location);
                var voidLamp2Distance = Core.Me.Distance2D(voidLamp2Id.Location);
                var voidLamp3Distance = Core.Me.Distance2D(voidLamp3Id.Location);
                var voidLamp4Distance = Core.Me.Distance2D(voidLamp4Id.Location);

                if (voidLamp1Distance > voidLamp2Distance && voidLamp1Distance > voidLamp3Distance && voidLamp1Distance > voidLamp4Distance)
                {
                    while (!Navigator.InPosition(voidLamp1Id.Location, Core.Me.Location, 3f))
                    {
                        Navigator.PlayerMover.MoveTowards(voidLamp1Id.Location);
                        await Coroutine.Yield();
                    }
                    await CommonTasks.StopMoving();
                    return await ScriptHelper.ObjectInteraction(voidLamp1Id);
                }
                if (voidLamp2Distance > voidLamp1Distance && voidLamp2Distance > voidLamp3Distance && voidLamp2Distance > voidLamp4Distance)
                {
                    while (!Navigator.InPosition(voidLamp2Id.Location, Core.Me.Location, 3f))
                    {
                        Navigator.PlayerMover.MoveTowards(voidLamp2Id.Location);
                        await Coroutine.Yield();
                    }
                    await CommonTasks.StopMoving();
                    return await ScriptHelper.ObjectInteraction(voidLamp2Id);
                }
                if (voidLamp3Distance > voidLamp1Distance && voidLamp3Distance > voidLamp2Distance && voidLamp3Distance > voidLamp4Distance)
                {
                    while (!Navigator.InPosition(voidLamp3Id.Location, Core.Me.Location, 3f))
                    {
                        Navigator.PlayerMover.MoveTowards(voidLamp3Id.Location);
                        await Coroutine.Yield();
                    }
                    await CommonTasks.StopMoving();
                    return await ScriptHelper.ObjectInteraction(voidLamp3Id);
                }
                if (voidLamp4Distance > voidLamp1Distance && voidLamp4Distance > voidLamp2Distance && voidLamp4Distance > voidLamp3Distance)
                {
                    while (!Navigator.InPosition(voidLamp4Id.Location, Core.Me.Location, 3f))
                    {
                        Navigator.PlayerMover.MoveTowards(voidLamp4Id.Location);
                        await Coroutine.Yield();
                    }
                    await CommonTasks.StopMoving();
                    return await ScriptHelper.ObjectInteraction(voidLamp4Id);
                }
            }

            return false;
        }

        // TO DO
        //
        // Obtain the bloody parchment => Use Return (if available) when you obtain the Bloody Parchment.
        //
        // Lady Amandine Tactics
        //
        // Use Void Lamps when targetable. (non-tanks)
        // --- Name:Void Lamp 0x1EA1688FBA0, Type:ff14bot.Objects.EventObject, ID:2000369, Obj:1074114908
        // --- Name:Void Lamp 0x1EA168907A0, Type:ff14bot.Objects.EventObject, ID:2000367, Obj:1074114910
        // --- Name:Void Lamp 0x1EA1688DDA0, Type:ff14bot.Objects.EventObject, ID:2000368, Obj:1074114909
        // --- Name:Void Lamp 0x1EA1688E5A0, Type:ff14bot.Objects.EventObject, ID:2000366, Obj:1074114911

    }
}
