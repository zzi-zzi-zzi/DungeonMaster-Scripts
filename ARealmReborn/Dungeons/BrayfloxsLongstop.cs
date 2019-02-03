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
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.ARealmReborn.Dungeons
{
    public class BrayfloxsLongstop : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 8;
        public override string Name => @"Brayflox's Longstop";
        public override Version Version => new Version(0, 9, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Goblin Pathfinder", 1004346, 0, 4f, new Vector3(22.98581f, 7.105875f, 26.96859f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("Great Yellow Pelican", 1280, 1, 32.70f, new Vector3(113.55f, -2.84f, -17.77f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Inferno Drake", 1284, 2, 27.74f, new Vector3(-9.10f, 5.74f, -91.09f), () => ScriptHelper.IsTodoChecked(2)),
                new Boss("Hellbender", 1286, 3, 33.11f, new Vector3(-110.22f, -2.38f, -33.58f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Aiatar", 1279, 4, 37.90f, new Vector3(-27.27f, 35.28f, -235.27f), () => ScriptHelper.IsTodoChecked(4))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Gate before Great Yellow Pelican
            new OffMeshConnection(new Vector3(104.94f, -0.12f, 13.28f), new Vector3(107.73f, -0.79f, 8.32f), ConnectionMode.Bidirectional),
            // Gate after Great Yellow Pelican
            new OffMeshConnection(new Vector3(113.31f, -3.07f, -31.18f), new Vector3(113.25f, -2.66f, -36.28f), ConnectionMode.Bidirectional),
            // Gate after Inferno Drake
            new OffMeshConnection(new Vector3(-17.54f, 5.74f, -72.07f), new Vector3(-21.63f, 5.74f, -69.03f), ConnectionMode.Bidirectional),
            // First gate after Hellbender
            new OffMeshConnection(new Vector3(-113.86f, -0.90f, -55.12f), new Vector3(-113.09f, -0.17f, -62.78f), ConnectionMode.Bidirectional),
            // Second gate after Hellbender
            new OffMeshConnection(new Vector3(-90.88f, 10.79f, -94.06f), new Vector3(-88.01f, 11.17f, -100.77f), ConnectionMode.Bidirectional),
            // Gate before Aiatar
            new OffMeshConnection(new Vector3(-101.28f, 21.14f, -187.73f), new Vector3(-100.35f, 21.69f, -194.17f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Brayflox Alltalks (Inferno Drake)
            AddAvoidObject<BattleCharacter>(5, 1300);
            // Toxic Vomit (Aiatar)
            AddAvoidObject<EventObject>(12, 2001694);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Queer Bubble (Hellbender)
                if (obj.Object.NpcId == 1383)
                    obj.Score += 1000;
                // Ashdrake
                if (obj.Object.NpcId == 1292)
                    obj.Score += 1000;
                // Mud Biast
                if (obj.Object.NpcId == 1291)
                    obj.Score += 1000;
                // Tempest Biast (Inferno Drake)
                if (obj.Object.NpcId == 1285)
                    obj.Score += 1000;
                // Violet Back (Great Yellow Pelican)
                if (obj.Object.NpcId == 1282)
                    obj.Score += 1000;
            }
        }

        // Goblin Pathfinder - NpcId1004346 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(1004346, ObjectRange = 25)]
        public async Task<bool> GoblinPathfinderHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (Talk.DialogOpen)
            {
                Talk.Next();
                return true;
            }
            if (!ScriptHelper.IsTodoChecked(0))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Runstop Headgate - NpcId2001462 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2001462, ObjectRange = 10)]
        public async Task<bool> RunstopHeadgateHandler(GameObject context)
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
        // Longstop Gutgate - NpcId2001466 - TODOStep: 3 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2001466, ObjectRange = 10)]
        public async Task<bool> LongstopGutgateHandler(GameObject context)
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

        // Brayflox's Longstop
        //
        // Great Yellow Pelican Tactics
        //
        // (?) => Frontal AoE attack. Tank facing away from group.
        //
        // Inferno Drake Tactics
        //
        // (?) => Frontal AoE attack. Tank facing away from group.
        //
        // Hellbender Tactics
        //
        // Stagnant Spray => Frontal AoE attack. Tank facing away from group.
        //
        // Aiatar Tactics
        //
        // Toxic Vomit => Drops poison circles on the ground. Heals the boss if near him; Rehabilitation (aura) => Heals the boss while in Toxic Vomit AoE.

    }
}
