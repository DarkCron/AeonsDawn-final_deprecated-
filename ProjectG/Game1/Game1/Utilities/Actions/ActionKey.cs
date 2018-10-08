using TBAGW;
using TBAGW.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using System;
using System.Xml.Serialization;

namespace TBAGW.Utilities.Actions
{
    [XmlRoot("Action key")]
    public class ActionKey
    {
        [XmlElement("Is key assigned")]
        public bool bKeyIsAssigned = false;
        [XmlElement("Is button assigned")]
        public bool bButtonIsAssigned = false;
        [XmlElement("Is key Adjustable")]
        public bool bKeyIsAdjustable = true;
        [XmlElement("Is GamePad Key")]
        public bool bKeyIsGamePadKey = false;

        [XmlElement("Key Value")]
        public Keys assignedActionKey = new Keys();

        [XmlElement("Button Value")]
        public Buttons assignedGamePadButton = new Buttons();

        [XmlElement("Action Indentiefier")]
        public String actionIndentifierString = "";

        [XmlElement("Column value")]
        public int column = -1;




        public ActionKey()
        {

        }

        public void Initialize(String actionIndentifierString, int column)
        {
            this.actionIndentifierString = actionIndentifierString;
            this.column = column;
        }

        public void assignKey(Keys key, String actionIndentifierString, int column, bool bIsDefault)
        {
            if (bKeyIsAdjustable && this.actionIndentifierString.Equals(actionIndentifierString) && this.column == column)
            {
                assignedActionKey = key;
                bKeyIsAssigned = true;
            }
        }

        public void assignUnadjustableKey(Keys key, String actionIndentifierString, int column, bool bIsDefault)
        {
            if (this.actionIndentifierString.Equals(actionIndentifierString) && this.column == column)
            {
                bKeyIsAdjustable = true;
                assignedActionKey = key;
                bKeyIsAssigned = true;
            }
        }

        public Keys identifyKey(String actionIndentifierString)
        {
            if (this.actionIndentifierString.Equals(actionIndentifierString) && bKeyIsAssigned)
            {
                return assignedActionKey;
            }

            return Keys.None;
        }

        public Buttons identifyButton(String actionIndentifierString)
        {
            if (this.actionIndentifierString.Equals(actionIndentifierString) && bButtonIsAssigned)
            {
                return assignedGamePadButton;
            }

            return (Buttons.BigButton);
        }

        public void assignButton(Buttons button, String actionIndentifierString, bool bIsDefault)
        {
            if (this.actionIndentifierString.Equals(actionIndentifierString) && bKeyIsGamePadKey && this.column == 2)
            {
                assignedGamePadButton = button;
                bButtonIsAssigned = true;
            }
        }

        public override string ToString()
        {
            return actionIndentifierString + " , "+assignedActionKey;
        }
    }
}
