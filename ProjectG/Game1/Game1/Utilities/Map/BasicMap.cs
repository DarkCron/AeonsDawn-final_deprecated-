using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.SriptProcessing.ScriptTriggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.Characters;

namespace TBAGW.Utilities.Map
{
    [XmlRoot("Map")]
    public class BasicMap
    {
        //[XmlArrayItem("MapTiles")]
        //public List<List<KeyValuePair<Rectangle, List<BasicTile>>>> mapTilesLayer = new List<List<KeyValuePair<Rectangle, List<BasicTile>>>>();
        [XmlElement("mapName")]
        public String mapName = "";
        [XmlElement("mapLocation")]
        public String mapLocation = "";
        [XmlArrayItem("MapTriggers")]
        public List<BasicTrigger> mapTriggers = new List<BasicTrigger>();
        [XmlArrayItem("SpriteLayer Infos")]
        public List<BaseSpriteInfoHolder> mapSpritesInfos = new List<BaseSpriteInfoHolder>();
        [XmlElement("SubMapIndex")]
        public int subMapIndex = -1;
        [XmlArrayItem("SubMaps")]
        public List<BasicMap> subMaps = new List<BasicMap>();
        [XmlElement("MapIdentifier")]
        public int identifier = 0;
        [XmlElement("MapTeams")]
        public List<BaseTeam> mapTeams = new List<BaseTeam>();
        [XmlArrayItem("Used SoundPools")]
        public List<BGInfo> soundPools = new List<BGInfo>();
        [XmlArrayItem("Used external Maps info charts")]
        public List<MapInfoChart> externalMaps = new List<MapInfoChart>();

        [XmlArrayItem("Map editor Waypoints")]
        public List<MapEditorWaypoint> WPPoints = new List<MapEditorWaypoint>();

        [XmlArrayItem("map regions")]
        public List<MapRegion> mapRegions = new List<MapRegion>();
        [XmlElement("Map NPC's")]
        public List<NPC> mapNPCs = new List<NPC>();
        [XmlElement("Highest chunk ID")]
        public int highestChunkID = 0;

        [XmlElement("Map last object ID")]
        public int objectIDLastAdded = 0;


        [XmlElement("Map Buildings")]
        public List<Building> testBuildings = new List<Building>();
        [XmlElement("Map Group")]
        public String mapGroup = "None";
        [XmlArrayItem("Chunks Map")]
        public List<MapChunk> Chunks = new List<MapChunk>();

        [XmlIgnore]
        public MapSaveInfo mapSaveInfo = new MapSaveInfo();

        [XmlIgnore]
        public List<SpriteLight> mapLights = new List<SpriteLight>();
        [XmlIgnore]
        public List<ObjectGroup> mapObjectGroups = new List<ObjectGroup>();
        [XmlIgnore]
        public List<BaseSprite> mapSprites = new List<BaseSprite>();

        [XmlIgnore]
        public List<SpriteLight> activeLights = new List<SpriteLight>();
        [XmlIgnore]
        public List<BaseSprite> interactableObjects = new List<BaseSprite>();
        [XmlIgnore]
        public List<BaseScript> listOfAllUniqueScripts = new List<BaseScript>();
        //[XmlIgnore]
        //public List<List<KeyValuePair<Rectangle, List<BasicTile>>>> chunksToConsider = new List<List<KeyValuePair<Rectangle, List<BasicTile>>>>();
        [XmlIgnore]
        public List<MapChunk> chunksToConsider = new List<MapChunk>();
        [XmlIgnore]
        Rectangle previousCamera = new Rectangle();
        [XmlIgnore]
        MapRegion previousRegion = new MapRegion();

        internal List<MapChunk> previousChunksToConsider = new List<MapChunk>();

        internal GameContentDataBase gcdb;
        internal bool bIsDoneReloading = true;
        internal bool ChunksPrepared = false;

        public List<BasicMap> subMapsByGroup(String gName)
        {
            List<BasicMap> temp = new List<BasicMap>();
            if (gName.Equals("None", StringComparison.OrdinalIgnoreCase))
            {
                return subMaps;
            }
            else
            {
                foreach (var item in subMaps)
                {
                    if (item.mapGroup.Equals(gName, StringComparison.OrdinalIgnoreCase))
                    {
                        temp.Add(item);
                    }
                }
            }

            return temp;
        }

        public BasicMap()
        {
        }

        internal void getMapSaveDataReady()
        {
            if (!mapSaveInfo.bInitialized)
            {
                mapSaveInfo = new MapSaveInfo(this);
            }
        }

        public void ReloadMap()
        {

            //foreach (var sprite in mapSprites)
            //{
            //    sprite.ReloadTextures();
            //}


            foreach (var subMap in subMaps)
            {
                subMap.ReloadMap();
            }

            foreach (var item in soundPools)
            {
                item.ReloadContent();
            }

        }

        public void ConvertMapSprites()
        {
            mapSpritesInfos.Clear();
            foreach (var item in mapSprites)
            {
                mapSpritesInfos.Add(new BaseSpriteInfoHolder(item));
            }
            foreach (var item in mapObjectGroups)
            {
                mapSpritesInfos.Add(new BaseSpriteInfoHolder(item));
            }
            foreach (var item in mapLights)
            {
                mapSpritesInfos.Add(new BaseSpriteInfoHolder(item));
            }

            foreach (var item in mapNPCs)
            {
                item.infoHolder = new BaseSpriteInfoHolder(item.baseCharacter);
                item.mapActiveID = item.baseCharacter.currentMapToDisplayOn.identifier;
            }

            foreach (var item in subMaps)
            {
                item.ConvertMapSprites();
            }
        }

        public void PostSerializationReload(GameContentDataBase gcdb)
        {
            bIsDoneReloading = false;

            foreach (var building in testBuildings)
            {
                building.Reload(this, gcdb);
                foreach (var layer in building.buildingTiles)
                {
                    foreach (var tile in layer)
                    {
                        tile.Reload(gcdb);
                    }
                }
            }

            foreach (var item in mapSpritesInfos)
            {
                switch (item.spriteType)
                {
                    case BaseSpriteInfoHolder.SpriteType.Character:
                        mapSprites.Add(item.Reload(gcdb) as BaseSprite);
                        break;
                    case BaseSpriteInfoHolder.SpriteType.Object:
                        mapSprites.Add(item.Reload(gcdb) as BaseSprite);
                        break;
                    case BaseSpriteInfoHolder.SpriteType.objectGroup:
                        mapObjectGroups.Add(item.Reload(gcdb) as ObjectGroup);
                        break;
                    case BaseSpriteInfoHolder.SpriteType.SpriteLight:
                        mapLights.Add(item.Reload(gcdb) as SpriteLight);
                        break;
                    default:
                        break;
                }

            }

            mapSpritesInfos.RemoveAll(msi => msi.spriteObject == null);
            mapSprites.RemoveAll(msi => msi == null);
            mapObjectGroups.RemoveAll(msi => msi == null);
            mapLights.RemoveAll(msi => msi == null);

            //foreach (var item in mapObjectGroups)
            //{
            //    item.Reload(gcdb);
            //}

            mapSprites.ForEach(ms => ms.postSerializationReload(gcdb));
            //    mapObjectGroups.ForEach(og => og.groupItems.ForEach(o => o.postSerializationReload(gcdb)));
            mapNPCs.ForEach(npc => npc.DataBaseReload(gcdb));
            mapLights.ForEach(ml => ml.postSerializationReload(gcdb));

            subMaps.ForEach(map => map.PostSerializationReload(gcdb));

            mapRegions.ForEach(region => region.Reload(gcdb));

            //MapObjectHelpClass.objectsToUpdateOutsideOfMap.AddRange(mapNPCs);
            allMapsGame().ForEach(m => m.mapNPCs.ForEach(npc => npc.GetNPCLogicReady(m)));
            allMapsGame().ForEach(map => MapObjectHelpClass.objectsToUpdateOutsideOfMap.AddRange(map.mapNPCs));

            bIsDoneReloading = true;
        }

        public void MinimalUpdate(GameTime gameTime, Vector2 camera)
        {
            if (subMapIndex == -1)
            {
                //foreach (var list in mapTilesLayer)
                //{
                //    foreach (var tile in list)
                //    {
                //        tile.Contains(camera);
                //    }
                //}
                //Rectangle trueCamera = new Rectangle((int)(-camera.X - 400), (int)(-camera.Y - 400), 1366 + 800, 768 + 800);
                ////trueCamera = new Rectangle((int)(camera.X) - (int)(100 / GameProcessor.zoom), (int)(-camera.Y) - (int)(100 / GameProcessor.zoom), (int)(1366 / GameProcessor.zoom) + (int)(200 / GameProcessor.zoom), (int)(768 / GameProcessor.zoom) + (int)(200 / GameProcessor.zoom));

                //var allObjectsInCamera = mapSprites.FindAll(o => o.spriteGameSize.Contains(trueCamera) || o.spriteGameSize.Intersects(trueCamera));
                //foreach (var sprite in allObjectsInCamera)
                //{
                //    sprite.Update(gameTime);
                //}

                //var allObjectGroupsInCamera = mapObjectGroups.FindAll(og => og.Contains(trueCamera));
                //foreach (var sprite in allObjectGroupsInCamera)
                //{
                //    sprite.Update(gameTime);
                //}


                //var allNPCsInCamera = mapNPCs.FindAll(npc => npc.baseCharacter.spriteGameSize.Contains(trueCamera) || npc.baseCharacter.spriteGameSize.Intersects(trueCamera));
                //foreach (var sprite in allNPCsInCamera)
                //{
                //    sprite.baseCharacter.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                //    //sprite.baseCharacter.Update(gameTime);
                //}

                //activeLights.Clear();
                //var allLightsInCamera = mapLights.FindAll(ml => ml.spriteGameSize.Contains(trueCamera) || ml.spriteGameSize.Intersects(trueCamera));
                //foreach (var sprite in allLightsInCamera)
                //{
                //    sprite.Update(gameTime);
                //    activeLights.Add(sprite);

                //}

                MapChunk.MinimalUpdate(this, gameTime);
                //try
                //{
                //    activeLights[0].baseAnimations[0].frameIndex = 0;
                //    }
                //catch (Exception)
                //{

                //}

                try
                {
                    PlayerController.selectedSprite.Update(gameTime);
                }
                catch (Exception)
                {

                }

            }
            else
            {
                subMaps[subMapIndex].Update(gameTime, camera);
            }

        }

        public void Update(GameTime gameTime, Vector2 camera)
        {
            if (subMapIndex == -1)
            {
                //foreach (var list in mapTilesLayer)
                //{
                //    foreach (var tile in list)
                //    {
                //        tile.Contains(camera);
                //    }
                //}
                //  Rectangle trueCamera = new Rectangle((int)(camera.X - 400), (int)(-camera.Y - 400), (int)((1366 + 200) / MapBuilder.cameraZoomX), (int)((768 + 200) / MapBuilder.cameraZoomX));
                // Rectangle trueCamera = new Rectangle((int)(camera.X / MapBuilder.cameraZoomX) - (int)(0 / MapBuilder.cameraZoomX), (int)(camera.Y / MapBuilder.cameraZoomX) - (int)(0 / MapBuilder.cameraZoomX), (int)(1366 / MapBuilder.cameraZoomX) + (int)(500 / MapBuilder.cameraZoomX), (int)(768 / MapBuilder.cameraZoomX) + (int)(500 / MapBuilder.cameraZoomX));
                //trueCamera = new Rectangle((int)(camera.X) - (int)(100 / GameProcessor.zoom), (int)(-camera.Y) - (int)(100 / GameProcessor.zoom), (int)(1366 / GameProcessor.zoom) + (int)(200 / GameProcessor.zoom), (int)(768 / GameProcessor.zoom) + (int)(200 / GameProcessor.zoom));
                float zoom = MapBuilder.cameraZoomX;
                float y = camera.Y * -1;
                Rectangle trueCamera = new Rectangle((int)(camera.X) - (int)(100 / zoom), (int)(y) - (int)(100 / zoom), (int)(1366 / zoom) + (int)(200 / zoom), (int)(768 / zoom) + (int)(200 / zoom));

                //trueCamera.Y *= -1;
                //ForceCheckChunksToConsider();
                CheckChunksToConsider(trueCamera);
                MapChunk.Update(this, gameTime);


                if (!CombatProcessor.bIsRunning)
                {
                    for (int i = 0; i < mapSaveInfo.mapPUEntities.Count; i++)
                    {
                        mapSaveInfo.mapPUEntities[i].Update(gameTime);
                    }
                }


                //  interactableObjects.Clear();
                //  allNPCsInCamera.ForEach(npc => interactableObjects.Add(npc.baseCharacter));
                //  allObjectsInCamera.ForEach(obj => interactableObjects.Add(obj));
                // allObjectGroupsInCamera.ForEach(objg => interactableObjects.AddRange(objg.groupItems));


                //listOfAllUniqueScripts.Clear();
                //listOfAllUniqueScripts = CheckUniqueScripts();


            }
            else
            {
                subMaps[subMapIndex].Update(gameTime, camera);
            }


        }

        public void AddSpriteLightToMap(SpriteLight sl)
        {
            objectIDLastAdded++;
            sl.objectIDAddedOnMap = objectIDLastAdded;
            mapLights.Add(sl);
            mapSpritesInfos.Add(new BaseSpriteInfoHolder(sl));
            MapChunk.AddObjectsEditor(this, sl);
            // ForceCheckChunksToConsider();
        }

        public void AddSpriteToMap(BaseSprite bs)
        {
            objectIDLastAdded++;
            bs.objectIDAddedOnMap = objectIDLastAdded;
            mapSprites.Add(bs);
            MapChunk.AddObjectsEditor(this, bs);
            //ForceCheckChunksToConsider();
        }

        public void AddSpriteToMap(NPC npc)
        {
            objectIDLastAdded++;
            mapNPCs.Add(npc);
            npc.baseCharacter.objectIDAddedOnMap = objectIDLastAdded;
            mapSprites.Add(npc.baseCharacter);
            //  ForceCheckChunksToConsider();
        }

        public void AddObjectGroupToMap(ObjectGroup objg)
        {

            // objectIDLastAdded++;
            mapObjectGroups.Add(objg);
            //npc.baseCharacter.objectIDAddedOnMap = objectIDLastAdded;
            MapChunk.AddObjectsEditor(this, objg);
            //  ForceCheckChunksToConsider();
        }

        private List<BaseScript> CheckUniqueScripts()
        {
            List<BaseScript> UniqueList = new List<BaseScript>();
            bool bAlreadyHasThatTrigger = false;
            foreach (var t in mapTriggers)
            {
                if (UniqueList.Count == 0)
                {
                    UniqueList.Add(t.script);
                    UniqueList[UniqueList.Count - 1].type = "Trigger";
                }
                else
                {
                    foreach (var u in UniqueList)
                    {
                        if (t.script.Equals(u))
                        {
                            bAlreadyHasThatTrigger = true;
                            break;
                        }
                    }
                    if (!bAlreadyHasThatTrigger)
                    {
                        UniqueList.Add(t.script);
                        bAlreadyHasThatTrigger = false;
                        UniqueList[UniqueList.Count - 1].type = "Trigger";
                    }
                }
            }

            foreach (var s in mapSprites)
            {
                foreach (var u in UniqueList)
                {
                    if (s.script.Equals(u))
                    {
                        bAlreadyHasThatTrigger = true;
                        break;
                    }
                }
                if (!bAlreadyHasThatTrigger)
                {
                    UniqueList.Add(s.script);
                    bAlreadyHasThatTrigger = false;
                    UniqueList[UniqueList.Count - 1].type = "Sprite";
                }
            }
            return UniqueList;
        }

        internal void CheckChunksToConsider(Vector2 camera, float zoom = 1.0f)
        {

            Rectangle trueCamera = new Rectangle((int)(camera.X) - (int)(100 / zoom), (int)(camera.Y) - (int)(100 / zoom), (int)(1366 / zoom) + (int)(200 / zoom), (int)(768 / zoom) + (int)(200 / zoom));

            CheckChunksToConsider(trueCamera);
        }

        internal void CheckChunksToConsider(Rectangle trueCamera)
        {
            // trueCamera.Y *= -1;
            if (previousCamera != trueCamera)
            {
                previousCamera = trueCamera;
                chunksToConsider = new List<MapChunk>();
                chunksToConsider.AddRange(Chunks.FindAll(c => c.region.Contains(trueCamera) || c.region.Intersects(trueCamera)));

                if (chunksToConsider.Count == 0)
                {

                }

                foreach (var chunk in chunksToConsider)
                {
                    foreach (var layer in chunk.lbt)
                    {
                        foreach (var item in layer)
                        {
                            if (item.bMustReload)
                            {
                                item.Reload();
                            }
                        }
                    }
                    chunk.objectsInChunk = null;
                    if (chunk.objectsInChunk == null)
                    {
                        chunk.AddObjs(mapSprites.FindAll(s => chunk.region.Contains(s.trueMapSize()) || chunk.region.Intersects(s.trueMapSize())).Cast<object>().ToList());
                        chunk.AddObjs(mapObjectGroups.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                        chunk.AddObjs(mapLights.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                        chunk.AddObjs(mapNPCs.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                        chunk.AddObjs(testBuildings.FindAll(s => s.IsIn(chunk.region)).Cast<object>().ToList());
                        ChunksPrepared = true;
                    }
                }

                //MapChunk.Update(this);
                MapChunk.finalizeChunkCheck(this);
                previousChunksToConsider = new List<MapChunk>(chunksToConsider);
            }
        }

        internal void CheckChunkSpecific(MapChunk chunk)
        {
            // trueCamera.Y *= -1;
            foreach (var layer in chunk.lbt)
            {
                foreach (var item in layer)
                {
                    if (item.bMustReload)
                    {
                        item.Reload();
                    }
                }
            }
            chunk.objectsInChunk = null;
            if (chunk.objectsInChunk == null)
            {
                chunk.AddObjs(mapSprites.FindAll(s => chunk.region.Contains(s.trueMapSize()) || chunk.region.Intersects(s.trueMapSize())).Cast<object>().ToList());
                chunk.AddObjs(mapObjectGroups.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                chunk.AddObjs(mapLights.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                chunk.AddObjs(mapNPCs.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                chunk.AddObjs(testBuildings.FindAll(s => s.IsIn(chunk.region)).Cast<object>().ToList());
                ChunksPrepared = true;
            }

        }

        internal void ForceCheckChunksToConsider(Rectangle trueCamera = default(Rectangle))
        {
            if (trueCamera == default(Rectangle))
            {
                trueCamera = previousCamera;
            }

            previousCamera = trueCamera;
            chunksToConsider = new List<MapChunk>();
            chunksToConsider.AddRange(Chunks.FindAll(c => c.region.Contains(trueCamera) || c.region.Intersects(trueCamera)));

            if (chunksToConsider.Count == 0)
            {

            }

            foreach (var chunk in chunksToConsider)
            {
                foreach (var layer in chunk.lbt)
                {
                    foreach (var item in layer)
                    {
                        if (item.bMustReload)
                        {
                            item.Reload();
                        }
                    }
                }
                if (chunk.objectsInChunk == null)
                {
                    chunk.objectsInChunk = new List<object>();
                }
                chunk.objectsInChunk.Clear();
                chunk.AddObjs(mapSprites.FindAll(s => chunk.region.Contains(s.trueMapSize()) || chunk.region.Intersects(s.trueMapSize())).Cast<object>().ToList());
                chunk.AddObjs(mapObjectGroups.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                chunk.AddObjs(mapLights.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                chunk.AddObjs(mapNPCs.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                chunk.AddObjs(testBuildings.FindAll(s => s.IsIn(chunk.region)).Cast<object>().ToList());
                ChunksPrepared = true;
            }

            //MapChunk.Update(this);
            previousChunksToConsider.Clear();
            MapChunk.forceFinalizeChunkCheck(this);
            previousChunksToConsider = new List<MapChunk>(chunksToConsider);
        }

        internal void PrepareChunksForLogic()
        {
            foreach (var chunk in Chunks)
            {

                chunk.AddObjs(mapSprites.FindAll(s => chunk.region.Contains(s.trueMapSize()) || chunk.region.Intersects(s.trueMapSize())).Cast<object>().ToList());
                chunk.AddObjs(mapObjectGroups.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                chunk.AddObjs(mapLights.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                chunk.AddObjs(mapNPCs.FindAll(s => s.Contains(chunk.region)).Cast<object>().ToList());
                chunk.AddObjs(testBuildings.FindAll(s => s.IsIn(chunk.region)).Cast<object>().ToList());
                ChunksPrepared = true;
            }
        }

        public void EditorAdditionAddObject(Object obj)
        {
            MapChunk.AddObjectsEditor(this, obj);
        }

        public void DrawMapEditor(SpriteBatch spriteBatch, GameContentDataBase gcdb, Rectangle camera, RenderTarget2D targetRender, Matrix m, float zoom = 1)
        {
            if (subMapIndex == -1)
            {
                var objectListInfo = new List<objectInfo>();
                if (MapBuilder.bShowInvisibleObjects)
                {
                    objectListInfo = new List<objectInfo>(MapChunk.consideredSprites);
                }
                else
                {
                    objectListInfo = MapChunk.consideredSprites.FindAll(cs => cs.bIsVisible);
                }


                objectListInfo.FindAll(oli => oli.objectType == objectInfo.type.ObjectGroup).Select(oli => oli.obj).Cast<ObjectGroup>().ToList().FindAll(og => og.bGenerateRender).ForEach(og => og.GenerateRender(spriteBatch));
                if (MapBuilder.objAddition != null && MapBuilder.objAddition.IsCorrect() && MapBuilder.objAddition.selectedObject.GetType() == typeof(ObjectGroup))
                {
                    var temp = MapBuilder.objAddition.selectedObject as ObjectGroup;
                    if (temp.bGenerateRender)
                    {
                        temp.GenerateRender(spriteBatch);
                    }
                }
                if (MapBuilder.objSelection != null && MapBuilder.objSelection.IsCorrect() && MapBuilder.objSelection.selectedObject.GetType() == typeof(ObjectGroup))
                {
                    var temp = MapBuilder.objSelection.selectedObject as ObjectGroup;
                    if (temp.bGenerateRender)
                    {
                        temp.GenerateRender(spriteBatch);
                    }
                }


                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(objectLayer);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, m);

                DrawObjectLayer(spriteBatch, MapBuilder.gcDB, objectListInfo);

                #region
                //if (false)
                //{
                //    foreach (var item in chunksToConsider[0].objectsInChunk.FindAll(obj => obj.GetType() == typeof(BaseSprite)))
                //    {
                //        (item as BaseSprite).getFXRender(spriteBatch);
                //    }

                //    foreach (var item in chunksToConsider[0].objectsInChunk.FindAll(obj => obj.GetType() == typeof(ObjectGroup)))
                //    {
                //        (item as ObjectGroup).groupItems.ForEach(i => i.getFXRender(spriteBatch));
                //    }
                //}

                //bool bDrawPlayerOnTop = false;
                //var allObjectsInCamera = mapSprites.FindAll(o => o.spriteGameSize.Contains(trueCamera) || o.spriteGameSize.Intersects(trueCamera));
                //foreach (var sprite in allObjectsInCamera)
                //{
                //    sprite.Draw(spriteBatch);
                //    //  sprite.EffectDraw(spriteBatch);
                //    if (!bDrawPlayerOnTop && sprite.IsOnTop(PlayerController.selectedSprite))
                //    {
                //        bDrawPlayerOnTop = true;
                //    }
                //}

                //var allObjectGroupsInCamera = mapObjectGroups.FindAll(og => og.Contains(trueCamera));
                //foreach (var sprite in allObjectGroupsInCamera)
                //{
                //    sprite.Draw(spriteBatch);
                //    foreach (var item in sprite.groupItems)
                //    {
                //        if (!bDrawPlayerOnTop && item.IsOnTop(PlayerController.selectedSprite))
                //        {
                //            bDrawPlayerOnTop = true;
                //        }
                //    }

                //}

                //foreach (var item in activeLights)
                //{
                //    item.Draw(spriteBatch);
                //}


                //var allNPCsInCamera = mapNPCs.FindAll(npc => npc.baseCharacter.spriteGameSize.Contains(trueCamera) || npc.baseCharacter.spriteGameSize.Intersects(trueCamera));
                //foreach (var sprite in allNPCsInCamera)
                //{
                //    sprite.Draw(spriteBatch);
                //    if (!bDrawPlayerOnTop && sprite.baseCharacter.IsOnTop(PlayerController.selectedSprite))
                //    {
                //        bDrawPlayerOnTop = true;
                //    }
                //}

                //foreach (var item in ScriptProcessor.scriptTempObjects)
                //{
                //    if (item is NPC)
                //    {
                //        (item as NPC).Draw(spriteBatch);
                //        (item as NPC).baseCharacter.Draw(spriteBatch);
                //    }
                //}

                //if (!GameProcessor.bInCombat && bDrawPlayerOnTop && GameProcessor.bIsInGame)
                //{
                //    PlayerController.selectedSprite.Draw(spriteBatch);
                //}



                // GameProcessor.DrawObjHeight(spriteBatch, HeightMap2DFx, objHeight, m);
                #endregion


                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(groundLayer);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, m);


                //CheckChunksToConsider(trueCamera);

                foreach (var item in chunksToConsider)
                {
                    if ((item.region.X + item.region.Y) % 3200 == 0)
                    {
                        spriteBatch.Draw(Game1.hitboxHelp, item.region, Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(Game1.hitboxHelp, item.region, Color.Red);
                    }

                    spriteBatch.DrawString(Game1.defaultFont, item.ToString(), item.region.Location.ToVector2() + new Vector2(75), Color.Black, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
                }

                //mapTilesLayer.FindAll(l => l.Key.Contains(trueCamera) || l.Key.Intersects(trueCamera));
                if (MapBuilder.bOnlyDrawOneLayer)
                {
                    foreach (var chunk in chunksToConsider)
                    {
                        chunk.returnLayer(Form1.layerDepth).ForEach(t => t.Draw(spriteBatch, gcdb));
                    }
                }
                else
                {
                    foreach (var chunk in chunksToConsider)
                    {
                        chunk.Checkhunk();
                        foreach (var layer in chunk.lbt)
                        {
                            layer.ForEach(t => t.Draw(spriteBatch, gcdb));
                        }
                    }
                }



                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(targetRender);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null);
                spriteBatch.Draw(groundLayer, groundLayer.Bounds, Color.White);
                spriteBatch.Draw(objectLayer, objectLayer.Bounds, Color.White);
                //      spriteBatch.Draw(HeightMap2DFx, HeightMap2DFx.Bounds, Color.White);


                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, m);
            }
            else
            {
                subMaps[subMapIndex].DrawMap(spriteBatch, gcdb, targetRender, zoom);
            }


        }

        public void DrawMapEditorRender(SpriteBatch spriteBatch, GameContentDataBase gcdb, Rectangle camera, float zoom = 1)
        {
            Rectangle trueCamera = new Rectangle((int)(camera.X) - (int)(100 / zoom), (int)(camera.Y) - (int)(100 / zoom), (int)(camera.Width / zoom) + (int)(200 / zoom), (int)(camera.Height / zoom) + (int)(200 / zoom));

            if (subMapIndex == -1)
            {
                CheckChunksToConsider(trueCamera);

                //mapTilesLayer.FindAll(l => l.Key.Contains(trueCamera) || l.Key.Intersects(trueCamera));
                foreach (var chunk in chunksToConsider)
                {
                    foreach (var layer in chunk.lbt)
                    {
                        layer.ForEach(t => t.Draw(spriteBatch, gcdb));
                    }
                }


                bool bDrawPlayerOnTop = false;
                var allObjectsInCamera = mapSprites.FindAll(o => o.spriteGameSize.Contains(trueCamera) || o.spriteGameSize.Intersects(trueCamera));
                foreach (var sprite in allObjectsInCamera)
                {
                    sprite.Draw(spriteBatch);
                    //  sprite.EffectDraw(spriteBatch);
                    if (!bDrawPlayerOnTop && sprite.IsOnTop(PlayerController.selectedSprite))
                    {
                        bDrawPlayerOnTop = true;
                    }
                }

                var allObjectGroupsInCamera = mapObjectGroups.FindAll(og => og.Contains(trueCamera));
                foreach (var sprite in allObjectGroupsInCamera)
                {
                    sprite.Draw(spriteBatch);
                    foreach (var item in sprite.groupItems)
                    {
                        if (!bDrawPlayerOnTop && item.IsOnTop(PlayerController.selectedSprite))
                        {
                            bDrawPlayerOnTop = true;
                        }
                    }

                }

                foreach (var item in activeLights)
                {
                    item.Draw(spriteBatch);
                }


                var allNPCsInCamera = mapNPCs.FindAll(npc => npc.baseCharacter.spriteGameSize.Contains(trueCamera) || npc.baseCharacter.spriteGameSize.Intersects(trueCamera));
                foreach (var sprite in allNPCsInCamera)
                {
                    sprite.Draw(spriteBatch);
                    if (!bDrawPlayerOnTop && sprite.baseCharacter.IsOnTop(PlayerController.selectedSprite))
                    {
                        bDrawPlayerOnTop = true;
                    }
                }

                foreach (var item in ScriptProcessor.scriptTempObjects)
                {
                    if (item is NPC)
                    {
                        (item as NPC).Draw(spriteBatch);
                        (item as NPC).baseCharacter.Draw(spriteBatch);
                    }
                }

                if (!GameProcessor.bInCombat && bDrawPlayerOnTop && GameProcessor.bIsInGame)
                {
                    PlayerController.selectedSprite.Draw(spriteBatch);
                }

            }
            else
            {
                //subMaps[subMapIndex].DrawMap(spriteBatch, gcdb, camera, r2DTarget, zoom);
            }


        }

        RenderTarget2D CombatObjectsLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        RenderTarget2D NonCombatObjectsLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        RenderTarget2D CombatFXBehindObjectLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        RenderTarget2D HeightMap2DFx = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        RenderTarget2D HeightMap2D = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        RenderTarget2D WaterLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        RenderTarget2D groundLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        RenderTarget2D PreciseWaterLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        RenderTarget2D WaterReflection = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        RenderTarget2D objectLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        Effect objHeight = Game1.contentManager.Load<Effect>(@"FX\Environment\2DHeight");
        Effect waterExtracter = Game1.contentManager.Load<Effect>(@"FX\Environment\WaterExtract");
        Effect waterReflection = Game1.contentManager.Load<Effect>(@"FX\Environment\WaterReflection");
        Effect waterReflection2 = Game1.contentManager.Load<Effect>(@"FX\Environment\WaterReflection2");
        Effect waterReflection3 = Game1.contentManager.Load<Effect>(@"FX\Environment\WaterReflection3");
        Effect combatObjectFX = Game1.contentManager.Load<Effect>(@"FX\Combat\CombatObject");
        static List<Texture2D> noiseMaps = new List<Texture2D>();
        static int counter = 0;

        internal RenderTarget2D GetHeightMask()
        {
            return HeightMap2DFx;
        }

        public void DrawMap(SpriteBatch spriteBatch, GameContentDataBase gcdb, RenderTarget2D r2DTarget, float zoom = 1)
        {
            if (noiseMaps.Count == 0)
            {
                noiseMaps.Add(Game1.contentManager.Load<Texture2D>(@"FX\Noise\Noise1"));
                noiseMaps.Add(Game1.contentManager.Load<Texture2D>(@"FX\Noise\Noise2"));
                noiseMaps.Add(Game1.contentManager.Load<Texture2D>(@"FX\Noise\Noise3"));
                noiseMaps.Add(Game1.contentManager.Load<Texture2D>(@"FX\Noise\Noise4"));
                noiseMaps.Add(Game1.contentManager.Load<Texture2D>(@"FX\Noise\Fog1"));
                noiseMaps.Add(Game1.contentManager.Load<Texture2D>(@"FX\Noise\Fog2"));
            }

            if (subMapIndex == -1)
            {

                var objectListInfo = MapChunk.consideredSprites.FindAll(cs => cs.bIsVisible);


                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(null);
                spriteBatch.GraphicsDevice.SetRenderTarget(groundLayer);
                spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
                List<BasicTile> waterTiles = new List<BasicTile>();

                //spriteBatch.GraphicsDevice.SetRenderTargets(GameProcessor.gameRender);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
                //mapTilesLayer.FindAll(l => l.Key.Contains(trueCamera) || l.Key.Intersects(trueCamera));
                //foreach (var item in Chunks)
                //{
                //    item.region.Y *=-1;
                //}
                foreach (var chunk in chunksToConsider)
                {
                    chunk.Checkhunk();
                    foreach (var layer in chunk.lbt)
                    {
                        foreach (var tile in layer)
                        {
                            if ((previousCamera.Contains(tile.mapPosition) || previousCamera.Intersects(tile.mapPosition)) && tile.tileSource.tileType != TileSource.TileType.Fluid)
                            {
                                tile.Draw(spriteBatch, gcdb);
                            }
                            else if ((previousCamera.Contains(tile.mapPosition) || previousCamera.Intersects(tile.mapPosition)) && tile.tileSource.tileType == TileSource.TileType.Fluid)
                            {
                                waterTiles.Add(tile);
                            }
                        }
                    }
                }




                //List<BasicTile> tiles = new List<BasicTile>();
                //foreach (var chunk in chunksToConsider)
                //{
                //    tiles.AddRange(this.possibleTilesWithController(chunk.region, this));
                //}
                //tiles = tiles.Distinct().ToList();
                //foreach (var tile in tiles)
                //{
                //    spriteBatch.Draw(Game1.hitboxHelp, tile.mapPosition, Color.LawnGreen);
                //}

                spriteBatch.End();


                spriteBatch.GraphicsDevice.SetRenderTarget(null);

                spriteBatch.GraphicsDevice.SetRenderTarget(WaterLayer);
                spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
                foreach (var tile in waterTiles)
                {
                    tile.Draw(spriteBatch, gcdb);
                }
                spriteBatch.End();




                spriteBatch.GraphicsDevice.SetRenderTarget(null);

                spriteBatch.GraphicsDevice.SetRenderTarget(PreciseWaterLayer);
                spriteBatch.GraphicsDevice.Clear(Color.Transparent);

                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                waterExtracter.Parameters["ground"].SetValue(groundLayer);
                waterExtracter.CurrentTechnique.Passes[0].Apply();
                spriteBatch.Draw(WaterLayer, WaterLayer.Bounds, Color.White);
                spriteBatch.End();


                var GenerateObjectGroupRenderList = objectListInfo.FindAll(oi => oi.objectType == objectInfo.type.ObjectGroup && (oi.obj as ObjectGroup).bGenerateRender);
                for (int i = 0; i < GenerateObjectGroupRenderList.Count; i++)
                {
                    (GenerateObjectGroupRenderList[i].obj as ObjectGroup).GenerateRender(spriteBatch);
                }


                spriteBatch.GraphicsDevice.SetRenderTarget(null);

                spriteBatch.GraphicsDevice.SetRenderTarget(objectLayer);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);

                if (!CombatProcessor.bIsRunning)
                {
                    for (int i = 0; i < mapSaveInfo.mapPUEntities.Count; i++)
                    {
                        mapSaveInfo.mapPUEntities[i].Draw(spriteBatch);
                    }
                }

                DrawObjectLayer(spriteBatch, gcdb, objectListInfo);
                //CombatArrowLayout.Draw(spriteBatch);

                spriteBatch.End();

                if (CombatProcessor.bIsRunning)
                {
                    spriteBatch.GraphicsDevice.SetRenderTarget(CombatObjectsLayer);
                    spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                    //spriteBatch.Draw(CombatProcessor.CombatGroundRender, CombatProcessor.CombatGroundRender.Bounds, Color.Blue*.6f);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
                    DrawObjectLayer(spriteBatch, gcdb, objectListInfo, objectListInfo);
                    CombatArrowLayout.Draw(spriteBatch);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                    spriteBatch.Draw(CombatProcessor.getTacticalTextPopUpRender(), new Rectangle(0, 0, 1366, 768), Color.White);
                    spriteBatch.End();


                    spriteBatch.GraphicsDevice.SetRenderTarget(NonCombatObjectsLayer);
                    spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
                    DrawObjectLayer(spriteBatch, gcdb, objectListInfo, combatObjectInfo);
                    spriteBatch.End();


                    spriteBatch.GraphicsDevice.SetRenderTarget(CombatFXBehindObjectLayer);
                    spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                    combatObjectFX.Parameters["SpriteTexture"].SetValue(NonCombatObjectsLayer);
                    combatObjectFX.Parameters["modifier"].SetValue(2.5f);
                    combatObjectFX.CurrentTechnique.Passes[0].Apply();
                    spriteBatch.Draw(CombatObjectsLayer, CombatObjectsLayer.Bounds, Color.White);
                    spriteBatch.End();
                }


                //  GameProcessor.DrawObjHeight(spriteBatch, HeightMap2DFx, objHeight, GameProcessor.CameraScaleMatrix);


                spriteBatch.End();

                spriteBatch.GraphicsDevice.SetRenderTarget(null);
                // spriteBatch.GraphicsDevice.SetRenderTargets(GameProcessor.shadowRender);

                //spriteBatch.Begin();
                //spriteBatch.Draw(GameProcessor.groundRender, GameProcessor.groundRender.Bounds, Color.White);
                //spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
                //GameProcessor.TestDrawShadows(spriteBatch);
                //  GameProcessor.bUpdateShadows = true;
                if (GameProcessor.bUpdateShadows && GameProcessor.bShadowsEnabled)
                {
                    GameProcessor.DrawShadows(spriteBatch, objectListInfo);
                    GameProcessor.bUpdateShadows = false;
                }


                {
                    spriteBatch.End();
                    spriteBatch.GraphicsDevice.SetRenderTarget(GameProcessor.shadowRender);
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, GameProcessor.shadowSampler, null, null, null, GameProcessor.CameraScaleMatrix);
                    spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));

                    DrawGeneratedShadowLayer(spriteBatch, objectListInfo);
                }
                //  GameProcessor.FXDrawLayers(spriteBatch,HeightMap2D);
                //GameProcessor.TestDrawSomeEffects(spriteBatch, GameProcessor.objectRender);
                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(null);
                //  spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);

                spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
                // // spriteBatch.GraphicsDevice.Clear(Color.Black);

                if (true && waterTiles.Count != 0)
                {
                    GameProcessor.DrawWaterShader(spriteBatch, HeightMap2D, waterTiles, objectListInfo);
                }
                //}else if (waterTiles.Count != 0)
                //{
                //    spriteBatch.GraphicsDevice.SetRenderTarget(HeightMap2D);
                //    spriteBatch.GraphicsDevice.Clear(Color.Transparent);
                //    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                //    spriteBatch.Draw(objectLayer, objectLayer.Bounds, objectLayer.Bounds, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                //    spriteBatch.End();
                //}




                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(null);

                spriteBatch.GraphicsDevice.SetRenderTarget(null);

                spriteBatch.GraphicsDevice.SetRenderTarget(WaterReflection);
                spriteBatch.GraphicsDevice.Clear(Color.Transparent);

                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                //waterReflection.Parameters["objects"].SetValue(HeightMap2D);
                //waterReflection.CurrentTechnique.Passes[0].Apply();
                //spriteBatch.Draw(PreciseWaterLayer, PreciseWaterLayer.Bounds, Color.White);
                //spriteBatch.End();

                if (true)
                {
                    if (GameProcessor.bWaterReflectionEnabled)
                    {
                        counter++;
                        if (counter > 2048)
                        {
                            counter = 0;
                        }
                        var tempSize = HeightMap2D.Bounds.Size;
                        waterReflection2.Parameters["objects"].SetValue(HeightMap2D);
                        waterReflection2.Parameters["noiseMap"].SetValue(noiseMaps[4]);
                        waterReflection2.Parameters["noiseWidth"].SetValue((float)noiseMaps[4].Width);
                        waterReflection2.Parameters["noiseHeight"].SetValue((float)noiseMaps[4].Height);
                        waterReflection2.Parameters["distanceHorMod"].SetValue((float)counter);
                        waterReflection2.Parameters["distanceMod"].SetValue(0.03f * GameProcessor.zoom);
                        waterReflection2.Parameters["width"].SetValue((float)tempSize.X);
                        waterReflection2.Parameters["height"].SetValue((float)tempSize.Y);
                        waterReflection2.CurrentTechnique.Passes[0].Apply();
                    }

                    spriteBatch.Draw(PreciseWaterLayer, PreciseWaterLayer.Bounds, Color.White);

                }
                spriteBatch.End();

                spriteBatch.GraphicsDevice.SetRenderTarget(r2DTarget);



                spriteBatch.Begin();

                spriteBatch.Draw(groundLayer, groundLayer.Bounds, Color.White);

                spriteBatch.Draw(WaterReflection, WaterReflection.Bounds, Color.White);

                spriteBatch.Draw(GameProcessor.shadowRender, new Rectangle(0, 0, 1366, 768), Color.White * .4f);


                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

                if (CombatProcessor.bIsRunning)
                {
                    spriteBatch.Draw(CombatProcessor.CombatGroundRender, CombatProcessor.CombatGroundRender.Bounds, Color.White);
                }

                spriteBatch.Draw(objectLayer, objectLayer.Bounds, Color.White);

                if (CombatProcessor.bIsRunning)
                {
                    //spriteBatch.Draw(NonCombatObjectsLayer, NonCombatObjectsLayer.Bounds, Color.Red);
                    spriteBatch.Draw(CombatFXBehindObjectLayer, CombatFXBehindObjectLayer.Bounds, Color.White);
                }

                EnvironmentHandler.DrawEnvironmentPreLightning(spriteBatch);
                //spriteBatch.Draw(HeightMap2D, TestEnvironment.rainRender.Bounds, Color.Red);

                spriteBatch.End();

                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);

            }
            else
            {
                subMaps[subMapIndex].DrawMap(spriteBatch, gcdb, r2DTarget, zoom);
            }


        }

        public static List<objectInfo> getCombatObjectInfo()
        {
            if (combatObjectInfo != null)
            {
                return combatObjectInfo;
            }
            return new List<objectInfo>();
        }

        private void DrawGeneratedShadowLayer(SpriteBatch spriteBatch, List<objectInfo> objectListInfo)
        {
            var mapSpriteInfo = objectListInfo.FindAll(msi => msi.bIsVisible);
            if (CombatProcessor.bIsRunning)
            {
                mapSpriteInfo.AddRange(combatObjectInfo);
            }

            //sprites.RemoveAll(s => !s.bIsVisible);
            var allUniqueObjs = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.Sprite).Select(oi => oi.obj).Cast<BaseSprite>().ToList().GroupBy(x => x.shapeID).Select(g => g.First()).ToList();
            var allCharacters = mapSpriteInfo.FindAll(oi => oi.obj.GetType() != typeof(NPC) && oi.objectType == objectInfo.type.Character).Select(oi => oi.obj).Cast<BaseCharacter>().ToList();
            var allNPCs = mapSpriteInfo.FindAll(oi => oi.obj.GetType() == typeof(NPC)).Select(oi => (oi.obj as NPC).baseCharacter).Cast<BaseCharacter>().ToList();
            var allObjGroups = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.ObjectGroup).Select(oi => oi.obj).Cast<ObjectGroup>().ToList();
            var allBuildings = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.Building).Select(oi => oi.obj).Cast<Building>().ToList();
            var allUniqueObjGroups = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.ObjectGroup).Select(oi => oi.obj).Cast<ObjectGroup>().ToList().GroupBy(x => x.groupID).Select(g => g.First()).ToList();

            if (CombatProcessor.bIsRunning)
            {
                allCharacters.RemoveAll(c => !c.IsAlive());
            }

            List<BaseSprite> sprites = new List<BaseSprite>();
            sprites.AddRange(mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.Sprite).Select(oi => oi.obj).Cast<BaseSprite>().ToList());
            sprites.AddRange(allCharacters);
            allCharacters.AddRange(allNPCs);
            sprites.AddRange(allNPCs);



            foreach (var sprite in sprites)
            {
                if (sprite == PlayerController.selectedSprite)
                {

                }
                var renderForItem = sprite.getShadowFX();
                if (renderForItem != null && !renderForItem.IsDisposed)
                {
                    Rectangle r = new Rectangle(new Point((int)(sprite.position - new Vector2(sprite.spriteGameSize.Height, 0)).X, (int)sprite.position.Y), renderForItem.Bounds.Size);
                    int adjustX = (int)(((renderForItem.Width * sprite.scaleVector.X) - (sprite.spriteGameSize.Height * sprite.scaleVector.X)) / 2);
                    Rectangle r2 = new Rectangle(sprite.spriteGameSize.Location - new Point(adjustX, 0), (r.Size.ToVector2() * sprite.scaleVector.X).ToPoint());

                    spriteBatch.Draw(renderForItem, r2, new Rectangle(0, 0, (int)(renderForItem.Width * GameProcessor.shadowQualityPercentage), (int)(renderForItem.Height * GameProcessor.shadowQualityPercentage)), Color.White);
                }
                else
                {

                }
            }

            List<KeyValuePair<Point, int>> ogInfo = new List<KeyValuePair<Point, int>>();
            foreach (var item in allUniqueObjGroups)
            {
                var temp = GameProcessor.gcDB.gameObjectGroups.Find(g => g.groupID == item.groupID);
                temp.CalculateGroupSize();
                ogInfo.Add(new KeyValuePair<Point, int>(temp.groupSize, item.groupID));
            }

            foreach (var sprite in allObjGroups)
            {
                var renderForItem = sprite.getShadowFX();
                if (renderForItem != null && !renderForItem.IsDisposed)
                {
                    var infoForGroup = ogInfo.Find(i => i.Value == sprite.groupID);
                    var adjustedSize = infoForGroup.Key;
                    if (sprite.scaleVector != new Vector2(1))
                    {
                        adjustedSize = (infoForGroup.Key.ToVector2() * sprite.scaleVector).ToPoint();
                    }
                    var r = new Rectangle(new Point((int)(sprite.getFXRenderDawLoc().Location.ToVector2()).X, (int)sprite.getFXRenderDawLoc().Location.ToVector2().Y), adjustedSize);
                    spriteBatch.Draw(renderForItem, r, new Rectangle(0, 0, (int)(renderForItem.Width * GameProcessor.shadowQualityPercentage), (int)(renderForItem.Height * GameProcessor.shadowQualityPercentage)), Color.White);
                }
                else
                {

                }
            }

            int index = 0;
            foreach (var item in allBuildings)
            {

                int id = testBuildings.IndexOf(item);

                var render = item.getShadowFX();
                if (render != null && !render.IsDisposed && id != -1)
                {
                    // spriteBatch.Draw(item, loadedMap.testBuildings[id].boundingZone.Location.ToVector2() - loadedMap.testBuildings[id].boundingDrawZone.Location.ToVector2(), new Rectangle(0, 0, (int)(item.Bounds.Width * percentage), (int)(item.Bounds.Height * percentage)), Color.White);
                    Rectangle tempR = new Rectangle(testBuildings[id].boundingZone.X - testBuildings[id].boundingDrawZone.X, testBuildings[id].boundingZone.Y - testBuildings[id].boundingDrawZone.Location.Y, (int)(render.Bounds.Width / GameProcessor.shadowQualityPercentage), (int)(render.Bounds.Height / GameProcessor.shadowQualityPercentage));
                    spriteBatch.Draw(render, tempR, render.Bounds, Color.White);
                    // spriteBatch.Draw(item, item.Bounds, Color.White);
                    index++;
                }
                else
                {

                }

            }
        }

        public List<RenderTarget2D> getMapRender()
        {
            List<RenderTarget2D> renders = new List<RenderTarget2D>();
            renders.Add(groundLayer);
            renders.Add(WaterReflection);
            renders.Add(GameProcessor.shadowRender);
            renders.Add(objectLayer);
            return renders;
        }

        private void DrawObjectLayer(SpriteBatch spriteBatch, GameContentDataBase gcdb, Rectangle camera, float zoom, List<BaseSprite> temp)
        {
            Rectangle trueCamera = new Rectangle((int)(camera.X) - (int)(100 / zoom), (int)(camera.Y) - (int)(100 / zoom), (int)(1366 / zoom) + (int)(200 / zoom), (int)(768 / zoom) + (int)(200 / zoom));

            //foreach (var item in mapNPCs)
            //{
            //    item.Draw(spriteBatch);
            //}

            if (!MapBuilder.bIsRunning)
            {
                // GameProcessor.TestDrawSomeEffects(spriteBatch);
            }


            if (!GameProcessor.bInCombat && GameProcessor.bIsInGame && PlayerController.selectedSprite != null)
            {
                PlayerController.selectedSprite.Draw(spriteBatch);
            }

            bool bDrawPlayerOnTop = false;
            //var allObjectsInCamera = mapSprites.FindAll(o => o.spriteGameSize.Contains(trueCamera) || o.spriteGameSize.Intersects(trueCamera));
            //foreach (var sprite in allObjectsInCamera)
            //{
            //    sprite.Draw(spriteBatch);
            //    //  sprite.EffectDraw(spriteBatch);
            //    if (!bDrawPlayerOnTop && sprite.bIsOnTop(PlayerController.selectedSprite))
            //    {
            //        bDrawPlayerOnTop = true;
            //    }
            //}

            //var allObjectGroupsInCamera = mapObjectGroups.FindAll(og => og.Contains(trueCamera));
            //foreach (var sprite in allObjectGroupsInCamera)
            //{
            //    sprite.Draw(spriteBatch);
            //    foreach (var item in sprite.groupItems)
            //    {
            //        if (!bDrawPlayerOnTop && item.bIsOnTop(PlayerController.selectedSprite))
            //        {
            //            bDrawPlayerOnTop = true;
            //        }
            //    }

            //}




            //var allNPCsInCamera = mapNPCs.FindAll(npc => npc.baseCharacter.spriteGameSize.Contains(trueCamera) || npc.baseCharacter.spriteGameSize.Intersects(trueCamera));
            //foreach (var sprite in allNPCsInCamera)
            //{
            //    sprite.Draw(spriteBatch);
            //    if (!bDrawPlayerOnTop && sprite.baseCharacter.bIsOnTop(PlayerController.selectedSprite))
            //    {
            //        bDrawPlayerOnTop = true;
            //    }
            //}

            //foreach (var item in ScriptProcessor.scriptTempObjects)
            //{
            //    if (item is NPC)
            //    {
            //        (item as NPC).Draw(spriteBatch);
            //        (item as NPC).baseCharacter.Draw(spriteBatch);
            //    }
            //}
            var buildings = testBuildings.FindAll(b => b.Contains(camera, zoom));
            var listB = new List<Building>();
            if (PlayerController.selectedSprite != null)
            {
                listB = new List<Building>(buildings.FindAll(b => b.boundingZone.Contains(PlayerController.selectedSprite.spriteGameSize) || b.boundingZone.Intersects(PlayerController.selectedSprite.spriteGameSize)));
                listB.RemoveAll(b => b.boundingZone.Height + b.boundingZone.Y > PlayerController.selectedSprite.spriteGameSize.Height + PlayerController.selectedSprite.spriteGameSize.Y);

            }

            var listSpritesNearBuilding = new List<BaseSprite>();
            if (PlayerController.selectedSprite != null)
            {
                foreach (var item in listB)
                {
                    listSpritesNearBuilding.AddRange(temp.FindAll(s => s.spriteGameSize.Contains(item.boundingZone) || s.spriteGameSize.Intersects(item.boundingZone)));
                }
            }

            var list = new List<BaseSprite>();
            if (PlayerController.selectedSprite != null)
            {
                list = temp.FindAll(s => s.spriteGameSize.Contains(PlayerController.selectedSprite.spriteGameSize) || s.spriteGameSize.Intersects(PlayerController.selectedSprite.spriteGameSize));
                list.Remove(PlayerController.selectedSprite);
                list.RemoveAll(s => !s.IsOnTop(PlayerController.selectedSprite));

            }

            foreach (var sprite in listSpritesNearBuilding)
            {
                sprite.Draw(spriteBatch);
                temp.Remove(sprite);
            }


            foreach (var item in buildings.FindAll(b => listB.Contains(b)))
            {
                spriteBatch.Draw(item.buildingRender, item.boundingZone, Color.White);
            }


            if (!GameProcessor.bStartCombatZoom)
            {

                foreach (var sprite in list)
                {
                    sprite.Draw(spriteBatch);
                }
                if (PlayerController.selectedSprite != null)
                {
                    PlayerController.selectedSprite.Draw(spriteBatch);
                }

                //foreach (var item in PlayerController.selectedSprite.closeProximityHitboxes())
                //{
                //    spriteBatch.Draw(Game1.hitboxHelp, item, Game1.hitboxHelp.Bounds, Color.Red);
                //}
                foreach (var sprite in temp)
                {
                    if (!list.Contains(sprite))
                    {
                        sprite.Draw(spriteBatch);
                        //foreach (var item in sprite.closeProximityHitboxes())
                        //{
                        //    spriteBatch.Draw(Game1.hitboxHelp,item,Game1.hitboxHelp.Bounds,Color.Red);
                        //}
                    }
                }

            }
            else if (GameProcessor.bStartCombatZoom)
            {
                var temp1 = temp.FindAll(s => typeof(BaseCharacter) != s.GetType());
                var temp2 = temp.FindAll(s => typeof(BaseCharacter) == s.GetType() && (s == GameProcessor.battleCaster || s == GameProcessor.battleTarget || BattleGUI.surroundingRbcChars.Find(kv => kv.Key == s).Key != null));
                var temp3 = temp.FindAll(s => typeof(BaseCharacter) == s.GetType() && (s != GameProcessor.battleCaster && s != GameProcessor.battleTarget));
                foreach (var sprite in temp1)
                {

                    sprite.Draw(spriteBatch);
                    if (!bDrawPlayerOnTop && sprite.IsOnTop(PlayerController.selectedSprite))
                    {
                        bDrawPlayerOnTop = true;
                    }

                }
                foreach (var sprite in temp2)
                {

                    sprite.Draw(spriteBatch);
                    if (!bDrawPlayerOnTop && sprite.IsOnTop(PlayerController.selectedSprite))
                    {
                        bDrawPlayerOnTop = true;
                    }

                }



                //foreach (var sprite in temp3)
                //{

                //   sprite.Draw(spriteBatch,Color.Black);
                //    if (!bDrawPlayerOnTop && sprite.bIsOnTop(PlayerController.selectedSprite))
                //    {
                //        bDrawPlayerOnTop = true;
                //    }

                //}
            }



            foreach (var item in buildings.FindAll(b => !listB.Contains(b)))
            {
                spriteBatch.Draw(item.buildingRender, item.boundingZone, Color.White);
            }

            var allActiveLightsInCamera = activeLights.FindAll(og => og.spriteGameSize.Contains(trueCamera) || og.spriteGameSize.Intersects(trueCamera));
            foreach (var item in allActiveLightsInCamera)
            {
                //   item.Draw(spriteBatch);
            }

            if (!GameProcessor.bInCombat && bDrawPlayerOnTop && GameProcessor.bIsInGame && PlayerController.selectedSprite != null)
            {
                PlayerController.selectedSprite.Draw(spriteBatch);
            }

        }

        #region Combat Processor Process Stuff Here
        static internal bool bGenerateCombatObjectInfo = false;
        static List<objectInfo> combatObjectInfo = new List<objectInfo>();

        internal static bool bIsFromCombat(objectInfo oi)
        {
            return combatObjectInfo.Contains(oi);
        }
        #endregion

        internal static void GenerateCombatInfo(List<BaseCharacter> encounterEnemies, List<BaseCharacter> heroCharacters)
        {
            bGenerateCombatObjectInfo = false;
            combatObjectInfo.Clear();
            encounterEnemies.ForEach(enemy => enemy.UpdatePosition());
            encounterEnemies.ForEach(enemy => combatObjectInfo.Add(new objectInfo(enemy, enemy.getHeightIndicator(), objectInfo.type.Character, enemy.trueMapSize(), false, true)));

            heroCharacters.ForEach(hero => hero.UpdatePosition());
            heroCharacters.ForEach(hero => combatObjectInfo.Add(new objectInfo(hero, hero.getHeightIndicator(), objectInfo.type.Character, hero.trueMapSize(), false, true)));
        }

        private void DrawObjectLayer(SpriteBatch spriteBatch, GameContentDataBase gcdb, List<objectInfo> temp, List<objectInfo> Except = default(List<objectInfo>))
        {
            List<objectInfo> ExceptThese = new List<objectInfo>();
            if (Except != default(List<objectInfo>))
            {
                ExceptThese = new List<objectInfo>(Except);
            }

            bool bDrawPlayerOnTop = false;

            var allBuildings = temp.FindAll(oi => oi.objectType == objectInfo.type.Building).Select(boi => boi.obj).Cast<Building>().ToList();
            var remainingSprites = new List<objectInfo>(temp);

            allBuildings = allBuildings.OrderBy(b => b.heightIndicator()).ToList();
            remainingSprites.RemoveAll(oi => allBuildings.Contains(oi.obj));
            
            if (CombatProcessor.bIsRunning)
            {
                remainingSprites.RemoveAll(oi => oi.obj == GameProcessor.mainControllerBeforeCombat);
                combatObjectInfo.RemoveAll(oi => !(oi.obj as BaseCharacter).IsAlive());
                combatObjectInfo.ForEach(oi => oi.heightIndicator = (oi.obj as BaseCharacter).getHeightIndicator());
                remainingSprites.AddRange(combatObjectInfo);
                remainingSprites = remainingSprites.OrderBy(oi => oi.heightIndicator).ToList();
                if (CombatProcessor.bMainCombat)
                {

                    //
                }
            }
            remainingSprites.RemoveAll(s => ExceptThese.Contains(s));

            var buildingsBackGround = new List<Building>(); //As in behind the controller
            if (PlayerController.selectedSprite != null)
            {
                buildingsBackGround = new List<Building>(allBuildings.FindAll(b => b.boundingZone.Contains(PlayerController.selectedSprite.trueMapSize()) || b.boundingZone.Intersects(PlayerController.selectedSprite.trueMapSize())));
                buildingsBackGround.RemoveAll(b => b.heightIndicator() > PlayerController.selectedSprite.getHeightIndicator());
            }

            //var spritesBehindPlayer = new List<objectInfo>();
            //if (PlayerController.selectedSprite != null)
            //{
            //    spritesBehindPlayer = remainingSprites.FindAll(s => s.mapSize.Contains(PlayerController.selectedSprite.trueMapSize()) || s.mapSize.Intersects(PlayerController.selectedSprite.trueMapSize()));
            //    spritesBehindPlayer.RemoveAll(sbp => sbp.obj == PlayerController.selectedSprite);
            //    spritesBehindPlayer.RemoveAll(s => s.heightIndicator > PlayerController.selectedSprite.getHeightIndicator());

            //}

            foreach (var item in buildingsBackGround)
            {
                var ItemsNearBuilding = remainingSprites.FindAll(s => s.objectType != objectInfo.type.Building && s.mapSize.Contains(item.boundingZone) || s.mapSize.Intersects(item.boundingZone));
                var ItemsBehindBuilding = ItemsNearBuilding.FindAll(s => s.heightIndicator <= item.heightIndicator());
                foreach (var sprite in ItemsBehindBuilding)
                {
                    sprite.Draw(spriteBatch);
                    ItemsNearBuilding.Remove(sprite); //faster then except function
                }

                spriteBatch.Draw(item.buildingRender, item.boundingZone, Color.White);

                foreach (var sprite in ItemsNearBuilding)
                {
                    sprite.Draw(spriteBatch);
                }

                remainingSprites.RemoveAll(oi => ItemsNearBuilding.Contains(oi));
                remainingSprites.RemoveAll(oi => ItemsBehindBuilding.Contains(oi));
            }



            //foreach (var sprite in spritesBehindPlayer)
            //{
            // //   sprite.Draw(spriteBatch);
            //}

            //if (PlayerController.selectedSprite != null)
            //{
            //  //  PlayerController.selectedSprite.Draw(spriteBatch);
            //}
            if (PlayerController.selectedSprite != null)
            {
                PlayerController.selectedSprite.UpdatePosition();
            }

            foreach (var sprite in remainingSprites)
            {
                sprite.Draw(spriteBatch);
                //spriteBatch.Draw(Game1.hitboxHelp,sprite.mapSize,Color.White);
            }





            foreach (var item in allBuildings.Except(buildingsBackGround).ToList())
            {
                // spriteBatch.Draw(item.buildingRender, item.boundingZone, Color.White);
                var ItemsNearBuilding = remainingSprites.FindAll(s => s.objectType != objectInfo.type.Building && s.mapSize.Contains(item.boundingZone) || s.mapSize.Intersects(item.boundingZone));
                var ItemsBehindBuilding = ItemsNearBuilding.FindAll(s => s.heightIndicator <= item.heightIndicator());
                foreach (var sprite in ItemsBehindBuilding)
                {
                    sprite.Draw(spriteBatch);
                    ItemsNearBuilding.Remove(sprite); //faster then except function
                }

                spriteBatch.Draw(item.buildingRender, item.boundingZone, Color.White);

                foreach (var sprite in ItemsNearBuilding)
                {
                    sprite.Draw(spriteBatch);
                }

                remainingSprites.RemoveAll(oi => ItemsNearBuilding.Contains(oi));
                remainingSprites.RemoveAll(oi => ItemsBehindBuilding.Contains(oi));
            }

            //foreach (var item in allActiveLightsInCamera)
            //{
            //    //   item.Draw(spriteBatch);
            //}


            //PlayerController.selectedSprite.Draw(spriteBatch);
            if (!GameProcessor.bInCombat && bDrawPlayerOnTop && GameProcessor.bIsInGame && PlayerController.selectedSprite != null)
            {
                PlayerController.selectedSprite.Draw(spriteBatch);
            }
        }



        public List<BaseSprite> returnAllObjectsInCamera(Rectangle camera, float zoom = 1)
        {
            Rectangle trueCamera = new Rectangle((int)(camera.X) - (int)(500 / zoom), (int)(camera.Y) - (int)(500 / zoom), (int)(1366 / zoom) + (int)(500 / zoom), (int)(768 / zoom) + (int)(500 / zoom));

            var allObjectsInCamera = mapSprites.FindAll(o => o.spriteGameSize.Contains(trueCamera) || o.spriteGameSize.Intersects(trueCamera));

            var allObjectLightsInCamera = activeLights.FindAll(o => o.Contains(trueCamera));
            allObjectLightsInCamera.ForEach(objg => allObjectsInCamera.Add(objg as BaseSprite));

            var allObjectGroupsInCamera = mapObjectGroups.FindAll(og => og.Contains(trueCamera));
            allObjectGroupsInCamera.ForEach(objg => allObjectsInCamera.AddRange(objg.groupItems));

            var allNPCsInCamera = mapNPCs.FindAll(npc => npc.baseCharacter.spriteGameSize.Contains(trueCamera) || npc.baseCharacter.spriteGameSize.Intersects(trueCamera));
            allNPCsInCamera.ForEach(npcs => allObjectsInCamera.Add(npcs.baseCharacter));

            allObjectsInCamera.AddRange(ScriptProcessor.returnTempObjsAsBaseSprites());

            if (PlayerController.selectedSprite != null)
            {
                allObjectsInCamera.Add(PlayerController.selectedSprite);
            }

            if (CombatProcessor.bIsRunning)
            {
                if (!CombatProcessor.bMainCombat)
                {
                    if (CombatProcessor.heroCharacters.Count != 0)
                    {
                        if (PlayerController.selectedSprite == null)
                        {
                            allObjectsInCamera.AddRange(CombatProcessor.heroCharacters);
                            allObjectsInCamera.AddRange(CombatProcessor.encounterEnemies);
                        }
                        else
                        {
                            allObjectsInCamera.AddRange(CombatProcessor.heroCharacters.FindAll(c => c != PlayerController.selectedSprite));
                            allObjectsInCamera.AddRange(CombatProcessor.encounterEnemies);
                        }
                    }

                }
                else if (CombatProcessor.bMainCombat)
                {
                    if (EncounterInfo.encounterGroups.Count != 0)
                    {
                        foreach (var group in EncounterInfo.encounterGroups)
                        {
                            foreach (var character in group.charactersInGroup)
                            {
                                if (PlayerController.selectedSprite == null)
                                {
                                    allObjectsInCamera.Add(character);
                                }
                                else if (character != PlayerController.selectedSprite)
                                {
                                    allObjectsInCamera.Add(character);
                                }
                            }
                        }
                    }
                }
            }

            //   PlayerController.selectedSprite.spriteGameSize.Y -= 5564;
            allObjectsInCamera = allObjectsInCamera.OrderBy(obj => obj.spriteGameSize.Y + obj.spriteGameSize.Height).ToList();
            // PlayerController.selectedSprite.spriteGameSize.Y += 5564;
            //  allObjectsInCamera.Reverse();

            //var lastObjBehindChar = allObjectsInCamera.Find(obj=>(obj.spriteGameSize.Contains(PlayerController.selectedSprite.spriteGameSize)|| obj.spriteGameSize.Intersects(PlayerController.selectedSprite.spriteGameSize))&&obj.spriteGameSize.Y<PlayerController.selectedSprite.position.Y);
            //allObjectsInCamera.Insert(allObjectsInCamera.IndexOf(lastObjBehindChar),PlayerController.selectedSprite);

            List<BaseSprite> bottomLayer = new List<BaseSprite>();
            foreach (var item in allObjectsInCamera)
            {
                if (item.shapeID == 10)
                {
                    bottomLayer.Add(item);
                }
            }
            allObjectsInCamera.RemoveAll(obj => obj.shapeID == 10);
            // allObjectsInCamera.Insert(0, PlayerController.selectedSprite);
            allObjectsInCamera.InsertRange(0, bottomLayer);

            //Console.WriteLine(PlayerController.selectedSprite.position.Y);

            if (GameProcessor.bStartCombatZoom)
            {
                allObjectsInCamera.RemoveAll(obj => obj.GetType() == typeof(BaseCharacter) && (obj != GameProcessor.battleCaster && obj != GameProcessor.battleTarget && BattleGUI.surroundingRbcChars.Find(kv => kv.Key == obj).Key == null));
            }

            return allObjectsInCamera;

        }

        public List<objectInfo> returnAllObjectsInCameraAsGeneric(Rectangle camera = default(Rectangle), float zoom = 1)
        {
            Rectangle trueCamera = new Rectangle((int)(camera.X) - (int)(500 / zoom), (int)(camera.Y) - (int)(500 / zoom), (int)(1366 / zoom) + (int)(500 / zoom), (int)(768 / zoom) + (int)(500 / zoom));

            //var allObjectsInCamera = mapSprites.FindAll(o => o.spriteGameSize.Contains(trueCamera) || o.spriteGameSize.Intersects(trueCamera));

            //var allObjectLightsInCamera = activeLights.FindAll(o => o.Contains(trueCamera));

            //var allObjectGroupsInCamera = mapObjectGroups.FindAll(og => og.Contains(trueCamera));

            //var allNPCsInCamera = mapNPCs.FindAll(npc => npc.baseCharacter.spriteGameSize.Contains(trueCamera) || npc.baseCharacter.spriteGameSize.Intersects(trueCamera));

            //allObjectsInCamera.AddRange(ScriptProcessor.returnTempObjsAsBaseSprites());
            List<objectInfo> objInfoList = new List<objectInfo>();

            foreach (var chunk in chunksToConsider)
            {
                bool bMustAddToStaticList = chunk.staticObjectInfoList.Count == 0 ? true : false;
                if (!bMustAddToStaticList)
                {
                    bMustAddToStaticList = chunk.staticObjectInfoList.Count == chunk.objectsInChunk.Count ? false : true;
                }

                if (bMustAddToStaticList)
                {
                    chunk.staticObjectInfoList.Clear();
                    foreach (var obj in chunk.objectsInChunk)
                    {
                        if (obj.GetType() == typeof(BaseSprite))
                        {
                            var temp = obj as BaseSprite;
                            objInfoList.Add(new objectInfo(temp, temp.getHeightIndicator(), objectInfo.type.Sprite, temp.trueMapSize(), temp.script == null ? false : true, temp.bIsVisible));
                            if (bMustAddToStaticList)
                            {
                                chunk.staticObjectInfoList.Add(objInfoList.Last());
                            }
                        }
                        else if (obj.GetType() == typeof(BaseCharacter))
                        {
                            var temp = obj as BaseCharacter;
                            objInfoList.Add(new objectInfo(temp, temp.getHeightIndicator(), objectInfo.type.Character, temp.trueMapSize(), temp.script == null ? false : true));
                            if (bMustAddToStaticList)
                            {
                                chunk.staticObjectInfoList.Add(objInfoList.Last());
                            }
                        }
                        else if (obj.GetType() == typeof(SpriteLight))
                        {
                            var temp = obj as SpriteLight;
                            objInfoList.Add(new objectInfo(temp, temp.getHeightIndicator(), objectInfo.type.Sprite, temp.trueMapSize(), temp.script == null ? false : true));
                            if (bMustAddToStaticList)
                            {
                                chunk.staticObjectInfoList.Add(objInfoList.Last());
                            }
                        }
                        else if (obj.GetType() == typeof(NPC))
                        {
                            //  var temp = obj as NPC;
                            // objInfoList.Add(new objectInfo(temp, temp.baseCharacter.getHeightIndicator(), objectInfo.type.Character, temp.baseCharacter.trueMapSize()));
                        }
                        else if (obj.GetType() == typeof(ObjectGroup))
                        {
                            var temp = obj as ObjectGroup;
                            temp.CalculateGroupSize();
                            temp.CalculateFxDrawLocBeforeFirstDraw();
                            temp.CalculateHeightIndicator();
                            temp.GenerateTrueMapSize();
                            objInfoList.Add(new objectInfo(temp, temp.heightIndicator, objectInfo.type.ObjectGroup, temp.trueMapSize, false));
                            if (bMustAddToStaticList)
                            {
                                chunk.staticObjectInfoList.Add(objInfoList.Last());
                            }
                        }
                        else if (obj.GetType() == typeof(Building))
                        {
                            var temp = obj as Building;
                            objInfoList.Add(new objectInfo(temp, temp.heightIndicator(), objectInfo.type.Building, temp.MapSize(), false));
                            if (bMustAddToStaticList)
                            {
                                chunk.staticObjectInfoList.Add(objInfoList.Last());
                            }
                        }
                    }

                    chunk.staticObjectInfoList = chunk.staticObjectInfoList.Distinct().ToList();
                }
                else if (!bMustAddToStaticList)
                {
                    objInfoList.AddRange(chunk.staticObjectInfoList);
                }
            }

            foreach (var obj in MapObjectHelpClass.objectsToUpdateOutsideOfMap)
            {
                if (obj.GetType() == typeof(NPC) && (obj as NPC).IsOnCurrentMap(this))
                {
                    var temp = obj as NPC;
                    objInfoList.Add(new objectInfo(temp, temp.baseCharacter.getHeightIndicator(), objectInfo.type.Character, temp.baseCharacter.trueMapSize(), true));
                }
            }

            if (PlayerController.selectedSprite != null)
            {
                objInfoList.Add(new objectInfo(PlayerController.selectedSprite, PlayerController.selectedSprite.getHeightIndicator(), objectInfo.type.Character, PlayerController.selectedSprite.trueMapSize(), false));
            }

            if (CombatProcessor.bIsRunning)
            {
                //if (!CombatProcessor.bMainCombat)
                //{
                //    if (CombatProcessor.heroCharacters.Count != 0)
                //    {
                //        if (PlayerController.selectedSprite == null)
                //        {
                //            allObjectsInCamera.AddRange(CombatProcessor.heroCharacters);
                //            allObjectsInCamera.AddRange(CombatProcessor.encounterEnemies);
                //        }
                //        else
                //        {
                //            allObjectsInCamera.AddRange(CombatProcessor.heroCharacters.FindAll(c => c != PlayerController.selectedSprite));
                //            allObjectsInCamera.AddRange(CombatProcessor.encounterEnemies);
                //        }
                //    }

                //}
                //else if (CombatProcessor.bMainCombat)
                //{
                //    if (EncounterInfo.encounterGroups.Count != 0)
                //    {
                //        foreach (var group in EncounterInfo.encounterGroups)
                //        {
                //            foreach (var character in group.charactersInGroup)
                //            {
                //                if (PlayerController.selectedSprite == null)
                //                {
                //                    allObjectsInCamera.Add(character);
                //                }
                //                else if (character != PlayerController.selectedSprite)
                //                {
                //                    allObjectsInCamera.Add(character);
                //                }
                //            }
                //        }
                //    }
                //}
            }

            objInfoList = objInfoList.OrderBy(oi => oi.heightIndicator).ToList();

            // Console.WriteLine("Size before: " + objInfoList.Count);
            objInfoList = objInfoList.Distinct().ToList();
            //Console.WriteLine("Size after: " + objInfoList.Count);
            //List<BaseSprite> bottomLayer = new List<BaseSprite>(); //objectsAlways below character
            //foreach (var item in allObjectsInCamera)
            //{
            //    if (item.shapeID == 10)
            //    {
            //        bottomLayer.Add(item);
            //    }
            //}
            //allObjectsInCamera.RemoveAll(obj => obj.shapeID == 10);
            //// allObjectsInCamera.Insert(0, PlayerController.selectedSprite);
            //allObjectsInCamera.InsertRange(0, bottomLayer);

            ////Console.WriteLine(PlayerController.selectedSprite.position.Y);

            //if (GameProcessor.bStartCombatZoom)
            //{
            //    allObjectsInCamera.RemoveAll(obj => obj.GetType() == typeof(BaseCharacter) && (obj != GameProcessor.battleCaster && obj != GameProcessor.battleTarget && BattleGUI.surroundingRbcChars.Find(kv => kv.Key == obj).Key == null));
            //}

            return objInfoList;

        }

        public void DrawSprites(SpriteBatch spriteBatch, Rectangle camera, float zoom = 1)
        {
            if (subMapIndex == -1)
            {
                foreach (var sprite in mapSprites)
                {
                    sprite.Draw(spriteBatch);
                }
            }
            else
            {
                subMaps[subMapIndex].DrawSprites(spriteBatch, camera, zoom);
            }


        }

        public override string ToString()
        {
            return mapName + "  Identifier: " + identifier.ToString();
        }

        public List<BasicTile> canMoveToThisTile(Point coord, BaseSprite bs)
        {
            List<MapChunk> consideredChunks = new List<MapChunk>();
            //foreach (var item in mapTilesLayer)
            //{
            //    int countBefore = item.Count;
            //    item.RemoveAll(c => c.Value.Count == 0);
            //    if (item.Count != countBefore)
            //    {

            //    }
            //}



            foreach (var chunk in Chunks)
            {
                foreach (var hb in bs.closeProximityHitboxes())
                {
                    // consideredChunks.Add(item.FindAll(l => l.Key.Contains(coord.ToVector2() * 64)));
                    if (chunk.region.Contains(hb))
                    {
                        consideredChunks.Add(chunk);
                    }
                    //  consideredChunks.AddRange(Chunks.FindAll(c => c.region.Contains(hb)));
                    break;
                }
            }

            //foreach (var list in consideredChunks)
            //{
            //    foreach (var item in list)
            //    {
            //        if (item.Value.Find(t => t.positionGrid == coord.ToVector2()) != default(BasicTile))
            //        {
            //            return false;
            //        }
            //    }
            //}
            bool tileCollision = false;
            Rectangle sgs = new Rectangle(bs.spriteGameSize.Location + coord, bs.spriteGameSize.Size);
            var tempList = new List<BasicTile>();

            for (int i = 0; i < 3; i++)
            {
                foreach (var chunk in consideredChunks)
                {
                    if (chunk.layer.Contains(i))
                    {
                        tempList.AddRange(chunk.returnLayer(i).FindAll(t => t.hitboxes.Count != 0 && (sgs.Contains(t.mapPosition) || sgs.Intersects(t.mapPosition))));
                    }
                }

            }

            return tempList;
        }

        public bool isWithinMap(BaseSprite bs, Vector2 movement)
        {
            List<MapChunk> consideredChunks = new List<MapChunk>();

            Rectangle mapSize = new Rectangle(bs.trueMapSize().Location, bs.trueMapSize().Size);


            mapSize.X += (int)((movement.X)) - 20;
            mapSize.Y += (int)((movement.Y)) - 20;
            mapSize.Width += 25 * 2;
            mapSize.Height += 25 * 2;

            foreach (var chunk in Chunks)
            {

                if (chunk.region.Contains(mapSize) || chunk.region.Intersects(mapSize))
                {
                    consideredChunks.Add(chunk);
                }
            }

            var tempList = new List<BasicTile>();

            for (int i = 0; i < 3; i++)
            {
                foreach (var chunk in consideredChunks)
                {
                    if (chunk.layer.Contains(i))
                    {
                        tempList.AddRange(chunk.returnLayer(i).FindAll(t => t.Contains(mapSize)));
                    }
                }

            }



            foreach (var hb in bs.closeProximityHitboxes())
            {
                Rectangle hbT = new Rectangle(hb.Location + movement.ToPoint(), hb.Size);
                if (tempList.Find(t => t.Contains(hbT)) == null)
                {
                    return false;
                }
            }
            return true;
        }

        public void CreateSubMap(int x, int y)
        {
            subMaps.Add(new BasicMap());
            foreach (var item in subMaps)
            {
                item.identifier = subMaps.IndexOf(item);
            }
        }

        public void DeleteSubMap(int s)
        {
            subMaps.RemoveAt(s);
            foreach (var item in subMaps)
            {
                item.identifier = subMaps.IndexOf(item);
            }
        }

        public List<BasicTile> possibleTiles(Rectangle rArea)
        {
            List<BasicTile> temp = new List<BasicTile>();


            //What this line does is, take the first layer, the ground layer, as the the base of the map and then check whether there are collisions on the same
            //tile position on higher layers, if so, don't add this tile as a gamemap tile.



            foreach (var chunk in chunksToConsider)
            {
                temp.AddRange(chunk.returnLayer(0).FindAll(t => t.hitboxes.Count == 0));
                foreach (var layer in chunk.layer.FindAll(v => v != 0 && v < 3))
                {
                    var tempList = chunk.returnLayer(layer).Where(t => t.hitboxes.Count != 0).ToList();
                    foreach (var tile in tempList)
                    {
                        temp.RemoveAll(t => t.positionGrid == tile.positionGrid);
                    }
                }
            }
            temp.RemoveAll(t => !rArea.Contains(t.mapPosition));


            var tempL_1 = GameProcessor.loadedMap.mapSprites.FindAll(x => x.bHasCollision && (rArea.Contains(x.spriteGameSize) || rArea.Intersects(x.spriteGameSize)));
            var tempL_2 = GameProcessor.loadedMap.mapObjectGroups.FindAll(x => x.Contains(rArea));

            List<BasicTile> finalCheck = new List<BasicTile>();
            foreach (var item in tempL_1)
            {
                finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
            }

            foreach (var item in tempL_2)
            {
                finalCheck.AddRange(temp.FindAll(t => item.Contains(t.mapPosition)));
            }

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                item.UpdatePosition();
                finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
            }

            if (PlayerController.selectedSprite != null)
            {
                finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(PlayerController.selectedSprite.spriteGameSize) || t.mapPosition.Contains(PlayerController.selectedSprite.spriteGameSize)));
            }

            temp.RemoveAll(t => finalCheck.Contains(t));

            return temp;
        }

        public List<BasicTile> possibleTilesWithController(Rectangle rArea, BasicMap bm)
        {
            List<BasicTile> temp = new List<BasicTile>();
            var chunksToConsider = bm.Chunks.FindAll(c => c.region.Contains(rArea) || c.region.Intersects(rArea));
            foreach (var chunk in chunksToConsider)
            {
                temp.AddRange(chunk.returnLayer(0).FindAll(t => t.hitboxes.Count == 0));
                foreach (var layer in chunk.layer.FindAll(v => v != 0 && v < 3))
                {
                    var tempList = chunk.returnLayer(layer).Where(t => t.hitboxes.Count != 0).ToList();
                    foreach (var tile in tempList)
                    {
                        temp.RemoveAll(t => t.positionGrid == tile.positionGrid);
                    }
                }
            }
            temp.RemoveAll(t => !rArea.Contains(t.mapPosition));

            var tempL_1 = bm.mapSprites.FindAll(x => x.bHasCollision && (rArea.Contains(x.trueMapSize()) || rArea.Intersects(x.trueMapSize())));
            var tempL_2 = bm.mapObjectGroups.FindAll(x => x.Contains(rArea) && x.bHasCollision);

            List<BasicTile> finalCheck = new List<BasicTile>();
            foreach (var item in tempL_1)
            {
                foreach (var hb in item.closeProximityHitboxes())
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(hb) || t.mapPosition.Contains(hb)));
                }
                if (item.Contains(new Vector2(832, 704)))
                {

                }
            }

            foreach (var item in tempL_2)
            {
                foreach (var gi in item.groupItems.FindAll(i => i.bHasCollision))
                {
                    foreach (var hb in gi.closeProximityHitboxes())
                    {
                        finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(hb) || t.mapPosition.Contains(hb)));
                    }
                    if (item.Contains(new Vector2(832, 704)))
                    {

                    }
                }
            }

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                // finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
            }

            //  finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(PlayerController.selectedSprite.spriteGameSize) || t.mapPosition.Contains(PlayerController.selectedSprite.spriteGameSize)));

            temp.RemoveAll(t => finalCheck.Contains(t));

            return temp;
        }

        public List<BasicTile> possibleTilesWithController(Rectangle rArea, MapChunk mc, BasicMap bm)
        {
            List<BasicTile> temp = new List<BasicTile>();
            var chunksToConsider = new List<MapChunk> { mc };
            foreach (var chunk in chunksToConsider)
            {
                temp.AddRange(chunk.returnLayer(0).FindAll(t => t.hitboxes.Count == 0));
                foreach (var layer in chunk.layer.FindAll(v => v != 0 && v < 3))
                {
                    var tempList = chunk.returnLayer(layer).Where(t => t.hitboxes.Count != 0).ToList();
                    foreach (var tile in tempList)
                    {
                        temp.RemoveAll(t => t.positionGrid == tile.positionGrid);
                    }
                }
            }
            temp.RemoveAll(t => !rArea.Contains(t.mapPosition));


            if (mc.objectsInChunk == null)
            {

                mc.Checkhunk();
                bm.CheckChunkSpecific(mc);
            }
            var tempL_1 = mc.objectsInChunk.FindAll(o => o is BaseSprite).Select(o => o).ToList().Cast<BaseSprite>().ToList().FindAll(x => x.bHasCollision && (rArea.Contains(x.trueMapSize()) || rArea.Intersects(x.trueMapSize())));
            var tempL_2 = mc.objectsInChunk.FindAll(o => o is ObjectGroup).Select(o => o).ToList().Cast<ObjectGroup>().ToList().FindAll(x => x.Contains(rArea) && x.bHasCollision);

            List<BasicTile> finalCheck = new List<BasicTile>();
            foreach (var item in tempL_1)
            {
                foreach (var hb in item.closeProximityHitboxes())
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(hb) || t.mapPosition.Contains(hb)));
                }
                if (item.Contains(new Vector2(832, 704)))
                {

                }
            }

            foreach (var item in tempL_2)
            {
                foreach (var gi in item.groupItems.FindAll(i => i.bHasCollision))
                {
                    foreach (var hb in gi.closeProximityHitboxes())
                    {
                        finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(hb) || t.mapPosition.Contains(hb)));
                    }
                    if (item.Contains(new Vector2(832, 704)))
                    {

                    }
                }
            }

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                // finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
            }

            //  finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(PlayerController.selectedSprite.spriteGameSize) || t.mapPosition.Contains(PlayerController.selectedSprite.spriteGameSize)));

            temp.RemoveAll(t => finalCheck.Contains(t));

            return temp;
        }

        public List<BasicTile> possibleTilesWithController(Rectangle rArea, BaseSprite bs)
        {
            List<BasicTile> temp = new List<BasicTile>();
            var chunksToConsider = Chunks.FindAll(c => c.region.Contains(rArea) || c.region.Intersects(rArea));
            foreach (var chunk in chunksToConsider)
            {
                temp.AddRange(chunk.returnLayer(0).FindAll(t => t.hitboxes.Count == 0));
                foreach (var layer in chunk.layer.FindAll(v => v != 0 && v < 3))
                {
                    var tempList = chunk.returnLayer(layer).Where(t => t.hitboxes.Count != 0).ToList();
                    foreach (var tile in tempList)
                    {
                        temp.RemoveAll(t => t.positionGrid == tile.positionGrid);
                    }
                }
            }
            temp.RemoveAll(t => !rArea.Contains(t.mapPosition));
            temp = temp.Distinct().ToList();

            var tempL_1 = GameProcessor.loadedMap.mapSprites.FindAll(x => x.bHasCollision && (rArea.Contains(x.trueMapSize()) || rArea.Intersects(x.trueMapSize())));
            var tempL_2 = GameProcessor.loadedMap.mapObjectGroups.FindAll(x => x.Contains(rArea));

            List<BasicTile> finalCheck = new List<BasicTile>();
            foreach (var item in tempL_1)
            {
                foreach (var hb in item.closeProximityHitboxes())
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(hb) || t.mapPosition.Contains(hb)));
                }
            }

            foreach (var item in tempL_2)
            {
                foreach (var gi in item.groupItems.FindAll(i => i.bHasCollision))
                {
                    foreach (var hb in gi.closeProximityHitboxes())
                    {
                        finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(hb) || t.mapPosition.Contains(hb)));
                    }
                }
            }

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                // finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
            }

            //  finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(PlayerController.selectedSprite.spriteGameSize) || t.mapPosition.Contains(PlayerController.selectedSprite.spriteGameSize)));

            temp.RemoveAll(t => finalCheck.Contains(t));


            List<BasicTile> tilesConnected = new List<BasicTile> { temp.Find(tile => tile.mapPosition.Contains(bs.trueMapSize()) || tile.mapPosition.Intersects(bs.trueMapSize())) };
            List<BasicTile> tilesChecked = new List<BasicTile>();


            if (tilesConnected[0] == null)
            {
                tilesConnected.RemoveAt(0);
                Console.WriteLine("Soft error, placed on tileblocker, finding nearest valid position...");
                for (int i = 0; i < 20; i++)
                {
                    var tTile = temp.Find(tile => tile.distanceCoordsFrom(bs.position) == i);
                    if (tTile != null)
                    {
                        tilesConnected.Add(tTile);
                        PlayerController.selectedSprite.position = tTile.positionGrid * 64;
                        break;
                    }
                }
            }

            List<BasicTile> tilesLeftToCheck = new List<BasicTile>(tilesConnected);

            while (tilesLeftToCheck.Count != 0)
            {
                var checkTile = tilesLeftToCheck[0];
                tilesLeftToCheck.Remove(checkTile);
                tilesChecked.Add(checkTile);
                var surroundingTiles = temp.Except(tilesConnected).ToList().FindAll(tile => tile.TileDistance(checkTile) == 1);
                tilesLeftToCheck.AddRange(surroundingTiles.Except(tilesChecked));
                tilesConnected.AddRange(surroundingTiles.Except(tilesConnected));
            }

            return tilesConnected;
        }

        public List<BasicTile> possibleTilesGameZone(List<BasicTile> gameZone)
        {
            List<BasicTile> temp = new List<BasicTile>(gameZone);

            List<BasicTile> finalCheck = new List<BasicTile>();

            temp.RemoveAll(t => t == null);

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                if (EncounterInfo.encounterGroups.Count != 0 && temp.Count != 0 && EncounterInfo.currentTurn().currentEnemy != null && item != EncounterInfo.currentTurn().currentEnemy.character)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else if (EncounterInfo.encounterGroups.Count != 0 && temp.Count != 0 && EncounterInfo.currentTurn().currentEnemy == null)
                {

                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else
                {
                    if (!CombatProcessor.bMainCombat)
                    {
                        finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                    }
                }
            }

            foreach (var item in CombatProcessor.heroCharacters)
            {
                if (EncounterInfo.encounterGroups.Count != 0 && temp.Count != 0 && EncounterInfo.currentTurn().selectedCharTurn != null && item != EncounterInfo.currentTurn().selectedCharTurn.character)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else if (EncounterInfo.encounterGroups.Count != 0 && temp.Count != 0 && !EncounterInfo.currentTurn().bIsPlayerTurnSet)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else
                {
                    if (!CombatProcessor.bMainCombat)
                    {
                        finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));

                    }
                }
                if (EncounterInfo.encounterGroups.Count != 0 && temp.Count != 0 && EncounterInfo.currentTurn().selectedCharTurn == null)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
            }

            if (EncounterInfo.encounterGroups.Count != 0 && temp.Count != 0 && EncounterInfo.currentTurn().bIsPlayerTurnSet && PlayerController.selectedSprite != null)
            {
                var t = finalCheck.Find(ti => ti.positionGrid.ToPoint() == (PlayerController.selectedSprite.position / 64).ToPoint());
                if (t != null)
                {
                    finalCheck.Remove(t);
                }
            }


            temp.RemoveAll(t => finalCheck.Contains(t));


            return temp;
        }

        public List<BasicTile> possibleTilesGameZoneForEnemy(List<BasicTile> gameZone, BaseSprite enemy)
        {
            List<BasicTile> temp = new List<BasicTile>(gameZone);
            temp.RemoveAll(t => t == null);

            List<BasicTile> finalCheck = new List<BasicTile>();

            if (PlayerController.selectedSprite != null)
            {
                var tempPos = PlayerController.selectedSprite.positionToMapCoords();
                temp.RemoveAll(t => t.positionGrid == tempPos);
            }

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().currentEnemy != null && item != EncounterInfo.currentTurn().currentEnemy.character)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().currentEnemy == null)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else
                {
                    if (!CombatProcessor.bMainCombat)
                    {
                        finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                    }
                }
            }

            foreach (var item in CombatProcessor.heroCharacters)
            {

                finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));

            }

            if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().currentEnemy != null && finalCheck.Find(t => t.mapPosition.Location == EncounterInfo.currentTurn().currentEnemy.character.position.ToPoint()) != null)
            {
                finalCheck.Remove(finalCheck.Find(t => t.mapPosition.Location == EncounterInfo.currentTurn().currentEnemy.character.position.ToPoint()));
            }



            temp.RemoveAll(t => finalCheck.Contains(t));

            temp.Distinct();

            enemy.UpdatePosition();
            List<BasicTile> tilesConnected = new List<BasicTile> { gameZone.Find(tile => tile.mapPosition.Contains(enemy.trueMapSize()) || tile.mapPosition.Intersects(enemy.trueMapSize())) };
            List<BasicTile> tilesChecked = new List<BasicTile>();
            List<BasicTile> tilesLeftToCheck = new List<BasicTile>(tilesConnected);

            while (tilesLeftToCheck.Count != 0)
            {
                var checkTile = tilesLeftToCheck[0];
                tilesLeftToCheck.Remove(checkTile);
                tilesChecked.Add(checkTile);
                var surroundingTiles = temp.Except(tilesConnected).ToList().FindAll(tile => tile.TileDistance(checkTile) == 1);
                tilesLeftToCheck.AddRange(surroundingTiles.Except(tilesChecked));
                tilesConnected.AddRange(surroundingTiles.Except(tilesConnected));
            }


            return tilesConnected;
        }


        internal List<BasicTile> possibleTilesGameZoneForEnemyINITIALIZATION(List<BasicTile> gameZone)
        {
            List<BasicTile> temp = new List<BasicTile>(gameZone);
            temp.RemoveAll(t => t == null);

            List<BasicTile> finalCheck = new List<BasicTile>();

            if (PlayerController.selectedSprite != null)
            {
                var tempPos = PlayerController.selectedSprite.positionToMapCoords();
                temp.RemoveAll(t => t.positionGrid == tempPos);
            }

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().currentEnemy != null && item != EncounterInfo.currentTurn().currentEnemy.character)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().currentEnemy == null)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else
                {
                    if (!CombatProcessor.bMainCombat)
                    {
                        finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                    }
                }
            }

            foreach (var item in CombatProcessor.heroCharacters)
            {

                finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));

            }

            if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().currentEnemy != null && finalCheck.Find(t => t.mapPosition.Location == EncounterInfo.currentTurn().currentEnemy.character.position.ToPoint()) != null)
            {
                finalCheck.Remove(finalCheck.Find(t => t.mapPosition.Location == EncounterInfo.currentTurn().currentEnemy.character.position.ToPoint()));
            }



            temp.RemoveAll(t => finalCheck.Contains(t));

            temp.Distinct();
            return temp;
        }

        public List<BasicTile> possibleTilesGameZone(Rectangle rArea)
        {
            List<BasicTile> temp = new List<BasicTile>();

            foreach (var chunk in chunksToConsider)
            {
                temp.AddRange(chunk.returnLayer(0).FindAll(t => t.hitboxes.Count == 0));
                foreach (var layer in chunk.layer.FindAll(v => v != 0 && v < 3))
                {
                    var tempList = chunk.returnLayer(layer).Where(t => t.hitboxes.Count != 0).ToList();
                    foreach (var tile in tempList)
                    {
                        temp.RemoveAll(t => t.positionGrid == tile.positionGrid);
                    }
                }
            }
            temp.RemoveAll(t => !rArea.Contains(t.mapPosition));


            var tempL_1 = GameProcessor.loadedMap.mapSprites.FindAll(x => x.bHasCollision && (rArea.Contains(x.spriteGameSize) || rArea.Intersects(x.spriteGameSize)));
            var tempL_2 = GameProcessor.loadedMap.mapObjectGroups.FindAll(x => x.Contains(rArea));

            List<BasicTile> finalCheck = new List<BasicTile>();
            foreach (var item in tempL_1)
            {
                finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
            }

            foreach (var item in tempL_2)
            {
                finalCheck.AddRange(temp.FindAll(t => item.Contains(t.mapPosition)));
            }

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().currentEnemy != null && item != EncounterInfo.currentTurn().currentEnemy.character)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().currentEnemy == null)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else
                {
                    if (!CombatProcessor.bMainCombat)
                    {
                        finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                    }
                }
            }

            foreach (var item in CombatProcessor.heroCharacters)
            {
                if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().selectedCharTurn != null && item != EncounterInfo.currentTurn().selectedCharTurn.character)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else if (EncounterInfo.encounterGroups.Count != 0 && !EncounterInfo.currentTurn().bIsPlayerTurnSet)
                {
                    finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));
                }
                else
                {
                    if (!CombatProcessor.bMainCombat)
                    {
                        finalCheck.AddRange(temp.FindAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize)));

                    }
                }

            }


            temp.RemoveAll(t => finalCheck.Contains(t));



            BaseSprite bs = PlayerController.selectedSprite;
            List<BasicTile> tilesConnected = new List<BasicTile> { temp.Find(tile => tile.mapPosition.Contains(bs.trueMapSize()) || tile.mapPosition.Intersects(bs.trueMapSize())) };
            List<BasicTile> tilesChecked = new List<BasicTile>();


            if (tilesConnected[0] == null)
            {
                tilesConnected.RemoveAt(0);
                Console.WriteLine("Soft error, placed on tileblocker, finding nearest valid position...");
                for (int i = 0; i < 20; i++)
                {
                    var tTile = temp.Find(tile => tile.distanceCoordsFrom(bs.position) == i);
                    if (tTile != null)
                    {
                        tilesConnected.Add(tTile);
                        PlayerController.selectedSprite.position = tTile.positionGrid * 64;
                        break;
                    }
                }
            }

            List<BasicTile> tilesLeftToCheck = new List<BasicTile>(tilesConnected);

            while (tilesLeftToCheck.Count != 0)
            {
                var checkTile = tilesLeftToCheck[0];
                tilesLeftToCheck.Remove(checkTile);
                tilesChecked.Add(checkTile);
                var surroundingTiles = temp.Except(tilesConnected).ToList().FindAll(tile => tile.TileDistance(checkTile) == 1);
                tilesLeftToCheck.AddRange(surroundingTiles.Except(tilesChecked));
                tilesConnected.AddRange(surroundingTiles.Except(tilesConnected));
            }

            return tilesConnected;
        }

        public List<BasicTile> tilesOnPosition(Vector2 coord)
        {
            List<BasicTile> temp = new List<BasicTile>();
            //List<KeyValuePair<Rectangle, List<BasicTile>>> consideredChunks = mapTilesLayer.FindAll(l => l.Key.Contains(coord*64));
            //foreach (var list in consideredChunks)
            //{
            //    var ti = list.Value.Find(t => t.positionGrid == coord);
            //    if (ti != default(BasicTile))
            //    {
            //        temp.Add(ti);
            //    }
            //}
            List<MapChunk> consideredChunks = new List<MapChunk>();
            consideredChunks.AddRange(Chunks.FindAll(c => c.region.Contains(coord * 64)));

            foreach (var chunk in consideredChunks)
            {
                foreach (var layer in chunk.lbt)
                {
                    var ti = layer.Find(t => t.positionGrid == coord);
                    if (ti != default(BasicTile))
                    {
                        temp.Add(ti);
                    }
                }
            }

            return temp;
        }

        public List<BasicTile> tilesOnPositionSpriteForGround(Rectangle r)
        {
            List<BasicTile> temp = new List<BasicTile>();

            Point testPos = new Point(r.Center.X, +r.Y + r.Height - r.Height / 8);
            List<MapChunk> consideredChunks = new List<MapChunk>();
            consideredChunks.AddRange(Chunks.FindAll(c => c.region.Contains(testPos)));

            foreach (var chunk in consideredChunks)
            {
                foreach (var layer in chunk.lbt)
                {
                    var ti = layer.FindAll(t => t.mapPosition.Contains(testPos));
                    if (ti.Count != 0)
                    {
                        temp.AddRange(ti);

                    }
                }
            }

            foreach (var item in temp)
            {
                item.Reload(GameProcessor.gcDB);
            }

            return temp;
        }

        public List<BasicTile> tilesOnPositionSpriteForWater(Rectangle r)
        {
            List<BasicTile> temp = new List<BasicTile>();
            Point testPos = new Point(r.Center.X, +r.Y + r.Height - r.Height / 8);
            List<MapChunk> consideredChunks = new List<MapChunk>();
            consideredChunks.AddRange(Chunks.FindAll(c => c.region.Contains(testPos)));


            foreach (var chunk in consideredChunks)
            {
                foreach (var layer in chunk.waterTiles)
                {
                    foreach (var tile in layer)
                    {
                        if (tile.bMustReload)
                        {
                            tile.reloadMapPosition();
                        }
                    }
                    var ti = layer.FindAll(t => t.mapPosition.Contains(testPos));
                    if (ti.Count != 0)
                    {
                        temp.AddRange(ti);

                    }
                }
            }

            foreach (var item in temp)
            {
                item.Reload(GameProcessor.gcDB);
            }

            return temp;
        }

        public List<BasicTile> tilesOnPosition(Vector2 coord, int l)
        {
            List<BasicTile> temp = new List<BasicTile>();



            foreach (var chunk in Chunks.FindAll(c => c.region.Contains(coord * 64)))
            {
                return chunk.returnLayer(l).FindAll(t => t.positionGrid == coord);
            }

            return temp;
        }

        public List<BasicTile> tilesOnPositionClick(Vector2 mouse, int l = 0)
        {
            List<BasicTile> temp = new List<BasicTile>();

            foreach (var chunk in Chunks.FindAll(c => c.region.Contains(mouse)))
            {
                return chunk.returnLayer(l).FindAll(t => t.mapPosition.Contains(mouse));
            }

            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord">grid position, accepts negative numbers (5,5) or (-3,26) ect...</param>
        /// <param name="tile"></param>
        /// <param name="l">layer</param>
        public void TryToChangeTile(Vector2 coord, BasicTile tile, int l)
        {
            tile.positionGrid = new Vector2(coord.X, coord.Y);
            tile.tileLayer = l;

            if (Chunks.Find(c => c.region.Contains(coord * 64)).Equals(default(MapChunk)))
            {
                CreateNewChunk(coord);
            }

            var chunkForTile = Chunks.Find(c => c.region.Contains(coord * 64));
            if (!chunkForTile.HasLayer(l))
            {
                chunkForTile.CreateLayer(l);
            }

            var tempTile = chunkForTile.returnLayer(l).Find(t => t.positionGrid == coord);
            if (tempTile != null && tempTile.tsID != tile.tsID)
            {
                tempTile = tile.Clone();
                tempTile.positionGrid = coord;
                tempTile = tile.Clone();
                tempTile.reloadMapPosition();
            }
            else
            {
                tempTile = tile.Clone();
                tempTile.positionGrid = coord;
                tempTile = tile.Clone();
                tempTile.reloadMapPosition();
                chunkForTile.Add(tempTile, l);
            }
        }

        public bool MustCreateNewChunk(Rectangle trueMapSize)
        {
            var test = Chunks.FindAll(c => c.region.Intersects(trueMapSize) || c.region.Contains(trueMapSize));
            if (test.Count == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord">True map location, as coordinates</param>
        internal void CreateNewChunk(Vector2 coord)
        {
            int xTile = 0;
            if (coord.X < 0)
            {
                xTile = (int)(coord.X / 25) - 1;
            }
            else
            {
                xTile = (int)(coord.X / 25);
            }

            int yTile = 0;
            if (coord.Y < 0)
            {
                yTile = (int)(coord.Y / 25) - 1;
            }
            else
            {
                yTile = (int)(coord.Y / 25);
            }

            Rectangle area = new Rectangle(xTile * 25 * 64, yTile * 25 * 64, 26 * 64, 26 * 64);
            Chunks.Add(new MapChunk(area, new List<BasicTile>(), 0, this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord">grid position, accepts negative numbers (5,5) or (-3,26) ect...</param>
        /// <param name="tile"></param>
        /// <param name="l">layer</param>
        public void TryToAddTile(Vector2 coord, BasicTile tile, int l)
        {
            tile.positionGrid = new Vector2(coord.X, coord.Y);
            tile.tileLayer = l;

            if (Chunks.Find(c => c.region.Contains(coord * 64)) == null)
            {
                CreateNewChunk(coord);
            }

            var chunkForTile = Chunks.Find(c => c.region.Contains(coord * 64));
            if (!chunkForTile.HasLayer(l))
            {
                chunkForTile.CreateLayer(l);
            }

            var tempTile = chunkForTile.returnLayer(l).Find(t => t.positionGrid == coord);
            if (tempTile != null && tempTile.tsID != tile.tsID)
            {
                tempTile = tile.Clone();
                tempTile.positionGrid = coord;
                tempTile = tile.Clone();
                tempTile.reloadMapPosition();
            }
            else
            {
                tempTile = tile.Clone();
                tempTile.positionGrid = coord;
                tempTile = tile.Clone();
                tempTile.reloadMapPosition();
                chunkForTile.Add(tempTile, l);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord">grid position, accepts negative numbers (5,5) or (-3,26) ect...</param>
        /// <param name="tile"></param>
        /// <param name="l">layer</param>
        public void TryToRemoveTile(Vector2 coord, int l)
        {

            if (Chunks.Find(c => c.region.Contains(coord * 64)) == null)
            {
                goto skip;
                CreateNewChunk(coord);
            }


            var chunkForTile = Chunks.Find(c => c.region.Contains(coord * 64));
            if (!chunkForTile.Equals(default(MapChunk)))
            {
                var tempTile = chunkForTile.returnLayer(l).Find(t => t.positionGrid == coord);
                if (tempTile != null)
                {
                    chunkForTile.lbt[chunkForTile.layer.IndexOf(l)].Remove(tempTile);
                }
            }
        skip: { }
        }

        public int totalAmountOfTiles()
        {
            int temp = 0;
            foreach (var chunk in Chunks)
            {
                foreach (var layer in chunk.lbt)
                {
                    temp += layer.Count;
                }
            }
            return temp;
        }

        public int totalAmountOfChunks()
        {
            return Chunks.Count;
        }

        internal void RemoveTempObjects(List<object> scriptTempObjects)
        {
            foreach (var item in scriptTempObjects)
            {
                if (item.GetType() == typeof(SpriteLight))
                {
                    mapLights.Remove(item as SpriteLight);
                }
            }
        }

        public List<object> allObjectsAtLocation(Vector2 mouse)
        {
            List<object> temp = new List<object>();
            temp.AddRange(mapSprites.FindAll(s => s.ContainsForEditorSelection(mouse)));
            temp.AddRange(mapObjectGroups.FindAll(mog => mog.Contains(mouse)));
            temp.AddRange(mapLights.FindAll(s => s.ContainsForEditorSelection(mouse)));
            temp.AddRange(mapNPCs.FindAll(npc => npc.ContainsForEditorSelection(mouse)));

            temp.AddRange(MapObjectHelpClass.objectFromOutsideOnThisMap.FindAll(o => o.GetType() == typeof(NPC)).Cast<NPC>().ToList().FindAll(npc => npc.ContainsForEditorSelection(mouse)));
            return temp;
        }

        public void massChangeScript(BaseSprite o, BaseScript s)
        {
            var tempList = mapSprites.FindAll(obj => obj.shapeID == o.shapeID && obj.scriptID == -1);
            foreach (var item in tempList)
            {
                item.scriptID = s.identifier;
            }

            foreach (var item in subMaps)
            {
                var tempList2 = item.mapSprites.FindAll(obj => obj.shapeID == o.shapeID && obj.scriptID == -1);
                foreach (var item2 in tempList2)
                {
                    item2.scriptID = s.identifier;
                }
            }
        }

        internal static List<BasicMap> allMapsGame()
        {
            if (GameProcessor.parentMap != null)
            {
                var list = new List<BasicMap> { GameProcessor.parentMap };
                list.AddRange(GameProcessor.parentMap.subMaps);
                return list;
            }
            return new List<BasicMap>();
        }

        static internal void HandleLoadGame(BaseCharacter mc)
        {
            var temp = MapChunk.consideredSprites.Find(cs => cs.obj == PlayerController.selectedSprite);
            MapChunk.consideredSprites.Remove(temp);
            mc.UpdatePosition();
            MapChunk.consideredSprites.Add(new objectInfo(mc, mc.getHeightIndicator(), objectInfo.type.Character, mc.trueMapSize(), false));
        }
    }

    public class objectInfo
    {
        internal enum type { Sprite, Character, ObjectGroup, Building }
        internal object obj;
        internal int heightIndicator;
        internal type objectType;
        internal Rectangle mapSize;
        internal bool bIsVisible;
        internal bool bHasCollision;
        internal bool bHasScript;
        internal bool bNeedsReflection = false;

        internal objectInfo(object obj, int heightIndicator, type objectType, Rectangle mapSize, bool bHasScript, bool bIsVisible = true)
        {
            this.obj = obj;
            this.heightIndicator = heightIndicator;
            this.objectType = objectType;
            this.mapSize = mapSize;
            this.bIsVisible = bIsVisible;
            bHasCollision = false;
            this.bHasScript = bHasScript;
            CheckCollision();

            if (obj is BaseSprite)
            {
                bNeedsReflection = (obj as BaseSprite).bNeedsWaterReflection;
            }
            else if (obj is ObjectGroup)
            {
                bNeedsReflection = (obj as ObjectGroup).bNeedsWaterReflection;
            }
            else if (obj.GetType() == typeof(NPC))
            {
                (obj as NPC).objInfo = this;
            }
        }

        internal void CheckCollision()
        {
            if (obj.GetType() == typeof(BaseSprite))
            {
                bHasCollision = (obj as BaseSprite).bHasCollision;
                if (bHasCollision && (obj as BaseSprite).closeProximityHitboxes().Count != 0)
                {
                    bHasCollision = true;
                }
            }
            else
    if (obj.GetType() == typeof(BaseCharacter))
            {
                bHasCollision = (obj as BaseSprite).bHasCollision;
                if (bHasCollision && (obj as BaseSprite).closeProximityHitboxes().Count != 0)
                {
                    bHasCollision = true;
                }
            }
            else
    if (obj.GetType() == typeof(ObjectGroup))
            {
                bHasCollision = (obj as ObjectGroup).bHasCollision;
                if (bHasCollision)
                {
                    var tempList = (obj as ObjectGroup).groupItems.FindAll(i => i.bHasCollision);
                    foreach (var item in tempList)
                    {
                        if (item.closeProximityHitboxes().Count != 0)
                        {
                            bHasCollision = true;
                            break;
                        }
                    }
                }

            }
            else
    if (obj.GetType() == typeof(SpriteLight))
            {
                bHasCollision = (obj as BaseSprite).bHasCollision;
                if (bHasCollision && (obj as BaseSprite).closeProximityHitboxes().Count != 0)
                {
                    bHasCollision = true;
                }
            }
            else
    if (obj.GetType() == typeof(Building))
            {
                bHasCollision = (obj as Building).bHasCollision;
            }
        }

        internal void Draw(SpriteBatch sb)
        {
            if (obj.GetType() == typeof(BaseSprite))
            {
                (obj as BaseSprite).Draw(sb);
            }
            else
            if (obj.GetType() == typeof(BaseCharacter))
            {
                if (GameProcessor.bStartCombatZoom)
                {

                }
                #region COMBAT PROCESSOR STUFF
                if (CombatProcessor.bIsRunning && !GameProcessor.bStartCombatZoom)
                {
                    if (BasicMap.bIsFromCombat(this) && !GameProcessor.bStartCombatZoom && CombatProcessor.bMainCombat)
                    {
                        SpriteBatch spriteBatch = sb;
                        var item = obj as BaseCharacter;

                        if (CombatProcessor.heroCharacters.Contains(obj))
                        {


                            if (item.bIsAlive && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsPlayerTurnSet)
                            {
                                if (EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == item) != null && EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == item).bIsCompleted)
                                {
                                    item.Draw(spriteBatch, Color.LightGray);
                                }
                                else
                                {
                                    item.Draw(spriteBatch);
                                }
                            }

                            if (item.bIsAlive && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && !EncounterInfo.currentTurn().bIsPlayerTurnSet)
                            {
                                item.Draw(spriteBatch);
                            }


                            BattleGUI.DrawSmallBars(spriteBatch, item, CombatProcessor.heroCharacters.IndexOf(item), BattleGUI.characterType.Hero);
                            if (item.bIsAlive && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsPlayerTurnSet)
                            {
                                if (EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == item) != null && EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == item).bIsCompleted)
                                {
                                    spriteBatch.DrawString(Game1.defaultFont, "E", item.position + new Vector2(45, 45), Color.White);
                                }
                            }
                        }
                        else if (CombatProcessor.encounterEnemies.Contains(obj))
                        {
                            var enemy = item;

                            if (enemy.bIsAlive && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsEnemyTurnSet)
                            {
                                if (EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == enemy) != null && EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == enemy).bIsCompleted)
                                {
                                    enemy.Draw(spriteBatch, Color.LightGray);
                                }
                                else
                                {
                                    enemy.Draw(spriteBatch);
                                }
                            }
                            else if (enemy.bIsAlive && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && !EncounterInfo.currentTurn().bIsEnemyTurnSet)
                            {
                                enemy.Draw(spriteBatch);
                            }

                            BattleGUI.DrawSmallBars(spriteBatch, enemy, CombatProcessor.encounterEnemies.IndexOf(enemy), BattleGUI.characterType.Enemy);

                            if (enemy.bIsAlive && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsEnemyTurnSet)
                            {
                                if (EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == enemy) != null && EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == enemy).bIsCompleted)
                                {
                                    spriteBatch.DrawString(Game1.defaultFont, "E", enemy.position + new Vector2(45, 45), Color.White);
                                }
                            }
                        }
                    }
                    else
                    {
                        (obj as BaseCharacter).Draw(sb);
                    }
                }
                else
                {
                    (obj as BaseCharacter).Draw(sb);
                }
                #endregion
            }
            else
            if (obj.GetType() == typeof(NPC) && MapChunk.regionsConsideredChunksContains(this))
            {
                (obj as NPC).Draw(sb);
            }
            else
            if (obj.GetType() == typeof(ObjectGroup))
            {
                (obj as ObjectGroup).Draw(sb);
                //foreach (var item in (obj as ObjectGroup).groupItems)
                //{
                //    //item.UpdateHitBoxes((obj as ObjectGroup));
                //    foreach (var box in item.closeProximityHitboxes())
                //    {
                //        sb.Draw(Game1.WhiteTex, box, Color.Red);
                //    }

                //}
            }
            else
            if (obj.GetType() == typeof(SpriteLight))
            {
                (obj as SpriteLight).Draw(sb);
            }
            else
            if (obj.GetType() == typeof(Building))
            {
                sb.Draw((obj as Building).buildingRender, (obj as Building).boundingZone, Color.White);
            }

        }

        public List<Rectangle> hitBoxes()
        {
            if (obj.GetType() == typeof(BaseSprite))
            {
                return (obj as BaseSprite).closeProximityHitboxes();
            }
            else if (obj.GetType() == typeof(BaseCharacter))
            {
                return (obj as BaseCharacter).closeProximityHitboxes();
            }
            else if (obj.GetType() == typeof(ObjectGroup))
            {
                var temp = new List<Rectangle>();
                (obj as ObjectGroup).groupItems.Where(gi => gi.bHasCollision).ToList().ForEach(gi => temp.AddRange(gi.closeProximityHitboxes()));
                return temp;
            }
            else if (obj.GetType() == typeof(SpriteLight))
            {
                return (obj as SpriteLight).closeProximityHitboxes();
            }
            else if (obj.GetType() == typeof(Building))
            {
                var temp = new List<Rectangle>();
                temp.Add((obj as Building).boundingZone);
                return temp;
            }

            return new List<Rectangle>();
        }

        public void CheckMapPosition()
        {
            if (obj.GetType() == typeof(BaseSprite))
            {
                (obj as BaseSprite).bRecalculateTrueMapSize = true;
                (obj as BaseSprite).trueMapSize();
            }
            else if (obj.GetType() == typeof(BaseCharacter))
            {
                (obj as BaseCharacter).bRecalculateTrueMapSize = true;
                (obj as BaseCharacter).trueMapSize();
            }
            else if (obj.GetType() == typeof(ObjectGroup))
            {
                (obj as ObjectGroup).groupItems.ForEach(gi => gi.bRecalculateTrueMapSize = true);
                (obj as ObjectGroup).groupItems.ForEach(gi => gi.trueMapSize());
            }
            else if (obj.GetType() == typeof(SpriteLight))
            {
                (obj as SpriteLight).bRecalculateTrueMapSize = true;
                (obj as SpriteLight).trueMapSize();
            }
            else if (obj.GetType() == typeof(Building))
            {
            }

        }

        public void Update(GameTime gt)
        {
            if (obj.GetType() == typeof(BaseSprite))
            {
                (obj as BaseSprite).Update(gt);
            }
            else if (obj.GetType() == typeof(BaseCharacter))
            {
                if (CombatProcessor.bIsRunning)
                {
                    var test = MapChunk.consideredSprites.FindAll(s => s.objectType == objectInfo.type.Character).Find(s => s.obj == GameProcessor.mainControllerBeforeCombat && PlayerController.selectedSprite != GameProcessor.mainControllerBeforeCombat);
                    if (this != test)
                    {
                        (obj as BaseCharacter).Update(gt);
                    }
                }
                else
                {
                    (obj as BaseCharacter).Update(gt);
                }


            }
            else if (obj.GetType() == typeof(NPC))
            {
                (obj as NPC).Update(gt);
            }
            else if (obj.GetType() == typeof(ObjectGroup))
            {
                (obj as ObjectGroup).Update(gt);
            }
            else if (obj.GetType() == typeof(SpriteLight))
            {
                (obj as SpriteLight).Update(gt);
            }
            else if (obj.GetType() == typeof(Building))
            {
                (obj as Building).Update(gt);
            }
        }

        internal void MinimalUpdate(GameTime gt)
        {
            if (obj.GetType() == typeof(BaseSprite))
            {
                (obj as BaseSprite).MinimalUpdate(gt);
            }
            else if (obj.GetType() == typeof(BaseCharacter))
            {
                (obj as BaseCharacter).MinimalUpdate(gt);
            }
            else if (obj.GetType() == typeof(NPC))
            {
                if ((obj as NPC).baseCharacter.smh.bIsBusy && (obj as NPC).baseCharacter.animationIndex == (int)BaseCharacter.CharacterAnimations.Movement)
                {
                    (obj as NPC).baseCharacter.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                }
                (obj as NPC).MinimalUpdate(gt);
            }
            else if (obj.GetType() == typeof(ObjectGroup))
            {
                (obj as ObjectGroup).MinimalUpdate(gt);
            }
            else if (obj.GetType() == typeof(SpriteLight))
            {
                (obj as SpriteLight).MinimalUpdate(gt);
            }
            else if (obj.GetType() == typeof(Building))
            {
                //(obj as Building).Update(gt);
            }
        }
    }

    [XmlRoot("Map Chunk")]
    public class MapChunk
    {
        [XmlArray("test int")]
        public List<List<BasicTile>> lbt = new List<List<BasicTile>>();
        [XmlArray("water tiles")]
        public List<List<BasicTile>> waterTiles = new List<List<BasicTile>>();
        [XmlElement("region")]
        public Rectangle region = new Rectangle();
        [XmlArray("layers")]
        public List<int> layer = new List<int>();
        [XmlElement("id")]
        public int id = 0;

        [XmlIgnore]
        public List<object> objectsInChunk = null;
        internal List<objectInfo> staticObjectInfoList = new List<objectInfo>();

        internal static List<objectInfo> consideredSprites = new List<objectInfo>();
        /// <summary>
        /// From, to
        /// </summary>
        internal static List<KeyValuePair<int, int>> changesToOrder = new List<KeyValuePair<int, int>>();

        public MapChunk()
        {
        }

        public MapChunk(Rectangle region, List<BasicTile> lbt, int layer, BasicMap m)
        {
            this.region = region;
            this.lbt = new List<List<BasicTile>>();
            this.lbt.Add(lbt); ;
            this.layer = new List<int>();
            this.layer.Add(layer);
            this.id = m.highestChunkID;
            m.highestChunkID++;
            objectsInChunk = new List<object>();
        }

        public static List<Rectangle> regionsConsideredChunks()
        {
            List<Rectangle> temp = new List<Rectangle>();
            GameProcessor.loadedMap.chunksToConsider.ForEach(c => temp.Add(c.region));
            return temp;
        }

        public void CreateLayer(int l)
        {
            layer.Add(l);
            lbt.Add(new List<BasicTile>());
        }

        public void Add(BasicTile bt, int l)
        {
            if (!HasLayer(l))
            {
                CreateLayer(l);
            }

            lbt[layer.IndexOf(l)].Add(bt);
        }

        public List<BasicTile> returnLayer(int l)
        {
            if (layer.Contains(l))
            {
                return lbt[layer.IndexOf(l)];
            }

            return new List<BasicTile>();
        }

        public bool HasLayer(int l)
        {
            return layer.Contains(l);
        }

        public void AddObjs(List<object> lo)
        {
            if (objectsInChunk == null)
            {
                objectsInChunk = lo;
            }
            else
            {
                objectsInChunk.AddRange(lo);
            }
        }

        public void AddObj(Object lo)
        {
            if (objectsInChunk == null)
            {
                objectsInChunk = new List<object>();
            }

            objectsInChunk.Add(lo);
        }

        public static void AddObj(BasicMap bm, Object obj, Rectangle mapsize)
        {
            foreach (var item in bm.Chunks)
            {
                if (item.region.Contains(mapsize) || item.region.Intersects(mapsize))
                {
                    item.AddObj(obj);
                }
            }
        }

        public static void finalizeChunkCheck(BasicMap bm)
        {

            if (!bm.previousChunksToConsider.SequenceEqual(bm.chunksToConsider))
            {
                consideredSprites = bm.returnAllObjectsInCameraAsGeneric();
                consideredSprites.ForEach(cs => cs.CheckMapPosition());
                bm.activeLights = consideredSprites.FindAll(cs => cs.obj.GetType() == typeof(SpriteLight)).Select(cs => cs.obj).Cast<SpriteLight>().ToList();
                if (CombatProcessor.bIsRunning)
                {
                    var list = consideredSprites.FindAll(cs => cs.objectType == objectInfo.type.Character);
                    var test = list.Find(cs => cs.obj == PlayerController.selectedSprite);
                    if (list.Contains(test))
                    {
                        consideredSprites.Remove(test);
                    }
                }
            }
            else
            {
                consideredSprites = consideredSprites.OrderBy(oi => oi.heightIndicator).ToList();
            }

        }

        public static void forceFinalizeChunkCheck(BasicMap bm)
        {
            MapObjectHelpClass.objectFromOutsideOnThisMap.Clear();
            MapObjectHelpClass.objectFromOutsideOnThisMap.AddRange(MapObjectHelpClass.objectsToUpdateOutsideOfMap.FindAll(o => o.GetType() == typeof(NPC) && (o as NPC).baseCharacter.currentMapToDisplayOn == bm));

            consideredSprites = bm.returnAllObjectsInCameraAsGeneric();
            consideredSprites.ForEach(cs => cs.CheckMapPosition());
            bm.activeLights = consideredSprites.FindAll(cs => cs.obj.GetType() == typeof(SpriteLight)).Select(cs => cs.obj).Cast<SpriteLight>().ToList();


        }

        public static void Update(BasicMap bm, GameTime gt)
        {
            MapObjectHelpClass.objectsUpdatedAlready.Clear();
            changesToOrder.Clear();
            foreach (var item in consideredSprites)
            {
                if (PlayerController.selectedSprite != null && !item.obj.Equals(PlayerController.selectedSprite))
                {
                    item.Update(gt);
                }
                else if (PlayerController.selectedSprite == null)
                {
                    item.Update(gt);
                }
                MapObjectHelpClass.objectsUpdatedAlready.Add(item.obj);
            }

            foreach (var item in changesToOrder)
            {
                var temp = consideredSprites[item.Key];
                consideredSprites.RemoveAt(item.Key);
                consideredSprites.Insert(item.Value, temp);
            }

            //consideredSprites.Clear();
            MapObjectHelpClass.Update(gt, bm);
            //var allObjectsInCamera = mapSprites.FindAll(o => o.spriteGameSize.Contains(trueCamera) || o.spriteGameSize.Intersects(trueCamera));
            //foreach (var sprite in allObjectsInCamera)
            //{
            //    sprite.Update(gameTime);
            //}

            //var allObjectGroupsInCamera = mapObjectGroups.FindAll(og => og.Contains(trueCamera));
            //foreach (var sprite in allObjectGroupsInCamera)
            //{
            //    sprite.Update(gameTime);
            //}


            //var allNPCsInCamera = mapNPCs.FindAll(npc => npc.baseCharacter.spriteGameSize.Contains(trueCamera) || npc.baseCharacter.spriteGameSize.Intersects(trueCamera));
            //foreach (var sprite in allNPCsInCamera)
            //{
            //    sprite.Update(gameTime);
            //}

            //activeLights.Clear();
            //var allLightsInCamera = mapLights.FindAll(ml => ml.Contains(trueCamera));
            //foreach (var sprite in allLightsInCamera)
            //{
            //    sprite.Update(gameTime);
            //    activeLights.Add(sprite);
            //}



        }

        public override string ToString()
        {
            return "Chunk: " + id + ", Region: " + region + ", Layers: " + layer.Count;
        }

        public static void AddObjectsEditor(BasicMap bm, Object obj)
        {
            Rectangle mapSize = new Rectangle();
            if (obj.GetType() == typeof(BaseSprite))
            {
                var temp = obj as BaseSprite;
                mapSize = temp.trueMapSize();
            }
            else if (obj.GetType() == typeof(BaseCharacter))
            {
                var temp = obj as BaseSprite;
                mapSize = temp.trueMapSize();
            }
            else if (obj.GetType() == typeof(SpriteLight))
            {
                var temp = obj as BaseSprite;
                mapSize = temp.trueMapSize();
            }
            else if (obj.GetType() == typeof(NPC))
            {
                var temp = (obj as NPC).baseCharacter;
                mapSize = temp.trueMapSize();
            }
            else if (obj.GetType() == typeof(ObjectGroup))
            {
                var temp = obj as ObjectGroup;
                mapSize = temp.trueMapSize;
            }
            if (bm.MustCreateNewChunk(mapSize))
            {
                bm.CreateNewChunk((mapSize.Location.ToVector2() / 64).ToPoint().ToVector2());
                Console.WriteLine("Created Chunk: " + bm.Chunks.Last().ToString());
            }

            bm.Chunks.FindAll(c => c.region.Intersects(mapSize) || c.region.Contains(mapSize)).ForEach(c => c.AddObj(obj));
            bm.ForceCheckChunksToConsider();
        }

        public static List<MapChunk> returnChunkRadius(BasicMap bm, Vector2 start, int radius)
        {
            if (!bm.ChunksPrepared)
            {
                try
                {
                    bm.PrepareChunksForLogic();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error getting chunks ready for: " + bm);
                    Console.WriteLine(e.Message);
                }
            }


            var temp = new List<MapChunk>();
            MapChunk startchunk = bm.Chunks.Find(c => c.region.Contains(start));
            Rectangle radiusRegion = new Rectangle(startchunk.region.Location - ((startchunk.region.Size.ToVector2() - new Vector2(96)) * radius).ToPoint(), ((startchunk.region.Size.ToVector2()) * (radius + 1)).ToPoint());
            temp = bm.Chunks.FindAll(c => radiusRegion.Contains(c.region) || radiusRegion.Intersects(c.region));
            return temp;
        }

        /// <summary>
        /// Check whether object is in one of the considered chunks, else no need to waste time drawing it (mostly used for NPC logic)
        /// </summary>
        /// <param name="objectInfo"></param>
        /// <returns></returns>
        internal static bool regionsConsideredChunksContains(objectInfo objectInfo)
        {
            foreach (var item in GameProcessor.loadedMap.chunksToConsider)
            {
                if (item.region.Contains(objectInfo.mapSize) || item.region.Intersects(objectInfo.mapSize))
                {
                    return true;
                }
            }
            return false;
        }

        internal static void MinimalUpdate(BasicMap basicMap, GameTime gt)
        {
            MapObjectHelpClass.objectsUpdatedAlready.Clear();

            foreach (var item in consideredSprites)
            {
                if (PlayerController.selectedSprite != null && !item.obj.Equals(PlayerController.selectedSprite))
                {
                    item.MinimalUpdate(gt);
                }
                else
                {

                }
                MapObjectHelpClass.objectsUpdatedAlready.Add(item.obj);
            }

            //consideredSprites.Clear();
            MapObjectHelpClass.MinimalUpdate(gt, basicMap);
        }

        internal void Checkhunk()
        {

            if (layer.Count != 0)
            {
                int max = layer.Max(i => i);
                if (layer.Last() != max)
                {
                    OrderChunkLayers();
                }
            }
        }

        private void OrderChunkLayers()
        {
            List<int> orderedLayer = new List<int>(layer);
            orderedLayer = orderedLayer.OrderBy(i => i).ToList();
            List<KeyValuePair<int, int>> layerAdjustMents = new List<KeyValuePair<int, int>>();

            for (int i = 0; i < orderedLayer.Count; i++)
            {
                if (orderedLayer[i] != layer[i])
                {
                    layerAdjustMents.Add(new KeyValuePair<int, int>(i, layer.IndexOf(orderedLayer[i])));
                }
            }

            List<List<BasicTile>> tileCopy = new List<List<BasicTile>>();
            for (int i = 0; i < lbt.Count; i++)
            {
                tileCopy.Add(new List<BasicTile>(lbt[i]));
            }

            for (int i = 0; i < layerAdjustMents.Count; i++)
            {
                lbt[layerAdjustMents[i].Key] = new List<BasicTile>(tileCopy[layerAdjustMents[i].Value]);
            }

            layer = new List<int>(orderedLayer);
            Console.WriteLine("Layers ordened, have a nice day!");
        }
    }

    static internal class MapObjectHelpClass
    {
        internal static List<Object> objectsToUpdateOutsideOfMap = new List<Object>();
        internal static List<Object> objectsUpdatedAlready = new List<Object>();
        internal static List<Object> objectFromOutsideOnThisMap = new List<Object>();

        internal static void Update(GameTime gt, BasicMap bm)
        {
            objectsToUpdateOutsideOfMap = objectsToUpdateOutsideOfMap.Distinct().ToList();
            foreach (var item in objectsToUpdateOutsideOfMap.Except(objectsUpdatedAlready))
            {
                if (item.GetType() == typeof(NPC))
                {
                    (item as NPC).MinimalOutsideUpdate(gt);
                }
            }

            foreach (var item in objectFromOutsideOnThisMap.Except(objectsUpdatedAlready))
            {
                if (item.GetType() == typeof(NPC))
                {
                    (item as NPC).Update(gt);
                }
            }
        }

        internal static void MinimalUpdate(GameTime gt, BasicMap bm)
        {
            objectsToUpdateOutsideOfMap = objectsToUpdateOutsideOfMap.Distinct().ToList();
            foreach (var item in objectsToUpdateOutsideOfMap.Except(objectsUpdatedAlready))
            {
                if (item.GetType() == typeof(NPC))
                {
                    //  (item as NPC).MinimalOutsideUpdate(gt);
                }
            }

            foreach (var item in objectFromOutsideOnThisMap.Except(objectsUpdatedAlready))
            {
                if (item.GetType() == typeof(NPC))
                {
                    (item as NPC).MinimalUpdate(gt);
                }
            }
        }
    }
}
