using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.Particles;
using TBAGW.Utilities.ReadWrite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TBAGW.Scenes.Editor
{
    class SpriteCollisionSelect : Scene
    {
        Texture2D displaySpriteSheet;

        Matrix spritePickerMatrix;
        const int cameraSpeed = 5;
        int cameraPosX = 0;
        int cameraPosY = 0;
        List<ScreenButton> Grid = new List<ScreenButton>();

        public Rectangle selectedTextureBox;

        int widthPix = 32;
        int heightPix = 32;

        Texture2D DisplayTexture;
        Rectangle Step3Box;
        /*
        *String loc for location of collision spritesheet
        *Rectangle Step3Box is for Width and height purposes
        *DisplayTexture is texture chosen in step 2
        */

        public void Initialize(Game1 game, String loc, Rectangle Step3Box, Texture2D DisplayTexture)
        {
            Grid.Clear();

            widthPix = Step3Box.Width;
            heightPix = Step3Box.Height;
            this.DisplayTexture = DisplayTexture;
            this.Step3Box = Step3Box;

            displaySpriteSheet = game.Content.Load<Texture2D>(loc);
            cameraPosX = 0;
            cameraPosY = 0;
            int amountOfGridBlocksX = displaySpriteSheet.Width / 64;
            int amountOfGridBlocksY = displaySpriteSheet.Height / 64;
            for (int i = 0; i < amountOfGridBlocksX; i++)
            {
                for (int j = 0; j < amountOfGridBlocksY; j++)
                {
                    Grid.Add(new ScreenButton(null, Game1.defaultFont, "Button: (" + i + "," + j + ")", new Vector2(i * 64, 200 + j * 64)));
                    Grid[Grid.Count - 1].buttonBox = new Rectangle(i * 64, 200 + j * 64, 64, 64);
                }
            }


        }

        public void ResetCamera()
        {
            cameraPosX = 0;
            cameraPosY = 0;
        }

        public override void Update(GameTime gameTime, Game1 game)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                cameraPosY -= cameraSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                cameraPosY += cameraSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                cameraPosX -= cameraSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                cameraPosX += cameraSpeed;
            }

            spritePickerMatrix = Matrix.CreateTranslation(-cameraPosX, cameraPosY, 1);

            Vector2 EditorCursorPos = Mouse.GetState().Position.ToVector2() + new Vector2(cameraPosX, -cameraPosY);
            foreach (var item in Grid)
            {
                item.bButtonSelected = item.ButtonBox().Contains(EditorCursorPos);

                if (item.bButtonSelected && Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.bMousePressed)
                {
                    selectedTextureBox = new Rectangle(item.buttonBox.X, item.buttonBox.Y - 200, item.buttonBox.Width, item.buttonBox.Height);
                }
            }


        }

        public override void UnloadContent(Game1 game)
        {
        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spritePickerMatrix);

            spriteBatch.DrawString(Game1.defaultFont, "Step 4: Select static collision", new Vector2(100, 50), Color.Black);
            spriteBatch.DrawString(Game1.defaultFont, "GridSize "+widthPix+"x"+heightPix, new Vector2(100, 100), Color.Black);

            spriteBatch.Draw(displaySpriteSheet, new Vector2(0, 200), Color.Black);

            foreach (var item in Grid)
            {
                if (item.bButtonSelected)
                {
                    spriteBatch.Draw(Game1.hitboxHelp, item.buttonBox, Color.BlueViolet*.3f);
                    spriteBatch.Draw(DisplayTexture,item.buttonBox.Location.ToVector2(), Step3Box,Color.White*.3f);
                }
            }


            spriteBatch.End();
            spriteBatch.Begin();

            return spriteBatch;
        }

        public Rectangle EnableNextStep()
        {
            if (default(Rectangle) != selectedTextureBox)
            {
                return selectedTextureBox;
            }

            return default(Rectangle);
        }

        public Texture2D ProcessedTexture()
        {

            return displaySpriteSheet;
        }
    }
}
