using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("STATCHART")]
    public class STATChart
    {
        public enum ACTIVESTATS { HP = 0, MANA, AMMO, SHIELD, STORED_AP }
        public enum PASSIVESTATS { MAXHP = 0, MAXMANA, MAXAMMO, MAXSHIELD, STR, DEF, AGI, INT, AP, MOB, MASTERY }
        public enum SPECIALSTATS { EXTRA_BASE_CRIT, EXTRA_BASE_DODGE, DOT_MANA, EXTRA_HIT_CHANCE, EXTRA_BASE_FAIL, DOT_HP }

        public enum ACTIVESTATSNames { HP = 0, MANA, AMMO, SHIELD, STOREDAP }
        public enum PASSIVESTATSNames { MAXHP = 0, MAXMANA, MAXAMMO, MAXSHIELD, STR, DEF, AGI, INT, AP, MOB, MASTERY }
        public enum SPECIALSTATSNames { CRIT, DODGE, MPREGEN, PRECISION, FAILCHANCE, HPREGEN }

        [XmlArrayItem("ACTIVE STATS")]
        public List<int> currentActiveStats = new List<int>();
        [XmlArrayItem("PASSIVE STATS")]
        public List<int> currentPassiveStats = new List<int>();
        [XmlArrayItem("SPECIAL STATS")]
        public List<int> currentSpecialStats = new List<int>();

        public STATChart()
        {


        }

        public STATChart(bool b)
        {
            if (currentActiveStats.Count == 0)
            {
                foreach (var item in Enum.GetValues(typeof(ACTIVESTATS)))
                {
                    currentActiveStats.Add(0);
                }

                foreach (var item in Enum.GetValues(typeof(PASSIVESTATS)))
                {
                    currentPassiveStats.Add(0);
                }

                foreach (var item in Enum.GetValues(typeof(SPECIALSTATS)))
                {
                    currentSpecialStats.Add(0);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Statchart was reinitialized when it wasn't supposed to, check for errors", "WARNING");
            }

        }

        public void DefaultStatChart()
        {
            currentActiveStats.Clear();
            currentPassiveStats.Clear();
            currentSpecialStats.Clear();

            foreach (var item in Enum.GetValues(typeof(ACTIVESTATS)))
            {
                currentActiveStats.Add(0);
            }

            foreach (var item in Enum.GetValues(typeof(PASSIVESTATS)))
            {
                currentPassiveStats.Add(0);
            }

            foreach (var item in Enum.GetValues(typeof(SPECIALSTATS)))
            {
                currentSpecialStats.Add(0);
            }
        }

        public STATChart AddPassiveStatChart(STATChart sc)
        {
            STATChart newSC = new STATChart();
            newSC.DefaultStatChart();

            foreach (int statIndex in Enum.GetValues(typeof(PASSIVESTATS)))
            {
                try
                {
                    newSC.currentPassiveStats[statIndex] = currentPassiveStats[statIndex] + sc.currentPassiveStats[statIndex];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return newSC;
        }

        public STATChart AddActiveStatChart(STATChart sc)
        {
            STATChart newSC = new STATChart();
            newSC.DefaultStatChart();

            foreach (int statIndex in Enum.GetValues(typeof(ACTIVESTATS)))
            {
                try
                {
                    newSC.currentActiveStats[statIndex] = currentActiveStats[statIndex] + sc.currentActiveStats[statIndex];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return newSC;
        }

        public STATChart AddSpecialStatChart(STATChart sc)
        {
            STATChart newSC = new STATChart();
            newSC.DefaultStatChart();

            foreach (int statIndex in Enum.GetValues(typeof(SPECIALSTATS)))
            {
                try
                {
                    newSC.currentSpecialStats[statIndex] = currentSpecialStats[statIndex] + sc.currentSpecialStats[statIndex];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    currentSpecialStats.Add(0);
                }
            }

            return newSC;
        }

        public STATChart StatChartAddition(STATChart sc)
        {
            STATChart newSC = new STATChart();
            newSC.DefaultStatChart();


            foreach (int statIndex in Enum.GetValues(typeof(PASSIVESTATS)))
            {
                try
                {
                    newSC.currentPassiveStats[statIndex] = currentPassiveStats[statIndex] + sc.currentPassiveStats[statIndex];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }


            foreach (int statIndex in Enum.GetValues(typeof(ACTIVESTATS)))
            {
                try
                {
                    newSC.currentActiveStats[statIndex] = currentActiveStats[statIndex] + sc.currentActiveStats[statIndex];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            foreach (int statIndex in Enum.GetValues(typeof(SPECIALSTATS)))
            {
                try
                {
                    newSC.currentSpecialStats[statIndex] = currentSpecialStats[statIndex] + sc.currentSpecialStats[statIndex];
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                    currentSpecialStats.Add(0);
                }
            }

            return newSC;
        }

        public List<String> PassiveChartNames()
        {
            List<String> list = new List<string>();

            int i = 0;
            foreach (var statName in Enum.GetNames(typeof(PASSIVESTATS)))
            {
                list.Add(statName + ": " + currentPassiveStats[i]);
                i++;
            }

            return list;
        }

        public List<String> ActiveChartNames()
        {
            List<String> list = new List<string>();

            int i = 0;
            foreach (var statName in Enum.GetNames(typeof(ACTIVESTATS)))
            {
                list.Add(statName + ": " + currentActiveStats[i]);
                i++;
            }

            return list;
        }

        public List<String> SpecialChartNames()
        {
            List<String> list = new List<string>();

            int i = 0;
            try
            {
                foreach (var statName in Enum.GetNames(typeof(SPECIALSTATS)))
                {
                    list.Add(statName + ": " + currentSpecialStats[i]);
                    i++;
                }
            }
            catch
            {
                currentSpecialStats.Add(0);
                SpecialChartNames();
            }


            return list;
        }

        internal STATChart Clone()
        {
            STATChart sc = new STATChart();
            sc.currentActiveStats = new List<int>(currentActiveStats);
            sc.currentPassiveStats = new List<int>(currentPassiveStats);
            sc.currentSpecialStats = new List<int>(currentSpecialStats);
            return sc;
        }

        internal void MakeSureActiveAndPassiveStatsEqual()
        {
            if (currentActiveStats[(int)ACTIVESTATS.HP] > currentPassiveStats[(int)PASSIVESTATS.MAXHP])
            {
                currentPassiveStats[(int)PASSIVESTATS.MAXHP] = currentActiveStats[(int)ACTIVESTATS.HP];
            }

            if (currentActiveStats[(int)ACTIVESTATS.MANA] > currentPassiveStats[(int)PASSIVESTATS.MAXMANA])
            {
                currentPassiveStats[(int)PASSIVESTATS.MAXMANA] = currentActiveStats[(int)ACTIVESTATS.MANA];
            }

            if (currentActiveStats[(int)ACTIVESTATS.AMMO] > currentPassiveStats[(int)PASSIVESTATS.MAXAMMO])
            {
                currentPassiveStats[(int)PASSIVESTATS.MAXAMMO] = currentActiveStats[(int)ACTIVESTATS.AMMO];
            }

            if (currentActiveStats[(int)ACTIVESTATS.SHIELD] > currentPassiveStats[(int)PASSIVESTATS.MAXSHIELD])
            {
                currentPassiveStats[(int)PASSIVESTATS.MAXSHIELD] = currentActiveStats[(int)ACTIVESTATS.SHIELD];
            }



            if (currentPassiveStats[(int)PASSIVESTATS.MAXHP] > currentActiveStats[(int)ACTIVESTATS.HP])
            {
                if (currentActiveStats[(int)ACTIVESTATS.HP] > 0)
                {
                    currentPassiveStats[(int)PASSIVESTATS.MAXHP] = currentActiveStats[(int)ACTIVESTATS.HP];
                }
                else
                {
                    currentActiveStats[(int)ACTIVESTATS.HP] = currentPassiveStats[(int)PASSIVESTATS.MAXHP];
                }

            }

            if (currentPassiveStats[(int)PASSIVESTATS.MAXMANA] > currentActiveStats[(int)ACTIVESTATS.MANA])
            {
                if (currentActiveStats[(int)ACTIVESTATS.MANA] > 0)
                {
                    currentPassiveStats[(int)PASSIVESTATS.MAXMANA] = currentActiveStats[(int)ACTIVESTATS.MANA];
                }
                else
                {
                    currentActiveStats[(int)ACTIVESTATS.MANA] = currentPassiveStats[(int)PASSIVESTATS.MAXMANA];
                }

                // currentActiveStats[(int)ACTIVESTATS.MANA] = currentPassiveStats[(int)PASSIVESTATS.MAXMANA];
            }

            if (currentPassiveStats[(int)PASSIVESTATS.MAXAMMO] > currentActiveStats[(int)ACTIVESTATS.AMMO])
            {
                currentActiveStats[(int)ACTIVESTATS.AMMO] = currentPassiveStats[(int)PASSIVESTATS.MAXAMMO];
            }

            if (currentPassiveStats[(int)PASSIVESTATS.MAXSHIELD] > currentActiveStats[(int)ACTIVESTATS.SHIELD])
            {
                currentActiveStats[(int)ACTIVESTATS.SHIELD] = currentPassiveStats[(int)PASSIVESTATS.MAXSHIELD];
            }

            if (currentPassiveStats[(int)PASSIVESTATS.MAXHP] <= 0)
            {
                currentActiveStats[(int)ACTIVESTATS.HP] = 1;
                currentPassiveStats[(int)PASSIVESTATS.MAXHP] = 1;
            }
        }

        internal bool RequirementCheckFromChar(STATChart sc, bool bReverse = false)
        {
            for (int i = 0; i < sc.currentActiveStats.Count; i++)
            {
                if (i > 0)
                {
                    if (!bReverse)
                    {
                        if (currentActiveStats[i] < sc.currentActiveStats[i])
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (currentActiveStats[i] > sc.currentActiveStats[i])
                        {
                            return false;
                        }
                    }
                }
            }

            for (int i = 0; i < sc.currentPassiveStats.Count; i++)
            {
                if (i > 0)
                {
                    if (!bReverse)
                    {
                        if (currentPassiveStats[i] < sc.currentPassiveStats[i])
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (currentPassiveStats[i] > sc.currentPassiveStats[i])
                        {
                            return false;
                        }
                    }
                }
            }


            for (int i = 0; i < sc.currentSpecialStats.Count; i++)
            {
                if (i > 0)
                {
                    if (!bReverse)
                    {
                        if (currentSpecialStats[i] < sc.currentSpecialStats[i])
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (currentSpecialStats[i] > sc.currentSpecialStats[i])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        internal void AddStatChartWithoutActive(STATChart sc)
        {
            for (int i = 0; i < currentPassiveStats.Count; i++)
            {
                currentPassiveStats[i] += sc.currentPassiveStats[i];
            }

            for (int i = 0; i < currentSpecialStats.Count; i++)
            {
                currentSpecialStats[i] += sc.currentSpecialStats[i];
            }
        }
    }
}
