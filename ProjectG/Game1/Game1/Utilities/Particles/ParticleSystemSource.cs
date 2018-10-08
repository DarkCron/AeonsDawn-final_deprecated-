using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities;

namespace TBAGW
{
    [XmlRoot("Particle System")]
    public class ParticleSystemSource
    {
        public enum ParticleStyle { Rain, Explosion, Implosion, Twister }

        /// <summary>
        /// Upward velocity Y
        /// </summary>
        [XmlElement("Gravity")]
        public float gravity = 0f;
        [XmlElement("Wind")]
        public float wind = 0f;
        [XmlElement("Gravity Enabled")]
        public bool bHasGravity = true;
        [XmlElement("Wind Enabled")]
        public bool bHasWind = true;

        [XmlElement("Decay horizontal velocity")]
        public float decayX = 0f;
        /// <summary>
        /// Life time particle in milliseconds
        /// </summary>
        [XmlElement("Life time minimum")]
        public int lifeTimeMin = 500;
        [XmlElement("Life time maximum")]
        public int lifeTimeMax = 5000;

        //Particle Scale
        [XmlElement("Scale Min")]
        public float scaleMin = 1;
        [XmlElement("Scale Max")]
        public float scaleMax = 3;

        [XmlElement("Particle Style")]
        public ParticleStyle particleStyle = ParticleStyle.Rain;

        //Particle Spawn Style
        [XmlElement("Particle Spawn Time Min")]
        public int spawnTimeMin = 100;
        [XmlElement("Particle Spawn Time Max")]
        public int spawnTimeMax = 300;

        [XmlElement("Spawn Position")]
        public Point spawnPosition = new Point(0, 0);
        /// <summary>
        /// Relative from a starting position, area does not apply to explosion or implosion Style
        /// </summary>
        [XmlElement("Spawn Area")]
        public Point spawnArea = new Point(50, 50);
        /// <summary>
        /// Spawn angle to also calculate velocity from, only applies to explosion Styles
        /// </summary>
        /// 
        [XmlElement("Angle Spawn Min")]
        public float spawnAngleMin = 0;
        [XmlElement("Angle Spawn Max")]
        public float spawnAngleMax = (float)(2 * Math.PI);

        //rotation speed particle
        [XmlElement("Rotation Speed Minimum")]
        public float particleRotationSpeedMin = 0f;
        [XmlElement("Rotation Speed Maximum")]
        public float particleRotationSpeedMax = 0f;

        [XmlElement("Particle Texture Source")]
        public String particleTexSource = "";
        [XmlElement("Particle System Base Source")]
        public String particleBaseTexSource = "";

        [XmlElement("Start with random frame?")]
        public bool bRandomFrameStart = false;
        [XmlElement("Fade over time?")]
        public bool bFadeOutOverTime = false;

        //Frames and index
        [XmlElement("Particle animation frames")]
        public List<Rectangle> particleFrames = new List<Rectangle>();
        [XmlElement("Particle base animation frames")]
        public List<Rectangle> particleBaseFrames = new List<Rectangle>();

        //Velocity
        [XmlElement("Particle Minimum velocity")]
        public float particleMinVelocity = 1f;
        [XmlElement("Particle Maximum velocity")]
        public float particleMaxVelocity = 5f;

        [XmlElement("Particle Scale modifier")]
        public float particleScaleModifier = 0f;

        [XmlElement("Particle Frame Timer")]
        public int particleFrameTimer = 120;


        [XmlElement("Base Scale")]
        public float baseScale = 1f;
        [XmlElement("Base Frame Timer")]
        public int baseFrameTimer = 100;
        [XmlElement("Base frame index")]
        public int baseFrameIndex = 0;

        [XmlElement("Min radius for twister")]
        public float radiusMin = 0;
        [XmlElement("Max radius for twister")]
        public float radiusMax = 0;
        [XmlElement("Radius adjuster")]
        public float radiusAdjust = 0f;
        [XmlElement("Twist around")]
        public Vector2 pivot = Vector2.Zero;

        [XmlElement("Particle System Name")]
        public String Name = "Particle System";

        [XmlElement("Particle System Save Location")]
        public String PSlocation = "";

        [XmlElement("Particle System ID")]
        public int identifier = 0;

        [XmlElement("Particle spawn how many")]
        public int particleAmountSpawn = 1;

        [XmlIgnore]
        public Texture2D particleBaseTex;
        [XmlIgnore]
        public Texture2D particleTex;
        [XmlIgnore]
        int nextParticleSpawnsIn = 0;
        [XmlIgnore]
        int baseFrameTimePassed = 0;
        [XmlIgnore]
        int spawnTimer = 0;
        [XmlIgnore]
        List<Particle> livingParticles = new List<Particle>();
        [XmlIgnore]
        List<ParticleSystemSource> containedParticleSystems = new List<ParticleSystemSource>();

        public ParticleSystemSource()
        {

        }

        public void ReloadTextures()
        {
            try
            {
                particleBaseTex = Game1.contentManager.Load<Texture2D>(particleBaseTexSource);
            }
            catch
            {
            }
            try
            {
                particleTex = Game1.contentManager.Load<Texture2D>(particleTexSource);
            }
            catch
            {
            }

            nextParticleSpawnsIn = GamePlayUtility.Randomize(spawnTimeMin, spawnTimeMax);
        }

        public void Update(GameTime gt)
        {
            spawnTimer += gt.ElapsedGameTime.Milliseconds;

            if (spawnTimer > nextParticleSpawnsIn)
            {
                nextParticleSpawnsIn = GamePlayUtility.Randomize(spawnTimeMin, spawnTimeMax);
                spawnTimer = 0;
                for (int i = 0; i < particleAmountSpawn; i++)
                {
                    GenerateParticle();
                }
               
            }

            foreach (var particle in livingParticles)
            {
                particle.Update(gt);
            }

            livingParticles.RemoveAll(p => !p.bIsAlive);

            try
            {
                baseFrameTimePassed += gt.ElapsedGameTime.Milliseconds;
                if (baseFrameTimePassed > baseFrameTimer)
                {
                    baseFrameTimePassed = 0;
                    baseFrameIndex++;
                }

                if (baseFrameIndex > particleBaseFrames.Count - 1)
                {
                    baseFrameIndex = 0;
                }
            }
            catch
            {
            }
        }

        private void GenerateParticle()
        {
            int randomLT = GamePlayUtility.Randomize(lifeTimeMin, lifeTimeMax);
            int randomFrame = 0;
            if (bRandomFrameStart)
            {
                try
                {
                    randomFrame = GamePlayUtility.Randomize(0, particleFrames.Count - 1);
                }
                catch
                {
                }
            }
            float randomRotation = 0f;
            float randomRotationSpeed = 0f;
            randomRotation = GamePlayUtility.ExpertRandomize(spawnAngleMin, spawnAngleMax);
            if(spawnAngleMin==spawnAngleMax) {
                randomRotation = spawnAngleMin;
            }
            if (particleRotationSpeedMax != 0f)
            {
                randomRotation = GamePlayUtility.ExpertRandomize(0, (float)(2 * Math.PI));
                randomRotationSpeed = GamePlayUtility.ExpertRandomize(particleRotationSpeedMin, particleRotationSpeedMax);
            }
            float randomScale = GamePlayUtility.ExpertRandomize(scaleMin, scaleMax);
            float randomVelocity = GamePlayUtility.ExpertRandomize(particleMinVelocity, particleMaxVelocity);
            float randomSpawnRotation = GamePlayUtility.ExpertRandomize(spawnAngleMin, spawnAngleMax);
            int randomX = 0;
            int randomY = 0;
            if (particleStyle == ParticleStyle.Rain || true)
            {
                randomX = GamePlayUtility.Randomize(0, spawnArea.X);
                randomY = GamePlayUtility.Randomize(0, spawnArea.Y);
            }
            Point randomSpawnPoint = new Point(randomX, randomY) + spawnPosition;
            Particle temp = new Particle(randomLT, randomFrame, randomRotation, randomRotationSpeed, randomScale, randomVelocity, randomSpawnRotation, particleStyle, randomSpawnPoint, this);
            switch (particleStyle)
            {
                case ParticleStyle.Rain:
                    break;
                case ParticleStyle.Explosion:
                    break;
                case ParticleStyle.Implosion:
                    break;
                case ParticleStyle.Twister:
                    float randomRadius = GamePlayUtility.ExpertRandomize(radiusMin, radiusMax);
                    float randomCircleX = GamePlayUtility.ExpertRandomize(-radiusMax, radiusMax);
                    float circleY = (float)Math.Sqrt((randomRadius * randomRadius) - (randomCircleX * randomCircleX));
                    temp.TurnParticleInTwister(new Vector2(randomCircleX, circleY), pivot);
                    break;
                default:
                    break;
            }
            livingParticles.Add(temp);
        }

        public void Draw(SpriteBatch sb)
        {

            try
            {
                if (particleBaseTex != null && !particleBaseTexSource.Equals("") && particleBaseFrames.Count >= 1)
                {
                    sb.Draw(particleBaseTex, spawnPosition.ToVector2(), particleBaseFrames[baseFrameIndex], Color.White, 0f, Vector2.Zero, baseScale, SpriteEffects.None, 0);
                }
            }
            catch
            {
            }

            try
            {
                if (particleFrames.Count >= 1)
                {
                    livingParticles.ForEach(particle => sb.Draw(particleTex, particle.position, particleFrames[particle.frameIndex], Color.White * particle.fadePercentage, particle.rotation, particle.center, particle.scale, SpriteEffects.None, 0));
                }
                //foreach (var particle in livingParticles)
                //{
                //    if (particleFrames.Count >= 1)
                //    {
                //        sb.Draw(particleTex, particle.position, particleFrames[particle.frameIndex], Color.White * particle.fadePercentage, particle.rotation, particle.center, particle.scale, SpriteEffects.None, 0);
                //    }
                //}
            }
            catch
            {

            }

        }

        public override string ToString()
        {
            return Name;
        }

        public ParticleSystemSource Clone() {
            ReloadTextures();
            ParticleSystemSource temp = (ParticleSystemSource)this.MemberwiseClone();
            temp.livingParticles = new List<Particle>();
            temp.containedParticleSystems = new List<ParticleSystemSource>();

            return temp;
        }
    }

    public class Particle
    {
        ParticleSystemSource parentSystem = new ParticleSystemSource();
        public int timeAlive = 0;
        public int lifeTime = 0;
        public int frameIndex = 0;
        public float rotation = 0f;
        public float rotationSpeed = 0f;
        public float scale = 1f;
        public bool bIsAlive = true;
        public float maxVelocity = 1f;
        public Vector2 velocity = Vector2.Zero;
        public Vector2 center = Vector2.Zero;
        public ParticleSystemSource.ParticleStyle particleStyle = ParticleSystemSource.ParticleStyle.Rain;
        public Vector2 position = new Vector2();
        int frameTime = 0;
        public float fadePercentage = 1f;

        public Particle(int lifeTime, int frameIndex, float rotation, float rotationSpeed, float scale, float maxVelocity, float spawnRotation, ParticleSystemSource.ParticleStyle particleStyle, Point spawnPoint, ParticleSystemSource system)
        {
            parentSystem = system;
            this.lifeTime = lifeTime;
            this.frameIndex = frameIndex;
            this.rotation = rotation;
            this.scale = scale;
            this.maxVelocity = maxVelocity;
            if (particleStyle != ParticleSystemSource.ParticleStyle.Explosion || particleStyle != ParticleSystemSource.ParticleStyle.Implosion)
            {
                CalculateVelocity(spawnRotation, maxVelocity);
            }
            else if (particleStyle == ParticleSystemSource.ParticleStyle.Rain)
            {
                velocity = new Vector2(0, maxVelocity);
            }
            this.rotationSpeed = rotationSpeed;
            this.particleStyle = particleStyle;
            position = spawnPoint.ToVector2();
        }

        public void TurnParticleInTwister(Vector2 position, Vector2 center)
        {
            velocity = Vector2.Zero;
            particleStyle = ParticleSystemSource.ParticleStyle.Twister;
            this.position = position + new Vector2(150, 150);
            this.center = center;
        }

        private void CalculateVelocity(float spawnRotation, float maxVelocity)
        {
            float angle = spawnRotation * 180 / (float)Math.PI;
            double velocityX = maxVelocity * Math.Sin(spawnRotation);
            double velocityY = maxVelocity * Math.Cos(spawnRotation);
            velocity = new Vector2((float)velocityX, (float)velocityY);
        }

        public void Update(GameTime gt)
        {
            timeAlive += gt.ElapsedGameTime.Milliseconds;
            if (timeAlive > lifeTime)
            {
                bIsAlive = false;
            }



            try
            {
                frameTime += gt.ElapsedGameTime.Milliseconds;
                if (frameTime > parentSystem.particleFrameTimer)
                {
                    frameTime = 0;
                    frameIndex++;
                }

                if (frameIndex > parentSystem.particleFrames.Count - 1)
                {
                    frameIndex = 0;
                }
            }
            catch
            {
            }


            switch (particleStyle)
            {
                case ParticleSystemSource.ParticleStyle.Rain:
                    DropVelocity();
                    break;
                case ParticleSystemSource.ParticleStyle.Explosion:
                    ExplosionVelocity();
                    break;
                case ParticleSystemSource.ParticleStyle.Implosion:
                    break;
                case ParticleSystemSource.ParticleStyle.Twister:
                    TwisterLogic();
                    break;
                default:
                    break;
            }

            if (parentSystem.bHasWind)
            {
                velocity.X += parentSystem.wind;
            }

            if (parentSystem.bHasGravity)
            {
                velocity.Y += parentSystem.gravity;
            }



            position += velocity;
            //rotation = rotationSpeed;
            //rotation += rotationSpeed;
            scale += parentSystem.particleScaleModifier;
            if (scale < 0)
            {
                scale = 0;
            }
            if (parentSystem.bFadeOutOverTime)
            {
                fadePercentage = 1 - (float)((float)timeAlive / (float)lifeTime);
            }
        }

        private void TwisterLogic()
        {
            position = parentSystem.spawnPosition.ToVector2() + parentSystem.pivot * parentSystem.baseScale;
            center = parentSystem.pivot * parentSystem.baseScale;
        }

        private void ExplosionVelocity()
        {
            center = parentSystem.pivot;
            if (parentSystem.gravity != 0f)
            {
                if (velocity.X != 0)
                {

                    if (velocity.X < 0)
                    {
                        //velocity.X += parentSystem.decayX;
                        if (velocity.X > -parentSystem.decayX)
                        {
                            velocity.X = 0;
                        }
                        else
                        {
                            velocity.X += parentSystem.decayX;
                        }
                    }
                    else if (velocity.X > 0)
                    {
                        //velocity.X -= parentSystem.decayX;
                        if (velocity.X < parentSystem.decayX)
                        {
                            velocity.X = 0;
                        }
                        else
                        {
                            velocity.X -= parentSystem.decayX;
                        }
                    }
                }
            }
        }

        private void DropVelocity()
        {
            velocity.X = 0;
            if (velocity.Y < 0)
            {
                velocity.Y = -velocity.Y;
            }
        }
    }
}
