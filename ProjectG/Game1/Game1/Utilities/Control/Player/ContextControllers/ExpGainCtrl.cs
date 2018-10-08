using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Actions;

namespace TBAGW.Utilities
{
    static public class ExpGainCtrl
    {
        static public void Update(List<Actions.ActionKey> keys)
        {
            if (keys.Count != 0)
            {
                GlobalControls(keys[keys.Count - 1]);
                KeyboardMouseUtility.bPressed = true;
            }
        }

        private static void GlobalControls(ActionKey key)
        {
            if (ExpGainScreen.bLevelledUp)
            {
                if (key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    ExpGainScreen.ConfirmPress();
                }
            }else if(ExpGainScreen.bDoneGivingExp)
            {
                if (key.actionIndentifierString.Equals(Game1.confirmString) && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    ExpGainScreen.ConfirmPress();
                }
            }
        }
    }
}
