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
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class TheTempleOfTheFist : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 51;
        public override string Name => @"The Temple of the Fist";
        public override Version Version => new Version(0, 8, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Coeurl Sruti", 6119, 0, 21.76f, new Vector3(422.23f, 65.15f, 466.06f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Arbuda", 6118, 1, 16.40f, new Vector3(-249.50f, 296.00f, -96.55f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Ivon Coeurlfist", 6117, 2, 19.47f, new Vector3(-249.99f, 276.00f, -462.00f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Moss Barrier
            new OffMeshConnection(new Vector3(337.0319f, 53.48928f, 310.951f), new Vector3(336.9767f, 53.48928f, 317.6466f), ConnectionMode.Bidirectional),
            // Moss Wall
            new OffMeshConnection(new Vector3(423.3761f, 65.1466f, 415.2088f), new Vector3(423.402f, 65.14661f, 420.6583f), ConnectionMode.Bidirectional),
            // Transition to Enlightenment
            new OffMeshConnection(new Vector3(468.5366f, 70.69473f, 460.9574f), new Vector3(-448.7344f, 274.7629f, 182.1891f), ConnectionMode.OneWay),
            // Transition to Dilemma
            new OffMeshConnection(new Vector3(-448.4513f, 273.1519f, 188.6515f), new Vector3(461.0766f, 67.00912f, 461.7069f), ConnectionMode.OneWay),
            // First gate before Arbuda
            new OffMeshConnection(new Vector3(-346.2586f, 288.9589f, 2.770248f), new Vector3(-337.643f, 289.1596f, 2.734842f), ConnectionMode.Bidirectional),
            // Second gate before Arbuda
            new OffMeshConnection(new Vector3(-250.155f, 296.2524f, -45.33004f), new Vector3(-250.05f, 296.2669f, -53.415f), ConnectionMode.Bidirectional),
            // Barrier before Ivon Coeurlfist
            new OffMeshConnection(new Vector3(-250.0644f, 276f, -439.0043f), new Vector3(-249.98f, 276f, -446.7042f), ConnectionMode.Bidirectional)
        };

        private uint[] _foreAndAft = new uint[] { 8155 };
        private uint[] _portAndStar = new uint[] { 8156 };
        private uint[] _rhalgrsPiece = new uint[] { 8173 };

        public override void OnEnter()
        {
            // Head of the Coeurl (Ivon Coeurlfist)
            AddAvoidObject<BattleCharacter>(5, 6115);
            // Twister (Ivon Coeurlfist)
            AddAvoidObject<BattleCharacter>(7, 6116);
            // Rhalgr's Piece => Move 20y from boss.
            AddAvoidObject<BattleCharacter>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _rhalgrsPiece.Contains(i.CastingSpellId)),
                20,
                x => x.NpcId == 6117,
                x => x.Location
                );
            // Moss Bloom
            AddAvoidObject<BattleCharacter>(5, 6133);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Gyr Abanian Zu
                if (obj.Object.NpcId == 6127)
                    obj.Score += 1000;
                // Coeurl Smriti (Coeurl Sruti)
                if (obj.Object.NpcId == 6120)
                    obj.Score += 1000;
            }
        }

        [EncounterHandler(6118, "Arbuda", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> ArbudaHandler(BattleCharacter c)
        {
            var arbudaId = GameObjectManager.GetObjectByNPCId(6118);

            if (Core.Me.HasAura(1136))
            {
                if (Core.Me.Location.Distance(new Vector3(-260.6502f, 296f, -87.467f)) > 1f && Core.Me.Location.Distance(new Vector3(-239.1116f, 296f, -108.6389f)) > 1f)
                {
                    // Find both points, but go to the "activated" seal.
                    // await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(new Vector3(-260.6502f, 296f, -87.467f), "Moonseal"), 1f, true);
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(new Vector3(-239.1116f, 296f, -108.6389f), "Moonseal"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }
            if (Core.Me.HasAura(1135))
            {
                if (Core.Me.Location.Distance(new Vector3(-239.3287f, 296f, -87.41592f)) > 1f && Core.Me.Location.Distance(new Vector3(-260.6499f, 296f, -108.648f)) > 1f)
                {
                    // Find both points, but go to the "activated" seal.
                    // await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(new Vector3(-239.3287f, 296f, -87.41592f), "Sunseal"), 2f, true);
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(new Vector3(-260.6499f, 296f, -108.648f), "Sunseal"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }
            if (c.IsCasting && _foreAndAft.Contains(c.CastingSpellId))
            {
                // Verify flanking logic is correct.
                if (!arbudaId.IsFlanking)
                {
                    // Find both points, but go to the closest point.
                    // await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.CalculatePointAtSide(8f, false), "Arbuda"), 1f, true);
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.CalculatePointAtSide(8f, true), "Arbuda"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }
            if (c.IsCasting && _portAndStar.Contains(c.CastingSpellId))
            {
                // Verify flanking logic is correct.
                if (arbudaId.IsFlanking)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.CalculatePointInFront(8f), "Arbuda"), 1f, true);
                    return true;
                }
                await CommonTasks.StopMoving();
                return true;
            }

            return false;
        }
        [EncounterHandler(6039, "Ivon Coeurlfist", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> IvonCoeurlfistHandler(BattleCharacter c)
        {
            var spiritOrbId = GameObjectManager.GetObjectByNPCId(6665);

            // Require tanks/healers to ignore Spirit Orbs
            // if (!isTank && !isHealer && spiritOrbId != null && spiritOrbId.IsVisible)
            if (spiritOrbId != null && spiritOrbId.IsVisible)
            {
                if (Core.Me.Distance2D(spiritOrbId.Location) > 2f)
                {
                    var spiritOrbPos = GameObjectManager.GetObjectsByNPCId(6665).OrderByDescending(i => i.Distance2D()).First().Location;

                    await CommonTasks.MoveTo(spiritOrbPos);
                    return true;
                }
                return true;
            }

            return false;
        }

        // Coeurl Sruti Tactics
        //
        // Heat Lightning => Purple markers above head. Avoid players by 5y.
        //
        // Arbuda Tactics
        //
        // Verify flanking logic is correct.
        // Hellseals => Move *ONLY* to the "activated" seal.
        // Tapas => SideStep? Avoid multiple circle AoE attacks in quick succession.
        // Killer Instinct => Arbuda assumes a defensive stance, similiar to Ravana. Only one side will be open to attack.
        // --- Attacking any other side will cause him to parry the attack back at you.
        //
        // Ivon Coeurlfist
        //
        // Fix: Move to orbs.
        // Rose of Destruction => Will mark one player with a large yellow arrow marker, requiring the party to stack to split damage.

    }
}
