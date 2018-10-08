using TBAGW.Utilities.GamePlay.Characters.Friendly.Team;
using TBAGW.Utilities.GamePlay.Spells;
using TBAGW.Utilities.GamePlay.Spells.ShotPattern;
using TBAGW.Utilities.GamePlay.Stats;
using TBAGW.Utilities.OnScreen.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Characters
{
    class BaseCharacter : Shape
    {
        public BasicStatChart stats = new BasicStatChart();
        protected List<Shape> bullets = new List<Shape>();
        protected BasicPattern bulletPattern = new BasicPattern();
        protected List<Spell> randomSpells = new List<Spell>();
        public bool bHasTarget = false;
        public bool bIsAlive = true;
        public bool bHasBullets = false;
        public bool bCanFire = true;
        public bool bFriendlyFire = true;
        public bool bThisCharacterSelected = false;
        protected BaseCharacter charTarget;
        public BaseCharAsset charAsset;
        public bool bInitializedAssets = false;
        public ManualShotPattern manualShot = new ManualShotPattern();

        public BaseCharacter(Texture2D shapeTexture, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2), String shapeName = "", Rectangle shapeTextureBounds = default(Rectangle))
            : base(shapeTexture, scale, position, bCollision, center, shapeName, shapeTextureBounds)
        {
            randomSpells.Add(SpellsAssetLoader.basicMissileIce);
            randomSpells.Add(SpellsAssetLoader.basicMissileGrass);
            randomSpells.Add(SpellsAssetLoader.basicMissileFire);
            randomSpells.Add(SpellsAssetLoader.basicMissileArcane);
            randomSpells.Add(SpellsAssetLoader.basicMissileDark);
            randomSpells.Add(SpellsAssetLoader.basicMissileWind);
            randomSpells.Add(SpellsAssetLoader.basicMissileEarth);
        }

        public BaseCharacter(BaseCharAsset charAsset, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2))
            : base(charAsset.shapeTexture, scale, position, bCollision, center, charAsset.heroName, charAsset.shapeTextureBounds)
        {
            bInitializedAssets = true;
            this.charAsset = charAsset;
        }

        public void InitializeChar(BaseCharAsset charAsset, List<int> stats)
        {
            bInitializedAssets = true;
            this.stats.AssignStats(stats);
            this.charAsset = charAsset;
        }


        public void setTarget(BaseCharacter charTarget)
        {
            bHasTarget = true;
            this.charTarget = charTarget;
        }

        public void setBulletPattern(BasicPattern bulletPattern)
        {
            this.bulletPattern = bulletPattern;
        }

        public virtual void Update(GameTime gameTime, List<BaseCharacter> activeCharacters)
        {
            UpdateMovement();

            if (SelectionUtility.primarySelectedCharacter == this)
            {
                bThisCharacterSelected = true;
            }
            else
            {
                bThisCharacterSelected = false;
            }

            if (bIsAlive)
            {
                base.Update(gameTime);
            }

            if (bHasTarget && !charTarget.bIsAlive)
            {
                bHasTarget = false;
            }

            if (!bThisCharacterSelected && bHasTarget && charTarget.bIsAlive && bIsAlive)
            {
                if (typeof(StraightShot) == bulletPattern.GetType())
                {
                    var temp = (bulletPattern as StraightShot).Update(gameTime, this, charTarget.centerPoint, randomSpells, true);
                    if (temp != default(SpellMissile))
                    {
                        bullets.Add(temp);
                        bHasBullets = true;
                    }
                }
            }


            foreach (var bullet in bullets)
            {
                bullet.Update(gameTime);

                foreach (var item in activeCharacters)
                {   //Can't hit yourself with bullet with item!=this
                    if (item != this && item.bIsAlive)
                    {
                        if (bFriendlyFire)
                        {
                            if (bullet.Contains(item))
                            {
                                item.bRender = false;
                                item.bIsAlive = false;
                            }
                        }
                        else
                        {
                            if (this.GetType() != item.GetType())
                            {
                                if (bullet.Contains(item))
                                {
                                    item.bRender = false;
                                    item.bIsAlive = false;
                                }
                            }
                        }
                    }
                }
            }

            if (bullets.Count != 0)
            {
                ClearNotRenderedSprites();
            }
            else if (bullets.Count == 0)
            {
                bHasBullets = false;
            }

        }

        protected virtual void UpdateMovement()
        {
            tempVolX += velocity.X;
            tempVolY += velocity.Y;

            if ((int)Math.Abs(tempVolX) > 1 && tempVolX < 0)
            {
                if (SceneUtility.isWithinMapWidth(position.X + (int)(tempVolX)))
                {
                    GenerateHitBoxes((int)(tempVolX), 0);
                    position.X += (int)(tempVolX);
                    SceneUtility.xAxis += -(int)tempVolX;
                    tempVolX -= (int)tempVolX;
                }
                else
                {
                    tempVolX = 0;
                }

            }
            else if ((int)Math.Abs(tempVolX) > 1 && tempVolX > 0)
            {
                if (SceneUtility.isWithinMapWidth(position.X + (int)(tempVolX)))
                {
                    GenerateHitBoxes(+(int)(tempVolX), 0);
                    position.X += (int)(tempVolX);
                    SceneUtility.xAxis += -(int)tempVolX;
                    tempVolX -= (int)tempVolX;
                }
                else
                {
                    tempVolX = 0;
                }
            }

            if ((int)Math.Abs(tempVolY) > 1 && tempVolY > 0)
            {
                if (SceneUtility.isWithinMapHeight(position.Y + (int)(tempVolY)))
                {
                    GenerateHitBoxes(0, (int)(tempVolY));
                    position.Y += (int)(tempVolY);
                    SceneUtility.yAxis += -(int)tempVolY;
                    tempVolY -= (int)tempVolY;
                }
                else
                {
                    tempVolY = 0;
                }
            }
            else if ((int)Math.Abs(tempVolY) > 1 && tempVolY < 0)
            {
                if (SceneUtility.isWithinMapHeight(position.Y + (int)(tempVolY)))
                {
                    GenerateHitBoxes(0, +(int)(tempVolY));
                    position.Y += (int)(tempVolY);
                    SceneUtility.yAxis += -(int)tempVolY;
                    tempVolY -= (int)tempVolY;
                }
                else
                {
                    tempVolY = 0;
                }
            }
        }

        public void SetVelocityManual(Vector2 targetVelocity)
        {
            velocity = targetVelocity;

            if (targetVelocity != Vector2.Zero)
            {
                if (velocity.X != 0 && velocity.Y != 0)
                {
                    float temp = velocity.X;
                    if (velocity.X > 0)
                    {
                        velocity.X = (float)Math.Sqrt(Math.Pow(temp, 2) / 2);
                    }
                    else
                    {
                        velocity.X = -(float)Math.Sqrt(Math.Pow(temp, 2) / 2);
                    }

                    if (velocity.Y > 0)
                    {
                        velocity.Y = (float)Math.Sqrt(Math.Pow(temp, 2) / 2);
                    }
                    else
                    {
                        velocity.Y = -(float)Math.Sqrt(Math.Pow(temp, 2) / 2);
                    }
                }
            }


        }

        private void ClearNotRenderedSprites()
        {
            List<int> j = new List<int>();
            int k = 0;
            foreach (Shape shape in bullets)
            {
                if (!shape.bRender)
                {

                    j.Add(k);
                }

                k++;
            }
            for (int l = j.Count - 1; l > -1; l--)
            {
                bullets[j[l]].Clear();
                bullets.RemoveAt(j[l]);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (bIsAlive)
            {
                base.Draw(spriteBatch);
            }


            foreach (var bullet in bullets)
            {
                (bullet as SpellMissile).Draw(spriteBatch);
            }
        }
    }
}
