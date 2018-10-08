using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Scenes.Editor.SpriteEditorSub
{
    class SimpleTypeSpriteEditor : Scene
    {
        public List<ScreenButton> spriteProperties = new List<ScreenButton>();

        Matrix spritePickerMatrix;
        const int cameraSpeed = 5;
        int cameraPosX = 0;
        int cameraPosY = 0;


        public void Initialize(Game1 game, Rectangle Step3Box, Texture2D DisplayTexture, Texture2D CollisionTexture = default(Texture2D), Rectangle Step5Box = default(Rectangle))
        {

        }

        public void Start()
        {
            if (spriteProperties.Count==0)
            {
                spriteProperties.Add(new ScreenButton(null, Game1.defaultFont, "Shapename: ", Vector2.Zero));
                spriteProperties.Add(new ScreenButton(null, Game1.defaultFont, "Collision on/off: ", Vector2.Zero));
                spriteProperties.Add(new ScreenButton(null, Game1.defaultFont, "Scale: ", Vector2.Zero));

                for (int i = 0; i < spriteProperties.Count; i++)
                {
                    spriteProperties[i].position = new Vector2(50, 150 + 50 * i);
                }
            }

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

        }

        public override void UnloadContent(Game1 game)
        {

        }

        public void ResetCamera()
        {
            cameraPosX = 0;
            cameraPosY = 0;
        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, spritePickerMatrix);


            spriteBatch.DrawString(Game1.defaultFont, "Step 6: Final step, adjust some last properties (Next up on the to do list)", new Vector2(100, 50), Color.Black);

            foreach (var item in spriteProperties)
            {
                spriteBatch.DrawString(Game1.defaultFont,item.buttonText,item.position,Color.Black);
            }

            spriteBatch.End();
            spriteBatch.Begin();


            return spriteBatch;
        }
    }
}
