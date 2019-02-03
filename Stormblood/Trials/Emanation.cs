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

namespace DungeonMasterScripts.Stormblood.Trials
{
    public class Emanation : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20048;
        public override string Name => @"Emanation";
        public override Version Version => new Version(0, 7, 1, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Lakshmi", 6385, 0, 30f, new Vector3(0.13f, 0.06f, -0.16f), () => ScriptHelper.IsTodoChecked(0)),
            };
        }

        private uint[] _devineDenial = new uint[] { 9990, 9569, 9568, 9349, 8521 };
        private uint[] _innerDemons = new uint[] { 9613, 8717, 6221, 2450 };

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Dreaming Kshatriya
                if (obj.Object.NpcId == 6386)
                    obj.Score += 1000;
            }
        }

        [EncounterHandler(6385, "Lakshmi", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> LakshmiHandler(BattleCharacter c)
        {
            //var dreamingKshatriyaId = GameObjectManager.GetObjectByNPCId(6386);

            if (c.IsCasting && _devineDenial.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 3f)
                {
                    // TO DO: Use duty action button.
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Lakshmi"), 3f, true);
                    return true;
                }
            }
            //while (dreamingKshatriyaId.IsCasting && _innerDemons.Contains(dreamingKshatriyaId.CastingSpellId))
            //{
            //    ScriptHelper.LookAway(c);
            //    await Coroutine.Yield();
            //}

            return false;
        }

        // Lakshmi Tactics
        //
        // Vril (6690) => Duty action resource you collect by running into them.
        // --- Maintain 1 stack if Vril object is visible.
        // Hand of Grace => Two players targeted with lotus markers. Spread from all players.
        // Hand of Beauty => Two players targeted with lotus markers. Spread from all players.
        // "Ultimate" attack => Use duty action button to avoid death.
        // Devine Denial => Use duty action button and be in center of platform.
        // Path of Light => Frontal cone attack where off-tank (the target) needs to go to one side of the boss to not cleave the raid.
        // Call of Light => Stack up to soak AoE damage.
        // Alluring Arm => Hand of Grace + Hand of Beauty at the same time.

    }
}
