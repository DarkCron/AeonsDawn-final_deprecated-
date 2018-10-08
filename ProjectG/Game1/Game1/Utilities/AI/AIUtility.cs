using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;

using static TBAGW.BaseClass;

namespace TBAGW
{
    static public class AIUtility
    {
        static public List<BaseCharacter> possibleAITargets = new List<BaseCharacter>();
        static public bool bDoSupport = false;

        public static KeyValuePair<List<List<BasicTile>>, List<BasicAbility>> returnTilesToAttackFrom(List<BasicTile> moveRadius, List<BasicTile> allTiles, BaseCharacter caster, BaseCharacter target)
        {
            List<BasicAbility> tempAbilityList = new List<BasicAbility>();
            List<List<BasicTile>> abilityRanges = new List<List<BasicTile>>();
            //   Console.WriteLine("---------------Report Begin----------");
            //  Console.WriteLine("Checking if enemy has an ability to attack the selected partymember with: " + caster.CharacterName + caster.position);
            List<BasicAbility> abilityCheckList = new List<BasicAbility>();
            if (bDoSupport)
            {
                abilityCheckList.AddRange(caster.AbilityList().FindAll(abi => abi.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT));
            }
            else
            {
                abilityCheckList.AddRange(caster.AbilityList().FindAll(abi => abi.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK));
            }

            foreach (var ability in abilityCheckList)
            {
                if (ability.IsAbilityAvailable(caster.trueSTATChart()))
                {
                    if (ability.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK && ability.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.MELEE && (ability.targetableTypes.Contains(target.CCC.equippedClass.classType) || ability.targetableTypes.Count == 0))
                    {
                        abilityRanges.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, allTiles, (target.position / 64).ToPoint().ToVector2() * 64));
                        var test = abilityRanges[abilityRanges.Count - 1].Find(t => moveRadius.Contains(t));
                        if (test != null)
                        {
                            tempAbilityList.Add(ability);
                            Console.WriteLine(ability.ToString() + " is in range!");
                        }
                        else
                        {
                            abilityRanges.RemoveAt(abilityRanges.Count - 1);
                        }
                    }
                    else if (ability.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK && (ability.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.RANGED || ability.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.MAGIC) && (ability.targetableTypes.Contains(target.CCC.equippedClass.classType) || ability.targetableTypes.Count == 0))
                    {
                        if (ability.abilityCanHitTargetInRange(caster, target))
                        {
                            abilityRanges.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, allTiles, (target.position / 64).ToPoint().ToVector2() * 64));
                            tempAbilityList.Add(ability);
                            Console.WriteLine(ability.ToString() + " is in range!");
                        }
                        else
                        {
                            // abilityRanges.RemoveAt(abilityRanges.Count - 1);
                        }
                    }
                    else if (ability.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT && (ability.targetableTypes.Contains(target.CCC.equippedClass.classType) || ability.targetableTypes.Count == 0))
                    {
                        abilityRanges.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, allTiles, (target.position / 64).ToPoint().ToVector2() * 64));
                        var test = abilityRanges[abilityRanges.Count - 1].Find(t => moveRadius.Contains(t));
                        if (test != null)
                        {
                            tempAbilityList.Add(ability);
                            Console.WriteLine(ability.ToString() + " is in range!");
                        }
                        else
                        {
                            // abilityRanges.RemoveAt(abilityRanges.Count - 1);
                        }
                    }
                }

            }
            List<BasicTile> temp = new List<BasicTile>();
            foreach (var range in abilityRanges)
            {
                temp.AddRange(range.Except(temp));
            }
            temp.RemoveAll(t => !moveRadius.Contains(t));
            if (abilityRanges.Count == 0 && tempAbilityList.Count != 0)
            {
                abilityRanges.Add(new List<BasicTile>());
            }
            //   Console.WriteLine("---------------Report End----------");
            return new KeyValuePair<List<List<BasicTile>>, List<BasicAbility>>(abilityRanges, tempAbilityList);
        }

        public static void AIPreCalculate(List<BasicTile> moveRadius, CharacterTurn ct)
        {
            BaseCharacter caster = ct.character;
            List<List<BasicTile>> abilityRangesAttack = new List<List<BasicTile>>();
            List<List<BasicTile>> abilityRangesSupport = new List<List<BasicTile>>();
            List<BaseCharacter> possibleTargetCharsAttack = new List<BaseCharacter>();
            List<BaseCharacter> possibleTargetCharsSupport = new List<BaseCharacter>();

            List<BaseCharacter> possibleAITargetsAttack = new List<BaseCharacter>();
            List<BaseCharacter> possibleAITargetsSupport = new List<BaseCharacter>();
            possibleAITargets.Clear();

            List<BasicAbility> attackAbilities = caster.AbilityList().FindAll(abi => abi.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK);
            List<BasicAbility> supportAbilities = caster.AbilityList().FindAll(abi => abi.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT);
            bool bCharHasSupportAbis = supportAbilities.Count > 0;

            #region supportCheck
            if (bCharHasSupportAbis)
            {
                var listOfCharactersTemp = ct.parentTS.charactersInGroup;

                foreach (var character in listOfCharactersTemp)
                {
                    if (character.bIsAlive)
                    {
                        possibleTargetCharsSupport.Add(character);
                    }
                }

                foreach (var ability in supportAbilities)
                {
                    foreach (var character in possibleTargetCharsSupport)
                    {
                        if (character.NeedsHealing(3))
                        {
                            abilityRangesSupport.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, CombatProcessor.zoneTiles, (character.position / 64).ToPoint().ToVector2() * 64));
                            Vector2 positionGridCaster = new Vector2((int)(caster.position.X / 64), (int)(caster.position.Y / 64));
                            var test = abilityRangesSupport[abilityRangesSupport.Count - 1].Find(t => moveRadius.Contains(t) || (t.positionGrid.X == positionGridCaster.X && t.positionGrid.Y == positionGridCaster.Y));
                            if (test != null)
                            {
                                if (!possibleAITargetsSupport.Contains(character))
                                {
                                    possibleAITargetsSupport.Add(character);
                                }
                            }
                        }

                    }
                }

            }


            #endregion

            var listOfCharacters = ct.parentTS.targetGroups[GamePlayUtility.Randomize(0, ct.parentTS.targetGroups.Count - 1)].charactersInGroup;

            if (attackAbilities.Count != 0)
            {
                foreach (var character in listOfCharacters)
                {
                    if (character.bIsAlive)
                    {
                        possibleTargetCharsAttack.Add(character);
                    }

                }
            }

            if (possibleAITargetsSupport.Count == 0 && attackAbilities.Count != 0)
            {
                bCharHasSupportAbis = false; //If nothing is in need of healing, try prioiritize attacking instead.
            }


            foreach (var ability in attackAbilities)
            {
                if (ability.IsAbilityAvailable(caster.trueSTATChart()))
                {
                    if (ability.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.MELEE)
                    {
                        foreach (var character in possibleTargetCharsAttack)
                        {

                            abilityRangesAttack.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, CombatProcessor.zoneTiles, (character.position / 64).ToPoint().ToVector2() * 64));
                            Vector2 positionGridCaster = new Vector2((int)(caster.position.X / 64), (int)(caster.position.Y / 64));
                            var test = abilityRangesAttack[abilityRangesAttack.Count - 1].Find(t => moveRadius.Contains(t) || (t.positionGrid.X == positionGridCaster.X && t.positionGrid.Y == positionGridCaster.Y));
                            if (test != null)
                            {
                                if (!possibleAITargetsAttack.Contains(character))
                                {
                                    possibleAITargetsAttack.Add(character);
                                }
                            }
                        }

                    }
                    else if (ability.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.RANGED || ability.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.MAGIC)
                    {
                        abilityRangesAttack.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, CombatProcessor.zoneTiles, (caster.position / 64).ToPoint().ToVector2() * 64));

                        foreach (var item in possibleTargetCharsAttack)
                        {
                            if (ability.abilityCanHitTargetInRange(caster, item))
                            {
                                if (!possibleAITargetsAttack.Contains(item))
                                {
                                    possibleAITargetsAttack.Add(item);
                                }
                            }
                        }
                    }
                    //else if (ability.abilityFightStyle == (int)BasicAbility.ABILITY_CAST_TYPE.RANGED)
                    //{
                    //    abilityRanges.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, CombatProcessor.zoneTiles, (caster.position / 64).ToPoint().ToVector2() * 64));

                    //    foreach (var item in heroCharactersStillAlive)
                    //    {
                    //        if (ability.abilityCanHitTargetInRange(caster, item))
                    //        {
                    //            if (!possibleAITargets.Contains(item))
                    //            {
                    //                possibleAITargets.Add(item);
                    //            }
                    //        }
                    //    }
                    //}


                }
            }

            if (!bCharHasSupportAbis)
            {
                possibleAITargets = new List<BaseCharacter>(possibleAITargetsAttack);
            }
            else
            {
                bool bShouldSupport = false;
                float percentageSupportAbis = (float)((float)supportAbilities.Count / (float)caster.AbilityList().Count);
                float percentageAttackAbis = 1 - percentageSupportAbis;
                percentageSupportAbis *= 100f;
                percentageAttackAbis *= 100f;
                int amountOfEnemies = possibleAITargetsAttack.Count();
                int amountOfAlliesInNeed = possibleAITargetsSupport.Count();
                if (amountOfAlliesInNeed > amountOfEnemies)
                {
                    int percent = GamePlayUtility.Randomize(30, (int)(percentageSupportAbis + 20));
                    if (percent > GamePlayUtility.Randomize(40, 70))
                    {
                        bShouldSupport = true;
                    }
                }

                if (bShouldSupport)
                {
                    possibleAITargets = new List<BaseCharacter>(possibleAITargetsSupport);
                    bDoSupport = true;
                }
                else if (possibleTargetCharsAttack.Count != 0)
                {
                    possibleAITargets = new List<BaseCharacter>(possibleAITargetsAttack);
                    bDoSupport = false;
                }
            }
        }

        internal static BaseCharacter SelectTargetBasedOnBehaviour(List<KeyValuePair<BaseCharacter, int>> lbci)
        {
            int randomnessFactor = 10;
            int biggestThread = lbci.Max(bci => bci.Value);
            List<BaseCharacter> characterToChooseFrom = new List<BaseCharacter>();
            foreach (var item in lbci)
            {
                if ((item.Value + randomnessFactor) >= biggestThread)
                {
                    characterToChooseFrom.Add(item.Key);
                }
            }



            return characterToChooseFrom[GamePlayUtility.Randomize(0, characterToChooseFrom.Count)];
        }

        internal static void Report()
        {
            foreach (var item in possibleAITargets)
            {
                Console.WriteLine("Characters generated threat from battle: " + item.returnTotalThreat() + "     from " + item.CharacterName);
            }
        }

        internal static void Report(List<KeyValuePair<BaseCharacter, int>> lbci)
        {
            foreach (var item in lbci)
            {
                Console.WriteLine("Characters generated threat from battle: " + item.Value + "     from " + item.Key.CharacterName);
            }
        }

        internal static KeyValuePair<BasicTile, BasicAbility> returnFavoredAttackAndPosition(KeyValuePair<List<List<BasicTile>>, List<BasicAbility>> llbtlba, BaseCharacter caster, BaseCharacter target)
        {


            List<BasicAbility> listOfNonDefaultAbilities = llbtlba.Value.FindAll(ba => !ba.abilityName.Equals(caster.CCC.AIDefaultAttack.abilityName) && ba.IsAbilityAvailable(caster.trueSTATChart()));
            BasicAbility randomNonDefaultAbility = new BasicAbility();
            BasicAbility finalAbility = null;


            if (listOfNonDefaultAbilities.Count != 0)
            {
                randomNonDefaultAbility = listOfNonDefaultAbilities[GamePlayUtility.Randomize(0, listOfNonDefaultAbilities.Count)];
                int randomChance = GamePlayUtility.Randomize(0, 101);
                if (randomChance <= randomNonDefaultAbility.castChance)
                {
                    finalAbility = randomNonDefaultAbility;
                }
            }

            if (finalAbility == default(BasicAbility) && llbtlba.Value.Find(ba => ba.abilityIdentifier == caster.CCC.AIDefaultAttack.abilityIdentifier) != default(BasicAbility))
            {
                finalAbility = caster.CCC.AIDefaultAttack;
            }
            else
            {
                finalAbility = listOfNonDefaultAbilities[GamePlayUtility.Randomize(0, listOfNonDefaultAbilities.Count - 1)];
            }

            Console.WriteLine(llbtlba.Value.IndexOf(llbtlba.Value.Find(ba => ba.abilityName.Equals(finalAbility.abilityName))));

            if (llbtlba.Value.Count == 0 && llbtlba.Key.Count != 0)
            {
                while (llbtlba.Value.Count != llbtlba.Key.Count)
                {
                    llbtlba.Value.Add(finalAbility);
                }

            }

            //if(!finalAbility.abilityName.Equals(llbtlba.Value[0].abilityName, StringComparison.OrdinalIgnoreCase))
            //{
            //    finalAbility = llbtlba.Value[0];
            //}

            List<BasicTile> AbilityLocationList = llbtlba.Key[llbtlba.Value.IndexOf(llbtlba.Value.Find(ba => ba.abilityIdentifier == finalAbility.abilityIdentifier))];


            if (AbilityLocationList.Find(bt => bt.mapPosition.Location.ToVector2() == caster.position) != null)
            {
                //    AbilityLocationList.Remove(AbilityLocationList.Find(bt => bt.mapPosition.Location.ToVector2() == caster.position));
            }
            BasicTile randomDestinationTile;

            if (AbilityLocationList.Count != 0)
            {
                randomDestinationTile = AbilityLocationList[GamePlayUtility.Randomize(0, AbilityLocationList.Count)];
            }
            else
            {
                randomDestinationTile = GameProcessor.loadedMap.possibleTiles(CombatProcessor.zone.returnBoundingZone()).Find(t => t.mapPosition == caster.spriteGameSize);
            }

            return new KeyValuePair<BasicTile, BasicAbility>(randomDestinationTile, finalAbility);
        }
    }
}
