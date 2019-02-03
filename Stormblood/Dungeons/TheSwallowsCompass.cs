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

namespace DungeonMasterScripts.ARealmReborn
{
    public class TheSwallowsCompass : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 61;
        public override string Name => @"The Swallow's Compass";
        public override Version Version => new Version(0, 9, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Geomantic Kiyofusa", 7207, 0, 30f, new Vector3(-235.95f, -478.40f, 62.52f), () => ScriptHelper.IsTodoChecked(0) || (GameObjectManager.GetObjectByNPCId(7207) == null || !GameObjectManager.GetObjectByNPCId(7207).IsVisible)),
                new Boss("Sai Taisui", 7206, 0, 24.06f, new Vector3(-299.20f, -478.33f, 7.71f), () => ScriptHelper.IsTodoChecked(0) || (GameObjectManager.GetObjectByNPCId(7206) == null || !GameObjectManager.GetObjectByNPCId(7206).IsVisible)),
                new Boss("Otengu", 7200, 1, 33.15f, new Vector3(-239.89f, -480.00f, -4.02f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Daidarabotchi", 7202, 2, 38.18f, new Vector3(25.82f, 0.00f, 294.70f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Wind Gust #1", 2007457, 3, 3f, new Vector3(-46.20f, -0.41f, 150.65f), () => ScriptHelper.IsTodoChecked(3) && Core.Me.Z < 100f),
                new Boss("Wind Gust #2", 2007457, 4, 3f, new Vector3(13.79f, 0.97f, -52.86f), () => ScriptHelper.IsTodoChecked(3) && Core.Me.Y > 64f),
                new Boss("Qitian Dasheng", 7203, 5, 36.08f, new Vector3(14.95f, 73.00f, -252.18f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First door before Otengu
            new OffMeshConnection(new Vector3(-180.78f, -478.40f, -15.92f), new Vector3(-180.87f, -478.40f, -22.32f), ConnectionMode.Bidirectional),
            // Second door before Otengu
            new OffMeshConnection(new Vector3(-255.39f, -478.40f, -59.28f), new Vector3(-262.29f, -478.40f, -59.11f), ConnectionMode.Bidirectional),
            // Third door before Otengu
            new OffMeshConnection(new Vector3(-299.20f, -478.39f, 15.54f), new Vector3(-299.15f, -478.40f, 21.44f), ConnectionMode.Bidirectional),
            // Debris before Otengu
            new OffMeshConnection(new Vector3(-264.99f, -478.36f, 59.17f), new Vector3(-252.09f, -478.31f, 59.04f), ConnectionMode.OneWay),
            // Door before Otengu
            new OffMeshConnection(new Vector3(-240.35f, -478.40f, 47.50f), new Vector3(-240.33f, -479.65f, 41.00f), ConnectionMode.Bidirectional),
            // First wave wall before Daidarabotchi
            new OffMeshConnection(new Vector3(220.34f, -454.00f, 33.55f), new Vector3(219.96f, -454.64f, 22.05f), ConnectionMode.Bidirectional),
            // Second wave wall before Daidarabotchi
            new OffMeshConnection(new Vector3(287.92f, -471.06f, -147.47f), new Vector3(288.16f, -471.59f, -160.86f), ConnectionMode.Bidirectional),
            // Gate before Qitian Dasheng
            new OffMeshConnection(new Vector3(14.95f, 65.05f, -191.30f), new Vector3(14.98f, 66.17f, -196.20f), ConnectionMode.Bidirectional)
        };

        private uint[] _fiveFingeredPunishment = new uint[] { 11184, 11179 };
        private uint[] _wileOfTheTengu = new uint[] { 12190, 11159 };

        public override void OnEnter()
        {
            // Wildswind(?)
            // AddAvoidObject<BattleCharacter>(5, 108);
            // Mythmaker #1 (Daidarabotchi)
            AddAvoidObject<EventObject>(10, 2009482);
            // Mythmaker #2 (Daidarabotchi)
            AddAvoidObject<EventObject>(12, 2009483);
            // Mythmaker #3 (Daidarabotchi)
            AddAvoidObject<EventObject>(15, 2009484);
            // Tengu Embers (Otengu)
            AddAvoidObject<BattleCharacter>(3, 7201);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Dragon Bi Fang
                if (obj.Object.NpcId == 7218)
                    obj.Score += 1000;
                // Torrent
                if (obj.Object.NpcId == 7209)
                    obj.Score += 1000;
                // Servant of the Sage (Qitian Dasheng)
                if (obj.Object.NpcId == 7205)
                    obj.Score += 1000;
                // Shadow of the Sage (Qitian Dasheng)
                if (obj.Object.NpcId == 7204)
                    obj.Score += 1000;
            }
        }

        // Dragon's Seal - NpcId2009461 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2009461, ObjectRange = 5)]
        public async Task<bool> DragonsSealHandler(GameObject context)
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
        [EncounterHandler(7200, "Otengu", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> OtenguHandler(BattleCharacter c)
        {
            while (c.IsCasting && _wileOfTheTengu.Contains(c.CastingSpellId))
            {
                ScriptHelper.LookAway(c);
                await Coroutine.Yield();
            }

            return false;
        }
        // Geomantic Aetheryte - NpcId2009459 - TODOStep: 1 - TODOValue (Before Pickup): 1 /  1\n
        [ObjectHandler(2009459, ObjectRange = 50)]
        public async Task<bool> GeomanticAetheryteHandler(GameObject context)
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
        // Geomantic Aetheryte - NpcId2009460 - TODOStep: 1 - TODOValue (Before Pickup): 1 /  1\n
		[ObjectHandler(2009460, ObjectRange = 50)]
        public async Task<bool> GeomanticAetheryteHandler2(GameObject context)
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
        [EncounterHandler(7202, "Daidarabotchi", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> DaidarabotchiHandler(BattleCharacter c)
        {
            var SafeSpot = new Vector3(-25.85f, 1.19f, 332.90f);

            if (ScriptHelper.HasTargetOmen(Core.Me))
            {
                await Coroutine.Sleep(2000);
                while (Core.Me.Location.Distance(SafeSpot) > 5f)
                {
                    Navigator.PlayerMover.MoveTowards(SafeSpot);
                    await Coroutine.Yield();
                }
            }

            return false;
        }
        [LocationHandler(-46.1973f, -0.4050641f, 150.6478f, 15f)]
        public async Task<bool> WindGustHandler(Vector3 context)
        {
            var WindGust = new Vector3(-50.63f, -0.50f, 141.92f);

            if (ScriptHelper.InCombat())
                return false;
            while (Core.Me.Location.Distance(new Vector3(-97.70f, -11.34f, 56.85f)) > 5f)
            {
                if (!Navigator.InPosition(WindGust, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(WindGust);
                    await Coroutine.Yield();
                }
            }
            await CommonTasks.StopMoving();

            return false;
        }
        [LocationHandler(13.79488f, 0.9662582f, -52.85693f, 15f)]
        public async Task<bool> WindGustHandler2(Vector3 context)
        {
            var WindGust2 = new Vector3(1.97f, 0.85f, -87.82f);

            if (ScriptHelper.InCombat())
                return false;
            while (Core.Me.Location.Distance(new Vector3(10.36f, 65.05f, -152.71f)) > 5f)
            {
                if (!Navigator.InPosition(WindGust2, Core.Me.Location, 1f))
                {
                    Navigator.PlayerMover.MoveTowards(WindGust2);
                    await Coroutine.Yield();
                }
            }
            await CommonTasks.StopMoving();

            return false;
        }
        [EncounterHandler(7203, "Qitian Dasheng", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> QitianDashengHandler(BattleCharacter c)
        {
            if (c.IsCasting && _fiveFingeredPunishment.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 5f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Qitian Dasheng"), 3f, true);
                    return true;
                }
            }

            return false;
        }

        // TO DO
        //
        // Wildswind => Avoidance commented out due to very bot-like behavior.
        //
        // Daidarabotchi Tactics
        //
        // Arm attacks (not telegraphed) covering left/right hemispheres of platform.
        // Mountain Falls (11173) => blue water drop icon above head => avoid all players (10y)
        // The Land take You, Claim You! => AoE that will follow the marked player. Lock on indicator.
        //
        // Qitian Dasheng Tactics
        //
        // Both Ends (glowing staff only) => move to boss (3y)
        // --- 11183, 11182, 11177, 11176; determine which is the glowing staff for moving to boss

    }
}
