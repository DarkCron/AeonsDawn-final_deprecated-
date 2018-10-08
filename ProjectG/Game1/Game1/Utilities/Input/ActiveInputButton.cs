using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.Input
{
    class ActiveInputButton
    {
        int x;
        int y;

        String text;

        bool bIsActiveSelected = false;

        public ActiveInputButton(int x, int y, String text)
        {
            this.x = x;
            this.y = y;
            this.text = text;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(ActiveInputButtonUtility.AIButtonTexture,new Vector2(x,y),ActiveInputButtonUtility.ButtonBGBox1,Color.White);

            spritebatch.Draw(ActiveInputButtonUtility.AIButtonTexture, new Vector2(x, y), ActiveInputButtonUtility.ButtonBGBox3, Color.White);

            if (bIsActiveSelected)
            {

            }
        }

    }
}
