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
    [XmlRoot("Base Talent Slot")]
    [XmlInclude(typeof(ClassUnlockTalent))]
    public abstract class BaseTalentSlot
    {
        public enum TalentType { None, ClassUnlock, AbilityUnlock, CharStatMod }

        [XmlElement("Identifier")]
        public int ID = 0;
        [XmlElement("Is Unlocked")]
        public bool bUnlocked = false;
        [XmlElement("Is Universal")]
        public bool bIsUniversal = false;
        [XmlElement("Talent Type")]
        public TalentType talentType = TalentType.None;
        [XmlElement("Classpoint req")]
        public List<ClassPoints> cpReq = new List<ClassPoints>();
        [XmlElement("Required Talents")]
        public List<int> requiredTalentIDs = new List<int>();
        [XmlElement("Talent Node")]
        public TalentNode talentNode = new TalentNode();
        [XmlElement("Node name")]
        public GameText name = new GameText("");
        [XmlElement("Node description")]
        public GameText description = new GameText("");

        internal bool bCanBeUnlocked = false;
        internal List<BaseTalentSlot> requiredTalents = new List<BaseTalentSlot>();
        internal CharacterClassCollection parentCCC;

        public BaseTalentSlot() { }

        public BaseTalentSlot(Point p, CharacterClassCollection CCC)
        {
            talentNode = new TalentNode(p);
            talentNode.parent = this;
            parentCCC = CCC;
            ID = CCC.LatestID;
            CCC.LatestID++;
        }

        virtual public void Reload(CharacterClassCollection CCC, BaseCharacter bc)
        {
            foreach (var item in cpReq)
            {
                item.Reload(GameProcessor.gcDB);
            }
            requiredTalents = new List<BaseTalentSlot>();
            foreach (var item in requiredTalentIDs)
            {
                requiredTalents.Add(CCC.actualTalentSlots.Find(t => t.ID == item));
            }
            requiredTalents.RemoveAll(t => t == null);
            parentCCC = CCC;
            talentNode.parent = this;

            if (bUnlocked)
            {
                Unlock(bc);
            }
        }

        virtual public bool CanUnlock(BaseCharacter bc)
        {
            if (bUnlocked)
                return false;

            var tempList = bc.CCC.getClassPointList();

            if (requiredTalents.Find(t=>!t.bUnlocked)==null)
            {
                foreach (var item in cpReq)
                {
                    if (tempList.Find(cp => item.classID == cp.classID && item.points <= cp.points) != default(ClassPoints))
                    {
                        return true;
                    }
                }
            }


            return false;
        }

        virtual public void ApplyUnlockCost(BaseCharacter bc)
        {
            var tempList = bc.CCC.getClassPointList();

            foreach (var item in cpReq)
            {
                var cp = tempList.Find(c => c.classID == item.classID && c.points >= item.points);
                cp.points -= item.points;
                cp.spendPoints += item.points;
            }

            bUnlocked = true;
        }

        virtual public void Unlock(BaseCharacter bc)
        {
            bUnlocked = true;
        }

        public override string ToString()
        {
            return talentNode.ToString();
        }

        internal void AddCPRequiremenent(BaseClass c)
        {
            if (cpReq.Find(cp => cp.classID == c.classIdentifier) == null)
            {
                cpReq.Add(new ClassPoints(c, 0));
            }

        }
    }
}
