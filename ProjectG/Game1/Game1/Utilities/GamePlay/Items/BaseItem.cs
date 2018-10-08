using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    [XmlRoot("BASE ITEM")]
    [XmlInclude(typeof(BaseEquipment))]
    [XmlInclude(typeof(BaseConsumable))]
    [XmlInclude(typeof(BaseMaterials))]
    [XmlInclude(typeof(BaseKeyItem))]
    public class BaseItem
    {
        public enum ITEM_TYPES { Consumables = 0, Equipment, Quest_Item, Generic, Materials, None }
        public enum ITEM_RARITY { Junk, Common, Uncommon, Rare, Epic, Legendary }


        [XmlElement("Stat modifier")]
        public STATChart statModifier = new STATChart(true); //fixed
        [XmlElement("Item Type")]
        public ITEM_TYPES itemType = ITEM_TYPES.Generic; //fixed
        [XmlElement("Item Name")]
        public String itemName = "Default item name"; //fixed
        [XmlElement("Item Icon Location")]
        public String itemTexLoc = ""; //fixed
        [XmlElement("Item Texture&Animation")]
        public ShapeAnimation itemTexAndAnimation = new ShapeAnimation(); //fixed
        [XmlElement("Item Description")]
        public String itemDescription = "A description"; //fixed
        [XmlElement("Short Item Description")]
        public String itemShortDescription = "A short description"; //fixed
        [XmlElement("Item Stack Size")]
        public int itemStackSize = 1; //fixed
        /// <summary>
        /// Max amount -1 is infinity
        /// </summary>
        [XmlElement("Item Max Amount")]
        public int itemMaxAmount = 32; //fixed
        [XmlElement("Item Price Buy")]
        public int itemBuyPrice = 10;
        [XmlElement("Item Price Sell")]
        public int itemSellPrice = 1;
        [XmlElement("Item ID")]
        public int itemID = 0; //fixed
        [XmlElement("Item Rarity")]
        public ITEM_RARITY ItemRarity = ITEM_RARITY.Common; //fixed
        [XmlElement("Highest Item Index")]
        public static int itemIDLatest = 0; //fixed
        [XmlElement("Item Amount")]
        public int itemAmount = 1; 

        [XmlIgnore]
        public bool bMarkedAsRare = false;

        public BaseItem()
        {

        }

        public BaseItem(bool b)
        {
            if (b)
            {
                itemID = MapBuilder.gcDB.itemID;
                MapBuilder.gcDB.itemID++;
            }

        }

        public void ReloadTexture()
        {
            itemTexAndAnimation.texFileLoc = itemTexLoc;
            itemTexAndAnimation.ReloadTexture();
        }

        public void ResetAnimations()
        {
            if (itemTexAndAnimation.animationFrames.Count != 0 && itemTexAndAnimation.animationFrames.Count != 1)
            {
                itemTexAndAnimation.SimpleReset();

            }
        }

        public void UpdateAnimation(GameTime gt)
        {
            if (itemTexAndAnimation.animationFrames.Count != 0 && itemTexAndAnimation.animationFrames.Count != 1)
            {
                itemTexAndAnimation.UpdateAnimationForItems(gt);
            }
        }

        public override string ToString()
        {
            return itemName + ", ID: " + itemID;
        }

        public bool MustCreateNewItemStack()
        {
            if (itemAmount < itemStackSize)
            {
                return false;
            }
            return true;
        }

        public void AddItemToStack()
        {
            itemAmount++;
        }

        public KeyValuePair<int, int> amountAndStacks()
        {
            int amount = itemAmount;
            int amountOfStacks = 0;
            if (itemStackSize==1)
            {
                return new KeyValuePair<int, int>(1, 0);
            }
            while (MustCreateNewItemStack())
            {
                itemAmount -= itemStackSize;
                amount = itemAmount;
                amountOfStacks++;
                if (amount == 0)
                {
                    break;
                }
               
            }

            return new KeyValuePair<int, int>(amount, amountOfStacks);
        }

        public void ConsumeItem()
        {
            itemAmount--;
            if (itemAmount <= 0)
            {
                switch (itemType)
                {

                    case ITEM_TYPES.Quest_Item:
                        var temp = PlayerSaveData.playerInventory.globalInventory.Find(i => i.itemID == this.itemID && i.itemAmount == 0);
                        if (temp != default(BaseItem))
                        {
                            PlayerSaveData.playerInventory.globalInventory.Remove(temp);
                        }

                        break;

                    default:
                        var temp2 = PlayerSaveData.playerInventory.localInventory.Find(i => i.itemID == this.itemID && i.itemAmount <= 0);
                        if (temp2 != default(BaseItem))
                        {
                            PlayerSaveData.playerInventory.localInventory.Remove(temp2);
                        }
                        break;
                }
            }
        }

        public Color itemRarityColour()
        {
            switch (ItemRarity)
            {
                case ITEM_RARITY.Junk:
                    return Color.DarkGray;
                    break;
                case ITEM_RARITY.Common:
                    return Color.White;
                    break;
                case ITEM_RARITY.Uncommon:
                    return Color.ForestGreen;
                    break;
                case ITEM_RARITY.Rare:
                    return Color.Blue;
                    break;
                case ITEM_RARITY.Epic:
                    return Color.Purple;
                    break;
                case ITEM_RARITY.Legendary:
                    return Color.Orange;
                    break;
                default:
                    return Color.White;
                    break;
            }
        }


        public bool ItemIsExhausted()
        {
            if (itemAmount == 0)
            {
                return false;
            }
            return true;
        }

        public int spacesFreeForItem()
        {
            return itemMaxAmount - itemAmount;
        }

        public List<BaseItem> managerUtility(List<BaseItem> lbi)
        {
            int totalAmountOfThisItem = 0;
            foreach (var item in lbi)
            {
                totalAmountOfThisItem += item.itemAmount;
            }
            int amountOfNewStacks = totalAmountOfThisItem / itemStackSize;
            int remainder = totalAmountOfThisItem - amountOfNewStacks * itemStackSize;
            List<BaseItem> temp = new List<BaseItem>();
            for (int i = 0; i < amountOfNewStacks; i++)
            {
                var tempItem = (BaseItem)this.MemberwiseClone();
                tempItem.itemAmount = itemStackSize;
                temp.Add(tempItem);
            }
            if (remainder > 0)
            {
                var tempItem = (BaseItem)this.MemberwiseClone();
                tempItem.itemAmount = remainder;
                temp.Add(tempItem);
            }
            return temp;
        }

        public BaseItem Clone()
        {
            var clone = (BaseItem)this.MemberwiseClone();
            clone.itemTexAndAnimation = this.itemTexAndAnimation.Clone();
            return clone;
        }

        public static implicit operator int(BaseItem bi)
        {
            return bi.itemID;
        }
    }
}
