using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities;

namespace TBAGW
{
    [XmlRoot("Item Loot Info")]
    public class ItemLootInfo
    {
        [XmlElement("item ID")]
        public int itemID = -1;
        [XmlElement("Item min drop amount")]
        public int minDrop = 1;
        [XmlElement("Item max drop amount")]
        public int maxDrop = 1;
        [XmlElement("Chance to drop")]
        public float chanceToDrop = 50;
        [XmlElement("Is drop amount influenced by chance")]
        public bool dropStackChanceStack = true;
        [XmlElement("Requirement to drop")]
        public int boolID = -1;
        [XmlElement("Is rare")]
        public bool bIsRare = false;

        [XmlIgnore]
        public bool bItemIsStackable = false;
        [XmlIgnore]
        public BaseItem item;

        public ItemLootInfo()
        { }

        public ItemLootInfo(BaseItem bi)
        {
            item = bi;
            itemID = item.itemID;
            GenerateLogic();
        }

        public void ReloadWithGCDB(GameContentDataBase gcdb)
        {
            if (itemID != -1)
            {
                try
                {
                    item = gcdb.gameItems.Find(i => i.itemID == itemID);
                    GenerateLogic();
                }
                catch
                {
                    Console.WriteLine("Error: item not found in database, ID:" + itemID);
                }
            }
        }

        private void GenerateLogic()
        {
            bItemIsStackable = item.itemStackSize != 1 ? true : false;
        }

        public int dropAmount()
        {
            if (!bItemIsStackable)
            {
                if (GamePlayUtility.randomChance() < chanceToDrop)
                {
                  
                    return 1;
                }
                return -1;
            }
            else
            {
                if (!dropStackChanceStack)
                {
                    return GamePlayUtility.Randomize(minDrop, maxDrop);
                }

                int amount = -1;
                while (amount < maxDrop && GamePlayUtility.randomChance() < chanceToDrop)
                {
                    if (amount == -1)
                    {
                        amount = minDrop;
                    }
                    amount++;
                }
                return amount;
            }
        }

        public bool canDrop()
        {
            if (boolID == -1)
            {
                return true;
            }
            else
            {
                var test = GameProcessor.psData.allConditionals().Find(b => b.boolID == boolID);
                if (test == null)
                {
                    return true;
                }
                else
                {
                    return test.IsEnabled();
                }
            }
        }

        public override string ToString()
        {
            if (item != null)
            {
                return item.ToString();
            }

            return base.ToString();
        }

        public BaseItem returnDrop()
        {
            if (!canDrop())
            {
                return null;
            }

            int amount = dropAmount();

            if (amount == -1 || amount == 0)
            {
                return null;
            }


            var temp = item.Clone();

            temp.itemAmount = amount;
            temp.bMarkedAsRare = bIsRare;

            return temp;
        }
    }
}
