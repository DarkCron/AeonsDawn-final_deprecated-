using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    [XmlRoot("BaseLight")]
    public class SpriteLight : BaseSprite
    {
        [XmlElement("Light off Animation")]
        public ShapeAnimation lightOffAnim = new ShapeAnimation();
        [XmlElement("LightMask")]
        public ShapeAnimation lightMask = new ShapeAnimation();
        [XmlElement("Default Color")]
        public Color lightColor = default(Color);
        [XmlElement("Is light active")]
        public bool bIsAlwaysOn = true;

        [XmlElement("Light scale")]
        public float lightScale = 1.4f;
        [XmlElement("Light ID")]
        public int lightID = 0;
        [XmlElement("Light time on")]
        public DayLightHandler.TimeBlocksNames timeOn = DayLightHandler.TimeBlocksNames.am19am22;
        [XmlElement("Light time off")]
        public DayLightHandler.TimeBlocksNames timeOff = DayLightHandler.TimeBlocksNames.am4am7;

        [XmlIgnore]
        public bool bIsLightOn = true;
        Rectangle lightZone = new Rectangle();

        static public SpriteLight convertFromBase(BaseSprite bs)
        {
            SpriteLight sl = (SpriteLight)bs;
            sl.spriteGameSize = bs.spriteGameSize;
            sl.baseAnimations = new List<ShapeAnimation>(bs.baseAnimations);
            sl.shapeName = bs.shapeName;
            return sl;
        }

        public SpriteLight Clone()
        {
            SpriteLight temp = new SpriteLight();
            var bs = (BaseSprite)this.ShallowCopy();
            temp = convertFromBase(bs);
            temp.lightMask = lightMask.Clone();
            temp.lightOffAnim = lightOffAnim.Clone();
            return temp;
        }

        public override void ReloadTextures()
        {
            base.ReloadTextures();
            lightOffAnim.ReloadTexture();
            lightMask.ReloadTexture();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            lightMask.UpdateAnimationForItems(gameTime);
        }

        public override void MinimalUpdate(GameTime gt)
        {
            base.MinimalUpdate(gt);
            lightMask.UpdateAnimationForItems(gt);
        }

        public override void Draw(SpriteBatch spriteBatch, Color sColor = default(Color), int index = -1, bool waterDraw = false)
        {
            if (bIsLightOn || !bIsLightOn && lightOffAnim == default(ShapeAnimation))
            {
                base.Draw(spriteBatch, sColor, index, waterDraw);
            }
            else
            {
                lightOffAnim.Draw(spriteBatch, this);
            }

        }

        public void LightUpdate(DayLightHandler.TimeBlocksNames block)
        {
            bIsAlwaysOn = false;
            if (bIsAlwaysOn)
            {
                bIsLightOn = true;
            }
            else
            {
                if (timeOn == block)
                {
                    bIsLightOn = true;
                }
                else if (timeOff == block)
                {
                    bIsLightOn = false;
                }
            }
        }

        public override bool Contains(Rectangle r)
        {
            if (base.Contains(r))
            {
                return true;
            }
            if (lightMask.animationFrames.Count == 0)
            {
                return false;
            }
            if (lightZone == new Rectangle())
            {
                RecalculateLight();
            }
            if(lightZone.Contains(r)||lightZone.Intersects(r))
            {
                return true;
            }


            return false;
        }

        public void RecalculateLight()
        {
            Vector2 pos = spriteGameSize.Center.ToVector2() - (new Vector2(lightMask.animationFrames[0].Width / 2, lightMask.animationFrames[0].Height / 2)) * lightScale;
            lightZone = new Rectangle(pos.ToPoint(), (lightMask.animationFrames[0].Size.ToVector2() * lightScale).ToPoint());
        }

        internal override Rectangle trueMapSize()
        {
            return base.trueMapSize();
        }
    }
}
