using TBAGW;
using TBAGW.Utilities;
using TBAGW.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using System;
using TBAGW.Utilities.OnScreen.Particles;
using System.Xml.Serialization;

namespace TBAGW.Utilities.Input
{
    [XmlRoot("Button")]
    [XmlInclude(typeof(SliderButton))]
    [XmlInclude(typeof(InputButton))]
    public class ScreenButton:Shape
    {


        [XmlElement("Button box")]
        public Rectangle buttonBox = new Rectangle();
        [XmlElement("Button hitbox")]
        public Rectangle buttonBoxHitBox = new Rectangle();
        [XmlElement("ButtonText")]
        public String buttonText;

        [XmlIgnore]
        protected Texture2D buttonTexture;
        [XmlIgnore]
        protected SpriteFont buttonFont;
        [XmlIgnore]
        public bool bButtonSelected = false;

        public ScreenButton(Texture2D buttonTexture,SpriteFont buttonFont, String buttonText, Vector2 position)
            :base(null,1,position,false,new Vector2(0),"A Button",new Rectangle())
        {
            this.buttonTexture = buttonTexture;
            this.buttonFont = buttonFont;
            this.buttonText = buttonText;
            base.position = position;
            AdjustBoxSize();
        }

        public ScreenButton()
        {

        }



        public void AdjustText(String buttonText)
        {
            this.buttonText = buttonText;
            AdjustBoxSize();
        }

        private void AdjustBoxSize()
        {


            if (!(buttonTexture == null))
            {
                buttonBox = new Rectangle((int)(position.X * 1),
(int)(position.Y * 1),
(int)(buttonTexture.Width * 1),
(int)(buttonTexture.Height * 1));
            }
            else if (!(buttonText == null))
            {
                buttonBox = new Rectangle((int)(position.X * 1),
(int)(position.Y * 1),
(int)(this.buttonFont.MeasureString(this.buttonText).X * 1),
(int)(this.buttonFont.MeasureString(this.buttonText).Y * 1));
            }
        }

        public bool ContainsMouse()
        {


                if (buttonBox.Contains(CursorUtility.trueCursorPos))
                {

                    return true;

                }
            



            return false;
        }

        public bool ContainsMouseGUI()
        {
                if (buttonBox.Contains(CursorUtility.GUICursorPos))
                {

                    return true;

                }
            return false;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (!(buttonTexture == null))
            {
                spritebatch.Draw(buttonTexture, buttonBox, Color.White);
            }
            else if (!(buttonText == null))
            {
                spritebatch.DrawString(buttonFont, buttonText, new Vector2(buttonBox.X, buttonBox.Y), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }

            spritebatch.Draw(Game1.hitboxHelp, buttonBoxHitBox, Game1.hitboxHelp.Bounds,Color.Blue);

        }

        public override void Update(GameTime gameTime)
        {
            AdjustBoxSize();


        }

        public Rectangle ButtonBox()
        {
            return buttonBox;
        }

        public virtual void Dispose()
        {
            buttonTexture.Dispose();
        }
    }

}
