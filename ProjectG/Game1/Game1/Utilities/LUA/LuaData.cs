using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW;
using TBAGW.Utilities.Characters;

namespace LUA
{
    class LuaData
    {
        Object o;
        public LuaData(Object o) { }

        public Object returnData() { return o; }
    }

    public static class LUAUtilities
    {
        internal static List<String> errorList = new List<string>();


        public static List<object> LUATableToListUtility(Type t, NLua.LuaTable luat)
        {
            List<object> temp = new List<object>();

            foreach (var luaElement in luat.Values)
            {
                temp.Add(luaElement);
            }

            return temp;
        }

        public static void SetupLuaUtils(Random r)
        {
            NLua.Lua L = new NLua.Lua();
            L.DoFile(TBAGW.Game1.rootContent + "LUA\\" + "utils.lua");
        }

    }

    public class LuaRectangle
    {
        public int x = 0;
        public int y = 0;
        public int width = 0;
        public int height = 0;

        public LuaRectangle() { }

        public LuaRectangle(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public Rectangle MGConvert()
        {
            Rectangle temp = new Rectangle(x, y, width, height);
            return temp;
        }
    }

    public class LuaRectangleList
    {
        public List<LuaRectangle> list = new List<LuaRectangle>();

        public LuaRectangleList() { }

        public List<Rectangle> MGConvert()
        {
            List<Rectangle> temp = new List<Rectangle>();
            list.ForEach(e => temp.Add(e.MGConvert()));
            return temp;
        }
    }

    public class LuaPoint
    {
        public enum Stuff { stuff1 }
        public int x = 0;
        public int y = 0;

        public LuaPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        public LuaPoint(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }

        public LuaPoint(int i)
        {
            this.x = i;
            this.y = i;
        }

        public LuaPoint()
        { }

        internal Vector2 toVector2()
        {
            Vector2 v = new Vector2(x,y);
            return v;
        }

        public Point MGConvert()
        {
            Point p = new Point(x, y);

            return p;
        }
    }

    public class LuaFRef
    {
        public String FName = "";

        internal NLua.LuaFunction F = null;
        public LuaFRef() { }

        public void Process(NLua.Lua state)
        {
            try
            {
                F = state[FName] as NLua.LuaFunction;
            }
            catch (Exception)
            {
            }

        }
    }

    public class LuaTileRange
    {
        //In tile coordinates, not true values
        public LuaPoint start = new LuaPoint(0);
        public List<LuaPoint> range = new List<LuaPoint>();

        public LuaTileRange() { }
    }

    public class LuaClassPoints
    {
        public int amount = 0;
        public int ID = -1;

        public LuaClassPoints() { }

        public LuaClassPoints(LuaGameClass lgc, int amount)
        {
            ID = lgc.ID;
            this.amount = amount;
        }
    }

    public class LuaStatEdit
    {
        #region stat names
        //Active
        enum HealthNames { HP, Health, Life }
        enum ManaNames { MP, Mana, Magic }
        enum AmmoNames { Ammo, Munition }
        enum ShieldNames { Shield, Barrier, Block, Armor, Armour, Resistance }
        enum StoredAPNames { StoredAP, Stored_AP, Stamina, Force }

        //Passive
        enum MaxHealthNames { MAXHP, MHP, MaxHealth, MaxLife, Max_HP, Max_Health, Max_Life }
        enum MaxManaNames { MAXMP, MMP, MaxMana, MaxMagic, Max_MP, Max_Mana, Max_Magic }
        enum MaxAmmoNames { Max_Ammo, MaxAmmo, Max_Munition, MaxMunition }
        enum MaxShieldNames { Max_Shield, Max_Barrier, Max_Block, Max_Armor, Max_Armour, Max_Resistance, MaxShield, MaxBarrier, MaxBlock, MaxArmor, MaxArmour, MaxResistance }

        enum StrNames { Str, Strength, Power }
        enum DefNames { Def, Defense, defence }
        enum AgiNames { Agi, Agility, Quick }
        enum IntNames { Int, Intellect, Wisdom }
        enum ApNames { AP, ActionPoints, Action, Action_Point, Actionpoint }
        enum MobNames { Mob, Mobility, Move }
        enum MasteryNames { MAS, Mastery, Skill }

        //Special
        enum CritNames { Crit, Critical, CritChance, Crit_Chance }
        enum DodgeNames { Dodge, DodgeChance, Dodge_Chance }
        enum MPRegenNames { MPRegen, MP_Regen, Magic_Regen, MagicRegen, Mana_Regen, ManaRegen }
        enum HPRegenNames { HPRegen, HP_Regen, Health_Regen, HealthRegen, Life_Regen, LifeRegen }
        enum HitChanceNames { Precision, HitChance, Hit_Chance, Hit }
        enum FailChance { Fail, Fail_Chance, MisCast, Miscast_Chance, MiscastChance, failChance }
        #endregion

        public List<LuaStat> modifyStats = new List<LuaStat>();

        public void AddModStat(String name, int value)
        { modifyStats.Add(new LuaStat(name, value));
        }

        public void AddModStat(LuaStat ls) { modifyStats.Add(ls); }

        public LuaStatEdit() { }

        public TBAGW.STATChart ExtractStatChart()
        {
            TBAGW.STATChart temp = new TBAGW.STATChart(true);

            foreach (var item in modifyStats)
            {
                TryExtractActiveStats(ref temp, item);
                TryExtractPassiveStats(ref temp, item);
                TryExtractSpecialStats(ref temp, item);
            }

            //ActivePassiveStatsEqual(ref temp);

            return temp;
        }

        public TBAGW.STATChart ExtractStatChart(LuaStatEdit lse)
        {
            TBAGW.STATChart temp = new TBAGW.STATChart(true);

            foreach (var item in lse.modifyStats)
            {
                TryExtractActiveStats(ref temp, item);
                TryExtractPassiveStats(ref temp, item);
                TryExtractSpecialStats(ref temp, item);
            }

            // ActivePassiveStatsEqual(ref temp);

            return temp;
        }

        static internal TBAGW.KeyValuePair<Type,int> getStat(String statName)
        {
            LuaStat temp = new LuaStat(statName,5);
            LuaStatEdit coll = new LuaStatEdit();
            coll.modifyStats.Add(temp);
            STATChart sc = coll.ExtractStatChart();
            for (int i = 0; i < sc.currentActiveStats.Count; i++)
            {
                if (sc.currentActiveStats[i]!=0)
                {
                    return new TBAGW.KeyValuePair<Type, int>(typeof(STATChart.ACTIVESTATS),i);
                }
            }

            for (int i = 0; i < sc.currentPassiveStats.Count; i++)
            {
                if (sc.currentPassiveStats[i] != 0)
                {
                    return new TBAGW.KeyValuePair<Type, int>(typeof(STATChart.PASSIVESTATS), i);
                }
            }

            for (int i = 0; i < sc.currentSpecialStats.Count; i++)
            {
                if (sc.currentSpecialStats[i] != 0)
                {
                    return new TBAGW.KeyValuePair<Type, int>(typeof(STATChart.SPECIALSTATS), i);
                }
            }

            return new TBAGW.KeyValuePair<Type, int>(null,0);
        }

        private void ActivePassiveStatsEqual(ref STATChart temp)
        {
            temp.MakeSureActiveAndPassiveStatsEqual();
        }

        private bool TryExtractSpecialStats(ref STATChart temp, LuaStat ls)
        {
            foreach (var item in Enum.GetNames(typeof(CritNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentSpecialStats[(int)TBAGW.STATChart.SPECIALSTATS.EXTRA_BASE_CRIT] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(DodgeNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentSpecialStats[(int)TBAGW.STATChart.SPECIALSTATS.EXTRA_BASE_DODGE] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(MPRegenNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentSpecialStats[(int)TBAGW.STATChart.SPECIALSTATS.DOT_MANA] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(HPRegenNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentSpecialStats[(int)TBAGW.STATChart.SPECIALSTATS.DOT_HP] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(HitChanceNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentSpecialStats[(int)TBAGW.STATChart.SPECIALSTATS.EXTRA_HIT_CHANCE] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(FailChance)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentSpecialStats[(int)TBAGW.STATChart.SPECIALSTATS.EXTRA_BASE_FAIL] += ls.value;
                    return true;
                }
            }

            return false;
        }

        private bool TryExtractPassiveStats(ref STATChart temp, LuaStat ls)
        {
            foreach (var item in Enum.GetNames(typeof(MaxHealthNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.MAXHP] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(MaxManaNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.MAXMANA] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(MaxAmmoNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.MAXAMMO] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(MaxShieldNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.MAXSHIELD] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(StrNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.STR] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(DefNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.DEF] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(AgiNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.AGI] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(IntNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.INT] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(ApNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.AP] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(MobNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.MOB] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(MasteryNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentPassiveStats[(int)TBAGW.STATChart.PASSIVESTATS.MASTERY] += ls.value;
                    return true;
                }
            }

            return false;
        }

        private bool TryExtractActiveStats(ref STATChart temp, LuaStat ls)
        {
            foreach (var item in Enum.GetNames(typeof(HealthNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentActiveStats[(int)TBAGW.STATChart.ACTIVESTATS.HP] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(ManaNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentActiveStats[(int)TBAGW.STATChart.ACTIVESTATS.MANA] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(AmmoNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentActiveStats[(int)TBAGW.STATChart.ACTIVESTATS.AMMO] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(ShieldNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentActiveStats[(int)TBAGW.STATChart.ACTIVESTATS.SHIELD] += ls.value;
                    return true;
                }
            }

            foreach (var item in Enum.GetNames(typeof(StoredAPNames)))
            {
                if (item.Equals(ls.name, StringComparison.OrdinalIgnoreCase))
                {
                    temp.currentActiveStats[(int)TBAGW.STATChart.ACTIVESTATS.STORED_AP] += ls.value;
                    return true;
                }
            }
            return false;
        }
    }

    public class LuaStatRequirement : LuaStatEdit
    {
        public bool bStatsMustBeHigher = true;

        public LuaStatRequirement() : base() { }

        public bool CharacterCheck(BaseCharacter bc)
        {
            var stat = this.ExtractStatChart();
            return bc.statChart.RequirementCheckFromChar(stat, bStatsMustBeHigher);
        }
    }

    public class LuaStat
    {
        public String name = "";
        public int value = 0;

        public LuaStat() { }

        public LuaStat(String name, int value) { Assign(name, value); }

        public void Assign(String name, int value)
        {
            this.name = name;
            this.value = value;
        }
    }

    static public class LuaCombatInfo
    {
        static public float partyAverageLevel = 0f;
        static public int partySize = 0;
        static public float partyMedianLevel = 0;

        static internal void GenerateInfo()
        {
            var tempList = new List<TBAGW.Utilities.Characters.BaseCharacter>(PlayerSaveData.heroParty);
            tempList = tempList.OrderBy(c => c.CCC.equippedClass.classLevel).ToList();

            bool bSizePartyEven = tempList.Count % 2 == 0 ? true : false;
            int totalLevel = 0;
            tempList.ForEach(c => totalLevel += c.CCC.equippedClass.currentLevel());

            partyAverageLevel = totalLevel / (float)tempList.Count;
            partySize = tempList.Count;
            if (bSizePartyEven)
            {
                partyMedianLevel = (tempList[tempList.Count / 2].CCC.equippedClass.currentLevel() + tempList[(tempList.Count / 2) - 1].CCC.equippedClass.currentLevel()) / 2f;
            }
            else
            {
                partyMedianLevel = tempList[tempList.Count / 2].CCC.equippedClass.currentLevel();
            }

        }
    }

    public class LuaAttackInfo
    {
        public BaseCharacter charSide1 = null;
        public BaseCharacter charSide2 = null;
        public BaseCharacter charBehind = null;
        public BaseCharacter attacker = null;

        public BaseCharacter defender = null;
        public BaseCharacter defSide1 = null;
        public BaseCharacter defSide2 = null;
        public BaseCharacter defBehind = null;

        public BasicAbility attack = null;

        public bool bCounter = false;
        public bool bMiss = false;
        public bool bCrit = false;
        public bool bIsHeal = false;

        public int minDmg = 0;
        public int maxDmg = 0;

        public int counterMinDmg = 0;
        public int counterMaxDmg = 0;

        static public LuaAttackInfo attackInfo = new LuaAttackInfo();

        internal LuaAttackInfo() { }
    }
}
