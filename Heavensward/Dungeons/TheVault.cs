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
    public class TheVault : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 34;
        public override string Name => @"The Vault";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Ser Adelphel Brightblade", 3849, 0, 22.20f, new Vector3(4.11f, -291.94f, -99.98f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("Ser Grinnaux the Bull", 3850, 1, 21.98f, new Vector3(-4.54f, 0.00f, 71.98f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Ser Charibert", 3642, 2, 27.08f, new Vector3(0.02f, 300.00f, -4.11f), () => ScriptHelper.IsTodoChecked(2))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First gate before Ser Adelphel Brightblade
            new OffMeshConnection(new Vector3(39.09f, -300.00f, 29.71f), new Vector3(44.79f, -299.92f, 29.70f), ConnectionMode.Bidirectional),
            // Second gate before Ser Adelphel Brightblade
            new OffMeshConnection(new Vector3(44.97f, -299.92f, -29.38f), new Vector3(38.47f, -300.00f, -29.46f), ConnectionMode.Bidirectional),
            // First gate after Ser Adelphel Brightblade
            new OffMeshConnection(new Vector3(7.10f, -282.35f, -151.81f), new Vector3(10.40f, -281.71f, -151.74f), ConnectionMode.Bidirectional),
            // Second gate after Ser Adelphel Brightblade
            new OffMeshConnection(new Vector3(100.40f, -263.98f, 0.09f), new Vector3(105.03f, -263.87f, 0.09f), ConnectionMode.Bidirectional),
            // Zone transition after Ser Adelphel Brightblade
            new OffMeshConnection(new Vector3(105.03f, -263.87f, 0.09f), new Vector3(101.05f, 0.00f, 0.34f), ConnectionMode.OneWay),
            // Gate before Ser Grinnaux the Bull
            new OffMeshConnection(new Vector3(20.94f, 0.00f, 72.11f), new Vector3(17.94f, 0.00f, 72.14f), ConnectionMode.Bidirectional),
            // First gate after Ser Grinnaux the Bull
            new OffMeshConnection(new Vector3(-18.07f, 0.00f, 72.11f), new Vector3(-20.97f, 0.00f, 72.04f), ConnectionMode.Bidirectional),
            // Second gate after Ser Grinnaux the Bull
            new OffMeshConnection(new Vector3(-109.12f, 0.00f, -0.03f), new Vector3(-113.12f, 0.13f, -0.03f), ConnectionMode.Bidirectional),
            // Zone transition after Ser Grinnaux the Bull
            new OffMeshConnection(new Vector3(-113.12f, 0.13f, -0.03f), new Vector3(-86.02f, 285.00f, -0.57f), ConnectionMode.OneWay),
            // Door before Ser Charibert
            new OffMeshConnection(new Vector3(-45.44f, 291.00f, 48.92f), new Vector3(-41.21f, 291.00f, 48.89f), ConnectionMode.Bidirectional)
        };

        public override void OnEnter()
        {
            // Brightsphere (Ser Adelphel Brightblade)
            AddAvoidObject<BattleCharacter>(7, 4385);
            // Stellar Implosion (Ser Grinnaux the Bull)
            AddAvoidObject<EventObject>(8, 2003393);
            // Aetherial Tear (Ser Grinnaux the Bull)
            AddAvoidObject<BattleCharacter>(8, 3293);
            // Dawn Knight (Ser Charibert)
            AddAvoidObject<BattleCharacter>(3, 3851);
            // Dusk Knight (Ser Charibert)
            AddAvoidObject<BattleCharacter>(3, 3852);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Vault Deacon (Ser Adelphel Brightblade)
                if (obj.Object.NpcId == 3843)
                    obj.Score += 2000;
                // Holy Flame (Ser Charibert)
                if (obj.Object.NpcId == 4400)
                    obj.Score += 1000;
                // Vault Ostiary (Ser Adelphel Brightblade)
                if (obj.Object.NpcId == 3841)
                    obj.Score += 1000;
            }
        }

        // Ser Grinnaux the Bull Tactics
        //
        // Red telegraphed AoE attack(s) => SideStep?
        // Hyperdimensional Slash => SideStep? Column AoE attack in the direction of a random player.
        // Faith Unmoving => Pushes all players directly backward from Grinnaux. Add avoidance lines between boss and Aetherial Tears.
        //
        // Ser Charibert Tactics
        //
        // Holy Chain => Chained players run away to break the chain.
        // Heavensflame => SideStep? Flame rings on the ground.

    }
}
