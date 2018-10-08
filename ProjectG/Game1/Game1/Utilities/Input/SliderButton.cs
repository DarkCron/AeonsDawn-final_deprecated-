using TBAGW;
using TBAGW.Utilities;
using TBAGW.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using System;
using System.Xml.Serialization;

namespace TBAGW.Utilities.Input
{
    [XmlRoot("Slider Button")]
    public class SliderButton : ScreenButton
    {
        [XmlElement("Slider Button Box")]
        public Rectangle sliderButtonBox = new Rectangle();

        [XmlIgnore]Texture2D sliderButtonTexture;

        [XmlElement("Slider Value")]
        public int sliderValue = 0;

        public SliderButton()
        {

        }

        public SliderButton(Texture2D buttonTexture, SpriteFont buttonFont, String buttonText, Texture2D sliderButtonTexture, Vector2 position)
            :base(buttonTexture,buttonFont,buttonText,position)
        {
            this.sliderButtonTexture = sliderButtonTexture;
            sliderButtonBox = new Rectangle(0, 0, sliderButtonTexture.Width, sliderButtonTexture.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            sliderButtonBox = new Rectangle((int)(base.buttonBox.X) + (int)(-sliderButtonBox.Width / 2 + (base.buttonBox.Width * sliderValue) / (100)), (int)(base.buttonBox.Y), (int)(sliderButtonTexture.Width * 1), (int)(sliderButtonTexture.Height * 1));

        }

        public int Slide(int whatToChange)
        {
            if (buttonBox.Contains(CursorUtility.GUICursorPos))
            {
                if (((int)Math.Ceiling((CursorUtility.GUICursorPos.X * 1 - ButtonBox().X) / ButtonBox().Width * 100)) > 0 && ((int)Math.Ceiling((CursorUtility.GUICursorPos.X * 1 - ButtonBox().X) / ButtonBox().Width * 100)) < 100)
                {
                    whatToChange = (int)Math.Ceiling((CursorUtility.GUICursorPos.X * 1 - ButtonBox().X) / ButtonBox().Width * 100);
                }
                else if (((int)Math.Ceiling((CursorUtility.GUICursorPos.X * 1 - ButtonBox().X) / ButtonBox().Width * 100)) > 100)
                {
                    whatToChange = 100;
                }
                else if (((int)Math.Ceiling((CursorUtility.GUICursorPos.X * 1 - ButtonBox().X) / ButtonBox().Width * 100)) < 0)
                {
                    whatToChange = 0;
                }
            }

            if(whatToChange ==99){
                whatToChange = 100;
            }else if(whatToChange == 1){
                whatToChange = 0;
            }

            return whatToChange;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            spritebatch.Draw(sliderButtonTexture, sliderButtonBox, Color.White);

        }

        public override void Dispose()
        {
            base.Dispose();
            sliderButtonTexture.Dispose();
        }
    }
}
