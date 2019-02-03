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
    public class ARelicRebornTheChimera : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20019;
        public override string Name => @"A Relic Reborn: the Chimera";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Dhorme Chimera", 2162, 0, 22.17f, new Vector3(547.41f, 348.38f, -748.11f), () => DMDirectorManager.Instance.InstanceEnded)
            };
        }

        private uint[] _theDragonsVoice = new uint[] { 12432, 10965, 7079, 3344, 2604, 2145, 1442, 1338, 1105 };
        private uint[] _theRamsVoice = new uint[] { 12431, 7078, 3343, 2603, 2144, 1285, 1104 };

        public override void OnEnter()
        {
            // The Ram's Keeper
            AddAvoidObject<EventObject>(10, 2002332);
            // Cacophony
            AddAvoidObject<BattleCharacter>(10, 1848);
            // The Ram's Voice
            AddAvoidObject<BattleCharacter>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _theRamsVoice.Contains(i.CastingSpellId)),
                10,
                x => x.NpcId == 2162,
                x => x.Location
                );
        }

        public override void OnExit()
        {

        }

        [EncounterHandler(2162, "Dhorme Chimera", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> DhormeChimeraHandler(BattleCharacter c)
        {
            if (c.IsCasting && _theDragonsVoice.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 5f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Dhorme Chimera"), 8f, true);
                    return true;
                }
            }

            return false;
        }

        // Dhorme Chimera Tactics
        //
        // Lion's Breath => Frontal cone AoE attack. Tank keeps the boss facing away from the group.
        // The Dragon's Breath => SideStep? Frontal cone AoE attack. Move to the side of the boss, or behind it.
        // The Ram's Breath => SideStep? Frontal cone AoE attack. Move to the side of the boss, or behind it.
        // The Scorpion's Sting => SideStep? Triggered by being behind the boss. All players avoid being behind the boss.

    }
}
