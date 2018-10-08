using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Trait")]
    public class BaseTrait
    {
        public enum TraitType { COMBAT_TRAIT = 0, CAMP_TRAIT, STAT_TRAIT, MODIFIER_TRAIT, TEMPORARY_TRAIT }

        [XmlElement("Stat modifier")]
        public STATChart statModifier = new STATChart(true);
        [XmlElement("Trait Type")]
        public int traitType = (int)TraitType.STAT_TRAIT;

        public BaseTrait()
        {

        }
        
        public BaseTrait Clone() {
            var temp = (BaseTrait)this.MemberwiseClone();
            temp.statModifier = this.statModifier.Clone();
            return temp;
        }


    }
}
