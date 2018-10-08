using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    public class BaseConsumable:BaseItem
    {
        public enum TargetRange { Self = 0, Selectable, Team}
        public enum ConsumableType { Support = 0, Offense}

        [XmlElement("Consumable usage radius")]
        public int usageRadius = 0;
        [XmlElement("Consumable can target everything")]
        public bool bCanTargetEveryone = false;
        [XmlElement("Consumable target range")]
        public TargetRange ConsumableTargetRange = TargetRange.Self;
        [XmlElement("Consumable Active Stat Modifier")]
        public ActiveStatModifier ConsumableActiveStatModifier = new ActiveStatModifier();
        [XmlElement("Consumable Type")]
        public ConsumableType ConsumableTypeEffect = ConsumableType.Support;
        [XmlElement("Consumable modifier")]
        public BaseModifier ConsumableModifier = new BaseModifier();

        public BaseConsumable() : base()
        {

        }

        public BaseConsumable(bool b):base(true) {
            itemType = ITEM_TYPES.Consumables;
            itemStackSize = 16;
            itemMaxAmount = -1;
            ConsumableActiveStatModifier = new ActiveStatModifier(true);
        }

        public bool HasModifier()
        {
            foreach (var item in ConsumableModifier.statModifier.currentPassiveStats)
            {
                if(item!=0)
                {
                    return true;
                }
            }


            foreach (var item in ConsumableModifier.statModifier.currentSpecialStats)
            {
                if (item != 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
