using TBAGW.Utilities.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.OnScreen
{
    class WindowPopUp
    {
        Rectangle popUpBox;
        Texture2D popUpTexture;
        public ScreenButton[] popUpButtons;
        Vector2 popUpOrigin;
        Vector2 popUpPos;

        public WindowPopUp(Rectangle popUpBox, Texture2D popUpTexture, ScreenButton[] popUpButtons)
        {
            this.popUpBox = popUpBox;
            this.popUpTexture = popUpTexture;
            this.popUpButtons = popUpButtons;
            popUpOrigin = new Vector2(283,279);
        }

        public void Update(GameTime gameTime)
        {
            AdjustPositions(gameTime);
        }

        private void AdjustPositions(GameTime gameTime)
        {
            foreach (ScreenButton button in popUpButtons)
            {
                button.Update(gameTime);
            }

            popUpPos = popUpOrigin * 1;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.GUITransform);
            //Draws Darker background
            spriteBatch.Draw(Game1.hitboxHelp,new Rectangle(0,0,1366,768),Color.Black*.8f);

            //Draws pop up texture and buttons
            spriteBatch.Draw(popUpTexture,popUpPos,popUpBox,Color.White*1f,0,Vector2.Zero,1,SpriteEffects.None,0);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
 
            foreach (ScreenButton button in popUpButtons)
            {
                button.Draw(spriteBatch);
            }

       }
    }
}
