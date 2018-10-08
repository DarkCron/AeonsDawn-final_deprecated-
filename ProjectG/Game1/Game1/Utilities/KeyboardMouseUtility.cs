using TBAGW;
using TBAGW.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;

namespace TBAGW.Utilities
{
    static class KeyboardMouseUtility
    {
        static InputControl timedControl = new InputControl();
        static public bool bPressed = false;
        static public bool bMousePressed = false;
        static public bool bMouseDrag = false;
        static public bool bMouseButtonPressed = false;
        static public List<Keys> prevPressedKeys = new List<Keys>();
        static public List<Keys> nowPressedKeys = new List<Keys>();

        /// <summary>
        /// 0 no scrolling, 1 scrolling up, -1 scrolling down
        /// </summary>
        static public int mouseScrollValue = 0;
        static int previousScrollCumulative = 0;
        static public bool bMouseMoved = false;
        static public Point uiMousePos = new Point();
        static public Vector2 gameMousePos = new Vector2();

        public static bool ScrollingUp()
        {
            return mouseScrollValue == 1;
        }

        public static bool ScrollingDown()
        {
            return mouseScrollValue == -1;
        }

        public static bool TimedButtonPress(GameTime gameTime, double millisecondTimer, int times)
        {
            if (timedControl.millisecondTimer(gameTime, millisecondTimer))
            {
                return true;

            }
            return false;
        }

        internal static bool HoldingLeftClick()
        {
            if (Mouse.GetState().LeftButton != default(ButtonState))
            {
                return true;
            }
            return false;
        }

        public static void Update(Game1 game)
        {

            nowPressedKeys.Clear();
            nowPressedKeys.AddRange(Keyboard.GetState().GetPressedKeys());

            if ((Keyboard.GetState().GetPressedKeys().Length != 0 || KeyboardMouseUtility.mouseScrollValue != 0))
            {
                bPressed = true;
            }
            else
            {
                bPressed = false;
            }

            //if (((Keyboard.GetState().IsKeyDown(Keys.LeftShift)|| Keyboard.GetState().IsKeyDown(Keys.LeftAlt) )&& Keyboard.GetState().GetPressedKeys().Length == 2))
            //{
            //    bPressed = true;
            //}
            ////else
            if (((Keyboard.GetState().IsKeyDown(Keys.LeftShift)) || Keyboard.GetState().IsKeyDown(Keys.LeftAlt)) && Keyboard.GetState().GetPressedKeys().Length == 1)
            {
                bPressed = false;
            }

            if ((Mouse.GetState().LeftButton != default(ButtonState)) || (Mouse.GetState().RightButton != default(ButtonState)))
            {
                bMousePressed = true;
                bPressed = true;
            }
            else
            {
                bMousePressed = false;

            }



            HandleMouseScroll();

            if (prevPressedKeys.Count != 0 && nowPressedKeys.Count != 0 && nowPressedKeys.Count != prevPressedKeys.Count)
            {
                //if (nowPressedKeys[0] == Keys.LeftShift)
                //{
                //    bPressed = false;
                //}
                //else if (nowPressedKeys.Count > 1 && nowPressedKeys[0] == Keys.LeftShift)
                //{
                //    bPressed = false;
                //}

                //if (nowPressedKeys.Count > 1 && nowPressedKeys[1] == Keys.LeftShift && nowPressedKeys[0] != Keys.LeftShift)
                //{
                //    bPressed = true;
                //}

                //if (nowPressedKeys.Count > 1 && nowPressedKeys[0] == Keys.LeftShift && nowPressedKeys[1] != Keys.LeftShift)
                //{
                //    bPressed = true;
                //}

                //if (nowPressedKeys.Count < prevPressedKeys.Count)
                //{
                //    bPressed = false;
                //}


            }
            else if (nowPressedKeys.Count == prevPressedKeys.Count && prevPressedKeys.Count != 0 && nowPressedKeys.Count != 0)
            {
                if (nowPressedKeys[nowPressedKeys.Count - 1] != prevPressedKeys[prevPressedKeys.Count - 1] && (!nowPressedKeys.Contains(Keys.LeftAlt) && !nowPressedKeys.Contains(Keys.LeftShift)))
                {
                    bPressed = false;
                }


            }

            prevPressedKeys.Clear();
            prevPressedKeys.AddRange(Keyboard.GetState().GetPressedKeys());
        }

        private static void HandleMouseScroll()
        {
            if (Mouse.GetState().ScrollWheelValue != 0)
            {
                if (Mouse.GetState().ScrollWheelValue > previousScrollCumulative)
                {
                    mouseScrollValue = 1;

                }
                else if (Mouse.GetState().ScrollWheelValue < previousScrollCumulative)
                {
                    mouseScrollValue = -1;
                }
                else
                {
                    mouseScrollValue = 0;
                }

                previousScrollCumulative = Mouse.GetState().ScrollWheelValue;



            }
        }

        /// <summary>
        /// An extra utility used along with buttonPressUtility functions to check whether buttons were pressed/being pressed
        /// while changing between scenes or gamescreens ect.
        /// use like this:
        /// !KeyboardMouseUtility.AnyButtonsPressed()
        /// </summary>
        public static bool AnyButtonsPressed()
        {
            if (Game1.bIsActive)
            {
                return bPressed;
            }
            else
            {
                return true;
            }

        }

        public static void AssignGameMousePos(Vector2 v)
        {
            gameMousePos = v;
        }



    }
}
