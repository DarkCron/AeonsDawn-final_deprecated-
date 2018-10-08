using TBAGW.Utilities.GamePlay.Characters;
using TBAGW.Utilities.GamePlay.Spells;
using TBAGW.Utilities.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TBAGW.Utilities.OnScreen.Particles
{
    [XmlRoot("Shape")]
    public class Shape
    {
        [XmlElement("Scale")]
        public int scale = 1;
        [XmlElement("HitboxBounds")]
        public Rectangle shapeTextureBounds = new Rectangle(); //SpriteSheetBounds
        [XmlElement("Position")]
        public Vector2 position = new Vector2(0);
        [XmlElement("HasCollision")]
        public bool bCollision = false;
        [XmlElement("Centerpoint")]
        public Vector2 centerPoint = Vector2.Zero;
        [XmlElement("NameOfTheShape")]
        public String shapeName = "";
        [XmlElement("Angle")]
        public float currentAngle = 0;
        [XmlElement("ProximityIndicator")]
        public Rectangle proximityIndicator = new Rectangle(); //Calculated automatically
        [XmlElement("RectangleToDraw")]
        public Rectangle rectangleToDraw = new Rectangle(); //Box on spritesheet that's drawn on screen
        [XmlElement("PreviousFrameBounds")]
        public Rectangle prevFrameBounds = new Rectangle(); //for dynamic hitboxes and animations
        [XmlElement("TextureLoc")]
        public String textureLoc = "";
        [XmlElement("HitboxTextureLoc")]
        public String hitboxTextureLoc = "";
        [XmlElement("HitboxTextureBox")]
        public Rectangle hitBoxTexBox = new Rectangle();
        [XmlElement("SpriteGameSize")]
        public Rectangle spriteGameSize = new Rectangle();


        [XmlIgnore]
        public Texture2D shapeTexture; //Complete texture file were shape is located
        [XmlIgnore]
        public Texture2D hitBoxTexture; //Complete texture file were shape is located
        [XmlIgnore]
        public List<Rectangle> shapeHitBox = new List<Rectangle>();
        [XmlIgnore]
        List<Rectangle> originalShapeHitBox = new List<Rectangle>();
        [XmlIgnore]
        public bool bPlayAnimation = false;
        [XmlIgnore]
        bool bHasAnimations = false;
        [XmlIgnore]
        public int animationIndex = (int)(Animation.Animation.AnimationType.Idle);
        [XmlIgnore]
        public bool bHasDynamicHitbox = false;
        [XmlIgnore]
        public bool bStopAtTarget = true;
        [XmlIgnore]
        public bool bRender = true;
        [XmlIgnore]
        public Animation.Animation shapeAnimation;
        [XmlIgnore]
        public int speed = 1;
        [XmlIgnore]
        protected Vector2 targetPos;
        [XmlIgnore]
        protected String rootContent = Environment.CurrentDirectory + @"\Content\";


        public Shape()
        {

        }

        public void LoadFromSerialization()
        {

            GenerateHitBoxes();
        }


        public Shape(Texture2D shapeTexture, int scale, Vector2 position, bool bCollision, Vector2 center = default(Vector2), String shapeName = "", Rectangle shapeTextureBounds = default(Rectangle))
        {
            this.shapeTexture = shapeTexture;
            //textureLoc = shapeTexture.Name.Replace(rootContent, "");
            this.scale = scale;
            this.position = position;
            this.bCollision = bCollision;
            this.shapeName = shapeName;

            if (shapeTexture != null || shapeTexture != default(Texture2D))
            {
                rectangleToDraw = (shapeTexture.Bounds);

                if (shapeTextureBounds != default(Rectangle))
                {
                    this.shapeTextureBounds = shapeTextureBounds;
                }
                else
                {
                    this.shapeTextureBounds = shapeTexture.Bounds;
                }
            }




            if (bCollision)
            {
                GenerateHitBoxes();
            }

        }

        public Shape(Texture2D shapeTexture, Texture2D hitboxTexture, Rectangle spriteGameSize, Rectangle hitBoxTexBox, Rectangle rectangleToDraw, int scale, Vector2 position, Vector2 center = default(Vector2), String shapeName = "")
        {
            this.shapeTexture = shapeTexture;
            textureLoc = shapeTexture.Name.Replace(rootContent, "");
            this.hitBoxTexture = hitboxTexture;
            hitboxTextureLoc = hitboxTexture.Name.Replace(rootContent, "");
            this.hitBoxTexBox = hitBoxTexBox;
            this.spriteGameSize = spriteGameSize;

            this.scale = scale;
            this.position = position;
            this.bCollision = true;
            this.shapeName = shapeName;

            this.shapeTextureBounds = shapeTexture.Bounds;
            this.rectangleToDraw = rectangleToDraw;

            GenerateHitBoxesFromOtherTex();

        }

        /// <summary>
        /// This initializes the animation, if you want to play it set bPlayAnimation = true;
        /// </summary>
        /// <param name="startFrame"></param>
        /// <param name="framecountPerAnimation"></param>
        /// <param name="animationSpriteSheet"></param>
        /// <param name="frameTime"></param>
        public void InitializeAnimation(List<Rectangle> startFrame, List<int> framecountPerAnimation, Texture2D animationSpriteSheet, double frameTime = 500)
        {
            shapeAnimation = new Animation.Animation(startFrame, framecountPerAnimation, animationSpriteSheet, frameTime);
            bHasAnimations = true;
        }

        //when shape has the same hitbox as texture
        public void GenerateHitBoxes()
        {

            Color[] rawTextureData = new Color[shapeTextureBounds.Width * shapeTextureBounds.Height];
            shapeTexture.GetData<Color>(0, shapeTextureBounds, rawTextureData, 0, rawTextureData.Length);
            int width = shapeTextureBounds.Width;
            int height = shapeTextureBounds.Height;
            Color[,] rawTextureDataGrid = new Color[width, height];

            List<Rectangle> discHitBoxTemp = new List<Rectangle>();



            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {

                    // Assumes row major ordering of the array.
                    rawTextureDataGrid[row, column] = rawTextureData[row * width + column];

                    if (rawTextureDataGrid[row, column] != default(Color))
                    {
                        discHitBoxTemp.Add(new Rectangle((int)position.X + column, (int)position.Y + row, 1, 1));
                    }
                }
            }
            int currentRow = discHitBoxTemp[0].X;
            Vector2 startPos = new Vector2(discHitBoxTemp[0].X, discHitBoxTemp[0].Y);
            int newWidth = 0;
            int pos = 0;
            int prevYPos = discHitBoxTemp[0].Y - 1;
            foreach (Rectangle hitboxTemp in discHitBoxTemp)
            {

                if (currentRow == hitboxTemp.X && (prevYPos + 1) == discHitBoxTemp[pos].Y)
                {
                    newWidth++;
                    prevYPos = discHitBoxTemp[pos].Y;

                }
                else
                {

                    shapeHitBox.Add(new Rectangle((int)(startPos.X - position.X) * scale + (int)position.X, (int)(startPos.Y - position.Y) * scale + (int)position.Y, 1 * scale, (newWidth) * scale));
                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 1;
                    prevYPos = discHitBoxTemp[pos].Y;
                }

                if (pos + 1 == discHitBoxTemp.Count)
                {
                    shapeHitBox.Add(new Rectangle((int)(startPos.X - position.X) * scale + (int)position.X, (int)(startPos.Y - position.Y) * scale + (int)position.Y, 1 * scale, (newWidth) * scale));
                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 0;
                }
                pos++;
            }

            //  OptimizeHitboxes();

            originalShapeHitBox.Clear();
            foreach (var hitbox in shapeHitBox)
            {
                originalShapeHitBox.Add(new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height));
            }

        }

        public void GenerateHitBoxesFromOtherTex()
        {
            Color[] rawTextureData = new Color[hitBoxTexture.Width * hitBoxTexture.Height];
            hitBoxTexture.GetData<Color>(0, hitBoxTexBox, rawTextureData, 0, rawTextureData.Length);
            int width = hitBoxTexBox.Width;
            int height = hitBoxTexBox.Height;
            Color[,] rawTextureDataGrid = new Color[width, height];

            List<Rectangle> discHitBoxTemp = new List<Rectangle>();



            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {

                    // Assumes row major ordering of the array.
                    rawTextureDataGrid[row, column] = rawTextureData[row * width + column];

                    if (rawTextureDataGrid[row, column] != default(Color))
                    {
                        discHitBoxTemp.Add(new Rectangle((int)position.X + column, (int)position.Y + row, 1, 1));
                    }
                }
            }
            int currentRow = discHitBoxTemp[0].X;
            Vector2 startPos = new Vector2(discHitBoxTemp[0].X, discHitBoxTemp[0].Y);
            int newWidth = 0;
            int pos = 0;
            int prevYPos = discHitBoxTemp[0].Y - 1;
            foreach (Rectangle hitboxTemp in discHitBoxTemp)
            {

                if (currentRow == hitboxTemp.X && (prevYPos + 1) == discHitBoxTemp[pos].Y)
                {
                    newWidth++;
                    prevYPos = discHitBoxTemp[pos].Y;

                }
                else
                {

                    shapeHitBox.Add(new Rectangle((int)(startPos.X - position.X) * scale + (int)position.X, (int)(startPos.Y - position.Y) * scale + (int)position.Y, 1 * scale, (newWidth) * scale));
                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 1;
                    prevYPos = discHitBoxTemp[pos].Y;
                }

                if (pos + 1 == discHitBoxTemp.Count)
                {
                    shapeHitBox.Add(new Rectangle((int)(startPos.X - position.X) * scale + (int)position.X, (int)(startPos.Y - position.Y) * scale + (int)position.Y, 1 * scale, (newWidth) * scale));
                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 0;
                }
                pos++;
            }

            //  OptimizeHitboxes();

            originalShapeHitBox.Clear();
            foreach (var hitbox in shapeHitBox)
            {
                originalShapeHitBox.Add(new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height));
            }
        }

        private void OptimizeHitboxes()
        {
            bool btemp = false;
            List<int> tempList = new List<int>();

            do
            {
                bool btemp2 = true;
                for (int i = 0; i < shapeHitBox.Count - 1; i++)
                {
                    for (int j = 0; j < shapeHitBox.Count - 1; j++)
                    {
                        if (shapeHitBox[i].Height == shapeHitBox[j].Height
                            && shapeHitBox[j].X == (shapeHitBox[i].X + 1 * scale)
                            && shapeHitBox[i].Y == shapeHitBox[j].Y)
                        {
                            tempList.Insert(0, i);


                            int tempWidth = shapeHitBox[i].Width;
                            int tempHeight = shapeHitBox[i].Height;
                            int tempX = shapeHitBox[i].X;
                            int tempY = shapeHitBox[i].Y;

                            int count = 1;
                            while (count != shapeHitBox.Count - 1
                                && shapeHitBox[j + count].Y == shapeHitBox[j].Y
                                && shapeHitBox[j + count].Height == shapeHitBox[j].Height)
                            {
                                count++;
                                tempWidth += 1 * scale;
                            }
                            tempWidth += scale;
                            shapeHitBox.Add(new Rectangle(tempX, tempY, tempWidth, tempHeight));
                            shapeHitBox.RemoveRange(j, count);


                            btemp2 = false;

                            break;
                        }
                        if (!btemp2)
                        {
                            break;
                        }
                    }

                    if (btemp2)
                    {
                        btemp = false;

                    }
                    else
                    {
                        btemp = true;
                    }
                }
            } while (btemp);

            foreach (var number in tempList)
            {
                shapeHitBox.RemoveAt(number);
            }

            originalShapeHitBox.Clear();
            foreach (var hitbox in shapeHitBox)
            {
                originalShapeHitBox.Add(new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height));
            }


        }

        protected List<Rectangle> GenerateDynamicHitBoxes(Rectangle frameToGenerateHitbox)
        {
            // Console.Out.WriteLine(frameToGenerateHitbox);
            //  Rectangle tmprct = new Rectangle(frameToGenerateHitbox.X+1,frameToGenerateHitbox.Y+1,frameToGenerateHitbox.Width-2,frameToGenerateHitbox.Height-2);
            Color[] rawTextureData = new Color[frameToGenerateHitbox.Width * frameToGenerateHitbox.Height];
            shapeAnimation.animationSpriteSheet.GetData<Color>(0, frameToGenerateHitbox, rawTextureData, 0, rawTextureData.Length);
            int width = frameToGenerateHitbox.Width;
            int height = frameToGenerateHitbox.Height;
            Color[,] rawTextureDataGrid = new Color[width, height];

            List<Rectangle> discHitBoxTemp = new List<Rectangle>();



            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {

                    // Assumes row major ordering of the array.
                    rawTextureDataGrid[row, column] = rawTextureData[row * width + column];

                    if (rawTextureDataGrid[row, column] != default(Color))
                    {
                        discHitBoxTemp.Add(new Rectangle((int)position.X + column, (int)position.Y + row, 1, 1));
                    }
                }
            }

            int currentRow = discHitBoxTemp[0].X;
            Vector2 startPos = new Vector2(discHitBoxTemp[0].X, discHitBoxTemp[0].Y);
            int newWidth = 0;
            int pos = 0;
            int prevYPos = discHitBoxTemp[0].Y - 1;
            foreach (Rectangle hitboxTemp in discHitBoxTemp)
            {

                if (currentRow == hitboxTemp.X && (prevYPos + 1) == discHitBoxTemp[pos].Y)
                {
                    newWidth++;
                    prevYPos = discHitBoxTemp[pos].Y;

                }
                else
                {

                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 1;
                    prevYPos = discHitBoxTemp[pos].Y;
                }

                if (pos + 1 == discHitBoxTemp.Count)
                {
                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 0;
                }
                pos++;
            }
            //  Console.Out.WriteLine("I got this far: ");
            return discHitBoxTemp;

        }

        public Rectangle[][][] shapeHitboxAnimations;
        public void DynamicHitBoxes()
        {
            bHasDynamicHitbox = true;
            shapeHitboxAnimations = new Rectangle[shapeAnimation.frames.Length][][];
            //  Console.Out.WriteLine(shapeHitboxAnimations.Length);
            for (int i = 0; i < shapeAnimation.frames.Length; i++)
            {
                shapeHitboxAnimations[i] = new Rectangle[shapeAnimation.framecountPerAnimation[i]][];
            }

            for (int i = 0; i < shapeAnimation.frames.Length; i++)
            {
                for (int j = 0; j < shapeAnimation.frames[i].Length; j++)
                {
                    List<Rectangle> discHitBoxTemp = GenerateDynamicHitBoxes(shapeAnimation.frames[i][j]);
                    // Console.Out.WriteLine(discHitBoxTemp.Count);
                    shapeHitboxAnimations[i][j] = discHitBoxTemp.ToArray();
                    // Console.Out.WriteLine(shapeHitboxAnimations[i][j].Length);
                }
            }
        }

        protected void GenerateHitBoxes(int x, int y)
        {
            List<Rectangle> discHitBoxTemp = new List<Rectangle>();
            foreach (var hitbox in originalShapeHitBox)
            {
                discHitBoxTemp.Add(new Rectangle(hitbox.X + (int)(x), hitbox.Y + (int)(y), hitbox.Width, hitbox.Height));
            }
            shapeHitBox.Clear();
            foreach (var hitbox in discHitBoxTemp)
            {
                shapeHitBox.Add(hitbox);
            }

            originalShapeHitBox.Clear();
            foreach (var hitbox in shapeHitBox)
            {
                originalShapeHitBox.Add(new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height));
            }

        }

        public void Position(Vector2 position)
        {
            this.position = position;
            //  originalPosition = position / 1;
            shapeHitBox.Clear();
            if (bCollision)
            {
                GenerateHitBoxes();
            }
        }

        public void MovePosition(Vector2 position)
        {
            this.position += position;
            //  originalPosition = position / 1;
            shapeHitBox.Clear();
            GenerateHitBoxes();
        }

        public void CleanPixels(int i)
        {
            int area;
            List<int> j = new List<int>();
            int k = 0;
            foreach (Rectangle hitBox in shapeHitBox)
            {
                area = hitBox.Width * hitBox.Height / scale / scale;
                if (area < i + 1)
                {

                    j.Add(k);
                }

                k++;
            }

            for (int l = j.Count - 1; l > 0; l--)
            {
                shapeHitBox.RemoveAt(j[l]);
            }

            //OptimizeHitboxes();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!bHasAnimations)
            {


                if ((int)currentAngle == (int)-(Math.PI / 2))
                {
                    spriteBatch.Draw(shapeTexture, (position) * 1, rectangleToDraw, Color.White, currentAngle, new Vector2(shapeTextureBounds.Width, 0), new Vector2(1 * scale, 1 * scale), SpriteEffects.None, 0);
                }
                else if ((int)currentAngle == (int)-(Math.PI))
                {
                    spriteBatch.Draw(shapeTexture, (position) * 1, rectangleToDraw, Color.White, currentAngle, new Vector2(shapeTextureBounds.Width, shapeTextureBounds.Height), new Vector2(1 * scale, 1 * scale), SpriteEffects.None, 0);
                }
                else if ((int)currentAngle == (int)(-(Math.PI) * 3 / 2))
                {
                    spriteBatch.Draw(shapeTexture, (position) * 1, rectangleToDraw, Color.White, currentAngle, new Vector2(0, shapeTextureBounds.Height), new Vector2(1 * scale, 1 * scale), SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(shapeTexture, (position) * 1, rectangleToDraw, Color.White, 0, Vector2.Zero, 1 * (scale), SpriteEffects.None, 0);
                }
            }
            else if (bHasAnimations)
            {
                // Console.Out.WriteLine("Called?");
                Texture2D tempTexture = shapeAnimation.animationSpriteSheet;
                Rectangle tempRectangle = shapeAnimation.currentFrameBounds;
                if ((int)currentAngle == (int)-(Math.PI / 2))
                {
                    spriteBatch.Draw(shapeTexture, (position) * 1, tempRectangle, Color.White, currentAngle, new Vector2(shapeTextureBounds.Width, 0), new Vector2(1 * scale, 1 * scale), SpriteEffects.None, 0);
                }
                else if ((int)currentAngle == (int)-(Math.PI))
                {
                    spriteBatch.Draw(shapeTexture, (position) * 1, tempRectangle, Color.White, currentAngle, new Vector2(shapeTextureBounds.Width, shapeTextureBounds.Height), new Vector2(1 * scale, 1 * scale), SpriteEffects.None, 0);
                }
                else if ((int)currentAngle == (int)(-(Math.PI) * 3 / 2))
                {
                    spriteBatch.Draw(shapeTexture, (position) * 1, tempRectangle, Color.White, currentAngle, new Vector2(0, shapeTextureBounds.Height), new Vector2(1 * scale, 1 * scale), SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(shapeTexture, (position) * 1, tempRectangle, Color.White, 0, Vector2.Zero, 1 * (scale), SpriteEffects.None, 0);
                }
            }

            if (!hitString.Equals(""))
            {
                spriteBatch.DrawString(Game1.defaultFont, hitString, position + new Vector2(64, 64), Color.Black);
            }

            /*
            foreach (Rectangle hitbox in shapeHitBox)
            {
                spriteBatch.Draw(Game1.hitboxHelp, new Rectangle(hitbox.X, hitbox.Y, hitbox.Width, hitbox.Height), Color.White);

            }*/

            if (bHasDynamicHitbox && bPlayAnimation)
            {
                if (shapeAnimation.currentAnimation == animationIndex)
                {
                    foreach (var hitbox in shapeHitboxAnimations[shapeAnimation.currentAnimation][shapeAnimation.currentFrame])
                    {
                        spriteBatch.Draw(Game1.hitboxHelp, new Rectangle(hitbox.X,
            hitbox.Y,
           hitbox.Width,
            hitbox.Height), Color.Blue);
                    }
                }

            }



        }

        public Vector2 velocity = Vector2.Zero;
        protected bool bGoLeft = false;
        protected bool bGoUp = false;
        public void SetTarget(Vector2 targetPos)
        {
            if (position.X > targetPos.X)
            {
                bGoLeft = true;
            }
            else
            {
                bGoLeft = false;
            }

            if (position.Y > targetPos.Y)
            {
                bGoUp = false;
            }
            else
            {
                bGoUp = true;
            }

            velocity = Vector2.Zero;
            //    this.originalTargetPos = targetPos / 1;
            this.targetPos = targetPos;
            int x = (int)Math.Abs(targetPos.X - centerPoint.X);
            int y = (int)Math.Abs(targetPos.Y - centerPoint.Y);
            double a = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            velocity.X = (float)(x / a);
            velocity.Y = (float)(y / a);
            velocity *= (float)Math.Sqrt(speed);
            tempVolX = 0;
            tempVolY = 0;
            //   Console.Out.WriteLine("Length x: "+x+"  Length y: "+y+"\n"+velocity.X+"         "+velocity.Y);
            // Console.Out.WriteLine(velocity.LengthSquared());
        }

        public void SetVelocity(Vector2 targetVelocity)
        {
            velocity.X = Math.Abs(targetVelocity.X);
            velocity.Y = Math.Abs(targetVelocity.Y);

            if (targetVelocity.X < 0)
            {
                bGoLeft = true;
            }
            else
            {
                bGoLeft = false;
            }

            if (targetVelocity.Y > 0)
            {
                bGoUp = false;
            }
            else
            {
                bGoUp = true;
            }
        }

        public void SetVelocity(float angle)
        {

            velocity.X = Math.Abs((float)Math.Sin(angle)) * speed;
            velocity.Y = Math.Abs((float)Math.Cos(angle)) * speed;

            if (Math.Sin(angle) < 0)
            {
                bGoLeft = true;
            }
            else
            {
                bGoLeft = false;
            }

            if (Math.Cos(angle) > 0)
            {
                bGoUp = false;
            }
            else
            {
                bGoUp = true;
            }
        }

        protected double tempVolX = 0;
        protected double tempVolY = 0;
        public virtual void Update(GameTime gameTime)
        {
            // targetPos = originalTargetPos;

            proximityIndicator = spriteGameSize;
            //  Console.Out.WriteLine(shapeHitBox[1] + " & " + originalShapeHitBox[1]);

            /*
             * This handles movement
             * 
             * 
             * 
             * 
             */

            bRender = SceneUtility.isWithinMap(position);

            /*
            if (position.X > SceneUtility.currentMapSize.X + shapeTextureBounds.Width * scale)
            {
                bRender = false;

            }
            else if (position.Y > SceneUtility.currentMapSize.Y + shapeTextureBounds.Height * scale)
            {
                bRender = false;
            }
            else if (position.X < 0 - shapeTextureBounds.Height * scale-100)
            {
                bRender = false;
            }
            else if (position.Y < 0 - shapeTextureBounds.Height * scale-100)
            {
                bRender = false;
            }*/



            /*
             * Do other stuff below
             * 
             * 
             * 
             */
            if (bHasAnimations && bPlayAnimation)
            {
                shapeAnimation.Update(gameTime);
                shapeAnimation.currentAnimation = animationIndex;
            }

            if (bHasDynamicHitbox && bHasAnimations && !prevFrameBounds.Equals(shapeAnimation.currentFrameBounds))
            {
                //   GenerateDynamicHitBoxes(shapeAnimation.currentFrameBounds);
            }

            if (bHasDynamicHitbox && bHasAnimations)
            {
                //   prevFrameBounds = shapeAnimation.currentFrameBounds;
            }

            //    originalPosition = position / 1;
        }


        public void Clear()
        {
            shapeHitBox.Clear();
            originalShapeHitBox.Clear();

            if (bHasDynamicHitbox)
            {
            }
        }

        public bool Contains(Vector2 target)
        {
            if (proximityIndicator.Contains(target) && bCollision)
            {
                if (!bHasDynamicHitbox)
                {
                    foreach (var hitbox in shapeHitBox)
                    {
                        if (hitbox.Contains(target))
                        {
                            return true;
                        }
                    }
                }
                else if (bHasDynamicHitbox)
                {

                    if (bHasDynamicHitbox && bPlayAnimation)
                    {
                        if (shapeAnimation.currentAnimation == animationIndex)
                        {
                            foreach (var hitbox in shapeHitboxAnimations[shapeAnimation.currentAnimation][shapeAnimation.currentFrame])
                            {
                                if (hitbox.Contains(target))
                                {
                                    return true;
                                }
                            }
                        }

                    }
                }

            }

            return false;
        }

        public virtual void Contains(Rectangle target)
        {
            if (proximityIndicator.Intersects(target) && bCollision)
            {
                if (!bHasDynamicHitbox)
                {
                    foreach (var hitbox in shapeHitBox)
                    {

                        if (hitbox.Intersects(target))
                        {

                            bRender = false;
                            break;
                        }
                    }
                }
                else if (bHasDynamicHitbox)
                {

                    if (bHasDynamicHitbox && bPlayAnimation)
                    {
                        if (shapeAnimation.currentAnimation == animationIndex)
                        {
                            foreach (var hitbox in shapeHitboxAnimations[shapeAnimation.currentAnimation][shapeAnimation.currentFrame])
                            {
                                if (hitbox.Intersects(target))
                                {
                                    bRender = false;
                                    break;
                                }
                            }
                        }

                    }
                }
            }

        }

        protected String hitString = "";
        public virtual bool Contains(Shape shape)
        {
            if (proximityIndicator.Intersects(shape.proximityIndicator) && bCollision)
            {
                if (!bHasDynamicHitbox)
                {
                    foreach (var hitbox in shapeHitBox)
                    {
                        foreach (var target in shape.shapeHitBox)
                        {
                            if (hitbox.Intersects(target))
                            {
                                bRender = false;
                                return true;

                            }
                        }
                    }
                }
                if (bHasDynamicHitbox)
                {

                    if (bHasDynamicHitbox && bPlayAnimation)
                    {
                        if (shapeAnimation.currentAnimation == animationIndex)
                        {
                            foreach (var hitbox in shapeHitboxAnimations[shapeAnimation.currentAnimation][shapeAnimation.currentFrame])
                            {
                                foreach (var target in shape.shapeHitBox)
                                {
                                    if (hitbox.Intersects(target) || target.Contains(hitbox))
                                    {
                                        bRender = false;
                                        return true;

                                    }
                                }
                            }
                        }

                    }
                }
            }
            return false;
        }

        public virtual bool Contains(SpellMissile missile)
        {
            if (proximityIndicator.Intersects(missile.proximityIndicator) && bCollision)
            {
                if (!bHasDynamicHitbox)
                {
                    foreach (var hitbox in shapeHitBox)
                    {
                        foreach (var target in missile.shapeHitBox)
                        {
                            if (hitbox.Intersects(target))
                            {
                                return true;

                            }
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Rotation is !!!counterclockwise!!!
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="angle2"></param>
        public void Rotate(int angle)
        {
            if (angle != 0)
            {
                List<Rectangle> tempHitbox = new List<Rectangle>();
                Rectangle tempRectangle = new Rectangle();


                switch (angle)
                {

                    case 90:
                        tempHitbox.Clear();

                        foreach (var hitbox in shapeHitBox)
                        {

                            int tempX = (int)(hitbox.X / 1);
                            int tempY = (int)(hitbox.Y / 1);
                            int tempWidth = (int)(hitbox.Width / 1);
                            int tempHeight = (int)(hitbox.Height / 1);
                            Vector2 tempPosition = new Vector2(tempX - (position.X), tempY - (position.Y));

                            tempRectangle.Y = (int)((-tempPosition.X + (shapeTextureBounds.Width - 1 * scale) - (tempWidth - 1 * scale) + position.Y)) + (int)((shapeTextureBounds.Width * (scale - 1)));
                            tempRectangle.X = (int)((tempPosition.Y) + (position.X));
                            tempRectangle.Width = (int)(tempHeight);
                            tempRectangle.Height = (int)(tempWidth);

                            tempRectangle.Width = (int)(tempRectangle.Width * 1);
                            tempRectangle.Height = (int)(tempRectangle.Height * 1);
                            tempRectangle.X = (int)(tempRectangle.X * 1);
                            tempRectangle.Y = (int)(tempRectangle.Y * 1);

                            tempHitbox.Add(tempRectangle);


                        }
                        currentAngle = (float)-Math.PI / 2;

                        if (angle == 270)
                        {
                            currentAngle = (float)-Math.PI * 3 / 2;
                        }

                        break;
                    case 180:

                        foreach (var hitbox in shapeHitBox)
                        {
                            int tempX = (int)(hitbox.X / 1);
                            int tempY = (int)(hitbox.Y / 1);
                            int tempWidth = (int)(hitbox.Width / 1);
                            int tempHeight = (int)(hitbox.Height / 1);
                            Vector2 tempPosition = new Vector2((tempX - (position.X)), (tempY - (position.Y)));

                            tempRectangle.X = (shapeTextureBounds.Width * scale - 1 * scale) - (int)(tempPosition.X) - (tempWidth - 1 * scale) + (int)(position.X);
                            tempRectangle.Y = -(int)(tempPosition.Y) + (shapeTextureBounds.Height * scale - 1 * scale) - (int)(tempHeight - 1 * scale) + (int)(position.Y);
                            tempRectangle.Width = (int)(tempWidth);
                            tempRectangle.Height = (int)(tempHeight);

                            tempRectangle.Width = (int)(tempRectangle.Width * 1);
                            tempRectangle.Height = (int)(tempRectangle.Height * 1);
                            tempRectangle.X = (int)(tempRectangle.X * 1);
                            tempRectangle.Y = (int)(tempRectangle.Y * 1);
                            tempHitbox.Add(tempRectangle);

                        }
                        currentAngle = (float)-Math.PI;
                        if (angle == 270)
                        {
                            shapeHitBox.Clear();
                            foreach (var hitbox in tempHitbox)
                            {
                                shapeHitBox.Add(hitbox);
                            }
                            goto case 90;
                        }
                        break;
                    case 270:
                        goto case 180;

                }


                shapeHitBox.Clear();
                foreach (var hitbox in tempHitbox)
                {
                    shapeHitBox.Add(hitbox);
                }
            }




        }

    }
}
