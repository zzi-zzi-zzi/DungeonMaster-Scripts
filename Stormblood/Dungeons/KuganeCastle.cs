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
using ff14bot.RemoteWindows;

namespace DungeonMasterScripts.Stormblood.Dungeons
{
    public class KuganeCastle : Dungeon
    {
        #region Script Metadata

        public override string Author => @"ZZI & y2krazy";
        public override uint DungeonId => 57;
        public override string Name => @"Kugane Castle";
        public override Version Version => new Version(0, 9, 0, 2);

        #endregion

        public override IEnumerable<Boss> GetAllBosses()
        {
            return new[]
            {
                new Boss("Zuiko-maru", 6085, 0, 25.86f, new Vector3(-136.66f, 0.15f, 0.00f), () => ScriptHelper.IsTodoChecked(1)),
                new Boss("Dojun-maru", 6087, 1, 30.35f, new Vector3(280.00f, -90.85f, 60.00f), () => ScriptHelper.IsTodoChecked(3)),
                new Boss("Yojimbo", 6089, 2, 29.23f, new Vector3(280.00f, -75.86f, 369.40f), () => ScriptHelper.IsTodoChecked(5))
            };
        }

        public override OffMeshConnection[] MeshConnections => new[]
        {
            // First door before Zuiko-maru
            new OffMeshConnection(new Vector3(-18.87668f, -4f, -46.62927f), new Vector3(-18.91334f, -4f, -43.17747f), ConnectionMode.Bidirectional),
            // First door before Zuiko-maru
            new OffMeshConnection(new Vector3(-83.54327f, 3.099442E-06f, 0.003690713f), new Vector3(-87.79295f, 3.099442E-06f, -0.02830664f), ConnectionMode.Bidirectional),
            // Door after Zuiko-maru
            new OffMeshConnection(new Vector3(-179.3789f, 4f, -0.05674365f), new Vector3(-183.9243f, 4f, -0.03990113f), ConnectionMode.Bidirectional),
            // Transition after Zuiko-maru
            new OffMeshConnection(new Vector3(-200.9699f, 5.703516f, 0.07021509f), new Vector3(268.9057f, -98.56416f, -251.3635f), ConnectionMode.Bidirectional),
            // First door before Dojun-maru
            new OffMeshConnection(new Vector3(248.9691f, -94.82437f, -118.3378f), new Vector3(248.9815f, -94.89815f, -114.0891f), ConnectionMode.Bidirectional),
            // Second door before Dojun-maru
            new OffMeshConnection(new Vector3(279.9596f, -92.97342f, 3.073636f), new Vector3(279.9695f, -92.92188f, 7.021797f), ConnectionMode.Bidirectional),
            // Door after Dojun-maru
            new OffMeshConnection(new Vector3(279.7881f, -92.89789f, 93.35789f), new Vector3(279.8008f, -93f, 96.80404f), ConnectionMode.Bidirectional),
            // Door before Yojimbo
            new OffMeshConnection(new Vector3(279.8747f, -74f, 313.1862f), new Vector3(279.8695f, -73.97678f, 317.4336f), ConnectionMode.Bidirectional)
        };

        private uint[] _helmCrack = new uint[] { 12347, 7828 };

        //public override void OnEnter()
        //{
        //    // Inoshikacho (Yojimbo)
        //    AddAvoidObject<BattleCharacter>(8, 6093);
        //}

        //public override void OnExit()
        //{

        //}

        public override void WeighTargetsFilter(List<TargetPriority> objPriorities)
        {
            foreach (var obj in objPriorities)
            {
                // Elite Onmitsu (Dojun-maru)
                if (obj.Object.NpcId == 6088)
                    obj.Score += 1000;
                // Harakiri Kosho
                if (obj.Object.NpcId == 6086)
                    obj.Score += 1000;
                // Harakiri Hanya
                if (obj.Object.NpcId == 6084)
                    obj.Score += 1000;
                // Joi Summoner
                if (obj.Object.NpcId == 6077)
                    obj.Score += 1000;
                // Dragon's Head (Yojimbo)
                if (obj.Object.NpcId == 3305)
                    obj.Score += 1000;
            }
        }

        [EncounterHandler(6085, "Zuiko-maru", 30, CallBehaviorMode.InCombat)]
        public async Task<bool> ZuikomaruHandler(BattleCharacter c)
        {
            if (c.IsCasting && _helmCrack.Contains(c.CastingSpellId))
            {
                if (Core.Me.Distance2D(c) > 5f)
                {
                    await CommonTasks.MoveAndStop(new ff14bot.Pathing.MoveToParameters(c.Location, "Zuiko-maru"), 3f, true);
                    return true;
                }
            }

            return false;
        }
        // Pile of Gold - NpcId2007814 - TODOStep: 5 - TODOValue (Before Pickup): 0 /  1\n
        [ObjectHandler(2007814, ObjectRange = 25)]
        public async Task<bool> PileofGoldHandler(GameObject context)
        {
            // if (ScriptHelper.InCombat())
                // return false;
            if (SelectYesno.IsOpen)
            {
                SelectYesno.ClickYes();
                return true;
            }
            if (context != null && context.IsVisible && context.IsTargetable)
                return await ScriptHelper.ObjectInteraction(context);

            return false;
        }

        // Dojun-maru Tactics
        //
        // Clockwork Medium => Determine what tiles are active to avoid taking damage. AddAvoidLines from spawned NPC's outside the area with 10y(?) width.
        //
        // Yojimbo Tactics
        //
        // Does not properly avoid Inoshikachos. Script-breaking in current form.

    }
}
