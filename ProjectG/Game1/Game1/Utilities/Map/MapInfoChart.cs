using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace TBAGW
{
    [XmlRoot("Map Info Chart")]
    public class MapInfoChart
    {
        [XmlElement("map ID")]
        public int mapId = 0;
        [XmlElement("map name")]
        public String mapName = "Map";
        [XmlElement("map location")]
        public String mapLocation = "";
        [XmlArrayItem("subMap ID's")]
        public List<int> subMapIDs = new List<int>();


    }
}
