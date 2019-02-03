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
    public class TheBowlOfEmbersHard : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20004;
        public override string Name => @"The Bowl of Embers (Hard)";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Ifrit", 1185, 0, 30.66f, new Vector3(15.00f, 0.00f, 0.00f), () => DMDirectorManager.Instance.InstanceEnded)
            };
        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Infernal Nail
                if (obj.Object.NpcId == 1186)
                    obj.Score += 1000;
            }
        }

        // Ifrit Tactics
        //
        // Tank keeps the boss facing away from the group.
        // Eruption => Cast on random player. Avoid by 10y(?). Interruptible?
        // Crimson Cyclone => Dash AoE attack. Avoid line in front of all Ifrit's once spawned @ edge of area.
        // Radiant Plume => SideStep? All the same spell ID, or can we discern between them?
        // --- These rings can appear around the perimeter of the arena.
        // --- These rings can appear in the center of the arena.
        // --- Both rings types can appear one after the other.
        // --- These rings can appear and cover everything outside of melee range.

    }
}
