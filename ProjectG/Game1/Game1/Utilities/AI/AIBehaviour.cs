using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    static public class AIBehaviour
    {
        public enum AIBehaviourType { Neutral = 0, Berserk, Magic_Hater, Random, Challenger, Coward, Avenger, Ambitious, Finisher, Intellect_Hater }

        static public List<KeyValuePair<BaseCharacter, int>> ProcessBehaviour(BaseCharacter bc, List<BaseCharacter> lbc)
        {
            List<KeyValuePair<BaseCharacter, int>> charsAndThreat = new List<KeyValuePair<BaseCharacter, int>>();
            int randomNum = GamePlayUtility.Randomize(0, lbc.Count);
            AIBehaviourType Behaviour = (bc).Behaviour;

            if (lbc.Count != 0 && CombatProcessor.heroCharacters.Contains(lbc[0]))
            {
                foreach (var character in lbc)
                {
                    int threat = character.returnTotalThreat();


                    float modifier = 1.3f;

                    switch (Behaviour)
                    {
                        case AIBehaviourType.Neutral:
                            break;
                        case AIBehaviourType.Berserk:
                            int maxSTR = lbc.Max(c => c.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.STR]);
                            if (character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.STR] == maxSTR)
                            {
                                threat = (int)(modifier * threat);
                                threat += 10;
                            }
                            break;
                        case AIBehaviourType.Magic_Hater:
                            if (character.CCC.equippedClass.classType == BaseClass.CLASSType.CASTER)
                            {
                                threat = (int)(modifier * threat);
                                threat += 10;
                            }
                            break;
                        case AIBehaviourType.Random:
                            if (lbc.IndexOf(character) == randomNum)
                            {
                                threat = (int)(modifier * threat);
                                threat += 10;
                            }
                            break;
                        case AIBehaviourType.Challenger:
                            int maxDEF = lbc.Max(c => c.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF]);
                            if (character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.DEF] == maxDEF)
                            {
                                threat = (int)(modifier * threat);
                                threat += 10;
                            }
                            break;
                        case AIBehaviourType.Coward:
                            maxSTR = lbc.Max(c => c.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.STR]);
                            if (character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.STR] == maxSTR)
                            {
                                threat = (int)((0.8f) * threat);
                                threat -= 7;
                            }
                            break;
                        case AIBehaviourType.Avenger:
                            int maxKD = lbc.Max(c => BattleStats.getKDFromBattle(c));
                            if (BattleStats.getKDFromBattle(character) == maxKD)
                            {
                                threat = (int)((modifier) * threat);
                                threat += 10;
                            }
                            break;
                        case AIBehaviourType.Ambitious:
                            int maxHP = lbc.Max(c => c.trueSTATChart().currentPassiveStats[(int)STATChart.ACTIVESTATS.HP]);
                            if (character.trueSTATChart().currentPassiveStats[(int)STATChart.ACTIVESTATS.HP] == maxHP)
                            {
                                threat = (int)((modifier) * threat);
                                threat += 10;
                            }
                            break;
                        case AIBehaviourType.Finisher:
                            int minHP = lbc.Min(c => c.trueSTATChart().currentPassiveStats[(int)STATChart.ACTIVESTATS.HP]);
                            if (character.trueSTATChart().currentPassiveStats[(int)STATChart.ACTIVESTATS.HP] == minHP)
                            {
                                threat = (int)((modifier) * threat);
                                threat += 10;
                            }
                            break;
                        case AIBehaviourType.Intellect_Hater:
                            int maxINT = lbc.Max(c => c.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.INT]);
                            if (character.trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.INT] == maxINT)
                            {
                                threat = (int)(modifier * threat);
                                threat += 10;
                            }
                            break;
                        default:
                            break;
                    }
                    charsAndThreat.Add(new KeyValuePair<BaseCharacter, int>(character, threat));

                }


            }
            else
            {

                List<BaseCharacter> charactersThatNeedHealing = lbc.FindAll(h => h.NeedsHealing());

                if (charactersThatNeedHealing.Count == 0)
                {
                    //FILL SUPPORTLOGIC HERE LATER
                    foreach (var character in lbc)
                    {
                        charsAndThreat.Add(new KeyValuePair<BaseCharacter, int>(character, GamePlayUtility.Randomize(0, 20)));
                    }
                }
                else
                {
                    foreach (var character in charactersThatNeedHealing)
                    {
                        charsAndThreat.Add(new KeyValuePair<BaseCharacter, int>(character, GamePlayUtility.Randomize(character.HealingRequired, character.HealingRequired * 2)));
                    }
                }


            }




            return charsAndThreat;
        }
    }
}
