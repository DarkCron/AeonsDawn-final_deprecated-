using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW
{
    [XmlRoot("Save Data")]
    public class PlayerSaveData
    {
        [XmlElement("Player ID")]
        public int playerIDSave = 3462143;
        [XmlElement("Player team")]
        public List<CharacterSaveData> heroTeamActive = new List<CharacterSaveData>();
        [XmlElement("Player Inventory")]
        public PlayerInventory playerInventorySave = new PlayerInventory();
        [XmlElement("Enabled Trigger Zones")]
        public List<int> enabledTZs = new List<int>();
        [XmlElement("Main Controller Position")]
        public Vector2 mainControllerPos = new Vector2();
        [XmlElement("Main Controller ID")]
        public int mainControllerID = 0;
        [XmlElement("Saved bool info non global")]
        public List<BoolSaveInfo> boolSaveInfo = new List<BoolSaveInfo>();
        [XmlElement("Heroes in current party")]
        public List<BaseCharacter> activePartySave = new List<BaseCharacter>();
        [XmlElement("Heroes in party")]
        public List<BaseCharacter> activeCompleteParty = new List<BaseCharacter>();
        [XmlElement("Saved map Location")]
        public String mapLoc = "";
        [XmlElement("Game Content Database Loc")]
        public String databaseLoc = "";
        [XmlElement("Time index")]
        public long timeIndex = 0;
        [XmlElement("World and time save info")]
        public WorldSaveInfo wsi = new WorldSaveInfo();
        [XmlElement("Save data name")]
        public String saveDataName = "";
        //TODO external global save data

        internal static List<ScriptBool> localBoolData = new List<ScriptBool>();
        public static GameText.Language gameLanguage = GameText.Language.English;
        public static List<BoolSaveInfo> playerBoolInfo = new List<BoolSaveInfo>();
        public static PlayerInventory playerInventory = new PlayerInventory();
        public static List<BaseCharacter> heroParty = new List<BaseCharacter>();
        public static int playerID = 3462143;
        internal bool bAutoSave = false;

        public List<ScriptBool> allConditionals()
        {
            List<ScriptBool> all = new List<ScriptBool>(localBoolData);
            return all;
        }

        public PlayerSaveData()
        {

        }

        public void GenerateSave()
        {
            int i = 0;
            foreach (var hero in PlayerSaveData.heroParty)
            {
                CharacterSaveData newData = new CharacterSaveData();
                newData.AdjustToCharacter(hero);
                heroTeamActive.Add(newData);
                if (hero.Equals(PlayerController.selectedSprite))
                {
                    mainControllerID = hero.shapeID;
                }
                i++;
            }

            mainControllerPos = PlayerController.selectedSprite.position;
            mapLoc = GameProcessor.loadedMap.mapLocation;
            if (mapLoc.EndsWith(".cgmapc"))
            {
                databaseLoc = mapLoc.Replace(".cgmapc", ".cgdbc");
            }
            else if (mapLoc.EndsWith(".cgmap"))
            {
                databaseLoc = mapLoc.Replace(".cgmap", ".cgdb");
            }

            timeIndex = DateTime.Now.Ticks;

            boolSaveInfo.Clear();
            for (int j = 0; j < localBoolData.Count; j++)
            {
                boolSaveInfo.Add(BoolSaveInfo.toSaveInfo(localBoolData[j]));
            }

            wsi = WorldSaveInfo.GenerateSave();
        }

        internal static ScriptBool getBool(int ID)
        {
            var temp = localBoolData.Find(b => b.boolID == ID);
            if (temp == null)
            {
                temp = GameProcessor.gcDB.gameScriptBools.Find(b => b.boolID == ID);
            }

            return temp;
        }

        internal static void AdjustBool(ScriptBool sb)
        {
            if (sb != null)
            {
                if (!sb.isGlobal)
                {
                    localBoolData.Remove(localBoolData.Find(b => b.boolID == sb.boolID));
                    localBoolData.Add(sb);
                }
            }

        }

        internal static void HandleReload(PlayerSaveData pSD)
        {
            localBoolData.Clear();
            for (int i = 0; i < pSD.boolSaveInfo.Count; i++)
            {
                var temp = GameProcessor.gcDB.gameScriptBools.Find(b => b.boolID == pSD.boolSaveInfo[i].ID).Clone();
                BoolSaveInfo.Reload(temp, pSD.boolSaveInfo[i]);
                localBoolData.Add(temp);
            }

            WorldSaveInfo.HandleReload(pSD.wsi);
        }

        private void ExtractInfoFromPlayerGameInfo()
        {
            playerIDSave = PlayerSaveData.playerID;

        }

        internal static void Reset()
        {
            localBoolData = new List<ScriptBool>();
            gameLanguage = GameText.Language.English;
            playerBoolInfo = new List<BoolSaveInfo>();
            playerInventory = new PlayerInventory();
            heroParty = new List<BaseCharacter>();
        }
    }

    [XmlRoot("Character Save Data")]
    public class CharacterSaveData
    {
        [XmlElement("Character ID")]
        public int charID = -1;
        [XmlElement("Class Experience Save Data")]
        public ClassCollectionSaveData ccsd = new ClassCollectionSaveData();

        public void HandleReloadSave(BaseCharacter bc)
        {

        }

        public void AdjustToCharacter(BaseCharacter bc)
        {
            charID = bc.shapeID;

            bc.CCC.SaveSaveEquippedClass();
            ccsd = bc.CCC.getClassCollectionSaveData();
        }
    }

    [XmlRoot("Bool Info")]
    public class BoolSaveInfo
    {
        [XmlElement("Bool ID")]
        public int ID = -1;
        [XmlElement("Bool bool")]
        public bool isTrue = false;
        [XmlElement("Choice Made")]
        public int choiceID = -1;

        internal ScriptBool sb = new ScriptBool();

        public BoolSaveInfo()
        {

        }

        static internal void Reload(ScriptBool sb, BoolSaveInfo bsi)
        {
            sb.scriptChoice = bsi.choiceID;
            sb.isOn = bsi.isTrue;
        }

        static internal BoolSaveInfo toSaveInfo(ScriptBool sb)
        {
            BoolSaveInfo bsi = new BoolSaveInfo();
            bsi.ID = sb.boolID;
            bsi.isTrue = sb.isOn;
            bsi.choiceID = sb.scriptChoice;
            return bsi;
        }
    }

    [XmlRoot("World Info Save Data")]
    public class WorldSaveInfo
    {
        public WorldSaveInfo() { }

        internal static WorldSaveInfo GenerateSave()
        {
            return new WorldSaveInfo();
        }

        internal static void HandleReload(WorldSaveInfo wsi)
        {

        }
    }
}
