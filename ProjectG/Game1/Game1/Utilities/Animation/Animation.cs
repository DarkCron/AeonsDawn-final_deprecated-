using TBAGW;
using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using TBAGW.Scenes;
using TBAGW.Utilities.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;

namespace TBAGW.Utilities.Animation
{
    public class Animation
    {
        public Rectangle[][] frames;
        public List<int> framecountPerAnimation;
        List<Rectangle> startFrame;
        Rectangle basicFrame;
        public Rectangle currentFrameBounds;
        //int amountOfFrames;
        InputControl simpleTimer = new InputControl();
        public double frameTime;
        public Texture2D animationSpriteSheet;
        public int currentFrame = 0;
        public int currentAnimation = (int)(AnimationType.Idle);
        public int previousAnimation = (int)(AnimationType.Idle);
        public Vector2 Position = Vector2.Zero;
        public float scale = 1.0f;

        public enum AnimationType { Idle = 0, Move, Attack, Hurt, Death };

        public Animation(List<Rectangle> startFrame, List<int> framecountPerAnimation, Texture2D animationSpriteSheet, double frameTime = 500)
        {
            frames = new Rectangle[startFrame.Count][];
            this.framecountPerAnimation = framecountPerAnimation;
            basicFrame = startFrame[0];
            this.startFrame = startFrame;
            GenerateFrames();

            this.animationSpriteSheet = animationSpriteSheet;
            this.frameTime = frameTime;
        }

        private void GenerateFrames()
        {
            List<Rectangle> tempList = new List<Rectangle>();
           
            for (int i = 0; i < frames.Length; i++)
            {
                tempList.Clear();
                if (framecountPerAnimation[i] != 0) //Checks if animation frames are available
                {
                    for (int j = 0; j < framecountPerAnimation[i]; j++)
                    {
                       // Console.Out.WriteLine(i+"   "+j);
                        tempList.Add(new Rectangle(startFrame[i].X + j * startFrame[i].Width, startFrame[i].Y, startFrame[i].Width, startFrame[i].Height));
                     //   frames[i][j] = new Rectangle(startFrame[i].X + j * startFrame[i].Width, startFrame[i].Y, startFrame[i].Width, startFrame[i].Height);
                    }

                    frames[i] = tempList.ToArray();
                }
                else //If not Set idle animation frames as animation
                {
                    for (int j = 0; j < framecountPerAnimation[0]; j++)
                    {
                        tempList.Add(new Rectangle(startFrame[i].X + j * startFrame[i].Width, startFrame[i].Y, startFrame[i].Width, startFrame[i].Height));
                       // frames[i][j] = new Rectangle(startFrame[i].X + j * startFrame[i].Width, startFrame[i].Y, startFrame[i].Width, startFrame[i].Height);
                    }
                    frames[i] = tempList.ToArray();
                }


            }
        }

        public void Update(GameTime gameTime)
        {
            if (previousAnimation != currentAnimation)
            {
              //  Console.Out.WriteLine("Called: "+currentFrame );
                currentFrame = 0;
                simpleTimer.elapsedMilliseconds = 0;
            }
            else
            {

                if (simpleTimer.millisecondTimer(gameTime, frameTime))
                {
                    simpleTimer.elapsedMilliseconds = 0;
                    if (currentFrame < framecountPerAnimation[currentAnimation] - 1)
                    {
                        //Console.Out.WriteLine("New Frame");
                        currentFrame++;
                    }
                    else
                    {
                        currentFrame = 0;
                    }
                }

            }

            currentFrameBounds = frames[currentAnimation][currentFrame];
            previousAnimation = currentAnimation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(animationSpriteSheet, Position, frames[currentAnimation][currentFrame], Color.White, 0, Vector2.Zero, 1*scale, SpriteEffects.None, 0);

        }
    }
}
