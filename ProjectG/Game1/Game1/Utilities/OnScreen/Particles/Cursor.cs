using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.OnScreen.Particles
{
    class Cursor:Shape
    {
        public Cursor(Texture2D shapeTexture, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2), String shapeName = "", Rectangle shapeTextureBounds = default(Rectangle))
            :base( shapeTexture,  scale,  position,  bCollision,  center, shapeName, shapeTextureBounds)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            List<Rectangle> tempList = new List<Rectangle>();
            int tempX = 0;
            foreach (var hitbox in shapeHitBox)
            {
                tempList.Add(new Rectangle((int)((base.position.X + tempX) * 1), (int)((base.position.Y) * 1 + hitbox.Height), hitbox.Width, hitbox.Height));
                tempX++;
            }
            shapeHitBox.Clear();
            foreach (var hitbox in tempList)
            {
                shapeHitBox.Add(hitbox);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
