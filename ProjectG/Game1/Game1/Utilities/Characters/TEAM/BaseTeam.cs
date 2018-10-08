using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TBAGW.Utilities.Characters;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("Base Team")]
    public class BaseTeam
    {
        [XmlElement("Team Identifier")]
        public int teamIdentifier = 0;
        [XmlArrayItem("Team members")]
        public List<int> teamMembers = new List<int>();
        [XmlElement("Team name")]
        public String teamName = "";

        public override string ToString()
        {
            return teamName+ " Team: "+teamIdentifier+" #characters: "+teamMembers.Count;
        }
    }
}
