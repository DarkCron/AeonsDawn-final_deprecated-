using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Class Collection")]
    public class CharacterClassCollection
    {
        [XmlElement("CCC identifier")]
        public int identifier = 0;
        [XmlElement("equippedClassIdentifier")]
        public int equippedClassIdentifier = 0;
        [XmlElement("class list IDs")]
        public List<int> charClassListIDs = new List<int>();
        [XmlElement("class seprate abilities IDs")]
        public List<int> charSeparateAbilityIDs = new List<int>();
        [XmlElement("class default ability")]
        public int defaultAbilityID = -1;
        [XmlElement("Equipped abilities")]
        public AbilityEquipList abiEquipList = new AbilityEquipList();
        [XmlElement("Class Experience")]
        public ClassExpCollection classExp = new ClassExpCollection();
        [XmlElement("Battle Statistics")]
        public BattleStatistics battleStatistics = new BattleStatistics();
        [XmlElement("CCC Talents")]
        public List<BaseTalentSlot> baseTalentSlot = new List<BaseTalentSlot>();
        [XmlElement("Latest Identifier Talents")]
        public int LatestID = 0;

        internal List<BaseTalentSlot> actualTalentSlots = new List<BaseTalentSlot>();
        ClassCollectionSaveData saveData = new ClassCollectionSaveData();
        [XmlIgnore]
        public BasicAbility AIDefaultAttack = new BasicAbility();
        [XmlIgnore]
        public BaseClass equippedClass = new BaseClass();
        [XmlIgnore]
        public List<BasicAbility> charSeparateAbilities = new List<BasicAbility>();
        [XmlIgnore]
        public List<BaseClass> charClassList = new List<BaseClass>();

        internal BaseCharacter parent;

        public CharacterClassCollection() { }

        public void ReloadFromDatabase(GameContentDataBase gcDB)
        {
            #region charClassList
            var classesToDelete = new List<int>();
            charClassListIDs = charClassListIDs.Distinct().ToList();
            foreach (var item in charClassListIDs)
            {
                try
                {
                    charClassList.Add(gcDB.gameClasses.Find(cl => cl.classIdentifier == item).Clone());
                }
                catch (Exception)
                {

                }
            }
            //Removes all abilityIDs of abilities that don't exist anymore.
            charClassListIDs.RemoveAll(id => charClassList.Find(cl => cl.classIdentifier == id) == default(BaseClass));
            #endregion

            #region charSeparateAbilities
            var abilitiesToDelete = new List<int>();
            charSeparateAbilityIDs = charSeparateAbilityIDs.Distinct().ToList();
            foreach (var item in charSeparateAbilityIDs)
            {
                try
                {
                    charSeparateAbilities.Add(gcDB.gameAbilities.Find(abi => abi.abilityIdentifier == item).Clone());
                }
                catch (Exception)
                {

                }
            }
            //Removes all abilityIDs of abilities that don't exist anymore.
            charSeparateAbilityIDs.RemoveAll(id => charSeparateAbilities.Find(ca => ca.abilityIdentifier == id) == default(BasicAbility));
            #endregion

            try
            {
                equippedClass = gcDB.gameClasses.Find(gc => gc.classIdentifier == equippedClassIdentifier).Clone();
            }
            catch (Exception)
            {
                equippedClass = new BaseClass(true);
            }

            try
            {
                var temp = gcDB.gameAbilities.Find(abi => abi.abilityIdentifier == defaultAbilityID);
                if (temp == null) { temp = new BasicAbility(); }
                AIDefaultAttack = temp.Clone();
            }
            catch (Exception e)
            {

            }

            try
            {
                abiEquipList.Reload(this);
            }
            catch (Exception e)
            {

            }

            try
            {
                //classExp.Initialize(this);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error initializing class EXP");
                if (Game1.bIsDebug)
                {
                    throw e;
                }
            }

           
            ReloadTalents();
        }

        public CharacterClassCollection Clone()
        {
            CharacterClassCollection temp = (CharacterClassCollection)this.MemberwiseClone();

            temp.charClassListIDs = new List<int>(charClassListIDs);
            temp.charSeparateAbilityIDs = new List<int>(charSeparateAbilityIDs);

            temp.charClassList = new List<BaseClass>();
            foreach (var item in charClassList)
            {
                temp.charClassList.Add(item.Clone());
            }
            temp.charSeparateAbilities = new List<BasicAbility>();
            foreach (var item in charSeparateAbilities)
            {
                temp.charSeparateAbilities.Add(item.Clone());
            }

            return temp;
        }

        public List<BasicAbility> allCharacterAbilities()
        {

            List<BasicAbility> temp = equippedClass.possibleClassAbilities();

            temp.AddRange(charSeparateAbilities.FindAll(sepAbi => !temp.Select(abi => abi.abilityIdentifier).ToList().Contains(sepAbi.abilityIdentifier)));
            return temp;
        }

        internal void SaveSaveEquippedClass()
        {
            charClassList.Remove(charClassList.Find(c => c.classIdentifier == equippedClass.classIdentifier));
            charClassList.Add(equippedClass);
        }

        //All possible abalities, whether the caster has enough MP or AP is checked properly in the BattleGui preparation class
        public List<BasicAbility> possibleAbilities()
        {
            //List<BasicAbility> temp = equippedClass.classAbilities.FindAll(ca => ca.reqClassLevel <= equippedClass.classEXP.classLevel);
            //temp.AddRange(charSeparateAbilities.FindAll(sepAbi => !temp.Select(abi => abi.abilityIdentifier).ToList().Contains(sepAbi.abilityIdentifier)));
            //return temp;
            List<BasicAbility> temp = new List<BasicAbility>(abiEquipList.abilities);
            if (parent != null && parent.bIsAI)
            {
                temp.Add(AIDefaultAttack);
                temp.AddRange(equippedClass.classAbilities);
                temp = temp.Distinct().ToList();

            }
            //temp.AddRange(charSeparateAbilities.FindAll(sepAbi => !temp.Select(abi => abi.abilityIdentifier).ToList().Contains(sepAbi.abilityIdentifier)));
            return temp;
        }

        public void ReloadDefaultAIAbility(GameContentDataBase gcdb, EnemyAIInfo AII)
        {
            defaultAbilityID = AII.defaultAIAbilityID;
            try
            {
                AIDefaultAttack = gcdb.gameAbilities.Find(abi => abi.abilityIdentifier == defaultAbilityID).Clone();
            }
            catch (Exception)
            {
                Console.WriteLine("Error generating default ability info for" + AII.ToString());
                AIDefaultAttack = new BasicAbility();
            }
        }

        public ClassCollectionSaveData getClassCollectionSaveData()
        {
            saveData.GenerateSave(this);
            return saveData;
        }

        public List<BaseClass> getAvailableClasses()
        {
            return charClassList.FindAll(c => c.bIsUnlocked);
        }

        public List<BaseClass> allClassesExceptEquipped()
        {
            return charClassList.FindAll(c => equippedClass.classIdentifier != c.classIdentifier);
        }

        internal void ReloadFromSave(CharacterSaveData csd)
        {
            csd.ccsd.ReloadFromSave(csd, this);
        }

        internal void ReloadTalents()
        {
            actualTalentSlots = new List<BaseTalentSlot>(baseTalentSlot);

            foreach (var talent in baseTalentSlot)
            {
                talent.Reload(this,parent);
            }

            foreach (var talent in actualTalentSlots)
            {
                talent.Reload(this,parent);
            }

        }

        internal List<ClassPoints> getClassPointList()
        {
            List<ClassPoints> lcp = new List<ClassPoints>();
            charClassList.ForEach(c=>lcp.Add(c.classPoints));
            return lcp;
        }

        internal TalentNode[] getTalentNodesForGrid()
        {
            return actualTalentSlots.Select(ts=>ts.talentNode).ToArray();
        }

        internal TalentNode[] getEditorTalentNodesForGrid()
        {
            return baseTalentSlot.Select(ts => ts.talentNode).ToArray();
        }

        internal void PrepareTalentTreeMapSave()
        {
            actualTalentSlots = baseTalentSlot;
        }
    }

    [XmlRoot("Ability Equip List")]
    public class AbilityEquipList
    {
        [XmlElement("Max Amount of equips")]
        public int amount = 5;
        [XmlElement("ids")]
        public List<int> abilityIDs = new List<int>();


        [XmlIgnore]
        public List<BasicAbility> abilities = new List<BasicAbility>();

        public AbilityEquipList() { }

        public void Clear()
        {
            abilities.Clear();
        }

        public void Reload(CharacterClassCollection ccc)
        {
            var listToRemove = new List<int>();
            var tempList = ccc.allCharacterAbilities();
            for (int i = 0; i < abilityIDs.Count; i++)
            {
                var temp = tempList.Find(abi => abi.abilityIdentifier == abilityIDs[i]);
                if (temp == null)
                {
                    listToRemove.Add(i);
                }
                else
                {
                    abilities.Add(temp);
                }
            }

            if (listToRemove.Count != 0)
            {
                listToRemove.Reverse();

                for (int i = 0; i < listToRemove.Count; i++)
                {
                    abilityIDs.RemoveAt(listToRemove[i]);
                }
            }

        }

        public void Remove(BasicAbility ba)
        {
            if ((abilities.Find(abi => abi.abilityIdentifier == ba.abilityIdentifier) != null) && abilities.Count > 1)
            {
                int index = abilities.IndexOf(abilities.Find(abi => abi.abilityIdentifier == ba.abilityIdentifier));
                abilityIDs.RemoveAt(index);
                abilities.RemoveAt(index);
            }
        }

        public void Add(BasicAbility ba)
        {
            if (!abilities.Contains(ba) && abilities.Find(abi => abi.abilityIdentifier == ba.abilityIdentifier) == null)
            {
                abilityIDs.Add(ba.abilityIdentifier);
                abilities.Add(ba);
            }

        }

        public bool CanAddAbility(BasicAbility ba)
        {
            if (abilities.Contains(ba))
            {
                return false;
            }
            if (abilities.Count < amount)
            {
                return true;
            }
            return false;
        }

        internal void ReloadFromSave(CharacterSaveData csd)
        {
            csd.ccsd.postLoadAbiEquipList.abilityIDs.Clear();
            csd.ccsd.postLoadAbiEquipList.abilities.Clear();

            for (int i = 0; i < csd.ccsd.abilityEquipList.abilityIDs.Count; i++)
            {
                csd.ccsd.postLoadAbiEquipList.abilityIDs.Add(csd.ccsd.abilityEquipList.abilityIDs[i]);
                csd.ccsd.postLoadAbiEquipList.abilities.Add(GameProcessor.gcDB.gameAbilities.Find(abi => abi.abilityIdentifier == csd.ccsd.abilityEquipList.abilityIDs[i]));
            }
        }
    }

    [XmlRoot("Class Experience Collection")]
    public class ClassExpCollection
    {
        [XmlElement("Saved EXP")]
        public List<ClassExperience> savedEXP = new List<ClassExperience>();



        public ClassExpCollection()
        {

        }

        static internal ClassExpCollection Initialize(CharacterClassCollection ccc)
        {
            ClassExpCollection temp = new ClassExpCollection();
            for (int i = 0; i < ccc.charClassList.Count; i++)
            {
                temp.savedEXP.Add(new ClassExperience(ccc.charClassList[i]));
            }

            return temp;
        }

        internal void ReloadFromSave(CharacterSaveData csd)
        {
            for (int i = 0; i < savedEXP.Count; i++)
            {
                savedEXP[i].ReloadFromSaveFile(csd);
            }
        }
    }

    [XmlRoot("Class experience")]
    public class ClassExperience
    {
        [XmlElement("EXP")]
        public int exp = 0;
        [XmlElement("level")]
        public int level = 0;
        [XmlElement("class ID")]
        public int classID = -1;
        [XmlElement("saved stats")]
        public STATChart stats = new STATChart(true);
        [XmlElement("Class points")]
        public ClassPoints classPoints = new ClassPoints();
        [XmlElement("Is unlocked")]
        public bool bIsUnlocked;

        BaseClass baseClass;

        public ClassExperience() { }

        public ClassExperience(BaseClass c)
        {
            baseClass = c;
            exp = c.classEXP.totalExp;
            level = c.classEXP.classLevel;
            classID = c.classIdentifier;
            stats = c.classStats;
            bIsUnlocked = c.bIsUnlocked;

            classPoints = ClassPoints.Copy(c);
        }

        internal void ReloadFromSaveFile(CharacterSaveData csd)
        {
            for (int i = 0; i < csd.ccsd.postLoadCCC.charClassList.Count; i++)
            {
                var c = csd.ccsd.postLoadCCC.charClassList[i];
                c.classEXP.totalExp = exp;
                c.classEXP.classLevel = level;
                ClassPoints.PasteBackFromSave(classPoints, c);
                c.classStats = stats;
                c.bIsUnlocked = bIsUnlocked;
                c.classEXP.CalculateResetExpAndLevels();
            }

        }
    }

    [XmlRoot("Class Collection Save Data")]
    public class ClassCollectionSaveData
    {
        [XmlElement("Class Exp")]
        public ClassExpCollection classExp = new ClassExpCollection();
        [XmlElement("Ability Equip List")]
        public AbilityEquipList abilityEquipList = new AbilityEquipList();
        [XmlElement("Talen info save")]
        public List<BaseTalentSlot> actualTalentSlots = new List<BaseTalentSlot>();

        internal CharacterClassCollection postLoadCCC = null;
        internal AbilityEquipList postLoadAbiEquipList = null;

        public void GenerateSave(CharacterClassCollection ccc)
        {
            classExp = ClassExpCollection.Initialize(ccc);
            abilityEquipList = ccc.abiEquipList;
            actualTalentSlots = new List<BaseTalentSlot>(ccc.baseTalentSlot);
        }

        internal void ReloadFromSave(CharacterSaveData csd, CharacterClassCollection ccc)
        {
            postLoadCCC = ccc;
            postLoadAbiEquipList = ccc.abiEquipList;

            classExp.ReloadFromSave(csd);
            abilityEquipList.ReloadFromSave(csd);
            ccc.actualTalentSlots = actualTalentSlots;
            ccc.ReloadTalents();
        }
    }

    [XmlRoot("Battle statistics")]
    public class BattleStatistics
    {
        [XmlElement("AoE Attacks")]
        public int aoeAmount = 0;
        [XmlElement("Hits")]
        public int hitAmount = 0;
        [XmlElement("Misses")]
        public int missAmount = 0;
        [XmlElement("Crits")]
        public int critAmount = 0;
        [XmlElement("Damage Done")]
        public int dmgAmount = 0;
        [XmlElement("Healing Done")]
        public int healingAmount = 0;
        [XmlElement("KO Blows")]
        public int KOAmount = 0;
        [XmlElement("Times KO'ed")]
        public int KOedAmount = 0;

        internal int aoeAmountThisFight = 0;
        internal int hitAmountThisFight = 0;
        internal int missAmountThisFight = 0;
        internal int critAmountThisFight = 0;
        internal int dmgAmountThisFight = 0;
        internal int healingAmountThisFight = 0;
        internal int KOAmountThisFight = 0;
        internal int KOedAmountThisFight = 0;

        public BattleStatistics() { }

        internal void AddAoE()
        {
            aoeAmount++;
            aoeAmountThisFight++;
        }

        internal void AddHit()
        {
            hitAmount++;
            hitAmountThisFight++;
        }

        internal void AddMiss()
        {
            missAmount++;
            missAmountThisFight++;
        }

        internal void AddCrit()
        {
            critAmount++;
            critAmountThisFight++;
        }

        internal void AddDMG(int dmg)
        {
            dmgAmount += dmg;
            dmgAmountThisFight += dmg;
        }

        internal void AddHealing(int healing)
        {
            healingAmount += healing;
            healingAmountThisFight += healing;
        }

        internal void AddKO()
        {
            KOAmount++;
            KOAmountThisFight++;
        }

        internal void AddKOed()
        {
            KOedAmount++;
        }
    }
}
