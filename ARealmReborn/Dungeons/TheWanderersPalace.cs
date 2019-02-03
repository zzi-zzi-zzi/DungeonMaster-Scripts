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
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;
using ff14bot.Helpers;

namespace DungeonMasterScripts.ARealmReborn.Dungeons
{
    public class TheWanderersPalace : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 10;
        public override string Name => @"The Wanderer's Palace";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Keeper of Halidom", 1548, 0, 33.33f, new Vector3(124.99f, -12.00f, 98.27f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("Nymian Device", 2001123, 1, 3f, new Vector3(118.00f, -9.00f, 65.38f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Rusted Nymian Device", 2001126, 2, 3f, new Vector3(42.98f, 0.99f, -58.34f), () => DMDirectorManager.Instance.GetTodoArgs(3).Item1 > 0 || ScriptHelper.IsTodoChecked(3)),
                new Boss("Nymian Device", 2001127, 3, 3f, new Vector3(53.61f, 8.95f, -96.00f), () => DMDirectorManager.Instance.GetTodoArgs(3).Item1 > 1 || ScriptHelper.IsTodoChecked(3)),
                new Boss("Rusted Nymian Device", 2001129, 4, 3f, new Vector3(36.00f, 0.99f, -150.59f), () => DMDirectorManager.Instance.GetTodoArgs(3).Item1 > 2 || ScriptHelper.IsTodoChecked(3)),
                new Boss("Rusted Nymian Device", 2001128, 5, 3f, new Vector3(121.63f, 0.93f, -146.01f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Giant Bavarois", 1549, 6, 32.54f, new Vector3(43.26f, -0.16f, -243.02f), () => ScriptHelper.IsTodoChecked(4)),
                new Boss("Tonberry King", 1547, 7, 36.75f, new Vector3(73.00f, 6.00f, -448.69f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Door before Keeper of Halidom
            new OffMeshConnection(new Vector3(124.98f, -14.00f, 184.36f), new Vector3(124.96f, -14.05f, 178.89f), ConnectionMode.Bidirectional),
            // Door after Keeper of Halidom
            new OffMeshConnection(new Vector3(125.04f, -9.46f, 64.83f), new Vector3(125.03f, -9.46f, 59.93f), ConnectionMode.Bidirectional),
            // Door near Rusted Nymian Devices
            new OffMeshConnection(new Vector3(42.91f, -0.16f, -27.26f), new Vector3(42.94f, 0.00f, -33.29f), ConnectionMode.Bidirectional),
            // Door before Giant Bavarois
            new OffMeshConnection(new Vector3(42.93f, 0.54f, -150.90f), new Vector3(42.90f, 0.54f, -155.79f), ConnectionMode.Bidirectional),
            // Door before Tonberry King
            new OffMeshConnection(new Vector3(73.05f, 10.39f, -379.41f), new Vector3(73.08f, 10.39f, -384.71f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Tonberry Slasher
            AddAvoidObject<BattleCharacter>(10, 2180);
            // Tonberry Stalker
            AddAvoidObject<BattleCharacter>(10, 1556);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Blue Bavarois (Giant Bavarois)
                if (obj.Object.NpcId == 1555)
                    obj.Score += 4000;
                // Purple Bavarois (Giant Bavarois)
                if (obj.Object.NpcId == 1554)
                    obj.Score += 3000;
                // White Bavarois (Giant Bavarois)
                if (obj.Object.NpcId == 1551)
                    obj.Score += 2000;
                // Green Bavarois (Giant Bavarois)
                if (obj.Object.NpcId == 1552)
                    obj.Score += 1000;
                // Tonberry
                if (obj.Object.NpcId == 35)
                    obj.Score += 1000;
            }
        }

        // Nymian Device - NpcId2001123 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Nymian Device - NpcId2001127 - TODOStep: 3 - TODOValue (Before Pickup): 1 /  4\n
        [ObjectHandler(2001123, ObjectRange = 25)]
        [ObjectHandler(2001127, ObjectRange = 25)]
        public async Task<bool> NymianDeviceHandler(GameObject context)
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
        // Lantern Oil - NpcId2002789 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  4\n
        [ObjectHandler(2002789, ObjectRange = 35)]
        public async Task<bool> LanternOilHandler(GameObject context)
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
        // Rusted Nymian Device - NpcId2001124 - TODOStep: 2 - TODOValue (Before Pickup): 3 /  0
        // Rusted Nymian Device - NpcId2001125 - TODOStep: 2 - TODOValue (Before Pickup): 3 /  0
        // Rusted Nymian Device - NpcId2001126 - TODOStep: 2 - TODOValue (Before Pickup): 1 /  0
        // Rusted Nymian Device - NpcId2001129 - TODOStep: 2 - TODOValue (Before Pickup): 2 /  0
        // Rusted Nymian Device - NpcId2001128 - TODOStep: 2 - TODOValue (Before Pickup): 1 /  0
        // Rusted Nymian Device - NpcId2001130 - TODOStep: 2 - TODOValue (Before Pickup): 2 /  0
        // Rusted Nymian Device - NpcId2001131 - TODOStep: 2 - TODOValue (Before Pickup): 2 /  0\n
        [ObjectHandler(2001124, ObjectRange = 25)]
        [ObjectHandler(2001125, ObjectRange = 25)]
        [ObjectHandler(2001126, ObjectRange = 25)]
        [ObjectHandler(2001129, ObjectRange = 25)]
        [ObjectHandler(2001128, ObjectRange = 25)]
        [ObjectHandler(2001130, ObjectRange = 25)]
        [ObjectHandler(2001131, ObjectRange = 25)]
        public async Task<bool> RustedNymianDeviceHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible && ScriptHelper.HasKeyItem(2001071))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }

        // The Wanderer's Palace
        //
        // Giant Bavarois Tactics
        //
        // Fixate(?) => If targeted, kite the boss around the area.
        //
        // Tonberry King Tactics
        //
        // Everybody's Grudge => Area-wide AoE attack. Keep Rancor buff stacks "manageable" by killing adds only when too many are alive.
        // --- Kill adds while boss has < 4 stacks of Rancor.

    }
}
