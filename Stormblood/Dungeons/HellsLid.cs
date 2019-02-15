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
using Clio.Utilities;
using DungeonMaster.DungeonProfile;
using DungeonMaster.Enumeration;
using DungeonMaster.Helpers;
using DungeonMaster.Managers;
using DungeonMaster.TargetingSystems;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class HellsLid : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 59;
        public override string Name => @"Hell's Lid";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Otake-maru", 6994, 0, 27.50f, new Vector3(-70.88f, -3.00f, 122.03f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Kamaitachi", 6995, 1, 26.59f, new Vector3(59.49f, -26.00f, -128.58f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Genbu", 6996, 2, 23.61f, new Vector3(61.81f, -88.00f, -480.80f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Jump to fire pit (forward only)
            new OffMeshConnection(new Vector3(-72.63982f, 37.03542f, 307.3419f), new Vector3(-72.75501f, 3.126698f, 296.2279f), ConnectionMode.OneWay),
            // 1st fire pit wall
            new OffMeshConnection(new Vector3(-72.01672f, 2.977304f, 281.2847f), new Vector3(-72.38127f, 2.809354f, 269.8404f), ConnectionMode.Bidirectional),
            // 2nd fire pit wall
            new OffMeshConnection(new Vector3(-71.93714f, 2.999997f, 203.4669f), new Vector3(-70.15527f, 1.823361f, 187.5965f), ConnectionMode.Bidirectional),
            // Jump after Kamaitachi (forward only)
            new OffMeshConnection(new Vector3(59.56952f, -27.45894f, -155.512f), new Vector3(56.61665f, -97.03144f, -173.0121f), ConnectionMode.OneWay)
        };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Tsumuji-kaze (Kamaitachi)
                if (obj.Object.NpcId == 7016)
                    obj.Score += 1000;
                // Chelonian Gate (Genbu)
                if (obj.Object.NpcId == 6997)
                    obj.Score += 1000;
            }
        }

        // Otake-maru Tactics
        //
        // Liquid Carapace (10176) => Run away from boss if you are the target of this spell.
        //
        // Are either of these the ID's for the items the boss "drops?" If so, add 15y avoidance radius once visible.
        // --- EventObject, ID:2002735
        // --- EventObject, ID:2007457
        //
        // Kamaitachi Tactics
        //
        // Gale (7181) => 10y avoidance.
        // Windage? (?) => 3y avoidance.
        //
        // Is Northerly (or Late Harvest?) the spell to avoid when boss disappears and "dashes" through the party?
        // --- Northerly (BattleCharacter) => 7182
        //
        // Genbu Tactics
        //
        // Sinister Tide (10197 or 10196?) => Avoid hexagon tiles that will be in the path after the orb is "dropped."
        //
        // Are any of these the hexagon "glyph" ID's?
        // --- EventObject => 2007457
        // --- EventObject => 2002735
        // --- EventObject => 2009378
        // --- EventObject => 2009380
        // --- EventObject => 2009382
        // --- EventObject => 2009383
        // --- EventObject => 2009379
        // --- EventObject => 2009381
        // --- AreaObject => 247
        // --- BattleCharacter => 108

    }
}
