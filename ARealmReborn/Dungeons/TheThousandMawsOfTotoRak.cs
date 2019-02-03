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
    public class TheThousandMawsOfTotoRak : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 1;
        public override string Name => @"The Thousand Maws of Toto-Rak";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Magitek Terminal #1", 2000116, 0, 5.78f, new Vector3(-112.01f, -4.13f, 112.03f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Magitek Photocell", 2000039, 1, 8.62f, new Vector3(-73.75f, -6.33f, 21.54f), () => DMDirectorManager.Instance.GetTodoArgs(0).Item1 > 3 || ScriptHelper.IsTodoChecked(2)),
                new Boss("Magitek Terminal #2", 2000118, 2, 5.00f, new Vector3(-80.03f, -8.13f, -47.97f), () => ScriptHelper.IsTodoChecked(2)),
                new Boss("Abacination Chamber Door", 2000385, 3, 7.14f, new Vector3(96.21f, -19.93f, -112.00f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Graffias", 444, 4, 38.43f, new Vector3(231.99f, -39.29f, -144.30f), () => ScriptHelper.IsTodoChecked(4))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Door before first terminal
            new OffMeshConnection(new Vector3(-208.20f, -0.15f, -94.13f), new Vector3(-207.99f, -0.14f, -98.42f), ConnectionMode.Bidirectional),
            // Barrier after first terminal
            new OffMeshConnection(new Vector3(-98.33f, -4.19f, 111.42f), new Vector3(-93.23f, -4.54f, 111.28f), ConnectionMode.Bidirectional),
            // Jump before second terminal
            new OffMeshConnection(new Vector3(14.28f, -15.73f, -12.44f), new Vector3(13.50f, -13.71f, -16.91f), ConnectionMode.OneWay),
            // Sticky Web before second terminal
            new OffMeshConnection(new Vector3(-63.56f, -8.00f, -48.02f), new Vector3(-67.76f, -8.09f, -47.77f), ConnectionMode.Bidirectional),
            // Barrier after second terminal
            new OffMeshConnection(new Vector3(-93.39f, -8.16f, -47.96f), new Vector3(-98.89f, -8.16f, -47.90f), ConnectionMode.Bidirectional),
            // First Sticky Web before Graffias
            new OffMeshConnection(new Vector3(-104.68f, -12.69f, -136.11f), new Vector3(-100.58f, -12.80f, -139.83f), ConnectionMode.Bidirectional),
            // Second Sticky Web before Graffias
            new OffMeshConnection(new Vector3(-8.75f, -12.52f, -136.50f), new Vector3(-5.04f, -12.86f, -140.01f), ConnectionMode.Bidirectional),
            // Door before Graffias
            new OffMeshConnection(new Vector3(94.08f, -20.18f, -112.03f), new Vector3(98.83f, -20.12f, -111.96f), ConnectionMode.Bidirectional),
            // Third Sticky Web before Graffias
            new OffMeshConnection(new Vector3(173.45f, -33.47f, -140.38f), new Vector3(177.96f, -33.92f, -140.90f), ConnectionMode.Bidirectional),
            // Jump before Graffias
            new OffMeshConnection(new Vector3(193.80f, -35.99f, -142.83f), new Vector3(197.10f, -39.47f, -137.95f), ConnectionMode.OneWay)
        };

        public override void OnEnter()
        {
            // Poison (Graffias)
            AddAvoidObject<EventObject>(8, 2000404);
            // Fleshy Pod
            AddAvoidObject<BattleCharacter>(1, 437);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Graffias's Tail (Graffias)
                if (obj.Object.NpcId == 440)
                    obj.Score += 2000;
                // Comesmite (Graffias)
                if (obj.Object.NpcId == 443)
                    obj.Score += 1000;
                // Warden's Whip
                if (obj.Object.NpcId == 441)
                    obj.Score += 1000;
                // Sticky Web
                if (obj.Object.NpcId == 439)
                    obj.Score += 1000;
            }
        }

        private readonly uint[] _excludeTargets = {
            437,
        };

        public override void IncludeTargetsFilter(List<GameObject> incomingObjects, HashSet<GameObject> outgoingObjects)
        {
            base.IncludeTargetsFilter(incomingObjects, outgoingObjects);
            foreach (var @in in incomingObjects.Where(i => _excludeTargets.Contains(i.NpcId)))
                outgoingObjects.Remove(@in);
        }

        // Accusation Chamber Door - NpcId2000384 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000384, ObjectRange = 25)]
        public async Task<bool> AccusationChamberDoorHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Magitek Photocell - NpcId2000110 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Photocell - NpcId2000108 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Photocell - NpcId2000109 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Photocell - NpcId2000102 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Photocell - NpcId2000101 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Photocell - NpcId2000103 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Photocell - NpcId2000100 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Photocell - NpcId2000104 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1
        // Magitek Photocell - NpcId2000105 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1
        // Magitek Photocell - NpcId2000039 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000110, ObjectRange = 25)]
		[ObjectHandler(2000108, ObjectRange = 25)]
		[ObjectHandler(2000109, ObjectRange = 25)]
		[ObjectHandler(2000102, ObjectRange = 25)]
		[ObjectHandler(2000101, ObjectRange = 25)]
		[ObjectHandler(2000103, ObjectRange = 25)]
        [ObjectHandler(2000100, ObjectRange = 25)]
        [ObjectHandler(2000104, ObjectRange = 25)]
        [ObjectHandler(2000105, ObjectRange = 25)]
        [ObjectHandler(2000039, ObjectRange = 25)]
        public async Task<bool> MagitekPhotocellHandler(GameObject context)
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
        // Magitek Terminal - NpcId2000116 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
        // Magitek Terminal - NpcId2000118 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2000116, ObjectRange = 25)]
        [ObjectHandler(2000118, ObjectRange = 25)]
        public async Task<bool> MagitekTerminalHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible && DMDirectorManager.Instance.GetTodoArgs(0).Item1 > 3)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Abacination Chamber Door - NpcId2000385 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2000385, ObjectRange = 25)]
        public async Task<bool> AbacinationChamberDoorHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context.IsVisible)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }

        // TO DO
        //
        // Create OMC's for the likely dozens of unnavigable stairs. /wrists

    }
}
