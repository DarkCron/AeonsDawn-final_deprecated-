using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW
{
    [XmlRoot("Content DataBase")]
    public class GameContentDataBase
    {
        [XmlElement("Game Abilities")]
        public List<BasicAbility> gameAbilities = new List<BasicAbility>();
        [XmlElement("Game Characters")]
        public List<BaseCharacter> gameCharacters = new List<BaseCharacter>();
        [XmlElement("Game TileSheets")]
        public List<TileSheet> gameTileSheets = new List<TileSheet>();
        [XmlElement("Game Classes")]
        public List<BaseClass> gameClasses = new List<BaseClass>();
        [XmlElement("Game Class Collections")]
        public List<CharacterClassCollection> gameCCCs = new List<CharacterClassCollection>();
        [XmlElement("Game Items")]
        public List<BaseItem> gameItems = new List<BaseItem>();
        [XmlElement("Game Object Groups")]
        public List<ObjectGroup> gameObjectGroups = new List<ObjectGroup>();
        [XmlElement("Game Object Objects")]
        public List<BaseSprite> gameObjectObjects = new List<BaseSprite>();
        [XmlElement("Game Source Tile")]
        public List<TileSource> gameSourceTiles = new List<TileSource>();
        [XmlElement("Game Scripts")]
        public List<BaseScript> gameScripts = new List<BaseScript>();
        [XmlArrayItem("Game ScriptBools")]
        public List<ScriptBool> gameScriptBools = new List<ScriptBool>();
        [XmlArrayItem("Game ParticleSystems")]
        public List<ParticleSystemSource> gameParticleSystems = new List<ParticleSystemSource>();
        [XmlElement("Game SFXs")]
        public List<SFXInfo> gameSFXs = new List<SFXInfo>();
        [XmlElement("Game Lights")]
        public List<SpriteLight> gameLights = new List<SpriteLight>();
        [XmlElement("Game Particle Animations")]
        public List<ParticleAnimation> gameParticleAnimations = new List<ParticleAnimation>();
        [XmlElement("Game Magic Base Animations")]
        public List<MagicCircleBase> gameMagicCircleAnimations = new List<MagicCircleBase>();
        [XmlElement("Game Map Groups")]
        public List<String> gameMapGroups = new List<String>();
        [XmlElement("Game Text Collections")]
        public List<GameText> gameTextCollection = new List<GameText>();
        [XmlElement("Game UI Collections")]
        public List<UICollectionSave> gameUICollections = new List<UICollectionSave>();
        [XmlElement("Highest ID Characters")]
        public int characterID = 0;
        [XmlElement("Highest ID Items")]
        public int itemID = 0;
        [XmlElement("Highest ID object")]
        public int objectID = 0;
        [XmlElement("Highest ID object Group")]
        public int objectGroupIDHighest = 0;
        [XmlElement("Highest ID tile")]
        public int tileSourceID = 0;
        [XmlElement("Highest ID trigger zone")]
        public int triggerZoneID = 0;
        [XmlElement("Highest ID script")]
        public int scriptID = 0;
        [XmlElement("Highest ID scriptbool")]
        public int scriptBoolID = 0;
        [XmlElement("Highest ID particle system")]
        public int particleSystemID = 0;
        [XmlElement("Highest ID maps")]
        public int mapID = 0;
        [XmlElement("Highest ID abilities")]
        public int abilityID = 0;
        [XmlElement("Highest ID classes")]
        public int classID = 0;
        [XmlElement("Highest ID CCCs")]
        public int CCCID = 0;
        [XmlElement("Highest ID SFX")]
        public int SFXID = 0;
        [XmlElement("Highest ID Light")]
        public int LightID = 0;
        [XmlElement("Highest ID Particle Animations")]
        public int particleAnimationIDLatest = 0;
        [XmlElement("Highest ID Base Magic Circle")]
        public int magicCircleIDLatest = 0;
        [XmlElement("Highest ID SubMap")]
        public int highestSubMapID = 0;
        [XmlElement("Highest ID Text")]
        public int highestTextID = 0;
        [XmlElement("Highest ID UI Collections")]
        public int highestUIID = 0;

        internal String contentDatabaseLoc = "";

        public void AddUICollection(UICollectionSave UICS)
        {
            UICS.parent.CollectionID = highestUIID++;
            gameUICollections.Add(UICS);       
        }

        public void AddMapGroup(String gName)
        {
            if (gameMapGroups.FindAll(name => name.Equals(gName, StringComparison.OrdinalIgnoreCase)).Count == 0)
            {
                gameMapGroups.Add(gName);
            }
        }

        public void ReplaceMapGroup(BasicMap parentMap, String gNameBefore, String gNameAfter)
        {
            foreach (var submap in parentMap.subMaps)
            {
                if (submap.mapGroup.Equals(gNameBefore, StringComparison.OrdinalIgnoreCase))
                {
                    submap.mapGroup = gNameAfter;
                }
            }
        }

        public void RemoveMapGroup(BasicMap parentMap, String gName)
        {
            if (!gName.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var submap in parentMap.subMaps)
                {
                    if (submap.mapGroup.Equals(gName, StringComparison.OrdinalIgnoreCase))
                    {
                        submap.mapGroup = "None";
                    }
                }

                gameMapGroups.RemoveAll(name => name.Equals(gName, StringComparison.OrdinalIgnoreCase));
            }
        }

        public void AddSubMap(BasicMap parentMap, BasicMap subMap)
        {
            highestSubMapID++;
            subMap.identifier = highestSubMapID;
            parentMap.subMaps.Add(subMap);
        }

        public GameContentDataBase()
        {

        }

        public void Reload()
        {
            if (gameMapGroups.Count == 0)
            {
                gameMapGroups.Add("None");
            }

            foreach (var item in gameTextCollection)
            {
                int difference = Enum.GetNames(typeof(GameText.Language)).Length - item.textCollection.Count;
                if (difference > 0)
                {
                    for (int i = 0; i < difference; i++)
                    {
                        item.textCollection.Add("");
                    }
                }
            }

            foreach (var script in gameScripts)
            {
                var copy = script.Clone();
                int index = 0;
                foreach (var line in script.scriptContent)
                {
                    if (line.Contains('-'))
                    {
                        while (copy.scriptContent[index].Contains("-"))
                        {
                            copy.scriptContent[index] = copy.scriptContent[index].Replace("-", "_");
                        }
                    }
                }
                script.scriptContent = new List<string>(copy.scriptContent);
            }

            foreach (var item in gameMagicCircleAnimations)
            {
                try
                {
                    item.ReloadTexture();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            foreach (var item in gameParticleAnimations)
            {
                try
                {
                    item.ReloadTexture();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                try
                {
                    item.ReloadGCDB(this);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }



            //LOAD ORDER IS ALWAYS: ABILITIES -> CLASSES -> CCCs
            foreach (var item in gameAbilities)
            {
                try
                {
                    item.ReloadTexture();
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
                try
                {
                    item.ReloadGCDB(this);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }



            foreach (var item in gameClasses)
            {
                try
                {
                    item.ReloadFromDatabase(this);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }



            foreach (var item in gameCCCs)
            {
                try
                {
                    item.ReloadFromDatabase(this);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }


            foreach (var item in gameItems)
            {
                try
                {
                    item.ReloadTexture();
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }



            foreach (var item in gameCharacters)
            {
                try
                {
                    item.ReloadTextures();
                    item.ReloadFromDatabase(this);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }



            foreach (var item in gameTileSheets)
            {
                try
                {
                    item.ReloadTextures();
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }


            try
            {
                foreach (var item in gameClasses)
                {
                    //  item.ReloadTexture();
                }
            }
            catch
            {

            }


            foreach (var item in gameAbilities)
            {
                try
                {
                    item.ReloadTexture();
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }


            foreach (var item in gameLights)
            {
                try
                {
                    item.ReloadTextures();
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }


            foreach (var item in gameObjectObjects)
            {
                try
                {
                    item.ReloadTextures();
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }



            foreach (var item in gameObjectGroups)
            {
                try
                {
                    item.Reload(this);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }
            }


            foreach (var item in gameSourceTiles)
            {
                try
                {
                    item.ReloadTextures();
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item);
                }

            }


            foreach (var item in gameSFXs)
            {
                try
                {
                    item.ReloadContent();
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR Loading: " + item.sfxName + " in: " + item.sfxLoc);
                }
            }

            foreach (var uic in gameUICollections)
            {
                try
                {
                    uic.Reload();
                }
                catch (Exception e)
                {

                   // throw;
                }
               
            }
        }

        public void AddTextCollection(GameText gt)
        {
            gt.textID = highestTextID;
            gameTextCollection.Add(gt);
            highestTextID++;
        }

        public void AddObject(BaseSprite bs)
        {
            bs.shapeID = objectID;
            gameObjectObjects.Add(bs);
            objectID++;
        }

        public void AddCharacter(BaseCharacter bc)
        {
            bc.shapeID = objectID;
            gameCharacters.Add(bc);
            objectID++;
        }

        public void AddObjectGroup(ObjectGroup og)
        {
            og.groupID = objectGroupIDHighest;
            gameObjectGroups.Add(og);
            objectGroupIDHighest++;
        }

        public void AddTile(TileSource ts)
        {
            ts.tileID = tileSourceID;
            gameSourceTiles.Add(ts);
            tileSourceID++;
        }

        public void AddScript(BaseScript bs, Object o)
        {
            bs.identifier = scriptID;
            gameScripts.Add(bs);

            if (o is BaseSprite)
            {
                (o as BaseSprite).scriptID = scriptID;
            }
            else if (o is NPCMoveToCommand)
            {
                (o as NPCMoveToCommand).scriptIdentifier = scriptID;
            }else if (o is MapZone)
            {
                (o as MapZone).scriptIdentifier = scriptID;
            }

            scriptID++;
        }

        public void AddScriptBool(ScriptBool sb)
        {
            sb.boolID = scriptBoolID;
            gameScriptBools.Add(sb);
            scriptBoolID++;
        }

        public void AddParticleSystem(ParticleSystemSource ps)
        {
            ps.identifier = particleSystemID;
            gameParticleSystems.Add(ps);
            particleSystemID++;
        }

        public void AddMap(BasicMap map)
        {
            mapID++;
            map.identifier = mapID;
        }

        public void AddTriggerZone()
        {
            triggerZoneID++;
        }

        public void AddAbility(BasicAbility ba)
        {
            ba.abilityIdentifier = abilityID;
            gameAbilities.Add(ba);
            abilityID++;
        }

        public void AddClass(BaseClass bc)
        {
            bc.classIdentifier = classID;
            gameClasses.Add(bc);
            classID++;
        }

        public void AddCCC(CharacterClassCollection CCC)
        {
            CCC.identifier = CCCID;
            gameCCCs.Add(CCC);
            CCCID++;
        }

        public void AddSFX(SFXInfo sfxi)
        {
            sfxi.sfxID = SFXID;
            SFXID++;
            gameSFXs.Add(sfxi);
        }

        public void AddLight(SpriteLight sl)
        {
            sl.shapeID = objectID;
            sl.lightID = LightID;
            sl.lightColor = Color.White;
            LightID++;
            objectID++;
            gameLights.Add(sl);
        }

        public void AddParticleAnimation(ParticleAnimation pa)
        {
            pa.particleAnimationID = particleAnimationIDLatest;
            particleAnimationIDLatest++;
            gameParticleAnimations.Add(pa);
        }

        public void AddMagicCircle(MagicCircleBase ma)
        {
            ma.magicCircleBaseID = magicCircleIDLatest;
            magicCircleIDLatest++;
            gameMagicCircleAnimations.Add(ma);
        }

        public void PostLoadCheck()
        {
            foreach (var character in gameCharacters)
            {
                while (character.portraitAnimations.Count != Enum.GetNames(typeof(BaseCharacter.PortraitExpressions)).Length)
                {
                    character.portraitAnimations.Add(new ShapeAnimation());
                }

                while (character.charBattleAnimations.Count != Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)).Length)
                {
                    if (character.charBattleAnimations.Count != 0)
                    {
                        character.charBattleAnimations.Add(character.charBattleAnimations[0].Clone());
                    }
                    else
                    {
                        character.charBattleAnimations.Add(new ShapeAnimation());
                    }

                }

                while (character.shapeHitBox.Count != Enum.GetNames(typeof(BaseCharacter.Rotation)).Length)
                {
                    character.shapeHitBox.Add(new List<Microsoft.Xna.Framework.Rectangle>());
                }

                while (character.battleAnimLocs.Count != Enum.GetNames(typeof(BaseCharacter.CharacterBattleAnimations)).Length)
                {
                    try
                    {
                        character.battleAnimLocs.Add(character.battleAnimLocs[0]);
                    }
                    catch
                    {
                        character.battleAnimLocs.Add(new Microsoft.Xna.Framework.Vector2(0, 0));
                    }

                }
            }

            //foreach (var ability in gameAbilities)
            //{
            //    while (ability.maxAbilityLevel == ability.abiAOEM.Count)
            //    {
            //        ability.abiAOEM.Add(new BaseModifier());
            //    }

            //    while (ability.maxAbilityLevel == ability.abiAOEMAS.Count)
            //    {
            //        ability.abiAOEMAS.Add(new ActiveStatModifier(true));
            //    }

            //    while (ability.maxAbilityLevel == ability.abilityDescription.Count)
            //    {
            //        ability.abilityDescription.Add("");
            //    }
            //}

            gameCharacters.ForEach(bs => bs.postSerializationReload(this));
            gameObjectObjects.ForEach(go => go.postSerializationReload(this));
            gameObjectGroups.ForEach(gog => gog.groupItems.ForEach(i => i.postSerializationReload(this)));
        }

        public void ReloadObjectGroups()
        {
            foreach (var item in gameObjectGroups)
            {
                item.groupItems.Clear();
                item.groupItemsIDs.ForEach(id => item.groupItems.Add(gameObjectObjects.Find(obj => obj.shapeID == id).ShallowCopy()));
            }
        }
    }

    [XmlRoot("Game Text")]
    public class GameText
    {
        public enum Language { English = 0 }

        [XmlArrayItem("Text Collection")]
        public List<String> textCollection = new List<string>();
        [XmlElement("Text ID")]
        public int textID = 0;
        [XmlElement("Text description")]
        public String textDescription = "";

        public GameText() { }

        public GameText(String s)
        {
            AddText(s,Language.English);
        }

        public void Initialize()
        {
            int difference = Enum.GetNames(typeof(GameText.Language)).Length - textCollection.Count;
            if (difference > 0)
            {
                for (int i = 0; i < difference; i++)
                {
                    textCollection.Add("");
                }
            }
        }

        public String getText()
        {
            return textCollection[(int)PlayerSaveData.gameLanguage];
        }

        public void AddText(String s,Language l)
        {
            Initialize();
            textCollection[(int)l] = s;
        }

        internal void SetText(string text, Language l = Language.English)
        {
            AddText(text,l);
        }
    }
}
