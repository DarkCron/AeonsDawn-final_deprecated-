using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("CHAR CLASS")]
    public class BaseClass
    {
        public enum CLASSSTATPRIORITIES { NONE = 0, LOW, MEDIUM, HIGH };
        public enum CLASSType { MELEE, RANGED, CASTER };

        [XmlElement("CLASS NAME")]
        public String ClassName = "A-Class";
        [XmlElement("CLASS Location")]
        public String ClassLoc = "";
        [XmlElement("Magic User")]
        public bool bUsesMagic = false;
        [XmlElement("Energy User")]
        public bool bUsesEnergy = false;
        [XmlElement("Melee CounterAttack")]
        public bool bHasMCounter = false;
        [XmlElement("Ranged CounterAttack")]
        public bool bHasRCounter = false;
        [XmlElement("Base Threat Class")]
        public int baseThreatClass = 0;
        [XmlElement("Class Type")]
        public CLASSType classType = CLASSType.MELEE;
        [XmlElement("Armour Type Class")]
        public BaseEquipment.ARMOUR_TYPES classTypeArmour = BaseEquipment.ARMOUR_TYPES.Medium;
        [XmlElement("Class Exp")]
        public ClassExp classEXP = new ClassExp();
        [XmlElement("Class Abilities by ID")]
        public List<int> classAbilitiesIDs = new List<int>();
        [XmlElement("Class Ability infos")]
        public List<ClassAbilityInfo> classAbilityInfos = new List<ClassAbilityInfo>();
        [XmlElement("Class Identifier")]
        public int classIdentifier = 0;
        [XmlElement("Script loc")]
        public String scriptLoc = "";
        [XmlElement("Script function")]
        public String functionName = "";
        [XmlElement("Class stats")]
        public STATChart classStats = new STATChart(true);
        [XmlElement("Class affinity")]
        public BasicAbility.ABILITY_AFFINITY classAffinity = BasicAbility.ABILITY_AFFINITY.None;
        [XmlElement("Class unlocked")]
        public bool bIsUnlocked = true;

        internal bool bMustGenerateStatUp = true;
        internal STATChart statUp = new STATChart(true);
        internal ClassPoints classPoints = new ClassPoints();
        bool bHasProperScript = false;
        bool bRecheckScript = false;
        NLua.Lua levelUpScript;
        [XmlIgnore]
        public List<BasicAbility> classAbilities = new List<BasicAbility>();
        [XmlIgnore]
        public int classLevel = 0;

        public BaseClass()
        {


        }

        public void ReloadFromDatabase(GameContentDataBase gcDB)
        {
            var abilitiesToDelete = new List<int>();
            classAbilitiesIDs = classAbilitiesIDs.Distinct().ToList();
            foreach (var item in classAbilityInfos)
            {
                try
                {
                    classAbilities.Add(gcDB.gameAbilities.Find(abi => item.abilityID == abi.abilityIdentifier));
                    item.parent = classAbilities.Last();
                }
                catch (Exception)
                {

                }
            }

            //for (int i = 0; i < classAbilities.Count; i++)
            //{
            //    classAbilityInfos.Add(new ClassAbilityInfo(classAbilities[i]));
            //}
            //Removes all abilityIDs of abilities that don't exist anymore.
            classAbilitiesIDs.RemoveAll(id => classAbilities.Find(ca => ca.abilityIdentifier == id) == default(BasicAbility));

            classEXP.Reload(this);
        }

        public void ReloadFromSave(PlayerSaveData psd)
        {

            classEXP.Reload(this);
        }

        public BaseClass(bool b)
        {

        }

        public BaseClass(String loc)
        {
            ClassLoc = loc;

        }

        public override string ToString()
        {
            return ClassName + " CLASS";
        }

        public BaseClass Clone()
        {
            BaseClass temp = (BaseClass)this.MemberwiseClone();
            temp.classEXP = classEXP.Clone();
            temp.classAbilities = new List<BasicAbility>();
            foreach (var ability in classAbilities)
            {
                temp.classAbilities.Add(ability.Clone());
            }
            return temp;
        }

        public void AddAbility(BasicAbility ba)
        {
            classAbilities.Add(ba);
            classAbilityInfos.Add(new ClassAbilityInfo(ba));
            // classAbilitiesIDs.Add(ba.abilityIdentifier);
        }

        internal void HandleLevelup(BaseClass equippedClass, BaseCharacter currentBC)
        {
            var obj = new LUA.LuaGameClass(equippedClass, currentBC);
            (levelUpScript[functionName] as NLua.LuaFunction).Call(obj);
            if (obj != null)
            {
                ((LUA.LuaGameClass)(obj)).HandleLevelUp(equippedClass, currentBC);
            }
            bMustGenerateStatUp = true;
        }

        internal void GenerateStatUp()
        {
            if (bMustGenerateStatUp)
            {
                statUp = new STATChart(true);

                for (int i = 0; i < classStats.currentActiveStats.Count; i++)
                {
                    if (classStats.currentPassiveStats[i] != 0)
                    {
                        statUp.currentPassiveStats[i] = classStats.currentPassiveStats[i];
                        if (statUp.currentPassiveStats[i] > classEXP.classLevel + 1)
                        {
                            statUp.currentPassiveStats[i] = classEXP.classLevel + 1;
                        }
                    }
                }

                bMustGenerateStatUp = false;
            }

        }

        internal void HandleLevelUpEditorSimulation(LUA.LuaGameClass lgc)
        {
            (levelUpScript[functionName] as NLua.LuaFunction).Call(lgc);
        }

        public void AddClassLevel()
        {
        }

        public bool bCanLevelUp()
        {
            return true;
        }

        public int currentLevel()
        {
            classLevel = classEXP.classLevel;
            return classLevel;
        }

        public void CheckScript()
        {
            if (!functionName.Equals("") && levelUpScript == null && !scriptLoc.Equals("") || (bRecheckScript && !scriptLoc.Equals("")))
            {
                try
                {
                    levelUpScript = new NLua.Lua();
                    levelUpScript.LoadCLRPackage();
                    levelUpScript.DoFile(Game1.rootContent + scriptLoc);
                }
                catch (Exception e)
                {
                    levelUpScript = null;
                }
            }

            if (levelUpScript != null)
            {
                bHasProperScript = levelUpScript.GetFunction(functionName) != null;
            }
            bHasProperScript = levelUpScript != null;
        }

        public bool HasProperScript()
        {
            CheckScript();
            return bHasProperScript;
        }

        public void AddPoints(LUA.LuaClassPoints p)
        {
            try
            {
                if (p.ID == classIdentifier)
                {
                    classPoints.classID = classIdentifier;
                    classPoints.points += Math.Abs(p.amount);
                }
            }
            catch (Exception)
            {

            }
        }

        public void AddPoints(ClassPoints cp)
        {
            classPoints.classID = classIdentifier;
            classPoints.points += Math.Abs(cp.points);
        }

        public List<BasicAbility> possibleClassAbilities()
        {
            List<BasicAbility> temp = new List<BasicAbility>();

            foreach (var item in classAbilities)
            {
                ClassAbilityInfo cai = classAbilityInfos.Find(info => info.abilityID == item.abilityIdentifier);
                if (cai.IsAvailable(this))
                {
                    temp.Add(item);
                }
            }

            return temp;
        }
    }
}
