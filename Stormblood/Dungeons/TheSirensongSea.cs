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
using DungeonMaster.Helpers;
using ff14bot.Managers;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class TheSirensongSea : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 52;
        public override string Name => @"The Sirensong Sea";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Lugat", 6071, 0, 30.01f, new Vector3(-2.70f, 2.90f, -208.00f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("The Governor", 6072, 1, 26.80f, new Vector3(-8.00f, 4.45f, 88.00f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Lorelei", 6074, 2, 26.73f, new Vector3(-44.50f, 7.75f, 471.63f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Jump off boat to the shore
            new OffMeshConnection(new Vector3(0.0494822f, 29.1945f, -381.861f), new Vector3(-1.070798f, 21.01823f, -372.6393f), ConnectionMode.OneWay),
            // Cross risen ship to Lugat
            new OffMeshConnection(new Vector3(4.360327f, 2.655678f, -270.7963f), new Vector3(0.8407645f, 2.561042f, -248.1174f), ConnectionMode.Bidirectional),
            // Jump on to the 1st boat before The Governor
            new OffMeshConnection(new Vector3(-69.83324f, 14.22266f, -31.12852f), new Vector3(-76.67336f, 12.47955f, -27.36099f), ConnectionMode.OneWay),
            // Jump on to the 2nd boat before The Governor
            new OffMeshConnection(new Vector3(-59.44104f, 20.22225f, 7.721699f), new Vector3(-56.32729f, 18.35015f, 14.62255f), ConnectionMode.OneWay),
            // 1st door after The Governor
            new OffMeshConnection(new Vector3(-8.834143f, 2.165961f, 112.7633f), new Vector3(-9.886842f, 1.907925f, 117.8027f), ConnectionMode.Bidirectional),
            // 2nd door after The Governor
            new OffMeshConnection(new Vector3(-13.42328f, 0.8221986f, 133.8056f), new Vector3(-14.37624f, 0.6482997f, 139.982f), ConnectionMode.Bidirectional),
            // 3rd door after The Governor
            new OffMeshConnection(new Vector3(12.65444f, -3.546474f, 241.0676f), new Vector3(12.58741f, -3.639585f, 246.9176f), ConnectionMode.Bidirectional),
            // Door before Lorelei
            new OffMeshConnection(new Vector3(-43.89697f, 7.375561f, 413.4292f), new Vector3(-44.31162f, 7.751198f, 420.8968f), ConnectionMode.Bidirectional)
        };

        // TO DO
        //
        // Verify OMC's for jumping across ships are all there.
        //
        // Lugat Tactics
        //
        // Hydroball (XXXX) => One party member is targeted. Stack to split the damage.
        //
        // The Governor Tactics
        //
        // Shadowflow => SideStep? Radial AoE with gaps.
        // Enter Night (XXXX) => Debuff (XXXX) => Run away from boss to break link.
        // Shadowsplit => SideStep? Radial AoE with gaps.
        //
        // Lorelei Tactics
        //
        // Virgin Tears => SideStep? Blue puddles around the room. Applies Bleed debuff.
        // Morbid Advance => Force your character to move forward based on character's facing direction.
        // --- Position yourself accordingly so that you avoid Virgin Tears as much as possible.
        // Morbid Retreat => Force your character to move backward based on character's facing direction.
        // --- Position yourself accordingly so that you avoid Virgin Tears as much as possible.

    }
}
