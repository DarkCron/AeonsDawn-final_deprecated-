using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TBAGW.Utilities.Particles
{
    public class ParticleTest : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        Texture2D testTexture;
        List<Rectangle> frames = new List<Rectangle>();
        int frameIndex = 0;

        int timeElapsed = 0;
        int timer = 90;

        public ParticleSystemSource testSystem = new ParticleSystemSource();

        public ParticleTest()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = false;

            //   this.Window.Position = new Point(100, 100);
            //graphics.IsFullScreen = true;
            this.IsMouseVisible = false;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            Window.Title = "Particle testing environment";

          
        }

        private void InitialzeParticleSystem()
        {
            testSystem.bRandomFrameStart = true;
            testSystem.decayX = 0.01f;
            testSystem.gravity = -0.14f;
            testSystem.lifeTimeMax = 5000;
            testSystem.lifeTimeMin = 3000;
            frames.Clear();
            frames.Add(new Rectangle(0, 0, 16, 16));
            frames.Add(new Rectangle(16, 0, 16, 16));
            frames.Add(new Rectangle(32, 0, 16, 16));
            frames.Add(new Rectangle(48, 0, 16, 16));
            testSystem.particleFrames = new List<Rectangle>(frames); ;
            testSystem.particleTexSource = @"Graphics\Particles\Engine\TestPaticle_flame_16x16";
            testSystem.particleBaseTexSource = @"Graphics\Particles\Engine\TestPaticle_16x16-sheet";
            frames.Add(new Rectangle(64, 0, 16, 16));
            testSystem.particleBaseFrames = new List<Rectangle>(frames);
            testSystem.baseScale = 5f;
            testSystem.baseFrameTimer = 90;
            testSystem.particleFrameTimer = 90;
            testSystem.particleMaxVelocity = 3f;
            testSystem.particleMinVelocity = 1f;
            testSystem.particleRotationSpeedMax = (float)Math.PI/180f;
            testSystem.particleRotationSpeedMin = 0f;
            testSystem.particleStyle = ParticleSystemSource.ParticleStyle.Twister;
            testSystem.spawnArea = new Point(30,30);
            testSystem.scaleMax = 2.5f;
            testSystem.scaleMin = 1.5f;
            testSystem.spawnAngleMax =  (float)Math.PI;
            testSystem.spawnAngleMin = 3 / 2 * (float)Math.PI;
            testSystem.spawnPosition = new Point(100, 50);
            testSystem.spawnTimeMin = 10;
            testSystem.spawnTimeMax = 20;
            testSystem.wind = -5f;
            testSystem.bFadeOutOverTime = true;
            testSystem.particleScaleModifier = -0.01f;
            testSystem.bHasGravity = false;
            testSystem.bHasWind = false;
            testSystem.radiusMin = 2f;
            testSystem.radiusMax = 8f;
            testSystem.pivot = new Vector2(8);
            testSystem.ReloadTextures();
        }

        protected override void Initialize()
        {

            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = 300;
            graphics.PreferredBackBufferHeight = 300;
            //   this.Window.Position = new Point(100, 100);
            //graphics.IsFullScreen = true;
            this.IsMouseVisible = false;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            base.Initialize();
            InitialzeParticleSystem();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            testTexture = Content.Load<Texture2D>(@"Graphics\Particles\Engine\TestPaticle_flame_16x16");
            frames.Clear();
            frames.Add(new Rectangle(0, 0, 16, 16));
            frames.Add(new Rectangle(16, 0, 16, 16));
            frames.Add(new Rectangle(32, 0, 16, 16));
            frames.Add(new Rectangle(48, 0, 16, 16));
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.Milliseconds;
            if (timeElapsed > timer)
            {
                timeElapsed = 0;
                frameIndex++;
            }

            if (frameIndex > frames.Count - 1)
            {
                frameIndex = 0;
            }

            testSystem.Update(gameTime);

            testSystem.spawnPosition = Mouse.GetState().Position;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //base.Draw(gameTime);
            graphics.GraphicsDevice.Clear(Color.Gray);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            // spriteBatch.Draw(testTexture,new Rectangle(30,30,100,100),frames[frameIndex], Color.White);
            //Do not try to load in external textures like this
            // spriteBatch.Draw(Game1.hitboxHelp, new Rectangle(50, 75, 250, 523), Color.Bisque);
            testSystem.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
