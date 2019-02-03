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
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.ARealmReborn.Trials
{
    public class BattleOnTheBigBridge : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20021;
        public override string Name => @"Battle on the Big Bridge";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Gilgamesh #1", 2665, 0, 25.24f, new Vector3(-49.24f, 2.00f, -0.11f), () => GameObjectManager.GetObjectByNPCId(2665) == null),
                new Boss("Gilgamesh #2", 2665, 1, 24.36f, new Vector3(132.18f, -5.00f, -0.43f), () => DMDirectorManager.Instance.InstanceEnded)
            };
        }

        private uint[] _grovel = new uint[] { 2117 };

        public override void OnEnter()
        {
            // Grovel
            AddAvoidObject<BattleCharacter>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _grovel.Contains(i.CastingSpellId)),
                10,
                x => x.NpcId == 2665,
                x => x.Location
                );
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Crossing Hippocerf
                if (obj.Object.NpcId == 2669)
                    obj.Score += 1000;
            }
        }

        // Gilgamesh Tactics
        //
        // Toad (Aura: 439) => CC'd for 20 seconds. Move around area in clockwise motion to survive.
        // Minimum (Aura: 438) => Player is shrunk and is Heavy for 20 seconds. Chased by boss, so move around area in clockwise motion.
        // Confused (Aura: 11) => Will attack nearby players and run through AoE attacks. Lasts for X seconds or until healed to 100%.
        // Giga Jump => Targets random player with "target" icon. Avoid all players by 5y(?).
        // Whirlwind (XXXX) => Avoid by 5y(?).

    }
}
