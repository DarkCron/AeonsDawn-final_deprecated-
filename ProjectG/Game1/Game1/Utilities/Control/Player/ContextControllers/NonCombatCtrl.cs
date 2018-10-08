using Game1.Forms.PlayTestForms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TBAGW.Utilities.Actions;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Utilities.Control.Player
{
    static public class NonCombatCtrl
    {
        const int Speed = 5;
        static GenerateRandomZoneEncounterForm zoneInfoForm = new GenerateRandomZoneEncounterForm();
        static public int ri = (int)BaseSprite.Rotation.Down;
        static public ControlSetup currentControls = ControlSetup.Standard;
        static UIElement currentlyControlledScreen;

        static public void changeToPickUpScreen(UIElement screen)
        {
            currentControls = ControlSetup.LootPickUp;
            currentlyControlledScreen = screen;
        }

        public enum ControlSetup { Standard = 0, LootPickUp }

        static public void Reset()
        {
            ri = (int)BaseSprite.Rotation.Down;
        }

        static public void Update(List<Actions.ActionKey> keys)
        {
            switch (currentControls)
            {
                case ControlSetup.Standard:
                    foreach (var key in keys)
                    {
                        StandardControls(key);
                    }

                    break;
                case ControlSetup.LootPickUp:
                    if (keys.Count > 0)
                    {
                        LootPickUpControls(keys.Last());
                    }
                    break;
                default:
                    break;
            }


            if (keys.Count == 0)
            {
                if (typeof(BaseCharacter) == PlayerController.selectedSprite.GetType())
                {
                    (PlayerController.selectedSprite as BaseCharacter).animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                }

            }


            if (keys[keys.Count - 1].actionIndentifierString.Equals(Game1.openMenuString) && !KeyboardMouseUtility.AnyButtonsPressed() && !BattleGUI.bIsRunning)
            {
                GameProcessor.bInGameMenu = true;
                GameMenuHandler.Start();
                KeyboardMouseUtility.bPressed = true;
                GameProcessor.EnableMenuStage();
            }

            if (keys[keys.Count - 1].actionIndentifierString.Equals(Game1.SettingsMenu) && !KeyboardMouseUtility.AnyButtonsPressed() && !BattleGUI.bIsRunning)
            {
                SettingsMenu.Start();
            }

            PlayerController.selectedSprite.rotationIndex = ri;

            if (GameProcessor.bCameraOnController)
            {
                //   GameProcessor.sceneCamera = ((new Vector2(-(PlayerController.selectedSprite.position.X + 32 - 1366 / GameProcessor.zoom / 2), -(PlayerController.selectedSprite.position.Y + 32 - 768 / GameProcessor.zoom / 2))));
            }

            /*
            var region = GameProcessor.loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite));
            if (region != null)
            {
                var zone = region.regionZones.Find(z => z.Contains(PlayerController.selectedSprite));
                if (zone != null)
                {
                    zoneInfoForm.Start(zone, region);
                }
            }
            */
        }

        private static void LootPickUpControls(ActionKey key)
        {
            if (key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed() && !KeyboardMouseUtility.bMouseButtonPressed && !KeyboardMouseUtility.bMousePressed)
            {
                (currentlyControlledScreen as PickUpScreen).TryPickUp();
            }

            if (key.actionIndentifierString.Equals(Game1.confirmString) && KeyboardMouseUtility.bMouseButtonPressed && !KeyboardMouseUtility.bMousePressed)
            {
                (currentlyControlledScreen as PickUpScreen).Clicked();
            }

            if (key.actionIndentifierString.Equals(Game1.confirmString) && KeyboardMouseUtility.bMouseButtonPressed && (currentlyControlledScreen as PickUpScreen).bBeingDragged)
            {
                (currentlyControlledScreen as PickUpScreen).Drag();
            }

            if (key.actionIndentifierString.Equals(Game1.cameraMoveDownString) && !ScriptProcessor.bIsRunning && (currentlyControlledScreen.Location().Y + currentlyControlledScreen.Location().Height < 768))
            {
                Vector2 movement = new Vector2(0, 5);
                (currentlyControlledScreen as PickUpScreen).MoveRenderControl(movement);
            }

            if (key.actionIndentifierString.Equals(Game1.cameraMoveUpString) && !ScriptProcessor.bIsRunning && (currentlyControlledScreen.Location().Y > 0))
            {
                Vector2 movement = new Vector2(0, -5);
                (currentlyControlledScreen as PickUpScreen).MoveRenderControl(movement);
            }

            if (key.actionIndentifierString.Equals(Game1.cameraMoveLeftString) && !ScriptProcessor.bIsRunning && (currentlyControlledScreen.Location().X > 0))
            {
                Vector2 movement = new Vector2(-5, 0);
                (currentlyControlledScreen as PickUpScreen).MoveRenderControl(movement);
            }

            if (key.actionIndentifierString.Equals(Game1.cameraMoveRightString) && !ScriptProcessor.bIsRunning && (currentlyControlledScreen.Location().X + currentlyControlledScreen.Location().Width < 1366))
            {
                Vector2 movement = new Vector2(5, 0);
                (currentlyControlledScreen as PickUpScreen).MoveRenderControl(movement);
            }




            if ((currentlyControlledScreen as PickUpScreen).getPanels().Count != 0)
            {
                bool bMoved = false;
                var temp = (currentlyControlledScreen as PickUpScreen).getPanels();
                int selectIndex = 0;
                selectIndex = temp.IndexOf((currentlyControlledScreen as PickUpScreen).selectedItemPanel);

                if (key.actionIndentifierString.Equals(Game1.moveDownString) && !ScriptProcessor.bIsRunning && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    selectIndex++;
                    bMoved = true;
                }

                if (key.actionIndentifierString.Equals(Game1.moveUpString) && !ScriptProcessor.bIsRunning && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    selectIndex--;
                    bMoved = true;
                }

                if (key.actionIndentifierString.Equals(Game1.moveLeftString) && !ScriptProcessor.bIsRunning && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    selectIndex -= 4;
                    bMoved = true;
                }

                if (key.actionIndentifierString.Equals(Game1.moveRightString) && !ScriptProcessor.bIsRunning && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    selectIndex += 4;
                    bMoved = true;
                }

                if (selectIndex >= temp.Count)
                {
                    selectIndex = 0;
                }
                else if (selectIndex < 0)
                {
                    selectIndex = temp.Count - 1;
                }



                if (bMoved)
                {
                    (currentlyControlledScreen as PickUpScreen).selectedItemPanel = temp[selectIndex];
                    (currentlyControlledScreen as PickUpScreen).verticalModifier = temp[selectIndex].End();

                    (currentlyControlledScreen as PickUpScreen).itemPanelScrollMatrix = Matrix.CreateTranslation(0, -(currentlyControlledScreen as PickUpScreen).verticalModifier, 0);
                    (currentlyControlledScreen as PickUpScreen).openDifferentDescription();
                }
            }




        }

        static public void StandardControls(ActionKey key)
        {
            var c = (PlayerController.selectedSprite as BaseCharacter);
            c.speed = 3f;
            if (PlayerController.selectedSprite.groundTile.tileSource.tileType != TileSource.TileType.Stairs && (key.actionIndentifierString.Equals(Game1.moveUpString) || key.actionIndentifierString.Equals(Game1.cameraMoveUpString)) && !ScriptProcessor.bIsRunning)
            {
                ri = (int)BaseSprite.Rotation.Up;
                if (PlayerController.selectedSprite.GetType() == typeof(BaseCharacter))
                {
                    (PlayerController.selectedSprite as BaseCharacter).Move(new Vector2(0, -c.speed));
                }
                else
                {
                    PlayerController.selectedSprite.Move(new Vector2(0, -c.speed));
                }
            }
            else
            if (PlayerController.selectedSprite.groundTile.tileSource.tileType == TileSource.TileType.Stairs && (key.actionIndentifierString.Equals(Game1.moveUpString) || key.actionIndentifierString.Equals(Game1.cameraMoveUpString)) && !ScriptProcessor.bIsRunning)
            {
                ri = (int)BaseSprite.Rotation.Up;
            }
            else
            if (PlayerController.selectedSprite.groundTile.tileSource.tileType != TileSource.TileType.Stairs && (key.actionIndentifierString.Equals(Game1.moveDownString) || key.actionIndentifierString.Equals(Game1.cameraMoveDownString)) && !ScriptProcessor.bIsRunning)
            {
                ri = (int)BaseSprite.Rotation.Down;
                if (PlayerController.selectedSprite.GetType() == typeof(BaseCharacter))
                {
                    (PlayerController.selectedSprite as BaseCharacter).Move(new Vector2(0, c.speed));
                }
                else
                {
                    PlayerController.selectedSprite.Move(new Vector2(0, c.speed));
                }
            }
            else
            if (PlayerController.selectedSprite.groundTile.tileSource.tileType == TileSource.TileType.Stairs && (key.actionIndentifierString.Equals(Game1.moveDownString) || key.actionIndentifierString.Equals(Game1.cameraMoveDownString)) && !ScriptProcessor.bIsRunning)
            {
                ri = (int)BaseSprite.Rotation.Down;
            }
            else
            if ((key.actionIndentifierString.Equals(Game1.moveLeftString) || key.actionIndentifierString.Equals(Game1.cameraMoveLeftString)) && !ScriptProcessor.bIsRunning)
            {
                ri = (int)BaseSprite.Rotation.Left;
                if (PlayerController.selectedSprite.GetType() == typeof(BaseCharacter))
                {
                    (PlayerController.selectedSprite as BaseCharacter).Move(new Vector2(-c.speed, 0));
                }
                else
                {
                    PlayerController.selectedSprite.Move(new Vector2(-c.speed, 0));
                }
            }
            else

            if ((key.actionIndentifierString.Equals(Game1.moveRightString) || key.actionIndentifierString.Equals(Game1.cameraMoveRightString)) && !ScriptProcessor.bIsRunning)
            {
                ri = (int)BaseSprite.Rotation.Right;
                if (PlayerController.selectedSprite.GetType() == typeof(BaseCharacter))
                {
                    (PlayerController.selectedSprite as BaseCharacter).Move(new Vector2(c.speed, 0));
                }
                else
                {
                    PlayerController.selectedSprite.Move(new Vector2(c.speed, 0));
                }
            }

            if (key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {

                if (GameProcessor.loadedMap.mapSaveInfo.CanPickUp(PlayerController.selectedSprite))
                {
                    GameProcessor.loadedMap.mapSaveInfo.HandlePickUp();
                }
                else
                {
                    if (PlayerController.selectedSprite.GetType() == typeof(BaseCharacter))
                    {
                        (PlayerController.selectedSprite as BaseCharacter).Interact();
                    }
                    else
                    {
                        PlayerController.selectedSprite.Interact();
                    }
                }


            }

            if (key.actionIndentifierString.Equals(Game1.cameraDefaultZoom))
            {
                GameProcessor.zoom = 2.1f;
            }

            if (key.actionIndentifierString.Equals(Game1.cameraZoomIn))
            {
                GameProcessor.zoom += 0.01f;
            }

            if (key.actionIndentifierString.Equals(Game1.cameraZoomOut))
            {
                GameProcessor.zoom -= 0.01f;
            }

            if (key.actionIndentifierString.Equals(Game1.pauseString) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (typeof(BaseCharacter) == PlayerController.selectedSprite.GetType())
                {
                    try
                    {
                        var temp = PlayerSaveData.heroParty.IndexOf(PlayerController.selectedSprite as BaseCharacter) + 1;
                        if (temp == PlayerSaveData.heroParty.Count)
                        {
                            temp = 0;
                        }
                        Vector2 pos = PlayerController.selectedSprite.position;
                        var oi = MapChunk.consideredSprites.FindAll(s => s.objectType == objectInfo.type.Character).Find(s => s.obj == PlayerController.selectedSprite);
                        var oil = MapChunk.consideredSprites.FindAll(s => s.objectType == objectInfo.type.Character);
                        PlayerController.selectedSprite = PlayerSaveData.heroParty[temp];
                        oi.obj = PlayerController.selectedSprite;
                        PlayerController.selectedSprite.position = pos;
                        PlayerController.selectedSprite.UpdatePosition();
                        GameProcessor.cameraFollowTarget = PlayerController.selectedSprite;
                    }
                    catch
                    {

                    }

                }

            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && key.actionIndentifierString.Equals(Game1.SettingsMenu))
            {
                SettingsMenu.Start();
            }
        }
    }
}
