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
using DungeonMaster.Helpers;
using DungeonMaster.Managers;
using DungeonMaster.TargetingSystems;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;
using ff14bot.Helpers;

namespace DungeonMasterScripts.Heavensward.Dungeons
{
    public class TheGreatGubalLibrary : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 31;
        public override string Name => @"The Great Gubal Library";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Demon Tome", 3923, 0, 19.99f, new Vector3(0.49f, 0.00f, 0.01f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("Byblos", 3925, 1, 23.79f, new Vector3(177.78f, -8.00f, 27.12f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("The Everliving Bibliotaph", 3930, 2, 23.02f, new Vector3(377.76f, -39.00f, -59.76f), () => ScriptHelper.IsTodoChecked(2))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First door before Demon Tome
            new OffMeshConnection(new Vector3(-317.64f, 0.00f, 0.07f), new Vector3(-312.15f, 0.00f, 0.12f), ConnectionMode.Bidirectional),
            // Second door before Demon Tome
            new OffMeshConnection(new Vector3(-173.39f, 12.00f, -34.11f), new Vector3(-169.62f, 12.00f, -30.39f), ConnectionMode.Bidirectional),
            // Third door before Demon Tome
            new OffMeshConnection(new Vector3(-48.28f, 0.00f, -0.14f), new Vector3(-43.58f, 0.00f, -0.14f), ConnectionMode.Bidirectional),
            // Door after Demon Tome
            new OffMeshConnection(new Vector3(43.93f, 0.00f, 0.04f), new Vector3(47.93f, 0.00f, 0.11f), ConnectionMode.Bidirectional),
            // First glyph before Byblos
            new OffMeshConnection(new Vector3(80.77f, -8.00f, 72.00f), new Vector3(86.37f, -9.00f, 71.99f), ConnectionMode.Bidirectional),
            // Second glyph before Byblos
            new OffMeshConnection(new Vector3(117.28f, -9.00f, 71.87f), new Vector3(122.18f, -8.00f, 71.91f), ConnectionMode.Bidirectional),
            // Door before Byblos
            new OffMeshConnection(new Vector3(156.09f, -9.00f, 96.04f), new Vector3(159.59f, -9.00f, 96.04f), ConnectionMode.Bidirectional),
            // Glyph after Byblos
            new OffMeshConnection(new Vector3(177.84f, -8.00f, -9.05f), new Vector3(177.82f, -8.00f, -13.16f), ConnectionMode.Bidirectional),
            // Gate before The Everliving Bibliotaph
            new OffMeshConnection(new Vector3(297.94f, -24.00f, -44.43f), new Vector3(297.91f, -24.00f, -39.13f), ConnectionMode.Bidirectional),
            // Door before The Everliving Bibliotaph
            new OffMeshConnection(new Vector3(324.78f, -39.00f, -59.68f), new Vector3(332.68f, -39.00f, -59.70f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Whale Oil (Byblos)
            AddAvoidObject<BattleCharacter>(7, 3929);
            // Voidsphere (The Everliving Bibliotaph)
            AddAvoidObject<BattleCharacter>(10, 4420);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Bibliophile (The Everliving Bibliotaph)
                if (obj.Object.NpcId == 3933)
                    obj.Score += 1000;
                // Bibliomancer (The Everliving Bibliotaph)
                if (obj.Object.NpcId == 3932)
                    obj.Score += 1000;
                // Biblioklept (The Everliving Bibliotaph)
                if (obj.Object.NpcId == 3931)
                    obj.Score += 1000;
                // Page 64 (Byblos)
                if (obj.Object.NpcId == 3915)
                    obj.Score += 1000;
            }
        }

        // Demon Tome Tactics
        //
        // Liquefy => SideStep? Alternating column AoE attack. Slowed if hit.
        // Disclosure => Move to closest flank of the boss when cast starts, then back to previous position after cast finishes.
        //
        // Byblos Tactics
        //
        // Gale Cut => Frontal-cone attack. Tanks should keep the boss facing away. If tanking, move to flank of boss when cast.
        // When Page 64's die, they will draw a tether between a spawned orb and a given player. Draw this orb into Byblos to break his invulnerability.
        // Tomewind (3928) => Move to nearest cloud when your HP > 80%
        // Tail Swipe => Only when someone is behind him? Avoid rear of boss by 5y(?).
        //
        // The Everliving Bibliotaph Tactics
        //
        // Voidsphere => Player with indicator should run to one side of the arena to drop the AoE he will place on you.
        // Void Summon => 3 intervals throughout the fight (roughly 85%, 55%, and 25% HP)
        // --- Platforms on the ground will light up, and will need X players to stand on them in order to cancel their summon.
        // ------ 6 platforms w/ 1 light (85%)
        // ------ 4 platforms w/ 2 lights (55%)
        // ------ 2 platforms w/ 3 lights (25%)

    }
}
