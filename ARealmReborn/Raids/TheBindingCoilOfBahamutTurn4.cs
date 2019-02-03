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

namespace DungeonMasterScripts.ARealmReborn.Raids
{
    public class TheBindingCoilOfBahamutTurn4 : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 30005;
        public override string Name => @"The Binding Coil of Bahamut - Turn 4";
        public override Version Version => new Version(0, 9, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Drive Cylinder", 0, 0, 0.00f, new Vector3(0.03f, -0.05f, 0.00f), () => DMDirectorManager.Instance.InstanceEnded)
            };
        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Spinner-rook (Drive Cylinder)
                if (obj.Object.NpcId == 1476)
                    obj.Score += 2000;
                // Clockwork Bug (Drive Cylinder)
                if (obj.Object.NpcId == 1474)
                    obj.Score += 1000;
            }
        }

        // Silent Terminal - NpcId2000633 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000633, ObjectRange = 25)]
        public async Task<bool> SilentTerminalHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            return await ScriptHelper.ObjectInteraction(context);
        }

        // Drive Cylinder Tactics
        //
        // Clockwork Soldier (1475) => Spell DPS focus on these.
        // Clockwork Knight (1477) => Physical DPS focus on these.
        // Pox => Spinner-rook casts this untelegraphed in front of them. Add logic to run to point behind them when they start casting.

    }
}
