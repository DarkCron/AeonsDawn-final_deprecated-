using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Status")]
    public class BasicStatus
    {
        [XmlElement("Stat modifier")]
        public STATChart statModifier = new STATChart(true);

        public BasicStatus()
        {

        }
    }
}
