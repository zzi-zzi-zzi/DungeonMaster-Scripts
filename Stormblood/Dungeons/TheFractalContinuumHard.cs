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
using DungeonMaster.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class TheFractalContinuumHard : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 60;
        public override string Name => @"The Fractal Continuum (Hard)";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Motherbit", 7056, 0, 17.59f, new Vector3(-0.02f, 46.60f, -350.03f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("The Ultima Warrior", 7055, 1, 20.99f, new Vector3(-342.40f, -13.82f, 251.70f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("The Ultima Beast", 7058, 2, 25.50f, new Vector3(-2.09f, 0.00f, 291.28f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // 1st "arrow" jump (forward only)
            new OffMeshConnection(new Vector3(-146.9353f, 53.46046f, -368.7611f), new Vector3(-78.26355f, 43.38135f, -204.5472f), ConnectionMode.OneWay),
            // 1st "arrow" jump (backward only)
            new OffMeshConnection(new Vector3(-83.0849f, 42.23291f, -198.9105f), new Vector3(-149.4316f, 53.11658f, -378.1033f), ConnectionMode.OneWay),
            // 2nd "arrow" jump (forward only)
            new OffMeshConnection(new Vector3(57.53016f, 53.37439f, -213.4186f), new Vector3(-1.480164f, 46.58582f, -325.7039f), ConnectionMode.OneWay),
            // Door before The Ultima Warrior
            new OffMeshConnection(new Vector3(-307.8003f, 0.0006048679f, 92.27423f), new Vector3(-311.1768f, 1.192093E-07f, 88.79391f), ConnectionMode.Bidirectional),
            // Fall before Ultima Warrior (forward only)
            new OffMeshConnection(new Vector3(-290.6764f, 1.192093E-07f, 192.2459f), new Vector3(-291.7563f, -13.74546f, 198.4772f), ConnectionMode.OneWay),
            // Zone transition line after Ultima Warrior room
            new OffMeshConnection(new Vector3(-402.4131f, -14f, 249.7367f), new Vector3(-0.102467f, 25.51709f, 198.6034f), ConnectionMode.Bidirectional),
            // Broken glass wall before The Ultima Beast
            new OffMeshConnection(new Vector3(76.92653f, 20f, 243.69f), new Vector3(80.32335f, 20f, 247.0809f), ConnectionMode.Bidirectional),
            // 1st Security Terminal door before The Ultima Beast
            new OffMeshConnection(new Vector3(69.51707f, 11.99997f, 332.3887f), new Vector3(64.59916f, 12f, 330.0424f), ConnectionMode.Bidirectional),
            // 2nd Security Terminal door before The Ultima Beast
            new OffMeshConnection(new Vector3(-0.007072094f, 12f, 372.1372f), new Vector3(-0.04300446f, 12f, 368.1357f), ConnectionMode.Bidirectional)
        };

        // Allagan Teleporter - NpcId2009278 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
		[ObjectHandler(2009278, ObjectRange = 25)]
        public async Task<bool> AllaganTeleporterHandler(GameObject context)
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
        // Security Terminal - NpcId2009276 - TODOStep: 4 - TODOValue (Before Pickup): 0 /  1
        // Security Terminal - NpcId2009277 - TODOStep: 4 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2009276, ObjectRange = 25)]
        [ObjectHandler(2009277, ObjectRange = 25)]
        public async Task<bool> SecurityTerminalHandler(GameObject context)
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

        // Motherbit Tactics
        //
        // Allagan Gravity => Circle AoE attack. Leaves behind a puddle that will pull you in if too close.
        // Citadel Buster => When Prototype Bits (7057) form a line, get behind them.
        //
        // The Ultima Warrior Tactics
        //
        // Mass Aetheroplasm => Stack on player with stack indicator to spread damage.
        // Citadel Buster => Targets random player.
        // Sophia => Stack black & white markers together.
        // Zurvan => Players affected by fire (10141 or 7770 or 7271?) or ice (10142 or 7771 or 7272?) must stand in appropriate marker. Fire with fire and ice with ice.
        //
        // The Ultima Beast Tactics
        //
        // Death Spin => Tank facing away from the party.
        // Aether Bend => Hits anyone not inside melee range. Move near Boss.
        // Flare Star => Drops Fire Puddles. Avoid by 8y(?).
        // Allagan Gravity => Spread out from others and move away from gravity puddles after.
        // Light Pillar => Expanding AoE from center. Dodge as it moves across platform.

    }
}
