using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TBAGW.AI;
using TBAGW.Utilities.Actions;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Utilities.Control.Player
{
    static public class CombatCtrl
    {
        static public BaseCharacter selectedChar = new BaseCharacter();

        static public void Update(List<ActionKey> keys)
        {
            if (TurnSet.bSelectingArea && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsPlayerTurnSet)
            {
                if (keys.Count != 0)
                {
                    AreaSelectContext(keys[keys.Count - 1]);
                }

            }
            else
            {

                if (!ScriptProcessor.bIsRunning && !LootScreen.IsRunning())
                {
                    if (BattleGUI.bIsRunning && EncounterInfo.encounterGroups.Count > 0 && !EncounterInfo.currentTurn().bIsEnemyTurnSet && !BattleGUI.bIsAttacking && (Mouse.GetState().RightButton == ButtonState.Pressed || keys.Last().actionIndentifierString.Equals(Game1.cancelString) || keys.Last().actionIndentifierString.Equals(Game1.SettingsMenu)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        KeyboardMouseUtility.bPressed = true;
                        BattleGUI.bChooseAbility = false;
                        BattleGUI.EndWithoutAttacking();
                    }

                    if ((PlayerController.selectedSprite == null && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden) || (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().selectedCharTurn == null && CombatProcessor.bMainCombat && EncounterInfo.currentTurn().bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden))
                    {

                        if (keys.Count != 0)
                        {
                            GlobalControls(keys);
                            NoSpriteSelectedControls(keys[keys.Count - 1]);
                            KeyboardMouseUtility.bPressed = true;
                        }
                    }
                    else if (selectedChar != null)
                    {

                        if (keys.Count != 0)
                        {
                            GlobalControls(keys);
                            SpriteSelectedControls(keys[keys.Count - 1]);
                            KeyboardMouseUtility.bPressed = true;
                        }
                        selectedChar = (BaseCharacter)PlayerController.selectedSprite;
                    }
                    else if (selectedChar == null && PlayerController.selectedSprite != null || (EncounterInfo.encounterGroups.Count != 0 && !EncounterInfo.currentTurn().bIsPlayerTurnSet))
                    {
                        selectedChar = (BaseCharacter)PlayerController.selectedSprite;
                        if (keys.Count != 0)
                        {
                            GlobalControls(keys);
                            SpriteSelectedControls(keys[keys.Count - 1]);
                            KeyboardMouseUtility.bPressed = true;
                        }
                    }
                    else if (BattleGUI.bIsRunning)
                    {
                        if (keys.Count != 0)
                        {
                            //GlobalControls(keys);
                            SpriteSelectedControls(keys[keys.Count - 1]);
                            KeyboardMouseUtility.bPressed = true;
                        }
                    }

                }
                else if (LootScreen.IsRunning())
                {
                    if (LootScreen.WaitingForKeyPress())
                    {
                        if (keys.Count != 0)
                        {
                            LootScreenKeys(keys[keys.Count - 1]);
                            KeyboardMouseUtility.bPressed = true;
                        }
                    }
                }

            }
        }

        private static void AreaSelectContext(ActionKey actionKey)
        {
            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.cameraMoveUpString, StringComparison.OrdinalIgnoreCase))
            {
                BattleGUI.gbc.rotationIndex = (int)BaseCharacter.Rotation.Up;
                KeyboardMouseUtility.bPressed = true;
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.cameraMoveDownString, StringComparison.OrdinalIgnoreCase))
            {
                BattleGUI.gbc.rotationIndex = (int)BaseCharacter.Rotation.Down;
                KeyboardMouseUtility.bPressed = true;
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.cameraMoveLeftString, StringComparison.OrdinalIgnoreCase))
            {
                BattleGUI.gbc.rotationIndex = (int)BaseCharacter.Rotation.Left;
                KeyboardMouseUtility.bPressed = true;
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.cameraMoveRightString, StringComparison.OrdinalIgnoreCase))
            {
                BattleGUI.gbc.rotationIndex = (int)BaseCharacter.Rotation.Right;
                KeyboardMouseUtility.bPressed = true;
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.cancelString, StringComparison.OrdinalIgnoreCase))
            {
                BattleGUI.HandleAreaCancel();
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.confirmString, StringComparison.OrdinalIgnoreCase))
            {
                BattleGUI.HandleAreaConfirm();
            }

            #region
            if (actionKey.actionIndentifierString.Equals(Game1.moveUpString))
            {
                //PlayerController.selectedSprite.Move(new Vector2(0, -Speed));
                //PathMoveHandler.SkipPathMovement();
                if (!PathMoveHandler.bIsBusy)
                {
                    selectedChar.rotationIndex = (int)BaseCharacter.Rotation.Up;
                    CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, selectedChar.position - new Vector2(0, 64), CombatProcessor.radiusTiles);
                    if (CombatProcessor.allPossibleNodes.Count != 0)
                    {
                        //  EncounterInfo.currentTurn().selectedCharTurn.stepsSet++;
                        EncounterInfo.currentTurn().bRegenerateRadius = true;
                    }
                    PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);

                }

            }

            if (actionKey.actionIndentifierString.Equals(Game1.moveDownString))
            {
                //PlayerController.selectedSprite.Move(new Vector2(0, Speed));

                //  PathMoveHandler.SkipPathMovement();
                if (!PathMoveHandler.bIsBusy)
                {
                    selectedChar.rotationIndex = (int)BaseCharacter.Rotation.Down;
                    CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, selectedChar.position + new Vector2(0, 64), CombatProcessor.radiusTiles);
                    if (CombatProcessor.allPossibleNodes.Count != 0)
                    {
                        //  EncounterInfo.currentTurn().selectedCharTurn.stepsSet++;
                        EncounterInfo.currentTurn().bRegenerateRadius = true;
                    }
                    PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);


                }
            }

            if (actionKey.actionIndentifierString.Equals(Game1.moveLeftString))
            {
                //  PlayerController.selectedSprite.Move(new Vector2(-Speed, 0));

                // PathMoveHandler.SkipPathMovement();
                if (!PathMoveHandler.bIsBusy)
                {
                    selectedChar.rotationIndex = (int)BaseCharacter.Rotation.Left;
                    CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, selectedChar.position - new Vector2(64, 0), CombatProcessor.radiusTiles);
                    if (CombatProcessor.allPossibleNodes.Count != 0)
                    {
                        //  EncounterInfo.currentTurn().selectedCharTurn.stepsSet++;
                        EncounterInfo.currentTurn().bRegenerateRadius = true;
                    }
                    PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);

                }
            }

            if (actionKey.actionIndentifierString.Equals(Game1.moveRightString))
            {
                // PlayerController.selectedSprite.Move(new Vector2(Speed, 0));

                //PathMoveHandler.SkipPathMovement();
                if (!PathMoveHandler.bIsBusy)
                {
                    selectedChar.rotationIndex = (int)BaseCharacter.Rotation.Right;
                    //int radius = selectedChar.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB] * selectedChar.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP] - EncounterInfo.currentTurn().selectedCharTurn.stepsSet;
                    //CombatProcessor.radiusTiles = MapListUtility.returnMapRadius(radius,GameProcessor.loadedMap.possibleTilesGameZone(CombatProcessor.zoneTiles),selectedChar);
                    CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, selectedChar.position + new Vector2(64, 0), CombatProcessor.radiusTiles);
                    if (CombatProcessor.allPossibleNodes.Count != 0)
                    {
                        // EncounterInfo.currentTurn().selectedCharTurn.stepsSet++;
                        EncounterInfo.currentTurn().bRegenerateRadius = true;
                    }
                    PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);

                }
            }
            #endregion
        }

        private static void LootScreenKeys(ActionKey key)
        {
            if (key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                LootScreen.HandleConfirmPress();
            }
        }

        private static void SpriteSelectedControls(ActionKey key)
        {
            if (BattleGUI.bIsRunning && EncounterInfo.currentTurn().bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden)
            {



                //if (key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed() && !BattleGUI.bChooseAbility && !BattleGUI.bIsAttacking)
                //{
                //    BattleGUI.bChooseAbility = true;
                //}


                if (!BattleGUI.bChooseAbility && BattleGUI.bIsAttacking && (key.actionIndentifierString.Equals(Game1.confirmString) || key.actionIndentifierString.Equals(Game1.openMenuString)) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    EncounterInfo.currentTurn().FinalizeCharacterRound();

                    BattleGUI.End();
                }

                if ((key.actionIndentifierString.Equals(Game1.confirmString) || key.actionIndentifierString.Equals(Game1.openMenuString)) && !KeyboardMouseUtility.AnyButtonsPressed() && BattleGUI.ExecutableAbilitySelected() && BattleGUI.bChooseAbility)
                {

                    BattleGUI.PlayerChoiceMade();
                    // BattleGUI.bStartZoom = true;
                }

                if (!BattleGUI.bChooseAbility && key.actionIndentifierString.Equals(Game1.confirmString) && !BattleGUI.bIsAttacking && !KeyboardMouseUtility.AnyButtonsPressed() && BattleGUI.ExecutableAbilitySelected() && !BattleGUI.bChooseAbility)
                {
                    //BattleGUI.bChooseAbility = true;
                }



                if (BattleGUI.bChooseAbility)
                {
                    if (key.actionIndentifierString.Equals(Game1.moveUpString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {

                        BattleGUI.PlayerChoiceUpDown(-1);
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveDownString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        BattleGUI.PlayerChoiceUpDown(+1);
                    }
                }



                if (key.actionIndentifierString.Equals(Game1.debugInfoString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    BattleStats.Report(BattleGUI.gbc);
                }

            }
            else if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && (!EncounterInfo.currentTurn().bIsPlayerTurnSet || (EncounterInfo.currentTurn().bIsPlayerTurnSet && EncounterInfo.currentTurn().bPlayerTurnEnemyOverriden)))
            {
                if ((key.actionIndentifierString.Equals(Game1.confirmString) || key.actionIndentifierString.Equals(Game1.openMenuString)) && !KeyboardMouseUtility.AnyButtonsPressed() && !BattleGUI.bStartZoom)
                {
                    if (GameProcessor.bMayEndCharacterTurn && EncounterInfo.currentTurn().bIsEnemyTurnSet && EncounterInfo.currentTurn().currentEnemy.character.attackedAsAI)
                    {
                        EncounterInfo.currentTurn().FinalizeEnemyRound();
                    }
                    BattleGUI.End();
                }
            }
            else
            {
                if ((PlayerController.selectedSprite as BaseCharacter).IsAlive() && EncounterInfo.encounterGroups.Count > 0 && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsPlayerTurnSet && !EncounterInfo.currentTurn().bPlayerMustSelectAction && !EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == PlayerController.selectedSprite).bIsCompleted)
                {
                    #region
                    if (key.actionIndentifierString.Equals(Game1.moveUpString))
                    {
                        //PlayerController.selectedSprite.Move(new Vector2(0, -Speed));
                        //PathMoveHandler.SkipPathMovement();
                        if (!PathMoveHandler.bIsBusy)
                        {
                            selectedChar.rotationIndex = (int)BaseCharacter.Rotation.Up;
                            CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, selectedChar.position - new Vector2(0, 64), CombatProcessor.radiusTiles);
                            if (CombatProcessor.allPossibleNodes.Count != 0)
                            {
                                //    EncounterInfo.currentTurn().selectedCharTurn.stepsSet++;
                                EncounterInfo.currentTurn().bRegenerateRadius = true;
                            }
                            PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);

                        }

                    }

                    if (key.actionIndentifierString.Equals(Game1.moveDownString))
                    {
                        //PlayerController.selectedSprite.Move(new Vector2(0, Speed));

                        //  PathMoveHandler.SkipPathMovement();
                        if (!PathMoveHandler.bIsBusy)
                        {
                            selectedChar.rotationIndex = (int)BaseCharacter.Rotation.Down;
                            CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, selectedChar.position + new Vector2(0, 64), CombatProcessor.radiusTiles);
                            if (CombatProcessor.allPossibleNodes.Count != 0)
                            {
                                //   EncounterInfo.currentTurn().selectedCharTurn.stepsSet++;
                                EncounterInfo.currentTurn().bRegenerateRadius = true;
                            }
                            PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);


                        }
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveLeftString))
                    {
                        //  PlayerController.selectedSprite.Move(new Vector2(-Speed, 0));

                        // PathMoveHandler.SkipPathMovement();
                        if (!PathMoveHandler.bIsBusy)
                        {
                            selectedChar.rotationIndex = (int)BaseCharacter.Rotation.Left;
                            CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, selectedChar.position - new Vector2(64, 0), CombatProcessor.radiusTiles);
                            if (CombatProcessor.allPossibleNodes.Count != 0)
                            {
                                //  EncounterInfo.currentTurn().selectedCharTurn.stepsSet++;
                                EncounterInfo.currentTurn().bRegenerateRadius = true;
                            }
                            PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);

                        }
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveRightString))
                    {
                        // PlayerController.selectedSprite.Move(new Vector2(Speed, 0));

                        //PathMoveHandler.SkipPathMovement();
                        if (!PathMoveHandler.bIsBusy)
                        {
                            selectedChar.rotationIndex = (int)BaseCharacter.Rotation.Right;
                            //int radius = selectedChar.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB] * selectedChar.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.AP] - EncounterInfo.currentTurn().selectedCharTurn.stepsSet;
                            //CombatProcessor.radiusTiles = MapListUtility.returnMapRadius(radius,GameProcessor.loadedMap.possibleTilesGameZone(CombatProcessor.zoneTiles),selectedChar);
                            CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, selectedChar.position + new Vector2(64, 0), CombatProcessor.radiusTiles);
                            if (CombatProcessor.allPossibleNodes.Count != 0)
                            {
                                //  EncounterInfo.currentTurn().selectedCharTurn.stepsSet++;
                                EncounterInfo.currentTurn().bRegenerateRadius = true;
                            }
                            PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);

                        }
                    }

                    if (EncounterInfo.currentTurn().bIsPlayerTurnSet && !BattleGUI.bIsRunning)
                    {

                        if (!KeyboardMouseUtility.bMouseButtonPressed && (key.actionIndentifierString.Equals(Game1.confirmString) || key.actionIndentifierString.Equals(Game1.openMenuString)) && !KeyboardMouseUtility.AnyButtonsPressed() && !PathMoveHandler.bIsBusy && EncounterInfo.currentTurn().selectedCharTurn != null && CombatProcessor.bMainCombat && !EncounterInfo.currentTurn().bPlayerMustSelectAction)
                        {
                            KeyboardMouseUtility.bPressed = true;
                            EncounterInfo.currentTurn().StartPopUpChoice(PlayerController.selectedSprite.position);
                        }
                        else if (KeyboardMouseUtility.bMouseButtonPressed && (key.actionIndentifierString.Equals(Game1.cancelString)) && !KeyboardMouseUtility.AnyButtonsPressed() && !PathMoveHandler.bIsBusy && EncounterInfo.currentTurn().selectedCharTurn != null && CombatProcessor.bMainCombat && !EncounterInfo.currentTurn().bPlayerMustSelectAction)
                        {
                            EncounterInfo.currentTurn().SelectCharacter();
                        }
                        else if (KeyboardMouseUtility.bMouseButtonPressed && !KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.confirmString) && !PathMoveHandler.bIsBusy && CombatProcessor.bMainCombat)
                        {
                            if (PlayerSaveData.heroParty.Find(c => c.spriteGameSize.Contains(KeyboardMouseUtility.gameMousePos)) == default(BaseCharacter))
                            {
                                EncounterInfo.currentTurn().RightButtonPlayerAction();
                            }
                            else
                            {
                                EncounterInfo.currentTurn().SelectCharacter();
                            }

                            KeyboardMouseUtility.bPressed = true;
                        }
                        else if (key.actionIndentifierString.Equals(Game1.tabKeyString) && !KeyboardMouseUtility.AnyButtonsPressed() && !PathMoveHandler.bIsBusy && EncounterInfo.currentTurn().selectedCharTurn != null && CombatProcessor.bMainCombat && !EncounterInfo.currentTurn().bPlayerMustSelectAction)
                        {
                            EncounterInfo.currentTurn().TabSelect();
                        }
                    }

                    //



                    //if (key.actionIndentifierString.Equals(Game1.cameraMoveUpString))
                    //{
                    //    GameProcessor.sceneCamera.Y += (int)(normalCameraSpeed * GameProcessor.zoom);
                    //}

                    //if (key.actionIndentifierString.Equals(Game1.cameraMoveDownString))
                    //{
                    //    GameProcessor.sceneCamera.Y -= (int)(normalCameraSpeed * GameProcessor.zoom);
                    //}

                    //if (key.actionIndentifierString.Equals(Game1.cameraMoveLeftString))
                    //{
                    //    GameProcessor.sceneCamera.X += (int)(normalCameraSpeed * GameProcessor.zoom);
                    //}

                    //if (key.actionIndentifierString.Equals(Game1.cameraMoveRightString))
                    //{
                    //    GameProcessor.sceneCamera.X -= (int)(normalCameraSpeed * GameProcessor.zoom);
                    //}
                    #endregion
                }
                else if (EncounterInfo.encounterGroups.Count > 0 && EncounterInfo.currentTurn().bPlayerMustSelectAction)
                {
                    if (key.actionIndentifierString.Equals(Game1.cancelString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        EncounterInfo.currentTurn().bPlayerMustSelectAction = false;
                        KeyboardMouseUtility.bPressed = true;
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveUpString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        BattleGUI.PopUpPreviousChoice();
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveDownString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        BattleGUI.PopUpNextChoice();
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveLeftString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        BattleGUI.SelectPreviousCharacter();
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveRightString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        BattleGUI.SelectNextCharacter();
                    }

                    if ((key.actionIndentifierString.Equals(Game1.confirmString) || key.actionIndentifierString.Equals(Game1.openMenuString)) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        // var temp = EncounterInfo.currentTurn().selectedCharTurn.character;
                        switch (BattleGUI.returnCurrentPopUpMenuSelection())
                        {
                            case (int)BattleGUI.PopUpChoices.Attack:
                                if (BattleGUI.selectedTarget != null && EncounterInfo.currentTurn().selectedCharTurn.character.UsableAbilityList(BattleGUI.selectedTarget).Count != 0)
                                {
                                    GameProcessor.StartBattleSoon(EncounterInfo.currentTurn().selectedCharTurn.character, BattleGUI.selectedTarget);
                                }
                                break;
                            case (int)BattleGUI.PopUpChoices.Defend:
                                EncounterInfo.currentTurn().selectedCharTurn.character.bSaveAP = true;
                                BattleGUI.DefendOption(EncounterInfo.currentTurn().selectedCharTurn.character, BattleGUI.selectedTarget);
                                EncounterInfo.currentTurn().FinalizeCharacterRound();
                                break;
                            case (int)BattleGUI.PopUpChoices.Item:
                                BattleGUI.ItemsOption();
                                break;
                        }

                    }
                }

            }

            if (!KeyboardMouseUtility.bMouseButtonPressed && (key.actionIndentifierString.Equals(Game1.confirmString) || key.actionIndentifierString.Equals(Game1.openMenuString)) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (CombatProcessor.bStartPhase1)
                {
                    CombatProcessor.StartMainCombat();
                    EncounterInfo.ChangeGroup(0);
                }
            }

        }

        private static void NoSpriteSelectedControls(ActionKey key)
        {

            int normalCameraSpeed = 3;

            if (EncounterInfo.currentTurn().bIsPlayerTurnSet && !BattleGUI.bIsRunning)
            {
                if (KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed() && !PathMoveHandler.bIsBusy && EncounterInfo.currentTurn().selectedCharTurn == null && CombatProcessor.bMainCombat && !EncounterInfo.currentTurn().bPlayerMustSelectAction)
                {
                    EncounterInfo.currentTurn().SelectCharacter();
                    KeyboardMouseUtility.bPressed = true;
                }

                if (KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.cancelString) && !KeyboardMouseUtility.AnyButtonsPressed() && !PathMoveHandler.bIsBusy && EncounterInfo.currentTurn().selectedCharTurn == null && CombatProcessor.bMainCombat && !EncounterInfo.currentTurn().bPlayerMustSelectAction)
                {
                    EncounterInfo.currentTurn().RightButtonPlayerAction();
                    KeyboardMouseUtility.bPressed = true;
                }

                if (key.actionIndentifierString.Equals(Game1.tabKeyString) && !KeyboardMouseUtility.AnyButtonsPressed() && !PathMoveHandler.bIsBusy && CombatProcessor.bMainCombat && !EncounterInfo.currentTurn().bPlayerMustSelectAction)
                {
                    EncounterInfo.currentTurn().TabSelect();
                }
            }

            if (EncounterInfo.currentTurn().selectedCharTurn == null)
            {
                if (key.actionIndentifierString.Equals(Game1.moveUpString) || key.actionIndentifierString.Equals(Game1.cameraMoveUpString))
                {
                    GameProcessor.sceneCamera.Y += (normalCameraSpeed / GameProcessor.zoom) * SettingsFile.speedModCamera;
                }

                if (key.actionIndentifierString.Equals(Game1.moveDownString) || key.actionIndentifierString.Equals(Game1.cameraMoveDownString))
                {
                    GameProcessor.sceneCamera.Y -= (normalCameraSpeed / GameProcessor.zoom) * SettingsFile.speedModCamera;
                }

                if (key.actionIndentifierString.Equals(Game1.moveLeftString) || key.actionIndentifierString.Equals(Game1.cameraMoveLeftString))
                {
                    GameProcessor.sceneCamera.X += (normalCameraSpeed / GameProcessor.zoom) * SettingsFile.speedModCamera;
                }


                if (key.actionIndentifierString.Equals(Game1.moveRightString) || key.actionIndentifierString.Equals(Game1.cameraMoveRightString))
                {
                    GameProcessor.sceneCamera.X -= (normalCameraSpeed / GameProcessor.zoom) * SettingsFile.speedModCamera;
                }
            }
        }

        private static void GlobalControls(List<Actions.ActionKey> keys)
        {
            if (!BattleGUI.bIsRunning && !TurnSet.bSelectingArea && EncounterInfo.encounterGroups.Count != 0)
            {
                if (!KeyboardMouseUtility.AnyButtonsPressed() && EncounterInfo.currentTurn().bIsPlayerTurnSet && KeyboardMouseUtility.bMouseButtonPressed && keys.Last().actionIndentifierString.Equals(Game1.confirmString))
                {
                    BattleGUI.HandleLowerTabSelect();

                }

            }
            //if (keys[keys.Count - 1].actionIndentifierString.Equals(Game1.openMenuString) && !KeyboardMouseUtility.AnyButtonsPressed() && !BattleGUI.bIsRunning && EncounterInfo.currentTurn().bIsPlayerTurnSet)
            //{
            //    GameProcessor.bInGameMenu = true;
            //    GameMenuHandler.Start();
            //    KeyboardMouseUtility.bPressed = true;
            //    GameProcessor.EnableMenuStage();
            //}
        }
    }
}
