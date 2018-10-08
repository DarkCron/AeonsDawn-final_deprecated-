using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("Loot list")]
    public class LootList
    {
        [XmlElement("Items that always drop")]
        public List<ItemLootInfo> universalDrop = new List<ItemLootInfo>();
        [XmlArray("Items drop per zone")]
        public List<List<ItemLootInfo>> dropsPerRegionLevel = new List<List<ItemLootInfo>>();
        [XmlArray("Money drop")]
        public List<List<int>> moneyDropPerLevel = new List<List<int>>();
        [XmlArray("Exp drop")]
        public List<int> expDropPerLevel = new List<int>();

        public LootList() { }

        public void ReloadGCDB(GameContentDataBase gcdb)
        {
            foreach (var item in universalDrop)
            {
                item.ReloadWithGCDB(gcdb);
            }

            foreach (var item in dropsPerRegionLevel)
            {
                foreach (var loot in item)
                {
                    loot.ReloadWithGCDB(gcdb);
                }
            }

            if (expDropPerLevel.Count !=moneyDropPerLevel.Count)
            {
                while(expDropPerLevel.Count!= moneyDropPerLevel.Count)
                {
                    expDropPerLevel.Add(0);
                }
            }
        }

        public void AddLevel()
        {
            dropsPerRegionLevel.Add(new List<ItemLootInfo>());
            moneyDropPerLevel.Add(new List<int> { 0, 0 });
            expDropPerLevel.Add(0);
        }
    }
}
