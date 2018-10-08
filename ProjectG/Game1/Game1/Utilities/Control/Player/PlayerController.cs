using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Utilities.Control.Player
{
    static public class PlayerController
    {
        //public enum Rotation { Up = 0, Right, Down, Left };
        static public MouseState mouseState;
        static public KeyboardState keyboardState;

        public enum Controllers { NonCombat = 0, Combat, Menu, Dialogue, SoloCombat, EXPGainScreen, StartScreen, ScriptProcessor, GameSettingsMenu, GameOptions }

        static internal Controllers currentController = Controllers.NonCombat;
        static public BaseSprite selectedSprite = null;
        static public BaseCharacter previousSelected = null;
        static public Actions.ActionKey lastPressedKey;
        static public Vector2 originalPositionCharacter = new Vector2();
        static internal Controllers previousMainController = Controllers.NonCombat;
        static internal bool anyButtonsPressed = false;

        static Point prevMousePos;
        static Point currentMousePos;
        static Point uiMousePos;
        static public bool bMouseMoved = false;

        static public void CheckMouse()
        {
            currentMousePos = Mouse.GetState().Position;
            if (currentMousePos != prevMousePos)
            {
                bMouseMoved = true;
                uiMousePos = (currentMousePos.ToVector2() / ResolutionUtility.stdScale).ToPoint();
                KeyboardMouseUtility.uiMousePos = uiMousePos;
            }
            else
            {
                bMouseMoved = false;
            }
            prevMousePos = currentMousePos;
            KeyboardMouseUtility.bMouseMoved = bMouseMoved;
        }

        static public void DirectControlSprite(BaseSprite controlled)
        {
            selectedSprite = controlled;
        }

        static public void Update(GameTime gt)
        {
            CheckMouse();
            // selectedSprite.position = new Vector2(400,300);
            anyButtonsPressed = false;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            //var wot = keyboardState.GetPressedKeys();
            //String test = "";
            //foreach (var item in wot)
            //{
            //    test += item;
            //    test += " + ";
            //}
            //Console.WriteLine(test);

            if (!KeyboardMouseUtility.AnyButtonsPressed()&&Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
               // ResolutionUtility.AdjustResolution(Game1.monitorSize.X, Game1.monitorSize.Y, Game1.graphics);
                ResolutionUtility.toggleFullscreen();
                KeyboardMouseUtility.bPressed = true;
            }




            if (keyboardState.GetPressedKeys().Length > 1)
            {
                foreach (var key in Game1.actionKeyList)
                {
                    if (keyboardState.GetPressedKeys()[keyboardState.GetPressedKeys().Length - 1] == (key.assignedActionKey))
                    {
                        lastPressedKey = key;

                    }

                }

            }
            else
            {
                foreach (var key in Game1.actionKeyList)
                {
                    if (keyboardState.IsKeyDown(key.assignedActionKey))
                    {
                        lastPressedKey = key;
                        //selectedSprite.MoverMustUpdateHitboxes = true;
                    }

                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
            { }

            //if (keyboardState.GetPressedKeys().Length >= 1)
            //{
            //    foreach (var key in Game1.actionKeyList)
            //    {
            //        if (keyboardState.IsKeyDown(key.assignedActionKey))
            //        {
            //            lastPressedKey = key;
            //            break;
            //        }

            //    }
            //}



            List<Actions.ActionKey> pressedKeys = new List<Actions.ActionKey>();
            foreach (var key in Game1.actionKeyList)
            {
                if (keyboardState.IsKeyDown(key.assignedActionKey))
                {

                    #region EDITOR PLAYTEST FUNCTION
                    if (MapBuilder.bPlayTest && Game1.bIsDebug)
                    {
                        if (key.actionIndentifierString.Equals(Game1.cancelString) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && MapBuilder.bPlayTest)
                        {
                            ReturnToEditorAction();
                            break;
                        }
                    }
                    #endregion

                    pressedKeys.Add(key);


                }
            }


            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                // KeyboardMouseUtility.bPressed = true;
                pressedKeys.Add(Game1.actionKeyList.Find(key => key.actionIndentifierString.Equals(Game1.confirmString, StringComparison.OrdinalIgnoreCase)));
                KeyboardMouseUtility.bMouseButtonPressed = true;
            }


            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                // KeyboardMouseUtility.bPressed = true;
                pressedKeys.Add(Game1.actionKeyList.Find(key => key.actionIndentifierString.Equals(Game1.cancelString, StringComparison.OrdinalIgnoreCase)));
                KeyboardMouseUtility.bMouseButtonPressed = true;
            }

            if (Mouse.GetState().RightButton != ButtonState.Pressed && Mouse.GetState().LeftButton != ButtonState.Pressed)
            {
                KeyboardMouseUtility.bMouseButtonPressed = false;
            }

            if (KeyboardMouseUtility.ScrollingDown() || KeyboardMouseUtility.ScrollingUp())
            {
                pressedKeys.Add(new Actions.ActionKey());
                pressedKeys.Last().actionIndentifierString = "Scroll";
            }

            #region NON EDITOR PLAYER CONTROLS
            if (pressedKeys.Count != 0)
            {
                // GameProcessor.bUpdateShadows = true;
                GameProcessor.bUpdateOnceMore = true;
                anyButtonsPressed = true;
                //  GameProcessor.GenerateCamera(GameProcessor.cameraFollowTarget, .06f, GameProcessor.zoom);
                if (LUA.LuaExecutionList.DemandOverride())
                {
                    LUA.LuaExecutionList.ControlOverride(pressedKeys);
                }

                switch (currentController)
                {
                    case Controllers.NonCombat:
                        NonCombatCtrl.Update(pressedKeys);
                        break;
                    case Controllers.Combat:
                        CombatCtrl.Update(pressedKeys);
                        break;
                    case Controllers.Menu:
                        MenuCtrl.Update(pressedKeys);
                        break;
                    case Controllers.Dialogue:
                        DialogueCtrl.Update(pressedKeys);
                        break;
                    case Controllers.SoloCombat:
                        SoloCombatCtrl.Update(pressedKeys);
                        break;
                    case Controllers.EXPGainScreen:
                        ExpGainCtrl.Update(pressedKeys);
                        break;
                    case Controllers.StartScreen:
                        ContextControllers.StartScreenCtrl.Update(pressedKeys);
                        break;
                    case Controllers.ScriptProcessor:
                        ScriptProcessorCtrl.Update(pressedKeys);
                        break;
                    case Controllers.GameSettingsMenu:
                        if (!SettingsMenu.bIsRunning) { currentController = Controllers.NonCombat; }
                        GameSettingsMenuCtrl.Update(pressedKeys);
                        break;
                    case Controllers.GameOptions:
                        if (!OptionsMenu.bIsRunning) { currentController = Controllers.NonCombat; }
                        GameOptionsMenuCtrl.Update(pressedKeys);
                        break;
                    default:
                        break;
                }
            }
            else if (KeyboardMouseUtility.bMouseMoved)
            {
                switch (currentController)
                {
                    case Controllers.NonCombat:
                        break;
                    case Controllers.Combat:
                        break;
                    case Controllers.Menu:
                        MenuCtrl.MouseMove();
                        break;
                    case Controllers.Dialogue:
                        break;
                    case Controllers.SoloCombat:
                        break;
                    case Controllers.EXPGainScreen:
                        break;
                    case Controllers.StartScreen:
                        ContextControllers.StartScreenCtrl.MouseMove();
                        break;
                    case Controllers.ScriptProcessor:
                        ScriptProcessorCtrl.HandleMouseMove();
                        break;
                    case Controllers.GameSettingsMenu:
                        GameSettingsMenuCtrl.HandleMouseMove();
                        break;
                    case Controllers.GameOptions:
                        GameOptionsMenuCtrl.HandleMouseMove();
                        break;
                    default:
                        break;
                }
            }
            else if (selectedSprite != null && !CombatProcessor.bIsRunning)
            {
                selectedSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
            }

            #endregion
            if (selectedSprite != null && (!CombatProcessor.bStartPhase1 && !CombatProcessor.bMainCombat && !CombatProcessor.bIsRunning) && !ScriptProcessor.bIsRunning)
            {
                selectedSprite.Update(gt);
            }

        stop: { }

        }

        private static void ReturnToEditorAction()
        {


            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Reload map to state before playtesting?", "Reload map?", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                MapBuilder.ReloadMap();
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
            }

            MapBuilder.bPlayTest = false;
            ResolutionUtility.AdjustResolution(Form1.editorRes.X, Form1.editorRes.Y, Game1.graphics);
            GameProcessor.bIsInGame = false;
        }

        internal static void InitializeStartScreenControls(StartScreen sc)
        {
            currentController = Controllers.StartScreen;
            ContextControllers.StartScreenCtrl.Start(sc);
        }

        internal static void Check()
        {

        }
    }
}
