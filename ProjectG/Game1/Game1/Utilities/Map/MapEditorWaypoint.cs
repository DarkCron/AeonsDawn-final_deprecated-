using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("Waypoint")]
    public class MapEditorWaypoint
    {
        [XmlElement("Name")]
        public String WPName = "Point";
        [XmlElement("Description")]
        public String WPDescription = "A description";
        [XmlElement("Position")]
        public Vector2 WPPosition = Vector2.Zero;

        public MapEditorWaypoint() { }

        public override string ToString()
        {
            String s = "WP: "+WPName+" @"+WPPosition;
            return s;
        }
    }
}
