using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LUA;
using TBAGW;

namespace LUA
{
    static internal class ListDataProvider
    {
        static List<LUADataElement> LDP = new List<LUADataElement>();

        public static void setLuaData(NLua.Lua L)
        {
            var luaData = LUAUtilities.LUATableToListUtility(typeof(LUADataElement), (L["returnData"] as NLua.LuaFunction).Call()[0] as NLua.LuaTable).Cast<LUADataElement>().ToList();
            luaData.ForEach(data => data.GenerateObject(GameProcessor.gcDB));
            luaData.RemoveAll(data => data.o == null);

        }
    }

    public class LUADataElement
    {
        public String name = "";
        public int ID = -1;
        public Object o = null;

        public LUADataElement() { }

        internal virtual void GenerateObject(GameContentDataBase gcdb)
        {

        }
    }

    public class ItemDEFLua : LUADataElement
    {
        public int itemAmount = 1;
        public int buyPrice = -1;
        public int sellPrice = -1;
        public ItemDEFLua() { }

        internal override void GenerateObject(GameContentDataBase gcdb)
        {
            base.GenerateObject(gcdb);
            var temp = gcdb.gameItems.Find(item => item.itemName.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (temp == null)
            {
                temp = gcdb.gameItems.Find(item => item.itemID == ID);
            }

            if (temp == null)
            {
            }
            else
            {
                o = temp.Clone();
                BaseItem bi = o as BaseItem;

                if (itemAmount != 0 && itemAmount % bi.itemStackSize == 0)
                {
                    bi.itemAmount = bi.itemStackSize;
                }
                bi.itemAmount = itemAmount <= bi.itemStackSize ? itemAmount : itemAmount % bi.itemStackSize;
                if (buyPrice != -1)
                {
                    bi.itemBuyPrice = buyPrice;
                }
                else
                {
                    buyPrice = bi.itemBuyPrice;
                }

                if (sellPrice != -1)
                {
                    bi.itemSellPrice = sellPrice;
                }
                else
                {
                    sellPrice = bi.itemSellPrice;
                }

            }
        }
    }

    public class LuaDataCollection
    {
        public List<LUADataElement> data = new List<LUADataElement>();

        internal LuaDataCollection Clone()
        {
            LuaDataCollection ldc = (LuaDataCollection)this.MemberwiseClone();
            ldc.data = new List<LUADataElement>(data);
            return ldc;
        }

        public LuaDataCollection() { }

        internal void ConvertCheck()
        {
            data.ForEach(data => data.GenerateObject(GameProcessor.gcDB));
            data.RemoveAll(data => data.o == null);
        }
    }
}
