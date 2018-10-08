using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TBAGW.Utilities.SriptProcessing.ScriptTriggers
{
    [XmlRoot("BasicTrigger")]
    public class BasicTrigger
    {
        [XmlElement("ObjectScript")]
        public BaseScript script = new BaseScript();
        [XmlElement("SquareWidth")]
        public int storedTriggerWidth = 64;
        [XmlElement("SquareHeight")]
        public int storedTriggerHeight = 64;
        [XmlElement("TriggerLocations")]
        public List<Rectangle> storedTriggerLocations = new List<Rectangle>();
        [XmlElement("TriggerID")]
        public int triggerID = -1;
        [XmlElement("MapName")]
        public String mapName = "";


        public BasicTrigger()
        {

        }

        public BasicTrigger(int triggerIdentifier, BaseScript script, String mapName = "")
        {
            this.script = script;
            triggerID = triggerIdentifier;
            this.mapName = mapName;
        }

        public bool Contains(Vector2 target)
        {
            bool bContains = false;
            foreach (var item in storedTriggerLocations)
            {
                if (item.Contains(target))
                {
                    bContains = true;
                }
            }
            return bContains;
        }

        public bool Contains(Rectangle target)
        {
            bool bContains = false;
            foreach (var item in storedTriggerLocations)
            {
                if (item.Intersects(target) || item.Contains(target))
                {
                    bContains = true;
                }
            }
            return bContains;
        }
    }
}
