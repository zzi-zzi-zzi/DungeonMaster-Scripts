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
using ff14bot.Managers;
using ff14bot.Objects;

namespace DungeonMasterScripts.Stormblood.Trials
{
    public class TheJadeStoa : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20051;
        public override string Name => @"The Jade Stoa";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Byakko", 7092, 0, 21.08f, new Vector3(0.00f, 0.00f, -7.05f), () => ScriptHelper.IsTodoChecked(0))
            };
        }

        private uint[] _bombogenesis = new uint[] { 10811, 10215 };
        private uint[] _distantClap = new uint[] { 10800, 10205 };

        public override void OnEnter()
        {
            // Bombogenesis (10215 or 10811?) => Spread out and avoid all players by 7y(?).
            AddAvoidObject<GameObject>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _bombogenesis.Contains(i.CastingSpellId)),
                10,
                x => x.NpcId == 7092 || (x.Type == ff14bot.Enums.GameObjectType.Pc && !x.IsMe),
                x => x.Location
                );
        }

        [EncounterHandler(7092, "Byakko", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> ByakkoHandler(BattleCharacter c)
        {
            if (c.IsCasting && _distantClap.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 5f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Byakko"), 5f, true);
                    return true;
                }
            }

            return false;
        }

        // Byakko Tactics
        //
        // State of Shock (10070 or 10208?) => Grab a player and slam them on the platform. Everyone else run to them and stack to share damage. Player has debuff?
        // Sweep of the Leg (?) => Move behind the boss. Always happens after State of Shock.
        // Unrelenting Anguish (10221) - Red orbs travel from boss outwards. Avoid red orbs while attacking the boss. Red Orb - No ID attainable?
        // Hakutei (Tiger Boss) => Tanks should pick this up straight away and face away from the group as the tiger has a nasty cleave.
        // --- White Herald (10234 or 10828?) => The tiger will then leap up into the air and mark his primary target (debuff ID?).
        // ------ The player marked should run away from the group to lessen damage to others.
        // --- Roar of Thunder (10233, 10682, 10827 or 10832?) => The tiger will then leap to the center and begin slowly casting this.
        // ------ Players should try to do as much damage as possible to lessen the incoming damage once the tiger has completed the cast.
        // ------ Once the cast is complete all players will be inflicted with down for the count and unable to move and thrown into the air and then dropped from a great height.
        // ------ While falling you will have to dodge orbs and AOEs (similar to before but will falling).
        // Hundredfold Havoc (10213, 10214, 10808 or 10809?) => The boss will cast two sets of lightning AOE's that move to the outside - if hit it will deal damage and paralysis.
        // Bombogenesis (10215 or 10811?) => Spread out and avoid all players by 7y(?).
        // Distant Clap (10205 or 10800?) => Stack near boss.

    }
}