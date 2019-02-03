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
    public class DomaCastle : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 54;
        public override string Name => @"Doma Castle";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Magitek Rearguard", 6200, 0, 22.08f, new Vector3(122.06f, 40.58f, 16.71f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Magitek Hexadrone", 6203, 1, 18.74f, new Vector3(-240.01f, 45.50f, 130.48f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Hypertuned Grynewaht", 6205, 2, 19.80f, new Vector3(-239.96f, 67.00f, -199.45f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First door before Magitek Rearguard
            new OffMeshConnection(new Vector3(298.82f, 24.75f, 129.04f), new Vector3(293.88f, 24.75f, 127.12f), ConnectionMode.Bidirectional),
            // Second door before Magitek Rearguard
            new OffMeshConnection(new Vector3(300.08f, 24.50f, 41.99f), new Vector3(301.16f, 24.50f, 36.60f), ConnectionMode.Bidirectional),
            // Third door before Magitek Rearguard
            new OffMeshConnection(new Vector3(169.92f, 40.07f, 28.25f), new Vector3(162.62f, 40.58f, 26.54f), ConnectionMode.Bidirectional),
            // Door after Magitek Rearguard
            new OffMeshConnection(new Vector3(33.19f, 40.00f, 90.07f), new Vector3(29.98f, 40.00f, 95.02f), ConnectionMode.Bidirectional),
            // Door before Magitek Hexadrone
            new OffMeshConnection(new Vector3(-184.88f, 44.30f, 124.97f), new Vector3(-206.75f, 44.00f, 131.20f), ConnectionMode.Bidirectional),
            // Door after Magitek Hexadrone
            new OffMeshConnection(new Vector3(-240.33f, 44.00f, 88.47f), new Vector3(-240.30f, 44.00f, 79.83f), ConnectionMode.Bidirectional),
            // First door before Hypertuned Grynewaht
            new OffMeshConnection(new Vector3(-240.16f, 64.00f, -92.49f), new Vector3(-239.87f, 64.00f, -97.09f), ConnectionMode.Bidirectional),
            // Second door before Hypertuned Grynewaht
            new OffMeshConnection(new Vector3(-239.90f, 66.00f, -139.60f), new Vector3(-239.83f, 66.00f, -143.90f), ConnectionMode.Bidirectional)
        };

        private uint[] _chainsaw = new uint[] { 8361, 8360 };

        public override void OnEnter()
        {
            // Rearguard Mines (Magitek Rearguard)
            AddAvoidObject<BattleCharacter>(3, 6202);
        }

        public override void OnExit()
        {

        }

        // Magitek Rearguard Tactics
        //
        // Garlean Fire => SideStep? AoE missiles in the direction of a targeted player.
        //
        // Magitek Hexadrone Tactics
        //
        // 2-Tonze Magitek Missile => Stack up to soak AoE damage.
        // Magitek Missiles => Stand in each pillar to lower AoE damage.
        // --- Pillars do not have their own Ids?
        // Magitek Bits => SideStep? Line AoEs with knockback.
        //
        // Hypertuned Grynewaht Tactics
        //
        // Chainsaw => Untelegraphed frontal cone AoE on tank. Move to flank/behind boss.
        // Delay-action Charge => Red indicators on random players. Avoid players (10y).
        // Gunsaw => Targets random player for a focused gun attack. Avoid players (10y).
        // Prey (debuff; 1253) => Move to corner of room until marker disappears, then move to boss.
        // Magitek Bits => SideStep? Line AoEs with knockback.

    }
}
