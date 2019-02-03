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
    public class TheStepsOfFaith : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20028;
        public override string Name => @"The Steps of Faith";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Vishap", 3330, 0, 50f, new Vector3(-6.02f, 0.02f, 310.92f), () => ScriptHelper.IsTodoChecked(2))
            };
        }

        public override void OnEnter()
        {
            // Powder Keg
            AddAvoidObject<BattleCharacter>(10, 3313);
        }

        public override void OnExit()
        {

        }

        // Dragonkiller - NpcId2004760 - TODOStep: 1 - TODOValue (Before Pickup): 40 /  100\n
        [ObjectHandler(2004760, ObjectRange = 90)]
        public async Task<bool> DragonkillerHandler(GameObject context)
        {
            // if (ScriptHelper.InCombat())
                // return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);
        }
        // Snare - NpcId2005115 - TODOStep: 1 - TODOValue (Before Pickup): 65 /  100
        // Snare - NpcId2005116 - TODOStep: 1 - TODOValue (Before Pickup): 65 /  100
        // Snare - NpcId2005117 - TODOStep: 1 - TODOValue (Before Pickup): 90 /  100
        // Snare - NpcId2005118 - TODOStep: 1 - TODOValue (Before Pickup): 90 /  100\n
        [ObjectHandler(2005115, ObjectRange = 90)]
        [ObjectHandler(2005116, ObjectRange = 90)]
        [ObjectHandler(2005117, ObjectRange = 90)]
        [ObjectHandler(2005118, ObjectRange = 90)]
        public async Task<bool> SnareHandler(GameObject context)
        {
            // if (ScriptHelper.InCombat())
                // return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);
        }
        // Dragonkiller - NpcId2004761 - TODOStep: 1 - TODOValue (Before Pickup): 65 /  100
        // Dragonkiller - NpcId2004762 - TODOStep: 1 - TODOValue (Before Pickup): 90 /  100\n
        [ObjectHandler(2004761, ObjectRange = 90)]
        [ObjectHandler(2004762, ObjectRange = 90)]
        public async Task<bool> DragonkillerHandler2(GameObject context)
        {
            // if (ScriptHelper.InCombat())
                // return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);
        }

        // Vishap Tactics
        //
        // Underfoot => Avoid boss's feet.
        // Bertha (2168) => Cannons to be used (ground targeting) to help damage adds/boss/kegs.
        // --- Focus Bertha attacks on boss.
        // --- Sidewise Slice (telegraphed attack by Vishap) => Exit/move away from cannons.
        // Dragonkiller => Shoots a harpoon at boss from towers. First one is free. Second and third require boss to be snared.
        // Landwaster => Use both Snares to interrupt cast and allow Dragonkiller to not be avoided by boss.

    }
}
