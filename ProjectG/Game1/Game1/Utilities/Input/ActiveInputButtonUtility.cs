using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.Input
{
    static public class ActiveInputButtonUtility
    {
        static public Texture2D AIButtonTexture;

        //Length of image border before stretching middle part, first of three parts
        const int BGborderLength = 8;

        static public Rectangle ButtonBGBox1 = new Rectangle(0,0, BGborderLength, 64);
        static public Rectangle ButtonBGBox2 = new Rectangle(BGborderLength, 0, 48, 64);
        static public Rectangle ButtonBGBox3 = new Rectangle(64- BGborderLength, 0, BGborderLength, 64);
        static public Rectangle ButtonBGContainer = new Rectangle(0,0,64,64);
        static public Rectangle ButtonCursor = new Rectangle(64,0,64,64);


        static public void Activate(Game1 game)
        {
            if (AIButtonTexture != default(Texture2D))
            {
                AIButtonTexture = game.Content.Load<Texture2D>("ActiveTextInput");
            }
        }
    }
}
