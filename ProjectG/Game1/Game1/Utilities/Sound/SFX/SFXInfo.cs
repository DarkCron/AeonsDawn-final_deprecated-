using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("SFX Info")]
    public class SFXInfo
    {
        [XmlElement("Song Location")]
        public String sfxLoc = "";
        [XmlElement("Song identifier")]
        public int sfxID = 0;
        [XmlElement("Song name")]
        public String sfxName = "Default sfx name";

        [XmlIgnore]
        public SoundEffect sfx;

        public SFXInfo()
        {

        }

        public void ReloadContent()
        {
            sfx = Game1.contentManager.Load<SoundEffect>(sfxLoc);
        }

        public override string ToString()
        {
            return sfxName + ", ID: " + sfxID;
        }
    }
}
