using TBAGW;
using TBAGW.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using System;
using TBAGW.Scenes.Editor;

namespace TBAGW.Utilities
{
    //Handles resolution and mouse position
    static class ResolutionUtility
    {
        static internal Vector2 stdViewPort = new Vector2(1366, 768);
        static public Vector2 stdScale = new Vector2(1, 1);
        static public bool bIsFullScreen = false;
        static public bool bMouseIsVisible = false;

        static public Vector2 mousePos = Vector2.Zero;

        static int toggleResolution = 1;

        static bool bPressed = false;
        static InputControl buttonControl = new InputControl();
        static Vector2 tempScale = new Vector2();
        static public Vector2 WindowSizeBeforeFullScreen = new Vector2(1366, 768);
        static public Vector2 gameScale = new Vector2(1, 1);

        ///<summary>
        /// Adjust resolution
        ///</summary>    
        static public void AdjustResolution(float resX, float resY, GraphicsDeviceManager graphics)
        {

            if (resX < stdViewPort.X)
            {
                graphics.PreferredBackBufferWidth = (int)resX;
                stdScale.X = resX / stdViewPort.X;
            }
            else if (resX > stdViewPort.X)
            {

                graphics.PreferredBackBufferWidth = (int)resX;
                stdScale.X = resX / stdViewPort.X;
            }
            else if (resX == stdViewPort.X)
            {
                graphics.PreferredBackBufferWidth = (int)resX;
                stdScale.X = 1;
            }

            if (resY < stdViewPort.Y)
            {
                graphics.PreferredBackBufferHeight = (int)resY;
                stdScale.Y = resY / stdViewPort.Y;
            }
            else if (resY > stdViewPort.Y)
            {
                graphics.PreferredBackBufferHeight = (int)resY;
                stdScale.Y = resY / stdViewPort.Y;
            }
            else if (resY == stdViewPort.Y)
            {
                graphics.PreferredBackBufferHeight = (int)resY;
                stdScale.Y = 1;
            }

            if (stdScale.X > 1)
            {
                tempScale.X = 1 / stdScale.X;
            }
            else
            {
                tempScale.X = stdScale.X;

            }

            if (stdScale.Y > 1)
            {
                tempScale.Y = 1 / stdScale.Y;
            }
            else
            {
                tempScale.Y = stdScale.Y;

            }
            Console.WriteLine("Before: " + stdScale);
            if (tempScale.X < tempScale.Y)
            {
                if (stdScale.X > 1)
                {
                    stdScale.Y = 1 / tempScale.X;
                }
                else
                {
                    stdScale.Y = tempScale.X;
                }
            }

            if (tempScale.Y < tempScale.X)
            {
                if (stdScale.Y > 1)
                {
                    stdScale.X = 1 / tempScale.Y;

                }
                else
                {
                    stdScale.X = tempScale.Y;
                }
            }
            Console.WriteLine("After: " + stdScale);


            graphics.ApplyChanges();
            // Game1.gameRender = new RenderTarget2D(graphics.GraphicsDevice,(int) resX, (int)resY);
            Game1.gameRender = new RenderTarget2D(graphics.GraphicsDevice, (int)1366, (int)768);
            //if (graphics.IsFullScreen)
            //{
            //    Game1.graphics.PreferredBackBufferWidth = (int)Game1.monitorSize.X;
            //    Game1.graphics.PreferredBackBufferHeight = (int)Game1.monitorSize.Y;
            //    Game1.graphics.ApplyChanges();
            //}
            //else
            {
                Game1.graphics.PreferredBackBufferWidth = (int)resX;
                Game1.graphics.PreferredBackBufferHeight = (int)resY;
                Game1.graphics.IsFullScreen = bIsFullScreen;
                Game1.graphics.ApplyChanges();
            }
            WindowSizeBeforeFullScreen = new Vector2(resX, resY);
        }


        static public Vector2 testFS(GraphicsDeviceManager graphics, Vector2 converVector)
        {
            if (graphics.IsFullScreen)
            {
                Console.Out.WriteLine("You called?");

                if (graphics.PreferredBackBufferWidth > stdViewPort.X)
                {

                    converVector.X = converVector.X * stdScale.X;
                }


                if (graphics.PreferredBackBufferHeight > stdViewPort.Y)
                {
                    converVector.Y = converVector.Y * stdScale.Y;
                }
            }

            return converVector;
        }

        static public Vector2 AdjustMousePosition(GraphicsDeviceManager graphics, Vector2 converVector)
        {
            if (!graphics.IsFullScreen)
            {
                if (graphics.PreferredBackBufferWidth < stdViewPort.X)
                {

                    converVector.X = converVector.X / stdScale.X;
                }


                if (graphics.PreferredBackBufferHeight < stdViewPort.Y)
                {
                    converVector.Y = converVector.Y / stdScale.Y;
                }
            }


            /*
            if (graphics.IsFullScreen)
            {


                if (graphics.PreferredBackBufferWidth > stdViewPort.X)
                {

                    converVector.X = converVector.X * stdScale.X;
                }


                if (graphics.PreferredBackBufferHeight > stdViewPort.Y)
                {
                    converVector.Y = converVector.Y * stdScale.Y;
                }

                if (graphics.PreferredBackBufferWidth < stdViewPort.X)
                {

                    converVector.X = converVector.X / stdScale.X;
                }


                if (graphics.PreferredBackBufferHeight < stdViewPort.Y)
                {
                    converVector.Y = converVector.Y / stdScale.Y;
                }
            }*/

            return converVector;
        }

        ///<summary>
        /// Key Handler, doubletimes as Update() method
        ///</summary>    
        static public void KeyHandler(GraphicsDeviceManager graphics, GameTime gametime)
        {
            //    AdjustMousePosition(graphics);

            if (bPressed)
            {
                bPressed = !buttonControl.secondTimer(gametime, 1);
            }

            if ((int)Game1.Screens.Editor != SceneUtility.currentScene)
            {
                if (((Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && Keyboard.GetState().IsKeyDown(Keys.Enter))) && !bPressed)
                {
                    toggleFullscreen();
                    AdjustMatrix();
                    bIsFullScreen = graphics.IsFullScreen;
                    bPressed = true;
                }
            }
            else
            {
                graphics.IsFullScreen = false;
            }



        }

        private static void AdjustMatrix()
        {
        }

        static public void toggleFullscreen()
        {

            if (Game1.bIsActive)
            {
                if (bIsFullScreen)
                {
                    //WindowSizeBeforeFullScreen = new Vector2(Game1.graphics.PreferredBackBufferWidth, Game1.graphics.PreferredBackBufferHeight);
                    Console.WriteLine("Going fullscreen");
                    // Game1.graphics.ToggleFullScreen();
                    bIsFullScreen = false;
                    Game1.graphics.IsFullScreen = false;
                   // Game1.graphics.PreferredBackBufferWidth = (int)Game1.monitorSize.X;
                    //Game1.graphics.PreferredBackBufferHeight = (int)Game1.monitorSize.Y;
                    Game1.graphics.ApplyChanges();
                    //Mouse.SetPosition(100,100);
                    KeyboardMouseUtility.bMousePressed = true;
                }
                else
                {
                    Console.WriteLine("Going windowed");
                    // Game1.graphics.ToggleFullScreen();
                    bIsFullScreen = true;
                    Game1.graphics.IsFullScreen = true;
                    Game1.graphics.PreferredBackBufferWidth = (int)WindowSizeBeforeFullScreen.X;
                    Game1.graphics.PreferredBackBufferHeight = (int)WindowSizeBeforeFullScreen.Y;
                    Game1.graphics.ApplyChanges();
                    //Mouse.SetPosition(100,100);
                    KeyboardMouseUtility.bMousePressed = true;
                }

            }

        }

        static public void MouseMustStayWithinGame(Game1 game)
        {
            if (game.IsActive)
            {
                if (game.Window.ClientBounds.X + game.Window.ClientBounds.Width < ResolutionUtility.mousePos.X)
                {
                    Mouse.SetPosition(game.Window.ClientBounds.X + game.Window.ClientBounds.Width, Mouse.GetState().Y);
                }

                if (game.Window.ClientBounds.X > ResolutionUtility.mousePos.X)
                {
                    Mouse.SetPosition(game.Window.ClientBounds.X, Mouse.GetState().Y);
                }

                if (game.Window.ClientBounds.Y + game.Window.ClientBounds.Height < ResolutionUtility.mousePos.Y)
                {
                    Mouse.SetPosition(Mouse.GetState().X, game.Window.ClientBounds.Y + game.Window.ClientBounds.Height);
                }

                if (game.Window.ClientBounds.Y > ResolutionUtility.mousePos.Y)
                {
                    Mouse.SetPosition(Mouse.GetState().X, game.Window.ClientBounds.Y);
                }
            }
        }
    }
}
