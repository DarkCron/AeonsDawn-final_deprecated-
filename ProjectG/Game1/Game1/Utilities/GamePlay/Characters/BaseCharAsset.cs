using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Characters
{
    class BaseCharAsset
    {
        public Texture2D bigGUIPic;
        public Texture2D smallGUIPic;
        public Texture2D shapeTexture;
        public Rectangle bigGUIPicBounds;
        public Rectangle smallGUIPicBounds;
        public Rectangle shapeTextureBounds;
        public String heroName = "";

        public BaseCharAsset(Texture2D bigGUIPic, Texture2D smallGUIPic, Texture2D shapeTexture, Rectangle bigGUIPicBounds, Rectangle smallGUIPicBounds, Rectangle shapeTextureBounds)
        {
            this.bigGUIPic = bigGUIPic;
            this.smallGUIPic = smallGUIPic;
            this.shapeTexture = shapeTexture;
            this.bigGUIPicBounds = bigGUIPicBounds;
            this.smallGUIPicBounds = smallGUIPicBounds;
            this.shapeTextureBounds = shapeTextureBounds;
        }

        public BaseCharAsset(Texture2D bigGUIPic, Rectangle bigGUIPicBounds, Rectangle smallGUIPicBounds, Rectangle shapeTextureBounds)
        {
            this.bigGUIPic = bigGUIPic;
            this.smallGUIPic = bigGUIPic;
            this.shapeTexture = bigGUIPic;
            this.bigGUIPicBounds = bigGUIPicBounds;
            this.smallGUIPicBounds = smallGUIPicBounds;
            this.shapeTextureBounds = shapeTextureBounds;
        }
    }
}
