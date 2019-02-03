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
using Buddy.Coroutines;
using Clio.Utilities;
using DungeonMaster.Attributes;
using DungeonMaster.DungeonProfile;
using DungeonMaster.Enumeration;
using DungeonMaster.Helpers;
using DungeonMaster.Managers;
using DungeonMaster.TargetingSystems;
using ff14bot;
using ff14bot.Behavior;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.ARealmReborn.Trials
{
    public class TheDragonsNeck : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 20026;
        public override string Name => @"The Dragon's Neck";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Typhon", 3046, 0, 22.25f, new Vector3(-263.70f, 19.12f, 22.14f), () => DMDirectorManager.Instance.InstanceEnded)
            };
        }

        private uint[] _severeSnort = new uint[] { 3117 };
        private uint[] _snortsault = new uint[] { 3159, 3121, 3120, 3119 };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Typhon
                if (obj.Object.NpcId == 3046)
                    obj.Score += 1000;
            }
        }

        [EncounterHandler(3046, "Typhon", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> TyphonHandler(BattleCharacter c)
        {
            if (Core.Me.HasAura(626))
            {
                var SafeSpot = new Vector3(-258.11f, 19.07f, 18.08f);

                while (!Navigator.InPosition(SafeSpot, Core.Me.Location, 18f))
                {
                    Navigator.PlayerMover.MoveTowards(SafeSpot);
                    MovementManager.Jump();
                    return true;
                }
                await CommonTasks.StopMoving();
            }
            if (c.IsCasting && _severeSnort.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 3f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Typhon"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }
            while (c.IsCasting && _snortsault.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 3f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Typhon"), 3f, true);
                    return true;
                }
                return true;
            }
            if (Core.Me.HasAura(613))
            {
                var ultrosId = GameObjectManager.GetObjectByNPCId(3047);

                if (c.IsCasting && _severeSnort.Contains(c.CastingSpellId))
                {
                    if (Core.Me.Distance2D(c) > 3f)
                    {
                        await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Typhon"), 3f, true);
                        await Coroutine.Sleep(100);
                        ActionManager.DoAction(3139, c);
                        await Coroutine.Sleep(1000);
                        return true;
                    }
                    return true;
                }
                if (!Navigator.InPosition(ultrosId.CalculatePointInFront(2f), Core.Me.Location, 2f))
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(ultrosId.CalculatePointInFront(2f), "Ultros"), 2f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }

            return false;
        }

        // The Dragon's Neck
        //
        // Ultros & Typhon Tactics
        //
        // Imp (Aura: 613) => Stack in front of Ultros to gain Wet Strike (Aura: 612) => Typhon casts Severe Snort (3117) => Cast Imp Punch (3139) on Typhon to interrupt him.
        // --- Does not stack in front of Ultros. Too many hidden objects?
        // ------ Name:Ultros 0x1A72B7B79F0, Type:ff14bot.Objects.BattleCharacter, ID:3047, Obj:1073750712
        // ------ Name:Ultros 0x1A72B7BA390, Type:ff14bot.Objects.BattleCharacter, ID:3047, Obj:1073750713
        // ------ Name:Ultros 0x1A72B7BCD30, Type:ff14bot.Objects.BattleCharacter, ID:3047, Obj:1073750714
        // ------ Name:Ultros 0x1A72B7BF6D0, Type:ff14bot.Objects.BattleCharacter, ID:3047, Obj:1073750715
        // ------ Name:Ultros 0x1A72B7C2070, Type:ff14bot.Objects.BattleCharacter, ID:3047, Obj:1073750716
        // ------ Name:Ultros 0x1A72B7C4A10, Type:ff14bot.Objects.BattleCharacter, ID:3047, Obj:1073750717
        // ------ Name:Ultros 0x1A72B7C73B0, Type:ff14bot.Objects.BattleCharacter, ID:3047, Obj:1073750718
        // ------ Name:Ultros 0x1A72B7C9D50, Type:ff14bot.Objects.BattleCharacter, ID:3047, Obj:1073750719
        // ------ Name:Ultros 0x1A72B7E6730, Type:ff14bot.Objects.BattleCharacter, ID:3047, Obj:1073750698
        // Snortsault (while !c.IsTargetable) => Typhon goes to the center and places a line (3y width?) of air from the front and back, splitting the platform in two and slowly rotates around the platform.
        // FUNGAH => Targets random player with red indicator over target. Avoid all players by 8y(?) if targeted by Typhon while casting.

    }
}
