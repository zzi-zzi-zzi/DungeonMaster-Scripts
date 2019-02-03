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
    public class CopperbellMines : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 3;
        public override string Name => @"Copperbell Mines";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                // Tiny Key #1
                new Boss("Spriggan Copper Copper", 631, 0, 30f, new Vector3(-220.90f, 23.77f, -215.66f), () => DMDirectorManager.Instance.GetTodoArgs(1).Item1 == 1 || DMDirectorManager.Instance.GetI8B == 1 || Core.Me.Y < 0 || !GameObjectManager.GetObjectByNPCId(2000160).IsVisible),
                new Boss("Lift #1", 2000160, 1, 2f, new Vector3(-181.91f, 24f, -208.22f), () => Core.Me.Y < 20f),
                // Clear shaft B4 of rubble
                new Boss("Blasting Device #1", 0, 2, 5f, new Vector3(41.37f, -9.25f, -135.24f), () => DMDirectorManager.Instance.GetTodoArgs(1).Item1 == 1),
                new Boss("Spriggan Sifter", 716, 3, 21.23f, new Vector3(47.14f, -9.67f, -92.58f), () => DMDirectorManager.Instance.GetTodoArgs(1).Item1 == 1 && Core.Me.Y < 0 && (DMDirectorManager.Instance.GetI8A > 1 || (GameObjectManager.GetObjectByNPCId(2000173) == null || !GameObjectManager.GetObjectByNPCId(2000173).IsVisible))),
                new Boss("Lift #2", 2000175, 4, 2f, new Vector3(55.97f, -8.26f, 6.02f), () => Core.Me.Y < -20f),
                // Firesand #5
                new Boss("Blasting Cap", 1303, 5, 11.19f, new Vector3(21.56f, -42.44f, 39.51f), () => DMDirectorManager.Instance.GetTodoArgs(0).Item1 > 0 || DMDirectorManager.Instance.GetTodoArgs(2).Item1 == 1),
                // Firesand #6
                new Boss("Blasting Cap", 1303, 6, 33.30f, new Vector3(97.34f, -41.83f, 41.55f), () => DMDirectorManager.Instance.GetTodoArgs(0).Item1 > 1 || DMDirectorManager.Instance.GetTodoArgs(2).Item1 == 1),
                // Clear shaft E1 of rubble
                new Boss("Blasting Device #2", 0, 7, 5f, new Vector3(56.73f, -38.00f, 47.99f), () => DMDirectorManager.Instance.GetTodoArgs(2).Item1 == 1),
                new Boss("Ichorous Ire", 554, 8, 15.07f, new Vector3(30.21f, -37.86f, 102.57f), () => DMDirectorManager.Instance.GetI8A > 1),
                // Arrive in shaft E2
                new Boss("Blasting Device #3", 0, 9, 5f, new Vector3(10.16f, -36.85f, 106.70f), () => DMDirectorManager.Instance.GetTodoArgs(3).Item1 == 1),
                new Boss("Gyges the Great", 101, 10, 35.76f, new Vector3(-102.49f, -58.50f, 15.31f), () => ScriptHelper.IsTodoChecked(4))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Door before Kottos
            new OffMeshConnection(new Vector3(-206.80f, 23.49f, -208.50f), new Vector3(-202.40f, 23.08f, -208.53f), ConnectionMode.Bidirectional),
            // Rubble before Kottos
            new OffMeshConnection(new Vector3(42.74f, -10.01f, -129.38f), new Vector3(43.70f, -9.67f, -118.58f), ConnectionMode.Bidirectional),
            // Door after Kottos
            new OffMeshConnection(new Vector3(43.53f, -9.92f, -60.76f), new Vector3(43.64f, -9.92f, -54.85f), ConnectionMode.Bidirectional),
            // Rubble before Ichorous Ire
            new OffMeshConnection(new Vector3(59.46f, -38.75f, 52.65f), new Vector3(58.60f, -39.04f, 63.30f), ConnectionMode.Bidirectional),
            // Rubble after Ichorous Ire
            new OffMeshConnection(new Vector3(6.97f, -38.07f, 111.69f), new Vector3(-1.16f, -37.71f, 113.34f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Blasting Cap (Ichorous Ire)
            AddAvoidObject<BattleCharacter>(7, 555);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Stone Servant (Gyges the Great)
                if (obj.Object.NpcId == 988)
                    obj.Score += 1000;
                // Spriggan Quencher (Ichorous Ire)
                if (obj.Object.NpcId == 985)
                    obj.Score += 1000;
            }
        }

        private readonly uint[] _excludeTargets = {
            555,
        };

        public override void IncludeTargetsFilter(List<GameObject> incomingObjects, HashSet<GameObject> outgoingObjects)
        {
            base.IncludeTargetsFilter(incomingObjects, outgoingObjects);
            foreach (var @in in incomingObjects.Where(i => _excludeTargets.Contains(i.NpcId)))
                outgoingObjects.Remove(@in);
        }

        // Tiny Key - NpcId2000159 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000159, ObjectRange = 25)]
        public async Task<bool> TinyKeyHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!ScriptHelper.HasKeyItem(2000441))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Sealed Blasting Door - NpcId2000160 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000160, ObjectRange = 25)]
        public async Task<bool> SealedBlastingDoorHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2000441))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Lift Lever - NpcId2000163 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Lift Lever - NpcId2000175 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000163, ObjectRange = 25)]
        [ObjectHandler(2000175, ObjectRange = 25)]
        public async Task<bool> LiftLeverHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible)
                return await ScriptHelper.ObjectInteraction(context, 2f);

            return false;
        }
        [LocationHandler(-184.25f, -6.00f, -208.06f, 3f)]
        public async Task<bool> ExitLiftHandler(Vector3 context)
        {
            var EndPoint = new Vector3(-170.00f, -7.67f, -208.60f);

            if (ScriptHelper.InCombat())
                return false;
            while (!Navigator.InPosition(EndPoint, Core.Me.Location, 3f))
            {
                Navigator.PlayerMover.MoveTowards(EndPoint);
                await Coroutine.Yield();
            }
            await CommonTasks.StopMoving();

            return false;
        }
        // Firesand - NpcId2000169 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Firesand - NpcId2000172 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Firesand - NpcId2001531 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1
        // Firesand - NpcId2000179 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1
        // Firesand - NpcId2001532 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1
        // Firesand - NpcId2001533 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000169, ObjectRange = 25)]
		[ObjectHandler(2000172, ObjectRange = 25)]
        [ObjectHandler(2001531, ObjectRange = 25)]
        [ObjectHandler(2000179, ObjectRange = 25)]
        [ObjectHandler(2001532, ObjectRange = 10)]
        [ObjectHandler(2001533, ObjectRange = 25)]
        public async Task<bool> FiresandHandler(GameObject context)
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
        // Powder Chamber - NpcId2001536 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Powder Chamber - NpcId2001537 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1
        // Powder Chamber - NpcId2001538 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2001536, ObjectRange = 25)]
        [ObjectHandler(2001537, ObjectRange = 25)]
        [ObjectHandler(2001538, ObjectRange = 25)]
        public async Task<bool> PowderChamberHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (DMDirectorManager.Instance.GetTodoArgs(0).Item1 == 2)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Blasting Device - NpcId2000170 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000170, ObjectRange = 25)]
        public async Task<bool> BlastingDeviceHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!GameObjectManager.GetObjectByNPCId(2001536).IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        [LocationHandler(43.46f, -9.85f, -128.56f, 3f)]
        public async Task<bool> MovePastRubbleHandler(Vector3 context)
        {
            var EndPoint = new Vector3(45.50f, -9.57f, -114.68f);

            if (ScriptHelper.InCombat())
                return false;
            while (!Navigator.InPosition(EndPoint, Core.Me.Location, 1f))
            {
                Navigator.PlayerMover.MoveTowards(EndPoint);
                await Coroutine.Yield();
            }
            await CommonTasks.StopMoving();

            return false;
        }
        // Tiny Key - NpcId2000178 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000178, ObjectRange = 30)]
        public async Task<bool> TinyKeyHandler2(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!ScriptHelper.HasKeyItem(2000441) && context.IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Sealed Blasting Door - NpcId2000173 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000173, ObjectRange = 25)]
        public async Task<bool> SealedBlastingDoorHandler2(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2000441))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Blasting Device - NpcId2000180 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000180, ObjectRange = 25)]
        public async Task<bool> BlastingDeviceHandler2(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!GameObjectManager.GetObjectByNPCId(2001537).IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        [LocationHandler(59.40f, -38.75f, 52.72194f, 3f)]
        public async Task<bool> MovePastRubbleHandler2(Vector3 context)
        {
            var EndPoint = new Vector3(58.64f, -38.94f, 65.79f);

            if (ScriptHelper.InCombat())
                return false;
            while (!Navigator.InPosition(EndPoint, Core.Me.Location, 1f))
            {
                Navigator.PlayerMover.MoveTowards(EndPoint);
                await Coroutine.Yield();
            }
            await CommonTasks.StopMoving();

            return false;
        }
        [EncounterHandler(554, "IchorousIre", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> IchorousIreHandler(BattleCharacter c)
        {
            var improvedBlastingDeviceId = GameObjectManager.GetObjectByNPCId(2000184);

            if (improvedBlastingDeviceId.IsVisible)
            {
                if (Core.Me.Distance2D(improvedBlastingDeviceId.Location) > 3f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(improvedBlastingDeviceId.Location, "Improved Blasting Device"), 2f, true);
                    return true;
                }
                improvedBlastingDeviceId.Interact();
                return true;
            }

            return false;
        }
        // Blasting Device - NpcId2001534 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2001534, ObjectRange = 25)]
        public async Task<bool> BlastingDeviceHandler3(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!GameObjectManager.GetObjectByNPCId(2001538).IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }

        // Kottos the Gigas Tactics
        //
        // Grand Slam => Frontal AoE attack. Tank away from party.
        //
        // Ichorous Ire Tactics
        //
        // Ichorous Ire => When count > 4, non-tanks can attack them.

    }
}
