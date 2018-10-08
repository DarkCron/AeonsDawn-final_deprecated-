using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Spells
{
    public class SpellMissile : Shape
    {
        public Spell baseSpell;

        public SpellMissile(Spell baseSpell, Texture2D shapeTexture, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2), String shapeName = "", Rectangle shapeTextureBounds = default(Rectangle))
            : base(shapeTexture, scale, position, bCollision, center, shapeName, shapeTextureBounds)
        {
            this.baseSpell = baseSpell;
            base.rectangleToDraw = baseSpell.spellTextureBounds;
        }

        public SpellMissile(Spell baseSpell, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2))
    : base(baseSpell.spellTextureSpriteSheet, scale, position, bCollision, center, baseSpell.spellName, baseSpell.spellTextureBounds)
        {
            this.baseSpell = baseSpell;
            base.rectangleToDraw = baseSpell.spellTextureBounds;
            /*   var pp = Game1.GraphicsDevice.PresentationParameters;
               lightsTarget = new RenderTarget2D(
                   GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
               mainTarget = new RenderTarget2D(
                   GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);*/

            var pp = Game1.graphics.GraphicsDevice.PresentationParameters;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (targetPos != Vector2.Zero && bStopAtTarget)
            {
                // centerPoint = originalCenterPoint;


                if (centerPoint.X != targetPos.X)
                {
                    if (Math.Abs((centerPoint.X) - targetPos.X) > speed)
                    {
                        tempVolX += velocity.X;


                        if (centerPoint.X < targetPos.X && tempVolX > 1)
                        {

                            position.X += (int)(tempVolX);
                            GenerateHitBoxes((int)(tempVolX), 0);
                            //      originalCenterPoint.X += (int)(tempVolX);
                            tempVolX -= (int)tempVolX;

                        }
                        else if (centerPoint.X > targetPos.X && tempVolX > 1)
                        {
                            GenerateHitBoxes(-(int)(tempVolX), 0);
                            position.X -= (int)(tempVolX);
                            //     originalCenterPoint.X -= (int)(tempVolX);
                            tempVolX -= (int)tempVolX;
                        }
                    }

                }
                if (centerPoint.Y != targetPos.Y)
                {
                    if (Math.Abs((centerPoint.Y) - targetPos.Y) > 1)
                    {
                        tempVolY += velocity.Y;

                        if (centerPoint.Y < targetPos.Y && tempVolY > 1)
                        {
                            GenerateHitBoxes(0, (int)(tempVolY));
                            position.Y += (int)(tempVolY);
                            //    originalCenterPoint.Y += (int)(tempVolY);
                            tempVolY -= (int)tempVolY;
                        }
                        else if (centerPoint.Y > targetPos.Y && tempVolY > 1)
                        {
                            GenerateHitBoxes(0, -(int)(tempVolY));
                            position.Y -= (int)(tempVolY);
                            //  originalCenterPoint.Y -= (int)(tempVolY);
                            tempVolY -= (int)tempVolY;
                        }
                    }
                }

            }
            else if (!bStopAtTarget || velocity != Vector2.Zero)
            {
                tempVolX += velocity.X;
                tempVolY += velocity.Y;

                if (tempVolX > 1 && bGoLeft)
                {
                    GenerateHitBoxes(-(int)(tempVolX), 0);
                    position.X -= (int)(tempVolX);
                    tempVolX -= (int)tempVolX;
                }
                else if (tempVolX > 1 && !bGoLeft)
                {
                    GenerateHitBoxes(+(int)(tempVolX), 0);
                    position.X += (int)(tempVolX);
                    tempVolX -= (int)tempVolX;
                }

                if (tempVolY > 1 && !bGoUp)
                {
                    GenerateHitBoxes(0, -(int)(tempVolY));
                    position.Y -= (int)(tempVolY);
                    tempVolY -= (int)tempVolY;
                }
                else if (tempVolY > 1 && bGoUp)
                {
                    GenerateHitBoxes(0, +(int)(tempVolY));
                    position.Y += (int)(tempVolY);
                    tempVolY -= (int)tempVolY;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            /*
            if (baseSpell.bHasOverlay)
            {
                //  spriteBatch.Draw(baseSpell.overlayTexture, (position) * 1, baseSpell.overlayBounds, baseSpell.overlayColor, , Vector2.Zero, 1 * (scale), SpriteEffects.None, 0);
                if ((int)currentAngle == (int)-(Math.PI / 2))
                {
                    spriteBatch.Draw(baseSpell.overlayTexture, (position) * 1, baseSpell.overlayBounds, baseSpell.overlayColor, currentAngle, new Vector2(baseSpell.overlayBounds.Width, 0), new Vector2(1 * scale, 1 * scale), SpriteEffects.None, 0);
                }
                else if ((int)currentAngle == (int)-(Math.PI))
                {
                    spriteBatch.Draw(baseSpell.overlayTexture, (position) * 1, baseSpell.overlayBounds, baseSpell.overlayColor, currentAngle, new Vector2(baseSpell.overlayBounds.Width, baseSpell.overlayBounds.Height - 32), new Vector2(1 * scale, 1 * scale), SpriteEffects.None, 0);
                }
                else if ((int)currentAngle == (int)(-(Math.PI) * 3 / 2))
                {
                    spriteBatch.Draw(baseSpell.overlayTexture, (position) * 1, baseSpell.overlayBounds, baseSpell.overlayColor, currentAngle, new Vector2(0, baseSpell.overlayBounds.Height - 32), new Vector2(1 * scale, 1 * scale), SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(baseSpell.overlayTexture, (position) * 1, baseSpell.overlayBounds, baseSpell.overlayColor, 0, Vector2.Zero, 1 * (scale), SpriteEffects.None, 0);
                }
            }*/

            base.Draw(spriteBatch);



        }

    }
}
