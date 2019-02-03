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
    public class TheHowlingEye : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20003;
        public override string Name => @"The Howling Eye";
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
                // Razor Plume (Garuda)
                if (obj.Object.NpcId == 1647)
                    obj.Score += 1000;
            }
        }

        // Garuda Tactics
        //
        // Slipstream => Frontal column spell => Tank move to side of boss, then back to previous position when cast completes.
        // Mistral Song => Garuda appears on one side of the room, firing a blast of wind through the area. Hide behind stone pillars to avoid the damage.
        //
        // --- Monolith EObjs: , , , 
        //
        // --- Garuda in center of room: -0.09651054, -1.793348, -0.05080796
        //
        // --- NESafeSpot => 7.58779, -1.839066, -7.236983
        // --- NWSafeSpot => -8.068699, -1.820575, -8.139473
        // --- SESafeSpot => 9.549785, -1.870029, 8.102727
        // --- SWSafeSpot => -6.462915, -1.858154, 6.506263
        //
        // --- Garuda on North side: 0.02851587, -2.160737, -21.9084
        //
        // --- NESafeSpot 
        // --- NWSafeSpot 
        // --- SESafeSpot 
        // --- SWSafeSpot 
        //
        // --- Garuda on South side: -0.07837719, -2.182315, 21.704
        //
        // --- NESafeSpot 
        // --- NWSafeSpot 
        // --- SESafeSpot 
        // --- SWSafeSpot 
        //
        // --- In Phase 2 (< 40% HP), avoid Mistral Song by moving to the side or behind Garuda.
        // Ariel Blast => Move to center safe spot.
        // Phase 2 => Avoid purple wind (area to fight becomes smaller).

    }
}
