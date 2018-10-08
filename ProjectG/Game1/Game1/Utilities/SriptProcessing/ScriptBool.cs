using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Utilities.SriptProcessing
{
    [XmlRoot("ScriptBool")]
    public class ScriptBool
    {
        [XmlElement("Switched On")]
        public bool isOn = false;
        [XmlElement("Is global")]
        public bool isGlobal = false;
        [XmlElement("Bool")]
        public bool scriptBool = false;
        [XmlElement("Identifier")]
        public int boolID = -1;
        [XmlElement("Choice")]
        public int scriptChoice = -1;
        [XmlElement("boolName")]
        public String boolName = "default name";
        [XmlElement("boolDescription")]
        public String boolDescription = "default description";
        [XmlArrayItem("choiceText")]
        public List<String> choiceText = new List<string>();
        [XmlArrayItem("choiceDescription")]
        public List<String> choiceDescription = new List<string>();

        public ScriptBool()
        {

        }

        public bool IsEnabled()
        {
            if (isOn)
            {
                return true;
            }
            return false;
        }

        internal List<String> choices()
        {
            var temp = new List<String>();
            int index = 0;
            foreach (var item in choiceText)
            {
                String text = "";
                text += item + ", ID:" + index +" ; "+ choiceDescription[index];
                temp.Add(text);
                index++;
            }

            return temp;
        }

        public override string ToString()
        {
            String answer = scriptBool ? "YES" : "NO";
            return boolName + " ID: " + boolID + " ,activated by default? " + answer;
        }

        public ScriptBool Clone()
        {
            ScriptBool temp = (ScriptBool)this.MemberwiseClone();
            return temp;
        }
    }

    [XmlRoot("Script Bool Check")]
    public class ScriptBoolCheck
    {
        public enum CheckType { Bool, Choice }

        [XmlElement("Script ID")]
        public int boolID = -1;
        [XmlElement("Check type")]
        public CheckType checkType = CheckType.Bool;
        [XmlElement("Bool check Requirement")]
        public bool bSameAsSBisOn = false;
        [XmlElement("Bool Activate Choices")]
        public List<int> choices = new List<int>();

        public ScriptBoolCheck()
        {

        }

        public bool CanBeActivated()
        {
            if (boolID == -1)
            {
                return true;
            }
            var test = PlayerSaveData.getBool(boolID);
            if (test == null)
            {
                return false;
            }
            else
            {
                switch (checkType)
                {
                    case CheckType.Bool:
                        return (test.isOn == bSameAsSBisOn);
                    case CheckType.Choice:
                        return choices.Contains(test.scriptChoice);
                    default:
                        break;
                }
            }
            return false;
        }

        public static implicit operator ScriptBoolCheck(String s)
        {
            ScriptBoolCheck temp = null;
            List<String> args = null;
            try
            {
                args = s.Split(',').ToList();
                args.RemoveAll(l => l.Equals("", StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                args = null;
            }
            if (args != null)
            {
                temp = new ScriptBoolCheck();
                temp.boolID = int.Parse(args[0]);
                temp.checkType = (CheckType)Enum.GetNames(typeof(CheckType)).ToList().IndexOf(args[1]);
                temp.bSameAsSBisOn = bool.Parse(args[2]);

                for (int i = 0; i < args.Count - 3; i++)
                {
                    temp.choices.Add(int.Parse(args[i + 3]));
                }

            }


            return temp;
        }

        internal String toCastString()
        {
            String s = "";

            s += boolID.ToString() + ",";
            s += checkType.ToString() + ",";
            s += bSameAsSBisOn.ToString();
            for (int i = 0; i < choices.Count; i++)
            {
                s += "," + choices[i];
            }

            return s;
        }
    }
}

namespace LUA
{
    public class LuaScriptBool
    {
        internal ScriptBool parent;

        public String name;
        public int id;
        public bool bActivate = false;
        public int choice = -1;

        public LuaScriptBool(String name, int id)
        {
            this.name = name;
            this.id = id;
        }

        public void SetData(bool bActivated, int choice = -1)
        {
            this.bActivate = bActivated;
            this.choice = choice;
        }

        public void Activate()
        {
            var temp = TBAGW.GameProcessor.gcDB.gameScriptBools.Find(sb => sb.boolID == id && sb.boolName.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (temp == default(ScriptBool))
            {
                temp.isOn = bActivate;
                temp.scriptChoice = choice;
            }
            else
            {
                Console.WriteLine("Soft error, given script bool doesn't exist: " + name + ", ID:" + id);
            }
        }
    }
}
