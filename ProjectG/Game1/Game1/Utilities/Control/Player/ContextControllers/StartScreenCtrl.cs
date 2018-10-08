using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Actions;

namespace TBAGW.Utilities.Control.Player.ContextControllers
{
    static public class StartScreenCtrl
    {
        static StartScreen sc;

        static internal void Start(StartScreen startScreen)
        {
            sc = startScreen;
        }

        static public void Update(List<Actions.ActionKey> keys)
        {
            if (Game1.bIsDebug)
            {
                if (!KeyboardMouseUtility.AnyButtonsPressed() && keys.Count != 0 && keys[0].actionIndentifierString.Equals(Game1.cameraMoveLeftString))
                {
                    SceneUtility.currentScene = (int)Game1.Screens.Editor;
                    StartScreen.bIsRunning = false;
                }
            }

            if (keys.Count != 0)
            {
                switch (sc.currentScreen)
                {
                    case StartScreen.Screens.start:
                        StandardControlsStart(keys[0]);
                        break;
                    case StartScreen.Screens.load:
                        StandardControlsLoad(keys[0]);
                        break;
                    default:
                        break;
                }

            }
        }

        private static bool StandardControlsLoad(ActionKey actionKey)
        {
            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.cancelString))
            {
                sc.CloseLoadScreen();
                KeyboardMouseUtility.bPressed = true;
                return false;
            }

            if (actionKey.actionIndentifierString.Equals(Game1.cameraMoveDownString))
            {
                LoadFileTab.AddScrollOffSet(4.2f);
            }

            if (KeyboardMouseUtility.ScrollingDown())
            {
                LoadFileTab.AddScrollOffSet(4.2f * 10);
            }

            if (actionKey.actionIndentifierString.Equals(Game1.cameraMoveUpString))
            {
                LoadFileTab.AddScrollOffSet(-4.2f);
            }

            if (KeyboardMouseUtility.ScrollingUp())
            {
                LoadFileTab.AddScrollOffSet(-4.2f * 10);
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && (actionKey.actionIndentifierString.Equals(Game1.moveDownString)))
            {
                sc.lgs.HandleDown();
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.moveUpString))
            {
                sc.lgs.HandleUp();
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed()&& !KeyboardMouseUtility.bMouseButtonPressed && actionKey.actionIndentifierString.Equals(Game1.confirmString))
            {
                sc.lgs.HandleConfirm();
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && KeyboardMouseUtility.bMouseButtonPressed && actionKey.actionIndentifierString.Equals(Game1.confirmString))
            {
                sc.lgs.HandleMouseClick();
            }
            return false;
        }

        private static void StandardControlsStart(ActionKey actionKey)
        {
            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.moveDownString))
            {
                if (sc.selectedButton == null)
                {
                    sc.selectedButton = sc.mButtons.Last();
                }
                int index = sc.mButtons.IndexOf(sc.selectedButton);
                if (++index >= sc.mButtons.Count)
                {
                    index = 0;
                }
                sc.selectedButton = sc.mButtons[index];
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.moveUpString))
            {
                if (sc.selectedButton == null)
                {
                    sc.selectedButton = sc.mButtons[0];
                }
                int index = sc.mButtons.IndexOf(sc.selectedButton);
                if (--index < 0)
                {
                    index = sc.mButtons.Count - 1;
                }
                sc.selectedButton = sc.mButtons[index];
            }

            if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.confirmString))
            {
                if (sc.selectedButton == sc.mButtons[0])
                {
                    sc.NewGameButton();
                }

                if (sc.selectedButton == sc.mButtons[1])
                {
                    sc.LoadGameButton();
                }

                if (sc.selectedButton == sc.mButtons[2])
                {
                    sc.OptionsButton();
                    //Game1.gameRef.Run();
                }

                if (sc.selectedButton == sc.mButtons[3])
                {
                    Game1.gameRef.Exit();
                    Game1.gameRef.Dispose();
                    //Game1.gameRef.Run();
                }
            }

        }

        internal static void MouseMove()
        {
            switch (sc.currentScreen)
            {
                case StartScreen.Screens.start:
                    if (KeyboardMouseUtility.bMouseMoved)
                    {
                        sc.selectedButton = sc.mButtons.Find(b => b.Contains(KeyboardMouseUtility.uiMousePos));
                    }
                    break;
                case StartScreen.Screens.load:
                    if (KeyboardMouseUtility.bMouseMoved && sc.lgs != null)
                    {
                        sc.lgs.HandleMouseMove();
                    }
                    break;
                default:
                    break;
            }

        }
    }
}
