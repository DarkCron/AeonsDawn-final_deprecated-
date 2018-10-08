using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace TBAGW
{
    [XmlRoot("Text Element UI")]
    public class TextElement : ActiveUIElement
    {
        public enum DisplayTypes { Text = 0, RemainingTime, TimePassed }

        [XmlElement("Text to display")]
        public String textToDisplay = "Text";
        [XmlElement("Text Colour")]
        public Color textColour = Color.Black;
        [XmlElement("Text Font")]
        public SpriteFont textFont;
        [XmlElement("Text Display Type")]
        public DisplayTypes displayType = DisplayTypes.Text;

        public TextElement() : base()
        {

        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            if (textFont == null)
            {
                textFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if (textFont == null)
            {
                textFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");
            }

            switch (displayType)
            {
                case DisplayTypes.Text:
                    sb.DrawString(textFont, textToDisplay, drawPos, textColour * elementOpacity);
                    break;
                case DisplayTypes.RemainingTime:
                    String temp = "Time remaining:\n";
                    int hours = timer.reportRemainingTime()[2];
                    int minutes = timer.reportRemainingTime()[1];
                    int seconds = timer.reportRemainingTime()[0];
                    if(hours<10)
                    {
                        temp += "0" + hours + ":";
                    }else
                    {
                        temp += hours + ":";
                    }
                    if (minutes < 10)
                    {
                        temp += "0" + minutes + ":";
                    }
                    else
                    {
                        temp += minutes + ":";
                    }
                    if (seconds < 10)
                    {
                        temp += "0" + seconds;
                    }
                    else
                    {
                        temp += seconds;
                    }
                    sb.DrawString(textFont, temp, drawPos, textColour * elementOpacity);
                    break;
                case DisplayTypes.TimePassed:
                    temp = "Time passed:\n";
                    hours = timer.reportTimePassed()[2];
                    minutes = timer.reportTimePassed()[1];
                    seconds = timer.reportTimePassed()[0];
                    if (hours < 10)
                    {
                        temp += "0" + hours + ":";
                    }
                    else
                    {
                        temp += hours + ":";
                    }
                    if (minutes < 10)
                    {
                        temp += "0" + minutes + ":";
                    }
                    else
                    {
                        temp += minutes + ":";
                    }
                    if (seconds < 10)
                    {
                        temp += "0" + seconds;
                    }
                    else
                    {
                        temp += seconds;
                    }
                    sb.DrawString(textFont, temp, drawPos, textColour * elementOpacity);
                    break;
                default:
                    break;
            }

        }
    }
}
