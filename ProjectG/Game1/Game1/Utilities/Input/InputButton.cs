using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TBAGW.Utilities;
using System.Collections.Generic;
using TBAGW.Utilities.Actions;
using TBAGW.Utilities.Input;
using System.Xml.Serialization;

namespace TBAGW.Utilities.Input
{
    [XmlRoot("Input Button")]
    public class InputButton : ScreenButton
    {
        [XmlArray("Key assignments")]
        [XmlArrayItem("Keys")]
        public ActionKey[] whatKeysIsActionAssignedTo = new ActionKey[3];

        [XmlArray("Keyboxes")]
        [XmlArrayItem("Keybox")]
        public Rectangle[] keysBoxes = new Rectangle[3];

        [XmlArray("Original keyboxes")]
        [XmlArrayItem("Original keybox")]
        public Rectangle[] originalKeysBoxes = new Rectangle[3];

        [XmlElement("Action Identifier String")]
        public String actionIndentifierString;

        [XmlIgnore]public SpriteFont controlsFont;

        [XmlArray("Button Names")]
        [XmlArrayItem("Button name")]
        public String[] buttonNames = { " ", " ", " " };

        public InputButton()
        {

        }

        public InputButton(Texture2D buttonTexture, SpriteFont buttonFont, String buttonText, Vector2 position)
            :base(buttonTexture,buttonFont,buttonText,position)
        {

        }

        public void PositionButton(int x, int y)
        {
            position.X = x;
            position.Y = y;

            for (int i = 0; i < 3; i++)
            {
                keysBoxes[i] = new Rectangle(x + 220 + 200 * i, y + 20, (int)controlsFont.MeasureString("  ").X, (int)controlsFont.MeasureString(" ").Y);
                originalKeysBoxes[i] = new Rectangle(x + 220 + 200 * i, y + 20, (int)controlsFont.MeasureString("  ").X, (int)controlsFont.MeasureString(" ").Y);

            }


        }

        public void setIndentifierString(String actionIndentifierString)
        {
            this.actionIndentifierString = actionIndentifierString;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Actions.Actions actions in Game1.actionList)
            {

                if (actionIndentifierString.Equals(actions.actionIndentifierString))
                {
                    this.whatKeysIsActionAssignedTo = actions.whatKeysIsActionAssignedTo;

                    for (int j = 0; j < 3; j++)
                    {
                        if (j != 2)
                        {


                            buttonNames[j] = actions.whatKeysIsActionAssignedTo[j].assignedActionKey.ToString();

                        }
                        else
                        {
                            buttonNames[j] = actions.whatKeysIsActionAssignedTo[j].assignedGamePadButton.ToString();
                        }

                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                keysBoxes[i] = new Rectangle((int)(originalKeysBoxes[i].X),
                    (int)(originalKeysBoxes[i].Y),
                    (int)(controlsFont.MeasureString(buttonNames[i]).X),
                    (int)(controlsFont.MeasureString(buttonNames[i]).Y));
            }

        }


        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);

            int i = 0;
            foreach (ActionKey key in whatKeysIsActionAssignedTo)
            {
                if (i != 2)
                {
                    if (key.bKeyIsAdjustable)
                    {
                        spritebatch.DrawString(controlsFont, key.assignedActionKey.ToString(), new Vector2(keysBoxes[i].X, keysBoxes[i].Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    }
                    else
                    {
                        spritebatch.DrawString(controlsFont, key.assignedActionKey.ToString(), new Vector2(keysBoxes[i].X, keysBoxes[i].Y), Color.Gray, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    }
                }
                else
                {
                    spritebatch.DrawString(controlsFont, key.assignedGamePadButton.ToString(), new Vector2(keysBoxes[i].X, keysBoxes[i].Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
                i++;
            }



        }
    }
}
