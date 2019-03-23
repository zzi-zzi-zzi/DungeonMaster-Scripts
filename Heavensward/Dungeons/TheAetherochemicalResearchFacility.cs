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

namespace DungeonMasterScripts.Heavensward.Dungeons
{
    public class TheAetherochemicalResearchFacility : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 38;
        public override string Name => @"The Aetherochemical Research Facility";
        public override Version Version => new Version(0, 9, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Regula van Hydrus", 3818, 0, 23.98f, new Vector3(-105.33f, 394.99f, -296.01f), () => ScriptHelper.IsTodoChecked(0)),
                //new Boss("Bioculture Node #1", 3830, 1, 19.31f, new Vector3(28.12f, 210.70f, 220.05f), () => ScriptHelper.IsTodoChecked(1) || (Core.Me.Z > 200 && (GameObjectManager.GetObjectByNPCId(3830) == null || !GameObjectManager.GetObjectByNPCId(3830).IsVisible))),
                //new Boss("Bioculture Node #2", 3830, 2, 29.84f, new Vector3(111.92f, 220.70f, 271.93f), () => ScriptHelper.IsTodoChecked(1) || (Core.Me.X > 80 && Core.Me.Z > 200 && (GameObjectManager.GetObjectByNPCId(3830) == null || !GameObjectManager.GetObjectByNPCId(3830).IsVisible))),
                new Boss("Bioculture Node #1", 3830, 1, 19.31f, new Vector3(28.12f, 210.70f, 220.05f), () => ScriptHelper.IsTodoChecked(1) || (Core.Me.Z > 200 && ((GameObjectManager.GetObjectByNPCId(3830).Location.Distance(new Vector3(28.12f, 210.70f, 220.05f)) > 5f) || (GameObjectManager.GetObjectByNPCId(3830) == null || !GameObjectManager.GetObjectByNPCId(3830).IsVisible)))),
                new Boss("Bioculture Node #2", 3830, 2, 29.84f, new Vector3(111.92f, 220.70f, 271.93f), () => ScriptHelper.IsTodoChecked(1) || (Core.Me.X > 80 && Core.Me.Z > 200 && ((GameObjectManager.GetObjectByNPCId(3830).Location.Distance(new Vector3(111.92f, 220.70f, 271.93f)) > 5f) || (GameObjectManager.GetObjectByNPCId(3830) == null || !GameObjectManager.GetObjectByNPCId(3830).IsVisible)))),
                new Boss("Harmachis", 3821, 3, 22.82f, new Vector3(253.91f, 225.14f, 272.05f), () => ScriptHelper.IsTodoChecked(2)),
                new Boss("Lift Terminal #1", 2005308, 4, 20.25f, new Vector3(203.53f, -28.16f, 195.85f), () => ScriptHelper.IsTodoChecked(3) || (WorldManager.ZoneId == 438 && WorldManager.SubZoneId == 1596) || (WorldManager.ZoneId == 438 && WorldManager.SubZoneId == 1602) || (Core.Me.X < 205 && Core.Me.Y < 0 && Core.Me.Z < 205 && (GameObjectManager.GetObjectByNPCId(2005308) == null || !GameObjectManager.GetObjectByNPCId(2005308).IsVisible))),
                new Boss("Lift Terminal #2", 2005309, 5, 25.36f, new Vector3(229.76f, -59.27f, 94.96f), () => ScriptHelper.IsTodoChecked(3) || (WorldManager.ZoneId == 438 && WorldManager.SubZoneId == 1596) || (WorldManager.ZoneId == 438 && WorldManager.SubZoneId == 1602) || (Core.Me.X > 210 && Core.Me.Y < 0 && Core.Me.Z < 110 && (GameObjectManager.GetObjectByNPCId(2005309) == null || !GameObjectManager.GetObjectByNPCId(2005309).IsVisible))),
                new Boss("Igeyorhm & Lahabrea", 3822, 6, 24.95f, new Vector3(227.29f, -96.46f, -187.32f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Ascian Prime", 3823, 7, 24.91f, new Vector3(230.08f, -456.46f, 71.98f), () => ScriptHelper.IsTodoChecked(4))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Pile of dead machines before Regula Van Hydrus
            new OffMeshConnection(new Vector3(-161.49f, 394.10f, -295.56f), new Vector3(-154.39f, 394.10f, -295.62f), ConnectionMode.Bidirectional),
            // Door after Regula Van Hydrus
            new OffMeshConnection(new Vector3(-73.80f, 393.93f, -295.54f), new Vector3(-69.49f, 394.02f, -295.54f), ConnectionMode.Bidirectional),
            // First transition line after Regula Van Hydrus
            new OffMeshConnection(new Vector3(-47.51f, 389.29f, -245.26f), new Vector3(-48.20f, 201.13f, 56.07f), ConnectionMode.Bidirectional),
            // Door after Bioculture Node #1
            new OffMeshConnection(new Vector3(28.02f, 209.96f, 262.53f), new Vector3(27.92f, 209.96f, 269.23f), ConnectionMode.Bidirectional),
            // Wall before Harmachis
            new OffMeshConnection(new Vector3(208.46f, 222.20f, 272.08f), new Vector3(229.56f, 225.07f, 272.13f), ConnectionMode.Bidirectional),
            // Door after Harmachis
            new OffMeshConnection(new Vector3(282.19f, 222.72f, 271.96f), new Vector3(287.49f, 222.20f, 272.02f), ConnectionMode.Bidirectional),
            // Transition line after Harmachis
            new OffMeshConnection(new Vector3(326.85f, 222.20f, 271.98f), new Vector3(-360.95f, -299.98f, -249.90f), ConnectionMode.Bidirectional),
            // First transition before Lahabrea & Igeyorhm
            new OffMeshConnection(new Vector3(-363.32f, -299.98f, -255.83f), new Vector3(308.83f, -27.92f, 275.83f), ConnectionMode.Bidirectional),
            // Second transition before Lahabrea & Igeyorhm
            new OffMeshConnection(new Vector3(228.03f, -90.16f, 48.98f), new Vector3(234.24f, -96.34f, -145.87f), ConnectionMode.Bidirectional)
        };

        private uint[] _entropicFlame = new uint[] { 10649, 9134, 7143, 6431, 4360, 4359 };
        private uint[] _magitekSpread = new uint[] { 6027, 4887, 4316 };
        private uint[] _petrifaction = new uint[] { 9649, 5431, 5374, 5154, 5028, 4331, 2824, 2526, 2516, 1979, 1969 };
        private uint[] _universalManipulation = new uint[] { 4357 };

        public override void OnEnter()
        {
            // "Void Zone"
            AddAvoidObject<EventObject>(6, 2006173);
            // "Void Zone" (Lahabrea & Igeyorhm)
            AddAvoidObject<EventObject>(6, 2002601);
            // Chaosphere (Ascian Prime)
            AddAvoidObject<BattleCharacter>(10, 4382);
            // Entropic Flame (Ascian Prime)
            AddAvoidLine(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _entropicFlame.Contains(i.CastingSpellId)),
                x => x.Location,
                x => x.Heading - (float)Math.PI,
                x => 60f,
                x => 20f,
                () => GameObjectManager.GetObjectsByNPCId(3823).Where(i => i.IsVisible),
                () => GameObjectManager.GetObjectByNPCId(3823).Location,
                40f
            );
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Blizzardsphere (Ascian Prime)
                //if (obj.Object.NpcId == 4383)
                //    obj.Score += 2000;
                // Cloned Conjurer
                if (obj.Object.NpcId == 3838)
                    obj.Score += 2000;
                // Magitek Turret II (Regula Van Hydrus)
                if (obj.Object.NpcId == 3820)
                    obj.Score += 2000;
                // Firesphere (Ascian Prime)
                //if (obj.Object.NpcId == 4384)
                //    obj.Score += 1000;
                // Cloned Thaumaturge
                if (obj.Object.NpcId == 3839)
                    obj.Score += 1000;
                // 6th Legion Medicus
                if (obj.Object.NpcId == 3829)
                    obj.Score += 1000;
                // Magitek Turret I (Regula Van Hydrus)
                if (obj.Object.NpcId == 3819)
                    obj.Score += 1000;
            }
        }

        //[EncounterHandler(3818, "Regula van Hydrus", 30, CallBehaviorMode.InCombat)]
        //public async Task<bool> RegulavanHydrusHandler(BattleCharacter c)
        //{
        //    while (c.IsCasting && _magitekSpread.Contains(c.CastingSpellId))
        //    {
        //        if (Core.Me.Distance2D(c.CalculatePointBehind(5f)) > 3f)
        //        {
        //            await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.CalculatePointBehind(5f), "Regula van Hydrus"), 3f, true);
        //            return true;
        //        }
        //    }

        //    return false;
        //}
        [LocationHandler(-90.06f, 392.15f, -274.19f, 50f)]
        public async Task<bool> ZoneTransitionHandler(Vector3 context)
        {
            var MidPoint = new Vector3(-89.54f, 395.07f, -295.54f);
            var MidPoint2 = new Vector3(-51.36f, 393.99f, -291.57f);
            var EndPoint = new Vector3(-47.49f, 389.24f, -245.13f);

            if (ScriptHelper.InCombat())
                return false;
            if (ScriptHelper.IsTodoChecked(0))
            {
                while (!Navigator.InPosition(MidPoint, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(MidPoint);
                    await Coroutine.Yield();
                }
                while (!Navigator.InPosition(MidPoint2, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(MidPoint2);
                    await Coroutine.Yield();
                }
                while (!Navigator.InPosition(EndPoint, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(EndPoint);
                    await Coroutine.Yield();
                }
                if (Core.Me.Location.Distance(EndPoint) < 2f)
                {
                    MovementManager.SetFacing(0.0019f);
                    MovementManager.MoveForwardStart();
                    await Coroutine.Sleep(2000);
                    MovementManager.MoveStop();
                }
            }

            return false;
        }
        [LocationHandler(28.12f, 210.70f, 220.05f, 5f)]
        public async Task<bool> BiocultureNodeHandler(Vector3 context)
        {
            var biocultureNodeId = GameObjectManager.GetObjectByNPCId(3830);
            var biocultureNodeExpectedLocation = new Vector3(28.12f, 210.70f, 220.05f);

            if (ScriptHelper.InCombat())
                return false;
            if (biocultureNodeId != null && biocultureNodeId.IsVisible && biocultureNodeId.Location.Distance(biocultureNodeExpectedLocation) < 5f)
            {
                if (biocultureNodeId != null && biocultureNodeId.IsVisible && biocultureNodeId.IsTargetable)
                {
                    while (!Navigator.InPosition(biocultureNodeId.Location, Core.Me.Location, 3f))
                    {
                        Navigator.PlayerMover.MoveTowards(biocultureNodeId.Location);
                        await Coroutine.Yield();
                    }
                    biocultureNodeId.Interact();
                    return true;
                }
            }

            return false;
        }
        [LocationHandler(111.92f, 220.70f, 271.93f, 5f)]
        public async Task<bool> BiocultureNodeHandler2(Vector3 context)
        {
            var biocultureNodeId = GameObjectManager.GetObjectByNPCId(3830);
            var biocultureNodeExpectedLocation = new Vector3(111.92f, 220.70f, 271.93f);

            if (ScriptHelper.InCombat())
                return false;
            if (biocultureNodeId != null && biocultureNodeId.IsVisible && biocultureNodeId.Location.Distance(biocultureNodeExpectedLocation) < 5f)
            {
                if (biocultureNodeId != null && biocultureNodeId.IsVisible && biocultureNodeId.IsTargetable)
                {
                    while (!Navigator.InPosition(biocultureNodeId.Location, Core.Me.Location, 3f))
                    {
                        Navigator.PlayerMover.MoveTowards(biocultureNodeId.Location);
                        await Coroutine.Yield();
                    }
                    biocultureNodeId.Interact();
                    return true;
                }
            }

            return false;
        }
        [EncounterHandler(3821, "Harmachis", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> HarmachisHandler(BattleCharacter c)
        {
            while (c.IsCasting && _petrifaction.Contains(c.CastingSpellId))
            {
                ScriptHelper.LookAway(c);
                await Coroutine.Yield();
            }

            return false;
        }
        [LocationHandler(278.10825f, 223.63345f, 271.99935f, 50f)]
        public async Task<bool> ZoneTransitionHandler2(Vector3 context)
        {
            var MidPoint = new Vector3(266.15f, 225.07f, 271.69f);
            var EndPoint = new Vector3(326.85f, 222.20f, 271.98f);

            if (ScriptHelper.InCombat())
                return false;
            if (ScriptHelper.IsTodoChecked(2))
            {
                while (!Navigator.InPosition(MidPoint, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(MidPoint);
                    await Coroutine.Yield();
                }
                while (!Navigator.InPosition(EndPoint, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(EndPoint);
                    await Coroutine.Yield();
                }
                if (Core.Me.Location.Distance(EndPoint) < 2f)
                {
                    MovementManager.SetFacing(1.56f);
                    MovementManager.MoveForwardStart();
                    await Coroutine.Sleep(2000);
                    MovementManager.MoveStop();
                }
            }

            return false;
        }
        // Lift Terminal - NpcId2005307 - TODOStep: 2 - TODOValue (Before Pickup): 1 /  1
        // Lift Terminal - NpcId2005308 - TODOStep: 2 - TODOValue (Before Pickup): 1 /  1
        // Lift Terminal - NpcId2005309 - TODOStep: 2 - TODOValue (Before Pickup): 1 /  1\n
        [ObjectHandler(2005307, ObjectRange = 35)]
        [ObjectHandler(2005308, ObjectRange = 35)]
        [ObjectHandler(2005309, ObjectRange = 35)]
        public async Task<bool> LiftTerminalHandler(GameObject context)
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
        [LocationHandler(-359.73285f, -299.984f, -249.57315f, 10f)]
        public async Task<bool> ZoneTransitionHandler3(Vector3 context)
        {
            var monitoringDroneId = GameObjectManager.GetObjectByNPCId(3837);
            var EndPoint = new Vector3(-363.32f, -299.98f, -255.83f);

            if (ScriptHelper.InCombat())
                return false;
            if (monitoringDroneId != null && monitoringDroneId.IsVisible && monitoringDroneId.IsTargetable)
            {
                while (!Navigator.InPosition(monitoringDroneId.Location, Core.Me.Location, 3f))
                {
                    Navigator.PlayerMover.MoveTowards(monitoringDroneId.Location);
                    await Coroutine.Yield();
                }
                monitoringDroneId.Interact();
                return true;
            }
            if (ScriptHelper.IsTodoChecked(2))
            {
                while (!Navigator.InPosition(EndPoint, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(EndPoint);
                    await Coroutine.Yield();
                }
                if (Core.Me.Location.Distance(EndPoint) < 2f)
                {
                    MovementManager.SetFacing(3.67f);
                    MovementManager.MoveForwardStart();
                    await Coroutine.Sleep(2000);
                    MovementManager.MoveStop();
                }
            }

            return false;
        }
        [LocationHandler(224.7445f, -89.71536f, 76.650255f, 30f)]
        public async Task<bool> ZoneTransitionHandler4(Vector3 context)
        {
            var MidPoint = new Vector3(226.42f, -89.44f, 86.27f);
            var MidPoint2 = new Vector3(226.23f, -89.45f, 68.92f);
            var EndPoint = new Vector3(228.03f, -90.16f, 48.98f);

            if (ScriptHelper.InCombat())
                return false;
            if (ScriptHelper.IsTodoChecked(2))
            {
                while (!Navigator.InPosition(MidPoint, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(MidPoint);
                    await Coroutine.Yield();
                }
                while (!Navigator.InPosition(MidPoint2, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(MidPoint2);
                    await Coroutine.Yield();
                }
                while (!Navigator.InPosition(EndPoint, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(EndPoint);
                    await Coroutine.Yield();
                }
                if (Core.Me.Location.Distance(EndPoint) < 2f)
                {
                    MovementManager.SetFacing(3.15f);
                    MovementManager.MoveForwardStart();
                    await Coroutine.Sleep(2000);
                    MovementManager.MoveStop();
                }
            }

            return false;
        }
        [EncounterHandler(3823, "Ascian Prime", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> AscianPrimeHandler(BattleCharacter c)
        {
            var firesphereId = GameObjectManager.GetObjectByNPCId(4384);
            var blizzardsphereId = GameObjectManager.GetObjectByNPCId(4383);
            var aetherialTearId = GameObjectManager.GetObjectByNPCId(3293);

            while (blizzardsphereId != null && blizzardsphereId.IsVisible && blizzardsphereId.IsTargetable)
            {
                if (!Navigator.InPosition(blizzardsphereId.Location, Core.Me.Location, 3f))
                {
                    Navigator.PlayerMover.MoveTowards(blizzardsphereId.Location);
                    await Coroutine.Yield();
                }
                blizzardsphereId.Interact();
                return true;
            }
            while (firesphereId != null && firesphereId.IsVisible && firesphereId.IsTargetable)
            {
                if (!Navigator.InPosition(firesphereId.Location, Core.Me.Location, 3f))
                {
                    Navigator.PlayerMover.MoveTowards(firesphereId.Location);
                    await Coroutine.Yield();
                }
                firesphereId.Interact();
                return true;
            }
            while (c.IsCasting && _universalManipulation.Contains(c.CastingSpellId))
            {
                await Coroutine.Sleep(1000);
                if (Core.Me.Distance2D(aetherialTearId.Location) > 1f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(aetherialTearId.Location, "Aetherial Tear"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }

            return false;
        }

        // Regula van Hydrus Tactics
        //
        // Magitek Spread => Greater than 180 degree frontal cone AoE attack. Move to rear of boss to avoid damage.
        //
        // Harmachis Tactics
        //
        // Ballistic Missile => Stack on targeted player to spread the AoE damage.
        // Gaseous Bomb => Stack on targeted player to spread the AoE damage.
        //
        // Lahabrea & Igeyorhm Tactics
        //
        // Blizzard Sphere => Avoid by 8y(?).
        // --- Name:Blizzardsphere 0x2A7CBC497B0, Type:ff14bot.Objects.BattleCharacter, ID:4383, Obj:1074042723
        // Fire Sphere => Avoid by 8y(?).
        // --- Name:Firesphere 0x2A7CBC5F2B0, Type:ff14bot.Objects.BattleCharacter, ID:4384, Obj:1074042731
        //
        // Ascian Prime Tactics
        //
        // "Donut" AoE attack. Stack near boss.

    }
}
