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
    public class Sastasha : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 4;
        public override string Name => @"Sastasha";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Chopper", 1204, 0, 14.94f, new Vector3(68.71f, 31.26f, -44.24f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Captain Madison #1", 1382, 1, 37.64f, new Vector3(-31.51f, 23.74f, 58.97f), () => ScriptHelper.IsTodoChecked(2)),
                new Boss("Shallowtail Reaver", 342, 2, 33.62f, new Vector3(-95.03f, 20.01f, 194.59f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Denn the Orcatoothed", 1206, 3, 46.46f, new Vector3(-334.29f, 5.58f, 318.57f), () => ScriptHelper.IsTodoChecked(4))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Door after Chopper
            new OffMeshConnection(new Vector3(61.72f, 32.47f, -36.04f), new Vector3(56.05f, 32.28f, -33.55f), ConnectionMode.Bidirectional),
            // Door after Captain Madison
            new OffMeshConnection(new Vector3(-32.11f, 23.80f, 59.11f), new Vector3(-39.19f, 24.04f, 62.63f), ConnectionMode.Bidirectional),
            // Captain's Quarters door
            new OffMeshConnection(new Vector3(-95.07f, 19.31f, 168.91f), new Vector3(-95.09f, 20.01f, 174.82f), ConnectionMode.Bidirectional),
            // Waverider Gate
            new OffMeshConnection(new Vector3(-127.92f, 15.78f, 155.41f), new Vector3(-134.11f, 15.90f, 158.23f), ConnectionMode.Bidirectional),
            // Door after Captain Madison #2
            new OffMeshConnection(new Vector3(-187.98f, 6.99f, 249.10f), new Vector3(-192.48f, 7.02f, 254.84f), ConnectionMode.Bidirectional)
        };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Shallowtail Reaver (Captain Madison)
                if (obj.Object.NpcId == 1208)
                    obj.Score += 1000;
                // Baleen Guard (Denn the Orcatoothed)
                if (obj.Object.NpcId == 1207)
                    obj.Score += 1000;
                // Scurvy Dog (Captain Madison #2)
                if (obj.Object.NpcId == 1205)
                    obj.Score += 1000;
                // Giant Clam
                if (obj.Object.NpcId == 342)
                    obj.Score += 1000;
            }
        }

        // Red Coral Formation - NpcId2000214 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  1
        // Blue Coral Formation - NpcId2000213 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  1
        // Green Coral Formation - NpcId2000215 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000214, ObjectRange = 25)]
		[ObjectHandler(2000213, ObjectRange = 25)]
		[ObjectHandler(2000215, ObjectRange = 25)]
        public async Task<bool> CoralFormationHandler(GameObject context)
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
        // Inconspicuous Switch - NpcId2000216 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000216, ObjectRange = 25)]
        public async Task<bool> InconspicuousSwitchHandler(GameObject context)
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
        // Captain's Quarters Key - NpcId2000250 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000250, ObjectRange = 25)]
        public async Task<bool> CaptainsQuartersKeyHandler(GameObject context)
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
        // Captain's Quarters - NpcId2000227 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000227, ObjectRange = 10)]
        public async Task<bool> CaptainsQuartersHandler(GameObject context)
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
        // Waverider Gate Key - NpcId2000255 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000255, ObjectRange = 25)]
        public async Task<bool> WaveriderGateKeyHandler(GameObject context)
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
        // Waverider Gate - NpcId2000231 - TODOStep: 4 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000231, ObjectRange = 10)]
        public async Task<bool> WaveriderGateHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.IsTodoChecked(3))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }

        // TO DO
        //
        // Determine why it's trying to go to the last boss instead of killing boss #3 (to obtain the key).
        //
        // Denn the Orcatoothed Tactics
        //
        // "Bubbles begin forming on the water's surface."
        // --- Determine if the bubbling of Unnatural Ripples can be detected in order to interact with the proper ones to prevent an add from spawning.
        // --- Name:Unnatural Ripples 0x1C97AD629A0, Type:ff14bot.Objects.EventObject, ID:2000405, Obj:1073837100
        // --- Name:Unnatural Ripples 0x1C97AD625A0, Type:ff14bot.Objects.EventObject, ID:2000407, Obj:1073837098
        // --- Name:Unnatural Ripples 0x1C97AD633A0, Type:ff14bot.Objects.EventObject, ID:2000406, Obj:1073837099
        // --- Name:Unnatural Ripples 0x1C97AD61FA0, Type:ff14bot.Objects.EventObject, ID:2000408, Obj:1073837097

    }
}
