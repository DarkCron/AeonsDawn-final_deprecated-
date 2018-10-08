using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.AI;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;


namespace TBAGW
{
    public class TurnSet
    {
        public List<CharacterTurn> groupTurnSet = new List<CharacterTurn>();
        public bool bIsPlayerTurnSet = false;
        public bool bIsEnemyTurnSet = false;
        public bool bIsAssistTurnSet = false;
        public bool bPlayerMustSelectAction = false;
        public List<BaseCharacter> charactersInGroup = new List<BaseCharacter>();
        public MapZone zone;
        List<BasicTile> zoneTiles;
        public bool bIsCompleted = false;
        public CharacterTurn selectedCharTurn = null;
        public CharacterTurn currentEnemy = null;
        static public bool bFirstTurn = true;
        public bool bRegenerateRadius = false;
        public List<TurnSet> targetGroups = new List<TurnSet>();
        public bool bPlayerTurnEnemyOverriden = false;


        public bool bTest = false;
        BaseCharacter AITarget = null;
        KeyValuePair<BasicTile, BasicAbility> AIbtba = default(KeyValuePair<BasicTile, BasicAbility>);
        internal static bool bSelectingArea = false;

        public TurnSet(List<BaseCharacter> chars, MapZone zone, List<BasicTile> zoneTiles, bool bPlayerGroup = false)
        {
            bIsPlayerTurnSet = bPlayerGroup;
            charactersInGroup = new List<BaseCharacter>(chars);
            this.zone = zone;
            this.zoneTiles = zoneTiles;
        }

        public void Start()
        {
            bIsCompleted = true;
            selectedCharTurn = null;
            GenerateTurns();
            BattleGUI.UpdateGUIElements();

            if (bIsPlayerTurnSet)
            {
                BattleScriptHandler.Execute(LUA.LuaBScriptEvent.EventType.startPT, this.toLuaTurnSetInfo());
            }
            else
            {
                BattleScriptHandler.Execute(LUA.LuaBScriptEvent.EventType.startET, this.toLuaTurnSetInfo());
            }

            PlayerController.previousSelected = null;
            groupTurnSet.ForEach(gts => gts.stepsSet = 0);
            EncounterInfo.UpdateAllStats();
            selectedCharTurn = null;
            bFirstTurn = false;
            PlayerController.selectedSprite = null;

        }

        public void Update()
        {
            var gts = groupTurnSet.Find(g => !g.bIsCompleted);
            //CombatProcessor.encounterEnemies

            if (bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden && !BattleGUI.bIsRunning && !bSelectingArea)
            {
                if (!KeyboardMouseUtility.AnyButtonsPressed() && Keyboard.GetState().IsKeyDown(Keys.X))
                {
                    var temp = EncounterInfo.encounterGroups.FindAll(eg => eg.bIsPlayerTurnSet);
                    foreach (var item in temp)
                    {
                        item.bIsCompleted = true;
                    }
                }

                if (EncounterInfo.currentTurn().selectedCharTurn != null && bRegenerateRadius)
                {
                    RegenerateRadius();
                }

                currentEnemy = null;


                if (selectedCharTurn != null && !selectedCharTurn.bIsCompleted)
                {
                    var tempTile = selectedCharTurn.returnCompleteArea().Find(t => t.mapPosition.Contains(GameProcessor.EditorCursorPos));
                    if (tempTile != null)
                    {
                        if (CombatArrowLayout.bShouldRecheck(tempTile))
                        {
                            CombatArrowLayout.Start(tempTile, selectedCharTurn);
                        }
                    }
                    else
                    {
                        CombatArrowLayout.bMustDraw = false;
                        CombatArrowLayout.Clear();
                    }
                }
                else
                {
                    if (CombatArrowLayout.CanClear())
                    {
                        CombatArrowLayout.Clear();
                    }
                }


            }
            else if (bIsPlayerTurnSet && BattleGUI.bIsRunning)
            {

            }
            else if (bIsEnemyTurnSet || EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden)
            {
                UpdateEnemyLogic(gts);
            }

            if (bSelectingArea)
            {

            }

            if (gts == null)
            {
                bIsCompleted = true;
            }

            if (bIsCompleted && !bPlayerTurnEnemyOverriden && !LUA.LuaExecutionList.DemandOverride() && !PathMoveHandler.bIsBusy && BattleGUI.TurnEffectsList.Find(te => te.bMustShow) == default(ChangeTurnEffect))
            {

                ResetAndChangeGroup();
            }
        }

        private void RegenerateRadius()
        {
            int radius = selectedCharTurn.character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB] * selectedCharTurn.character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP] - (EncounterInfo.currentTurn().selectedCharTurn.stepsSet - 2);

            switch (selectedCharTurn.character.rotationIndex)
            {
                case (int)BaseCharacter.Rotation.Up:
                    CombatProcessor.radiusTiles = MapListUtility.returnValidMapRadius(radius, EncounterInfo.currentTurn().selectedCharTurn.returnCompleteArea(), selectedCharTurn.character.position - new Vector2(0, 64));
                    CombatProcessor.radiusTiles.RemoveAll(t => t.distanceCoordsFrom(selectedCharTurn.character.position) > radius - 2);
                    break;
                case (int)BaseCharacter.Rotation.Down:
                    CombatProcessor.radiusTiles = MapListUtility.returnValidMapRadius(radius, EncounterInfo.currentTurn().selectedCharTurn.returnCompleteArea(), selectedCharTurn.character.position + new Vector2(0, 64));
                    CombatProcessor.radiusTiles.RemoveAll(t => t.distanceCoordsFrom(selectedCharTurn.character.position + new Vector2(0, 64)) > radius - 2);
                    break;
                case (int)BaseCharacter.Rotation.Right:
                    CombatProcessor.radiusTiles = MapListUtility.returnValidMapRadius(radius, EncounterInfo.currentTurn().selectedCharTurn.returnCompleteArea(), selectedCharTurn.character.position + new Vector2(64, 0));

                    CombatProcessor.radiusTiles.RemoveAll(t => t.distanceCoordsFrom(selectedCharTurn.character.position + new Vector2(64, 0)) > radius - 2);
                    break;
                case (int)BaseCharacter.Rotation.Left:
                    CombatProcessor.radiusTiles = MapListUtility.returnValidMapRadius(radius, EncounterInfo.currentTurn().selectedCharTurn.returnCompleteArea(), selectedCharTurn.character.position - new Vector2(64, 0));
                    CombatProcessor.radiusTiles.RemoveAll(t => t.distanceCoordsFrom(selectedCharTurn.character.position) > radius - 2);
                    break;
            }

            //CombatProcessor.radiusTiles.RemoveAll(t => t.distanceCoordsFrom(selectedCharTurn.character.position) > radius - 1);
            bRegenerateRadius = false;
        }

        public void TabSelect()
        {
            if (!bPlayerMustSelectAction && !PathMoveHandler.bIsBusy)
            {
                if (EncounterInfo.currentTurn().selectedCharTurn != null)
                {
                    PlayerController.previousSelected = EncounterInfo.currentTurn().selectedCharTurn.character;
                }
                else
                {
                    PlayerController.previousSelected = null;
                }

                //   selectedCharTurn = groupTurnSet.Find(sct => sct.character == CombatProcessor.heroCharacters.Find(c => c.spriteGameSize.Contains(GameProcessor.EditorCursorPos)) && !sct.bIsCompleted);
                int index = 0;
                var availableChars = groupTurnSet.FindAll(ct => !ct.bIsCompleted);
                if (availableChars.Count == 0)
                {
                    selectedCharTurn = availableChars[index];
                }
                else
                {
                    index = availableChars.IndexOf(EncounterInfo.currentTurn().selectedCharTurn);
                    if (index == -1)
                    {
                        index = 0;
                    }
                    else if (index + 1 != availableChars.Count)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }

                    selectedCharTurn = availableChars[index];
                }


                var v = PlayerController.originalPositionCharacter;
                if (selectedCharTurn != null && !selectedCharTurn.bIsCompleted && selectedCharTurn.character != PlayerController.selectedSprite || (bFirstTurn && selectedCharTurn != null))
                {

                    var selectedChar = EncounterInfo.currentTurn().selectedCharTurn.character;
                    if (PlayerController.previousSelected != null && !EncounterInfo.currentTurn().groupTurnSet.Find(gts2 => gts2.character == PlayerController.previousSelected).bIsCompleted && !bFirstTurn)
                    {
                        //PlayerController.previousSelected.position = PlayerController.originalPositionCharacter;
                        PlayerController.previousSelected.changePosition(PlayerController.originalPositionCharacter);
                        groupTurnSet.Find(sct => sct.character == PlayerController.previousSelected).stepsSet = 0;
                    }
                    PlayerController.selectedSprite = selectedChar;
                    GameProcessor.cameraFollowTarget = selectedChar;
                    bFirstTurn = false;
                    PlayerController.originalPositionCharacter = selectedChar.position;
                    //   GameProcessor.sceneCamera = ((new Vector2(-(selectedChar.position.X + 32 - 1366 / GameProcessor.zoom / 2), -(selectedChar.position.Y + 32 - 768 / GameProcessor.zoom / 2))));
                    ReGenerateTurn();
                }
                if (selectedCharTurn != null && selectedCharTurn.bIsCompleted)
                {
                    var selectedChar = EncounterInfo.currentTurn().selectedCharTurn.character;
                    // GameProcessor.sceneCamera = ((new Vector2(-(selectedChar.position.X + 32 - 1366 / GameProcessor.zoom / 2), -(selectedChar.position.Y + 32 - 768 / GameProcessor.zoom / 2))));
                }
                if (selectedCharTurn == null)
                {
                    if (PlayerController.previousSelected != null && !EncounterInfo.currentTurn().groupTurnSet.Find(gts2 => gts2.character == PlayerController.previousSelected).bIsCompleted && !bFirstTurn)
                    {
                        PlayerController.previousSelected.changePosition(PlayerController.originalPositionCharacter);
                        groupTurnSet.Find(sct => sct.character == PlayerController.previousSelected).stepsSet = 0;
                    }
                    // PlayerController.selectedSprite.position = PlayerController.originalPositionCharacter;

                    bFirstTurn = true;
                    GameProcessor.cameraFollowTarget = null;
                }
            }
        }

        public void SelectCharacter()
        {
            #region Left button logic
            if (!bPlayerMustSelectAction && !PathMoveHandler.bIsBusy)
            {
                if (EncounterInfo.currentTurn().selectedCharTurn != null)
                {
                    PlayerController.previousSelected = EncounterInfo.currentTurn().selectedCharTurn.character;
                }
                else
                {
                    PlayerController.previousSelected = null;
                }

                selectedCharTurn = groupTurnSet.Find(sct => sct.character == charactersInGroup.Find(c => c.spriteGameSize.Contains(GameProcessor.EditorCursorPos)) && !sct.bIsCompleted);
                var v = PlayerController.originalPositionCharacter;
                if (selectedCharTurn != null && !selectedCharTurn.bIsCompleted && selectedCharTurn.character != PlayerController.selectedSprite || (bFirstTurn && selectedCharTurn != null))
                {

                    var selectedChar = EncounterInfo.currentTurn().selectedCharTurn.character;
                    if (PlayerController.previousSelected != null && !EncounterInfo.currentTurn().groupTurnSet.Find(gts2 => gts2.character == PlayerController.previousSelected).bIsCompleted && !bFirstTurn)
                    {
                        //PlayerController.previousSelected.position = PlayerController.originalPositionCharacter;
                        PlayerController.previousSelected.changePosition(PlayerController.originalPositionCharacter);
                        groupTurnSet.Find(sct => sct.character == PlayerController.previousSelected).stepsSet = 0;
                    }
                    PlayerController.selectedSprite = selectedChar;
                    GameProcessor.cameraFollowTarget = selectedChar;
                    bFirstTurn = false;
                    PlayerController.originalPositionCharacter = selectedChar.position;
                    GameProcessor.sceneCamera = ((new Vector2(-(selectedChar.position.X + 32 - 1366 / GameProcessor.zoom / 2), -(selectedChar.position.Y + 32 - 768 / GameProcessor.zoom / 2))));
                    ReGenerateTurn();
                }
                if (selectedCharTurn != null && selectedCharTurn.bIsCompleted)
                {
                    var selectedChar = EncounterInfo.currentTurn().selectedCharTurn.character;
                    GameProcessor.sceneCamera = ((new Vector2(-(selectedChar.position.X + 32 - 1366 / GameProcessor.zoom / 2), -(selectedChar.position.Y + 32 - 768 / GameProcessor.zoom / 2))));
                }
                if (selectedCharTurn == null)
                {
                    if (PlayerController.previousSelected != null && !EncounterInfo.currentTurn().groupTurnSet.Find(gts2 => gts2.character == PlayerController.previousSelected).bIsCompleted && !bFirstTurn)
                    {
                        PlayerController.previousSelected.changePosition(PlayerController.originalPositionCharacter);
                        groupTurnSet.Find(sct => sct.character == PlayerController.previousSelected).stepsSet = 0;
                    }
                    // PlayerController.selectedSprite.position = PlayerController.originalPositionCharacter;

                    bFirstTurn = true;
                    GameProcessor.cameraFollowTarget = null;
                }


            }
            #endregion
        }

        public void SelectCharacter(BaseCharacter c)
        {
            #region Left button logic
            if (!bPlayerMustSelectAction && !PathMoveHandler.bIsBusy)
            {
                if (EncounterInfo.currentTurn().selectedCharTurn != null)
                {
                    PlayerController.previousSelected = EncounterInfo.currentTurn().selectedCharTurn.character;
                }
                else
                {
                    PlayerController.previousSelected = null;
                }

                selectedCharTurn = groupTurnSet.Find(sct => sct.character == c && !sct.bIsCompleted);
                var v = PlayerController.originalPositionCharacter;
                if (selectedCharTurn != null && !selectedCharTurn.bIsCompleted && selectedCharTurn.character != PlayerController.selectedSprite || (bFirstTurn && selectedCharTurn != null))
                {

                    var selectedChar = EncounterInfo.currentTurn().selectedCharTurn.character;
                    if (PlayerController.previousSelected != null && !EncounterInfo.currentTurn().groupTurnSet.Find(gts2 => gts2.character == PlayerController.previousSelected).bIsCompleted && !bFirstTurn)
                    {
                        //PlayerController.previousSelected.position = PlayerController.originalPositionCharacter;
                        PlayerController.previousSelected.changePosition(PlayerController.originalPositionCharacter);
                        groupTurnSet.Find(sct => sct.character == PlayerController.previousSelected).stepsSet = 0;
                    }
                    PlayerController.selectedSprite = selectedChar;
                    GameProcessor.cameraFollowTarget = selectedChar;
                    bFirstTurn = false;
                    PlayerController.originalPositionCharacter = selectedChar.position;
                    GameProcessor.sceneCamera = ((new Vector2(-(selectedChar.position.X + 32 - 1366 / GameProcessor.zoom / 2), -(selectedChar.position.Y + 32 - 768 / GameProcessor.zoom / 2))));
                    ReGenerateTurn();
                }
                if (selectedCharTurn != null && selectedCharTurn.bIsCompleted)
                {
                    var selectedChar = EncounterInfo.currentTurn().selectedCharTurn.character;
                    GameProcessor.sceneCamera = ((new Vector2(-(selectedChar.position.X + 32 - 1366 / GameProcessor.zoom / 2), -(selectedChar.position.Y + 32 - 768 / GameProcessor.zoom / 2))));
                }
                if (selectedCharTurn == null)
                {
                    if (PlayerController.previousSelected != null && !EncounterInfo.currentTurn().groupTurnSet.Find(gts2 => gts2.character == PlayerController.previousSelected).bIsCompleted && !bFirstTurn)
                    {
                        PlayerController.previousSelected.changePosition(PlayerController.originalPositionCharacter);
                        groupTurnSet.Find(sct => sct.character == PlayerController.previousSelected).stepsSet = 0;
                    }
                    // PlayerController.selectedSprite.position = PlayerController.originalPositionCharacter;

                    bFirstTurn = true;
                    GameProcessor.cameraFollowTarget = null;
                }
            }
            #endregion
        }


        public void RightButtonPlayerAction()
        {
            if (selectedCharTurn != null && !selectedCharTurn.bIsCompleted)
            {
                BaseCharacter selectedChar = selectedCharTurn.character;
                #region Right Button Logic


                Vector2 temp = new Vector2(-1);
                if (selectedCharTurn.returnCompleteArea().Find(t => t.mapPosition.Contains(GameProcessor.EditorCursorPos)) != null)
                {
                    temp = (GameProcessor.EditorCursorPos / 64).ToPoint().ToVector2() * 64;
                }

                if (temp != new Vector2(-1))
                //    if (temp != new Vector2(-1) && !selectedChar.spriteGameSize.Contains(temp))
                {
                    if (CombatProcessor.zone.Contains(GameProcessor.EditorCursorPos) && !PathMoveHandler.bIsBusy)
                    {
                        CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, GameProcessor.EditorCursorPos, selectedCharTurn.returnCompleteArea());
                        PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);
                        bPlayerMustSelectAction = true;
                        BattleGUI.InitializePopUpMenu();
                        //FinalizeCharacterRound();
                    }
                    else if (CombatProcessor.zone.Contains(GameProcessor.EditorCursorPos) && PathMoveHandler.bIsBusy)
                    {
                        PathMoveHandler.SkipPathMovement();
                        CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, GameProcessor.EditorCursorPos, selectedCharTurn.returnCompleteArea());
                        PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);
                    }
                }

                #endregion

            }

            if (selectedCharTurn == null)
            {
                foreach (var item in CombatProcessor.zoneTiles)
                {
                    if (item.mapPosition.Contains(KeyboardMouseUtility.gameMousePos))
                    {
                        GameProcessor.GenerateCameraInstant(KeyboardMouseUtility.gameMousePos, 3, GameProcessor.zoom);
                    }
                }
            }
        }

        public void StartPopUpChoice(Vector2 pos)
        {
            //CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(pos, GameProcessor.EditorCursorPos, selectedCharTurn.returnCompleteArea());
            // PathMoveHandler.Start(PlayerController.selectedSprite, CombatProcessor.allPossibleNodes);
            bPlayerMustSelectAction = true;
            BattleGUI.InitializePopUpMenu();
        }

        int randomEnemy = 0;
        internal void UpdateEnemyLogic(CharacterTurn gts)
        {
            if (!PathMoveHandler.bIsBusy)
            {
                PathMoveHandler.SkipPathMovement();
            }
            if (gts != null && !LUA.LuaExecutionList.DemandOverride() && !PathMoveHandler.bIsBusy && !bTest && BattleGUI.TurnEffectsList.Find(te => te.bMustShow) == null)
            {
                if (bPlayerTurnEnemyOverriden)
                {
                    gts = currentEnemy;
                }

                if (GameProcessor.cameraFollowTarget != gts.character)
                {
                    GameProcessor.cameraFollowTarget = gts.character;
                }

                bTest = true;
                //randomEnemy = GamePlayUtility.Randomize(0, CombatProcessor.heroCharacters.Count - 1);
                selectedCharTurn = null;
                currentEnemy = gts;
                AIUtility.AIPreCalculate(gts.returnCompleteArea(), gts);
                //AIUtility.Report(AIBehaviour.ProcessBehaviour(currentEnemy.character,AIUtility.possibleAITargets));
                BaseCharacter target = null;
                var temp = AIBehaviour.ProcessBehaviour(currentEnemy.character, AIUtility.possibleAITargets);
                if (temp.Count != 0)
                {
                    target = AIUtility.SelectTargetBasedOnBehaviour(temp);
                }

                var playableMap = GameProcessor.loadedMap.possibleTilesGameZoneForEnemy(CombatProcessor.zoneTiles, currentEnemy.character);

                var beep = new KeyValuePair<List<List<BasicTile>>, List<BasicAbility>>();
                if (target != null)
                {
                    // var beep = AIUtility.returnTilesToAttackFrom(CombatProcessor.zoneTiles,gts.character,target);
                    beep = AIUtility.returnTilesToAttackFrom(gts.returnCompleteArea(), playableMap, gts.character, target);

                    while (beep.Key.Count == 0 && beep.Value.Count == 0 && AIUtility.possibleAITargets.Count - 1 > 0)
                    {
                        AIUtility.possibleAITargets.Remove(target);
                        target = AIUtility.SelectTargetBasedOnBehaviour(AIBehaviour.ProcessBehaviour(currentEnemy.character, AIUtility.possibleAITargets));
                        beep = AIUtility.returnTilesToAttackFrom(playableMap, playableMap, gts.character, target);
                    }
                }

                if (target != null && beep.Key.Count == 0 && beep.Value.Count == 0)
                {
                    target = null;
                }

                gts.ReGenerateTurn(playableMap);

                List<Node> allPossibleNodes;
                if (target != null)
                {
                    //allPossibleNodes = PathFinder.NewPathSearch(gts.character.position, target.position + new Vector2(64, 0), playableMap);
                    var enemyFavor = AIUtility.returnFavoredAttackAndPosition(beep, gts.character, target);
                    if (enemyFavor.Key != null)
                    {
                        allPossibleNodes = PathFinder.NewPathSearch(gts.character.position, enemyFavor.Key.mapPosition.Location.ToVector2(), playableMap);
                    }
                    else
                    {
                        allPossibleNodes = null;
                    }
                    AITarget = target;
                    AIbtba = enemyFavor;
                }
                else
                {
                    List<int> amountOfCharsForEachGroup = new List<int>();
                    List<float> percentagesForEachGroup = new List<float>();
                    int totalChars = 0;
                    foreach (var item in gts.parentTS.targetGroups)
                    {
                        int tempCount = item.charactersInGroup.FindAll(c => c.IsAlive()).Count;
                        totalChars += tempCount;
                        amountOfCharsForEachGroup.Add(tempCount);
                    }

                    foreach (var item in amountOfCharsForEachGroup)
                    {
                        percentagesForEachGroup.Add(((float)item / (float)totalChars) * 100);
                    }

                    TurnSet targetGroup = null;
                    int tempIndex = 0;
                    while (targetGroup == null)
                    {
                        int randomNumber = GamePlayUtility.Randomize(0, 100);
                        if (percentagesForEachGroup[tempIndex] >= randomNumber)
                        {
                            targetGroup = targetGroups[tempIndex];
                        }
                        tempIndex++;
                        if (tempIndex > percentagesForEachGroup.Count)
                        {
                            targetGroup = targetGroups[0];
                        }
                    }

                    BaseCharacter randomCharacter = targetGroup.charactersInGroup.FindAll(c => c.IsAlive())[GamePlayUtility.Randomize(0, targetGroup.charactersInGroup.FindAll(c => c.IsAlive()).Count)];

                    allPossibleNodes = PathFinder.NewPathSearch(gts.character.position, randomCharacter.position + new Vector2(64, 0), playableMap);
                }
                if (allPossibleNodes != null)
                {
                    CombatProcessor.what = new List<Node>(allPossibleNodes);
                    var NodesWithinRange = MapListUtility.findEqualNodesToTileList(allPossibleNodes, gts.returnCompleteArea());
                    PathMoveHandler.Start(gts.character, NodesWithinRange);
                }

                //FinalizeEnemyRound();
            }

            if (gts != null && !PathMoveHandler.bIsBusy && bTest && !GameProcessor.bStartCombatZoom)
            {

                //THIS IS THE VERY LAST LINE BEFORE AN ENEMY HAS IT'S TURN COMPLETED
                if (!AIbtba.Equals(default(KeyValuePair<BasicTile, BasicAbility>)))
                {

                    if ((AIbtba.Key == null || AIbtba.Key.mapPosition.Location.ToVector2() == gts.character.position) && AIbtba.Value.abilityCanHitTargetInRange(gts.character, AITarget) && AIbtba.Value.IsAbilityAvailable(gts.character.trueSTATChart()))
                    {
                        FaceCharacterInCorrectDirection(gts.character, AITarget);

                        GameProcessor.StartBattleSoon(gts.character, AITarget, gts.character.AbilityList().IndexOf(AIbtba.Value));
                        //GameProcessor.StartBattleSoon(gts.character, AITarget, gts.character.AbilityList().IndexOf(gts.character.AbilityList().Find(ba => ba.abilityName.Equals(AIbtba.Value.abilityName))));
                    }
                    else
                    {
                        if (!currentEnemy.character.attackedAsAI)
                        {
                            LastDitchEffortAttack();
                        }
                        else
                        {
                            FinalizeEnemyRound();
                        }
                    }
                }
                else
                {
                    if (!currentEnemy.character.attackedAsAI)
                    {
                        LastDitchEffortAttack();
                    }
                    else
                    {
                        FinalizeEnemyRound();
                    }
                }
                //
            }
            else if (gts == null)
            {
                if (bPlayerTurnEnemyOverriden)
                {
                    bPlayerTurnEnemyOverriden = false;
                }
            }
        }

        private void FaceCharacterInCorrectDirection(BaseCharacter caster, BaseCharacter target)
        {
            if (caster.position + new Vector2(64, 0) == target.position)
            {
                caster.rotationIndex = (int)BaseCharacter.Rotation.Right;
            }
            else if (caster.position + new Vector2(-64, 0) == target.position)
            {
                caster.rotationIndex = (int)BaseCharacter.Rotation.Left;
            }
            else if (caster.position + new Vector2(0, 64) == target.position)
            {
                caster.rotationIndex = (int)BaseCharacter.Rotation.Down;
            }
            else if (caster.position + new Vector2(0, -64) == target.position)
            {
                caster.rotationIndex = (int)BaseCharacter.Rotation.Up;
            }
        }

        public void FinalizeEnemyRound()
        {

            currentEnemy.character.attackedAsAI = false;
            BattleGUI.End();
            bTest = false;
            AITarget = null;
            AIbtba = default(KeyValuePair<BasicTile, BasicAbility>);
            currentEnemy.character.attackedAsAI = false;
            currentEnemy.bIsCompleted = true;
            //var MaxAP = currentEnemy.character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
            //BasicTile finalTile = currentEnemy.returnCompleteArea().Find(t => t.mapPosition.Location == PathMoveHandler.finalPos.ToPoint());
            //var expendedAP = currentEnemy.characterArea.IndexOf(currentEnemy.characterArea.Find(area => area.Contains(finalTile))) + 1;
            //currentEnemy.character.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP] += MaxAP - expendedAP;

            if (currentEnemy.character.bSaveAP || true)
            {
                var MaxAP = currentEnemy.character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
                BasicTile finalTile = currentEnemy.returnCompleteArea().Find(t => t.mapPosition.Location == PathMoveHandler.finalPos.ToPoint());
                var expendedAP = currentEnemy.characterArea.IndexOf(currentEnemy.characterArea.Find(area => area.Contains(finalTile))) + 1;
                currentEnemy.character.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP] += MaxAP - expendedAP;
            }

            if (currentEnemy.character.bAIExecuteDefend)
            {
                BattleGUI.DefendOption(currentEnemy.character, currentEnemy.character);
            }

            //   Console.WriteLine(currentEnemy.character.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP]);
            BattleScriptHandler.Execute(LUA.LuaBScriptEvent.EventType.postCT, this.toLuaTurnSetInfo());
            TBAGW.EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden = false; //This line must always be after the Execute line

            EncounterInfo.ClearDeathChars();
            EncounterInfo.UpdateAllStats();
        }

        private void LastDitchEffortAttack()
        {
            BasicAbility testAbility = currentEnemy.character.CCC.AIDefaultAttack.Clone();
            testAbility.abilityCooldownTimer = 0;

            if (currentEnemy.character.AbilityList().Find(abi => abi.abilityIdentifier == testAbility.abilityIdentifier) == null)
            {
                currentEnemy.character.CCC.equippedClass.classAbilities.Add(testAbility);
            }

            List<BasicAbility> possibleAbilities = new List<BasicAbility>();
            foreach (var ability in currentEnemy.character.AbilityList())
            {
                if (ability.IsAbilityAvailable(currentEnemy.character.trueSTATChart()))
                {
                    possibleAbilities.Add(ability);
                }
            }

            foreach (var item in possibleAbilities)
            {
                if (GamePlayUtility.randomChance() < item.castChance)
                {
                    testAbility = item;
                    break;
                }
            }

            if (testAbility.IsAbilityAvailable(currentEnemy.character.trueSTATChart()))
            {
                List<BaseCharacter> allTargetableCharactersInRange = new List<BaseCharacter>();
                if (testAbility.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK)
                {
                    foreach (var item in currentEnemy.parentTS.targetGroups)
                    {
                        foreach (var character in item.charactersInGroup)
                        {
                            if (testAbility.abilityCanHitTargetInRange(currentEnemy.character, character))
                            {
                                allTargetableCharactersInRange.Add(character);
                            }
                        }
                    }
                    if (allTargetableCharactersInRange.Count > 0)
                    {
                        // BattleGUI.AIStart(currentEnemy.character, allTargetableCharactersInRange[GamePlayUtility.Randomize(0, allTargetableCharactersInRange.Count)], currentEnemy.character.AbilityList().IndexOf(testAbility));
                        GameProcessor.StartBattleSoon(currentEnemy.character, allTargetableCharactersInRange[GamePlayUtility.Randomize(0, allTargetableCharactersInRange.Count)], currentEnemy.character.AbilityList().IndexOf(testAbility));
                    }
                }
                else if (testAbility.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT)
                {

                    foreach (var character in currentEnemy.parentTS.charactersInGroup)
                    {
                        if (character == currentEnemy.character)
                        {

                        }
                        if (character != currentEnemy.character && testAbility.abilityCanHitTargetInRange(currentEnemy.character, character))
                        {
                            allTargetableCharactersInRange.Add(character);
                        }
                    }

                    if (allTargetableCharactersInRange.Count > 0)
                    {
                        //BattleGUI.AIStart(currentEnemy.character, allTargetableCharactersInRange[GamePlayUtility.Randomize(0, allTargetableCharactersInRange.Count)], currentEnemy.character.AbilityList().IndexOf(testAbility));
                        GameProcessor.StartBattleSoon(currentEnemy.character, allTargetableCharactersInRange[GamePlayUtility.Randomize(0, allTargetableCharactersInRange.Count)], currentEnemy.character.AbilityList().IndexOf(testAbility));
                    }
                }

            }
            if (!BattleGUI.bIsRunning)
            {
                currentEnemy.character.attackedAsAI = true;
            }

        }

        private void ResetAndChangeGroup()
        {
            bIsCompleted = false;
            foreach (var item in groupTurnSet)
            {
                item.bIsCompleted = false;
            }

            if (bIsPlayerTurnSet)
            {
                BattleScriptHandler.Execute(LUA.LuaBScriptEvent.EventType.postPT, this.toLuaTurnSetInfo());
            }
            else
            {
                BattleScriptHandler.Execute(LUA.LuaBScriptEvent.EventType.postET, this.toLuaTurnSetInfo());
            }


            EncounterInfo.ChangeGroup();

            randomEnemy = GamePlayUtility.Randomize(0, CombatProcessor.heroCharacters.Count - 1);
        }

        public void FinalizeCharacterRound()
        {
            BattleGUI.End();
            bPlayerMustSelectAction = false;
            selectedCharTurn.bIsCompleted = true;

            if (selectedCharTurn.character.bSaveAP)
            {
                var MaxAP = selectedCharTurn.character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP];
                BasicTile finalTile = selectedCharTurn.returnCompleteArea().Find(t => t.mapPosition.Location == PathMoveHandler.finalPos.ToPoint());
                var expendedAP = selectedCharTurn.characterArea.IndexOf(selectedCharTurn.characterArea.Find(area => area.Contains(finalTile))) + 1;
                selectedCharTurn.character.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP] += MaxAP - expendedAP;
            }

            //Console.WriteLine(selectedCharTurn.character.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP]);
            BattleScriptHandler.Execute(LUA.LuaBScriptEvent.EventType.postCT, this.toLuaTurnSetInfo());
            EncounterInfo.ClearDeathChars();

            selectedCharTurn = null;
            EncounterInfo.UpdateAllStats();


        }

        public void EnemyIsDeath()
        {

        }

        public void GenerateTurns()
        {
            bIsCompleted = false;
            if (groupTurnSet.Count == 0)
            {
                foreach (var c in charactersInGroup)
                {
                    groupTurnSet.Add(new CharacterTurn(c, zone, zoneTiles, this));
                }
            }

        }

        public void ReGenerateTurn(BaseCharacter bc)
        {
            if (bc.IsAlive())
            {
                var ts = groupTurnSet.Find(gts => gts.character == bc);
                ts.ReGenerateTurn(GameProcessor.loadedMap.possibleTilesGameZone(CombatProcessor.zoneTiles));
            }

        }

        public void ReGenerateTurn()
        {
            selectedCharTurn.ReGenerateTurn(GameProcessor.loadedMap.possibleTilesGameZone(CombatProcessor.zoneTiles));
        }

        public void SetAsEnemy()
        {
            bIsPlayerTurnSet = false;
            bIsEnemyTurnSet = true;
            bIsAssistTurnSet = false;
        }

        public void SetAsAssist()
        {
            bIsPlayerTurnSet = false;
            bIsEnemyTurnSet = false;
            bIsAssistTurnSet = true;
        }

        internal LUA.LuaTurnSetInfo toLuaTurnSetInfo()
        {
            LUA.LuaTurnSetInfo ltsi = new LUA.LuaTurnSetInfo();
            ltsi.bIsAssist = this.bIsAssistTurnSet;
            ltsi.bIsEnemy = this.bIsEnemyTurnSet;
            ltsi.bIsPlayer = this.bIsPlayerTurnSet;
            ltsi.parent = this;
            ltsi.charactersInGroup = this.charactersInGroup;
            this.groupTurnSet.ForEach(ct => ltsi.charTurnInfos.Add(ct.toLuaCharacterTurnInfo()));

            if (EncounterInfo.currentTurn().bIsPlayerTurnSet && !bPlayerTurnEnemyOverriden)
            {
                ltsi.CTCallFrom = selectedCharTurn == null ? null : selectedCharTurn.toLuaCharacterTurnInfo();
            }
            else
            {
                ltsi.CTCallFrom = currentEnemy == null ? null : currentEnemy.toLuaCharacterTurnInfo();
            }

            var tempL = EncounterInfo.encounterGroups.FindAll(g => g != this);
            foreach (var item in tempL)
            {
                ltsi.otherGroups.Add(item.toLuaTurnSetInfoSimple());
            }

            return ltsi;
        }

        internal LUA.LuaTurnSetInfo toLuaTurnSetInfoSimple()
        {
            LUA.LuaTurnSetInfo ltsi = new LUA.LuaTurnSetInfo();
            ltsi.bIsAssist = this.bIsAssistTurnSet;
            ltsi.bIsEnemy = this.bIsEnemyTurnSet;
            ltsi.bIsPlayer = this.bIsPlayerTurnSet;
            ltsi.parent = this;
            ltsi.charactersInGroup = this.charactersInGroup;
            this.groupTurnSet.ForEach(ct => ltsi.charTurnInfos.Add(ct.toLuaCharacterTurnInfo()));

            if (EncounterInfo.currentTurn().bIsPlayerTurnSet)
            {
                ltsi.CTCallFrom = selectedCharTurn == null ? null : selectedCharTurn.toLuaCharacterTurnInfo();
            }
            else
            {
                ltsi.CTCallFrom = currentEnemy == null ? null : currentEnemy.toLuaCharacterTurnInfo();
            }

            return ltsi;
        }

        internal void Remove(CharacterTurn ct, LUA.LuaTurnSetInfo.SideTurnType stt)
        {
            //CombatProcessor.encounterEnemies
            groupTurnSet.Remove(ct);
            try
            {
                charactersInGroup.Remove(ct.character);
            }
            catch (Exception e)
            {

            }

        }

        internal void AddViaChangeSide(CharacterTurn ct, LUA.LuaTurnSetInfo.SideTurnType stt)
        {
            groupTurnSet.Add(ct);
            try
            {
                charactersInGroup.Add(ct.character);

                ct.bSideTurned = true;
                ct.sideTurnType = stt;
            }
            catch (Exception e)
            {

            }
        }
    }


}

namespace LUA
{
    public class LuaTurnSetInfo
    {
        public bool bIsPlayer = false;
        public bool bIsEnemy = false;
        public bool bIsAssist = false;
        public LuaCharacterTurnInfo CTCallFrom = null;
        public List<LuaCharacterTurnInfo> charTurnInfos = new List<LuaCharacterTurnInfo>();
        public List<LuaTurnSetInfo> otherGroups = new List<LuaTurnSetInfo>();
        public enum SideTurnType { Normal, MindControl }

        internal List<BaseCharacter> charactersInGroup = new List<BaseCharacter>();
        internal TBAGW.TurnSet parent = null;

        public LuaTurnSetInfo() { }

        public LuaCharacterInfo RandomMember(bool bAllowDeadHeroes = false)
        {
            LuaCharacterInfo lci = new LuaCharacterInfo();
            if (bIsPlayer)
            {
                if (bAllowDeadHeroes)
                {
                    return TBAGW.PlayerSaveData.heroParty[GamePlayUtility.Randomize(0, TBAGW.PlayerSaveData.heroParty.Count - 1)].toCharInfo();
                }
                else
                {
                    var charsAlive = TBAGW.PlayerSaveData.heroParty.FindAll(h => h.IsAlive());
                    return charsAlive[GamePlayUtility.Randomize(0, charsAlive.Count - 1)].toCharInfo();
                }
            }
            else
            {
                var charsAlive = charactersInGroup.FindAll(h => h.IsAlive());
                return charsAlive[GamePlayUtility.Randomize(0, charsAlive.Count - 1)].toCharInfo();
            }
        }

        public bool CTIsFromCurrentTS(LuaCharacterTurnInfo lcti)
        {
            return charTurnInfos.Find(ct => ct.parent == lcti.parent) != default(LuaCharacterTurnInfo);
        }

        public bool EndCTFromCurrentTurn()
        {
            return charTurnInfos.Find(ct => ct.parent == CTCallFrom.parent) != null;
        }

        //public bool CTIsFromCurrentTS()
        //{
        //    return true;
        //   // return charTurnInfos.Find(ct => ct.parent == lcti.parent) != default(LuaCharacterTurnInfo);
        //}

        public void AddChar(Enemy e)
        {
            if (e != null)
            {
                try
                {
                    BaseCharacter temp = e.ToEnemyDuringBattle(TBAGW.CombatProcessor.zone, TBAGW.CombatProcessor.zoneTiles);
                    TBAGW.CharacterTurn ct = new TBAGW.CharacterTurn(temp, TBAGW.CombatProcessor.zone, TBAGW.CombatProcessor.zoneTiles, parent);
                    if (temp != null && ct != null)
                    {
                        parent.groupTurnSet.Add(ct);
                        parent.charactersInGroup.Add(temp);
                        TBAGW.CombatProcessor.encounterEnemies.Add(temp);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public void HandleChangeSide(LuaCharacterTurnInfo lci, LuaTurnSetInfo lsiFrom, LuaTurnSetInfo lsiTo, SideTurnType stt = SideTurnType.Normal)
        {
            lsiFrom.parent.Remove(lci.parent, stt);
            lsiTo.parent.AddViaChangeSide(lci.parent, stt);
        }
    }

    public static class LuaCombatCommands
    {

        public static void EndTurn(LuaTurnSetInfo ltsi)
        {
            ltsi.parent.groupTurnSet.ForEach(e => e.bIsCompleted = true);
        }

        public static void StartAI(LuaCharacterTurnInfo lcti)
        {
            lcti.parent.bIsCompleted = false;
            TBAGW.EncounterInfo.currentTurn().bTest = false;

            if (TBAGW.EncounterInfo.currentTurn().bIsPlayerTurnSet)
            {
                TBAGW.EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden = true;
            }

            TBAGW.EncounterInfo.currentTurn().UpdateEnemyLogic(lcti.parent);
            TBAGW.EncounterInfo.currentTurn().currentEnemy = lcti.parent;
        }
    }
}


