using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Utilities
{
    [XmlRoot("GlobalGameData")]
    public class GlobalGameData
    {
        [XmlElement("")]
        List<ScriptBool> globalSBs = new List<ScriptBool>();

        public GlobalGameData()
        {

        }
    }
}
