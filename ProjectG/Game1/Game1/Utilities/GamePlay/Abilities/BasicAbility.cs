using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;

using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW
{
    /// <summary>
    /// statmodifiers are lasting modifiers that are applied to the character once the ability -succesfully- hits. Abilitymodifiers are modifiers that are applied only before and during 
    /// the ability and it's attack, these do not last multiple rounds. 
    /// Example: Titan's Bash: +2 STR Ability, after this ability hits the giving character will receive a +1 DEF Bonus for 2 rounds.
    /// The +2 STR is an abilityModifier, the +1 DEF bonus is a statModifier.
    /// </summary>
    [XmlRoot("Ability")]
    public class BasicAbility
    {
        public enum ABILITY_TYPE { ATTACK = 0, SUPPORT }
        public enum ABILITY_CAST_TYPE { MELEE = 0, RANGED, MAGIC }
        public enum ABILITY_AFFINITY { None, Physical, Holy, Darkness, Fire, Water, Earth, Wind, Thunder, Nature }

        [XmlElement("Modifier name")]
        public String modifierName = "Modifier name";
        [XmlElement("Modifier ID")]
        public int modifierID = 0;
        [XmlElement("Stat modifier")]
        public BaseModifier statModifier = new BaseModifier();
        [XmlElement("Enemy Stat modifier")]
        public BaseModifier EnemyStatModifier = new BaseModifier();
        [XmlElement("Ability modifier")] //Like heal +3 HP, use here
        public STATChart abilityModifier = new STATChart(true);
        [XmlElement("Ability level")]
        public int abilityLevel = 0;
        [XmlElement("Ability level MAX")]
        public int maxAbilityLevel = 0;
        [XmlElement("Ability MANA Cost")]
        public int AbilityManaCost = 0;
        [XmlElement("Ability AP Cost")]
        public int AbilityAPCost = 0;
        [XmlElement("Ability Hit Chance")]
        public int AbilityHitChance = 0;
        [XmlElement("Ability Crit Chance")]
        public int AbilityCritChance = 0;

        [XmlElement("Ability Affinity")]
        public ABILITY_AFFINITY affinity = ABILITY_AFFINITY.Physical;
        [XmlElement("Ability Type")]
        public int abilityType = (int)ABILITY_TYPE.ATTACK;
        [XmlElement("Ability Fight Style")]
        public int abilityFightStyle = (int)ABILITY_CAST_TYPE.MELEE;
        [XmlElement("Ability Name")]
        public String abilityName = "Struggle";
        [XmlElement("Can target self")]
        public bool bCanOnlyTargetSelf = false;
        [XmlElement("Can not target self")]
        public bool bCanNotTargetSelf = false;
        /// <summary>
        /// 0 = can only target itself
        /// </summary>
        [XmlElement("Ability Minimum range")]
        public int abilityMinRange = 0;
        [XmlElement("Ability MAximum range")]
        public int abilityMaxRange = 1;
        [XmlElement("Ability cooldown")]
        public int abilityCooldown = 1;
        [XmlElement("Ability is a debuff")]
        public bool bAbilityHasDebuff = false;

        [XmlElement("Ability is AOE")]
        public bool bIsAOE = false;
        [XmlElement("Ability AOE Radius")]
        public int AOEradius = 1;
        [XmlElement("Ability AOE Modifier")]
        public BaseModifier abiAOEM = new BaseModifier();
        [XmlElement("Ability AOE Modifier ActiveStats")]
        public ActiveStatModifier abiAOEMAS = new ActiveStatModifier();

        [XmlElement("Ability can be AI Ability")]
        public bool bCanBeAIAbility = false;
        [XmlElement("Zone Level")]
        public int zoneLevel = 0;
        [XmlElement("Priority Chance")]
        public int castChance = 10;
        [XmlElement("Castable targets")]
        public List<BaseClass.CLASSType> targetableTypes = new List<BaseClass.CLASSType>();
        [XmlElement("Minimum Zone level to use ability")]
        public int minZoneLevel = 0;

        [XmlElement("Ability Descriptions per level")]
        public String abilityDescription = "";
        [XmlElement("Ability icon")]
        public ShapeAnimation abilityIcon = new ShapeAnimation();

        [XmlElement("Ability required class Level")]
        public int reqClassLevel = 0;

        [XmlElement("Ability identifier")]
        public int abilityIdentifier = 0;

        [XmlElement("Ability Particl Animation ID")]
        public int abilityPAID = 0;
        [XmlElement("Ability base damage")]
        public int baseDamage = 1;
        [XmlElement("Ability Script Location")]
        public String scriptLoc = "";
        [XmlElement("Scipt function")]
        public String scriptFunction = "";
        [XmlElement("Ability can target ground")]
        public bool bGroundTargetable = false;
        [XmlElement("Ability mod function name")]
        public String modFunctionName = "";
        [XmlElement("Sound Effect Execute")]
        public String soundEffectLoc = "";

        internal SoundEffect executeSoundEffect = null;
        internal SoundEffectInstance playableSound = null;

        [XmlIgnore]
        public int abilityCooldownTimer = 0;
        [XmlIgnore]
        public ParticleAnimation PAanim = new ParticleAnimation();

        internal NLua.Lua script;
        internal LUA.LuaAbilityModArea lama = null;
        internal List<BasicTile> lamaTiles = new List<BasicTile>();
        internal NLua.LuaFunction modFunction = null;
        internal bool bPlayedSE = false;

        public BasicAbility()
        {

        }

        public void CreateAbility()
        {
            //statModifier.Add(new BaseModifier());
            //EnemyStatModifier.Add(new BaseModifier());
            //abilityModifier.Add(new STATChart(true));
            //AbilityManaCost.Add(0);
            //AbilityAPCost.Add(0);
            //AbilityCritChance.Add(0);
            //AbilityHitChance.Add(0);
        }

        public void Reset()
        {
            abilityCooldownTimer = 0;
        }

        public void TickAbilityCoolDown()
        {
            if (abilityCooldownTimer > 0)
            {
                abilityCooldownTimer--;
            }
        }

        public void TickAbilityCoolDown(int i)
        {
            if (abilityCooldownTimer > 0)
            {
                abilityCooldownTimer -= i;
            }
            if (abilityCooldownTimer < 0)
            {
                abilityCooldownTimer = 0;
            }
        }

        public bool IsAbilityAvailable(STATChart ts)
        {
            if (abilityCooldownTimer == 0 && ts.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA] >= currentMPCost() && ts.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP] >= currentAPCost())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAbilityAvailable(BaseCharacter bc)
        {
            STATChart ts = bc.trueSTATChart();
            if (abilityCooldownTimer == 0 && ts.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA] >= currentMPCost() && ts.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP] >= currentAPCost())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReloadTexture()
        {
            try
            {
                abilityIcon.ReloadTexture();

            }
            catch
            {
            }
        }

        public void ReloadGCDB(GameContentDataBase gcdb)
        {
            try
            {
                if (abilityPAID != -1)
                {
                    var temp = gcdb.gameParticleAnimations.Find(PA => PA.particleAnimationID == abilityPAID);
                    if (temp != null)
                    {
                        PAanim = temp.Clone();
                    }
                }
                else
                {
                    PAanim = null;
                }

            }
            catch
            {
            }

            try
            {
                executeSoundEffect = Game1.contentManager.Load<SoundEffect>(soundEffectLoc);
            }
            catch (Exception e)
            {

            }
        }

        public bool CanAbilityLevelUp()
        {
            if (abilityLevel < maxAbilityLevel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LevelUpAbility()
        {
            if (CanAbilityLevelUp())
            {
                abilityLevel++;
            }
        }

        public override string ToString()
        {
            String temp = abilityName + ", ID: " + abilityIdentifier + "\n Used by: ";
            foreach (var item in MapBuilder.gcDB.gameClasses)
            {
                if (item.classAbilities.Find(ba => ba.abilityIdentifier == abilityIdentifier) != default(BasicAbility))
                {
                    temp += item.ClassName + ", ";
                }
            }
            if (temp.EndsWith(", ", StringComparison.OrdinalIgnoreCase))
            {
                temp = temp.Substring(0, temp.Length - 2);
            }
            temp += ", MaxLevel: " + maxAbilityLevel;
            return temp;
        }

        public bool CanTargetSelf()
        {
            return !bCanNotTargetSelf;
        }

        public int currentMPCost()
        {
            return AbilityManaCost;
        }

        public int currentAPCost()
        {
            return AbilityAPCost;
        }

        public int currentHitChance()
        {
            return AbilityHitChance;
        }

        public int currentCritChance()
        {
            return AbilityCritChance;
        }

        public STATChart currentAbilityModifier()
        {
            return abilityModifier;
        }

        public BaseModifier currentStatModifier()
        {
            return statModifier;
        }

        public BaseModifier AppliedStatModifier()
        {
            var temp = statModifier.Clone();
            temp.modifierName = modifierName;
            return temp;
        }

        public bool bCanHitTarget(BaseCharacter caster, BaseCharacter target, List<BasicTile> bt)
        {
            if (bCanNotTargetSelf && caster == target)
            {
                return false;
            }

            if (IsAbilityAvailable(caster.trueSTATChart()))
            {
                if (MapListUtility.returnValidMapRadius(abilityMinRange, abilityMaxRange, bt, target.position).Find(t => t.mapPosition.Location == caster.position.ToPoint()) != null)
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

        public bool abilityCanHitTargetInRange(BaseCharacter caster, BaseCharacter target)
        {
            if (bCanNotTargetSelf && caster == target)
            {
                return false;
            }

            Point casterPos = (caster.position / 64).ToPoint();
            Point targetPos = (target.position / 64).ToPoint();
            Point temp = casterPos - targetPos;
            int distance = Math.Abs(temp.X) + Math.Abs(temp.Y);
            if (distance >= abilityMinRange && distance <= abilityMaxRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EditorAddLevel()
        {
            //maxAbilityLevel++;
            //statModifier.Add(new BaseModifier());
            //EnemyStatModifier.Add(new BaseModifier());
            //abilityModifier.Add(new STATChart(true));
            //AbilityManaCost.Add(0);
            //AbilityAPCost.Add(0);
            //AbilityCritChance.Add(0);
            //AbilityHitChance.Add(0);
        }

        public void EditorRemoveLevel()
        {
            //maxAbilityLevel--;
            //statModifier.RemoveAt(statModifier.Count - 1);
            //EnemyStatModifier.RemoveAt(EnemyStatModifier.Count - 1);
            //abilityModifier.RemoveAt(abilityModifier.Count - 1);
            //AbilityManaCost.RemoveAt(AbilityManaCost.Count - 1);
            //AbilityAPCost.RemoveAt(AbilityAPCost.Count - 1);
            //AbilityCritChance.RemoveAt(AbilityCritChance.Count - 1);
            //AbilityHitChance.RemoveAt(AbilityHitChance.Count - 1);
        }

        public String ReturnAbilityHitChance(BaseCharacter caster)
        {
            int chance = caster.trueSTATChart().currentSpecialStats[(int)STATChart.SPECIALSTATS.EXTRA_HIT_CHANCE] + currentHitChance() + currentAbilityModifier().currentSpecialStats[(int)STATChart.SPECIALSTATS.EXTRA_HIT_CHANCE];
            return chance.ToString();
        }

        public String ReturnAbilityCritChance(BaseCharacter caster)
        {
            int chance = caster.trueSTATChart().currentSpecialStats[(int)STATChart.SPECIALSTATS.EXTRA_BASE_CRIT] + currentCritChance() + currentAbilityModifier().currentSpecialStats[(int)STATChart.SPECIALSTATS.EXTRA_BASE_CRIT];
            return chance.ToString();
        }

        public int ReturnAbilityHitChanceNum(BaseCharacter caster)
        {
            int chance = caster.trueSTATChart().currentSpecialStats[(int)STATChart.SPECIALSTATS.EXTRA_HIT_CHANCE] + currentHitChance() + currentAbilityModifier().currentSpecialStats[(int)STATChart.SPECIALSTATS.EXTRA_HIT_CHANCE];
            return chance;
        }

        public int ReturnAbilityCritChanceNum(BaseCharacter caster)
        {
            int chance = caster.trueSTATChart().currentSpecialStats[(int)STATChart.SPECIALSTATS.EXTRA_BASE_CRIT] + currentCritChance() + currentAbilityModifier().currentSpecialStats[(int)STATChart.SPECIALSTATS.EXTRA_BASE_CRIT];
            return chance;
        }

        public void ApplyAbilityCosts(BaseCharacter caster)
        {
            caster.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA] -= currentMPCost();
            caster.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP] -= currentAPCost();
            if (currentStatModifier() != default(BaseModifier))
            {
                caster.modifierList.Add(currentStatModifier());
            }
        }

        public bool isAbilityInRange(int r)
        {
            if (abilityMinRange <= r && r <= abilityMaxRange)
            {
                return true;
            }
            return false;
        }

        public void AssignZoneLevel(int zl)
        {
            zoneLevel = zl;
            abilityLevel = zl;
        }

        public bool AICanUseThisAbility()
        {
            if (zoneLevel >= minZoneLevel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public BasicAbility Clone()
        {
            BasicAbility temp = (BasicAbility)this.MemberwiseClone();
            temp.abilityIcon = abilityIcon.Clone();
            temp.statModifier = statModifier.Clone();
            temp.EnemyStatModifier = EnemyStatModifier.Clone();
            temp.abilityModifier = abilityModifier.Clone();
            //temp.AbilityManaCost = new List<int>(AbilityManaCost);
            //temp.AbilityAPCost = new List<int>(AbilityAPCost);
            //temp.AbilityHitChance = new List<int>(AbilityHitChance);
            //temp.AbilityCritChance = new List<int>(AbilityCritChance);
            temp.abiAOEM = abiAOEM.Clone();
            temp.abiAOEMAS = abiAOEMAS.Clone();
            temp.targetableTypes = new List<BaseClass.CLASSType>(targetableTypes);
            //temp.abilityDescription = "";

            return temp;
        }

        public void MakeSureAbilityIsCorrectAfterLoad()
        {
            if (abilityLevel > maxAbilityLevel)
            {
                abilityLevel = maxAbilityLevel;
                Console.WriteLine("Ability level was too high, deleted a level. Sorry.");
            }
        }

        internal String GetName()
        {
            return abilityName;
        }

        internal String GetCosts()
        {
            String temp = "";
            temp += "MP: " + currentMPCost() + " AP: " + currentAPCost() + " Range: " + abilityMinRange + "-" + abilityMaxRange;

            return temp;
        }

        internal bool CheckScript()
        {
            if (scriptFunction.Equals(""))
            {
                return false;
            }
            else if (!scriptLoc.Equals(""))
            {
                NLua.Lua state = new NLua.Lua();
                state.LoadCLRPackage();
                try
                {
                    state.DoFile(Game1.rootContent + scriptLoc);
                }
                catch (Exception)
                {
                    script = null;
                    return false;
                }

                bool bIsCorrectScriptAndFunction = state.GetFunction(scriptFunction) != null;
                if (bIsCorrectScriptAndFunction)
                {
                    script = state;
                    if (!modFunctionName.Equals(""))
                    {
                        modFunction = state.GetFunction(modFunctionName);
                    }
                    else
                    {
                        modFunction = null;
                    }

                    return true;
                }
                return false;
            }
            return false;
        }

        private void PostScriptExecute()
        {
            throw new NotImplementedException();
        }

        internal void SetAreaInfo(LUA.LuaAbilityModArea lama)
        {
            this.lama = lama;
        }

        public String GetEstimatedPotency(BaseCharacter bc)
        {
            String potencyText = "";
            int levelCheck = bc.CCC.equippedClass.classEXP.classLevel + 2;
            if (levelCheck < 0) { levelCheck = 0; }
            BaseCharacter temp = new BaseCharacter();
            STATChart tempSC = new STATChart(true);
            for (int i = 0; i < tempSC.currentActiveStats.Count; i++)
            {
                tempSC.currentActiveStats[i] = levelCheck;
            }
            for (int i = 0; i < tempSC.currentPassiveStats.Count; i++)
            {
                tempSC.currentPassiveStats[i] = levelCheck;
            }
            for (int i = 0; i < tempSC.currentSpecialStats.Count; i++)
            {
                tempSC.currentSpecialStats[i] = levelCheck;
            }
            temp.statChart = tempSC;
            var lai = LUA.LuaAbilityInfo.abiToLuaAbilityInfo(this, bc.toCharInfo(), temp.toCharInfo());
            lai.ExecuteScript();

            potencyText = lai.minDmg + " - " + lai.maxDmg;

            return potencyText;
        }

        internal void SetCoolDown()
        {
            abilityCooldownTimer = abilityCooldown;
        }

        internal bool IsOnCoolDown()
        {
            if (abilityCooldownTimer != 0)
            {
                return true;
            }
            return false;
        }

        internal void ResetCoolDown()
        {
            abilityCooldownTimer = 0;
        }

        internal bool CanCounterClass(BaseClass equippedClass)
        {
            switch ((ABILITY_CAST_TYPE)abilityFightStyle)
            {
                case ABILITY_CAST_TYPE.MELEE:
                    if (equippedClass.classType == BaseClass.CLASSType.MELEE)
                    {
                        return true;
                    }
                    break;
                case ABILITY_CAST_TYPE.RANGED:
                    if (equippedClass.classType == BaseClass.CLASSType.RANGED)
                    {
                        return true;
                    }
                    break;
                case ABILITY_CAST_TYPE.MAGIC:
                    if (equippedClass.classType == BaseClass.CLASSType.CASTER)
                    {
                        return true;
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        internal void playSoundEffect()
        {
            if (!bPlayedSE&&executeSoundEffect!=null)
            {
                if (playableSound==null)
                {
                    playableSound = executeSoundEffect.CreateInstance();
                }
                playableSound.Volume = SceneUtility.masterVolume / 100f * SceneUtility.soundEffectsVolume / 100f;

                if (playableSound.State == SoundState.Stopped) { playableSound.Stop(true); }

                playableSound.Play();
            }
            bPlayedSE = true;
        }

        internal void InitializeAbilityBeforeAttack()
        {
            bPlayedSE = false;
        }
    }

    [XmlRoot("Class ability info")]
    public class ClassAbilityInfo
    {
        [XmlElement("Ability ID")]
        public int abilityID = -1;
        [XmlElement("Required level (to unlock)")]
        public int reqClassLevel = 0;
        [XmlElement("Required class points")]
        public List<ClassPoints> classPoints = new List<ClassPoints>();
        [XmlElement("Is Unlocked")]
        public bool bIsUnlocked = false;
        [XmlElement("Script bool ID")]
        public int scriptBoolIDunlockable = -1;

        internal ScriptBool unlockReq = null;
        internal bool bUnlockable = false;
        internal BasicAbility parent;
        public ClassAbilityInfo() { }

        public ClassAbilityInfo(BasicAbility ba)
        {
            abilityID = ba.abilityIdentifier;
            parent = ba;
        }

        public bool IsAvailable(BaseClass c)
        {
            if (reqClassLevel <= c.classEXP.classLevel)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return parent.ToString();
        }
    }
}

namespace LUA
{
    public class LuaAbilityInfo
    {
        internal enum CallType { Attack, Area }

        public String abiType = "ATTACK";
        public String abiCastType = "MELEE";
        public String abiAffinity = "Physical";

        public int hitChance = 100;
        public int critChance = 0;

        public int minDmg = 0;
        public int maxDmg = 6;

        public LuaCharacterInfo caster = null;
        public LuaCharacterInfo target = null;

        public String abiName = "";
        public bool bGenerateMaxDmg = true;

        public int surrEnemiesCaster = 0;
        public int surrAliesCaster = 0;
        public int surrEnemiesTarget = 0;
        public int surrAliesTarget = 0;

        public bool bCalledForArea = false;
        public bool bAbsorbs = false;
        public bool bIsAttack = true;

        internal BasicAbility parent;


        //TODO modifiers...


        public LuaAbilityInfo() { }

        internal static LUA.LuaAbilityInfo abiToLuaAbilityInfo(TBAGW.BasicAbility ba, LuaCharacterInfo caster, LuaCharacterInfo target, CallType ct = CallType.Attack)
        {
            LuaAbilityInfo lai = new LuaAbilityInfo();
            lai.abiType = ba.abilityType.ToString();
            lai.abiCastType = ba.abilityFightStyle.ToString();
            lai.bIsAttack = ba.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK;

            lai.minDmg = ba.baseDamage;
            lai.minDmg = (int)(lai.minDmg * target.parent.combatInfo.baseVSModifier(ba));
            lai.maxDmg = (int)(lai.minDmg * 1.5f);

            lai.abiName = ba.abilityName;
            lai.target = target;

            lai.caster = caster;
            lai.target = target;

            lai.parent = ba;

            lai.hitChance = ba.ReturnAbilityHitChanceNum(caster.parent);
            lai.critChance = ba.ReturnAbilityCritChanceNum(caster.parent);

            if (caster != null)
            {
                caster.parent.combatInfo.CheckAffinity(target.parent);
            }
            if (target != null)
            {
                target.parent.combatInfo.CheckAffinity(target.parent);
            }


            if (target.parent.combatInfo.Absorbs(ba.affinity))
            {
                lai.bAbsorbs = true;
            }

            if (BattleGUI.bIsRunning)
            {
                var surroundInfo = BattleGUI.GetSurroundInfo();
                lai.surrAliesCaster = surroundInfo.gbcAllies;
                lai.surrEnemiesCaster = surroundInfo.gbcEnemies;
                lai.surrAliesTarget = surroundInfo.rbcAllies;
                lai.surrEnemiesTarget = surroundInfo.rbcEnemies;
                lai.critChance += surroundInfo.critChance;
                lai.hitChance += surroundInfo.hitChance;
            }

            switch (ct)
            {
                case CallType.Attack:
                    break;
                case CallType.Area:
                    lai.bCalledForArea = true;
                    break;
                default:
                    break;
            }

            return lai;
        }

        internal void ExecuteScript()
        {
            try
            {
                parent.CheckScript();
                (parent.script[parent.scriptFunction] as NLua.LuaFunction).Call(this);

            }
            catch (Exception e)
            {

            }

            if (minDmg > maxDmg)
            {
                maxDmg = minDmg;
            }
            if (bGenerateMaxDmg)
            {
                maxDmg = (int)(minDmg * 1.3f);
            }
        }

        public void ProcessAreaMod(LuaAbilityModArea lama)
        {
            if (lama.CanConvertArea())
            {
                lama.CanConvertArea();
                parent.SetAreaInfo(lama);
            }
            else
            {
                LuaAbilityModArea t = new LuaAbilityModArea();
                t.bHasCorrectArea = true;
                t.centerOffSet = new Point(0);
                t.areaRelLocs.Add(t.centerOffSet);
                parent.SetAreaInfo(lama);
            }
        }

        internal void HandleAbilityMod()
        {
            if (parent.modFunction != null)
            {

                var returnVal = parent.modFunction.Call(this);
                if (returnVal != null)
                {
                    if (returnVal[0].GetType() == typeof(LuaAbilityCastModCollection))
                    {
                        foreach (var item in (returnVal[0] as LuaAbilityCastModCollection).abiModList)
                        {
                            item.Process();
                        }
                    }
                }
            }
        }
    }

    public class LuaAbilityModArea
    {

        internal List<String> abilityArea = new List<string>();
        internal Point centerOffSet = new Point();
        internal bool bHasCorrectArea = false;
        internal List<Point> areaRelLocs = new List<Point>();
        public bool bCenterIncluded = true;

        public LuaAbilityModArea() { }

        public void ModifyArea(String s)
        {
            abilityArea.Add(s);
        }

        public void RemoveAreaLine(int i)
        {
            if (i >= 0 && i < abilityArea.Count)
            {
                abilityArea.RemoveAt(i);
            }
        }

        internal bool CanConvertArea()
        {
            for (int i = 0; i < abilityArea.Count; i++)
            {
                if (abilityArea[i].ToLower().Contains('o'))
                {
                    bHasCorrectArea = true;
                    centerOffSet = new Point(abilityArea[i].ToLower().IndexOf('o'), i);
                    ConvertArea();
                    return true;
                }

            }
            return false;
        }

        private void ConvertArea()
        {
            areaRelLocs.Clear();
            for (int i = 0; i < abilityArea.Count; i++)
            {
                for (int j = 0; j < abilityArea[i].Length; j++)
                {
                    if (abilityArea[i][j] == '*')
                    {
                        areaRelLocs.Add(new Point(j, i) - centerOffSet);
                    }
                }
            }
            if (bCenterIncluded)
            {
                areaRelLocs.Add(new Point(0, 0));//This is the center
            }
        }
    }

    public class LuaAbilityCastModCollection
    {
        public List<LuaAbilityCastMod> abiModList = new List<LuaAbilityCastMod>();

        public LuaAbilityCastModCollection() { }

        public LuaAbilityCastModCollection(LuaAbilityCastMod lacm)
        {
            abiModList.Add(lacm);
        }
    }

    public class LuaAbilityCastMod
    {
        public LuaCharacterInfo lci = new LuaCharacterInfo();
        public LuaStatEdit statMod = new LuaStatEdit();
        public int length = 2;

        public LuaAbilityCastMod(LuaCharacterInfo lci)
        {
            this.lci = lci;
        }

        internal virtual void Process()
        {
            STATChart sc = statMod.ExtractStatChart();
            BaseModifier bm = new BaseModifier();
            bm.statModifier = sc.Clone();
            bm.abilityModifierLength = length;
            lci.parent.modifierList.Add(bm);
        }

        internal LuaAbilityCastMod Clone()
        {
            LuaAbilityCastMod temp = (LuaAbilityCastMod)this.MemberwiseClone();
            return temp;
        }
    }

    public class LuaMindControl : LuaAbilityCastMod
    {
        public LuaMindControl(LuaCharacterInfo lci) : base(lci) { }

        internal override void Process()
        {
            base.Process();
            LuaTurnSetInfo ltsiTo = EncounterInfo.currentTurn().toLuaTurnSetInfo();
            LuaTurnSetInfo ltsiFrom = EncounterInfo.getTurnSetFrom(lci.parent).toLuaTurnSetInfo();
            LuaCharacterTurnInfo lcti = EncounterInfo.getCharacterTurnFrom(lci.parent).toLuaCharacterTurnInfo();

            ltsiTo.HandleChangeSide(lcti, ltsiFrom, ltsiTo, LuaTurnSetInfo.SideTurnType.MindControl);
        }
    }
}