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

namespace DungeonMasterScripts.Heavensward.Dungeons
{
    public class TheAery : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 39;
        public override string Name => @"The Aery";
        public override Version Version => new Version(0, 7, 0, 1);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Rangda", 3452, 0, 29.49f, new Vector3(335.88f, 94.00f, -208.49f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Gyascutus", 3455, 1, 35.82f, new Vector3(1.76f, 59.66f, 64.12f), () => ScriptHelper.IsTodoChecked(5)),
                new Boss("Nidhogg", 3458, 2, 39.21f, new Vector3(34.96f, 148.40f, -278.98f), () => ScriptHelper.IsTodoChecked(7))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First "spires" before Rangda
            new OffMeshConnection(new Vector3(265.47f, 64.70f, 47.58f), new Vector3(269.81f, 64.79f, 45.33f), ConnectionMode.Bidirectional),
            // Second "spires" before Rangda
            new OffMeshConnection(new Vector3(358.80f, 81.18f, -119.35f), new Vector3(360.72f, 82.07f, -124.61f), ConnectionMode.Bidirectional),
            // Jump after Rangda
            new OffMeshConnection(new Vector3(283.05f, 92.33f, -172.74f), new Vector3(228.61f, 11.92f, -167.43f), ConnectionMode.OneWay),
            // First gas wall before Gyascutus
            new OffMeshConnection(new Vector3(133.26f, 5.87f, -104.12f), new Vector3(129.33f, 7.44f, -92.89f), ConnectionMode.Bidirectional),
            // Second gas wall before Gyascutus

            // First pillar before Nidhogg (OneWay)
            new OffMeshConnection(new Vector3(-83.35f, 92.73f, -14.10f), new Vector3(-36.87f, 108.25f, -40.17f), ConnectionMode.OneWay),
            // Second pillar before Nidhogg (OneWay)
            new OffMeshConnection(new Vector3(84.29f, 107.71f, -46.11f), new Vector3(102.44f, 123.26f, -84.25f), ConnectionMode.OneWay),
            // Spire before Nidhogg
            new OffMeshConnection(new Vector3(34.95f, 133.81f, -193.76f), new Vector3(34.97f, 137.42f, -204.46f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Malady (Gyascutus)
            AddAvoidObject<BattleCharacter>(5, 3457);
            // The Scarlet Price (Nidhogg)
            AddAvoidObject<BattleCharacter>(5, 2006454);
        }

        public override void OnExit()
        {

        }

        private uint[] _electricCachexia = new uint[] { 6235, 3889 };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // The Sable Price (Nidhogg)
                if (obj.Object.NpcId == 3460)
                    obj.Score += 1000;
                // Leyak (Rangda)
                if (obj.Object.NpcId == 3453)
                    obj.Score += 1000;
            }
        }

        [EncounterHandler(3452, "Rangda", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> RangdaHandler(BattleCharacter c)
        {
            if (c.IsCasting && _electricCachexia.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 5f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Rangda"), 5f, true);
                    return true;
                }
            }

            return false;
        }
        [LocationHandler(211.0701f, 5.515992f, -119.7546f, 5f)]
        public async Task<bool> DragonBreathHandler(GameObject context)
        {
            // Wait 5 seconds.
            return false;
        }
        [LocationHandler(201.5262f, -2.970733f, -56.57514f, 5f)]
        public async Task<bool> DragonBreathHandler2(GameObject context)
        {
            // Wait 5 seconds.
            return false;
        }

        // Rangda Tactics
        //
        // Electric Predation => Frontal-cone attack. Tanks should keep the boss facing away.
        // Prey debuff (Aura: 420) => Run to the nearest statue to transfer it.
        // --- Blackened Statue => BattleCharacter => 3454
        //
        // Gyascutus Tactics
        //
        // Adds:
        // --- Mustard Gas (3456) => At intervals, bombs will spawn.
        // ------ Allow these to eat two gas clouds to manage how many Bloated (Aura: 702) stacks the add has before killing it.
        //
        // Nidhogg Tactics
        //
        // The Scarlet Whisper => Frontal-cone attack. Tanks should keep the boss facing away.
        // Draconian Light => Cast by Estinien to shield everyone from Massacre.
        // --- Kill all adds, then stand next to NPC until Draconian Light buff fades.
        // ------ Estinien Wyrmblood => BattleCharacter => 3465

    }
}
