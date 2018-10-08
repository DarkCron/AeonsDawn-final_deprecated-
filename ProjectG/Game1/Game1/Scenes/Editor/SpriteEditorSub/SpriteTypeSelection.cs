using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Scenes.Editor.SpriteEditorSub
{
    class SpriteTypeSelection : Scene
    {
        public List<ScreenButton> spriteTypes = new List<ScreenButton>();
        Rectangle Step3Box; //Texture box
        Rectangle Step5Box; //Collision box
        Texture2D DisplayTexture;
        Texture2D CollisionTexture;

        Matrix spritePickerMatrix;
        const int cameraSpeed = 5;
        int cameraPosX = 0;
        int cameraPosY = 0;

        public enum SpriteTypes { SimpleType=0, MissileType, EnemyType, HeroType }

        public int selectedType =-1;

        public void Initialize(Game1 game, Rectangle Step3Box, Texture2D DisplayTexture, Texture2D CollisionTexture = default(Texture2D), Rectangle Step5Box = default(Rectangle))
        {
            this.Step3Box = Step3Box;
            this.DisplayTexture = DisplayTexture;
            this.Step5Box = Step5Box;
            this.CollisionTexture = CollisionTexture;

            spriteTypes.Clear();
            spriteTypes.Add(new ScreenButton(null, Game1.defaultFont, SpriteTypes.SimpleType.ToString(), Vector2.Zero));
            spriteTypes.Add(new ScreenButton(null,Game1.defaultFont, SpriteTypes.MissileType.ToString(), Vector2.Zero));
            spriteTypes.Add(new ScreenButton(null, Game1.defaultFont, SpriteTypes.HeroType.ToString(), Vector2.Zero));
            spriteTypes.Add(new ScreenButton(null, Game1.defaultFont, SpriteTypes.EnemyType.ToString(), Vector2.Zero));

            for (int i = 0; i < spriteTypes.Count; i++)
            {
                spriteTypes[i].position = new Vector2(0,150+ (Step3Box.Height + 50*i));
            }

            BasicSpriteFinalize.shapeTexture=DisplayTexture;
            BasicSpriteFinalize.hitboxTexture=CollisionTexture;
            BasicSpriteFinalize.rectangleToDraw=Step3Box;
            BasicSpriteFinalize.hitBoxTexBox = Step5Box;
            BasicSpriteFinalize.Start();
        }

        public void ResetCamera()
        {
            cameraPosX = 0;
            cameraPosY = 0;
        }

        public override void Reload()
        {

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

            foreach (var item in spriteTypes)
            {
                item.Update(gameTime);
                item.bButtonSelected = item.ButtonBox().Contains(EditorCursorPos);

                if (item.bButtonSelected && Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.bMousePressed)
                {
                    selectedType = (int)(SpriteTypes)Enum.Parse(typeof(SpriteTypes), item.buttonText); 
                }
            }
        }

        public int EnableNextStep()
        {
            if (selectedType!=-1)
            {
                return selectedType;
            }

            return -1;
        }

        public override void UnloadContent(Game1 game)
        {

        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spritePickerMatrix);


            spriteBatch.DrawString(Game1.defaultFont, "Step 5: Select type of Sprite you'd want to create\nSelected texture:                     Selected static collision: (OPTIONAL)", new Vector2(100, 50), Color.Black);
            spriteBatch.Draw(Game1.hitboxHelp, new Vector2(125, 120), Step3Box, Color.Blue * .2f);
            spriteBatch.Draw(DisplayTexture, new Vector2(125, 120), Step3Box, Color.White);

            if (Step5Box!=default(Rectangle))
            {
                spriteBatch.Draw(Game1.hitboxHelp, new Vector2(400, 120), Step3Box, Color.Blue * .2f);
                spriteBatch.Draw(DisplayTexture, new Vector2(400, 120), Step3Box, Color.White);
                spriteBatch.Draw(CollisionTexture, new Vector2(400, 120), Step5Box, Color.Black);

                spriteBatch.Draw(Game1.hitboxHelp, new Vector2(730, 120), Step5Box, Color.Blue * .2f);
                spriteBatch.Draw(CollisionTexture, new Vector2(730, 120), Step5Box, Color.Black);
            }


            int i = 0;
            foreach (var item in spriteTypes)
            {
                if (!item.bButtonSelected)
                {
                    spriteBatch.DrawString(Game1.defaultFont, item.buttonText, item.position, Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(Game1.defaultFont, item.buttonText, item.position, Color.BlueViolet);
                }
                i++;
            }

            spriteBatch.End();
            spriteBatch.Begin();


            return spriteBatch;
        }
    }
}
