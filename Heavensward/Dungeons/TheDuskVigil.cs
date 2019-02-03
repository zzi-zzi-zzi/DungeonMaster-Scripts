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

namespace DungeonMasterScripts.Heavensward.Dungeons
{
    public class TheDuskVigil : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 36;
        public override string Name => @"The Dusk Vigil";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Towering Oliphant", 3405, 0, 29.61f, new Vector3(0.38f, -0.05f, -9.91f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Ser Yuhelmeric", 3406, 1, 28.73f, new Vector3(191.39f, -8.05f, -127.92f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Opinicus", 3409, 2, 23.70f, new Vector3(-69.30f, 32.06f, -394.56f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First avalanche before Towering Oliphaunt
            new OffMeshConnection(new Vector3(-32.82f, -15.87f, 168.50f), new Vector3(-1.94f, -12.35f, 167.88f), ConnectionMode.Bidirectional),
            // Second avalanche before Towering Oliphaunt
            new OffMeshConnection(new Vector3(31.84f, -8.10f, 97.55f), new Vector3(31.33f, -2.47f, 67.78f), ConnectionMode.Bidirectional),
            // Door before Towering Oliphaunt
            new OffMeshConnection(new Vector3(-0.29f, 0.65f, 32.72f), new Vector3(-0.38f, -0.05f, 27.89f), ConnectionMode.Bidirectional),
            // First door before Ser Yuhelmeric
            new OffMeshConnection(new Vector3(157.98f, -8.00f, -24.03f), new Vector3(162.48f, -8.05f, -24.04f), ConnectionMode.Bidirectional),
            // Second door before Ser Yuhelmeric
            new OffMeshConnection(new Vector3(191.98f, -8.05f, -70.60f), new Vector3(191.96f, -8.05f, -74.30f), ConnectionMode.Bidirectional),
            // Door after Ser Yuhelmeric
            new OffMeshConnection(new Vector3(192.05f, -8.05f, -153.94f), new Vector3(192.03f, -8.05f, -159.26f), ConnectionMode.Bidirectional),
            // Transition line after Ser Yuhelmeric
            new OffMeshConnection(new Vector3(191.99f, -7.93f, -201.75f), new Vector3(192.86f, 16.00f, -224.87f), ConnectionMode.Bidirectional),
            // First frozen door before Opinicus
            new OffMeshConnection(new Vector3(96.08f, 24.00f, -234.14f), new Vector3(96.03f, 24.00f, -223.34f), ConnectionMode.Bidirectional),
            // Second frozen door before Opinicus
            new OffMeshConnection(new Vector3(22.22f, 28.00f, -212.83f), new Vector3(22.44f, 28.00f, -222.93f), ConnectionMode.Bidirectional),
            // Third frozen door before Opinicus
            new OffMeshConnection(new Vector3(-31.04f, 32.00f, -285.94f), new Vector3(-41.43f, 32.00f, -286.04f), ConnectionMode.Bidirectional),
            // Door before Opinicus
            new OffMeshConnection(new Vector3(-69.50f, 32.00f, -291.32f), new Vector3(-69.47f, 32.00f, -296.02f), ConnectionMode.Bidirectional)
        };

        private uint[] _deathSpiral = new uint[] { 7082, 3680 };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Frozen Chirurgeon (Ser Yuhelmeric)
                if (obj.Object.NpcId == 3408)
                    obj.Score += 2000;
                // Frozen Knight (Ser Yuhelmeric)
                if (obj.Object.NpcId == 3407)
                    obj.Score += 1000;
                // Snoll
                if (obj.Object.NpcId == 3389)
                    obj.Score += 1000;
            }
        }

        // Cold Steel Lever - NpcId2005172 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2005172, ObjectRange = 25)]
        public async Task<bool> ColdSteelLeverHandler(GameObject context)
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
        // Damaged Winch - NpcId2005173 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2005173, ObjectRange = 25)]
        public async Task<bool> DamagedWinchHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2001569))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Barracks Key - NpcId2005175 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2005175, ObjectRange = 25)]
        public async Task<bool> BarracksKeyHandler(GameObject context)
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
        // Barracks Door - NpcId2005177 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2005177, ObjectRange = 25)]
        public async Task<bool> BarracksDoorHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2001570))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Lord Commander's Key - NpcId2005176 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2005176, ObjectRange = 25)]
        public async Task<bool> LordCommandersKeyHandler(GameObject context)
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
        // Lord Commander's Seat - NpcId2005178 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2005178, ObjectRange = 25)]
        public async Task<bool> LordCommandersSeatHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.HasKeyItem(2001571))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        [EncounterHandler(3406, "Ser Yuhelmeric", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> SerYuhelmericHandler(BattleCharacter c)
        {
            if (c.IsCasting && _deathSpiral.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 5f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Ser Yuhelmeric"), 5f, true);
                    return true;
                }
            }

            return false;
        }
        // Chapel Door - NpcId2005357 - TODOStep: 5 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2005357, ObjectRange = 25)]
        public async Task<bool> ChapelDoorHandler(GameObject context)
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

        // Towering Oliphaunt Tactics
        // 
        // Trunk Tawse => Frontal-cone attack. Tanks should keep the boss facing away.
        //
        // Opinicus Tactics
        //
        // Whirling Gaol => Hide behind a crumbled pile to LoS the center of the platform (not the boss).
        // --- Whirling Gaol => BattleCharacter => 4381
        // Winds of Winter => Hide behind a crumbled pile to LoS the boss.
        // --- Rubble => BattleCharacter => 3410

    }
}
