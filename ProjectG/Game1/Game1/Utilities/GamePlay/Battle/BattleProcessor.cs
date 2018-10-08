using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    static public class BattleProcessor
    {
        static STATChart giverAS = new STATChart(); //AS = ACTUAL STATS
        static STATChart receiverAS = new STATChart(); //AS = ACTUAL STATS
        static BaseCharacter giver = new BaseCharacter();
        static BaseCharacter receiver = new BaseCharacter();

        static BasicAbility castAbility = new BasicAbility();

        static int baseDodge = 5;
        static int baseCrit = 5;
        static int baseFail = 5;
        static List<int> bMods = new List<int> { 5, 5, 5 };

        public enum BattleMods { CRIT = 0, DODGE, FAIL, MAX_DAMAGE, MIN_DAMAGE, MIN_STATUS_CHANCE, MAX_STATUS_CHANCE }

        public static void InitiateCombat(BaseCharacter g, BaseCharacter r, BasicAbility gca, BasicAbility rca)
        {
            bMods.Clear();
            foreach (var item in Enum.GetNames(typeof(BattleMods)))
            {
                bMods.Add(0);
            }
            castAbility = gca;
            giver = g;
            receiver = r;
            // giverAS.DefaultStatChart();
            // receiverAS.DefaultStatChart();

            giverAS = g.trueSTATChart();
            receiverAS = r.trueSTATChart();
            Console.WriteLine(g.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]);

            if (gca.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK)
            {
                bMods[(int)BattleMods.MIN_DAMAGE] = CalculateMinDamage();
                bMods[(int)BattleMods.MAX_DAMAGE] = CalculateMaxDamage();
                bMods[(int)BattleMods.MIN_STATUS_CHANCE] = 0;
                bMods[(int)BattleMods.MAX_STATUS_CHANCE] = 0;
                bMods[(int)BattleMods.CRIT] = 5;
                bMods[(int)BattleMods.DODGE] = 5;
                bMods[(int)BattleMods.FAIL] = 5;
            }

            BattleGUI.Start(giverAS, receiverAS, g, r, gca, rca);
        }

        static internal KeyValuePair<List<int>, List<int>> giverDMG = new KeyValuePair<List<int>, List<int>>();

        public static KeyValuePair<List<int>, List<int>> CalculateDamage(BaseCharacter g, BaseCharacter r, BasicAbility gca, BasicAbility rca)
        {
            BasicAbility Idle = new BasicAbility();
            Idle.abilityName = "Idle";


            castAbility = gca;
            giver = g;
            receiver = r;


            giverAS = g.trueSTATChart();
            giverAS = giverAS.StatChartAddition(gca.abilityModifier);
            receiverAS = r.trueSTATChart();

            List<int> giverDMG = new List<int>();
            List<int> targetDMG = new List<int>();
            if (gca.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK)
            {
                giverDMG.Add(CalculateMinDamage());
                giverDMG.Add(CalculateMaxDamage());
                targetDMG.Add(0);
                targetDMG.Add(0);
            }

            return new KeyValuePair<List<int>, List<int>>(giverDMG, targetDMG);
        }

        private static int CalculateMaxDamage()
        {
            /*
            MAX_DAMAGE:	MELEE						
	: - R DEF - R MASTERY + G STRx2 - (R SHIELDx2) + G MASTERY x 2						
	MAGIC						
	: - R CONC - R MASTERY + G CONCx2 - (R MAGIC SHIELDx2) + G MASTERY x 2						
	RANGED						
	: - R DEF - R MASTERY + G AGIx2 - (R SHIELDx2) + G MASTERY x 2						


            */
            int maxDmg = 0;
            switch (castAbility.abilityFightStyle)
            {
                case (int)BasicAbility.ABILITY_CAST_TYPE.MELEE:
                    int temp = -receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF]
                        - receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY]
                        - receiverAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] * 2
                        + giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.STR] * 2
                        + (giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2);
                    if (temp < 0)
                    {
                        temp = giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2;
                    }
                    maxDmg = temp;
                    break;
                case (int)BasicAbility.ABILITY_CAST_TYPE.RANGED:
                    temp = -receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF]
    - receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY]
    - receiverAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD] * 2
    + giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.AGI] * 2
    + (giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2);
                    if (temp < 0)
                    {
                        temp = giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2;
                    }
                    maxDmg = temp;
                    break;
                case (int)BasicAbility.ABILITY_CAST_TYPE.MAGIC:
                    temp = -receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.INT]
- receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY]
+ giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.INT] * 2
+ (giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2);
                    if (temp < 0)
                    {
                        temp = giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2;
                    }
                    maxDmg = temp;
                    break;
            }

            if (maxDmg <= 0)
            {
                maxDmg = giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2;
            }
            return maxDmg;
        }

        private static int CalculateMinDamage()
        {
            /*
            MIN_DAMAGE:  MELEE
                        : -R DEF - R MASTERY + G STR - (R SHIELD) +G MASTERY x 2
                        MAGIC
                        : -R CONC - R MASTERY + G CONC - (R MAGIC SHIELD) +G MASTERY x 2
                        RANGED
                        : -R DEF - R MASTERY + G AGI - (R SHIELD) +G MASTERY x 2
                */
            int minDmg = 0;
            switch (castAbility.abilityFightStyle)
            {
                case (int)BasicAbility.ABILITY_CAST_TYPE.MELEE:
                    int temp = -receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF]
                        - receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY]
                        - receiverAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]
                        + giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.STR]
                        + (giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2);
                    if (temp < 0)
                    {
                        temp = giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2;
                    }
                    minDmg = temp;
                    break;
                case (int)BasicAbility.ABILITY_CAST_TYPE.RANGED:
                    temp = -receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF]
    - receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY]
    - receiverAS.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]
    + giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.AGI]
    + (giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2);
                    if (temp < 0)
                    {
                        temp = giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2;
                    }
                    minDmg = temp;
                    break;
                case (int)BasicAbility.ABILITY_CAST_TYPE.MAGIC:
                    temp = -receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.INT]
- receiverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY]
+ giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.INT]
+ (giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2);
                    if (temp < 0)
                    {
                        temp = giverAS.currentPassiveStats[(int)STATChart.PASSIVESTATS.MASTERY] * 2;
                    }
                    minDmg = temp;
                    break;
            }

            return minDmg;
        }

        internal static KeyValuePair<List<int>, List<int>> CalculateHealing(BaseCharacter g, BaseCharacter r, BasicAbility gca, BasicAbility rca)
        {
            BasicAbility Idle = new BasicAbility();
            Idle.abilityName = "Idle";


            castAbility = gca;
            giver = g;
            receiver = r;


            giverAS = g.trueSTATChart();
            giverAS = giverAS.StatChartAddition(gca.abilityModifier);
            receiverAS = r.trueSTATChart();

            List<int> giverDMG = new List<int>();
            List<int> targetDMG = new List<int>();
            if (gca.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT)
            {
                giverDMG.Add(-(g.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP]));
                giverDMG.Add(-(g.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP] + g.trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP] / 2));
                targetDMG.Add(0);
                targetDMG.Add(0);
            }

            return new KeyValuePair<List<int>, List<int>>(giverDMG, targetDMG);
        }

        internal static KeyValuePair<List<int>, List<int>> CalculateDamage(LUA.LuaAbilityInfo labii)
        {
            return new KeyValuePair<List<int>, List<int>>(new List<int> { labii.minDmg, labii.maxDmg }, new List<int> { 0, 0 });
        }

        internal static KeyValuePair<List<int>, List<int>> CalculateHealing(LUA.LuaAbilityInfo labii)
        {
            return new KeyValuePair<List<int>, List<int>>(new List<int> { labii.minDmg, labii.maxDmg }, new List<int> { 0, 0 });
        }
    }
}
