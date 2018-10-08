using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    static public class LootGenerator
    {
        static List<LootList> lootListToProcess = new List<LootList>();

        public static void Generate(List<BaseCharacter> lbcs)
        {
            lootListToProcess.Clear();
            foreach (var item in lbcs)
            {
                lootListToProcess.Add(item.lootList);
            }
        }

        public static List<BaseItem> GenerateLoot(int regionLevel = 0)
        {
            List<BaseItem> items = new List<BaseItem>();
            foreach (var lootList in lootListToProcess)
            {
                foreach (var uItem in lootList.universalDrop)
                {
                    items.Add(uItem.returnDrop());
                }

                if (regionLevel < lootList.dropsPerRegionLevel.Count)
                {
                    foreach (var rItem in lootList.dropsPerRegionLevel[regionLevel])
                    {
                        items.Add(rItem.returnDrop());
                    }
                }
            }

            items.RemoveAll(i => i == null);

            List<KeyValuePair<int, int>> info = new List<KeyValuePair<int, int>>();
            foreach (var item in items)
            {
                info.Add(item.amountAndStacks());
            }
            List<BaseItem> final = new List<BaseItem>();
            int index = 0;
            foreach (var item in info)
            {
                for (int i = 0; i < item.Value; i++)
                {
                    var temp = items[index].Clone();
                    temp.itemAmount = temp.itemStackSize;
                    final.Add(temp);
                }

                var temp2 = items[index].Clone();
                temp2.itemAmount = item.Key;
                final.Add(temp2);

                index++;
            }

            var allUniqueStackables = final.FindAll(item => item.itemStackSize >= 1).GroupBy(i => i.itemID).Select(grp => grp.First()).ToList();

            info = new List<KeyValuePair<int, int>>();
            List<itemStackAmount> lisa = new List<itemStackAmount>();
            foreach (var item in allUniqueStackables)
            {
                var temp = item.Clone();
                temp.itemAmount = 0;
                foreach (var it in final)
                {
                    if (temp.itemID == it.itemID)
                    {
                        temp.itemAmount += it.itemAmount;

                    }
                }
                info.Add(temp.amountAndStacks());
                lisa.Add(new itemStackAmount(temp, info.Last().Value, info.Last().Key));
            }


            final = new List<BaseItem>();
            foreach (var item in lisa)
            {
                for (int i = 0; i < item.stacks; i++)
                {
                    var temp = item.type.Clone();
                    temp.itemAmount = temp.itemStackSize;
                    final.Add(temp);
                }

                var temp2 = item.type.Clone();
                temp2.itemAmount = item.nonStacks;
                final.Add(temp2);

                index++;
            }
            final.RemoveAll(ele => ele.itemAmount == 0);

            return final;
        }

        internal struct itemStackAmount
        {
            internal BaseItem type;
            internal int stacks;
            internal int nonStacks;

            internal itemStackAmount(BaseItem type, int stacks, int nonStacks)
            {
                this.type = type;
                this.stacks = stacks;
                this.nonStacks = nonStacks;
            }
        }

        public static List<KeyValuePair<BaseCharacter, int>> GenerateExp(int regionLevel = 0)
        {
            List<KeyValuePair<BaseCharacter, int>> temp = new List<KeyValuePair<BaseCharacter, int>>();
            int expAmount = 0;
            foreach (var item in lootListToProcess)
            {
                expAmount += item.expDropPerLevel[regionLevel];
            }

            foreach (var item in PlayerSaveData.heroParty.FindAll(bc => bc.IsAlive()))
            {
                temp.Add(new KeyValuePair<BaseCharacter, int>(item, expAmount));
            }

            return temp;
        }
    }
}
