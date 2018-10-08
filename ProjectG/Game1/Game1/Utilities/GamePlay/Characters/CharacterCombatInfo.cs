using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Character Additional Combat Info")]
    public class CharacterCombatInfo
    {
        [XmlElement("Affinity Effects")]
        public List<AffinityEffect> affinityEffects = new List<AffinityEffect>();
        [XmlIgnore]
        public BasicAbility.ABILITY_AFFINITY mainAffinity = BasicAbility.ABILITY_AFFINITY.None;

        internal BaseCharacter parent;

        internal void CheckAffinity()
        {
            mainAffinity = parent.CCC.equippedClass.classAffinity;
        }


        internal void CheckAffinity(BaseCharacter bc)
        {
            parent = bc;
            mainAffinity = parent.CCC.equippedClass.classAffinity;
        }

        internal bool IsWeakTo(BasicAbility.ABILITY_AFFINITY affinity)
        {
            if (affinityEffects.Count == 0)
            {
                return false;
            }
            else
            {
                var effect = affinityEffects.Find(eff => eff.parent == affinity);
                if (effect == default(AffinityEffect))
                {
                    return false;
                }
                return effect.bIsWeakness;
            }
        }

        internal bool IsStrongTo(BasicAbility.ABILITY_AFFINITY affinity)
        {
            if (affinityEffects.Count == 0)
            {
                return false;
            }
            else
            {
                var effect = affinityEffects.Find(eff => eff.parent == affinity);
                if (effect == default(AffinityEffect))
                {
                    return false;
                }
                return effect.bIsStrongAgainst;
            }
        }

        internal bool Absorbs(BasicAbility.ABILITY_AFFINITY affinity)
        {
            if (affinityEffects.Count == 0)
            {
                return false;
            }
            else
            {
                var effect = affinityEffects.Find(eff => eff.parent == affinity);
                if (effect == default(AffinityEffect))
                {
                    return false;
                }
                return effect.bAbsorbs;
            }
        }

        internal float baseVSModifier(BasicAbility ba)
        {
            BasicAbility.ABILITY_AFFINITY affinity = ba.affinity;
            var temp = affinityEffects.Find(aff => aff.parent == affinity);

            if (temp != null)
            {
                return temp.modifier;
            }
            else
            {
                return AffinityCounter(affinity);
            }
        }

        internal float baseVSModifier(BasicAbility.ABILITY_AFFINITY affinity)
        {
            var temp = affinityEffects.Find(aff => aff.parent == affinity);

            if (temp != null)
            {
                return temp.modifier;
            }else
            {
                return AffinityCounter(affinity);
            }
        }

        private float AffinityCounter(BasicAbility.ABILITY_AFFINITY affinity)
        {
            if (mainAffinity == affinity)
            {
                return 0.0f;
            }
            switch (affinity)
            {
                case BasicAbility.ABILITY_AFFINITY.None:
                    return 1.0f;
                case BasicAbility.ABILITY_AFFINITY.Physical:
                    return 1.0f;
                case BasicAbility.ABILITY_AFFINITY.Holy:
                    if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Darkness)
                    {
                        return 2.0f;
                    }else
                    {
                        return 1.0f;
                    }

                case BasicAbility.ABILITY_AFFINITY.Darkness:
                    if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Holy)
                    {
                        return 2.0f;
                    }
                    else
                    {
                        return 1.0f;
                    }

                case BasicAbility.ABILITY_AFFINITY.Fire:
                    if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Nature)
                    {
                        return 2.0f;
                    }
                    else if(mainAffinity == BasicAbility.ABILITY_AFFINITY.Water)
                    {
                        return 0.5f;
                    }else
                    {
                        return 1.0f;
                    }
                case BasicAbility.ABILITY_AFFINITY.Water:
                    if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Fire)
                    {
                        return 2.0f;
                    }
                    else if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Nature)
                    {
                        return 0.5f;
                    }
                    else
                    {
                        return 1.0f;
                    }
                case BasicAbility.ABILITY_AFFINITY.Nature:
                    if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Water)
                    {
                        return 2.0f;
                    }
                    else if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Fire)
                    {
                        return 0.5f;
                    }
                    else
                    {
                        return 1.0f;
                    }
                case BasicAbility.ABILITY_AFFINITY.Earth:
                    if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Fire)
                    {
                        return 1.4f;
                    }else
                    if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Nature)
                    {
                        return 1.4f;
                    }
                    else if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Water)
                    {
                        return 0.4f;
                    }
                    else
                    {
                        return 1.0f;
                    }
                case BasicAbility.ABILITY_AFFINITY.Wind:
                    if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Thunder)
                    {
                        return 0.2f;
                    }else if (mainAffinity != BasicAbility.ABILITY_AFFINITY.Holy && mainAffinity != BasicAbility.ABILITY_AFFINITY.Darkness)
                    {
                        return 1.3f;
                    }else
                    {
                        return 1.0f;
                    }
                case BasicAbility.ABILITY_AFFINITY.Thunder:
                    if (mainAffinity == BasicAbility.ABILITY_AFFINITY.Wind)
                    {
                        return 0.2f;
                    }
                    else if (mainAffinity != BasicAbility.ABILITY_AFFINITY.Holy && mainAffinity != BasicAbility.ABILITY_AFFINITY.Darkness)
                    {
                        return 1.3f;
                    }
                    else
                    {
                        return 1.0f;
                    }
                default:
                    break;
            }

            return 1.0f;
        }
    }

    [XmlRoot("Affinity Effect Info")]
    public class AffinityEffect
    {
        [XmlElement("Affinity")]
        public BasicAbility.ABILITY_AFFINITY parent = BasicAbility.ABILITY_AFFINITY.Physical;
        [XmlElement("Is Weakness")]
        public bool bIsWeakness = false;
        [XmlElement("Is Strong Against")]
        public bool bIsStrongAgainst = false;
        [XmlElement("Absorbs")]
        public bool bAbsorbs = false;
        [XmlElement("Modifier")]
        public float modifier = 1.0f;


        public AffinityEffect() { }

        public AffinityEffect(BasicAbility.ABILITY_AFFINITY af)
        {
            parent = af;
        }
    }

}
