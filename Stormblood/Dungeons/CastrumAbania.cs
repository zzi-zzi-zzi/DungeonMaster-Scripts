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
using System.Threading.Tasks;
using Clio.Utilities;
using DungeonMaster.Attributes;
using DungeonMaster.DungeonProfile;
using DungeonMaster.Helpers;
using DungeonMaster.TargetingSystems;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class CastrumAbania : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 55;
        public override string Name => @"Castrum Abania";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // 1st door before Magna Roader
            new OffMeshConnection(new Vector3(-272.893f, -6.007182f, 99.80691f),
                new Vector3(-269.3601f, -6.126387f, 96.19979f), ConnectionMode.Bidirectional),
            // 2nd door before Magna Roader
            new OffMeshConnection(new Vector3(-212.9513f, -6.173335f, 82.59279f),
                new Vector3(-212.8783f, -6.250043f, 87.94285f), ConnectionMode.Bidirectional),
            // 3rd door before Magna Roader
            new OffMeshConnection(new Vector3(-212.8965f, -2.045638f, 132.5938f),
                new Vector3(-212.8893f, -2.095681f, 139.2455f), ConnectionMode.Bidirectional),
            // 1st laser beam wall after Magna Roader
            new OffMeshConnection(new Vector3(-165.5338f, 1.958261f, 293.4391f),
                new Vector3(-162.5728f, 1.958262f, 296.3858f), ConnectionMode.Bidirectional),
            // 2nd laser beam wall after Magna Roader
            new OffMeshConnection(new Vector3(-80.27465f, 5.958236f, 311.2277f),
                new Vector3(-76.69224f, 5.958237f, 311.1907f), ConnectionMode.Bidirectional),
            // 3rd laser beam wall after Magna Roader
            new OffMeshConnection(new Vector3(-7.448692f, 9.95823f, 263.4287f),
                new Vector3(-4.380218f, 9.958231f, 260.4529f), ConnectionMode.Bidirectional),
            // 1st door after Number XXIV
            new OffMeshConnection(new Vector3(113.3588f, 25.90308f, 141.8893f),
                new Vector3(117.5956f, 25.89697f, 137.6506f), ConnectionMode.Bidirectional),
            // 2nd door after Number XXIV
            new OffMeshConnection(new Vector3(212.1904f, 20.00026f, 43.00548f),
                new Vector3(215.6089f, 20.00026f, 39.5635f), ConnectionMode.Bidirectional),
            // Door before Inferno
            new OffMeshConnection(new Vector3(267.5828f, 20.08962f, 28.19647f),
                new Vector3(269.2203f, 19.96426f, 21.85478f), ConnectionMode.Bidirectional),
        };

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Magna Roader", 6263, 0, 30.09f, new Vector3(-213.11f, -2.00f, 199.25f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Number XXIV", 6267, 1, 28.65f, new Vector3(10.48f, 14.00f, 175.65f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Inferno", 6268, 2, 29.53f, new Vector3(285.78f, 19.97f, -38.39f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // 12th Legion Death Claw (Inferno)
                if (obj.Object.NpcId == 6270)
                    obj.Score += 2000;
                // 12th Legion Packer (Inferno)
                if (obj.Object.NpcId == 6269)
                    obj.Score += 1000;
                // 12th Legion Optio (Magna Roader)
                if (obj.Object.NpcId == 6264)
                    obj.Score += 1000;
            }
        }

        // Mark XLIII Mini Cannon - NpcId6266 - TODOStep: 1 - TODOValue (Before Pickup): 0 /  1
		[ObjectHandler(6266, ObjectRange = 25)]
        public async Task<bool> MarkXLIIIMiniCannonHandler(GameObject context)
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
        // Is there a door after Magna Roader that is closed until you kill it?
        //
        // Magna Roader Tactics
        //
        // Wild Speed => Rushes around the room in line patterns. kill adds and use cannons to stop him.
        // Use Mark XLIII Mini Cannon (ground targeted AE) on boss to stop Wild Speed.
        //
        // Number XXIV Tactics
        //
        // Elemental Pillars => Spawns 3 elemental circles (fire, ice and thunder). Run to the circle of the same element as the boss.
        // --- Boss Attunement Auras:
        // ------ Fire Convergence (1275)
        // ------ Ice Convergence (1276)
        // ------ Lightning Convergence (1277)
        //
        // Spawned Pillars are in this list?
        // --- EventObject => 2008072
        // --- EventObject => 2008527
        // --- EventObject => 2002735
        // --- EventObject => 2008054
        // --- EventObject => 2002872
        // --- EventObject => 2008052
        // --- EventObject => 2008049
        // --- EventObject => 2008046
        // --- EventObject => 2008070
        // --- EventObject => 2008045
        // --- EventObject => 2008053
        // --- EventObject => 2008040
        // --- EventObject => 2008089
        // --- EventObject => 2008056
        // --- EventObject => 2008050
        //
        // Elemental "rings"?
        // --- EventObject => 2008517
        // --- EventObject => 2008515
        // --- EventObject => 2008516

    }
}
