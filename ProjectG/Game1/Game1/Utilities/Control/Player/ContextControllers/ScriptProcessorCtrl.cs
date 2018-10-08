using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Actions;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Utilities.Control.Player
{
    static public class ScriptProcessorCtrl
    {
        static public void Update(List<ActionKey> keys)
        {
            if (!ScriptProcessor.bIsRunning)
            {
                Stop();
            }

            if (keys.Count != 0)
            {
                ActionKey actionKey = keys[0];

                switch (ScriptProcessor.currentDisplayMode)
                {
                    case (int)ScriptProcessor.ActiveScriptDisplayMode.Text:
                        if (!KeyboardMouseUtility.AnyButtonsPressed() && (actionKey.actionIndentifierString.Equals(Game1.confirmString) || actionKey.actionIndentifierString.Equals(Game1.openMenuString)))
                        {
                            ScriptProcessor.ConversationTextConfirmHandle();
                        }
                        break;
                    case (int)ScriptProcessor.ActiveScriptDisplayMode.Choice:
                        if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.confirmString) && KeyboardMouseUtility.bMouseButtonPressed)
                        {
                            ScriptProcessor.ChoiceHandleMouseClick();
                        }

                        if (!KeyboardMouseUtility.AnyButtonsPressed() && (actionKey.actionIndentifierString.Equals(Game1.confirmString) || actionKey.actionIndentifierString.Equals(Game1.openMenuString)))
                        {
                            ScriptProcessor.HandleChoiceConfirmButton();
                        }

                        if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.moveUpString))
                        {
                            ScriptProcessor.HandleChoiceMoveUp();
                        }

                        if (!KeyboardMouseUtility.AnyButtonsPressed() && actionKey.actionIndentifierString.Equals(Game1.moveDownString))
                        {
                            ScriptProcessor.HandleChoiceMoveDown();
                        }
                        break;
                    case (int)ScriptProcessor.ActiveScriptDisplayMode.Conversation:
                        if (!KeyboardMouseUtility.AnyButtonsPressed() && (actionKey.actionIndentifierString.Equals(Game1.confirmString) || actionKey.actionIndentifierString.Equals(Game1.openMenuString)))
                        {
                            ScriptProcessor.ConversationTextConfirmHandle();
                        }
                        break;
                    case (int)ScriptProcessor.ActiveScriptDisplayMode.None:
                        break;
                    default:
                        break;
                }
            }
        }

        static public void Start()
        {
            if (PlayerController.currentController!=PlayerController.Controllers.ScriptProcessor)
            {
                PlayerController.previousMainController = PlayerController.currentController;
            }
           
            PlayerController.currentController = PlayerController.Controllers.ScriptProcessor;
        }

        static public void Stop()
        {
            PlayerController.currentController = PlayerController.previousMainController;
        }

        internal static void HandleMouseMove()
        {
            switch (ScriptProcessor.currentDisplayMode)
            {
                case (int)ScriptProcessor.ActiveScriptDisplayMode.Text:

                    break;
                case (int)ScriptProcessor.ActiveScriptDisplayMode.Choice:
                    ScriptProcessor.HandleChoiceMouseMove(KeyboardMouseUtility.uiMousePos);
                    break;
                case (int)ScriptProcessor.ActiveScriptDisplayMode.Conversation:
                    break;
                case (int)ScriptProcessor.ActiveScriptDisplayMode.None:
                    break;
                default:
                    break;
            }
        }
    }
}
