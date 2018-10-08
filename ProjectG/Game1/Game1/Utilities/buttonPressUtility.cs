using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TBAGW.Utilities;
using System.Collections.Generic;
using TBAGW.Utilities.Actions;
using TBAGW.Utilities.Input;


namespace TBAGW.Utilities
{
    static class buttonPressUtility
    {
        static public bool bPressed = false;
        static InputControl buttonControl = new InputControl();//for keyboard keys

        static public void Update(GameTime gameTime)
        {
            if (bPressed && (Keyboard.GetState().GetPressedKeys().Length != 0||GamePad.GetState(PlayerIndex.One).IsConnected) )
            {
                bPressed = !buttonControl.millisecondTimer(gameTime, 150);
            }

            if (Keyboard.GetState().GetPressedKeys().Length == 0&&!GamePad.GetState(PlayerIndex.One).IsConnected && KeyboardMouseUtility.mouseScrollValue == 0)
            {
                bPressed = false;
            }
            

        }

        static public bool isPressed(String keyString)
        {
            foreach (Actions.Actions action in Game1.actionList)
            {

                if (action.actionIndentifierString.Equals(keyString))
                {
                    foreach (ActionKey key in action.whatKeysIsActionAssignedTo)
                    {
                        if ((Keyboard.GetState().IsKeyDown(key.identifyKey(keyString))) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            bPressed = true;
                            buttonControl.elapsedMilliseconds = 0;
                            return true;
                        }

                        if (key.bKeyIsGamePadKey && GamePad.GetState(PlayerIndex.One).IsButtonDown(key.identifyButton(keyString)) && !KeyboardMouseUtility.AnyButtonsPressed() && !GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))
                        {
                            bPressed = true;
                            buttonControl.elapsedMilliseconds = 0;
                            return true;
                        }
                        else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            if(keyString.Equals(Game1.moveDownString)&&GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y<0){
                                bPressed = true;
                                buttonControl.elapsedMilliseconds = 0;
                                return true;
                            }
                            else if (keyString.Equals(Game1.moveUpString) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                            {
                                bPressed = true;
                                buttonControl.elapsedMilliseconds = 0;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        //ignores bPressed
        static public bool isPressedSub(String keyString)
        {
            foreach (Actions.Actions action in Game1.actionList)
            {

                if (action.actionIndentifierString.Equals(keyString))
                {
                    foreach (ActionKey key in action.whatKeysIsActionAssignedTo)
                    {
                        if ((Keyboard.GetState().IsKeyDown(key.identifyKey(keyString))))
                        {
                           
                            buttonControl.elapsedMilliseconds = 0;
                            return true;
                        }
                        
                        if (key.bKeyIsGamePadKey && GamePad.GetState(PlayerIndex.One).IsButtonDown(key.identifyButton(keyString))&&!GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))
                        {
                            
                            buttonControl.elapsedMilliseconds = 0;
                            return true;
                        }
                        else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftThumbstickDown))
                        {
                            if (keyString.Equals(Game1.moveDownString) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                            {
                                
                                buttonControl.elapsedMilliseconds = 0;
                                return true;
                            }
                            else if (keyString.Equals(Game1.moveUpString) && GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                            {
                               
                                buttonControl.elapsedMilliseconds = 0;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Single presses only, no drag and hold.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        static public bool isMousePressed(ButtonState state)
        {
            if (!KeyboardMouseUtility.AnyButtonsPressed()&&Game1.bIsActive && state == ButtonState.Pressed)
            {
                return true;
            }

            return false;
        }

        
    }
}
