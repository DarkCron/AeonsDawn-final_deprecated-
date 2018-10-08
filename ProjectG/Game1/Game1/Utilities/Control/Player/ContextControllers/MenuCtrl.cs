using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TBAGW.Utilities.Actions;

namespace TBAGW.Utilities.Control.Player
{
    static public class MenuCtrl
    {
        static public void Update(List<Actions.ActionKey> keys)
        {
            if (keys.Count != 0)
            {
                GlobalControls(keys);
                switch (GameMenuHandler.currentPage)
                {
                    case GameMenuHandler.GameMenuPages.EquipmentPage:
                        SpecificEquipPageControls(keys[keys.Count - 1]);
                        break;
                    case GameMenuHandler.GameMenuPages.ItemsPage:
                        SpecificItemPageControls(keys[keys.Count - 1]);
                        break;
                    case GameMenuHandler.GameMenuPages.QuestPage:
                        if (keys[keys.Count - 1].actionIndentifierString.Equals(Game1.tabKeyString) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            GameMenuHandler.TabClick();
                        }
                        break;
                    case GameMenuHandler.GameMenuPages.CharactersPage:
                        if (GameMenuHandler.selectedCharacterContext == null)
                        {
                            SpecificCharactersPageControls(keys[keys.Count - 1]);
                        }
                        else
                        {
                            SpecificCharacterContextPageControls(keys[keys.Count - 1]);
                        }

                        if (keys[keys.Count - 1].actionIndentifierString.Equals(Game1.tabKeyString) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            GameMenuHandler.TabClick();
                        }
                        break;
                    case GameMenuHandler.GameMenuPages.MapPage:
                        if (keys[keys.Count - 1].actionIndentifierString.Equals(Game1.tabKeyString) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            GameMenuHandler.TabClick();
                        }
                        break;
                    default:
                        break;
                }
                FinalCheck(keys.Last());
                KeyboardMouseUtility.bPressed = true;
            }
        }

        private static void FinalCheck(ActionKey actionKey)
        {

            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.cancelString))
            {
                GameProcessor.bInGameMenu = false;
                if (CombatProcessor.bIsRunning)
                {
                    GameProcessor.DisableMenuStage();
                    PlayerController.currentController = PlayerController.Controllers.Combat;
                }
                else
                {
                    PlayerController.currentController = PlayerController.Controllers.NonCombat;
                }
            }
        }

        private static void SpecificCharacterContextPageControls(ActionKey key)
        {


            switch (GameMenuHandler.selectedCharacterContext.currentCharContext)
            {
                case CharacterContextMenu.CharacterContextMenuType.None:
                    break;
                case CharacterContextMenu.CharacterContextMenuType.Select:
                    #region

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.confirmString))
                    {
                        GameMenuHandler.selectedCharacterContext.HandleConfirm();
                        KeyboardMouseUtility.bPressed = true;
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.cancelString))
                    {
                        GameMenuHandler.selectedCharacterContext = null;
                        KeyboardMouseUtility.bPressed = true;
                    }
                    #endregion
                    break;
                case CharacterContextMenu.CharacterContextMenuType.AbilityLineUp:
                    #region
                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.cancelString))
                    {
                        GameMenuHandler.selectedCharacterContext.Close();
                        KeyboardMouseUtility.bPressed = true;
                    }

                    switch (GameMenuHandler.selectedCharacterContext.abilityLineupInfo.currentType)
                    {
                        case AbilityLineupInfo.controlType.Normal:
                            #region
                            if (!KeyboardMouseUtility.AnyButtonsPressed() && (key.actionIndentifierString.Equals(Game1.moveLeftString) || key.actionIndentifierString.Equals(Game1.moveRightString)))
                            {
                                GameMenuHandler.selectedCharacterContext.abilityLineupInfo.SwitchLeftRight();

                                KeyboardMouseUtility.bPressed = true;
                            }

                            if (!KeyboardMouseUtility.AnyButtonsPressed() && KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString))
                            {
                                GameMenuHandler.selectedCharacterContext.abilityLineupInfo.HandleMouseClick();
                            }
                            else if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.confirmString))
                            {
                                GameMenuHandler.selectedCharacterContext.abilityLineupInfo.HandleConfirm();
                            }


                            if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.moveDownString))
                            {
                                GameMenuHandler.selectedCharacterContext.abilityLineupInfo.HandleUpDown(true);
                            }

                            if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.moveUpString))
                            {
                                GameMenuHandler.selectedCharacterContext.abilityLineupInfo.HandleUpDown(false);
                            }

                            if (GameMenuHandler.selectedCharacterContext.abilityLineupInfo.bCursorFocusRight)
                            {
                                if (key.actionIndentifierString.Equals(Game1.cameraMoveDownString))
                                {
                                    AbilitySelectTab.AddScrollOffSetRight(-4.2f);
                                }

                                if (key.actionIndentifierString.Equals(Game1.cameraMoveUpString))
                                {
                                    AbilitySelectTab.AddScrollOffSetRight(4.2f);
                                }


                            }
                            else
                            {
                                if (key.actionIndentifierString.Equals(Game1.cameraMoveDownString))
                                {
                                    AbilitySelectTab.AddScrollOffSetLeft(-4.2f);
                                }

                                if (key.actionIndentifierString.Equals(Game1.cameraMoveUpString))
                                {
                                    AbilitySelectTab.AddScrollOffSetLeft(4.2f);
                                }
                            }
                            #endregion
                            break;
                        default:
                            break;
                    }

                    #endregion
                    break;
                case CharacterContextMenu.CharacterContextMenuType.ClassLineUp:
                    #region
                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.cancelString))
                    {
                        GameMenuHandler.selectedCharacterContext.classLineupInfo.Close();
                        KeyboardMouseUtility.bPressed = true;
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.moveDownString))
                    {
                        GameMenuHandler.selectedCharacterContext.classLineupInfo.HandleUpDown(false);
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.moveUpString))
                    {
                        GameMenuHandler.selectedCharacterContext.classLineupInfo.HandleUpDown(true);
                    }

                    if (key.actionIndentifierString.Equals(Game1.cameraMoveDownString))
                    {
                        GameMenuHandler.selectedCharacterContext.classLineupInfo.HandleScrollUpDown(4.2f);
                    }

                    if (key.actionIndentifierString.Equals(Game1.cameraMoveUpString))
                    {
                        GameMenuHandler.selectedCharacterContext.classLineupInfo.HandleScrollUpDown(-4.2f);
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString))
                    {
                        GameMenuHandler.selectedCharacterContext.classLineupInfo.HandleMouseButtonPress(KeyboardMouseUtility.uiMousePos);
                        KeyboardMouseUtility.bPressed = true;
                    }

                    if (KeyboardMouseUtility.ScrollingDown())
                    {
                        GameMenuHandler.selectedCharacterContext.classLineupInfo.HandleScrollUpDown(4.2f * 4);
                    }

                    if (KeyboardMouseUtility.ScrollingUp())
                    {
                        GameMenuHandler.selectedCharacterContext.classLineupInfo.HandleScrollUpDown(-4.2f * 4);
                    }
                    #endregion
                    break;
                case CharacterContextMenu.CharacterContextMenuType.ClassPointLineUp:
                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.cancelString))
                    {
                        GameMenuHandler.selectedCharacterContext.classPointLineupInfo.Close();
                        KeyboardMouseUtility.bPressed = true;
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveDownString))
                    {
                        GameMenuHandler.selectedCharacterContext.classPointLineupInfo.HandleUpDown(false);
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveUpString))
                    {
                        GameMenuHandler.selectedCharacterContext.classPointLineupInfo.HandleUpDown(true);
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveLeftString))
                    {
                        GameMenuHandler.selectedCharacterContext.classPointLineupInfo.HandleLeft(false);
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveRightString))
                    {
                        GameMenuHandler.selectedCharacterContext.classPointLineupInfo.HandleRight(true);
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.cameraMoveDownString))
                    {
                        GameMenuHandler.selectedCharacterContext.classPointLineupInfo.SelectUpTalentNode();
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.cameraMoveUpString))
                    {
                        GameMenuHandler.selectedCharacterContext.classPointLineupInfo.SelectDownTalentNode();
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.cameraMoveLeftString))
                    {
                        GameMenuHandler.selectedCharacterContext.classPointLineupInfo.SelectLeftTalentNode();
                    }

                    if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.cameraMoveRightString))
                    {
                        GameMenuHandler.selectedCharacterContext.classPointLineupInfo.SelectRightTalentNode();
                    }
                    break;
                default:
                    break;
            }


        }

        private static void SpecificCharactersPageControls(ActionKey key)
        {
            bool bChangeMatrix = false;
            if (GameMenuHandler.charTabList.Count > 2 && key.actionIndentifierString.Equals(Game1.cameraMoveDownString) && -GameMenuHandler.characterTabVerticalModifier <= GameMenuHandler.charTabList[GameMenuHandler.charTabList.Count - 2].tabLocation.Y - 25)
            {
                GameMenuHandler.characterTabVerticalModifier -= 4.2f;
                bChangeMatrix = true;
            }

            if (GameMenuHandler.charTabList.Count > 2 && key.actionIndentifierString.Equals(Game1.cameraMoveUpString) && GameMenuHandler.characterTabVerticalModifier < 0)
            {
                GameMenuHandler.characterTabVerticalModifier += 4.2f;
                bChangeMatrix = true;
            }

            if (GameMenuHandler.CharacterTabContainsMouse())
            {

                if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.confirmString))
                {
                    var selectedTab = GameMenuHandler.selectedCharacterTab();
                    if (!selectedTab.Equals(default(GameMenuHandler.CharacterTabdisplay)))
                    {
                        GameMenuHandler.selectedCharacterContext = new CharacterContextMenu(GameMenuHandler.characterTabVerticalModifier, new Point(100, 50), new Point(100, 150), selectedTab);
                    }
                }
            }

            if (GameMenuHandler.charTabList.Count > 2 && KeyboardMouseUtility.ScrollingDown() && -GameMenuHandler.characterTabVerticalModifier <= GameMenuHandler.charTabList[GameMenuHandler.charTabList.Count - 2].tabLocation.Y - 25)
            {
                GameMenuHandler.characterTabVerticalModifier -= 4.2f * 4;
                bChangeMatrix = true;
            }

            if (GameMenuHandler.charTabList.Count > 2 && KeyboardMouseUtility.ScrollingUp() && GameMenuHandler.characterTabVerticalModifier < 0)
            {
                GameMenuHandler.characterTabVerticalModifier += 4.2f * 4;
                bChangeMatrix = true;
            }

            if (bChangeMatrix)
            {

                GameMenuHandler.characterTabAdjustedMatrix = Matrix.CreateTranslation(new Vector3(0, GameMenuHandler.characterTabVerticalModifier, 1));
            }


        }

        private static void SpecificItemPageControls(ActionKey key)
        {
            if (!GameMenuHandler.bChooseCharacterToUseOn)
            {
                if (key.actionIndentifierString.Equals(Game1.tabKeyString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.TabClick();
                }

                if (KeyboardMouseUtility.bMouseButtonPressed && KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.itemsTabLeftClick();
                }


                if (KeyboardMouseUtility.bMouseButtonPressed && KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.cancelString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.itemsTabRightClick();
                    KeyboardMouseUtility.bPressed = true;
                }

                if (GameMenuHandler.bDisplayOptions)
                {
                    if (key.actionIndentifierString.Equals(Game1.moveUpString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        GameMenuHandler.itemOptionSelectionIndex--;

                    }

                    if (key.actionIndentifierString.Equals(Game1.moveDownString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        GameMenuHandler.itemOptionSelectionIndex++;

                    }

                    if (GameMenuHandler.itemOptionSelectionIndex == ItemOptionDisplay.choiceBoxes.Count)
                    {
                        GameMenuHandler.itemOptionSelectionIndex = 0;
                    }

                    if (GameMenuHandler.itemOptionSelectionIndex < 0)
                    {
                        GameMenuHandler.itemOptionSelectionIndex = ItemOptionDisplay.choiceBoxes.Count - 1;
                    }

                    if (!KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        GameMenuHandler.SelectItemOptionViaKeyboardDisplay();
                    }
                }
                else if (!GameMenuHandler.bDisplayOptions)
                {
                    int index = GameMenuHandler.onlyTheseItemsToConsider.IndexOf(GameMenuHandler.itemPageSelectedItem);
                    bool bChange = false;

                    if (key.actionIndentifierString.Equals(Game1.moveUpString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        index -= 10;
                        bChange = true;
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveDownString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        index += 10;
                        bChange = true;
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveLeftString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        index--;
                        bChange = true;
                    }

                    if (key.actionIndentifierString.Equals(Game1.moveRightString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        index++;
                        bChange = true;
                    }

                    if (!KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
                    {
                        GameMenuHandler.SelectItemViaKeyboardDisplayOptions();
                    }


                    if (bChange)
                    {
                        if (index < 0)
                        {
                            if (GameMenuHandler.maxUpperPage != 0 && GameMenuHandler.upperCurrentIndex >= 0)
                            {
                                GameMenuHandler.upperCurrentIndex--;
                                GameMenuHandler.RegenerateItemList();
                                index = GameMenuHandler.onlyTheseItemsToConsider.Count - 1;
                                if (GameMenuHandler.upperCurrentIndex == 0 - 1)
                                {
                                    GameMenuHandler.upperCurrentIndex = GameMenuHandler.maxUpperPage;
                                    GameMenuHandler.RegenerateItemList();
                                    index = GameMenuHandler.onlyTheseItemsToConsider.Count - 1;
                                }
                            }
                            else
                            {

                                GameMenuHandler.upperCurrentIndex = 0;
                                GameMenuHandler.RegenerateItemList();
                                index = GameMenuHandler.onlyTheseItemsToConsider.Count - 1;
                            }
                        }
                        else if (index >= GameMenuHandler.onlyTheseItemsToConsider.Count)
                        {
                            if (GameMenuHandler.maxUpperPage != 0 && GameMenuHandler.upperCurrentIndex != GameMenuHandler.maxUpperPage)
                            {
                                GameMenuHandler.upperCurrentIndex++;
                                GameMenuHandler.RegenerateItemList();
                                index = 0;
                            }
                            else
                            {

                                GameMenuHandler.upperCurrentIndex = 0;
                                GameMenuHandler.RegenerateItemList();
                                index = 0;
                            }

                        }

                        if (bChange && GameMenuHandler.onlyTheseItemsToConsider.Count != 0)
                        {
                            GameMenuHandler.itemPageSelectedItem = GameMenuHandler.onlyTheseItemsToConsider[index];
                            GameMenuHandler.SelectItemViaKeyboard(GameMenuHandler.itemPageSelectedItem, index);
                        }

                    }


                }
            }
            else if (GameMenuHandler.bChooseCharacterToUseOn)
            {
                if (key.actionIndentifierString.Equals(Game1.cameraMoveDownString) && ((float)((float)-GameMenuHandler.verticalModifier / (float)GameMenuHandler.maxVertical)) <= 1)
                {
                    GameMenuHandler.verticalModifier -= 4;
                }

                if (key.actionIndentifierString.Equals(Game1.cameraMoveUpString) && ((float)((float)-GameMenuHandler.verticalModifier / (float)GameMenuHandler.maxVertical)) >= 0)
                {
                    GameMenuHandler.verticalModifier += 4;
                }

                if (KeyboardMouseUtility.bMouseButtonPressed && KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.itemsTabLeftClickCharacter();
                }

                if (key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    if (GameMenuHandler.selectedCharacterItems != null)
                    {
                        GameMenuHandler.UseItemOnViaKeyboard();
                    }
                }

                int index = PlayerSaveData.heroParty.IndexOf(GameMenuHandler.selectedCharacterItems);
                bool bChange = false;

                if (key.actionIndentifierString.Equals(Game1.moveUpString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    index -= 1;
                    bChange = true;
                }

                if (key.actionIndentifierString.Equals(Game1.moveDownString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    index += 1;
                    bChange = true;
                }

                if (bChange)
                {
                    if (index <= -1)
                    {
                        GameMenuHandler.selectedCharacterItems = null;
                    }
                    else if (index >= PlayerSaveData.heroParty.Count)
                    {
                        GameMenuHandler.selectedCharacterItems = null;
                    }
                    else
                    {
                        GameMenuHandler.selectedCharacterItems = PlayerSaveData.heroParty[index];
                        GameMenuHandler.verticalModifier = (-GameMenuHandler.maxVertical / PlayerSaveData.heroParty.Count) * index;
                    }
                }
            }
        }

        internal static void MouseMove()
        {
            switch (GameMenuHandler.currentPage)
            {
                case GameMenuHandler.GameMenuPages.EquipmentPage:
                    break;
                case GameMenuHandler.GameMenuPages.ItemsPage:
                    break;
                case GameMenuHandler.GameMenuPages.QuestPage:
                    break;
                case GameMenuHandler.GameMenuPages.CharactersPage:
                    if (GameMenuHandler.selectedCharacterContext != null)
                    {
                        GameMenuHandler.selectedCharacterContext.HandleMouseOver();
                    }

                    break;
                case GameMenuHandler.GameMenuPages.MapPage:
                    break;
                default:
                    break;
            }
        }

        private static void SpecificEquipPageControls(Actions.ActionKey key)
        {
            if (key.actionIndentifierString.Equals(Game1.cameraMoveDownString) && ((float)((float)-GameMenuHandler.verticalModifier / (float)GameMenuHandler.maxVertical)) <= 1)
            {
                GameMenuHandler.verticalModifier -= 4;
            }

            if (key.actionIndentifierString.Equals(Game1.cameraMoveUpString) && ((float)((float)-GameMenuHandler.verticalModifier / (float)GameMenuHandler.maxVertical)) >= 0)
            {
                GameMenuHandler.verticalModifier += 4;
            }

            if (KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.cancelString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                GameMenuHandler.EquipmentTabRightClick();
                KeyboardMouseUtility.bPressed = true;
            }

            if (KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                GameMenuHandler.EquipmentTabLeftClick();
            }

            if (!GameMenuHandler.bDisplayOptions && GameMenuHandler.selectedEquipmentPieceCharacterPanel == null)
            {
                int index = PlayerSaveData.heroParty.IndexOf(GameMenuHandler.selectedCharacterEquipment);
                bool bChange = false;

                if (key.actionIndentifierString.Equals(Game1.moveUpString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    index -= 1;
                    bChange = true;
                }

                if (key.actionIndentifierString.Equals(Game1.moveDownString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    index += 1;
                    bChange = true;
                }


                if (key.actionIndentifierString.Equals(Game1.cancelString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.selectedCharacterEquipment = null;
                    KeyboardMouseUtility.bPressed = true;
                }




                if (bChange)
                {
                    if (index <= -1)
                    {
                        GameMenuHandler.selectedCharacterEquipment = PlayerSaveData.heroParty[PlayerSaveData.heroParty.Count - 1];
                        GameMenuHandler.verticalModifier = (-GameMenuHandler.maxVertical / PlayerSaveData.heroParty.Count) * (PlayerSaveData.heroParty.Count - 1);
                    }
                    else if (index >= PlayerSaveData.heroParty.Count)
                    {
                        GameMenuHandler.selectedCharacterEquipment = null;
                    }
                    else
                    {
                        GameMenuHandler.selectedCharacterEquipment = PlayerSaveData.heroParty[index];
                        GameMenuHandler.verticalModifier = (-GameMenuHandler.maxVertical / PlayerSaveData.heroParty.Count) * index;
                    }
                }
            }

            if (!GameMenuHandler.bDisplayOptions && GameMenuHandler.selectedCharacterEquipment != null && GameMenuHandler.selectedEquipmentPiece == null)
            {
                if (!KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.tabKeyString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    if (GameMenuHandler.selectedCharacterEquipment.weapon != GameMenuHandler.selectedEquipmentPieceCharacterPanel)
                    {
                        GameMenuHandler.selectedEquipmentPieceCharacterPanel = GameMenuHandler.selectedCharacterEquipment.weapon;
                        GameMenuHandler.bWeaponTab = true;
                        GameMenuHandler.EquipmentCurrentPage = 1;
                        GameMenuHandler.bDisplayOptions = false;
                        GameMenuHandler.equipmentOptionSelectionIndex = 0;
                        GameMenuHandler.selectedEquipmentPiece = null;
                        GameMenuHandler.RegenerateEquipList();
                    }
                    else
                    {
                        GameMenuHandler.selectedEquipmentPieceCharacterPanel = GameMenuHandler.selectedCharacterEquipment.armour;
                        GameMenuHandler.bWeaponTab = false;
                        GameMenuHandler.EquipmentCurrentPage = 1;
                        GameMenuHandler.bDisplayOptions = false;
                        GameMenuHandler.equipmentOptionSelectionIndex = 0;
                        GameMenuHandler.selectedEquipmentPiece = null;
                        GameMenuHandler.RegenerateEquipList();
                    }
                }

                if (key.actionIndentifierString.Equals(Game1.cancelString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.selectedEquipmentPieceCharacterPanel = null;
                    KeyboardMouseUtility.bPressed = true;
                }
            }

            if (!GameMenuHandler.bDisplayOptions && GameMenuHandler.selectedCharacterEquipment != null && GameMenuHandler.selectedEquipmentPieceCharacterPanel != null ||
            (!GameMenuHandler.bDisplayOptions && GameMenuHandler.selectedCharacterEquipment != null && GameMenuHandler.bWeaponTab && GameMenuHandler.selectedCharacterEquipment.weapon == null) ||
               (!GameMenuHandler.bDisplayOptions && GameMenuHandler.selectedCharacterEquipment != null && !GameMenuHandler.bWeaponTab && GameMenuHandler.selectedCharacterEquipment.armour == null))
            {
                if (key.actionIndentifierString.Equals(Game1.cancelString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.selectedEquipmentPiece = null;
                    KeyboardMouseUtility.bPressed = true;
                }

                if (!KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.EquipmentShowChoices(GameMenuHandler.equipmentItems.IndexOf(GameMenuHandler.selectedEquipmentPiece));
                    KeyboardMouseUtility.bPressed = true;
                }

                int index = GameMenuHandler.equipmentItems.IndexOf(GameMenuHandler.selectedEquipmentPiece);
                bool bChange = false;

                if (key.actionIndentifierString.Equals(Game1.moveUpString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    index -= 6;
                    bChange = true;
                }

                if (key.actionIndentifierString.Equals(Game1.moveDownString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    index += 6;
                    bChange = true;

                }

                if (key.actionIndentifierString.Equals(Game1.moveLeftString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    index--;
                    bChange = true;
                }

                if (key.actionIndentifierString.Equals(Game1.moveRightString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    index++;
                    bChange = true;
                }

                if (bChange)
                {
                    if (index < 0)
                    {
                        if (GameMenuHandler.EquipmentMaxPage != 0 && GameMenuHandler.EquipmentCurrentPage >= 0)
                        {
                            GameMenuHandler.EquipmentCurrentPage--;
                            index = GameMenuHandler.equipmentItems.Count - 1;
                            if (GameMenuHandler.EquipmentCurrentPage == 0 - 1)
                            {
                                GameMenuHandler.EquipmentCurrentPage = GameMenuHandler.EquipmentMaxPage;
                                GameMenuHandler.RegenerateItemList();
                                index = GameMenuHandler.equipmentItems.Count - 1;
                            }
                        }
                        else
                        {

                            GameMenuHandler.EquipmentCurrentPage = 0;
                            GameMenuHandler.RegenerateItemList();
                            index = GameMenuHandler.equipmentItems.Count - 1;
                        }
                    }
                    else if (index >= GameMenuHandler.equipmentItems.Count)
                    {
                        if (GameMenuHandler.EquipmentMaxPage != 0 && GameMenuHandler.EquipmentCurrentPage != GameMenuHandler.EquipmentMaxPage)
                        {
                            GameMenuHandler.EquipmentCurrentPage++;
                            GameMenuHandler.RegenerateItemList();
                            index = 0;
                        }
                        else
                        {

                            GameMenuHandler.EquipmentCurrentPage = 0;
                            GameMenuHandler.RegenerateItemList();

                            index = 0;
                        }

                    }

                    if (bChange && GameMenuHandler.equipmentItems.Count != 0)
                    {
                        GameMenuHandler.selectedEquipmentPiece = GameMenuHandler.equipmentItems[index];
                        GameMenuHandler.selectedItemHighlighterPosition.Location = GameMenuHandler.equipmentItemBoxes[index].Location;
                        GameMenuHandler.equipmentDisplay.Generate(index);
                        //GameMenuHandler.SelectItemViaKeyboard(GameMenuHandler.itemPageSelectedItem, index);
                    }

                }
            }

            if (GameMenuHandler.bDisplayOptions && GameMenuHandler.selectedCharacterEquipment != null && GameMenuHandler.selectedEquipmentPiece != null)
            {

                if (key.actionIndentifierString.Equals(Game1.moveUpString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.equipmentOptionSelectionIndex--;

                }

                if (key.actionIndentifierString.Equals(Game1.moveDownString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.equipmentOptionSelectionIndex++;

                }


                if (GameMenuHandler.equipmentOptionSelectionIndex == EquipmentOptionDisplay.choiceBoxes.Count)
                {
                    GameMenuHandler.equipmentOptionSelectionIndex = 0;
                }

                if (GameMenuHandler.equipmentOptionSelectionIndex < 0)
                {
                    GameMenuHandler.equipmentOptionSelectionIndex = EquipmentOptionDisplay.choiceBoxes.Count - 1;
                }

                if (!KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.cancelString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.bDisplayOptions = false;
                    KeyboardMouseUtility.bPressed = true;
                }

                if (!KeyboardMouseUtility.bMouseButtonPressed && key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.EquipChoiceConfirm();
                }
            }

            if (GameMenuHandler.selectedCharacterEquipment == null && GameMenuHandler.selectedCharacterEquipment == null)
            {
                if (key.actionIndentifierString.Equals(Game1.tabKeyString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    GameMenuHandler.TabClick();
                }
            }

        }

        private static void GlobalControls(List<ActionKey> keys)
        {
            if ((keys[keys.Count - 1].actionIndentifierString.Equals(Game1.openMenuString)) && !KeyboardMouseUtility.bMouseButtonPressed && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                GameProcessor.bInGameMenu = false;
                if (CombatProcessor.bIsRunning)
                {
                    GameProcessor.DisableMenuStage();
                    PlayerController.currentController = PlayerController.Controllers.Combat;
                }
                else
                {
                    PlayerController.currentController = PlayerController.Controllers.NonCombat;
                }
            }

            if (KeyboardMouseUtility.bMouseButtonPressed && KeyboardMouseUtility.bMouseButtonPressed && keys[keys.Count - 1].actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                GameMenuHandler.TabClick();
            }


        }
    }
}
