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
    public class TheTamTaraDeepcroft : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 2;
        public override string Name => @"The Tam-Tara Deepcroft";
        public override Version Version => new Version(0, 9, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Dalamud Priest #1", 79, 0, 30.37f, new Vector3(-7.88f, 30.83f, -16.25f), () => ScriptHelper.IsTodoChecked(0) && DMDirectorManager.Instance.GetI8D == 0),
                new Boss("Dalamud Priest #2", 79, 1, 26.56f, new Vector3(-23.17f, 24.71f, 20.99f), () => ScriptHelper.IsTodoChecked(0) && DMDirectorManager.Instance.GetI8D == 1),
                new Boss("Cultist Rosary", 2000057, 2, 30f, new Vector3(-179.95f, 14.71f, -5.00f), () => ScriptHelper.IsTodoChecked(0) && DMDirectorManager.Instance.GetI8D == 2),
                new Boss("Octavel the Unforgiving", 119, 3, 17.50f, new Vector3(-95.20f, 14.97f, 3.90f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Galvanth the Dominator", 73, 4, 21.85f, new Vector3(-48.83f, 14.05f, -13.92f), () => ScriptHelper.IsTodoChecked(3))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Wall after Cultist Orb #1
            new OffMeshConnection(new Vector3(10.62f, 29.38f, -10.63f), new Vector3(10.36f, 29.21f, -5.83f), ConnectionMode.Bidirectional),
            // Wall after Cultist Orb #2
            new OffMeshConnection(new Vector3(-16.97f, 23.28f, 38.69f), new Vector3(-20.73f, 23.20f, 41.15f), ConnectionMode.Bidirectional),
            // Bridge before Galvanth the Dominator
            new OffMeshConnection(new Vector3(-85.91f, 15.05f, 5.50f), new Vector3(-68.34f, 15.05f, -4.07f), ConnectionMode.Bidirectional)
        };

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Inconspicuous Imp (Galvanth the Dominator)
                if (obj.Object.NpcId == 456)
                    obj.Score += 3000;
                // Deepcroft Miteling (Galvanth the Dominator)
                if (obj.Object.NpcId == 111)
                    obj.Score += 2000;
                // Void Soulcounter (Dalamud Priest)
                if (obj.Object.NpcId == 455)
                    obj.Score += 1000;
                // Octavel the Unforgiving
                if (obj.Object.NpcId == 119)
                    obj.Score += 1000;
                // Skeleton Soldier (Galvanth the Dominator)
                if (obj.Object.NpcId == 110)
                    obj.Score += 1000;
                // Dalamud Priest
                if (obj.Object.NpcId == 79)
                    obj.Score += 1000;
            }
        }

        // Cultist Orb - NpcId2000061 - TODOStep: 0 - TODOValue (Before Pickup): 0 /  4
        // Cultist Orb - NpcId2000062 - TODOStep: 0 - TODOValue (Before Pickup): 1 /  4\n
        [ObjectHandler(2000061, ObjectRange = 25)]
		[ObjectHandler(2000062, ObjectRange = 25)]
        public async Task<bool> CultistOrbHandler(GameObject context)
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
        // Cultist Rosary - NpcId2000057 - TODOStep: 0 - TODOValue (Before Pickup): 2 /  4\n
		[ObjectHandler(2000057, ObjectRange = 25)]
        public async Task<bool> CultistRosaryHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (!ScriptHelper.IsTodoChecked(1) && !ScriptHelper.HasKeyItem(2000244))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Sealed Barrier - NpcId2000060 - TODOStep: 0 - TODOValue (Before Pickup): 2 /  4\n
		[ObjectHandler(2000060, ObjectRange = 25)]
        public async Task<bool> SealedBarrierHandler(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.IsTodoChecked(1))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }
        // Cultist Orb - NpcId2000063 - TODOStep: 0 - TODOValue (Before Pickup): 2 /  4
        // Cultist Orb - NpcId2000067 - TODOStep: 0 - TODOValue (Before Pickup): 3 /  4\n
		[ObjectHandler(2000063, ObjectRange = 25)]
		[ObjectHandler(2000067, ObjectRange = 25)]
        public async Task<bool> CultistOrbHandler2(GameObject context)
        {
            if (ScriptHelper.InCombat())
                return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (ScriptHelper.IsTodoChecked(1))
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }

        // TO DO
        //
        // Determine why it's trying to skip Dalamud Priest #2.

    }
}