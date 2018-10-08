using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("Character Specializations")]
    public class CharSpecialization
    {
        [XmlElement("Name")]
        public String name = "spec";
        [XmlElement("Lua loc")]
        public String luaLoc = "";
        [XmlElement("Spec level")]
        public int level = 0;


        internal NLua.Lua luaInfo = new NLua.Lua();

        public CharSpecialization() { }

        public bool IsVisible()
        {
            return true;
        }

        public bool CanLevelUp()
        {
            return false;
        }

    }
}
