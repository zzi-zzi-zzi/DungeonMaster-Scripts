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
    public class TheHowlingEyeHard : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20006;
        public override string Name => @"The Howling Eye (Hard)";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Garuda", 1644, 0, 35.79f, new Vector3(1.14f, -2.03f, -18.00f), () => DMDirectorManager.Instance.InstanceEnded)
            };
        }

        private uint[] _mistralSong = new uint[] { 11150, 11083, 11074, 6620, 1816, 1761, 1517, 1409, 1390, 1383, 667, 660 };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Satin Plume
                if (obj.Object.NpcId == 1648)
                    obj.Score += 4000;
                // Razor Plume
                if (obj.Object.NpcId == 1647)
                    obj.Score += 3000;
                // Chirada
                if (obj.Object.NpcId == 1646)
                    obj.Score += 2000;
                // Suparna
                if (obj.Object.NpcId == 1645)
                    obj.Score += 1000;
            }
        }

        // Garuda Tactics
        //
        // Adds:
        // --- Focus "green beam" add first. Changes between Chirada and Suparna?
        // Slipstream => Frontal column spell => Tank move to side of boss, then back to previous position when cast completes.
        // Mistral Song => Garuda appears on one side of the room, firing a blast of wind through the area. Hide behind stone pillars to avoid the damage.
        // --- Monolith EObjs: 2000646, 2000647, 2000648, 2000649
        // --- Garuda in center of room: -0.09651054, -1.793348, -0.05080796
        // ------ NESafeSpot => 7.58779, -1.839066, -7.236983
        // ------ NWSafeSpot => -8.068699, -1.820575, -8.139473
        // ------ SESafeSpot => 9.549785, -1.870029, 8.102727
        // ------ SWSafeSpot => -6.462915, -1.858154, 6.506263
        // --- Garuda on North side: 0.02851587, -2.160737, -21.9084
        // ------ NESafeSpot => 6.968157, -1.836251, -2.964691
        // ------ NWSafeSpot => -8.037441, -1.846786, -4.139885
        // ------ SESafeSpot => 8.941764, -1.824921, 9.412521
        // ------ SWSafeSpot => -5.717588, -1.851137, 7.869577
        // --- Garuda on South side: -0.07837719, -2.182315, 21.704
        // ------ NESafeSpot => 6.505735, -1.864202, -8.523278
        // ------ NWSafeSpot => -7.260379, -1.831279, -9.38976
        // ------ SESafeSpot => 9.032708, -1.811108, 3.795121
        // ------ SWSafeSpot => -5.824129, -1.853106, 2.123384
        // --- In Phase 2 (< 40% HP), avoid Mistral Song by moving to the side or behind Garuda.
        // Ariel Blast => Move to center safe spot.
        // Reckoning => Stack in center of area for healing.
        // --- Still a thing to worry about?
        // Phase 2 => Avoid tornadoes (area to fight becomes smaller).
        // Phase 2 => Avoid purple wind (area to fight becomes smaller).

    }
}
