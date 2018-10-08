using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Characters.Friendly
{
    class HeroAsset:BaseCharAsset
    {


        public HeroAsset(Texture2D bigGUIPic, Texture2D smallGUIPic, Texture2D shapeTexture, Rectangle bigGUIPicBounds, Rectangle smallGUIPicBounds, Rectangle shapeTextureBounds)
            :base(bigGUIPic,smallGUIPic,shapeTexture,bigGUIPicBounds,smallGUIPicBounds,shapeTextureBounds)
        {

        }

        public HeroAsset(Texture2D bigGUIPic, Rectangle bigGUIPicBounds, Rectangle smallGUIPicBounds, Rectangle shapeTextureBounds)
            : base(bigGUIPic, bigGUIPicBounds, smallGUIPicBounds, shapeTextureBounds)
        {

        }
    }
}
