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
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class ShisuiOfTheVioletTides : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 50;
        public override string Name => @"Shisui of the Violet Tides";
        public override Version Version => new Version(0, 7, 1, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Amikiri", 6237, 0, 41.68f, new Vector3(-0.51f, 18.50f, 62.53f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Ruby Princess", 6241, 1, 37.54f, new Vector3(0.24f, 27.25f, -219.46f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Shisui Yohi", 6243, 2, 40.39f, new Vector3(0.08f, 45.92f, -440.36f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First water pad before Amikiri
            new OffMeshConnection(new Vector3(-14.05f, -3.85f, 231.58f), new Vector3(-27.48f, 4.62f, 219.01f), ConnectionMode.OneWay),
            // Second water pad before Amikiri
            new OffMeshConnection(new Vector3(-26.19f, 6.87f, 138.76f), new Vector3(-14.36f, 15.40f, 122.21f), ConnectionMode.OneWay),
            // First door after Amikiri
            new OffMeshConnection(new Vector3(0.02f, 21.00f, 24.39f), new Vector3(0.14f, 21.00f, 16.98f), ConnectionMode.Bidirectional),
            // Second door after Amikiri - IS THIS ONE NECESSARY?
            // new OffMeshConnection(new Vector3(-0.30f, 26.00f, -17.32f), new Vector3(-0.33f, 26.00f, -22.94f), ConnectionMode.Bidirectional),
            // Third door after Amikiri - IS THIS ONE NECESSARY?
            // new OffMeshConnection(new Vector3(-0.18f, 26.00f, -48.43f), new Vector3(-0.06f, 26.08f, -54.03f), ConnectionMode.Bidirectional),
            // Fourth door after Amikiri
            new OffMeshConnection(new Vector3(0.29f, 26.08f, -79.49f), new Vector3(0.27f, 26.00f, -84.62f), ConnectionMode.Bidirectional),
            // Fifth door after Amikiri
            new OffMeshConnection(new Vector3(-0.08f, 26.08f, -164.67f), new Vector3(-0.12f, 26.00f, -170.05f), ConnectionMode.Bidirectional),
            // Jump after Ruby Princess
            new OffMeshConnection(new Vector3(-50.54f, 25.92f, -208.34f), new Vector3(-56.38f, 21.05f, -208.32f), ConnectionMode.OneWay),
            // First gate after Ruby Princess
            new OffMeshConnection(new Vector3(9.10f, 30.92f, -286.55f), new Vector3(12.69f, 30.92f, -290.17f), ConnectionMode.Bidirectional),
            // Second gate after Ruby Princess
            new OffMeshConnection(new Vector3(-0.07f, 40.92f, -372.00f), new Vector3(-0.05f, 40.92f, -378.00f), ConnectionMode.Bidirectional)
        };

        private uint[] _madStare = new uint[] { 8066 };

        public override void OnEnter()
        {
            // Violet Bombfish
            AddAvoidObject<BattleCharacter>(5, 6335);
        }

        public override void OnExit()
        {

        }

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Naishi-no-jo (Shisui Yohi)
                if (obj.Object.NpcId == 6245)
                    obj.Score += 3000;
                // Naishi-no-Kami (Shisui Yohi)
                if (obj.Object.NpcId == 6244)
                    obj.Score += 2000;
                // Amikiri Leg (Amikiri)
                if (obj.Object.NpcId == 6239)
                    obj.Score += 2000;
                // Churn (Shisui Yohi)
                if (obj.Object.NpcId == 6246)
                    obj.Score += 1000;
                // Kamikiri (Amikiri)
                if (obj.Object.NpcId == 6238)
                    obj.Score += 1000;
            }
        }

        [EncounterHandler(6243, "Shisui Yohi", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> ShisuiYohiHandler(BattleCharacter c)
        {
            while (c.IsCasting && _madStare.Contains(c.CastingSpellId))
            {
                ScriptHelper.LookAway(c);
                await Coroutine.Yield();
            }

            return false;
        }

        // Amikiri Tactics
        // 
        // Digest => Targets three players with blue AoEs (avoid players 5y) which will leave an AoE puddle, which players should avoid standing in (Debuff Aura = 289).
        //
        // Ruby Princess Tactics
        //
        // Seduce => Run into one of the four boxes around the outside of the arena to gain the Old debuff. (Aura = 1259)
        // --- Box ID's?
        // ------ 2008106, 2008107, 2008108, 2008109
        // Abyssal Volcano => A large circular AoE (SideStep?) as well as marking two players with an AoE which will track them. Affected players should run along the edge of the are to avoid this effect as well as avoiding touching the boxes.
        //
        // Shisui Yohi
        //
        // During the Thick Fog phase the boss will move around the arena, look for a large mass of churning ripples. Standing in these bubbles will damage the player. The boss will re-emerge from these ripples. When the boss re-enters the arena after the Churns are destroyed, it will instantly cast Black Tide which will deal a moderate amount damage to any player standing near it and debuff any player hit with Vulnerability Up. Players should pay attention to the location of the ripples and avoid standing in that area of the arena. (15y avoidance)

    }
}
