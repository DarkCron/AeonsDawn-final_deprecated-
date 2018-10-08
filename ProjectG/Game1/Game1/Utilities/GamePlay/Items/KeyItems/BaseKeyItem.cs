using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    public class BaseKeyItem:BaseItem
    {
        [XmlElement("Key Item Active Stat Modifier")]
        public ActiveStatModifier KeyItemActiveStatModifier = new ActiveStatModifier();

        public BaseKeyItem():base() {

        }

        public BaseKeyItem(bool b) : base(true)
        {
            itemType = ITEM_TYPES.Quest_Item;
            itemStackSize = 1;
            itemMaxAmount = 1;
            KeyItemActiveStatModifier = new ActiveStatModifier(true);
        }
    }
}
