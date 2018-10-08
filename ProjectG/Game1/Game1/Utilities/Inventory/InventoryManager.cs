using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TBAGW.BaseItem;

namespace TBAGW
{
    public static class InventoryManager
    {
        static public PlayerInventory playerInventory;

        static public void Start(PlayerInventory pi) {
            playerInventory = pi;
        }

        public static bool CanAddItemToInventory(BaseItem bi)
        {
            return playerInventory.CanAddItemToInventory(bi);
        }

        public static void removeAll(BaseItem bi)
        {
            switch (bi.itemType)
            {

                case ITEM_TYPES.Quest_Item:
                    var temp = PlayerSaveData.playerInventory.globalInventory.FindAll(i => i.itemID == bi.itemID);
                    if (temp.Count != 0)
                    {
                        PlayerSaveData.playerInventory.globalInventory.RemoveAll(i => temp.Contains(i));
                    }

                    break;

                default:
                    var temp2 = PlayerSaveData.playerInventory.localInventory.FindAll(i => i.itemID == bi.itemID);
                    if (temp2.Count != 0)
                    {
                        PlayerSaveData.playerInventory.localInventory.RemoveAll(i => temp2.Contains(i));
                    }
                    break;
            }
        }

        static public void AddItemToInventory(BaseItem bi) {
            if(playerInventory==null)
            {
                Start(PlayerSaveData.playerInventory);
            }
            switch (bi.itemType)
            {
              
                case BaseItem.ITEM_TYPES.Consumables:
                    playerInventory.localInventory.Add(bi);
                    break;
                case BaseItem.ITEM_TYPES.Equipment:
                    playerInventory.localInventory.Add(bi);
                    break;
                case BaseItem.ITEM_TYPES.Quest_Item:
                    playerInventory.globalInventory.Add(bi);
                    break;
                case BaseItem.ITEM_TYPES.Generic:
                    Console.WriteLine(bi+" Typing is wrong, please check.");
                    break;
                case BaseItem.ITEM_TYPES.Materials:
                    playerInventory.localInventory.Add(bi);
                    break;
                default:
                    break;
            }
        }
    }
}
