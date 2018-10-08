using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;

namespace TBAGW
{
    static public class EncounterInfo
    {
        static public bool bBattleWon = false;
        static public bool bIsRunning = false;
        static public List<TurnSet> encounterGroups = new List<TurnSet>();
        static public EncounterObjective objective = new EncounterObjective();
        /// <summary>
        /// Represents the current party active from the encounterGroups, it's the encounterGroups index
        /// </summary>
        static public int currentGroupIndex = 0;
        static public String currentGroupName = "";
        internal static bool bGenerateLuaTurnInfo = true;
        internal static LUA.LuaTurnSetInfo turnSetInfo;
        static internal List<DangerTile> dangerTiles = new List<DangerTile>();

        static public void ChangeGroup(int i)
        {
            currentGroupIndex = i;


            if (encounterGroups[currentGroupIndex].bIsPlayerTurnSet)
            {
                currentGroupName = "Player Turn";
            }
            else if (encounterGroups[currentGroupIndex].bIsEnemyTurnSet)
            {
                currentGroupName = "Enemy Turn";
            }
            else if (encounterGroups[currentGroupIndex].bIsAssistTurnSet)
            {
                currentGroupName = "Assist group Turn";
            }

            encounterGroups[currentGroupIndex].Start();
        }

        static public void ChangeGroup()
        {
            RoundEndLogic(encounterGroups[currentGroupIndex]);
            if (currentGroupIndex < encounterGroups.Count - 1)
            {
                currentGroupIndex++;
                TurnSet.bFirstTurn = true;
            }
            else
            {
                currentGroupIndex = 0;
            }

            if (encounterGroups[currentGroupIndex].bIsPlayerTurnSet)
            {
                currentGroupName = "Player Turn";
                BattleGUI.TurnEffectsList[(int)BattleGUI.TurnEffects.PlayerTurn].ShowEffect();
            }
            else if (encounterGroups[currentGroupIndex].bIsEnemyTurnSet)
            {
                currentGroupName = "Enemy Turn";
                BattleGUI.TurnEffectsList[(int)BattleGUI.TurnEffects.EnemyTurn].ShowEffect();
            }
            else if (encounterGroups[currentGroupIndex].bIsAssistTurnSet)
            {
                currentGroupName = "Assist group Turn";
                if (encounterGroups[currentGroupIndex].charactersInGroup.Count != 0)
                {
                    BattleGUI.TurnEffectsList[(int)BattleGUI.TurnEffects.AssistTurn].ShowEffect();
                }
            }

            RoundStartLogic(encounterGroups[currentGroupIndex]);
            encounterGroups[currentGroupIndex].Start();
        }

        internal static void TurnEffectCompletedAfterChangeTurn()
        {
            for (int i = 0; i < dangerTiles.Count; i++)
            {
                if (dangerTiles[i].IsReady())
                {
                    dangerTiles[i].Execute();
                }
            }

            dangerTiles.RemoveAll(dt => dt.Remove());
        }

        private static void RoundEndLogic(TurnSet turnSet)
        {

        }

        private static void RoundStartLogic(TurnSet turnSet)
        {
            for (int i = 0; i < dangerTiles.Count; i++)
            {
                dangerTiles[i].Tick(currentGroupIndex);
            }

            foreach (var character in turnSet.charactersInGroup)
            {
                if (character.CCC == null)
                {
                    Console.WriteLine("ERROR: Missing CCC in " + character);
                    character.ReloadFromDatabase(GameProcessor.gcDB);
                }
                character.ProcessTurn();
            }

            if (turnSet.bIsPlayerTurnSet)
            {

            }
            else if (turnSet.bIsEnemyTurnSet)
            {

            }

            ClearDeathChars();
        }

        static public void ClearDeathChars()
        {
            foreach (var group in EncounterInfo.encounterGroups)
            {
                var temp = group.groupTurnSet.Find(gts => !gts.character.IsAlive());
                if (temp != null)
                {
                    temp.character.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] = 0;
                }
                if (temp != null&&CombatProcessor.heroCharacters.Contains(temp.character))
                {
                    CombatProcessor.heroCharacters.Remove(temp.character);
                }
                group.groupTurnSet.RemoveAll(gts => !gts.character.IsAlive());
                group.charactersInGroup.RemoveAll(c => !c.IsAlive());
                CombatProcessor.encounterEnemies.RemoveAll(c => !c.IsAlive());
                if (temp != null && PlayerController.selectedSprite == temp.character)
                {
                    PlayerController.selectedSprite = PlayerSaveData.heroParty.Find(h => h.IsAlive());
                }
            }
        }

        static public void Start(List<TurnSet> encounterGs)
        {
            encounterGroups = encounterGs;
            foreach (var item in encounterGroups)
            {
                item.GenerateTurns();
            }
            bBattleWon = false;
            bIsRunning = true;
            BattleGUI.TurnEffectsList[(int)BattleGUI.TurnEffects.PlayerTurn].bMustShow = true;
            Reset();
        }

        internal static void AddDangerTile(DangerTile dt)
        {
            
            dangerTiles.Add(dt);
        }

        private static void Reset()
        {
            bGenerateLuaTurnInfo = true;
            dangerTiles.Clear();
            DangerTile.Reset();
        }

        static public void Update(GameTime gt)
        {
            bBattleWon = objective.ObjectiveReached(encounterGroups);
            bIsRunning = bBattleWon;

            if (EncounterObjective.Lost(objective))
            {
                CombatProcessor.bLostBattle = true;
            }


            if (bBattleWon && !CombatProcessor.bAfterBattleScreen)
            {
                CombatProcessor.InitiateVictory();
            }
            else if (!bBattleWon)
            {
                LUA.LuaBScriptEvent.msTime = gt.ElapsedGameTime.Milliseconds;
                if (bGenerateLuaTurnInfo)
                {
                    turnSetInfo = currentTurn().toLuaTurnSetInfo();
                    bGenerateLuaTurnInfo = false;
                }
                BattleScriptHandler.Execute(LUA.LuaBScriptEvent.EventType.updateEV, turnSetInfo);
                encounterGroups[currentGroupIndex].Update();
            }

        }

        static public TurnSet currentTurn()
        {
            return encounterGroups[currentGroupIndex];
        }

        static public void UpdateAllStats()
        {
            foreach (var item in encounterGroups)
            {
                foreach (var gts in item.groupTurnSet)
                {
                    gts.character.UpdateStats(gts.character.trueSTATChart());
                }
            }
        }

        internal static List<TurnSet> friendlyTurnSetsFor(BaseCharacter bc)
        {
            List<TurnSet> enemies = new List<TurnSet>();
            foreach (var group in encounterGroups)
            {
                if (group.charactersInGroup.Contains(bc))
                {
                    enemies = group.targetGroups;
                }
            }

            var temp = encounterGroups.FindAll(eg => !enemies.Contains(eg));
            return temp;
        }

        internal static bool AreEnemies(BaseCharacter caster, BaseCharacter target)
        {
            if (caster == target || target == null)
            {
                return false;
            }

            var casterGroup = encounterGroups.Find(group=>group.charactersInGroup.Contains(caster));
            var targetGroup = encounterGroups.Find(group => group.charactersInGroup.Contains(target));

            return casterGroup.targetGroups.Contains(targetGroup);
        }

        internal static TurnSet getTurnSetFrom(BaseCharacter bc)
        {
            foreach (var turnset in encounterGroups)
            {
                foreach (var c in turnset.charactersInGroup)
                {
                    if (c==bc)
                    {
                        return turnset;
                    }
                }
            }

            return null;
        }

        internal static CharacterTurn getCharacterTurnFrom(BaseCharacter bc)
        {
            foreach (var turnset in encounterGroups)
            {
                foreach (var c in turnset.groupTurnSet)
                {
                    if (c.character == bc)
                    {
                        return c;
                    }
                }
            }

            return null;
        }
    }
}
