using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Actions;

namespace TBAGW.Utilities.Control.Player
{
    static public class GameSettingsMenuCtrl
    {
        static public void Update(List<ActionKey> keys)
        {
            ActionKey key = keys.Last();

            if ((!KeyboardMouseUtility.AnyButtonsPressed() ) && (key.actionIndentifierString.Equals(Game1.confirmString) || key.actionIndentifierString.Equals(Game1.openMenuString)))
            {
                SettingsMenu.HandleConfirmOrClick();
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed()) && (key.actionIndentifierString.Equals(Game1.moveDownString) || key.actionIndentifierString.Equals(Game1.cameraMoveDownString)))
            {
                SettingsMenu.HandleUpDown(true);
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed()) && (key.actionIndentifierString.Equals(Game1.moveUpString) || key.actionIndentifierString.Equals(Game1.cameraMoveUpString)))
            {
                SettingsMenu.HandleUpDown(false);
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed()) && (key.actionIndentifierString.Equals(Game1.moveLeftString) || key.actionIndentifierString.Equals(Game1.cameraMoveLeftString)))
            {
                SettingsMenu.HandleLeftRight(false);
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed()) && (key.actionIndentifierString.Equals(Game1.moveRightString) || key.actionIndentifierString.Equals(Game1.cameraMoveRightString)))
            {
                SettingsMenu.HandleLeftRight(true);
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed() ) && (key.actionIndentifierString.Equals(Game1.cancelString)||key.actionIndentifierString.Equals(Game1.SettingsMenu)))
            {
                SettingsMenu.HandleCancel();
                KeyboardMouseUtility.bPressed = true;
            }

            if (KeyboardMouseUtility.ScrollingDown())
            {
                LoadFileTab.AddScrollOffSet(4.2f * 10);
            }

            if (KeyboardMouseUtility.ScrollingUp())
            {
                LoadFileTab.AddScrollOffSet(-4.2f * 10);
            }
        }

        internal static void HandleMouseMove()
        {
            SettingsMenu.HandleMouseMove();
        }
    }
}
