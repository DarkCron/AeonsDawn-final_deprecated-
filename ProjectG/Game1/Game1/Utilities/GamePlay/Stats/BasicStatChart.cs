using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Stats
{
    class BasicStatChart
    {
        public enum stats { Health = 0, Mana, Strength, Stamina, Agility, Luck, Defense, Speed, SpecDefense, SpecAttack, Shield,MaxHealth, MaxMana };

        //Represents the base stats
        public List<int> baseStats = new List<int>();
        public List<int> currentStats = new List<int>();

        public BasicStatChart(List<int> baseStats = default(List<int>))
        {
            if(baseStats!=default(List<int>)){
                foreach (var stat in baseStats)
                {
                    this.baseStats.Add(stat);
                }
                while (Enum.GetNames(typeof(stats)).Length!=this.baseStats.Count)
                {
                    this.baseStats.Add(1);
                }
            }
            else
            {
                while (Enum.GetNames(typeof(stats)).Length != this.baseStats.Count)
                {
                    this.baseStats.Add(1);
                }
            }

            ResetCurrentStats();
        }

        public void ResetCurrentStats()
        {
            currentStats.Clear();
            foreach (var stat in baseStats)
            {
                currentStats.Add(stat);
            }
        }

        public void AssignStats(List<int> baseStats = default(List<int>))
        {
            if (baseStats != default(List<int>))
            {
                int temp = 0;
                foreach (var stat in baseStats)
                {
                    this.baseStats.Add(stat);
                    temp++;
                }
                while (Enum.GetNames(typeof(stats)).Length != this.baseStats.Count)
                {
                    this.baseStats.Add(this.baseStats[temp]);
                    temp++;
                }
            }
        }

        public String HealthAsString()
        {
            return "HEALTH: "+baseStats[(int)stats.MaxHealth]+@" / "+currentStats[(int)stats.Health];
        }

        public String ManaAsString()
        {
            return "MANA: " + baseStats[(int)stats.MaxMana] + @" / " + currentStats[(int)stats.Mana];
        }
    }
}
