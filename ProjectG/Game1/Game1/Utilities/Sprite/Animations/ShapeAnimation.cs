using LUA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using LUA;
using TBAGW.Utilities.Characters;

namespace TBAGW.Utilities.Sprite
{
    [XmlRoot("Shape Animation")]
    [XmlInclude(typeof(ParticleAnimation))]
    [XmlInclude(typeof(MagicCircleBase))]
    public class ShapeAnimation
    {
        [XmlElement("Uses Directions")]
        public bool bDirectional = false;
        [XmlArrayItem("Frames")]
        public List<Rectangle> animationFrames = new List<Rectangle>();
        [XmlArrayItem("FramesDirectional")]
        public List<List<Rectangle>> animationFramesDirectional = new List<List<Rectangle>>();
        [XmlElement("Frame Interval(in ms)")]
        public int frameInterval = 250;
        [XmlElement("Texture File Location")]
        public String texFileLoc = "TempTexture";
        [XmlElement("Animation Identifier")]
        public int animationID = 0;
        [XmlElement("Animation Name")]
        public String animationName = "anim";

        [XmlIgnore]
        public bool bPause = false;
        [XmlIgnore]
        public BaseCharacter bc;
        [XmlIgnore]
        public bool bAnimationFinished = false;
        [XmlIgnore]
        public bool bMustEndAnimation = false;
        [XmlIgnore]
        public int frameIndex = 0;
        [XmlIgnore]
        public int elapsedFrameTime = 0; //in milliseconds
        [XmlIgnore]
        public Texture2D animationTexture;
        [XmlIgnore]
        public bool bPlayAnimation = false;
        [XmlIgnore]
        public bool bSimplePlayOnce = false;
        [XmlIgnore]
        public int animationEnder = 0;
        [XmlIgnore]
        static Texture2D shadowBoxTex = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\ShadowBox");

        public void ReloadTexture()
        {
            if (!texFileLoc.Equals(""))
            {
                animationTexture = Game1.contentManager.Load<Texture2D>(texFileLoc);
            }
            if (animationFrames.Count == 0)
            {
                animationFrames.Add(new Rectangle(0, 0, 64, 64));
            }
        }

        public void SimpleReset()
        {
            frameIndex = 0;
            elapsedFrameTime = 0;
            bAnimationFinished = false;
            bMustEndAnimation = false;
        }

        public void ToggleDirectionalAnim()
        {
            if (bDirectional)
            {
                bDirectional = false;
            }
            else
            {
                bDirectional = true;
                if (animationFramesDirectional.Count == 0)
                {
                    foreach (var item in Enum.GetNames(typeof(BaseSprite.Rotation)))
                    {
                        animationFramesDirectional.Add(new List<Rectangle>());
                    }
                }
            }
        }

        public ShapeAnimation()
        {

        }

        public void Start()
        {
            if (bMustEndAnimation)
            {
                bMustEndAnimation = false;
            }
        }

        public void Update(GameTime gameTime, BaseSprite bs)
        {
            if (animationTexture != null && !bPause)
            {
                if (animationFrames.Count != 0 && animationFramesDirectional.Count != 0)
                {

                }

                elapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedFrameTime > frameInterval)
                {
                    elapsedFrameTime = 0;
                    if (bDirectional)
                    {
                        if (frameIndex != animationFramesDirectional[bs.rotationIndex].Count - 1)
                        {
                            frameIndex++;
                        }
                        else
                        {
                            frameIndex = 0;
                            if (bMustEndAnimation)
                            {
                                bMustEndAnimation = false;
                                frameIndex = 0;
                                bPlayAnimation = false;

                                if (bs.GetType().Equals(typeof(BaseCharacter)))
                                {
                                    (bs as BaseCharacter).animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                                    Console.WriteLine("Detected a BaseCharacter with animation, ending current animation, switching to Idle");
                                }

                                if (bs.GetType().Equals(typeof(BaseSprite)))
                                {
                                    bs.animationIndex = animationEnder;
                                    Console.WriteLine("Detected a BaseCharacter with animation, ending current animation, switching to 0");
                                }

                            }
                        }
                    }
                    else
                    {
                        if (frameIndex != animationFrames.Count - 1)
                        {
                            frameIndex++;
                        }
                        else
                        {
                            frameIndex = 0;
                            if (bMustEndAnimation)
                            {
                                if (bs.GetType().Equals(typeof(BaseCharacter)))
                                {
                                    (bs as BaseCharacter).animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                                    Console.WriteLine("Detected a BaseCharacter with animation, ending current animation, switching to Idle");
                                }


                                if (bs.GetType().Equals(typeof(BaseSprite)))
                                {
                                    bs.animationIndex = animationEnder;
                                    Console.WriteLine("Detected a BaseCharacter with animation, ending current animation, switching to 0");
                                }
                            }

                        }
                    }
                }
            }

            if (bs != null)
            {
                bs.UpdateAnimInfo();
            }

        }

        public virtual void UpdateAnimationForItems(GameTime gameTime)
        {
            if (animationTexture != null && !bPause && !bAnimationFinished)
            {
                if (!(animationFrames.Count != 0 && animationFramesDirectional.Count != 0))
                {

                }

                elapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;

                if (elapsedFrameTime > frameInterval)
                {
                    elapsedFrameTime = 0;

                    if (frameIndex != animationFrames.Count - 1)
                    {
                        frameIndex++;
                    }
                    else if (!bSimplePlayOnce)
                    {
                        frameIndex = 0;
                    }
                    else
                    {
                        bAnimationFinished = true;
                    }
                }

            }


        }

        public void PreviewUpdate(GameTime gameTime, int ri)
        {
            if (animationTexture != null && animationFramesDirectional[ri].Count != 0 && !bPause)
            {
                elapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedFrameTime > frameInterval)
                {
                    elapsedFrameTime = 0;
                    if (bDirectional)
                    {
                        if (frameIndex != animationFramesDirectional[ri].Count - 1)
                        {
                            frameIndex++;
                        }
                        else
                        {
                            frameIndex = 0;
                        }
                    }
                    else
                    {
                        if (frameIndex != animationFrames.Count - 1)
                        {
                            frameIndex++;
                        }
                        else
                        {
                            frameIndex = 0;
                        }
                    }
                }
            }
            else if (animationFramesDirectional[ri].Count != 0)
            {
                frameIndex = 0; //failsafe
            }


        }

        public void DrawForWaterReflecton(SpriteBatch sb, BaseSprite bs, Color drawColor = default(Color))
        {
            if (drawColor == default(Color))
            {
                drawColor = Color.White;
            }

            if (drawColor == Color.White)
            {
                drawColor = Color.White * ((float)bs.spriteOpacity / 100f);
            }
            else
            {
                drawColor = drawColor * ((float)bs.spriteOpacity / 100f); ;
            }

            if (animationTexture != null)
            {
                if (!bDirectional)
                {
                    // sb.Draw(animationTexture, bs.spriteGameSize, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                    // bs.MovementLogic();

                    switch (bs.groundTileType)
                    {
                        case TileSource.TileType.Ground:
                            sb.Draw(animationTexture, null, bs.spriteGameSize, animationFrames[frameIndex], null, 0f, bs.scaleVector, drawColor, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Building:
                            sb.Draw(animationTexture, bs.spriteGameSize, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:
                            //  sb.Draw(animationTexture, bs.spriteGameSize, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            var tempRect = new Rectangle(bs.spriteGameSize.X, bs.spriteGameSize.Y + 15, bs.spriteGameSize.Width, bs.spriteGameSize.Height - 15);
                            float divider = ((float)bs.spriteGameSize.Height - 15f) / (float)bs.spriteGameSize.Height; ;
                            float divider2 = (15f) / (float)bs.spriteGameSize.Height; ;
                            var refR = animationFrames[frameIndex];
                            var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                            sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            break;
                        default:
                            sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }
                }
                else
                {
                    switch (bs.groundTileType)
                    {
                        case TileSource.TileType.Ground:
                            sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Building:
                            sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:


                            var tempRect = new Rectangle(bs.spriteGameSize.X, bs.spriteGameSize.Y + 15, bs.spriteGameSize.Width, bs.spriteGameSize.Height - 15);
                            float divider = (64f - 15f) / 64f; ;
                            float divider2 = (15f) / 64f; ;
                            var refR = animationFramesDirectional[bs.rotationIndex][frameIndex];
                            var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 1), refR.Width, (int)(refR.Height * divider));
                            sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            break;
                        default:
                            sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }

                    bs.spriteGameSize.Width = 64;
                    bs.spriteGameSize.Height = 64;
                }
            }
            else
            {
                Console.WriteLine("Attempting reload texture!");
                ReloadTexture();
            }

        }

        public void DrawForWaterReflecton(SpriteBatch sb, BaseSprite bs, Rectangle destination, Color drawColor = default(Color))
        {
            if (drawColor == default(Color))
            {
                drawColor = Color.White;
            }

            if (drawColor == Color.White)
            {
                drawColor = Color.White * ((float)bs.spriteOpacity / 100f);
            }
            else
            {
                drawColor = drawColor * ((float)bs.spriteOpacity / 100f); ;
            }

            if (animationTexture != null)
            {
                if (!bDirectional)
                {
                    // sb.Draw(animationTexture, bs.spriteGameSize, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                    // bs.MovementLogic();

                    switch (bs.groundTileType)
                    {
                        case TileSource.TileType.Ground:
                            sb.Draw(animationTexture, destination, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Building:
                            sb.Draw(animationTexture, destination, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:
                            //  sb.Draw(animationTexture, destination, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            var tempRect = new Rectangle(destination.X, destination.Y + 15, destination.Width, destination.Height - 15);
                            float divider = ((float)destination.Height - 15f) / (float)destination.Height; ;
                            float divider2 = (15f) / (float)destination.Height; ;
                            var refR = animationFrames[frameIndex];
                            var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                            sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            break;
                        default:
                            sb.Draw(animationTexture, destination, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }
                }
                else
                {
                    switch (bs.groundTileType)
                    {
                        case TileSource.TileType.Ground:
                            sb.Draw(animationTexture, destination, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Building:
                            sb.Draw(animationTexture, destination, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:


                            var tempRect = new Rectangle(destination.X, destination.Y + 15, destination.Width, destination.Height - 15);
                            float divider = (64f - 15f) / 64f; ;
                            float divider2 = (15f) / 64f; ;
                            var refR = animationFramesDirectional[bs.rotationIndex][frameIndex];
                            var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 1), refR.Width, (int)(refR.Height * divider));
                            sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            break;
                        default:
                            sb.Draw(animationTexture, destination, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Attempting reload texture!");
                ReloadTexture();
            }

        }

        public void Draw(SpriteBatch sb, BaseSprite bs, Color drawColor = default(Color))
        {
            if (drawColor == default(Color))
            {
                drawColor = Color.White;
            }

            if (drawColor == Color.White)
            {
                drawColor = Color.White * ((float)bs.spriteOpacity / 100f);
            }
            else
            {
                drawColor = drawColor * ((float)bs.spriteOpacity / 100f); ;
            }

            if (animationTexture != null)
            {
                if (!bDirectional)
                {
                    // sb.Draw(animationTexture, bs.spriteGameSize, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                    // bs.MovementLogic();

                    switch (bs.groundTileType)
                    {
                        case TileSource.TileType.Ground:
                           // Rectangle r = bs.trueMapSize();

                            sb.Draw(animationTexture, null, bs.trueMapSize(), animationFrames[frameIndex], Vector2.Zero, bs.toRadiansRotation(), new Vector2(1), drawColor, (SpriteEffects)bs.spriteEffect, 0);


                            break;
                        case TileSource.TileType.Building:
                            sb.Draw(animationTexture, bs.trueMapSize(), animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:
                            var tempRect = new Rectangle(bs.trueMapSize().X, bs.trueMapSize().Y + 15, bs.trueMapSize().Width, bs.trueMapSize().Height - 15);
                            float divider = ((float)bs.trueMapSize().Height - 15f) / (float)bs.trueMapSize().Height; ;
                            float divider2 = (15f) / (float)bs.trueMapSize().Height; ;
                            var refR = animationFrames[frameIndex];
                            var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));

                            sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, bs.toRadiansRotation(), Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            break;
                        default:
                            sb.Draw(animationTexture, bs.trueMapSize(), animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }
                }
                else
                {
                    switch (bs.groundTile.tileSource.tileType)
                    {
                        case TileSource.TileType.Ground:
                            sb.Draw(animationTexture, bs.trueMapSize(), animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Building:
                            sb.Draw(animationTexture, bs.trueMapSize(), animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:

                            if (true)
                            {
                                var tempRect = new Rectangle(bs.trueMapSize().X, bs.trueMapSize().Y + 15, bs.trueMapSize().Width, bs.trueMapSize().Height - 15);
                                float divider = ((float)bs.trueMapSize().Height - 15f) / (float)bs.trueMapSize().Height; ;
                                float divider2 = (15f) / (float)bs.trueMapSize().Height; ;
                                var refR = animationFramesDirectional[bs.rotationIndex][frameIndex];
                                var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                                sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            }
                            break;
                        case TileSource.TileType.Stairs:
                            if (bs.GetType() == typeof(BaseCharacter))
                            {
                                var p = bs.groundTile.mapPosition.Location.ToVector2() - (bs.spriteGameSize.Location.ToVector2() + new Vector2(32, 56));
                                //  p.Y = p.X;
                                //   p.X = 0;
                                // Rectangle r = new Rectangle(bs.spriteGameSize.X,bs.spriteGameSize.Y+(int)p.X-32,bs.spriteGameSize.Width,bs.spriteGameSize.Height);

                                if (bs.groundTile.spriteEffect == SpriteEffects.FlipHorizontally)
                                {
                                    if (bs.rotationIndex == (int)BaseCharacter.Rotation.Right)
                                    {
                                        var diff = 64 + p.X + p.Y - 0;
                                        bs.position += new Vector2(0, diff);

                                    }
                                    else if (bs.rotationIndex == (int)BaseCharacter.Rotation.Left)
                                    {
                                        var diff = 64 + p.X + p.Y + 0;
                                        bs.position += new Vector2(0, diff);
                                        //  bs.position.Y = bs.groundTile.mapPosition.Y+46-
                                    }
                                }


                                if (bs.groundTile.spriteEffect == SpriteEffects.None)
                                {
                                    if (bs.rotationIndex == (int)BaseCharacter.Rotation.Right)
                                    {
                                        var diff = p.Y - p.X + 0;
                                        bs.position += new Vector2(0, diff);
                                    }
                                    else if (bs.rotationIndex == (int)BaseCharacter.Rotation.Left)
                                    {
                                        var diff = p.Y - p.X - 0;
                                        bs.position += new Vector2(0, diff);
                                    }

                                }
                                bs.UpdatePosition();
                                //      sb.Draw(animationTexture, r, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                                sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            }
                            else
                            {
                                sb.Draw(animationTexture, bs.trueMapSize(), animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            }
                            //   sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        default:
                            sb.Draw(animationTexture, bs.trueMapSize(), animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }

                    //bs.spriteGameSize.Width = 64;
                    //bs.spriteGameSize.Height = 64;
                }
            }
            else
            {
                Console.WriteLine("Attempting reload texture!");
                ReloadTexture();
            }

        }

        public void Draw(SpriteBatch sb, BaseSprite bs, Rectangle destination, Color drawColor = default(Color))
        {
            if (drawColor == default(Color))
            {
                drawColor = Color.White;
            }

            if (drawColor == Color.White)
            {
                drawColor = Color.White * ((float)bs.spriteOpacity / 100f);
            }
            else
            {
                drawColor = drawColor * ((float)bs.spriteOpacity / 100f); ;
            }

            if (animationTexture != null)
            {
                if (!bDirectional)
                {
                    // sb.Draw(animationTexture, bs.spriteGameSize, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                    // bs.MovementLogic();

                    switch (bs.groundTileType)
                    {
                        case TileSource.TileType.Ground:
                            sb.Draw(animationTexture, destination, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Building:
                            sb.Draw(animationTexture, destination, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:
                            //  sb.Draw(animationTexture, destination, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            var tempRect = new Rectangle(destination.X, destination.Y + 15, destination.Width, destination.Height - 15);
                            float divider = ((float)destination.Height - 15f) / (float)destination.Height; ;
                            float divider2 = (15f) / (float)destination.Height; ;
                            var refR = animationFrames[frameIndex];
                            var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                            sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            break;
                        default:
                            sb.Draw(animationTexture, destination, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }
                }
                else
                {
                    switch (bs.groundTileType)
                    {
                        case TileSource.TileType.Ground:
                            sb.Draw(animationTexture, destination, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Building:
                            sb.Draw(animationTexture, destination, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:

                            if (true)
                            {
                                var tempRect = new Rectangle(destination.X, destination.Y + 15, destination.Width, destination.Height - 15);
                                float divider = (64f - 15f) / 64f; ;
                                float divider2 = (15f) / 64f; ;
                                var refR = animationFramesDirectional[bs.rotationIndex][frameIndex];
                                var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                                sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            }
                            else
                            {

                                var tempRect = new Rectangle(destination.X, destination.Y + 15, destination.Width, destination.Height - 15);
                                float divider = (64f - 15f) / 64f; ;
                                float divider2 = (15f) / 64f; ;
                                var refR = animationFramesDirectional[bs.rotationIndex][frameIndex];
                                var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 1), refR.Width, (int)(refR.Height * divider));
                                sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            }
                            break;
                        default:
                            sb.Draw(animationTexture, destination, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Attempting reload texture!");
                ReloadTexture();
            }

        }

        public void DrawLight(SpriteBatch sb, SpriteLight bs, Vector2 pos, float scale, Color drawColor = default(Color))
        {
            if (drawColor == default(Color))
            {
                drawColor = Color.White * ((float)bs.spriteOpacity / 100f);
            }

            if (animationTexture != null)
            {
                try
                {
                    Vector2 offSet = Vector2.Zero;
                    switch (bs.groundTileType)
                    {
                        case TileSource.TileType.Ground:
                            break;
                        case TileSource.TileType.Building:
                            break;
                        case TileSource.TileType.Fluid:
                            offSet.Y = 15;
                            break;
                        default:
                            break;
                    }
                    sb.Draw(animationTexture, pos + offSet, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, scale, (SpriteEffects)bs.spriteEffect, 0);
                }
                catch
                {

                }


            }
            else
            {
                Console.WriteLine("Attempting reload texture!");
                ReloadTexture();
            }

        }

        public void Draw(SpriteBatch sb, Rectangle destination, float opacity = 1f, SpriteEffects spriteEffect = SpriteEffects.None, Color drawColor = default(Color))
        {

            if (drawColor == default(Color))
            {
                drawColor = Color.White * (opacity);
            }

            if (animationTexture != null)
            {
                if (animationFrames.Count == 0)
                {
                    animationFrames.Add(new Rectangle(0, 0, 64, 64));
                }

                //              if (spriteEffect == SpriteEffects.FlipHorizontally)
                //              {
                //                  adjustedPosition = new Rectangle((int)(destination.X * ResolutionUtility.gameScale.X) +(64 - animationFrames[frameIndex].Width)
                //, (int)(destination.Y * ResolutionUtility.gameScale.Y)
                //, (int)(animationFrames[frameIndex].Width * ResolutionUtility.gameScale.X)
                //   , (int)(animationFrames[frameIndex].Height * ResolutionUtility.gameScale.Y)
                //  );
                //              }
                //adjustedPosition.X += adjustedPosition.Width - animationFrames[frameIndex].Width;
                //   adjustedPosition.Y += adjustedPosition.Height - animationFrames[frameIndex].Height;
                if (animationFrames[frameIndex].Width != 64)
                {

                }
                sb.Draw(animationTexture, destination, animationFrames[frameIndex], drawColor * opacity, 0f, Vector2.Zero, spriteEffect, 0);

            }
            else if (!texFileLoc.Equals(""))
            {
                Console.WriteLine("Attempting reload texture!");
                ReloadTexture();
            }

        }

        public void DrawTile(SpriteBatch sb, Rectangle destination,BasicTile.Rotation rot, float opacity = 1f, SpriteEffects spriteEffect = SpriteEffects.None, Color drawColor = default(Color))
        {

            if (drawColor == default(Color))
            {
                drawColor = Color.White * (opacity);
            }

            if (animationTexture != null)
            {
                if (animationFrames.Count == 0)
                {
                    animationFrames.Add(new Rectangle(0, 0, 64, 64));
                }

                if (animationFrames[frameIndex].Width != 64)
                {

                }

                switch (rot)
                {
                    case BasicTile.Rotation.Zero:
                        sb.Draw(animationTexture, destination, animationFrames[frameIndex], drawColor * opacity, (float)(Math.PI / 2 * 0), Vector2.Zero, spriteEffect, 0);
                        break;
                    case BasicTile.Rotation.Ninety:
                        Rectangle r = new Rectangle(destination.Location + new Point(destination.Width, destination.Height * 0), destination.Size);
                        sb.Draw(animationTexture, r, animationFrames[frameIndex], drawColor * opacity, (float)(Math.PI / 2), Vector2.Zero, spriteEffect, 0);
                        break;
                    default:
                        break;
                }

               // Rectangle r = new Rectangle(destination.Location + new Point(destination.Width,destination.Height*0),destination.Size);
                //sb.Draw(animationTexture, r, animationFrames[frameIndex], drawColor * opacity, (float)(Math.PI/2), Vector2.Zero, spriteEffect, 0);
                //sb.Draw(animationTexture, destination, animationFrames[frameIndex], Color.Red * opacity, (float)(Math.PI / 2*0), Vector2.Zero, spriteEffect, 0);
            }
            else if (!texFileLoc.Equals(""))
            {
                Console.WriteLine("Attempting reload texture!");
                ReloadTexture();
            }

        }

        public void Draw(SpriteBatch sb, BaseSprite bs, Vector2 drawLoc, int ri, Color drawColor = default(Color))
        {
            if (drawColor == default(Color))
            {
                drawColor = Color.White * ((float)bs.spriteOpacity / 100f);
            }

            if (animationTexture != null)
            {
                Rectangle temp = new Rectangle((int)drawLoc.X, (int)drawLoc.Y, bs.spriteGameSize.Width, bs.spriteGameSize.Height);

                if (!bDirectional)
                {

                    sb.Draw(animationTexture, temp, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                }
                else
                {
                    sb.Draw(animationTexture, temp, animationFramesDirectional[ri][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                    bs.spriteGameSize.Width = 64;
                    bs.spriteGameSize.Height = 64;
                }
            }
            else
            {
                Console.WriteLine("Attempting reload texture!");
                ReloadTexture();
            }

        }

        public void DrawFX(SpriteBatch sb, BaseSprite bs, Color drawColor = default(Color))
        {
            if (drawColor == default(Color))
            {
                drawColor = Color.White;
            }

            if (drawColor == Color.White)
            {
                drawColor = Color.White * ((float)bs.spriteOpacity / 100f);
            }
            else
            {
                drawColor = drawColor * ((float)bs.spriteOpacity / 100f); ;
            }

            if (animationTexture != null)
            {
                if (!bDirectional)
                {
                    // sb.Draw(animationTexture, bs.spriteGameSize, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                    // bs.MovementLogic();

                    switch (bs.groundTileType)
                    {
                        case TileSource.TileType.Ground:
                            Rectangle r = bs.spriteGameSize;
                            sb.Draw(animationTexture, null, r, animationFrames[frameIndex], Vector2.Zero, 0f, bs.scaleVector, drawColor, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Building:
                            r = bs.spriteGameSize;
                            sb.Draw(animationTexture, r, animationFrames[frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:
                            r = bs.spriteGameSize;
                            var tempRect = new Rectangle(r.X, bs.spriteGameSize.Y + 15, r.Width, bs.spriteGameSize.Height - 15);
                            float divider = ((float)bs.spriteGameSize.Height - 15f) / (float)bs.spriteGameSize.Height; ;
                            float divider2 = (15f) / (float)bs.spriteGameSize.Height; ;
                            var refR = animationFrames[frameIndex];
                            var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                            sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            break;
                        default:
                            sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }
                }
                else
                {
                    switch (bs.groundTile.tileSource.tileType)
                    {
                        case TileSource.TileType.Ground:
                            sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Building:
                            sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        case TileSource.TileType.Fluid:

                            var tempRect = new Rectangle(bs.spriteGameSize.X, bs.spriteGameSize.Y + 15, bs.spriteGameSize.Width, bs.spriteGameSize.Height - 15);
                            float divider = ((float)bs.spriteGameSize.Height - 15f) / (float)bs.spriteGameSize.Height; ;
                            float divider2 = (15f) / (float)bs.spriteGameSize.Height; ;
                            var refR = animationFramesDirectional[bs.rotationIndex][frameIndex];
                            var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                            sb.Draw(animationTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);

                            break;
                        case TileSource.TileType.Stairs:
                            if (bs.GetType() == typeof(BaseCharacter))
                            {
                                var p = bs.groundTile.mapPosition.Location.ToVector2() - (bs.spriteGameSize.Location.ToVector2() + new Vector2(32, 56));
                                //  p.Y = p.X;
                                //   p.X = 0;
                                // Rectangle r = new Rectangle(bs.spriteGameSize.X,bs.spriteGameSize.Y+(int)p.X-32,bs.spriteGameSize.Width,bs.spriteGameSize.Height);

                                if (bs.groundTile.spriteEffect == SpriteEffects.FlipHorizontally)
                                {
                                    if (bs.rotationIndex == (int)BaseCharacter.Rotation.Right)
                                    {
                                        var diff = 64 + p.X + p.Y - 0;
                                        bs.position += new Vector2(0, diff);

                                    }
                                    else if (bs.rotationIndex == (int)BaseCharacter.Rotation.Left)
                                    {
                                        var diff = 64 + p.X + p.Y + 0;
                                        bs.position += new Vector2(0, diff);
                                        //  bs.position.Y = bs.groundTile.mapPosition.Y+46-
                                    }
                                }


                                if (bs.groundTile.spriteEffect == SpriteEffects.None)
                                {
                                    if (bs.rotationIndex == (int)BaseCharacter.Rotation.Right)
                                    {
                                        var diff = p.Y - p.X + 0;
                                        bs.position += new Vector2(0, diff);
                                    }
                                    else if (bs.rotationIndex == (int)BaseCharacter.Rotation.Left)
                                    {
                                        var diff = p.Y - p.X - 0;
                                        bs.position += new Vector2(0, diff);
                                    }

                                }
                                bs.UpdatePosition();
                                //      sb.Draw(animationTexture, r, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                                sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            }
                            else
                            {
                                sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            }
                            //   sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                        default:
                            sb.Draw(animationTexture, bs.spriteGameSize, animationFramesDirectional[bs.rotationIndex][frameIndex], drawColor, 0f, Vector2.Zero, (SpriteEffects)bs.spriteEffect, 0);
                            break;
                    }

                    //bs.spriteGameSize.Width = 64;
                    //bs.spriteGameSize.Height = 64;
                }
            }
            else
            {
                Console.WriteLine("Attempting reload texture!");
                ReloadTexture();
            }

        }

        public void PreviewDraw(SpriteBatch sb, int ri = -1)
        {
            if (animationTexture != null)
            {
                try
                {
                    if (!bDirectional && animationFrames.Count != 0 || (bDirectional && ri == -1))
                    {
                        sb.Draw(animationTexture, new Rectangle(0, 0, 64, 64), animationFrames[frameIndex], Color.White);
                    }
                    else if (bDirectional && ri != -1 && animationFramesDirectional[ri].Count != 0)
                    {
                        sb.Draw(animationTexture, new Rectangle(0, 0, 64, 64), animationFramesDirectional[ri][frameIndex], Color.White);
                    }
                }
                catch (Exception e)
                {
                    frameIndex = 0;
                    Console.WriteLine("Error at preview draw animation: " + e);
                }

            }
        }

        public void BAnimUpdate(GameTime gameTime, BaseSprite bs)
        {

            if (animationTexture != null && !bPause && !bAnimationFinished)
            {
                elapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedFrameTime > frameInterval)
                {
                    elapsedFrameTime = 0;

                    if (frameIndex != animationFrames.Count - 1)
                    {
                        bAnimationFinished = false;
                        frameIndex++;
                    }
                    else if (!bSimplePlayOnce)
                    {
                        frameIndex = 0;

                    }

                    if (bSimplePlayOnce)
                    {

                    }

                    if (bSimplePlayOnce && frameIndex == animationFrames.Count - 1)
                    {
                        bAnimationFinished = true;
                    }
                }
            }
            if (!bAnimationFinished&&bPause)
            {
                bPause = false;
            }


        }

        public void TestBattleAnimationDraw(SpriteBatch sb)
        {
            if (animationTexture != null)
            {
                try
                {
                    sb.Draw(animationTexture, new Rectangle(0, 0, animationFrames[frameIndex].Width, animationFrames[frameIndex].Height), animationFrames[frameIndex], Color.White);

                }
                catch (Exception)
                {
                    frameIndex = 0;
                }
            }
        }

        public void BattleAnimationDraw(SpriteBatch sb, Vector2 loc, float scale, bool bDrawReverse = false, Color c = default(Color), float opacity = 1.0f)
        {
            if (c == default(Color))
            {
                c = Color.White;
            }

            if (opacity == 0.0f)
            {
                opacity = 0.00001f;
            }
            if (animationTexture != null)
            {
                int shadowPosY = (int)(loc.Y + (animationFrames[frameIndex].Height * scale) - 16 * scale);
                int height = (int)(16 * scale);
                int distanceMod = 45;
                Rectangle shadowBox = new Rectangle((int)(loc.X + distanceMod * scale + bc.battleAnimShadowAdjustLocs.X * scale), (int)(shadowPosY + bc.battleAnimShadowAdjustLocs.Y * scale), (int)(animationFrames[frameIndex].Width * scale - distanceMod * 2 * scale), height);
                sb.Draw(shadowBoxTex, shadowBox, shadowBoxTex.Bounds, Color.Black * 0.85f * opacity * ((float)(c.A / 255f)));

                //     sb.Draw(animationTexture, new Rectangle((int)loc.X, (int)loc.Y, animationFrames[fi].Width, animationFrames[fi].Height), animationFrames[fi], Color.White);
                if (BattleGUI.bDrawFrames)
                {
                    sb.Draw(Game1.hitboxHelp, loc, animationFrames[frameIndex], Color.Blue, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                if (!bDrawReverse)
                {
                    sb.Draw(animationTexture, loc, animationFrames[frameIndex], c * opacity, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else
                {
                    sb.Draw(animationTexture, loc, animationFrames[frameIndex], c * opacity, 0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }

            }
            else
            {
                ReloadTexture();
            }
        }

        public virtual void BattleAnimationDrawWOShadow(SpriteBatch sb, Vector2 loc, float scale, bool bDrawReverse = false, Color c = default(Color))
        {

            if (c == default(Color))
            {
                c = Color.White;
            }
            else if (c.A == 0)
            {

            }


            if (animationTexture != null)
            {
                //  int shadowPosY = (int)(loc.Y + (animationFrames[frameIndex].Height * scale) - 16 * scale);
                //int height = (int)(16 * scale);
                //int distanceMod = 45;

                //     sb.Draw(animationTexture, new Rectangle((int)loc.X, (int)loc.Y, animationFrames[fi].Width, animationFrames[fi].Height), animationFrames[fi], Color.White);
                if (BattleGUI.bDrawFrames)
                {
                    //    sb.Draw(Game1.hitboxHelp, loc, animationFrames[frameIndex], Color.Blue, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                if (!bDrawReverse)
                {
                    sb.Draw(animationTexture, loc, animationFrames[frameIndex], c, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
                }
                else
                {
                    sb.Draw(animationTexture, loc, animationFrames[frameIndex], c, 0f, Vector2.Zero, scale, SpriteEffects.FlipHorizontally, 0);
                }

            }
            else
            {
                ReloadTexture();
            }
        }

        public override string ToString()
        {
            return animationName + " ID: " + animationID;
        }

        public ShapeAnimation Clone()
        {
            ShapeAnimation temp = (ShapeAnimation)this.MemberwiseClone();
            temp.animationFrames = new List<Rectangle>(animationFrames);
            temp.animationFramesDirectional = new List<List<Rectangle>>(animationFramesDirectional);
            return temp;
        }

        public bool NearlyFinished(int length = 3)
        {
            if (length >= animationFrames.Count)
            {
                return true;
            }
            else if (frameIndex + length >= animationFrames.Count)
            {
                return true;
            }
            return false;
        }

        public bool HasProperAnim()
        {
            if (animationFrames.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Point AnimSourceSize()
        {
            return animationFrames[0].Size;
        }

        internal bool HasPassedClimax()
        {
            if (frameIndex >= 4)
            {
                return true;
            }
            return false;
        }

        internal bool OnClimax()
        {
            if (frameIndex == 4)
            {
                return true;
            }
            return false;
        }

        public static ShapeAnimation animFromLuaInfo(LuaShapeAnimationInfo lsai)
        {
            ShapeAnimation temp = new ShapeAnimation();
            temp.animationFrames = new List<Rectangle>(lsai.RectList.MGConvert());
            temp.texFileLoc = lsai.texSource;
            temp.frameInterval = lsai.frameInterval;

            try
            {
                temp.ReloadTexture();
            }
            catch (Exception)
            {
                temp.texFileLoc = "TempTexture";
                temp.ReloadTexture();
                Console.WriteLine("Error loading tex file in 'animFromLuaInfo' from: " + lsai);
            }

            return temp;
        }
    }
}
namespace LUA
{
    public class LuaShapeAnimationInfo
    {
        public LUA.LuaRectangleList RectList = new LUA.LuaRectangleList();
        public String texSource = "";
        public int frameInterval = 250;

        public LuaShapeAnimationInfo() { }
    }
}
