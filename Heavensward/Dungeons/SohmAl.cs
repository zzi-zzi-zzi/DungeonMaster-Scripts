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

namespace DungeonMasterScripts.Heavensward.Dungeons
{
    public class SohmAl : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 37;
        public override string Name => @"Sohm Al";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Raskovnik", 3791, 0, 23.30f, new Vector3(-126.98f, 12.00f, 168.31f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("Myath", 3793, 1, 30.66f, new Vector3(158.84f, 137.61f, -94.17f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Tioman", 3798, 2, 27.00f, new Vector3(-103.49f, 348.16f, -395.87f), () => ScriptHelper.IsTodoChecked(2))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First vines before Raskovnik
            new OffMeshConnection(new Vector3(-253.14f, -2.90f, 189.06f), new Vector3(-246.26f, -0.86f, 187.83f), ConnectionMode.Bidirectional),
            // Second vines before Raskovnik
            new OffMeshConnection(new Vector3(-165.00f, 4.10f, 198.15f), new Vector3(-159.44f, 5.20f, 196.90f), ConnectionMode.Bidirectional),
            // Transition line after Raskovnik
            new OffMeshConnection(new Vector3(-94.29f, 18.76f, 123.55f), new Vector3(332.21f, 93.97f, 64.00f), ConnectionMode.Bidirectional),
            // Transition line after Myath
            new OffMeshConnection(new Vector3(195.34f, 140.14f, -139.04f), new Vector3(-95.14f, 317.82f, -212.22f), ConnectionMode.Bidirectional),
            // First rubble pile before Tioman
            new OffMeshConnection(new Vector3(-177.48f, 326.39f, -277.12f), new Vector3(-185.51f, 328.91f, -286.50f), ConnectionMode.OneWay),
            // Second rubble pile before Tioman
            new OffMeshConnection(new Vector3(-191.56f, 332.23f, -330.22f), new Vector3(-185.91f, 333.91f, -337.63f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Fireball
            AddAvoidObject<EventObject>(7, 2005287);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Chyme of the Mountain (Myath)
                if (obj.Object.NpcId == 3794)
                    obj.Score += 3000;
                // Blood of the Mountain (Myath)
                if (obj.Object.NpcId == 3797)
                    obj.Score += 2000;
                // The Right Wing of Injury (Tioman)
                if (obj.Object.NpcId == 4389)
                    obj.Score += 1000;
                // The Left Wing of Tragedy (Tioman)
                if (obj.Object.NpcId == 4388)
                    obj.Score += 1000;
                // Ice Boulder
                if (obj.Object.NpcId == 3812)
                    obj.Score += 1000;
                // Drakespur
                if (obj.Object.NpcId == 3803)
                    obj.Score += 1000;
                // Rheum of the Mountain (Myath)
                if (obj.Object.NpcId == 3796)
                    obj.Score += 1000;
                // Dravanian Hornet (Raskovnik)
                if (obj.Object.NpcId == 3444)
                    obj.Score += 1000;
            }
        }

        // Myath Tactics
        //
        // Mad Dash => Consume an add, target a player and throw the add at the player.
        // --- Consumes a blue add: spread away from the party member with a blue indicator over their head.
        // --- Consumes a red add: stack on the party member with the red indicator over their head.
        //
        // Tioman Tactics
        //
        // Abyssic Buster => Frontal-cone attack. Tanks should keep the boss facing away.
        // Comet => Mark two players and after several seconds, marks fade and comets will fall where the players were standing, dealing room-wide AoE damage based on proximity to these meteors.
        // Heavensfall => Marks a player, spawning clusters of circular AoEs on the player. Marked players should move away from the party.

    }
}
