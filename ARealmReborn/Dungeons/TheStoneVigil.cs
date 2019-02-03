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
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.ARealmReborn.Dungeons
{
    public class TheStoneVigil : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 11;
        public override string Name => @"The Stone Vigil";
        public override Version Version => new Version(0, 7, 0, 0);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Chudo-Yudo", 1677, 0, 24.84f, new Vector3(0.00f, 0.01f, 107.18f), () => ScriptHelper.IsTodoChecked(0)),
                new Boss("Koshchei", 1678, 1, 30.59f, new Vector3(54.67f, 5.02f, -79.94f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Isgebind", 1680, 2, 36.86f, new Vector3(-0.02f, 0.06f, -262.78f), () => ScriptHelper.IsTodoChecked(2))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // Jump before Chudo-Yudo
            new OffMeshConnection(new Vector3(-0.02f, 4.10f, 137.65f), new Vector3(-0.02f, 0.03f, 131.92f), ConnectionMode.OneWay),
            // Fire wall after Chudo-Yudo
            new OffMeshConnection(new Vector3(-0.25f, -0.05f, -85.71f), new Vector3(-0.19f, 0.00f, -94.20f), ConnectionMode.Bidirectional),
            // Gate before Isgebind
            new OffMeshConnection(new Vector3(-0.08f, 4.00f, -214.43f), new Vector3(-0.12f, 4.00f, -217.93f), ConnectionMode.Bidirectional),
            // Jump before Isgebind
            new OffMeshConnection(new Vector3(-0.06f, 4.47f, -222.07f), new Vector3(-0.05f, 0.36f, -226.45f), ConnectionMode.OneWay)
        };

        private uint[] _rimeWreath = new uint[] { 9589, 3920, 3448, 1025 };

        public override void OnEnter()
        {
            // Rime Wreath (Isgebind)
            AddAvoidObject<BattleCharacter>(
                () => GameObjectManager.GetObjectsOfType<BattleCharacter>().Any(i => i.IsCasting && _rimeWreath.Contains(i.CastingSpellId)),
                20,
                x => x.NpcId == 1680,
                x => x.Location
                );
            // Sheet of Ice (Isgebind)
            AddAvoidObject<EventObject>(8, 2000611);
        }

        public override void OnExit()
        {

        }

        // Strongroom Gate - NpcId2001880 - TODOStep: 2 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2001880, ObjectRange = 25)]
        public async Task<bool> StrongroomGateHandler(GameObject context)
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
        [EncounterHandler(1677, "Chudo-Yudo", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> ChudoYudoHandler(BattleCharacter c)
        {
            var isgebindId = GameObjectManager.GetObjectByNPCId(1680);
            var berthaLeftId = GameObjectManager.GetObjectByNPCId(2001884);
            var berthaCenterId = GameObjectManager.GetObjectByNPCId(2001885);
            var berthaRightId = GameObjectManager.GetObjectByNPCId(2001886);

            if ((berthaLeftId != null && berthaLeftId.IsVisible) && (berthaCenterId != null && berthaCenterId.IsVisible) && (berthaRightId != null && berthaRightId.IsVisible))
            {
                var berthaLeftDistance = berthaLeftId.Location.Distance(isgebindId.Location);
                var berthaCenterDistance = berthaCenterId.Location.Distance(isgebindId.Location);
                var berthaRightDistance = berthaRightId.Location.Distance(isgebindId.Location);

                if (berthaLeftDistance > berthaCenterDistance && berthaLeftDistance > berthaRightDistance)
                {
                    if (Core.Me.Distance2D(berthaLeftId.Location) > 3f)
                    {
                        await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(berthaLeftId.Location, "Left Bertha"), 2f, true);
                        return true;
                    }
                    berthaLeftId.Interact();
                    return true;
                }
                if (berthaCenterDistance > berthaLeftDistance && berthaCenterDistance > berthaRightDistance)
                {
                    if (Core.Me.Distance2D(berthaCenterId.Location) > 3f)
                    {
                        await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(berthaCenterId.Location, "Center Bertha"), 2f, true);
                        return true;
                    }
                    berthaCenterId.Interact();
                    return true;
                }
                if (berthaRightDistance > berthaLeftDistance && berthaRightDistance > berthaCenterDistance)
                {
                    if (Core.Me.Distance2D(berthaRightId.Location) > 3f)
                    {
                        await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(berthaRightId.Location, "Right Bertha"), 2f, true);
                        return true;
                    }
                    berthaRightId.Interact();
                    return true;
                }
            }

            return false;
        }

        // Chudo-Yudo Tactics
        //
        // The Lion's Breath => Frontal AoE attack. Tank boss facing away from group.
        // Swinge => Frontal AoE attack. Move behind or flank the boss.
        //
        // Koshchei Tactics
        //
        // Blazing Trail => Frontal AoE attack. Tank boss facing away from group.
        // Typhoon => A tornado drops around the area. Avoid by 8y(?).
        //
        // Isgebind Tactics
        //
        // Frost Breath => Frontal AoE attack. Tank boss facing away from group.
        // --- Move to flank of boss if in front.
        // Cauterize => Boss flies in the air, then reappears at the side of the room and does a "breath" attack covering one third of the room.
        // Touchdown => Avoid tank by 10y while boss is not targetable to avoid damage when the boss comes to the ground.

    }
}
