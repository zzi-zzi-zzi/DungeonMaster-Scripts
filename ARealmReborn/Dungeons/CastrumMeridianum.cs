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

namespace DungeonMasterScripts.ARealmReborn.Dungeons
{
    public class CastrumMeridianum : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 15;
        public override string Name => @"Castrum Meridianum";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("The Black Eft", 557, 0, 22.48f, new Vector3(11.78f, 70.17f, -38.64f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Searchlight Terminal", 2000567, 1, 25.04f, new Vector3(-71.90f, 74.74f, 151.47f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Magitek Vanguard", 269, 2, 28.94f, new Vector3(-22.31f, 74.00f, 102.63f), () => DMDirectorManager.Instance.GetTodoArgs(2).Item1 > 0),
                new Boss("Magitek Vanguard", 269, 3, 25.24f, new Vector3(46.59f, 66.69f, 85.31f), () => DMDirectorManager.Instance.GetTodoArgs(2).Item1 > 1),
                new Boss("Unstable Paneling", 2000571, 4, 0.57f, new Vector3(95.74f, 64.96f, 103.15f), () => DMDirectorManager.Instance.GetTodoArgs(3).Item1 > 0),
                new Boss("Unstable Paneling", 2000574, 5, 0.39f, new Vector3(104.31f, 64.94f, 67.24f), () => DMDirectorManager.Instance.GetTodoArgs(3).Item1 > 1),
                new Boss("Magitek Vanguard F-1", 2116, 6, 26.79f, new Vector3(-10.69f, 69.66f, 26.93f), () => DMDirectorManager.Instance.GetTodoArgs(4).Item1 > 0),
                new Boss("Searchlight Terminal", 2000568, 7, 0.93f, new Vector3(24.72f, 77.70f, -154.37f), () => ScriptHelper.IsTodoChecked(6)),
                new Boss("Magitek Colossus Rubricatus", 2117, 8, 40.19f, new Vector3(-90.86f, 85.18f, -260.67f), () => DMDirectorManager.Instance.GetTodoArgs(4).Item1 > 1),
                new Boss("Searchlight Terminal", 2000580, 9, 0.94f, new Vector3(-26.23f, 79.75f, -42.17f), () => DMDirectorManager.Instance.GetTodoArgs(7).Item1 > 0),
                new Boss("Searchlight Terminal", 2000581, 10, 4.05f, new Vector3(-23.85f, 79.75f, -15.35f), () => DMDirectorManager.Instance.GetTodoArgs(7).Item1 > 1),
                new Boss("Livia sas Junius", 2118, 11, 29.13f, new Vector3(-102.89f, 70.35f, -31.60f), () => DMDirectorManager.Instance.InstanceEnded)
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First door after Magitek Vanguard F-I
            new OffMeshConnection(new Vector3(20.47f, 70.28f, 2.85f), new Vector3(19.55f, 70.23f, -2.77f), ConnectionMode.Bidirectional),
            // Second door after Magitek Vanguard F-I
            new OffMeshConnection(new Vector3(6.31f, 70.17f, -72.99f), new Vector3(6.35f, 70.33f, -78.49f), ConnectionMode.Bidirectional),
            // Door after Magitek Colossus Rubricatus
            new OffMeshConnection(new Vector3(-13.37f, 81.79f, -199.78f), new Vector3(-13.29f, 81.09f, -192.34f), ConnectionMode.Bidirectional),
            // Door before Livia sas Junius
            new OffMeshConnection(new Vector3(-53.21f, 68.65f, -85.66f), new Vector3(-54.79f, 68.56f, -79.21f), ConnectionMode.Bidirectional)
        };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // 8th Cohort Medicus (Magitek Colossus Rubricatus)
                if (obj.Object.NpcId == 2111)
                    obj.Score += 3000;
                // 8th Cohort Signifer (The Black Eft & Magitek Vanguard F-I)
                if (obj.Object.NpcId == 2109)
                    obj.Score += 3000;
                // 7th Cohort Secutor (Livia sas Junius)
                if (obj.Object.NpcId == 2108)
                    obj.Score += 2000;
                // 8th Cohort Laquearius (The Black Eft & Magitek Colossus Rubricatus)
                if (obj.Object.NpcId == 2106)
                    obj.Score += 2000;
                // Imperial War Hound (The Black Eft)
                if (obj.Object.NpcId == 2113)
                    obj.Score += 1000;
                // 8th Cohort Sagittarius (Magitek Vanguard F-I)
                if (obj.Object.NpcId == 2110)
                    obj.Score += 1000;
                // 7th Cohort Eques (Livia sas Junius)
                if (obj.Object.NpcId == 2107)
                    obj.Score += 1000;
            }
        }

        // Searchlight Terminal - NpcId2000873 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  2
        // Searchlight Terminal - NpcId2000564 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  2
        // Searchlight Terminal - NpcId2000567 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Searchlight Terminal - NpcId2000568 - TODOStep: 4 - TODOValue (Before Pickup): 1 /  3
        // Searchlight Terminal - NpcId2000580 - TODOStep: 4 - TODOValue (Before Pickup): 2 /  3
        // Searchlight Terminal - NpcId2000581 - TODOStep: 4 - TODOValue (Before Pickup): 2 /  3\n
        [ObjectHandler(2000873, ObjectRange = 25)]
		[ObjectHandler(2000564, ObjectRange = 25)]
        [ObjectHandler(2000567, ObjectRange = 25)]
        [ObjectHandler(2000568, ObjectRange = 25)]
        [ObjectHandler(2000580, ObjectRange = 25)]
        [ObjectHandler(2000581, ObjectRange = 25)]
        public async Task<bool> SearchlightTerminalHandler(GameObject context)
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
        // Disposal Chute - NpcId2000597 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000597, ObjectRange = 25)]
        public async Task<bool> DisposalChuteHandler(GameObject context)
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
        // Incendiary #37 - NpcId2001113 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  2
        // Incendiary #37 - NpcId2000570 - TODOStep: 2 - TODOValue (Before Pickup): 1 /  2\n
		[ObjectHandler(2001113, ObjectRange = 25)]
		[ObjectHandler(2000570, ObjectRange = 25)]
        public async Task<bool> Incendiary37Handler(GameObject context)
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
        // Unstable Paneling - NpcId2000571 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  2
        // Unstable Paneling - NpcId2000574 - TODOStep: 3 - TODOValue (Before Pickup): 1 /  2\n
		[ObjectHandler(2000571, ObjectRange = 25)]
		[ObjectHandler(2000574, ObjectRange = 25)]
        public async Task<bool> UnstablePanelingHandler(GameObject context)
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
        // Imperial Identification Key - NpcId2000869 - TODOStep: 4 - TODOValue (Before Pickup): 1 /  3
        // Imperial Identification Key - NpcId2000870 - TODOStep: 4 - TODOValue (Before Pickup): 2 /  3\n
        [ObjectHandler(2000869, ObjectRange = 25)]
        [ObjectHandler(2000870, ObjectRange = 25)]
        public async Task<bool> ImperialIdentificationKeyHandler(GameObject context)
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
        // Magitek Terminal - NpcId2000583 - TODOStep: 4 - TODOValue (Before Pickup): 2 /  3
        // Magitek Terminal - NpcId2000582 - TODOStep: 4 - TODOValue (Before Pickup): 2 /  3\n
		[ObjectHandler(2000583, ObjectRange = 25)]
		[ObjectHandler(2000582, ObjectRange = 25)]
        public async Task<bool> MagitekTerminalHandler(GameObject context)
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
        // Mark XLIII Anti-aircraft Cannon - NpcId2000601 - TODOStep: 4 - TODOValue (Before Pickup): 2 /  3
        // Mark XLIII Anti-aircraft Cannon - NpcId2000595 - TODOStep: 4 - TODOValue (Before Pickup): 2 /  3
        // Mark XLIII Anti-aircraft Cannon - NpcId2000600 - TODOStep: 4 - TODOValue (Before Pickup): 2 /  3\n
		[ObjectHandler(2000601, ObjectRange = 25)]
		[ObjectHandler(2000595, ObjectRange = 25)]
		[ObjectHandler(2000600, ObjectRange = 25)]
        public async Task<bool> MarkXLIIIAntiaircraftCannonHandler(GameObject context)
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

        // TO DO
        //
        // Use Disposal Chute after The Black Eft dies.
        // --- Range=15; need to find reliable way to use it if tanking/solo. (557 != IsVisible?)
        // Use Mark XLIII Anti-aircraft Cannons to bring down the airship. (Tanks only?)
        //
        // The Black Eft Tactics
        //
        // Photon Stream => Frontal attack. Tanks should keep the boss facing away.
        //
        // Livia sas Junius Tactics
        //
        // Use Magitek Missiles to load the cannons.
        // --- Right Side (facing boss):
        // ------ Name:Magitek Missile 0x19987F0D770, Type:ff14bot.Objects.EventObject, ID:2000663, Obj:1073755215
        // --- Left Side (facing boss):
        // ------ Name:Magitek Missile 0x19987F0A570, Type:ff14bot.Objects.EventObject, ID:2000662, Obj:1073755216
        // Use Mark XLIII Artillery Cannons (2115) when loaded with missiles, then ground target (Spell: Cannonfire) attack on boss.

    }
}
