using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Actions;

namespace TBAGW.Utilities.Control.Player
{
    static public class GameOptionsMenuCtrl
    {
        static public void Update(List<ActionKey> keys)
        {
            ActionKey key = keys.Last();

            if ((!KeyboardMouseUtility.AnyButtonsPressed() ) && (key.actionIndentifierString.Equals(Game1.confirmString) || key.actionIndentifierString.Equals(Game1.openMenuString)))
            {
                OptionsMenu.HandleConfirmOrClick();
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed()) && (key.actionIndentifierString.Equals(Game1.moveDownString) || key.actionIndentifierString.Equals(Game1.cameraMoveDownString)))
            {
                OptionsMenu.HandleUpDown(true);
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed()) && (key.actionIndentifierString.Equals(Game1.moveUpString) || key.actionIndentifierString.Equals(Game1.cameraMoveUpString)))
            {
                OptionsMenu.HandleUpDown(false);
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed()) && (key.actionIndentifierString.Equals(Game1.moveLeftString) || key.actionIndentifierString.Equals(Game1.cameraMoveLeftString)))
            {
                OptionsMenu.HandleLeftRight(false);
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed()) && (key.actionIndentifierString.Equals(Game1.moveRightString) || key.actionIndentifierString.Equals(Game1.cameraMoveRightString)))
            {
                OptionsMenu.HandleLeftRight(true);
                KeyboardMouseUtility.bPressed = true;
            }

            if ((!KeyboardMouseUtility.AnyButtonsPressed() ) && (key.actionIndentifierString.Equals(Game1.cancelString)||key.actionIndentifierString.Equals(Game1.SettingsMenu)))
            {
                OptionsMenu.HandleCancel();
                KeyboardMouseUtility.bPressed = true;
            }

        }

        internal static void HandleMouseMove()
        {
            OptionsMenu.HandleMouseMove();
        }
    }
}
