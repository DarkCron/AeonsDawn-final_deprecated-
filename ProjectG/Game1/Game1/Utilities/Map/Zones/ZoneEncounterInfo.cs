using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Encounter info per zone")]
    public class ZoneEncounterInfo
    {
        [XmlElement("Encounter chance")]
        public int encounterChance = 33;
        [XmlArrayItem("enemies chances")]
        public List<int> enemySpawnChance = new List<int>();
        [XmlElement("Minimum pack size")]
        public int packSizeMin = 1;
        [XmlElement("Maximum pack size")]
        public int packSizeMax = 5;
        [XmlElement("Enemy IDs")]
        public List<int> enemyInfoIDs = new List<int>();

        [XmlIgnore]
        public List<EnemyAIInfo> enemies = new List<EnemyAIInfo>();

        public ZoneEncounterInfo()
        {

        }

        public void Reload(GameContentDataBase gcdb)
        {
            //enemyInfoIDs.Clear();
            //  enemySpawnChance.Clear();
            foreach (var item in enemies)
            {
                try
                {
                    item.Reload(gcdb);
                }
                catch (Exception e)
                {

                }
            }
        }

        public void AddEnemy(EnemyAIInfo eai)
        {
            enemyInfoIDs.Add(eai.infoID);
            enemies.Add(eai);
            enemySpawnChance.Add(30);
        }

        public void RemoveEnemy(int index)
        {
            enemies.RemoveAt(index);
            enemySpawnChance.RemoveAt(index);
            enemyInfoIDs.RemoveAt(index);
        }
    }
}
