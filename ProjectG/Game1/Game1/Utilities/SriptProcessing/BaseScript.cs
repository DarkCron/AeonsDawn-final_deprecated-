using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TBAGW.Utilities.SriptProcessing
{
    [XmlRoot("BaseScript")]
    public class BaseScript
    {
        [XmlElement("ScriptIdentifier")]
        public int identifier = -1;
        [XmlElement("ScriptContent")]
        public List<String> scriptContent = new List<string>();
        [XmlElement("ScriptLineIndex")]
        public int scriptLineIndex  = 0;
        [XmlElement("RepeatLineNumber")]
        public int repeatLine = 0;

        [XmlIgnore]
        public String type = "";
        [XmlIgnore]
        public int timer = -1;
        [XmlIgnore]
        public int passedTime = 0;
        [XmlIgnore]
        public bool bTimerPassed = false;
        [XmlIgnore]
        public bool bReachedEnd = false;

        public BaseScript()
        {

        }

        public void Update(GameTime gt)
        {
            if (timer!=-1)
            {
                if (passedTime ==0)
                {
                    bTimerPassed = false;
                }

                passedTime = gt.ElapsedGameTime.Milliseconds;

                if (passedTime>timer)
                {
                    bTimerPassed = true;
                    passedTime = 0;
                }
            }

            if (scriptContent.Count-1==scriptLineIndex||scriptLineIndex==repeatLine)
            {
                bReachedEnd = true;
                Console.WriteLine("Reading done!");
            }
        }

        public BaseScript(int identifier)
        {
            this.identifier = identifier;
        }

        public override string ToString()
        {
            String toString = "SID: "+identifier+"; Index: "+scriptLineIndex;
            if (!type.Equals(""))
            {
                toString += "; @: " + type;
            }
            return toString;
        }

        public BaseScript Clone()
        {
            BaseScript s = (BaseScript)this.MemberwiseClone();
            s.scriptContent = new List<string>(scriptContent);
            return s;
        }
    }
}
