using TBAGW.Utilities.GamePlay.Characters.Friendly;
using TBAGW.Utilities.GamePlay.Spells;
using TBAGW.Utilities.GamePlay.Spells.ShotPattern;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Characters.Hostile
{
    class BasicEnemy : BaseCharacter
    {


        public BasicEnemy(Texture2D shapeTexture, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2), String shapeName = "", Rectangle shapeTextureBounds = default(Rectangle))
            : base(shapeTexture, scale, position, bCollision, center, shapeName, shapeTextureBounds)
        {


        }

        public override void Update(GameTime gameTime, List<BaseCharacter> activeObjects)
        {

                base.Update(gameTime,activeObjects);

            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

        }

    }
}
