using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Scenes.MainGame
{
    class OWGame:Scene
    {
        ScreenButton goToBattleButton;
        bool buttonPressed = false;
        bool buttonSelected = false;

        public override void Initialize(Game1 game)
        {
            base.Initialize(game);
            String buttonString = "Go to battle mode";
            Vector2 buttonPos = new Vector2(1366 / 2, 768 / 2) - Game1.defaultFont.MeasureString(buttonString)/2;
            goToBattleButton = new ScreenButton(default(Texture2D),Game1.defaultFont,buttonString,buttonPos);
        }

        public override void Update(GameTime gameTime, Game1 game)
        {
            base.Update(gameTime, game);
            goToBattleButton.Update(gameTime);

            if(goToBattleButton.ContainsMouse()){
                buttonSelected = true;

                if (Game1.bIsActive && Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    buttonPressed = true;
                }
                else
                {
                    buttonPressed = false;
                }
                
            }
            else
            {
                buttonSelected = false;
            }

            if(buttonPressed){
                HandleSelection();
            }
        }

        private void HandleSelection()
        {
            SceneUtility.ChangeScene((int)(Game1.Screens.BGame));
        }

        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            Game1.graphics.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 1.0f, 0);
            goToBattleButton.Draw(spriteBatch);

            if(buttonSelected){
                spriteBatch.Draw(Game1.selectionTexture,goToBattleButton.ButtonBox(),Color.White);
            }

            return base.Draw(gametime, spriteBatch);
            
        }

        public override void UnloadContent(Game1 game)
        {
            base.UnloadContent(game);
        }


    }
}
