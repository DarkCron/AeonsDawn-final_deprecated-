using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("Active Stat Modifier")]
    public class ActiveStatModifier
    {
        [XmlElement("Active stat modifier numbers")]
        public List<int> activeStatModifier = new List<int>();
        [XmlElement("Active stat modifier length")]
        public int length = 1;

        [XmlIgnore]
        private int turnsPassed = 0;
        [XmlIgnore]
        public String displayName = "";

        public ActiveStatModifier()
        {

        }

        public ActiveStatModifier(bool b)
        {
            foreach (var item in Enum.GetNames(typeof(STATChart.ACTIVESTATS)))
            {
                activeStatModifier.Add(0);
            }
        }

        public void tick()
        {
            if (length != -1)
            {
                turnsPassed++;
            }
        }

        public bool isOver()
        {
            if (length != -1)
            {
                if (turnsPassed >= length)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void ProcessStats(STATChart sc, STATChart trueSC)
        {
            int maxTrueHP = trueSC.currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP];
            int maxTrueMP = trueSC.currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA];

            int currentTrueHP = trueSC.currentActiveStats[(int)STATChart.ACTIVESTATS.HP];
            int currentTrueMP = trueSC.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA];

            if (currentTrueHP <= maxTrueHP)
            {
                if (currentTrueHP + activeStatModifier[(int)STATChart.ACTIVESTATS.HP] < maxTrueHP)
                {
                    sc.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] += activeStatModifier[(int)STATChart.ACTIVESTATS.HP];
                }
                else
                {
                    sc.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] = maxTrueHP;
                }
            }

            if (currentTrueMP <= maxTrueMP)
            {
                if (currentTrueMP + activeStatModifier[(int)STATChart.ACTIVESTATS.MANA] < maxTrueMP)
                {
                    sc.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA] += activeStatModifier[(int)STATChart.ACTIVESTATS.MANA];
                }
                else
                {
                    sc.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA] = maxTrueMP;
                }
            }

            sc.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] += activeStatModifier[(int)STATChart.ACTIVESTATS.SHIELD];

            sc.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP] += activeStatModifier[(int)STATChart.ACTIVESTATS.STORED_AP];
        }

        public List<String> ActiveChartNames()
        {
            List<String> list = new List<string>();

            int i = 0;
            foreach (var statName in Enum.GetNames(typeof(STATChart.ACTIVESTATS)))
            {
                list.Add(statName + ": " + activeStatModifier[i]);
                i++;
            }

            return list;
        }

        public ActiveStatModifier Clone()
        {
            ActiveStatModifier temp = (ActiveStatModifier)this.MemberwiseClone();
            temp.activeStatModifier = new List<int>(activeStatModifier);
            return temp;
        }

        public static ActiveStatModifier Generate(STATChart sc)
        {
            ActiveStatModifier temp = new ActiveStatModifier();
            temp.activeStatModifier = new List<int>(sc.currentActiveStats);
            foreach (var item in temp.activeStatModifier)
            {
                if (item != 0)
                {
                    return temp;
                }
            }
            return null;
        }
    }
}
