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

namespace DungeonMasterScripts.ARealmReborn.Trials
{
    public class TheNavel : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20002;
        public override string Name => @"The Navel";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Titan", 1801, 0, 40.28f, new Vector3(0.00f, 0.00f, -16.00f), () => DMDirectorManager.Instance.InstanceEnded)
            };
        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Granite Gaol (Titan)
                if (obj.Object.NpcId == 1804)
                    obj.Score += 2000;
                // Titan's Heart (Titan)
                if (obj.Object.NpcId == 1802)
                    obj.Score += 1000;
            }
        }

        [EncounterHandler(1801, "Titan", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> TitanHandler(BattleCharacter c)
        {
            if (!c.HasTarget && c.CurrentHealthPercent > 50f)
            {
                if (Core.Me.Location.Distance(new Vector3(1.102976f, 3.576279E-07f, -0.1875899f)) < 10f)
                {
                    // Use random point @ 19y radius instead of a specific point.
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(new Vector3(-1.27678f, -0.002887249f, 19.67578f), "Geocrush"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }
            if (!c.HasTarget && c.CurrentHealthPercent < 50f)
            {
                if (Core.Me.Location.Distance(new Vector3(1.102976f, 3.576279E-07f, -0.1875899f)) < 10f)
                {
                    // Use random point @ 17y radius instead of a specific point.
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(new Vector3(0.1695436f, -0.002887011f, -18.18046f), "Geocrush"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }

            return false;
        }

        // Titan Tactics
        //
        // Fix: Does not move to Geocrush "safe spot" when Titan jumps into the air.
        // Leap (Geocrush) => Move as close as possible to edge of safe area.
        // --- 1.102976, 3.576279E-07, -0.1875899 (approx. center of platform)
        // Weight of the Land => Circle AoE on random players. Move out ASAP!

    }
}