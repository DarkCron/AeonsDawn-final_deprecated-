using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("Inventory")]
    public class PlayerInventory
    {
        [XmlElement("Local inventory")]
        public List<BaseItem> localInventory = new List<BaseItem>();
        [XmlElement("Global inventory")]
        public List<BaseItem> globalInventory = new List<BaseItem>();
        [XmlElement("Local inventory Size")]
        public int localInventoryMaxSize = 32;

        public bool CanAddItemToInventory(BaseItem bi)
        {
            if (bi.itemType == BaseItem.ITEM_TYPES.Quest_Item)
            {
                return true;
            }
            else
            {
                if (localInventory.Count < 32) { return true; }
                else
                {
                    InventoryManager.AddItemToInventory(bi);
                    ManageStackableItems();
                    if (localInventory.Count < 32)
                    {
                        var temp = localInventory.Find(i=>i.itemID == bi.itemID && i.itemAmount >= bi.itemAmount);
                        temp.itemAmount -= bi.itemAmount;
                        if(temp.itemAmount == 0)
                        {
                            localInventory.Remove(temp);
                        }
                        return true;
                    }else
                    {
                        return false;
                    }
                }
            }
        }

        public void TryAddItemToLocalInventory(BaseItem bi)
        {
            var allIdenticalItems = localInventory.FindAll(abi => abi.itemID == bi.itemID);
            var freeItem = allIdenticalItems.Find(fi => !fi.MustCreateNewItemStack());
            if (freeItem != null)
            {
                freeItem.AddItemToStack();
            }
            else
            {
                localInventory.Add(bi);
            }
        }

        public void SoftResetItemAnimations()
        {
            foreach (var item in localInventory)
            {
                item.itemTexAndAnimation.SimpleReset();
            }

            foreach (var item in globalInventory)
            {
                item.itemTexAndAnimation.SimpleReset();
            }
        }

        public void ReloadAllItemsResources()
        {
            foreach (var item in localInventory)
            {
                item.ReloadTexture();
            }

            foreach (var item in globalInventory)
            {
                item.ReloadTexture();
            }
        }

        public void ManageStackableItems()
        {
            List<BaseItem> tempLocalInventory = new List<BaseItem>(localInventory);
            List<BaseItem> differentItems = new List<BaseItem>();
            foreach (var item in localInventory)
            {
                if (differentItems.Find(i => i.itemID == item.itemID) == null)
                {
                    differentItems.Add(item);
                }
            }
            foreach (var item in differentItems)
            {
                if (item.itemType != BaseItem.ITEM_TYPES.Equipment)
                {
                    var allTheSameItems = localInventory.FindAll(i => i.itemID == item.itemID && i.spacesFreeForItem() != 0);
                    if (allTheSameItems.Count != 0 && allTheSameItems.Count != 1)
                    {
                        var tempUtil = item.managerUtility(allTheSameItems);
                        tempLocalInventory.RemoveAll(i => allTheSameItems.Contains(i));
                        tempLocalInventory.AddRange(tempUtil);
                    }
                }
            }
            List<BaseItem> allConsumables = tempLocalInventory.FindAll(i => i.itemType == BaseItem.ITEM_TYPES.Consumables).OrderBy(i => i.itemName).ToList();
            List<BaseItem> allEquipment = tempLocalInventory.FindAll(i => i.itemType == BaseItem.ITEM_TYPES.Equipment).OrderBy(i => i.itemName).ToList();
            List<BaseItem> allKeyItems = tempLocalInventory.FindAll(i => i.itemType == BaseItem.ITEM_TYPES.Quest_Item).OrderBy(i => i.itemName).ToList();
            List<BaseItem> allMaterials = tempLocalInventory.FindAll(i => i.itemType == BaseItem.ITEM_TYPES.Materials).OrderBy(i => i.itemName).ToList();

            localInventory.Clear();
            localInventory.AddRange(allConsumables);
            localInventory.AddRange(allMaterials);
            localInventory.AddRange(allKeyItems);
            localInventory.AddRange(allEquipment);
            //localInventory = tempLocalInventory;
        }
    }
}
