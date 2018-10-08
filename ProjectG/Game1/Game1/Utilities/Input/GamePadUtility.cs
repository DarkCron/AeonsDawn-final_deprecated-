using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Actions;

namespace Game1.Utilities.Input
{
    static public class GamePadUtility
    {
        static int playerIndex = 0;

        static public bool IsButtonPressed()
        {
            if (GamePad.GetState(playerIndex).IsConnected)
            {
                if (GamePad.GetState(playerIndex).Buttons.A == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.B == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.X == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.Y == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.Start == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.Back == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.BigButton == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.LeftShoulder == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.RightShoulder == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.LeftStick == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).Buttons.RightStick == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).DPad.Up == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).DPad.Down == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).DPad.Left == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).DPad.Right == ButtonState.Pressed)
                {
                    return true;
                }

                if (GamePad.GetState(playerIndex).ThumbSticks.Left.ToPoint() != Microsoft.Xna.Framework.Point.Zero)
                {
                    Console.WriteLine("Left ThumbStick Input");
                    return true;
                }

                if (GamePad.GetState(playerIndex).ThumbSticks.Right.ToPoint() != Microsoft.Xna.Framework.Point.Zero)
                {
                    Console.WriteLine("Right ThumbStick Input");
                    return true;
                }
            }

            return false;
        }

    }
}
