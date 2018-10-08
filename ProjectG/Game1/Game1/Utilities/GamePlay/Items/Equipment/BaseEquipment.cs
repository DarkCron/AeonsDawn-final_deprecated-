using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static TBAGW.BaseClass;

namespace TBAGW
{
    public class BaseEquipment : BaseItem
    {
        public enum EQUIP_TYPES { Weapon = 0, Armor }
        public enum ARMOUR_TYPES { Light, Medium, Heavy, Special }

        [XmlElement("Type of Equipment")]
        public EQUIP_TYPES EquipType = EQUIP_TYPES.Armor;
        [XmlElement("Who can use weapon")]
        public CLASSType WeaponUseType = CLASSType.MELEE;
        [XmlElement("Who can use armour")]
        public ARMOUR_TYPES ArmourUseType = ARMOUR_TYPES.Medium;
        [XmlElement("Equipment Active Stat Modifier")]
        public ActiveStatModifier EquipmentActiveStatModifier = new ActiveStatModifier();

        public BaseEquipment() : base()
        {

        }

        public BaseEquipment(bool b) : base(true)
        {
            itemType = ITEM_TYPES.Equipment;
            itemStackSize = 1;
            itemMaxAmount = -1;
            EquipmentActiveStatModifier = new ActiveStatModifier(true);
        }

        public List<KeyValuePair<String, int>> Differencewith(BaseEquipment be)
        {
            List<KeyValuePair<String, int>> temp = new List<KeyValuePair<string, int>>();
            if (be == null)
            {
                int index = 0;
                foreach (var item in statModifier.currentPassiveStats)
                {
                    if (item != 0)
                    {
                        STATChart.PASSIVESTATSNames name = (STATChart.PASSIVESTATSNames)index;
                        temp.Add(new KeyValuePair<string, int>(name.ToString()+": "+item, item));
                    }
                    index++;
                }
            }
            else
            {
                int index = 0;
                foreach (var item in statModifier.currentPassiveStats)
                {
                    if (item != 0 || be.statModifier.currentPassiveStats[index] != 0)
                    {
                        STATChart.PASSIVESTATSNames name = (STATChart.PASSIVESTATSNames)index;
                        temp.Add(new KeyValuePair<string, int>(name.ToString() + ": " + item, -(be.statModifier.currentPassiveStats[index] - item)));
                    }
                    index++;
                }
            }

            return temp;
        }
    }
}
