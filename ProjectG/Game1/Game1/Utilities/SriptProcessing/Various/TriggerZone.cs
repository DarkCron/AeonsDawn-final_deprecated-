using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    
    public class TriggerZone
    {
        [XmlElement("Trigger Zone ID")]
        public int triggerZoneID = 0;
        [XmlElement("TZ Objects")]
        public List<BaseSprite> TZObjects = new List<BaseSprite>();
        [XmlElement("TZ tiles")]
        public List<BasicTile> TZtiles = new List<BasicTile>();
        [XmlElement("TZ name")]
        public String TZName = "";
        [XmlElement("TZ description")]
        public String TZDescription = "";

        /// <summary>
        /// Is active includes both, updating, drawing and collisions
        /// </summary>
        [XmlIgnore]
        public bool bIsActive = false;
    }
}
