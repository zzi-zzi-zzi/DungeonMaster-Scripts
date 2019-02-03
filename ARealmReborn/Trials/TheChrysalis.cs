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
    public class TheChrysalis : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20029;
        public override string Name => @"The Chrysalis";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Nabriales", 3287, 0, 28.96f, new Vector3(0.00f, 0.00f, -10.00f), () => DMDirectorManager.Instance.InstanceEnded)
            };
        }

        public override void OnEnter()
        {
            // Extension
            AddAvoidObject<BattleCharacter>(24, 3294);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Aetherial Tear
                if (obj.Object.NpcId == 3293)
                    obj.Score += 1000;
                // Shadow Sprite
                if (obj.Object.NpcId == 3292)
                    obj.Score += 1000;
            }
        }

        // Nabriales Tactics
        //
        // Physical Vulnerability Up (Aura: 657) => Go to "black" spheres if you do not have this debuff.
        // --- Dark Aether Isten => BattleCharacter => 3289
        // Magic Vulnerability Up (Aura: 658) => Go to "red" spheres if you do not have this debuff.
        // --- Dark Aether Salas => BattleCharacter => 3291
        // Comet => Tanks should stack on "circles" to absorb the Comet impact damage, then move to the next one(s).

    }
}
