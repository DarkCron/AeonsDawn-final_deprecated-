using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Sprite;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    public class ParticleAnimation : ShapeAnimation
    {
        [XmlElement("PA ID")]
        public int particleAnimationID = 0;
        [XmlElement("PA Name")]
        public String particleAnimationName = "";
        [XmlElement("MC ID")]
        public int magicCircleID = -1;
        [XmlElement("MC Timer")]
        public int magicCircleTimer = 50;
        [XmlElement("MC Vertical OffSet")]
        public int magicCircleVOS = 100;
        [XmlElement("Summon Colour")]
        public Color magicCircleColor = Color.White;
        [XmlElement("Length time")]
        public int lengthTime = 1000;
        [XmlElement("Build up Time")]
        public int buildUp = 0;
        [XmlElement("Build Off Time")]
        public int buildOff = 0;
        [XmlElement("Circle opacity")]
        public float circleOpacity = 1.0f;
        [XmlElement("Main PA relative scale")]
        public float scalePA = 1.0f;
        [XmlElement("Main PA relative scale offset")]
        public Vector2 scalePAOS = Vector2.Zero;

        [XmlIgnore]
        BaseCharacter refCharForAnim = null;
        [XmlIgnore]
        public MagicCircleBase mcb;
        [XmlIgnore]
        bool startMainAnimation = false;
        [XmlIgnore]
        int timePassedComplete = 0;



        enum animSTATE { buildUp, main, buildOff }
        animSTATE currentState = animSTATE.buildUp;

        BaseCharacter.CharacterBattleAnimations supposedAnim = BaseCharacter.CharacterBattleAnimations.Hurt;
        BaseCharacter.CharacterBattleAnimations supposedNextAnim = BaseCharacter.CharacterBattleAnimations.Hurt;

        internal void Reset()
        {
            timePassedComplete = 0;
            startMainAnimation = false;
            bAnimationFinished = false;
            elapsedFrameTime = 0;
            frameIndex = 0;
            currentState = animSTATE.buildUp;
        }

        public void GenerateLogic(BaseCharacter bc, BaseCharacter.CharacterBattleAnimations supposedAnim, BaseCharacter.CharacterBattleAnimations supposedNextAnim)
        {
            refCharForAnim = bc;
            this.supposedAnim = supposedAnim;
            this.supposedNextAnim = supposedNextAnim;
        }

        public void GenerateFrameIntervals()
        {

            if (mcb != null)
            {
                mcb.frameInterval = lengthTime / mcb.animationFrames.Count;
            }

            int lengthForAnimation = lengthTime - buildUp - buildOff;
            int t = animationFrames.Count;
            if (t == 0) { t = 1; }
            frameInterval = lengthForAnimation / t;
        }

        public override string ToString()
        {
            return particleAnimationName + " ID: " + particleAnimationID;
        }

        public void ReloadGCDB(GameContentDataBase gcdb)
        {
            if (magicCircleID == -1)
            {
                if (gcdb.gameMagicCircleAnimations.Count != 0)
                {
                    magicCircleID = gcdb.gameMagicCircleAnimations[0].magicCircleBaseID;
                    ReloadGCDB(gcdb);
                }
            }
            else
            {
                mcb = gcdb.gameMagicCircleAnimations.Find(cb => cb.magicCircleBaseID == magicCircleID).Clone();
                mcb.frameInterval = magicCircleTimer;
            }
        }

        public override void UpdateAnimationForItems(GameTime gameTime)
        {
            if (!bAnimationFinished && this.particleAnimationID == 0 && this.particleAnimationName.Equals(""))
            {
                bAnimationFinished = true;
                Console.WriteLine("Ending default PA, soft error");
            }
            timePassedComplete += gameTime.ElapsedGameTime.Milliseconds;
            if (timePassedComplete < buildUp && !bAnimationFinished)
            {
                currentState = animSTATE.buildUp;

                circleOpacity = (float)timePassedComplete / (float)buildUp;
                if (circleOpacity < 0)
                {
                    circleOpacity = 0f;
                }
                else if (circleOpacity > 1)
                {
                    circleOpacity = 1.0f;
                }
            }
            if (!startMainAnimation && currentState != animSTATE.buildOff && timePassedComplete >= buildUp && timePassedComplete < lengthTime - buildOff && !bAnimationFinished)
            {
                currentState = animSTATE.main;

                startMainAnimation = true;
                elapsedFrameTime = 0;
                frameIndex = 0;
                circleOpacity = 1.0f;
                bSimplePlayOnce = true;
            }
            if (timePassedComplete >= lengthTime - buildOff - 400 && !bAnimationFinished)
            {
                currentState = animSTATE.buildOff;

                startMainAnimation = false;
                circleOpacity = (float)((lengthTime - timePassedComplete)) / (float)buildOff;
                if (circleOpacity < 0)
                {
                    circleOpacity = 0f;
                }
                else if (circleOpacity > 1)
                {
                    circleOpacity = 1.0f;
                }
            }

            if (timePassedComplete > lengthTime)
            {
                timePassedComplete = 0;
                if (mcb != null)
                {
                    mcb.frameIndex = 0;
                    mcb.elapsedFrameTime = 0;
                    circleOpacity = 0f;

                }
                if (false)//repeat
                {
                    bAnimationFinished = false;
                }

                frameIndex = 0;
                currentState = animSTATE.buildUp;
            }

            if (circleOpacity < 0)
            {
                circleOpacity = 0f;
            }
            else if (circleOpacity > 1)
            {
                circleOpacity = 1.0f;
            }

            if (bAnimationFinished)
            {

            }

            if (startMainAnimation || (currentState == animSTATE.buildOff && !bAnimationFinished))
            {

                base.UpdateAnimationForItems(gameTime);
            }

            if (mcb != null)
            {
                mcb.frameInterval = 45;
                mcb.UpdateAnimationForItems(gameTime);
            }




            if (refCharForAnim != null && !bAnimationFinished)
            {
                switch (currentState)
                {
                    case animSTATE.buildUp:
                        if (refCharForAnim.animationBattleIndex != (int)BaseCharacter.CharacterBattleAnimations.Idle)
                        {
                            refCharForAnim.ChangeBattleAnimation((int)BaseCharacter.CharacterBattleAnimations.Idle);
                        }

                        break;
                    case animSTATE.main:
                        if (refCharForAnim.animationBattleIndex != (int)supposedAnim)
                        {
                            refCharForAnim.ChangeBattleAnimation((int)supposedAnim);
                            refCharForAnim.currentBattleAnimation().bSimplePlayOnce = true;
                        }
                        else
                        {
                            var refer = refCharForAnim.currentBattleAnimation();
                            if (!NearlyFinished(5))
                            {
                                if (refCharForAnim.currentBattleAnimation().frameIndex >= refCharForAnim.climaxHurtFrame)
                                {
                                    refCharForAnim.currentBattleAnimation().bPause = true;
                                }
                            }
                            else
                            {
                                currentState = animSTATE.buildOff;
                            }

                        }
                        break;
                    case animSTATE.buildOff:

                        if (refCharForAnim.animationBattleIndex != (int)supposedNextAnim)
                        {
                            if (refCharForAnim.currentBattleAnimation().bAnimationFinished || refCharForAnim.currentBattleAnimation().NearlyFinished(4))
                            {
                                refCharForAnim.ChangeBattleAnimation((int)supposedNextAnim);
                            }
                            else if (refCharForAnim.currentBattleAnimation().bPause)
                            {
                                refCharForAnim.currentBattleAnimation().bPause = false;
                            }
                        }

                        break;
                    default:
                        break;
                }
            }
            else if (bAnimationFinished && refCharForAnim!=null)
            {
                if (refCharForAnim.animationBattleIndex != (int)supposedNextAnim)
                {
                    if (refCharForAnim.currentBattleAnimation().bAnimationFinished)
                    {
                        refCharForAnim.ChangeBattleAnimation((int)supposedNextAnim);
                    }
                    else if (refCharForAnim.currentBattleAnimation().bPause)
                    {
                        refCharForAnim.currentBattleAnimation().bPause = false;
                    }
                }

                if (refCharForAnim.currentBattleAnimation().bPause)
                {
                    refCharForAnim.currentBattleAnimation().bPause = false;
                }
            }

        }

        internal void AssignRefCharSafe(BaseCharacter bc)
        {
            if (refCharForAnim == null || refCharForAnim != bc)
            {
                refCharForAnim = bc;
            }
        }

        public bool HasProperCharacterLogic()
        {
            if (refCharForAnim != null)
            {
                return true;
            }
            return false;
        }

        public void externalUpdate(GameTime gt)
        {
            refCharForAnim.currentBattleAnimation().BAnimUpdate(gt, refCharForAnim);
        }

        public override void BattleAnimationDrawWOShadow(SpriteBatch sb, Vector2 loc, float scale, bool bDrawReverse = false, Color c = default(Color))
        {
            if (mcb != null)
            {
                Color temp = magicCircleColor * circleOpacity;
                //Console.WriteLine(temp.A);
                if (circleOpacity == 0)
                {
                    temp.A = 255;
                }
                mcb.BattleAnimationDrawWOShadow(sb, loc + new Vector2(0, magicCircleVOS) * scale, scale, false, temp);
            }

            if (refCharForAnim != null && HasProperAnim() && refCharForAnim.currentBattleAnimation().HasProperAnim())
            {
                var test = (timePassedComplete % 7 == 0) ? 1.0f : 0f;
                var animColour = Color.White;

                switch (currentState)
                {
                    case animSTATE.buildUp:
                        test = 1.0f;
                        break;
                    case animSTATE.main:
                        animColour = Color.LightGoldenrodYellow;
                        break;
                    case animSTATE.buildOff:
                        test = 1.0f;
                        break;
                    default:
                        break;
                }

                var temp = AnimSourceSize().ToVector2() * scale - refCharForAnim.currentBattleAnimation().AnimSourceSize().ToVector2() * scale;
                var tempRect = new Rectangle(((temp / 2)).ToPoint() + new Point(0, (int)(magicCircleVOS * scale) + 70), (refCharForAnim.currentBattleAnimation().AnimSourceSize().ToVector2() * scale).ToPoint());
                var sEffect = !bDrawReverse ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                refCharForAnim.currentBattleAnimation().Draw(sb, tempRect, test, sEffect, animColour);
            }

            if (startMainAnimation || (currentState == animSTATE.buildOff && !bAnimationFinished))
            {
                var delta = 0.0f;
                if (scalePA > 1)
                {
                    delta = scalePA - 1.0f;
                }
                else if (scalePA < 1)
                {
                    delta = -(1.0f - scalePA);
                }
                base.BattleAnimationDrawWOShadow(sb, loc + scalePAOS * (scalePA + delta), scale + delta, false, c);
            }

        }

        public void BattleAnimationDrawWOShadowTrue(SpriteBatch sb, Vector2 loc, float scale, Vector2 charLoc, float charScale, bool bDrawReverse = false, Color c = default(Color))
        {
            if (mcb != null && !bAnimationFinished)
            {
                Color temp = magicCircleColor * circleOpacity;
                //Console.WriteLine(temp.A);
                if (circleOpacity == 0)
                {
                    //temp = magicCircleColor;
                    temp.R = (byte)(magicCircleColor.R * 0.1f);
                    temp.G = (byte)(magicCircleColor.G * 0.1f);
                    temp.B = (byte)(magicCircleColor.B * 0.1f);
                    temp.A = 0;
                }
                mcb.BattleAnimationDrawWOShadow(sb, loc + new Vector2(0, magicCircleVOS) * scale, scale, false, temp);
            }

            if ( HasProperAnim())// && refCharForAnim.currentBattleAnimation().HasProperAnim())
            {
                var test = (timePassedComplete % 24 == 0 || timePassedComplete % 3 == 0) ? 1.0f : 0f;
                var animColour = Color.White;

                switch (currentState)
                {
                    case animSTATE.buildUp:
                        test = 1.0f;
                        break;
                    case animSTATE.main:
                        animColour = Color.PaleGoldenrod;
                        break;
                    case animSTATE.buildOff:
                        test = 1.0f;
                        break;
                    default:
                        break;
                }
                if (test == 1.0f)
                {

                }

                //var temp = AnimSourceSize().ToVector2() * scale - refCharForAnim.currentBattleAnimation().AnimSourceSize().ToVector2() * scale;
                // var tempRect = new Rectangle(((temp / 2)).ToPoint() + new Point(0, (int)(magicCircleVOS * scale) + 70), (refCharForAnim.currentBattleAnimation().AnimSourceSize().ToVector2() * scale).ToPoint());
                //  var sEffect = !bDrawReverse ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                if (refCharForAnim!=null)
                {
                    refCharForAnim.currentBattleAnimation().BattleAnimationDraw(sb, charLoc, charScale, bDrawReverse, animColour, test);
                }
            }

            if ((startMainAnimation && !bAnimationFinished) || (currentState == animSTATE.buildOff && !bAnimationFinished))
            {
                var delta = 0.0f;
                if (scalePA > 1)
                {
                    delta = scalePA - 1.0f;
                }
                else if (scalePA < 1)
                {
                    delta = -(1.0f - scalePA);
                }
                base.BattleAnimationDrawWOShadow(sb, loc + scalePAOS * (scalePA + delta), scale + delta, false, c);
            }

        }

        public ParticleAnimation Clone()
        {
            var temp = base.Clone() as ParticleAnimation;
            temp.particleAnimationID = particleAnimationID;
            temp.particleAnimationName = particleAnimationName;
            if (mcb != null)
            {
                temp.mcb = mcb.Clone();
            }
            return temp;
        }
    }

    public class MagicCircleBase : ShapeAnimation
    {
        [XmlElement("MC ID")]
        public int magicCircleBaseID = 0;
        [XmlElement("MC Name")]
        public String magicCircleBaseName = "";


        public override string ToString()
        {
            return magicCircleBaseName + " ID: " + magicCircleBaseID;
        }

        public MagicCircleBase Clone()
        {
            var temp = base.Clone() as MagicCircleBase;
            temp.magicCircleBaseID = magicCircleBaseID;
            temp.magicCircleBaseName = magicCircleBaseName;
            return temp;
        }
    }
}
