using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Effect")]
    public class BaseEffect //An effect last only in battle, a status is persistent until healed.
    {
        [XmlElement("Stat modifier")]
        public STATChart statModifier = new STATChart(true);

        public BaseEffect()
        {

        }
    }
}
