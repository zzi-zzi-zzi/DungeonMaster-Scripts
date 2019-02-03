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
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class BardamsMettle : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 53;
        public override string Name => @"Bardam's Mettle";
        public override Version Version => new Version(0, 7, 1, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Garula", 6173, 0, 22.66f, new Vector3(3.96f, -0.50f, 251.17f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Bardam", 6177, 1, 30f, new Vector3(-28.46f, -45.01f, -14.33f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Yol", 6155, 2, 42.48f, new Vector3(23.94f, -167.50f, -483.54f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First wall before Garula
            new OffMeshConnection(new Vector3(-35.13f, 4.26f, 406.85f), new Vector3(-41.53f, 5.52f, 406.82f), ConnectionMode.Bidirectional),
            // Second wall before Garula
            new OffMeshConnection(new Vector3(4.08f, 1.46f, 290.77f), new Vector3(4.25f, 2.31f, 284.07f), ConnectionMode.Bidirectional),
            // Jump after Garula
            new OffMeshConnection(new Vector3(3.81f, -0.32f, 216.15f), new Vector3(5.64f, -29.76f, 205.62f), ConnectionMode.OneWay),
            // First Rock hammer before Bardam
            new OffMeshConnection(new Vector3(-51.26f, -45.00f, 119.25f), new Vector3(-46.42f, -45.11f, 112.76f), ConnectionMode.Bidirectional),
            // Second Rock hammer before Bardam
            new OffMeshConnection(new Vector3(-12.68f, -42.00f, 44.00f), new Vector3(-18.04f, -42.54f, 38.61f), ConnectionMode.Bidirectional),
            // First jump after Bardam
            new OffMeshConnection(new Vector3(-27.76f, -45.44f, -39.27f), new Vector3(-26.08f, -163.02f, -113.42f), ConnectionMode.OneWay),
            // Seconds jump after Bardam
            new OffMeshConnection(new Vector3(-49.14f, -162.71f, -170.44f), new Vector3(-52.33f, -192.33f, -180.58f), ConnectionMode.OneWay),
            // First gate before Yol
            new OffMeshConnection(new Vector3(-20.53f, -188.18f, -235.30f), new Vector3(-15.22f, -187.55f, -240.45f), ConnectionMode.Bidirectional),
            // Second gate before Yol
            new OffMeshConnection(new Vector3(25.13f, -172.82f, -360.76f), new Vector3(25.13f, -172.91f, -368.06f), ConnectionMode.Bidirectional),
            // Door before Yol
            new OffMeshConnection(new Vector3(24.00f, -172.33f, -405.06f), new Vector3(24.08f, -172.33f, -413.16f), ConnectionMode.Bidirectional)
        };

        private uint[] _emptyGaze = new uint[] { 7940 };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Right Wing (Yol)
                if (obj.Object.NpcId == 6183)
                    obj.Score += 2000;
                // Left Wing (Yol)
                if (obj.Object.NpcId == 6184)
                    obj.Score += 1000;
                // Corpsecleaner Eagle (Yol)
                if (obj.Object.NpcId == 6181)
                    obj.Score += 1000;
            }
        }

        [EncounterHandler(6177, "Bardam", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> BardamHandler(BattleCharacter c)
        {
            while (c.IsCasting && _emptyGaze.Contains(c.CastingSpellId))
            {
                ScriptHelper.LookAway(c);
                await Coroutine.Yield();
            }

            return false;
        }

        // Garula Tactics
        //
        // Rush => Move as far as possible from boss.
        //
        // Bardam Tactics
        //
        // Travail (lightning) => Stand on prism towers to dodge the attack.
        // Travail (firestorm) => To prevent dropping your AOE onto an ally's escape path, everyone should spread out and run clockwise.
        // Looming Shadow => Hide behind Star shard to dodge.
        // --- NOTE: This is a non-combat boss fight. Phases are composed of various mechanics players will have to successfully complete or dodge.
        //
        // Yol Tactics
        //
        // Flutterfall => Players marked with orange markers need to spread to avoid overlapping splash damage.
        // Eye of the Fierce => Look away from boss.
        // Yol retreats and spawns two Corpsecleaner Eagles (6181), the second when the first dies.
        // --- During this time, Yol will appear at the edge of the arena, performing line AoE attacks with no markers. Move out from in front of Yol when he appears to avoid damage.
        // Wingbeat Marks a player with a green marker. The marked player will be knocked to the outer edge of the arena. Get back to the center of the arena to avoid outer AoEs.

    }
}
