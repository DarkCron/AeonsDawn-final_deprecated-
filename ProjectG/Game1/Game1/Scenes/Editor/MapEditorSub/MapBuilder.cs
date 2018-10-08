using TBAGW.Utilities;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.Map;

using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;
using TBAGW.Utilities.SriptProcessing.ScriptTriggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using TBAGW.Utilities.Characters;
using TBAGW.Forms;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using TBAGW.Forms.ItemCreation;
using TBAGW.Scenes.Editor.MapEditorSub;
using TBAGW.Forms.GameObjects;

namespace TBAGW.Scenes.Editor
{

    class MapBuilder : Scene
    {
        #region FIELDS
        #region MapBuilder FIELDS
        static Matrix MapEditorMatrix;
        internal const int cameraSpeed = 5;
        static internal int cameraPosX = 0;
        static internal int cameraPosY = 0;
        static internal float cameraZoomX = 1;

        Matrix LowerBarMatrix;
        int lbCameraPosX = 0;

        Matrix SideBarMatrix;
        int sideCameraPosX = 0;
        int sideCameraPosY = 0;

        static public BasicMap loadedMap;
        static public BasicMap parentMap;

        bool bMouseInMapBuilderMainWindow = false;

        static public bool bShowTSWindow = false;

        List<String> fileList = new List<string>();

        Texture2D tileSheetSelectWindowBGTex;
        Texture2D lowerSelectBarBG;
        Texture2D lowerSelectBarAddButton;
        Texture2D lowerSelectBarSeparate;
        Texture2D smallerWindowBoxMinimize;

        List<ScreenButton> tileSheetButtons = new List<ScreenButton>();
        public List<Texture2D> usedTileSheetTextures = new List<Texture2D>();
        public List<String> usedTileSheetTexturesNames = new List<String>();
        public List<TileSheet> usedTileSheets = new List<TileSheet>();
        int selectedTileSheetIndex = 0;

        static RenderTarget2D sideBarRender;
        static RenderTarget2D mainWindowRender;
        //   RenderTarget2D miniMapRender;
        public int elapsedSeconds = 0; //To time minimap render
        public int minimapRenderTimer = 1000; //In Seconds, so mini map gets updated every 5 seconds

        bool bShowMiniMap = false;
        static public bool bAllowEditing = false;

        static public BasicTile paintTile = default(BasicTile);
        static public int brushSize = 1;
        static List<Vector2> coords = new List<Vector2>();
        Vector2 mousePosInMapCoord = new Vector2();
        static public bool bShowPaint = false;
        public String mapLocationDisk = "";
        static public bool bPlayTest = false;
        #endregion

        #region ScriptLayer Fields
        static bool bScriptBuilder = false;
        public static bool bIsRunning = true;
        static bool bObjectBuilder = false;

        static public bool bCreatingScript = false;
        bool bShowIdentifiers = false;
        int lastSelectedTileX = 0;
        int lastSelectedTileY = 0;
        BasicTrigger selectedTrigger = default(BasicTrigger);

        #endregion

        #region TileSheet editor FIELDS
        #endregion
        #endregion
        static public BaseSprite testSprite = default(BaseSprite);
        static public bool bObjectLayer = false;
        //static public BaseSprite selectedSprite = default(BaseSprite);
        //static public List<BaseSprite> selectedSprites = new List<BaseSprite>();
        static public EditorObjectSelection objSelection = new EditorObjectSelection();
        static public EditorObjectSelection objAddition = new EditorObjectSelection();
        static public int selectedSpriteIndex = 0;
        static internal int HighestLayer = 10;

        Texture2D gridTex;
        static public Vector2 AdjustedMouse;
        static public List<BaseClass> loadedMGClasses = new List<BaseClass>();
        static public List<BaseClass> loadedCUSTOMClasses = new List<BaseClass>();
        RenderTarget2D gifRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 64, 64);
        static public bool bDrawGUI = false;

        static public ObjectGroup pasteGroup = null;
        static public bool bRegionEditor = false;
        static public bool bZoneEditor = false;
        static public Vector2 drawPoint1 = new Vector2(-1);
        static public Vector2 drawPoint2 = new Vector2(-1);
        static Rectangle newZoneRegionSize = new Rectangle();
        static Rectangle porperZoneRegionSize = new Rectangle();
        static public MapRegion selectedRegion = new MapRegion();
        static public MapZone selectedZone = new MapZone();
        static internal Game mainG;
        static internal GameContentDataBase gcDB = new GameContentDataBase();
        static public bool ShowParticleSystem = false;
        static public ParticleSystemSource selectedParticleSource = null;
        static internal MapEditorControls controls = new MapEditorControls();
        static internal float testWorldBrightness = 0.75f;
        static internal bool testLight = false;
        static RenderTarget2D lightTempRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static internal ObjectInfoWindow oiw = new ObjectInfoWindow();
        static public List<BasicTile> specialBrush = new List<BasicTile>();
        static public List<EditorPreviewRender> eprList = new List<EditorPreviewRender>();
        static internal bool bOnlyDrawOneLayer = false;
        static internal memoryBrush smartBrush = new memoryBrush();
        static internal bool bShowInvisibleObjects = false;


        static UICollection uic = new UICollection();


        static public void ReloadActiveMap()
        {
            loadedMap = parentMap;
            ResetCamera();
            loadedMap.ForceCheckChunksToConsider();
            GameProcessor.loadedMap = loadedMap;
        }

        static public void ReloadActiveMap(BasicMap map)
        {
            loadedMap = map;
            ResetCamera();
            loadedMap.ForceCheckChunksToConsider();
            GameProcessor.loadedMap = map;
        }

        static public void StartRegionEditing(MapRegion r)
        {
            StandardMapBuilder();
            bRegionEditor = true;
            drawPoint1 = new Vector2(-1);
            drawPoint2 = new Vector2(-1);
            newZoneRegionSize = new Rectangle();
            selectedRegion = r;
        }

        internal static void StartZoneEditing(MapZone selectedItem)
        {
            StandardMapBuilder();
            bZoneEditor = true;
            drawPoint1 = new Vector2(-1);
            drawPoint2 = new Vector2(-1);
            newZoneRegionSize = new Rectangle();
            selectedZone = selectedItem;
        }

        static public void Center(Rectangle mapSize)
        {
            cameraPosX = mapSize.X - (int)(1366 / 2 / cameraZoomX) - mapSize.Width / 2;
            cameraPosY = mapSize.Y - (int)(768 / 2 / cameraZoomX) - mapSize.Height / 2;
            cameraPosY *= -1;
            //cameraZoomX = 1;
        }

        static public void StandardMapBuilder()
        {
            objSelection = new EditorObjectSelection();
            objAddition = new EditorObjectSelection();
            controls.currentType = MapEditorControls.controlType.None;
            bIsRunning = true;
            bObjectLayer = false;
            bScriptBuilder = false;
            bPlayTest = false;
            bRegionEditor = false;
            bZoneEditor = false;
            bDrawGUI = false;
            ShowParticleSystem = false;
            HitboxEditor.bIsRunning = false;
        }

        static public void ResetCamera()
        {
            cameraPosX = 0;
            cameraPosY = 0;
            cameraZoomX = 1;
        }

        static public void UpdateCameraPosition()
        {
            MapEditorMatrix = Matrix.CreateTranslation(-cameraPosX, cameraPosY, 1) * Matrix.CreateScale(cameraZoomX, cameraZoomX, 1);
        }

        static public void UpdateCameraPosition(Vector2 v)
        {
            MapEditorMatrix = Matrix.CreateTranslation(-v.X, v.Y, 1) * Matrix.CreateScale(cameraZoomX, cameraZoomX, 1);
            cameraPosX = (int)v.X;
            cameraPosY = (int)v.Y;
            loadedMap.ForceCheckChunksToConsider();
        }

        static internal Form1 functionForm;
        public void Start(Game game, BasicMap map)
        {
            controls.Assign(this);
            //  FileSelector.Start();
            mainG = game;
            functionForm = new Form1();
            functionForm.Show();


            ResetCamera();
            if (sideBarRender != null)
            {
                sideBarRender.Dispose();
                mainWindowRender.Dispose();
                //      miniMapRender.Dispose();
            }

            sideBarRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            mainWindowRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

            bAllowEditing = false;

            bShowMiniMap = false;
            loadedMap = map;
            parentMap = map;
            tileSheetSelectWindowBGTex = game.Content.Load<Texture2D>(@"Editor\MapEditor\SmallerWindowBox");
            lowerSelectBarBG = game.Content.Load<Texture2D>(@"Editor\MapEditor\LowerSelectBarBG");
            lowerSelectBarAddButton = game.Content.Load<Texture2D>(@"Editor\MapEditor\LowerSelectBarAddButton");
            lowerSelectBarSeparate = game.Content.Load<Texture2D>(@"Editor\MapEditor\LowerSelectBarSeparate");
            smallerWindowBoxMinimize = game.Content.Load<Texture2D>(@"Editor\MapEditor\SmallerWindowBoxMinimize");



            brushSize = 1;
            CalculateBrushSize();

            String completeContentDir = Game1.rootTBAGW + @"Maps\TileSheets\TestTileSet.cgtsc";
            //tring completeContentDir = Game1.root + @"\Maps\GameTS\TileSheets\TestTileSet.cgtsc";
            /* TileSheet tileSheetTest = EditorFileWriter.TileSheetReader(completeContentDir);
             MapBuilderTileSelector.AssignTiles(tileSheetTest);

             bool alreadyHasTileSheet = false;
             foreach (var item in loadedMap.usedTileSheets)
             {
                 if (item.tileSheetLocation.Equals(tileSheetTest.tileSheetLocation))
                 {
                     alreadyHasTileSheet = true;
                 }
             }
             if (!alreadyHasTileSheet)
             {
                 loadedMap.usedTileSheetLocations.Add(tileSheetTest.tileSheetLocation);
                 loadedMap.usedTileSheets.Add(tileSheetTest);
             }*/

            bScriptBuilder = false;
            bIsRunning = true;
            bObjectBuilder = false;

            bCreatingScript = false;

            gridTex = Game1.contentManager.Load<Texture2D>(@"Editor\MapEditor\Grid");

            InitializeForm();
        }

        internal static void TurnLightsOn()
        {
        }

        public static void ReloadMap()
        {
            loadedMap = EditorFileWriter.MapReader(loadedMap.mapLocation);
        }

        private void InitializeForm()
        {
            functionForm.mapName = loadedMap.mapName;
            functionForm.mapX = "na";
            functionForm.mapY = "na";
            functionForm.getEditorWayPointsReady(loadedMap);
        }

        internal static void HandleBuildings()
        {
            loadedMap.testBuildings = BuildingGenerator.Generate(loadedMap);
        }

        //public PlayerInventory pi = new PlayerInventory();
        public override void Update(GameTime gameTime, Game1 game)
        {
            if (Game1.bIsDebug && TestEnvironment.bDoTest && false)
            {
                TestEnvironment.Test();
            }

            if (TalentGridEditor.bIsRunning)
            {
                TalentGridEditor.Update(gameTime);
                return;
            }

            // uic = TestEnvironment.uic;
            //uic = gcDB.gameUICollections[0].parent;
            if (false && (uic.elementsInCollection == null || uic.elementsInCollection.Count == 0))
            {
                var tempUIScreen = new UIScreen();
                tempUIScreen.size = new Point(500, 300);
                tempUIScreen.initialSize = new Point(500, 300);
                uic.AddElement(tempUIScreen);
                var tempButton = new GButton();
                tempButton.size = new Point(64, 64);
                tempButton.position = new Point(425, 225);
                uic.AddElement(tempButton);
                TestEnvironment.uic = uic;
                uic.startMainElement.position = new Point(50, 50);
            }

            //if (false)
            //{
            //    foreach (var item in BasicMap.allMapsGame())
            //    {
            //        foreach (var chunk in item.Chunks)
            //        {
            //            List<BasicTile> tiles = new List<BasicTile>();
            //            foreach (var layer in chunk.lbt)
            //            {
            //                tiles.AddRange(layer.FindAll(tile => !chunk.region.Contains(tile.mapPosition) && !chunk.region.Intersects(tile.mapPosition)));
            //                Console.WriteLine("removed " + tiles.Count + " tiles");

            //            }
            //            chunk.lbt.ForEach(layer => layer.RemoveAll(tile => tiles.Contains(tile)));
            //        }
            //    }

            //}

            if (Keyboard.GetState().IsKeyDown(Keys.J) && !KeyboardMouseUtility.AnyButtonsPressed() && Game1.bIsDebug)
            {
                TestEnvironment.uic.startMainElement.position = (Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale).ToPoint();
                //ClassExp ce = new ClassExp();
                //ce.requiredExp.Add(120);
                //ce.requiredExp.Add(160);
                //ce.requiredExp.Add(250);
                //ce.requiredExp.Add(400);
                //ce.totalExp = 700;
                //ce.CalculateResetExpAndLevels();
                // loadedMap.ConvertMapSprites();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.V) && !KeyboardMouseUtility.AnyButtonsPressed() && Game1.bIsDebug)
            {
                //int tt = loadedMap.totalAmountOfTiles();
                //Console.WriteLine(tt + " tiles over " + loadedMap.totalAmountOfChunks() + " chunks");
             
            }

            //if (Keyboard.GetState().IsKeyDown(Keys.B) && !KeyboardMouseUtility.AnyButtonsPressed())
            //{
            //    if (!HitboxEditor.bIsRunning)
            //    {
            //        HitboxEditor.Start(gcDB.gameCharacters.Find(gc => gc.shapeID == 3));
            //    }
            //    else
            //    {
            //        HitboxEditor.bIsRunning = false;
            //    }

            //}
            if (!HitboxEditor.bIsRunning && !ShowParticleSystem)
            {
                if (!bPlayTest)
                {
                    Vector2 EditorCursorPos = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale / cameraZoomX + new Vector2(cameraPosX, -cameraPosY);
                    TestEnvironment.Update(gameTime, EditorCursorPos.ToPoint());

                    controls.Update(gameTime, EditorCursorPos);
                    //loadedMap.mapLights[0].position = EditorCursorPos;
                    //loadedMap.mapLights[0].UpdatePosition();

                    switch (controls.currentType)
                    {
                        case MapEditorControls.controlType.None:
                            break;
                        case MapEditorControls.controlType.TileLayer:
                            break;
                        case MapEditorControls.controlType.ObjectLayerEditing:
                            break;
                        case MapEditorControls.controlType.ObjectLayerAdding:
                            break;
                        case MapEditorControls.controlType.HitboxEditor:
                            break;
                        case MapEditorControls.controlType.TileSelect:
                            if (!EditorHelpClass.previousLoadedMapChunks.SequenceEqual(loadedMap.chunksToConsider))
                            {
                                foreach (var chunk in EditorHelpClass.previousLoadedMapChunks.FindAll(c => !loadedMap.chunksToConsider.Contains(c)))
                                {
                                    EditorHelpClass.openMoveTiles.RemoveAll(tile => chunk.region.Contains(tile.mapPosition) || chunk.region.Intersects(tile.mapPosition));
                                }

                                foreach (var chunk in loadedMap.chunksToConsider.FindAll(c => !EditorHelpClass.previousLoadedMapChunks.Contains(c)))
                                {
                                    EditorHelpClass.openMoveTiles.AddRange(loadedMap.possibleTilesWithController(chunk.region, loadedMap));
                                }
                                EditorHelpClass.openMoveTiles = EditorHelpClass.openMoveTiles.Distinct().ToList();
                            }
                            EditorHelpClass.previousLoadedMapChunks = loadedMap.chunksToConsider;

                            break;
                        default:
                            break;
                    }

                    if (objAddition != null && objAddition.IsCorrect())
                    {
                        objAddition.HandleMoveToMousePos(EditorCursorPos);
                    }

                    foreach (var item in eprList)
                    {
                        if (item.bRemove)
                        {
                            item.Remove();
                        }
                        else
                        {
                            item.Update(gameTime);
                        }
                    }
                    eprList.RemoveAll(epr => epr.bRemove);

                    if (eprList.Count != 0 && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Vector2 EditorCursorPosTemp = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale / cameraZoomX + new Vector2(cameraPosX, -cameraPosY);
                        var tempList = eprList.FindAll(epr => epr.Contains(EditorCursorPosTemp.ToPoint()));
                        tempList = tempList.OrderBy(epr => epr.renderPosition.Y).ToList();
                        //  var temp = eprList.Last(epr => epr.Contains(Mouse.GetState().Position));
                        if (tempList.Count != 0)
                        {
                            var temp = tempList.Last();
                            temp.renderPosition = (EditorCursorPosTemp.ToPoint() - temp.location.Center).ToVector2();
                        }

                    }

                    if (eprList.Count != 0 && Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        Vector2 EditorCursorPosTemp = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale / cameraZoomX + new Vector2(cameraPosX, -cameraPosY);
                        var tempList = eprList.FindAll(epr => epr.Contains(EditorCursorPosTemp.ToPoint()));
                        tempList = tempList.OrderBy(epr => epr.renderPosition.Y).ToList();
                        //  var temp = eprList.Last(epr => epr.Contains(Mouse.GetState().Position));
                        if (tempList.Count != 0)
                        {
                            var temp = tempList.Last();
                            temp.Remove();
                            eprList.Remove(temp);
                        }

                    }

                    #region MapBuilderStuff

                    AdjustedMouse = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;
                    //     EditorCursorPos /= ResolutionUtility.stdScale;

                    int mapTilePosX = (int)(EditorCursorPos.X / 64);
                    //if (EditorCursorPos.X < 0)
                    //{
                    //    mapTilePosX = 0;
                    //}
                    int mapTilePosY = (int)(EditorCursorPos.Y / 64);
                    //if (EditorCursorPos.Y < 0)
                    //{
                    //    mapTilePosY = 0;
                    //}
                    mousePosInMapCoord = new Vector2(mapTilePosX, mapTilePosY);

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && pasteGroup != null)
                    {
                        pasteGroup.Reload(gcDB);
                        pasteGroup.PlaceGroup(EditorCursorPos);
                        loadedMap.mapObjectGroups.Add(pasteGroup);
                        pasteGroup = null;
                        functionForm.objGForm.reloadLBs();
                    }



                    if (bIsRunning && !bObjectLayer)
                    {
                        //controls.currentType = MapEditorControls.controlType.None;
                        #region mapBuilderStuff
                        Vector2 camera = new Vector2(0 + cameraPosX, 0 + cameraPosY);

                        loadedMap.Update(gameTime, camera);

                        #endregion
                    }
                    else if (bScriptBuilder && !bCreatingScript)
                    {
                        #region Script Builder Stuff
                        //#region script controls
                        //if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.S) && !KeyboardMouseUtility.AnyButtonsPressed())
                        //{
                        //    bIsRunning = true;
                        //    bScriptBuilder = false;
                        //}

                        //if (Keyboard.GetState().IsKeyDown(Keys.S) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        //{
                        //    cameraPosY -= (int)(cameraSpeed / cameraZoomX);
                        //}

                        //if (Keyboard.GetState().IsKeyDown(Keys.W) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        //{
                        //    cameraPosY += (int)(cameraSpeed / cameraZoomX);
                        //}

                        //if (Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        //{
                        //    cameraPosX -= (int)(cameraSpeed / cameraZoomX);
                        //}

                        //if (Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        //{
                        //    cameraPosX += (int)(cameraSpeed / cameraZoomX);
                        //}

                        //if (Keyboard.GetState().IsKeyDown(Keys.Z) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && cameraZoomX > .2f)
                        //{
                        //    cameraZoomX -= 0.01f;
                        //}

                        //if (Keyboard.GetState().IsKeyDown(Keys.C) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        //{
                        //    cameraZoomX += 0.01f;
                        //}

                        //if (Keyboard.GetState().IsKeyDown(Keys.X) && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        //{
                        //    cameraZoomX = 1f;
                        //}

                        //if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !KeyboardMouseUtility.AnyButtonsPressed())
                        //{
                        //    EditorFileWriter.MapWriter(loadedMap);
                        //}

                        //if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !KeyboardMouseUtility.AnyButtonsPressed())
                        //{


                        //}
                        //#endregion
                        //#region Select Trigger
                        //foreach (var trigger in loadedMap.mapTriggers)
                        //{
                        //    foreach (var triggerLoc in trigger.storedTriggerLocations)
                        //    {
                        //        if (triggerLoc.Contains(EditorCursorPos))
                        //        {
                        //            if (Mouse.GetState().LeftButton == ButtonState.Pressed && Game1.bIsActive)
                        //            {
                        //                selectedTrigger = trigger;
                        //            }
                        //        }
                        //    }
                        //}
                        //#endregion
                        //#region Deselect Trigger SHIFT + X
                        //if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Keyboard.GetState().IsKeyDown(Keys.X) && !KeyboardMouseUtility.AnyButtonsPressed())
                        //{
                        //    selectedTrigger = default(BasicTrigger);

                        //}
                        //#endregion
                        //#region Delete trigger with RMB at mousepos
                        //int j = -1;
                        //foreach (var trigger in loadedMap.mapTriggers)
                        //{
                        //    int i = -1;
                        //    foreach (var triggerLoc in trigger.storedTriggerLocations)
                        //    {
                        //        if (triggerLoc.Contains(EditorCursorPos))
                        //        {
                        //            if (Mouse.GetState().RightButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                        //            {
                        //                i = trigger.storedTriggerLocations.IndexOf(triggerLoc);
                        //                break;
                        //            }

                        //            if (Mouse.GetState().RightButton == ButtonState.Pressed && Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        //            {
                        //                i = trigger.storedTriggerLocations.IndexOf(triggerLoc);
                        //                break;
                        //            }
                        //        }
                        //    }
                        //    if (i != -1)
                        //    {
                        //        trigger.storedTriggerLocations.RemoveAt(i);
                        //    }

                        //    if (trigger.storedTriggerLocations.Count == 0 && loadedMap.mapTriggers.Count != 0)
                        //    {
                        //        j = loadedMap.mapTriggers.IndexOf(trigger);
                        //        break;
                        //    }
                        //}
                        //if (j != -1)
                        //{
                        //    if (loadedMap.mapTriggers[j] == selectedTrigger)
                        //    {
                        //        selectedTrigger = default(BasicTrigger);
                        //    }
                        //    loadedMap.mapTriggers.RemoveAt(j);
                        //}
                        //#endregion
                        //#region Draw extra Triggers based on the selected one
                        //if (selectedTrigger != default(BasicTrigger))
                        //{
                        //    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Mouse.GetState().LeftButton == ButtonState.Pressed && Game1.bIsActive)
                        //    {
                        //        int mouseTileX = (int)(EditorCursorPos.X / 64);
                        //        int mouseTileY = (int)(EditorCursorPos.Y / 64);
                        //        bool bHasTriggerOnThatTile = false;
                        //        int triggerIndex = -1;
                        //        foreach (var trigger in loadedMap.mapTriggers)
                        //        {
                        //            if (trigger == selectedTrigger)
                        //            {
                        //                triggerIndex = loadedMap.mapTriggers.IndexOf(trigger);
                        //                foreach (var triggerLoc in trigger.storedTriggerLocations)
                        //                {
                        //                    if (triggerLoc.X == mouseTileX * 64 && triggerLoc.Y == mouseTileY * 64)
                        //                    {
                        //                        bHasTriggerOnThatTile = true;
                        //                        break;
                        //                    }
                        //                }
                        //            }

                        //        }

                        //        if (!bHasTriggerOnThatTile && triggerIndex != -1)
                        //        {
                        //            loadedMap.mapTriggers[triggerIndex].storedTriggerLocations.Add(new Rectangle(mouseTileX * loadedMap.mapTriggers[loadedMap.mapTriggers.Count - 1].storedTriggerWidth, mouseTileY * loadedMap.mapTriggers[loadedMap.mapTriggers.Count - 1].storedTriggerHeight, loadedMap.mapTriggers[loadedMap.mapTriggers.Count - 1].storedTriggerWidth, loadedMap.mapTriggers[loadedMap.mapTriggers.Count - 1].storedTriggerHeight));
                        //        }
                        //    }
                        //}
                        //#endregion
                        #endregion
                    }

                    #region OBJECTLAYER LOGIC
                    if (bObjectLayer)
                    {
                        // controls.currentType = MapEditorControls.controlType.ObjectLayerEditing;
                        bShowPaint = false;

                        Vector2 camera = new Vector2(0 + cameraPosX, 0 + cameraPosY);
                        loadedMap.Update(gameTime, camera);
                    }
                    #endregion

                    if (FrameSelector.bIsRunning)
                    {
                        FrameSelector.Update();
                    }

                    if (SpriteSelector.bIsRunning)
                    {
                        SpriteSelector.Update();
                    }
                    #region  On screen region drawing
                    if (bRegionEditor)
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                        {


                            if (drawPoint1 != new Vector2(-1))
                            {
                                drawPoint2 = EditorCursorPos;

                                newZoneRegionSize.X = ((int)drawPoint1.X / 64) * 64;
                                newZoneRegionSize.Y = ((int)drawPoint1.Y / 64) * 64;

                                int width = ((int)((EditorCursorPos.X - newZoneRegionSize.X) / 64) * 64);
                                int height = ((int)((EditorCursorPos.Y - newZoneRegionSize.Y) / 64) * 64);

                                if (width > 0)
                                {
                                    newZoneRegionSize.Width = width;

                                }
                                else
                                {
                                    newZoneRegionSize.X = newZoneRegionSize.X + width;
                                    newZoneRegionSize.Width = -width;
                                }

                                if (height > 0)
                                {
                                    newZoneRegionSize.Height = height;
                                }
                                else
                                {
                                    newZoneRegionSize.Y = newZoneRegionSize.Y + height;
                                    newZoneRegionSize.Height = -height;
                                }
                            }

                            if (drawPoint1 == new Vector2(-1))
                            {
                                drawPoint1 = EditorCursorPos;
                                newZoneRegionSize.X = ((int)drawPoint1.X / 64) * 64;
                                newZoneRegionSize.Y = ((int)drawPoint1.Y / 64) * 64;
                                newZoneRegionSize.Width = 64;
                                newZoneRegionSize.Height = 64;
                            }

                            if (drawPoint1 != new Vector2(-1) && drawPoint2 != new Vector2(-1))
                            {
                                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Do you want to save the current selected boundary to the selected region or zone? Selected:" + selectedRegion.ToString(), "Save selection?", System.Windows.Forms.MessageBoxButtons.YesNo);
                                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                                {
                                    selectedRegion.regionSizes.Add(newZoneRegionSize);
                                    StartRegionEditing(selectedRegion);
                                }
                                else if (dialogResult == System.Windows.Forms.DialogResult.No)
                                {
                                    StartRegionEditing(selectedRegion);
                                }
                            }
                        }

                        if (Mouse.GetState().RightButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                        {

                            if (drawPoint1 != new Vector2(-1))
                            {
                                drawPoint1 = new Vector2(-1);
                                newZoneRegionSize = new Rectangle();
                            }

                            if (drawPoint1 == new Vector2(-1))
                            {

                                var temp = selectedRegion.regionSizes.FindAll(boundary => boundary.Contains(EditorCursorPos));
                                if (temp != null && temp.Count != 0)
                                {
                                    System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete all boundaries at the location you right clicked?", "Warning", System.Windows.Forms.MessageBoxButtons.YesNo);
                                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        foreach (var item in temp)
                                        {
                                            selectedRegion.regionSizes.Remove(item);
                                        }
                                    }
                                }
                            }
                        }

                        if (drawPoint2 == new Vector2(-1))
                        {
                            newZoneRegionSize.X = ((int)drawPoint1.X / 64) * 64;
                            newZoneRegionSize.Y = ((int)drawPoint1.Y / 64) * 64;

                            int width = ((int)((EditorCursorPos.X - newZoneRegionSize.X) / 64) * 64);
                            int height = ((int)((EditorCursorPos.Y - newZoneRegionSize.Y) / 64) * 64);

                            if (width > 0)
                            {
                                newZoneRegionSize.Width = width;

                            }
                            else
                            {
                                newZoneRegionSize.X = newZoneRegionSize.X + width;
                                newZoneRegionSize.Width = -width;
                            }

                            if (height > 0)
                            {
                                newZoneRegionSize.Height = height;
                            }
                            else
                            {
                                newZoneRegionSize.Y = newZoneRegionSize.Y + height;
                                newZoneRegionSize.Height = -height;
                            }
                        }
                    }
                    #endregion

                    #region  On screen zone drawing
                    if (bZoneEditor)
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                        {


                            if (drawPoint1 != new Vector2(-1))
                            {
                                drawPoint2 = EditorCursorPos;

                                newZoneRegionSize.X = ((int)drawPoint1.X / 64) * 64;
                                newZoneRegionSize.Y = ((int)drawPoint1.Y / 64) * 64;

                                int width = ((int)((EditorCursorPos.X - newZoneRegionSize.X) / 64) * 64);
                                int height = ((int)((EditorCursorPos.Y - newZoneRegionSize.Y) / 64) * 64);

                                if (width > 0)
                                {
                                    newZoneRegionSize.Width = width;

                                }
                                else
                                {
                                    newZoneRegionSize.X = newZoneRegionSize.X + width;
                                    newZoneRegionSize.Width = -width;
                                }

                                if (height > 0)
                                {
                                    newZoneRegionSize.Height = height;
                                }
                                else
                                {
                                    newZoneRegionSize.Y = newZoneRegionSize.Y + height;
                                    newZoneRegionSize.Height = -height;
                                }
                            }

                            if (drawPoint1 == new Vector2(-1))
                            {
                                drawPoint1 = EditorCursorPos;
                                newZoneRegionSize.X = ((int)drawPoint1.X / 64) * 64;
                                newZoneRegionSize.Y = ((int)drawPoint1.Y / 64) * 64;
                                newZoneRegionSize.Width = 64;
                                newZoneRegionSize.Height = 64;
                            }

                            if (drawPoint1 != new Vector2(-1) && drawPoint2 != new Vector2(-1))
                            {
                                System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Do you want to save the current selected boundary to the selected region or zone? Selected:" + selectedZone.ToString(), "Save selection?", System.Windows.Forms.MessageBoxButtons.YesNo);
                                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                                {
                                    selectedZone.zoneSizes.Add(newZoneRegionSize);
                                    StartZoneEditing(selectedZone);
                                }
                                else if (dialogResult == System.Windows.Forms.DialogResult.No)
                                {
                                    StartZoneEditing(selectedZone);
                                }
                            }
                        }

                        if (Mouse.GetState().RightButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed() && selectedZone != null)
                        {

                            if (drawPoint1 != new Vector2(-1))
                            {
                                drawPoint1 = new Vector2(-1);
                                newZoneRegionSize = new Rectangle();
                            }

                            if (drawPoint1 == new Vector2(-1))
                            {

                                var temp = selectedZone.zoneSizes.FindAll(boundary => boundary.Contains(EditorCursorPos));
                                //  var temp = new List<Rectangle>();
                                if (temp != null && temp.Count != 0)
                                {
                                    System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("Are you sure you want to delete all boundaries at the location you right clicked?", "Warning", System.Windows.Forms.MessageBoxButtons.YesNo);
                                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        foreach (var item in temp)
                                        {
                                            selectedZone.zoneSizes.Remove(item);
                                        }
                                    }
                                }
                            }
                        }

                        if (drawPoint2 == new Vector2(-1))
                        {
                            newZoneRegionSize.X = ((int)drawPoint1.X / 64) * 64;
                            newZoneRegionSize.Y = ((int)drawPoint1.Y / 64) * 64;

                            int width = ((int)((EditorCursorPos.X - newZoneRegionSize.X) / 64) * 64);
                            int height = ((int)((EditorCursorPos.Y - newZoneRegionSize.Y) / 64) * 64);

                            if (width > 0)
                            {
                                newZoneRegionSize.Width = width;

                            }
                            else
                            {
                                newZoneRegionSize.X = newZoneRegionSize.X + width;
                                newZoneRegionSize.Width = -width;
                            }

                            if (height > 0)
                            {
                                newZoneRegionSize.Height = height;
                            }
                            else
                            {
                                newZoneRegionSize.Y = newZoneRegionSize.Y + height;
                                newZoneRegionSize.Height = -height;
                            }
                        }
                    }
                    #endregion

                    if (testSprite != default(BaseSprite))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Up) && !KeyboardMouseUtility.AnyButtonsPressed())
                        {
                            if (testSprite.rotationIndex < 3)
                            {
                                testSprite.rotationIndex++;
                            }
                            else
                            {
                                testSprite.rotationIndex = 0;
                            }
                        }
                        testSprite.Update(gameTime);
                        if (testSprite.spriteGameSize.Contains(AdjustedMouse))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                            {
                                Console.Out.WriteLine("You clicked me you son of a");
                            }
                        }
                    }

                    if (functionForm.tabControl4.SelectedTab != functionForm.tabPage11)
                    {
                        selectedRegion = new MapRegion();
                        selectedZone = new MapZone();
                    }


                    UpdateForm();

                    MapEditorMatrix = Matrix.CreateTranslation(-cameraPosX, cameraPosY, 1) * Matrix.CreateScale(cameraZoomX, cameraZoomX, 1);
                    // LowerBarMatrix = Matrix.CreateTranslation(-lbCameraPosX, 1, 1);
                    // SideBarMatrix = Matrix.CreateTranslation(-sideCameraPosX, -sideCameraPosY, 1);
                    #endregion
                }
                else if (bPlayTest)
                {
                    #region PlayTestStuff
                    GameProcessor.Update(gameTime);
                    #endregion
                }
            }

            if (HitboxEditor.bIsRunning)
            {
                MapBuilder.controls.Update(gameTime, Vector2.Zero);
                HitboxEditor.Update();
            }
            if (ShowParticleSystem)
            {
                if (selectedParticleSource != null)
                {
                    selectedParticleSource.Update(gameTime);
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {

                        selectedParticleSource.spawnPosition = Mouse.GetState().Position;

                    }
                }
            }
        }

        static public void ActivateScriptLayer()
        {
            bIsRunning = false;
            bScriptBuilder = true;
        }

        static public void SaveMap()
        {
            gcDB.gameCCCs.ForEach(CCC => CCC.PrepareTalentTreeMapSave());
            parentMap.ConvertMapSprites();
            gcDB.gameUICollections.ForEach(uic => uic.GenerateSave());
            EditorFileWriter.MapWriter(parentMap);
        }

        private void UpdateForm()
        {
            functionForm.UpdateFormContent();
            if (functionForm.bTSOpen != bShowTSWindow)
            {
                bShowTSWindow = functionForm.bTSOpen;
            }

        }

        internal void PaintLogic(Vector2 editorMousePos)
        {
            bool paint = false;
            int mapTilePosX = (int)(editorMousePos.X / 64);
            if (editorMousePos.X <= 0)
            {
                mapTilePosX += -1;
            }
            int mapTilePosY = (int)(editorMousePos.Y / 64);
            if (editorMousePos.Y <= 0)
            {
                mapTilePosY += -1;
            }
            mousePosInMapCoord = new Vector2(mapTilePosX, mapTilePosY);

            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                KeyboardMouseUtility.bPressed = true;
                switch (controls.tileType)
                {
                    case MapEditorControls.tileLayerType.normal:
                        switch (paintTile.spriteEffect)
                        {
                            case SpriteEffects.None:
                                paintTile.spriteEffect = SpriteEffects.FlipHorizontally;
                                break;
                            case SpriteEffects.FlipHorizontally:
                                paintTile.spriteEffect = SpriteEffects.None;
                                break;
                            case SpriteEffects.FlipVertically:
                                paintTile.spriteEffect = SpriteEffects.FlipHorizontally;
                                break;
                            default:
                                break;
                        }
                        break;
                    case MapEditorControls.tileLayerType.smartBrush:
                        if (null != smartBrush.getTiles())
                        {
                            foreach (var item in smartBrush.getTiles())
                            {
                                switch (item.bt.spriteEffect)
                                {
                                    case SpriteEffects.None:
                                        item.bt.spriteEffect = SpriteEffects.FlipHorizontally;
                                        break;
                                    case SpriteEffects.FlipHorizontally:
                                        item.bt.spriteEffect = SpriteEffects.None;
                                        break;
                                    case SpriteEffects.FlipVertically:
                                        item.bt.spriteEffect = SpriteEffects.FlipHorizontally;
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }
                        break;
                    default:
                        break;
                }

            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                KeyboardMouseUtility.bPressed = true;
                switch (controls.tileType)
                {
                    case MapEditorControls.tileLayerType.normal:
                        switch (paintTile.spriteEffect)
                        {
                            case SpriteEffects.None:
                                paintTile.spriteEffect = SpriteEffects.FlipVertically;
                                break;
                            case SpriteEffects.FlipHorizontally:
                                paintTile.spriteEffect = SpriteEffects.FlipVertically;
                                break;
                            case SpriteEffects.FlipVertically:
                                paintTile.spriteEffect = SpriteEffects.None;
                                break;
                            default:
                                break;
                        }
                        break;
                    case MapEditorControls.tileLayerType.smartBrush:
                        if (null != smartBrush.getTiles())
                        {
                            foreach (var item in smartBrush.getTiles())
                            {
                                switch (item.bt.spriteEffect)
                                {
                                    case SpriteEffects.None:
                                        item.bt.spriteEffect = SpriteEffects.FlipVertically;
                                        break;
                                    case SpriteEffects.FlipHorizontally:
                                        item.bt.spriteEffect = SpriteEffects.FlipVertically;
                                        break;
                                    case SpriteEffects.FlipVertically:
                                        item.bt.spriteEffect = SpriteEffects.None;
                                        break;
                                    default:
                                        break;
                                }
                            }

                        }
                        break;
                    default:
                        break;
                }

            }

            paint = true;
            if (controls.tileType == MapEditorControls.tileLayerType.normal)
            {

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && Game1.bIsActive && paint)
                {
                    foreach (var item in coords)
                    {
                        int temp = 0;
                        if (paintTile.tileSource.defaultLayer != 0)
                        {
                            if (Form1.layerDepth == 0)
                            {
                                temp = paintTile.tileSource.defaultLayer;
                            }
                            else
                            {
                                temp = paintTile.tileSource.defaultLayer;
                            }

                        }

                        if (Form1.layerDepth != 0 && paintTile.tileSource.defaultLayer != Form1.layerDepth)
                        {
                            temp = Form1.layerDepth;
                        }

                        if (loadedMap.tilesOnPosition(item + mousePosInMapCoord, temp).Count == 0)
                        {
                            loadedMap.TryToAddTile(item + mousePosInMapCoord, paintTile.Clone(), temp);
                        }

                    }

                    loadedMap.CheckChunksToConsider(new Vector2(cameraPosX, -cameraPosY), cameraZoomX);
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed && Game1.bIsActive && paint)
                {
                    foreach (var item in coords)
                    {
                        int temp = 0;
                        if (paintTile.tileSource.defaultLayer != 0)
                        {
                            if (Form1.layerDepth == 0)
                            {
                                temp = paintTile.tileSource.defaultLayer;
                            }
                            else
                            {
                                temp = paintTile.tileSource.defaultLayer;
                            }

                        }


                        if (Form1.layerDepth != 0 && paintTile.tileSource.defaultLayer != Form1.layerDepth)
                        {
                            temp = Form1.layerDepth;
                        }

                        loadedMap.TryToRemoveTile(item + mousePosInMapCoord, temp);
                    }
                }
            }
            else if (controls.tileType == MapEditorControls.tileLayerType.smartBrush)
            {
                if (smartBrush.bIsLearning && !Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    smartBrush.bIsLearning = false;
                }
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Game1.bIsActive && paint)
                {
                    if (smartBrush.bIsLearning || (smartBrush.getTiles() != null && smartBrush.getTiles().Count == 0))
                    {
                        if (memoryBrush.bLearnFromAllLayers)
                        {
                            var temp = loadedMap.tilesOnPosition(mousePosInMapCoord);
                            if (temp.Count != 0)
                            {
                                temp.ForEach(t => smartBrush.Add(t));
                            }
                        }
                        else if (!memoryBrush.bLearnFromAllLayers)
                        {
                            var temp = loadedMap.tilesOnPosition(mousePosInMapCoord, Form1.layerDepth);
                            if (temp.Count != 0)
                            {
                                smartBrush.Add(temp[0]);
                            }
                        }
                    }
                    else
                    {
                        smartBrush.Clear();
                        var temp = loadedMap.tilesOnPosition(mousePosInMapCoord, Form1.layerDepth);
                        if (temp.Count != 0)
                        {
                            smartBrush.Add(temp[0]);
                        }
                    }
                }
                else if (Mouse.GetState().LeftButton == ButtonState.Pressed && !Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Game1.bIsActive && paint)
                {
                    smartBrush.bIsLearning = false;
                    foreach (var item in smartBrush.getTiles())
                    {

                        if (loadedMap.tilesOnPosition(item.bt.positionGrid + mousePosInMapCoord, item.bt.tileLayer).Count == 0)
                        {
                            loadedMap.TryToAddTile(item.bt.positionGrid + mousePosInMapCoord, item.bt.Clone(), item.bt.tileLayer);
                        }

                    }

                    loadedMap.CheckChunksToConsider(new Vector2(cameraPosX, -cameraPosY), cameraZoomX);

                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed && Game1.bIsActive && paint)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                    {
                        smartBrush.Clear();
                    }
                    else if (smartBrush.getTiles() != null)
                    {
                        foreach (var item in smartBrush.getTiles())
                        {

                            if (loadedMap.tilesOnPosition(item.bt.positionGrid + mousePosInMapCoord, item.bt.tileLayer).Count != 0)
                            {
                                loadedMap.TryToRemoveTile(item.bt.positionGrid + mousePosInMapCoord, item.bt.tileLayer);
                            }

                        }

                        loadedMap.CheckChunksToConsider(new Vector2(cameraPosX, -cameraPosY), cameraZoomX);
                    }



                }
            }

            bShowPaint = true;


        }

        static public void CalculateBrushSize()
        {
            coords.Clear();
            switch (brushSize)
            {
                case 1:
                    coords.Add(new Vector2(0, 0));
                    break;
                case 2:
                    coords.Add(new Vector2(0, 0));
                    coords.Add(new Vector2(-1, 0));
                    coords.Add(new Vector2(0, -1));
                    coords.Add(new Vector2(0 - 1, 0 - 1));
                    break;

            }

            if (brushSize > 2)
            {
                coords.Add(new Vector2(0, 0));
                for (int i = brushSize - 2; i > 0; i--)
                {
                    int x = i;
                    int y = 0;
                    int decisionOver2 = 1 - x;   // Decision criterion divided by 2 evaluated at x=r, y=0
                    while (y <= x)
                    {
                        coords.Add(new Vector2(x + (int)0, y + (int)0)); // Octant 1
                        coords.Add(new Vector2(y + (int)0, x + (int)0)); // Octant 2
                        coords.Add(new Vector2(-x + (int)0, y + (int)0)); // Octant 4
                        coords.Add(new Vector2(-y + (int)0, x + (int)0)); // Octant 3
                        coords.Add(new Vector2(-x + (int)0, -y + (int)0)); // Octant 5
                        coords.Add(new Vector2(-y + (int)0, -x + (int)0)); // Octant 6
                        coords.Add(new Vector2(x + (int)0, -y + (int)0)); // Octant 8
                        coords.Add(new Vector2(y + (int)0, -x + (int)0)); // Octant 7
                        y++;
                        if (decisionOver2 <= 0)
                        {
                            decisionOver2 += 2 * y + 1;   // Change in decision criterion for y -> y+1
                        }
                        else
                        {
                            x--;
                            decisionOver2 += 2 * (y - x) + 1;   // Change for y -> y+1, x -> x-1
                        }
                    }
                }

            }
        }

        public override void UnloadContent(Game1 game)
        {
        }

        private int timer = 5000;
        private int timePassed = 0;


        public override SpriteBatch Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            if (TalentGridEditor.bIsRunning)
            {
                TalentGridEditor.Draw(spriteBatch);
                return spriteBatch;
            }

            Game1.graphics.GraphicsDevice.Clear(Color.White);

            if (!bPlayTest && !HitboxEditor.bIsRunning && !ShowParticleSystem)
            {
                timePassed += gametime.ElapsedGameTime.Milliseconds;
                #region Item editor preview draw
                if (functionForm == Form1.ActiveForm && functionForm != null && bAllowEditing && paintTile != null && timePassed >= timer && paintTile.tileSource.tileAnimation != null && paintTile.tileSource.tileAnimation.animationFrames.Count > 0)
                {
                    //spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
                    //spriteBatch.GraphicsDevice.SetRenderTarget(gifRender);
                    //spriteBatch.Begin();
                    //BasicTile tempTile = paintTile.Clone();
                    //spriteBatch.Draw(tempTile.tileSource.tileAnimation.animationTexture, new Rectangle(0, 0, 64, 64), tempTile.tileSource.tileAnimation.animationFrames[0], Color.Gray);
                    //spriteBatch.End();
                    //spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                    //byte[] data = new byte[gifRender.Width * gifRender.Height];
                    //MemoryStream m = new MemoryStream(data);
                    //gifRender.SaveAsPng(m, gifRender.Width, gifRender.Height);
                    //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m);
                    //functionForm.pictureBox3.Image = bmp;
                    //functionForm.pictureBox3.Refresh();

                }
                #endregion

                #region animation pre render Char creator
                if (Form1.charEditorForm == Form1.ActiveForm && Form1.charEditorForm.tabPage2 == Form1.charEditorForm.tabControl1.SelectedTab)
                {
                    var cf = Form1.charEditorForm;

                    if (cf.listBox3.SelectedIndex != -1 && cf.listBox4.SelectedIndex != -1)
                    {
                        if (gifRender.Width != 64 && gifRender.Height != 64)
                        {
                            gifRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 64, 64);
                        }

                        if (!CharacterEditorForm.characters[cf.listBox3.SelectedIndex].charAnimations[cf.listBox4.SelectedIndex].bDirectional)
                        {
                            spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
                            spriteBatch.GraphicsDevice.SetRenderTarget(gifRender);
                            spriteBatch.Begin();
                            CharacterEditorForm.characters[cf.listBox3.SelectedIndex].charAnimations[cf.listBox4.SelectedIndex].PreviewDraw(spriteBatch);
                            spriteBatch.End();
                            spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                            byte[] data = new byte[gifRender.Width * gifRender.Height];
                            MemoryStream m = new MemoryStream(data);
                            gifRender.SaveAsPng(m, gifRender.Width, gifRender.Height);
                            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m);
                            cf.pictureBox1.Image = bmp;
                            cf.pictureBox1.Refresh();
                        }
                        else if (CharacterEditorForm.characters[cf.listBox3.SelectedIndex].charAnimations[cf.listBox4.SelectedIndex].bDirectional && cf.listBox5.SelectedIndex != -1)
                        {
                            CharacterEditorForm.characters[cf.listBox3.SelectedIndex].charAnimations[cf.listBox4.SelectedIndex].PreviewUpdate(gametime, cf.listBox5.SelectedIndex);
                            spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
                            spriteBatch.GraphicsDevice.SetRenderTarget(gifRender);
                            spriteBatch.Begin();
                            CharacterEditorForm.characters[cf.listBox3.SelectedIndex].charAnimations[cf.listBox4.SelectedIndex].PreviewDraw(spriteBatch, cf.listBox5.SelectedIndex);
                            spriteBatch.End();
                            spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                            byte[] data = new byte[gifRender.Width * gifRender.Height];
                            MemoryStream m = new MemoryStream(data);
                            gifRender.SaveAsPng(m, gifRender.Width, gifRender.Height);
                            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m);
                            cf.pictureBox1.Image = bmp;
                            cf.pictureBox1.Refresh();
                        }

                    }
                }
                else if (Form1.charEditorForm == Form1.ActiveForm && Form1.charEditorForm.tabPage3 == Form1.charEditorForm.tabControl1.SelectedTab)
                {
                    var cf = Form1.charEditorForm;

                    if (cf.listBox6.SelectedIndex != -1 && gcDB.gameCharacters[cf.listBox6.SelectedIndex].charAnimations[(int)BaseCharacter.CharacterAnimations.Idle].animationTexture != null)
                    {
                        spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
                        spriteBatch.GraphicsDevice.SetRenderTarget(gifRender);
                        spriteBatch.Begin();
                        gcDB.gameCharacters[cf.listBox6.SelectedIndex].charAnimations[(int)BaseCharacter.CharacterAnimations.Idle].PreviewDraw(spriteBatch);
                        spriteBatch.End();
                        spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                        byte[] data = new byte[gifRender.Width * gifRender.Height];
                        MemoryStream m = new MemoryStream(data);
                        gifRender.SaveAsPng(m, gifRender.Width, gifRender.Height);
                        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m);
                        cf.pictureBox3.Image = bmp;
                        cf.pictureBox3.Refresh();
                    }
                }
                #endregion
                #region pre render char battle animation editor
                if (Form1.charEditorForm == Form1.ActiveForm && Form1.charEditorForm.tabPage4 == Form1.charEditorForm.tabControl1.SelectedTab)
                {
                    var cf = Form1.charEditorForm;

                    if (cf.listBox9.SelectedIndex != -1 && cf.listBox11.SelectedIndex != -1)
                    {
                        if (gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations.Count != 0 &&
                        gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations[cf.listBox11.SelectedIndex] != null
                            && gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations[cf.listBox11.SelectedIndex].animationFrames.Count != 0)
                        {
                            if (gifRender.Width != gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations[cf.listBox11.SelectedIndex].animationFrames[0].Width
                                && gifRender.Height != gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations[cf.listBox11.SelectedIndex].animationFrames[0].Height)
                            {
                                gifRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations[cf.listBox11.SelectedIndex].animationFrames[0].Width, gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations[cf.listBox11.SelectedIndex].animationFrames[0].Height);
                                Console.WriteLine("AYE");
                            }
                            spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
                            spriteBatch.GraphicsDevice.SetRenderTarget(gifRender);
                            spriteBatch.Begin();
                            gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations[cf.listBox11.SelectedIndex].BAnimUpdate(gametime, gcDB.gameCharacters[cf.listBox9.SelectedIndex]);
                            gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations[cf.listBox11.SelectedIndex].TestBattleAnimationDraw(spriteBatch);
                            spriteBatch.End();
                            spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                            spriteBatch.Begin();
                            gcDB.gameCharacters[cf.listBox9.SelectedIndex].charBattleAnimations[cf.listBox11.SelectedIndex].TestBattleAnimationDraw(spriteBatch);
                            spriteBatch.End();
                            byte[] data = new byte[gifRender.Width * gifRender.Height];
                            MemoryStream m = new MemoryStream(data);
                            gifRender.SaveAsPng(m, gifRender.Width, gifRender.Height);
                            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m);
                            cf.pictureBox5.Image = bmp;
                            cf.pictureBox5.Refresh();
                        }
                    }
                }
                #endregion
                #region Advanced object editor preview draw
                if (functionForm == Form1.ActiveForm && functionForm.tabPage8 == functionForm.tabControl2.SelectedTab)
                {
                    if (functionForm.listBox7.SelectedIndex != -1 && functionForm.listBox8.SelectedIndex != -1)
                    {
                        if (gifRender.Width != 64 && gifRender.Height != 64)
                        {
                            gifRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 64, 64);
                        }

                        if (!parentMap.mapSprites[functionForm.listBox7.SelectedIndex].baseAnimations[functionForm.listBox8.SelectedIndex].bDirectional)
                        {
                            spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
                            spriteBatch.GraphicsDevice.SetRenderTarget(gifRender);
                            spriteBatch.Begin();
                            parentMap.mapSprites[functionForm.listBox7.SelectedIndex].baseAnimations[functionForm.listBox8.SelectedIndex].PreviewDraw(spriteBatch);
                            spriteBatch.End();
                            spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                            byte[] data = new byte[gifRender.Width * gifRender.Height];
                            MemoryStream m = new MemoryStream(data);
                            gifRender.SaveAsPng(m, gifRender.Width, gifRender.Height);
                            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m);
                            functionForm.pictureBox1.Image = bmp;
                            functionForm.pictureBox1.Refresh();
                        }
                    }
                }
                #endregion
                #region Item editor preview draw
                if (Form1.ic != null && Form1.ic == ItemCreator.ActiveForm && Form1.ic.listBox1.SelectedIndex != -1 && false)
                {
                    if (gifRender.Width != 64 && gifRender.Height != 64)
                    {
                        gifRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 64, 64);
                    }

                    spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.GraphicsDevice.SetRenderTarget(gifRender);
                    spriteBatch.Begin();
                    ((BaseItem)Form1.ic.listBox1.SelectedItem).itemTexAndAnimation.UpdateAnimationForItems(gametime);
                    ((BaseItem)Form1.ic.listBox1.SelectedItem).itemTexAndAnimation.PreviewDraw(spriteBatch);
                    spriteBatch.End();
                    spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                    byte[] data = new byte[gifRender.Width * gifRender.Height];
                    MemoryStream m = new MemoryStream(data);
                    gifRender.SaveAsPng(m, gifRender.Width, gifRender.Height);
                    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m);
                    Form1.ic.pictureBox1.Image = bmp;
                    Form1.ic.pictureBox1.Refresh();

                }
                #endregion


                if (bIsRunning || bObjectLayer)
                {
                    #region mapBuilder drawing
                    minimapRenderTimer = 5000;

                    TestEnvironment.Draw(spriteBatch);

                    #region maingameWindow render
                    Rectangle camera = new Rectangle(0 + cameraPosX, 0 - cameraPosY, 1366, 768);
                    spriteBatch.GraphicsDevice.SetRenderTarget(mainWindowRender);
                    spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, MapEditorMatrix);

                    loadedMap.DrawMapEditor(spriteBatch, gcDB, camera, mainWindowRender, MapEditorMatrix, cameraZoomX);


                    switch (controls.currentType)
                    {
                        case MapEditorControls.controlType.None:
                            break;
                        case MapEditorControls.controlType.TileLayer:
                            break;
                        case MapEditorControls.controlType.ObjectLayerEditing:
                            if (objSelection != null && objSelection.IsCorrect())
                            {
                                objSelection.MarkedDraw(spriteBatch);
                            }
                            break;
                        case MapEditorControls.controlType.ObjectLayerAdding:
                            if (controls.currentType == MapEditorControls.controlType.ObjectLayerAdding && objAddition != null && objAddition.IsCorrect())
                            {

                                objAddition.MarkedDraw(spriteBatch);
                            }
                            break;
                        case MapEditorControls.controlType.HitboxEditor:
                            break;
                        case MapEditorControls.controlType.TileSelect:
                            EditorHelpClass.openMoveTiles.ForEach(tile => tile.Draw(spriteBatch, gcDB, 0.4f, Color.Red));
                            break;
                        default:
                            break;
                    }
                    spriteBatch.Draw(Game1.hitboxHelp, Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale / cameraZoomX + new Vector2(cameraPosX, -cameraPosY), Color.Purple);
                    //    loadedMap.DrawSprites(spriteBatch,camera,cameraZoomX);
                    if (bShowPaint && paintTile != default(BasicTile))
                    {
                        switch (controls.tileType)
                        {
                            case MapEditorControls.tileLayerType.normal:
                                foreach (var item in coords)
                                {
                                    var r = new Rectangle((int)((item + mousePosInMapCoord).X * 64), (int)((item + mousePosInMapCoord).Y * 64), 64, 64);
                                    spriteBatch.Draw(Game1.hitboxHelp, r, Game1.hitboxHelp.Bounds, Color.Gold);
                                    paintTile.DrawOnDemand(spriteBatch, item + mousePosInMapCoord, Color.LightGray);
                                }
                                break;
                            case MapEditorControls.tileLayerType.smartBrush:
                                if (smartBrush.getTiles() != null)
                                {
                                    if (smartBrush.bIsLearning)
                                    {
                                        foreach (var item in smartBrush.learningFrom)
                                        {
                                            var r = new Rectangle((int)(item.positionGrid.X * 64), (int)(item.positionGrid.Y * 64), 64, 64);
                                            spriteBatch.Draw(Game1.hitboxHelp, r, Game1.hitboxHelp.Bounds, Color.Gold);
                                            item.DrawOnDemand(spriteBatch, item.positionGrid, Color.LightCoral);
                                        }
                                    }
                                    else
                                    {

                                        foreach (var item in smartBrush.getTiles())
                                        {
                                            var r = new Rectangle((int)((item.bt.positionGrid.X + mousePosInMapCoord.X) * 64), (int)((item.bt.positionGrid.Y + mousePosInMapCoord.Y) * 64), 64, 64);
                                            spriteBatch.Draw(Game1.hitboxHelp, r, Game1.hitboxHelp.Bounds, Color.Gold);
                                            item.bt.DrawOnDemand(spriteBatch, item.bt.positionGrid + mousePosInMapCoord, Color.LightGray);
                                        }
                                    }
                                }

                                break;
                            default:
                                break;
                        }

                    }


                    if (controls.currentType == MapEditorControls.controlType.ObjectLayerEditing)
                    {

                    }

                    if ((bZoneEditor || bRegionEditor) && drawPoint1 != new Vector2(-1))
                    {
                        spriteBatch.Draw(Game1.hitboxHelp, newZoneRegionSize, Color.Purple);
                    }

                    if (bRegionEditor || bZoneEditor)
                    {

                        if (selectedRegion != null)
                        {
                            foreach (var item in selectedRegion.regionSizes)
                            {
                                spriteBatch.Draw(Game1.hitboxHelp, item, Color.Orange);
                            }
                        }

                        if (selectedZone != null)
                        {
                            foreach (var item in selectedZone.zoneSizes)
                            {
                                spriteBatch.Draw(Game1.hitboxHelp, item, Color.LightBlue);
                            }
                        }


                    }
                    #endregion

                    foreach (var item in eprList)
                    {
                        item.Draw(spriteBatch);
                    }

                    if (uic != null && TestEnvironment.uic != null && uic.UICollectionRender != null)
                    {
                        uic = TestEnvironment.uic;
                        spriteBatch.End();
                        // spriteBatch.GraphicsDevice.Clear(Color.White);
                        spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

                        spriteBatch.Draw(uic.UICollectionRender, new Rectangle(uic.startMainElement.position, uic.UICollectionRender.Bounds.Size), Color.White);
                    }

                    //IMPORTANT DRAWING IS BEING DONE HERE!
                    #region rendertargetsDraw

                    spriteBatch.End();
                    spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                    spriteBatch.GraphicsDevice.Clear(Color.White);

                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);

                    if (!testLight)
                    {
                        spriteBatch.Draw(mainWindowRender, new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, 1366, 768), Color.White);
                    }
                    else
                    {
                        GameScreenEffect.DrawLightEditor(spriteBatch, mainWindowRender, lightTempRender, loadedMap);
                        spriteBatch.End();
                        spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                        spriteBatch.GraphicsDevice.Clear(Color.White);
                        spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
                        spriteBatch.Draw(lightTempRender, new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, 1366, 768), Color.White);
                    }


                    foreach (var item in eprList)
                    {
                        item.FinalDraw(spriteBatch);
                    }

                    spriteBatch.End();

                    if (bDrawGUI)
                    {
                        BattleGUI.QuickDrawInEditor(spriteBatch, gametime);
                    }
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, SceneUtility.transform);
                    #endregion

                    if (bIsRunning && !bPlayTest)
                    {
                        switch (controls.currentType)
                        {
                            case MapEditorControls.controlType.None:
                                break;
                            case MapEditorControls.controlType.TileLayer:
                                if (controls.tileType == MapEditorControls.tileLayerType.smartBrush)
                                {
                                    if (memoryBrush.bLearnFromAllLayers)
                                    {
                                        spriteBatch.DrawString(Game1.defaultFont, "Smartbrush all layers", new Vector2(350, 50), Color.Gold, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                                    }
                                    else
                                    {
                                        spriteBatch.DrawString(Game1.defaultFont, "Smartbrush only selected layer", new Vector2(350, 50), Color.Gold, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                                    }
                                }
                                break;
                            case MapEditorControls.controlType.ObjectLayerEditing:
                                switch (controls.currentObjectEditType)
                                {
                                    case MapEditorControls.objectLayerType.normal:
                                        break;
                                    case MapEditorControls.objectLayerType.ObjectGroup:
                                        spriteBatch.DrawString(Game1.defaultFont, "Object group editor, TAB to exit", new Vector2(350, 50), Color.Gold, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                                        break;
                                    case MapEditorControls.objectLayerType.MultiSelect:
                                        spriteBatch.DrawString(Game1.defaultFont, "Multi select, TAB to exit", new Vector2(350, 50), Color.Gold, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case MapEditorControls.controlType.ObjectLayerAdding:
                                break;
                            case MapEditorControls.controlType.HitboxEditor:
                                break;
                            default:
                                break;
                        }



                        spriteBatch.DrawString(Game1.defaultFont, "Editing enabled: " + bAllowEditing, new Vector2(20, 50), Color.Black);
                        spriteBatch.DrawString(Game1.defaultFont, "Current layer: " + Form1.layerDepth, new Vector2(20, 75), Color.Black);

                    }
                    #endregion
                }


                if (timePassed > timer)
                {
                    if (Form1.ActiveForm == functionForm && functionForm.bUpdateImage)
                    {
                        byte[] data = new byte[mainWindowRender.Width * mainWindowRender.Height];

                        MemoryStream m = new MemoryStream(data);
                        mainWindowRender.SaveAsPng(m, mainWindowRender.Width, mainWindowRender.Height);
                        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m);
                        functionForm.doSomething(bmp);
                        timePassed = 0;
                    }
                    else
                    {
                        timePassed = 0;
                    }

                }

                spriteBatch.End();

                if (FrameSelector.bIsRunning)
                {
                    FrameSelector.Draw(spriteBatch);
                }

                if (SpriteSelector.bIsRunning)
                {
                    SpriteSelector.Draw(spriteBatch);
                }

                if (Form1.charEditorForm.tabPage2 == Form1.charEditorForm.tabControl1.SelectedTab && Form1.charEditorForm != Form1.ActiveForm && Form1.charEditorForm.listBox3.SelectedIndex != -1 && Form1.charEditorForm.listBox4.SelectedIndex != -1)
                {
                    CharacterEditorForm.characters[Form1.charEditorForm.listBox3.SelectedIndex].charAnimations[Form1.charEditorForm.listBox4.SelectedIndex].frameIndex = 0;
                }

            }
            else if (bPlayTest)
            {
                GameProcessor.Draw(spriteBatch);
            }
            else if (HitboxEditor.bIsRunning)
            {
                HitboxEditor.Draw(spriteBatch);
            }
            else if (ShowParticleSystem)
            {
                if (selectedParticleSource != null)
                {
                    spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                    selectedParticleSource.Draw(spriteBatch);
                    spriteBatch.End();
                }
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);

            if (!MapBuilder.bPlayTest)
            {
                spriteBatch.DrawString(Game1.defaultFont, "mouse: {" + mousePosInMapCoord.X + "," + mousePosInMapCoord.Y + "}", new Vector2(10, 10), Color.LightGoldenrodYellow, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

            }

            return spriteBatch;
        }

        static public void OpenNewTileSheet(TileSheet ts)
        {
        }

        static public void AssignSubMap(BasicMap SubMap)
        {
            loadedMap = SubMap;
        }

        internal struct memoryBrush
        {
            internal struct tileInfo
            {
                internal BasicTile bt;
                internal int layer;

                internal tileInfo(BasicTile bt, int layer)
                {
                    this.bt = bt;
                    this.layer = layer;
                }

                internal bool Equals(tileInfo ti)
                {
                    if (this.bt.positionGrid == ti.bt.positionGrid && bt.tileLayer == ti.bt.tileLayer)
                    {
                        return true;
                    }
                    return false;
                }
            }

            Vector2 offset;
            List<tileInfo> lbt;
            internal List<BasicTile> learningFrom;
            static internal bool bUsing = false;
            internal bool bIsLearning;
            static internal bool bLearnFromAllLayers = false;

            internal void Start(BasicTile bt)
            {
                bIsLearning = true;
                offset = bt.positionGrid;
                lbt = new List<tileInfo>();
                learningFrom = new List<BasicTile>();
                learningFrom.Add(bt);
                var temp = bt.Clone();
                temp.positionGrid -= offset;
                lbt.Add(new tileInfo(temp, Form1.layerDepth));
            }

            internal void Add(BasicTile bt)
            {
                if (lbt.Count == 0)
                {
                    Start(bt);
                }
                var temp = bt.Clone();
                temp.positionGrid -= offset;
                var ti = new tileInfo(temp, Form1.layerDepth);
                bool bHasTI = false;

                foreach (var item in lbt)
                {
                    if (item.Equals(ti))
                    {
                        bHasTI = true;
                        break;
                    }
                }

                if (!bHasTI)
                {
                    lbt.Add(ti);
                    learningFrom.Add(bt);
                }
            }

            internal List<tileInfo> getTiles()
            {
                return lbt;
            }

            internal void Clear()
            {
                lbt.Clear();
            }
        }

    }

    static internal class EditorHelpClass
    {
        static internal List<BasicTile> openMoveTiles = new List<BasicTile>();
        static internal List<MapChunk> previousLoadedMapChunks = new List<MapChunk>();
    }

    static public class FileSelector
    {
        static System.Windows.Forms.OpenFileDialog testFileDialogue;
        public static bool bRunFileSelector = false;

        public static void Start()
        {
            testFileDialogue = new System.Windows.Forms.OpenFileDialog();
            bRunFileSelector = true;
            testFileDialogue.InitialDirectory = Environment.CurrentDirectory;
        }

        public static void Update()
        {
            if (bRunFileSelector)
            {
                // Set filter options and filter index.
                testFileDialogue.Filter = "CG Files (.cgts)|*.cgts";
                testFileDialogue.FilterIndex = 1;

                testFileDialogue.Multiselect = false;

                // Call the ShowDialog method to show the dialog box.

                System.Windows.Forms.DialogResult testDia = testFileDialogue.ShowDialog();

                // Process input if the user clicked OK.
                if (testDia == System.Windows.Forms.DialogResult.OK)
                {
                    /*
                    // Open the selected file to read.
                    System.IO.Stream fileStream = openFileDialog1.File.OpenRead();

                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                    {
                        // Read the first line from the file and write it the textbox.
                        tbResults.Text = reader.ReadLine();
                    }
                    fileStream.Close();¨*/
                    Console.WriteLine("You selected: " + testFileDialogue.FileName);
                    bRunFileSelector = false;
                }
            }

        }
    }

    static public class FrameSelector
    {
        #region
        static int SceneX = 0;
        static int SceneY = 0;
        static Vector2 editorMouse = Vector2.Zero;
        static public List<Rectangle> frames = new List<Rectangle>();
        static Texture2D animationSheet;
        static Matrix sceneCamera;
        static public bool bIsRunning = false;
        static RenderTarget2D frameAnimRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static Point frameSurface = new Point(64);
        static Point frameOffSet = new Point(0);
        #endregion

        static public void StartSimple(ShapeAnimation anim, int ri = -1)
        {
            MapBuilder.controls.bIgnore = true;
            frameOffSet = new Point(0);
            frameSurface = new Point(64);
            frameAnimRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            MapBuilder.bIsRunning = false;
            bIsRunning = false;
            SceneY = 0;
            SceneX = 0;
            animationSheet = anim.animationTexture;
            bIsRunning = true;
            frames = anim.animationFrames;
            if (anim.bDirectional)
            {
                frames = anim.animationFramesDirectional[ri];
            }
        }

        static public void StartComplex(ShapeAnimation anim, int w, int h, int fosW, int fosH)
        {
            MapBuilder.controls.bIgnore = true;
            frameOffSet = new Point(fosW, -fosH);
            frameSurface = new Point(w, h);
            frameAnimRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            MapBuilder.bIsRunning = false;
            bIsRunning = false;
            SceneY = 0;
            SceneX = 0;
            animationSheet = anim.animationTexture;
            bIsRunning = true;
            frames = anim.animationFrames;
        }

        static public void StartComplex(Texture2D tex, List<Rectangle> lf, int w, int h, int fosW, int fosH)
        {
            MapBuilder.controls.bIgnore = true;
            frameOffSet = new Point(fosW, -fosH);
            frameSurface = new Point(w, h);
            frameAnimRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            MapBuilder.bIsRunning = false;
            bIsRunning = false;
            SceneY = 0;
            SceneX = 0;
            animationSheet = tex;
            bIsRunning = true;
            frames = lf;
        }

        static public void Stop()
        {
            bIsRunning = false;
            MapBuilder.bIsRunning = false;
            MapBuilder.StandardMapBuilder();
            MapBuilder.controls.bIgnore = false;

        }

        static public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                SceneY += 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                SceneY -= 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                SceneX -= 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                SceneX += 3;
            }

            editorMouse = Mouse.GetState().Position.ToVector2() + new Vector2(SceneX - frameOffSet.X, -SceneY);
          //  frames.Clear();
            if (animationSheet.Bounds.Contains(editorMouse))
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    int xTile = (int)((editorMouse.X) / frameSurface.X);
                    int yTile = (int)((editorMouse.Y) / frameSurface.Y);
                    frames.Add(new Rectangle(xTile * frameSurface.X - frameOffSet.X, yTile * frameSurface.Y - frameOffSet.Y, frameSurface.X, frameSurface.Y));
                    if (frames.Last().X + frames.Last().Width > animationSheet.Width)
                    {
                        frames[frames.Count - 1] = new Rectangle(frames.Last().X, frames.Last().Y, frameSurface.X - (frames.Last().X + frames.Last().Width - animationSheet.Width), frames.Last().Height);
                    }
                    if (frames.Last().Y + frames.Last().Height > animationSheet.Height)
                    {
                        frames[frames.Count - 1] = new Rectangle(frames.Last().X, frames.Last().Y, frames.Last().Width, frameSurface.Y - (frames.Last().Y + frames.Last().Height - animationSheet.Height));
                    }
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    int xTile = (int)(editorMouse.X / frameSurface.X);
                    int yTile = (int)(editorMouse.Y / frameSurface.Y);
                    frames.Remove(new Rectangle(xTile * frameSurface.X - frameOffSet.X, yTile * frameSurface.Y - frameOffSet.Y, frameSurface.X, frameSurface.Y));
                    frames.RemoveAll(f => f.Contains(editorMouse));
                }
            }


            sceneCamera = Matrix.CreateTranslation(-SceneX, SceneY, 1);

        }

        static public void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.CornflowerBlue);
            sb.GraphicsDevice.SetRenderTarget(frameAnimRender);
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, sceneCamera);
            sb.Draw(animationSheet, frameOffSet.ToVector2(), Color.White);
            int i = 0;
            foreach (var item in frames)
            {
                sb.Draw(Game1.hitboxHelp, new Rectangle(item.X + frameOffSet.X, item.Y + frameOffSet.Y, item.Width, item.Height), Color.Blue * .5f);
                //  sb.Draw(Game1.hitboxHelp, item, Color.Blue * .5f);
                sb.DrawString(Game1.defaultFont, i.ToString(), item.Location.ToVector2() + frameOffSet.ToVector2(), Color.Black);
                i++;
            }
            sb.End();

            sb.GraphicsDevice.SetRenderTarget(Game1.gameRender);
            sb.GraphicsDevice.Clear(Color.OliveDrab);

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            sb.Draw(frameAnimRender, Vector2.Zero, Color.White);
            sb.End();
        }
    }

    static public class SpriteSelector
    {
        #region
        static int SceneX = 0;
        static int SceneY = 0;
        static Vector2 editorMouse = Vector2.Zero;
        static public Rectangle frame = default(Rectangle);
        static Texture2D animationSheet;
        static Matrix sceneCamera;
        static public bool bIsRunning = false;
        static RenderTarget2D frameAnimRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static Point frameSurface = new Point(64);
        static Point frameOffSet = new Point(0);
        static public BaseSprite editingSprite = new BaseSprite();
        #endregion

        static public void StartComplex(BaseSprite bs, int w, int h, int fosW, int fosH)
        {
            frameOffSet = new Point(fosW, -fosH);
            frameSurface = new Point(w, h);
            frameAnimRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            MapBuilder.bIsRunning = false;
            bIsRunning = false;
            SceneY = 0;
            SceneX = 0;
            animationSheet = bs.shapeTexture;
            bIsRunning = true;
            frame = bs.shapeTextureBounds;
            editingSprite = bs;
        }

        static public void Stop()
        {
            bIsRunning = false;
            MapBuilder.bIsRunning = false;
            MapBuilder.StandardMapBuilder();
        }

        static public void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                SceneY += 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                SceneY -= 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                SceneX -= 3;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                SceneX += 3;
            }

            editorMouse = Mouse.GetState().Position.ToVector2() + new Vector2(SceneX + frameOffSet.X, -SceneY);
            if (animationSheet.Bounds.Contains(editorMouse))
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    int xTile = (int)((editorMouse.X) / frameSurface.X);
                    int yTile = (int)((editorMouse.Y) / frameSurface.Y);
                    frame = new Rectangle(xTile * frameSurface.X + frameOffSet.X, yTile * frameSurface.Y - frameOffSet.Y, frameSurface.X, frameSurface.Y);
                    editingSprite.rectangleToDraw = frame;
                    editingSprite.hitBoxTexBox = frame;
                    editingSprite.GenerateHitBoxesFromOtherTex();
                    editingSprite.GenerateRotationHitboxes();
                }
                else if (Mouse.GetState().RightButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    frame = default(Rectangle);
                }
            }


            sceneCamera = Matrix.CreateTranslation(-SceneX, SceneY, 1);

        }

        static public void Draw(SpriteBatch sb)
        {
            sb.GraphicsDevice.Clear(Color.CornflowerBlue);
            sb.GraphicsDevice.SetRenderTarget(frameAnimRender);
            sb.Begin(SpriteSortMode.Deferred, null, null, null, null, null, sceneCamera);
            sb.Draw(animationSheet, frameOffSet.ToVector2(), Color.White);
            if (frame != default(Rectangle))
            {
                sb.Draw(Game1.hitboxHelp, new Rectangle(frame.X + frameOffSet.X, frame.Y + frameOffSet.Y, frame.Width, frame.Height), Color.Blue * .5f);
            }


            sb.End();

            sb.GraphicsDevice.SetRenderTarget(Game1.gameRender);
            sb.GraphicsDevice.Clear(Color.OliveDrab);

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            sb.Draw(frameAnimRender, Vector2.Zero, Color.White);
            sb.End();
        }
    }

    public class EditorPreviewRender
    {
        RenderTarget2D render;
        RenderTarget2D aidRender;
        public Rectangle location;
        Object renderObject;
        Type renderObjectType;
        public bool bRemove = true;
        public Vector2 renderPosition = new Vector2(0);
        float scale = 1.0f;

        public EditorPreviewRender(Type type, Object obj, float scale = 1)
        {
            renderObjectType = type;
            this.scale = scale;

            if (type == typeof(BaseItem) || type.IsSubclassOf(typeof(BaseItem)))
            {
                var temp = obj as BaseItem;

                renderObject = temp.Clone();

                if (temp.itemTexAndAnimation.HasProperAnim())
                {
                    bRemove = false;
                    var tempSize = temp.itemTexAndAnimation.AnimSourceSize();
                    render = new RenderTarget2D(Game1.graphics.GraphicsDevice, (int)(tempSize.X * scale), (int)(tempSize.Y * scale));
                }

                location = new Rectangle(0, 0, (int)(render.Width), (int)(render.Height));
            }
            else if (type == typeof(ParticleAnimation))
            {
                var temp = obj as ParticleAnimation;

                renderObject = temp.Clone();

                if (temp.HasProperAnim())
                {
                    bRemove = false;
                    var tempSize = temp.AnimSourceSize();
                    render = new RenderTarget2D(Game1.graphics.GraphicsDevice, (int)(tempSize.X * scale), (int)(tempSize.Y * scale + temp.magicCircleVOS * scale));
                }

                location = new Rectangle(0, 0, render.Width, render.Height);
            }
            else if (type == typeof(BasicMap))
            {
                var temp = obj as BasicMap;

                renderObject = temp;

                bRemove = false;
                render = new RenderTarget2D(Game1.graphics.GraphicsDevice, (int)(1366 / 2.2f), (int)(768 / 2.2f));
                aidRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

                location = new Rectangle(0, 0, render.Width, render.Height);
            }


        }

        public void Update(GameTime gt)
        {
            if (renderObjectType == typeof(BaseItem) || renderObjectType.IsSubclassOf(typeof(BaseItem)))
            {
                var temp = renderObject as BaseItem;
                temp.itemTexAndAnimation.UpdateAnimationForItems(gt);
            }
            else if (renderObjectType == typeof(ParticleAnimation))
            {
                var temp = renderObject as ParticleAnimation;
                temp.UpdateAnimationForItems(gt);

                if (temp.HasProperCharacterLogic())
                {
                    temp.externalUpdate(gt);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.Black);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            if (renderObjectType == typeof(BaseItem) || renderObjectType.IsSubclassOf(typeof(BaseItem)))
            {
                var temp = renderObject as BaseItem;
                temp.itemTexAndAnimation.Draw(sb, location);
            }
            else if (renderObjectType == typeof(ParticleAnimation))
            {
                var temp = renderObject as ParticleAnimation;
                temp.BattleAnimationDrawWOShadow(sb, Vector2.Zero, scale, false, Color.White);
            }
            else if (renderObjectType == typeof(BasicMap))
            {

                var temp = renderObject as BasicMap;

                sb.End();
                sb.GraphicsDevice.SetRenderTarget(aidRender);
                sb.GraphicsDevice.Clear(Color.Black);
                sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                temp.DrawMapEditorRender(sb, MapBuilder.gcDB, new Rectangle(0, 0, 1366, 768));
                sb.End();

                sb.GraphicsDevice.SetRenderTarget(render);
                sb.GraphicsDevice.Clear(Color.Black);
                sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                sb.Draw(aidRender, location, aidRender.Bounds, Color.White);
            }

            sb.End();
        }

        public void FinalDraw(SpriteBatch sb)
        {
            sb.Draw(render, renderPosition, location, Color.White);
        }

        public bool Contains(Point p)
        {
            Rectangle r = new Rectangle((int)renderPosition.X, (int)renderPosition.Y, location.Width, location.Height);
            return r.Contains(p);
        }

        public void Remove()
        {
            if (render != null && !render.IsDisposed)
            {
                render.Dispose();
            }

            if (aidRender != null && !aidRender.IsDisposed)
            {
                aidRender.Dispose();
            }
        }
    }
}
