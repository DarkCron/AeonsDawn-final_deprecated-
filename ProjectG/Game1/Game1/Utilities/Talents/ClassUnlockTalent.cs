using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Class Unlock Talent")]
    public class ClassUnlockTalent : BaseTalentSlot
    {
        [XmlElement("Class ID")]
        public int classID = -1;

        internal BaseClass parentClass;

        public ClassUnlockTalent() : base() { }

        public ClassUnlockTalent(Point p, CharacterClassCollection CCC) : base(p, CCC) { }

        public override void Unlock(BaseCharacter bc)
        {
            base.Unlock(bc);
            var cl = parentCCC.charClassList.Find(c => c.classIdentifier == classID);
            if (cl != null)
            {
                cl.bIsUnlocked = true;
            }
        }
    }
}
