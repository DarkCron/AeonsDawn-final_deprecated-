using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Enemy AI Info")]
    public class EnemyAIInfo
    {
        [XmlElement("Enemy name")]
        public String enemyName = "Enemy";
        [XmlElement("Character ID")]
        public int charID = -1;
        [XmlElement("Enemy base stats")]
        public STATChart enemyStats = new STATChart(true);


        internal BaseCharacter enemyCharBase
        {
            get
            {
                if (EnemyChar == null)
                {
                    EnemyChar = GameProcessor.gcDB.gameCharacters.Find(c => c.shapeID == charID);
                    if (EnemyChar == null && Game1.bIsDebug) { throw new Exception("Enemy char is null, fix this!"); }
                    EnemyChar = EnemyChar.Clone();
                    return GenerateAICharacter();
                }
                else
                {
                    return GenerateAICharacter();
                }

            }
        }

        private BaseCharacter GenerateAICharacter()
        {
            BaseCharacter temp = EnemyChar.Clone();
            temp.statChart = enemyStats.Clone();
            temp.displayName = enemyName;
            temp.weapon = EnemyWeapon;
            temp.enemyWeaponArray = enemyWeaponArray;
            temp.armour = EnemyArmor;
            temp.enemyArmourArray = enemyArmourArray;
            temp.CCC = CCC.Clone();
            temp.CCC.parent = temp;

            if (HasProperLua() && GameProcessor.bIsInGame)
            {
                ExecuteLuaCreation(ref temp);
            }

            temp.statChart.MakeSureActiveAndPassiveStatsEqual();
            temp.bIsAI = true;
            temp.eai = this;

            return temp;
        }

        private void ExecuteLuaCreation(ref BaseCharacter temp)
        {
            bool bRetry = false;
            //LUA.LuaCharacterInfo charInfo = temp.toCharInfo();

            try
            {
                LUA.LuaCharacterInfo charInfo = temp.toCharInfo();
                var attempt = luaState.GetFunction("mainCreation");
                if (luaState.GetFunction("mainCreation") == null)
                {
                    goto ImDone;
                }
                (luaState["mainCreation"] as NLua.LuaFunction).Call(charInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine("**********LUA ERROR for " + this.ToString() + "**********");
                if (Game1.bIsDebug)
                {
                    bRetry = true;
                    throw e;
                }
                Console.WriteLine(e);
                Console.WriteLine("**********END*****************");
            }

            if (Game1.bIsDebug && bRetry)
            {
                if (HasProperLua())
                {
                    ExecuteLuaCreation(ref temp);
                }
            }

        ImDone: { }
        }

        BaseCharacter EnemyChar;

        [XmlElement("Character Weapon ID")]
        public int charWeaponID = -1;
        internal BaseEquipment enemyWeapon
        {
            get
            {
                if (EnemyWeapon == null)
                {
                    EnemyWeapon = GameProcessor.gcDB.gameItems.Find(c => c.itemID == charWeaponID) as BaseEquipment;
                    if (EnemyWeapon == null && Game1.bIsDebug) { throw new Exception("Enemy Weapon is null, fix this!"); }
                    EnemyWeapon = (EnemyWeapon as BaseEquipment).Clone() as BaseEquipment;
                    return EnemyWeapon;
                }
                else
                {
                    return EnemyWeapon;
                }

            }
        }
        BaseEquipment EnemyWeapon;

        [XmlElement("Character Armor ID")]
        public int charArmorID = -1;
        internal BaseEquipment enemyArmor
        {
            get
            {
                if (EnemyArmor == null)
                {
                    EnemyArmor = GameProcessor.gcDB.gameItems.Find(c => c.itemID == charArmorID) as BaseEquipment;
                    if (EnemyArmor == null && Game1.bIsDebug) { throw new Exception("Enemy Armor is null, fix this!"); }
                    EnemyArmor = (EnemyArmor as BaseEquipment).Clone() as BaseEquipment;
                    return EnemyArmor;
                }
                else
                {
                    return EnemyArmor;
                }

            }
        }
        BaseEquipment EnemyArmor;

        [XmlElement("Enemy weapon selection")]
        public List<int> enemyWeaponArray = new List<int>();
        [XmlElement("Enemy armour selection")]
        public List<int> enemyArmourArray = new List<int>();

        [XmlElement("CCC identifier")]
        public int CCCidentifier = 0;
        [XmlIgnore]
        public CharacterClassCollection CCC = new CharacterClassCollection();

        [XmlElement]
        public int defaultAIAbilityID = -1;
        [XmlElement("Info ID")]
        public int infoID = -1;
        [XmlElement("Lua info file")]
        public String luaLoc = "";
        internal NLua.Lua luaState = null;

        public EnemyAIInfo() { }

        public EnemyAIInfo(int ID) { charID = ID; }

        public EnemyAIInfo(BaseCharacter bc, MapRegion r)
        {
            EnemyChar = bc.Clone();
            charID = bc.shapeID;
            EnemyWeapon = bc.weapon == null ? null : bc.weapon.Clone() as BaseEquipment;
            charWeaponID = bc.weaponID;
            charArmorID = bc.armourID;
            EnemyArmor = bc.armour == null ? null : bc.armour.Clone() as BaseEquipment;
            enemyStats = bc.statChart.Clone();
            CCCidentifier = bc.CCCidentifier;
            CCC = bc.CCC.Clone();
            enemyWeaponArray = new List<int>(bc.enemyWeaponArray);
            enemyArmourArray = new List<int>(bc.enemyArmourArray);
            enemyName = bc.displayName;
            infoID = r.highestEnemyID++;
        }

        public override string ToString()
        {
            String text = "";
            if (charID == -1)
            {
                text = "Char not complete!";
            }
            else
            {
                try
                {
                    text = Scenes.Editor.MapBuilder.gcDB.gameCharacters.Find(c => c.shapeID == charID).ToString();
                }
                catch (Exception)
                {
                    text = "Character ID is invalid, error! Reassign character or delete this.";
                }

            }
            return text;
        }

        public STATChart getCharacterStats()
        {
            return enemyStats;
        }

        public void Reload(GameContentDataBase gcdb)
        {
            CCC = gcdb.gameCCCs.Find(ccc => ccc.identifier == CCCidentifier).Clone();

            CCC.ReloadDefaultAIAbility(gcdb, this);

            CCC.ReloadFromDatabase(gcdb);

        }

        public bool HasProperLua()
        {
            bool temp = false;

            if (!luaLoc.Equals(""))
            {
                try
                {
                    luaState = new NLua.Lua();
                    luaState.LoadCLRPackage();
                    luaState.DoFile(Game1.rootContent + luaLoc);
                    return true;
                }
                catch (Exception)
                {

                }
            }

            return temp;
        }
    }
}
