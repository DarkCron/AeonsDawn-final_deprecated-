using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;
using TBAGW.Utilities.Actions;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW
{
    class SoloCombatCtrl
    {
        static public BaseCharacter selectedChar = new BaseCharacter();


        static public void Update(List<ActionKey> keys)
        {
            if (BattleGUI.bSoloCompleted)
            {
                if (keys.Count != 0)
                {
                    ProcessEnd(keys[keys.Count - 1]);
                    KeyboardMouseUtility.bPressed = true;
                }
            }else
            {
                if (keys.Count != 0)
                {
                    Process(keys[keys.Count - 1]);
                    KeyboardMouseUtility.bPressed = true;
                }
            }
        }

        private static void Process(ActionKey actionKey)
        {
            if (actionKey.actionIndentifierString.Equals(Game1.confirmString, StringComparison.OrdinalIgnoreCase) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                BattleGUI.bSoloSkipTimer = true;
            }

            if (actionKey.actionIndentifierString.Equals(Game1.cancelString, StringComparison.OrdinalIgnoreCase) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                BattleGUI.CharacterSoloStop();
            }
        }

        private static void ProcessEnd(ActionKey actionKey)
        {
            if (actionKey.actionIndentifierString.Equals(Game1.confirmString, StringComparison.OrdinalIgnoreCase) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                BattleGUI.CharacterSoloStop();
            }
        }
    }
}
