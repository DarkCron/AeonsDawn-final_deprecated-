using Game1.Utilities.GamePlay.Battle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TBAGW.AI;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW
{
    public static class GameProcessor
    {
        public static BasicMap parentMap;
        public static BasicMap loadedMap;
        static public Vector2 sceneCamera = new Vector2(0);

        static public RenderTarget2D gameRender;
        static public RenderTarget2D shadowRender;
        static internal RenderTarget2D lastDrawnMapRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

        static public bool bInCombat = false;
        static public bool bInNonCombat = false;
        static public bool bInGameMenu = false;
        static public bool bInDialogue = false;
        static public bool bInSoloCombat = false;
        static public BaseSprite mainController;
        static public bool bIsInGame = false;
        static public Matrix CameraScaleMatrix;
        static public Matrix CameraScaleMatrixNoZooming;
        static public List<BasicTile>[] overlayBasicTile = { new List<BasicTile>(), new List<BasicTile>() };
        static public Vector2 selectedGridPos = new Vector2(-1);
        static public Vector2 clickedGridPos = new Vector2(-1);
        static public float zoom = 2.1f;
        static public Point cameraPosition = new Point(0);
        static public bool bGameUpdateIsPaused = false;
        static public BaseCharacter g;
        static public BaseCharacter r;
        // static public List<BaseCharacter> encounterEnemies = new List<BaseCharacter>();
        static public Vector2 EditorCursorPos;
        static public PlayerSaveData psData;
        static public GameContentDataBase gcDB = new GameContentDataBase();
        static public List<UIElement> popUpRenders = new List<UIElement>();


        static Effect grassEffect = Game1.contentManager.Load<Effect>(@"FX\GrassEffect");
        //static Texture2D someGrassTex = Game1.contentManager.Load<Texture2D>(@"grass");
        //static Texture2D someGrassTex2 = Game1.contentManager.Load<Texture2D>(@"grass2");
        static float horizontalGrassModifier = 1;
        static int grassModifierTimePassed = 0;
        static public List<ActiveUIElement> UIElements = new List<ActiveUIElement>();

        static public bool bCameraOnController = true;
        static public float zoomBeforeCameraChange = 1f;
        static public float cameraZoomAmount = 1f;
        static public BaseSprite cameraFollowTarget;
        static public Vector2 cameraMoveAmount = new Vector2();
        static int cameraStepsDone = 0;
        static int cameraStepsNeedToDo = 0;

        static public BaseSprite mainControllerBeforeCombat = null;
        static public bool bIsOverWorldOutsideGame = true;
        static public List<RenderTarget2D> UIRenders = new List<RenderTarget2D>();
        static internal bool bUpdateShadows = true;
        static internal bool bUpdateOnceMore = false;
        //static 
        internal static bool bShadowsEnabled = true;
        internal static bool bWaterReflectionEnabled = true;

        static GameProcessor() { }

        public static void GenerateCameraMC(float cs = 3, float z = 0f)
        {
            cameraStepsDone = 0;
            cameraStepsNeedToDo = (int)(cs * 60);

            cameraFollowTarget = PlayerController.selectedSprite;
            zoomBeforeCameraChange = zoom;
            float distanceX = -(cameraFollowTarget.position.X + cameraFollowTarget.spriteGameSize.Width / 2 - 1366 / 2 / z) - sceneCamera.X;
            float distanceY = -(cameraFollowTarget.position.Y + cameraFollowTarget.spriteGameSize.Height / 2 - 768 / 2 / z) - sceneCamera.Y;



            cameraMoveAmount = new Vector2(distanceX / (cs * 60), distanceY / (cs * 60));

            if (z != 0f)
            {
                float zoomDiff = z - zoom;
                cameraZoomAmount = zoomDiff / (cs * 60);
            }
        }

        public static void GenerateCamera(BaseSprite cft, float cs = 3, float z = 0f)
        {
            cameraStepsDone = 0;
            cameraStepsNeedToDo = (int)(cs * 60);
            if (z == 0f) { z = 2.1f; }
            cameraFollowTarget = cft;
            zoomBeforeCameraChange = zoom;
            bCameraOnController = false;
            float distanceX = -(cft.position.X + cft.spriteGameSize.Width / 2 - 1366 / 2 / z) - sceneCamera.X;
            float distanceY = -(cft.position.Y + cft.spriteGameSize.Height / 2 - 768 / 2 / z) - sceneCamera.Y;


            if (cameraStepsNeedToDo == 0)
            {
                cameraStepsNeedToDo = 1;
            }

            cameraMoveAmount = new Vector2(distanceX / (float)(cameraStepsNeedToDo), distanceY / (float)(cameraStepsNeedToDo));
            if (Math.Abs(cameraMoveAmount.X) < 0.5f)
            {
                cameraMoveAmount.X = 0;
            }

            if (Math.Abs(cameraMoveAmount.Y) < 0.5f)
            {
                cameraMoveAmount.Y = 0;
            }

            if (z != 0f)
            {
                float zoomDiff = z - zoom;
                cameraZoomAmount = zoomDiff / (cs * 60);
            }
        }

        public static void GenerateCamera(Vector2 cft, float cs = 3, float z = 0f)
        {
            cameraStepsDone = 0;
            cameraStepsNeedToDo = (int)(cs * 60);
            if (z == 0f) { z = 2.1f; }
            cameraFollowTarget = null;
            zoomBeforeCameraChange = zoom;
            bCameraOnController = false;
            float distanceX = -(cft.X - 1366 / 2 / z) - sceneCamera.X;
            float distanceY = -(cft.Y - 768 / 2 / z) - sceneCamera.Y;


            if (cameraStepsNeedToDo == 0)
            {
                cameraStepsNeedToDo = 1;
            }

            cameraMoveAmount = new Vector2(distanceX / (float)(cameraStepsNeedToDo), distanceY / (float)(cameraStepsNeedToDo));
            if (Math.Abs(cameraMoveAmount.X) < 0.5f)
            {
                cameraMoveAmount.X = 0;
            }

            if (Math.Abs(cameraMoveAmount.Y) < 0.5f)
            {
                cameraMoveAmount.Y = 0;
            }

            if (z != 0f)
            {
                float zoomDiff = z - zoom;
                cameraZoomAmount = zoomDiff / (cs * 60);
            }
        }

        public static void GenerateCameraInstant(BaseSprite cft, float cs = 3, float z = 0f)
        {
            cameraStepsDone = 0;
            cameraStepsNeedToDo = (int)(cs * 60);
            if (z == 0f) { z = 2.1f; }
            cameraFollowTarget = cft;
            zoomBeforeCameraChange = zoom;
            bCameraOnController = false;
            float distanceX = -(cft.position.X + cft.spriteGameSize.Width / 2 - 1366 / 2 / z) - sceneCamera.X;
            float distanceY = -(cft.position.Y + cft.spriteGameSize.Height / 2 - 768 / 2 / z) - sceneCamera.Y;


            if (cameraStepsNeedToDo == 0)
            {
                cameraStepsNeedToDo = 1;
            }

            cameraMoveAmount = new Vector2(distanceX / (float)(cameraStepsNeedToDo), distanceY / (float)(cameraStepsNeedToDo));
            if (Math.Abs(cameraMoveAmount.X) < 0.5f)
            {
                cameraMoveAmount.X = 0;
            }

            if (Math.Abs(cameraMoveAmount.Y) < 0.5f)
            {
                cameraMoveAmount.Y = 0;
            }

            if (z != 0f)
            {
                float zoomDiff = z - zoom;
                cameraZoomAmount = zoomDiff / (cs * 60);
            }

            sceneCamera += new Vector2(distanceX, distanceY);
        }

        public static void GenerateCameraInstant(Vector2 cft, float cs = 3, float z = 0f)
        {
            cameraStepsDone = 0;
            cameraStepsNeedToDo = (int)(cs * 60);
            if (z == 0f) { z = 2.1f; }
            cameraFollowTarget = null;
            zoomBeforeCameraChange = zoom;
            bCameraOnController = false;
            float distanceX = -(cft.X - 1366 / 2 / z) - sceneCamera.X;
            float distanceY = -(cft.Y - 768 / 2 / z) - sceneCamera.Y;


            if (cameraStepsNeedToDo == 0)
            {
                cameraStepsNeedToDo = 1;
            }

            cameraMoveAmount = new Vector2(distanceX / (float)(cameraStepsNeedToDo), distanceY / (float)(cameraStepsNeedToDo));
            if (Math.Abs(cameraMoveAmount.X) < 0.5f)
            {
                cameraMoveAmount.X = 0;
            }

            if (Math.Abs(cameraMoveAmount.Y) < 0.5f)
            {
                cameraMoveAmount.Y = 0;
            }

            if (z != 0f)
            {
                float zoomDiff = z - zoom;
                cameraZoomAmount = zoomDiff / (cs * 60);
            }

            sceneCamera += new Vector2(distanceX, distanceY);
        }

        public static void ResetCamera()
        {
            bCameraOnController = false;
            zoom = zoomBeforeCameraChange;
            cameraFollowTarget = PlayerController.selectedSprite;
            zoom = zoomBeforeCameraChange;
        }

        public static void GenerateCameraLogic(int cameraSteps = 20)
        {

        }

        public static void LaunchForEditor(BasicMap newMap)
        {
            loadedMap = newMap;
            parentMap = MapBuilder.parentMap;
            parentMap.gcdb = MapBuilder.gcDB;
            parentMap.subMaps.ForEach(m => m.gcdb = gcDB);
            MapObjectHelpClass.objectsToUpdateOutsideOfMap.Clear();
            BasicMap.allMapsGame().ForEach(map => MapObjectHelpClass.objectsToUpdateOutsideOfMap.AddRange(map.mapNPCs));
            if (ScriptProcessor.HasPCController())
            {
                BattleGUI.InitializeResources();
                GameScreenEffect.InitializeResources();
                psData = new PlayerSaveData();
                BattleGUI.InitializeResources();
                GamePlayUtility.Initiate(DateTime.Now.Millisecond + (int)(DateTime.Now.Millisecond * 3.3f) + (int)(DateTime.Now.Millisecond * 38.224f) + DateTime.Now.Second + DateTime.Now.Hour + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year);
                bIsInGame = true;
                MapBuilder.bPlayTest = true;


                mainController = ScriptProcessor.mapHero;
                PlayerController.DirectControlSprite(mainController);
                gameRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
                shadowRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

                sceneCamera = new Vector2(mainController.position.X - 1366 / 2, mainController.position.Y - 768 / 2);
                EnableNonCombatStage();
                SaveDataProcessor.Launch();
                ResetCamera();
                DayLightHandler.GenerateDefaultLight();
                MapEditor.bIsRunning = false;
            }
            else
            {
                MapBuilder.bPlayTest = false;
            }
        }

        public static void Launch()
        {
            if (SettingsMenu.bIsRunning) { SettingsMenu.Close(); }
            ResetCamera();
            EnableNonCombatStage();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var game = EditorFileWriter.GameMapReader(Path.Combine(Environment.CurrentDirectory, @"TBAGW\Maps\StartMap\Short.cgmapc"));
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Time elapsed : " + elapsedMs);


            BasicMap newMap = game.Key;
            gcDB = game.Value;
            newMap.gcdb = gcDB;
            newMap.subMaps.ForEach(m => m.gcdb = gcDB);
            MapObjectHelpClass.objectsToUpdateOutsideOfMap.Clear();
            BasicMap.allMapsGame().ForEach(map => MapObjectHelpClass.objectsToUpdateOutsideOfMap.AddRange(map.mapNPCs));

            if (ScriptProcessor.HasPCController(newMap))
            {

                ScriptProcessor.gameMap = newMap;
                BattleGUI.InitializeResources();
                GameScreenEffect.InitializeResources();
                psData = new PlayerSaveData();
                BattleGUI.InitializeResources();
                GamePlayUtility.Initiate(DateTime.Now.Millisecond + (int)(DateTime.Now.Millisecond * 3.3f) + (int)(DateTime.Now.Millisecond * 38.224f) + DateTime.Now.Second + DateTime.Now.Hour + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year);
                bIsInGame = true;
                MapBuilder.bPlayTest = true;
                loadedMap = newMap;
                parentMap = newMap;
                mainController = ScriptProcessor.mapHero;
                PlayerController.DirectControlSprite(mainController);
                gameRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
                shadowRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

                sceneCamera = new Vector2(mainController.position.X - 1366 / 2, mainController.position.Y - 768 / 2);
                EnableNonCombatStage();
                SaveDataProcessor.Launch();
                ResetCamera();
                //  MapBuilder.bPlayTest = true;
                DayLightHandler.GenerateDefaultLight();
                MapEditor.bIsRunning = false;
            }
            else
            {
                MapBuilder.bPlayTest = false;
            }

            loadedMap.ForceCheckChunksToConsider();
        }

        public static void LaunchFromSaveFile(PlayerSaveData playerSave, String root)
        {
            if (SettingsMenu.bIsRunning) { SettingsMenu.Close(); }
            var game = EditorFileWriter.GameMapReader(Path.Combine(root, playerSave.mapLoc));
            BasicMap newMap = game.Key;
            gcDB = game.Value;
            newMap.gcdb = gcDB;
            newMap.subMaps.ForEach(m => m.gcdb = gcDB);
            MapObjectHelpClass.objectsToUpdateOutsideOfMap.Clear();
            BasicMap.allMapsGame().ForEach(map => MapObjectHelpClass.objectsToUpdateOutsideOfMap.AddRange(map.mapNPCs));


            ScriptProcessor.gameMap = newMap;
            BattleGUI.InitializeResources();
            GameScreenEffect.InitializeResources();
            psData = new PlayerSaveData();
            BattleGUI.InitializeResources();
            GamePlayUtility.Initiate(DateTime.Now.Millisecond + (int)(DateTime.Now.Millisecond * 3.3f) + (int)(DateTime.Now.Millisecond * 38.224f) + DateTime.Now.Second + DateTime.Now.Hour + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year);
            bIsInGame = true;
            MapBuilder.bPlayTest = true;
            loadedMap = newMap;
            parentMap = newMap;
            SaveDataProcessor.HandleLoad(playerSave);
            mainController = PlayerController.selectedSprite;
            PlayerController.DirectControlSprite(mainController);
            gameRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            shadowRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

            sceneCamera = new Vector2(mainController.position.X - 1366 / 2, mainController.position.Y - 768 / 2);
            EnableNonCombatStage();
            SaveDataProcessor.Launch();
            ResetCamera();
            //  MapBuilder.bPlayTest = true;
            DayLightHandler.GenerateDefaultLight();
            MapEditor.bIsRunning = false;


            loadedMap.ForceCheckChunksToConsider();
        }

        public static void ChangeSubMap(BasicMap newMap, BaseSprite bs)
        {
            loadedMap = newMap;
            ChangePlayerLoc(bs.spriteGameSize.Location.ToVector2());
            loadedMap.ForceCheckChunksToConsider();
            loadedMap.getMapSaveDataReady();
        }

        public static void ChangePlayerLoc(Vector2 newLOC)
        {
            int tileX = (int)(newLOC.X / 64);
            int tileY = (int)(newLOC.Y / 64);
            Vector2 properLoc = new Vector2(tileX * 64, tileY * 64);
            PlayerController.selectedSprite.position = newLOC;
            PlayerController.selectedSprite.UpdatePosition();
        }

        #region GAME STAGE
        static public void EnableSoloCombatStage()
        {
            bInSoloCombat = true;
            bInCombat = false;
            bInGameMenu = false;
            bInDialogue = false;
            bInNonCombat = false;
        }

        static public void EnableCombatStage()
        {
            bInSoloCombat = false;
            bInCombat = true;
            bInGameMenu = false;
            bInDialogue = false;
            bInNonCombat = false;
            PlayerController.currentController = PlayerController.Controllers.Combat;
        }

        static public void EnableNonCombatStage()
        {
            bInSoloCombat = false;
            bInCombat = false;
            bInGameMenu = false;
            bInDialogue = false;
            bInNonCombat = true;
            PlayerController.currentController = PlayerController.Controllers.NonCombat;
        }

        static public void EnableMenuStage()
        {
            bInSoloCombat = false;
            bInGameMenu = true;
            bInDialogue = false;
            PlayerController.currentController = PlayerController.Controllers.Menu;
        }

        static public void DisableMenuStage()
        {
            bInSoloCombat = false;
            bInGameMenu = false;
            bInDialogue = false;
            if (bInCombat)
            {
                PlayerController.currentController = PlayerController.Controllers.Combat;
            }
            else if (bInNonCombat)
            {
                PlayerController.currentController = PlayerController.Controllers.NonCombat;
            }
        }

        static public void EnableDialogueStage()
        {
            bInCombat = false;
            bInGameMenu = false;
            bInDialogue = true;
            bInNonCombat = false;
            PlayerController.currentController = PlayerController.Controllers.Dialogue;
        }
        #endregion

        static int timer = 200;
        static float preBattleTimer = 1.0f * 60;
        static int preBattleTimePassed = 0;
        static MapRegion previousRegion = default(MapRegion);
        static MapZone previousZone = default(MapZone);
        public static void Update(GameTime gameTime)
        {
            PlayerController.Check();
            if (SettingsMenu.bIsRunning)
            {
                SettingsMenu.Update(gameTime);
                return;
            }
            if (OptionsMenu.bIsRunning)
            {
                OptionsMenu.Update(gameTime);
                return;
            }

            SoundEffectSong.Update(gameTime);
            if (!CombatProcessor.bIsRunning)
            {
                zoom = 2.1f;
            }
            SoundEffectSong.Update(gameTime);
            EnvironmentHandler.Update(gameTime);
            // DayLightHandler.bPaused = false;
            //BaseScript sc = new BaseScript();
            //sc.scriptContent.Add("aaaaaaaas");
            //ScriptProcessor.ChangeActiveScript(sc);
            GameScreenEffect.Update(gameTime, sceneCamera);
            //grassModifierTimePassed += gameTime.ElapsedGameTime.Milliseconds;
            //foreach (var item in PlayerSaveData.heroParty)
            //{
            //    item.statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] = -20;
            //}

            if (ScriptProcessor.backgroundScripts.Count != 0)
            {
                ScriptProcessor.UpdateBGScript(gameTime);
            }

            if (PlayerController.selectedSprite != null)
            {
                MapRegion currentRegion = loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite));
                if (currentRegion != default(MapRegion) && currentRegion != previousRegion)
                {

                    previousRegion = currentRegion;
                    currentRegion.GetRegionReady();
                    GameScreenEffect.InitializeRegionName(currentRegion.regionName);
                }
                if (currentRegion != null)
                {
                    MapZone currentZone = currentRegion.getZone(PlayerController.selectedSprite);
                    if (currentZone != default(MapZone) && currentZone != previousZone)
                    {
                        if (currentZone.HasGoodEntranceScript())
                        {
                            ScriptProcessor.ChangeActiveScript(currentZone.entranceScript, null, PlayerController.selectedSprite, true);
                            previousZone = currentZone;
                        }
                    }
                }

            }

            if (bStartCombatZoom && !LUA.LuaExecutionList.DemandOverride())
            {

                if (GameScreenEffect.CombatZoomDone())
                {
                    preBattleTimePassed++;
                }

                if (preBattleTimePassed > preBattleTimer)
                {
                    bStartCombatZoom = false;
                    bStartBattle = true;
                    StartBattleSoon(battleCaster, battleTarget, battleCastIndex);
                }
            }

            if (BattleGUI.bBGUISoloModifierRunning)
            {
                BattleGUI.CharacterSoloUpdate(gameTime);
                if (bInGameMenu)
                {
                    PlayerController.currentController = PlayerController.Controllers.SoloCombat;
                    PlayerController.Update(gameTime);
                }
            }

            foreach (var item in popUpRenders)
            {
                item.Update(gameTime);
            }


            gcDB.gameSourceTiles.Find(ts => ts.tileID == 11).tileAnimation.frameInterval = 150;
            gcDB.gameSourceTiles.Find(ts => ts.tileID == 11).Update(gameTime);

            //GameProcessor.loadedMap.mapNPCs[0].ChangeCommandEdit(0);
            //if (Keyboard.GetState().IsKeyDown(Keys.NumPad7) && !KeyboardMouseUtility.AnyButtonsPressed())
            //{
            //}

            UIElements.ForEach(UIel => UIel.Update(gameTime));
            UIElements.RemoveAll(UIel => UIel.IsDoneRemove());

            //if (Keyboard.GetState().IsKeyDown(Keys.R) && !KeyboardMouseUtility.AnyButtonsPressed())
            //{
            //    //loadedMap.til
            //}

            if (Keyboard.GetState().IsKeyDown(Keys.Y) && !KeyboardMouseUtility.AnyButtonsPressed() && !CombatProcessor.bIsRunning)
            {
                PlayerSaveData.playerID = 1337;
                // PlayerSaveData.heroParty;
                SaveDataProcessor.AttemptSave();

                UIElements.Clear();

                //TextElement saveText = new TextElement();
                //saveText.SetTimer(2000);
                //saveText.textColour = Color.Gold;
                //saveText.textToDisplay = "Saved";
                //saveText.elementFadeOut = 2000;
                //UIElements.Add(saveText);

                //TextElement timerText = new TextElement();
                //timerText.SetTimer((0 * 60 + 5 * 60 + 0 * 60) * 1000);
                //timerText.displayType = TextElement.DisplayTypes.TimePassed;
                //timerText.textColour = Color.Gold;
                //timerText.elementFadeOut = 2000;
                //timerText.drawPos = new Vector2(100, 300);
                //UIElements.Add(timerText);

                //TextElement cdText = new TextElement();
                //cdText.SetTimer((0 * 60 + 5 * 60 + 0 * 60) * 1000);
                //cdText.displayType = TextElement.DisplayTypes.RemainingTime;
                //cdText.textColour = Color.Gold;
                //cdText.elementFadeOut = 2000;
                //cdText.drawPos = new Vector2(100, 450);
                //UIElements.Add(cdText);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad5) && Game1.bIsDebug && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                BasicTile tempTile = new BasicTile();
                tempTile.tileSource = gcDB.gameSourceTiles.Find(ts => ts.tileID == 11);
                tempTile.tsID = 11;
                Vector2 coords = new Vector2(((int)PlayerController.selectedSprite.position.X / 64), ((int)PlayerController.selectedSprite.position.Y / 64));
                loadedMap.TryToChangeTile(coords, tempTile, 0);

            }

            if (Keyboard.GetState().IsKeyDown(Keys.T) && Game1.bIsDebug && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                SaveDataProcessor.HandleLoad(EditorFileWriter.SaveFileReader(1));

                UIElements.Clear();

                TextElement saveText = new TextElement();
                saveText.SetTimer(2000);
                saveText.textColour = Color.Gold;
                saveText.textToDisplay = "Loaded";
                saveText.elementFadeOut = 2000;
                UIElements.Add(saveText);

            }

            //if (Keyboard.GetState().IsKeyDown(Keys.I) && !KeyboardMouseUtility.AnyButtonsPressed())
            //{

            //    //BaseScript tempScript = new BaseScript();
            //    //tempScript.scriptContent.Add("50% Chance to fail/succeed");
            //    //tempScript.scriptContent.Add("@TRC_50_1");
            //    //tempScript.scriptContent.Add("You succeed!");
            //    //tempScript.scriptContent.Add("@TGO_9");
            //    //tempScript.scriptContent.Add("@TRF");
            //    //tempScript.scriptContent.Add("DOES IT?");
            //    //tempScript.scriptContent.Add("@TRF_1");
            //    //tempScript.scriptContent.Add("You failed!");
            //    //tempScript.scriptContent.Add("@TGO_9");
            //    //tempScript.scriptContent.Add("@THE_9");
            //    //tempScript.scriptContent.Add("It's that easy, yay!");

            //    //ScriptProcessor.ChangeActiveScript(tempScript);
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.U) && !KeyboardMouseUtility.AnyButtonsPressed())
            //{


            //    BaseScript tempScript = new BaseScript();
            //    String colorCoding = "_" + Color.OrangeRed.R + "," + Color.OrangeRed.G + "," + Color.OrangeRed.B;

            //    tempScript.scriptContent.Add("@CWB_50_2");
            //    tempScript.scriptContent.Add("@GMP_6,8_3");
            //    tempScript.scriptContent.Add("@PSS_0_1.5f_5,8" + colorCoding);

            //    // ScriptProcessor.ChangeActiveScript(tempScript);
            //}

            //if (Keyboard.GetState().IsKeyDown(Keys.O) && !KeyboardMouseUtility.AnyButtonsPressed())
            //{
            //    //GameScreenEffect.TurnOnWeather(0);
            //    GameScreenEffect.TurnOnWeather(0);
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.N) && !KeyboardMouseUtility.AnyButtonsPressed())
            //{
            //}

            // GenerateCamera(cameraFollowTarget, .06f, zoom);

            SoundController.Update();


            if (Keyboard.GetState().IsKeyDown(Keys.V) && Game1.bIsDebug && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                var t = PlayerSaveData.heroParty;

                PlayerSaveData.playerInventory.localInventory.Clear();
                for (int i = 0; i < 32; i++)
                {
                    int random = GamePlayUtility.Randomize(0, gcDB.gameItems.Count);
                    PlayerSaveData.playerInventory.localInventory.Add((gcDB.gameItems[random]).Clone());
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.B) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                PlayerSaveData.playerInventory.ManageStackableItems();
            }


            if (!BattleGUI.bIsRunning && !bGameUpdateIsPaused && !bInGameMenu && !LUA.LuaExecutionList.DemandOverride())
            {


                DayLightHandler.Update(gameTime);


                #region normal gameprocessor
                // loadedMap.mapSprites[0].currentHitBox.Clear();

                EditorCursorPos = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale / zoom + new Vector2(-sceneCamera.X, -sceneCamera.Y);
                KeyboardMouseUtility.AssignGameMousePos(EditorCursorPos);

                //int mapTilePosX = (int)(EditorCursorPos.X / 64);
                //if (EditorCursorPos.X < 0)
                //{
                //    mapTilePosX = 0;
                //}
                //int mapTilePosY = (int)(EditorCursorPos.Y / 64);
                //if (EditorCursorPos.Y < 0)
                //{
                //    mapTilePosY = 0;
                //}
                //selectedGridPos = new Vector2(mapTilePosX, mapTilePosY);
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Game1.bIsDebug)
                {
                    LUA.LuaDangerTile ldt = new LUA.LuaDangerTile(LUA.LuaHelp.RandomMapPosition());
                    DangerTile dt = ldt.TryConvert();
                    EncounterInfo.dangerTiles.Add(dt);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Game1.bIsDebug && !KeyboardMouseUtility.AnyButtonsPressed() && bInNonCombat)
                {

                    if (loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)) != null && loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)).getZone(PlayerController.selectedSprite) != null && loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)).getZone(PlayerController.selectedSprite).Contains(PlayerController.selectedSprite.positionToMapCoords() * 64))
                    {
                        int amountOfChars = PlayerSaveData.heroParty.Count() + loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)).getZone(PlayerController.selectedSprite).zoneEncounterInfo.packSizeMax;
                        var tempZone = loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)).getZone(PlayerController.selectedSprite);
                        if (amountOfChars < loadedMap.possibleTilesGameZone(tempZone.returnBoundingZone()).Count)
                        {
                            Console.WriteLine("Left");
                            EnableCombatStage();
                            CombatProcessor.Initialize(PlayerController.selectedSprite, loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)));
                            mainControllerBeforeCombat = PlayerController.selectedSprite;
                        }
                        else
                        {
                            Console.WriteLine("Error, combatzone not large enough. Skipping fight.");
                        }

                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.F) && Game1.bIsDebug && !KeyboardMouseUtility.AnyButtonsPressed() && bInNonCombat && false)
                {
                    //ScriptProcessor.bIsRunning
                    // ResolutionUtility.toggleFullscreen();
                    BaseScript tempScript = new BaseScript();
                    //tempScript.scriptContent.Add("@TPT_4,20");
                    //tempScript.scriptContent.Add("@CWB_100_0");
                    //GameScreenEffect.worldColor = 1f;
                    tempScript.scriptContent.Add("@CWB_100_0");
                    tempScript.scriptContent.Add("@CWB_60_5");
                    tempScript.scriptContent.Add("@TPT_-28,16_0");
                    tempScript.scriptContent.Add("@PSE_6");
                    tempScript.scriptContent.Add("@GMP_-28,8_0");
                    tempScript.scriptContent.Add("@GMP_-27,8_1");
                    tempScript.scriptContent.Add("@PSE_5");
                    String colorCoding = "_" + Color.WhiteSmoke.R + "," + Color.WhiteSmoke.G + "," + Color.WhiteSmoke.B;
                    tempScript.scriptContent.Add("@PSS_0_4_-26,8" + colorCoding);

                    tempScript.scriptContent.Add("@GMP_-29,8_3");

                    tempScript.scriptContent.Add("@PSS_0_4_-30,8" + colorCoding);

                    tempScript.scriptContent.Add("@GMP_-28,8_0");
                    tempScript.scriptContent.Add("@GMP_-28,7_0");
                    //@MSG_SpeakerID_charLeftID_charRightID_charLeftExp_CharRightExp_conv
                    tempScript.scriptContent.Add("@PSE_10");
                    tempScript.scriptContent.Add("@MSG_0_0_-1_0_0_A chest! Finally something worth my time.");
                    tempScript.scriptContent.Add("@PSE_7");

                    tempScript.scriptContent.Add("@MSG_6_0_-1_3_0_Begone!");
                    tempScript.scriptContent.Add("@PSE_9");
                    //gcDB
                    tempScript.scriptContent.Add("@PMW_true_6_1_0_-28,11_-28,10,-28,9,-28,8,-28,7_100_true");
                    tempScript.scriptContent.Add("@COS_false_0_0_6_true");
                    tempScript.scriptContent.Add("@PMW_true_6_3_1_-29,10_-29,9,-29,8,-29,7,-28,7_100_true");
                    tempScript.scriptContent.Add("@COS_false_1_0_6_true");
                    tempScript.scriptContent.Add("@PMW_true_6_2_2_-27,10_-27,9,-27,8,-27,7,-28,7_100_true");
                    tempScript.scriptContent.Add("@COS_false_2_0_6_false");
                    tempScript.scriptContent.Add("@PSE_9");
                    tempScript.scriptContent.Add("@PSE_8");
                    // GameProcessor.gcDB.gameSFXs
                    tempScript.scriptContent.Add("@TPT_-46,19_3");
                    tempScript.scriptContent.Add("@CWB_100_0");
                    tempScript.scriptContent.Add("@PSE_8");
                    tempScript.scriptContent.Add("@PSS_0_4_-46,19" + colorCoding);

                    tempScript.scriptContent.Add("@PMW_true_6_1_4_-51,18_-51,18_100_true");
                    tempScript.scriptContent.Add("@COS_false_4_0_3_true");
                    tempScript.scriptContent.Add("@PMW_true_6_1_5_-51,19_-51,19_100_true");
                    tempScript.scriptContent.Add("@COS_false_5_0_3_true");
                    tempScript.scriptContent.Add("@PMW_true_6_1_6_-51,20_-51,20_100_true");
                    tempScript.scriptContent.Add("@COS_false_6_0_3_true");

                    tempScript.scriptContent.Add("@PMW_true_6_3_7_-45,18_-45,18_100_true");
                    tempScript.scriptContent.Add("@COS_false_7_0_3_true");
                    tempScript.scriptContent.Add("@PMW_true_6_3_8_-45,19_-45,19_100_true");
                    tempScript.scriptContent.Add("@COS_false_8_0_3_true");
                    tempScript.scriptContent.Add("@PMW_true_6_3_9_-45,20_-45,20_100_true");
                    tempScript.scriptContent.Add("@COS_false_9_0_3_true");

                    tempScript.scriptContent.Add("@PMW_true_6_2_10_-49,16_-49,16_100_true");
                    tempScript.scriptContent.Add("@COS_false_10_0_3_true");
                    tempScript.scriptContent.Add("@PMW_true_6_2_11_-48,16_-48,16_100_true");
                    tempScript.scriptContent.Add("@COS_false_11_0_3_true");
                    tempScript.scriptContent.Add("@PMW_true_6_2_12_-47,16_-47,16_100_true");
                    tempScript.scriptContent.Add("@COS_false_12_0_3_true");

                    tempScript.scriptContent.Add("@PMW_true_6_0_13_-49,22_-49,22_100_true");
                    tempScript.scriptContent.Add("@COS_false_13_0_10_true");
                    tempScript.scriptContent.Add("@PMW_true_6_0_14_-48,22_-48,22_100_true");
                    tempScript.scriptContent.Add("@COS_false_14_0_10_true");
                    tempScript.scriptContent.Add("@PMW_true_6_0_15_-47,22_-47,22_100_true");
                    tempScript.scriptContent.Add("@COS_false_15_0_10_false");
                    tempScript.scriptContent.Add("@GMP_-48,19_3");
                    tempScript.scriptContent.Add("@PSE_0");
                    tempScript.scriptContent.Add("@MSG_6_0_-1_3_0_You have been warned!");
                    tempScript.scriptContent.Add("@CWB_80_2");
                    //tempScript.scriptContent.Add("@GMP_6,20_2");
                    //tempScript.scriptContent.Add("@PMW_true_6_1_0_1,20_2,20,3,20,4,20,5,20_100_true");
                    //tempScript.scriptContent.Add("@COS_false_0_0_6_true");
                    //tempScript.scriptContent.Add("@PMW_true_6_3_1_6,25_6,24,6,23,6,22,6,21_100_true");
                    //tempScript.scriptContent.Add("@COS_false_1_0_6_true");
                    //tempScript.scriptContent.Add("@PMW_true_6_2_2_11,20_10,20,9,20,8,20,7,20_100_true");
                    //tempScript.scriptContent.Add("@COS_false_2_0_6_false");
                    //tempScript.scriptContent.Add("@CWB_0_0");
                    //tempScript.scriptContent.Add("@TPT_6,25");
                    //tempScript.scriptContent.Add("@CWB_100_4");
                    // testNPC.baseCharacter.script.scriptContent.Insert(1, "@COS_false_0_100_1");
                    //testNPC.baseCharacter.script.scriptContent.Insert(1, "@PMW_true_3_0_0_8,15_8,16,8,17,8,18,8,19_0_true");

                    //ScriptProcessor.ChangeActiveScript(tempScript);
                }

                //if (Keyboard.GetState().IsKeyDown(Keys.P) && !KeyboardMouseUtility.AnyButtonsPressed())
                //{
                //    if ((!bInGameMenu && !CombatProcessor.bMainCombat && EncounterInfo.encounterGroups.Count == 0) || !bInGameMenu && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsPlayerTurnSet)
                //    {
                //        bInGameMenu = true;
                //        GameMenuHandler.Start();
                //        KeyboardMouseUtility.bPressed = true;
                //    }

                //}

                if (Keyboard.GetState().IsKeyDown(Keys.Right) && Keyboard.GetState().IsKeyDown(Keys.LeftShift) && Game1.bIsDebug && !KeyboardMouseUtility.AnyButtonsPressed() && bInCombat)
                {
                    CombatProcessor.InitiateVictory();
                    //CombatProcessor.Reset();
                    ////encounterEnemies.Clear();
                    //EncounterInfo.encounterGroups.Clear();
                    //EnableNonCombatStage();
                    //zoom = 2.1f;
                    //CombatProcessor.bMainCombat = false;
                    //if (PlayerController.selectedSprite == null)
                    //{
                    //    if ((mainControllerBeforeCombat is BaseCharacter) && (mainControllerBeforeCombat as BaseCharacter).IsAlive())
                    //    {
                    //        PlayerController.selectedSprite = mainControllerBeforeCombat;
                    //    }
                    //    else
                    //    {
                    //        PlayerController.selectedSprite = PlayerSaveData.heroParty[GamePlayUtility.Randomize(0, PlayerSaveData.heroParty.Count - 1)];
                    //    }
                    //}
                }



                if (bInCombat && !LUA.LuaExecutionList.DemandOverride())
                {
                    if (!CombatProcessor.bLostBattle)
                    {
                        if (!CombatProcessor.bAfterBattleScreen)
                        {
                            CombatProcessor.Update(gameTime);
                        }
                        else if (CombatProcessor.bAfterBattleScreen)
                        {
                            CombatProcessor.UpdateFinalScreen(gameTime);
                        }

                    }
                    else
                    {
                        CombatProcessor.UpdateBattleLost(gameTime);
                    }

                }


                if (cameraFollowTarget != null && !bStartCombatZoom)
                {

                    GenerateCamera(cameraFollowTarget, .06f, zoom);
                    if (cameraStepsDone < cameraStepsNeedToDo && cameraMoveAmount != Vector2.Zero)
                    {
                        cameraStepsDone++;
                        sceneCamera += cameraMoveAmount;
                        zoom += cameraZoomAmount;
                        CameraScaleMatrix = Matrix.CreateTranslation(new Vector3(sceneCamera.X, sceneCamera.Y, 1)) * Matrix.CreateScale(new Vector3(zoom, zoom, 1));
                        //bUpdateShadows = true;
                    }
                    else if (bUpdateOnceMore && Vector2.Zero == cameraMoveAmount)
                    {
                        bUpdateOnceMore = false;
                        //bUpdateShadows = true;

                    }

                }

                PlayerController.Update(gameTime);

                //if (!bInCombat)
                //{

                //}


                if (bInCombat)
                {
                    cameraPosition = new Point();
                    // KeyValuePair<Rectangle, float> camerInfo = CombatProcessor.zone.returnOptimalCameraAndAngle();
                    // zoom = camerInfo.Value;
                    // sceneCamera.Location = camerInfo.Key.Location;
                }

                var test = new Vector2(-sceneCamera.X, sceneCamera.Y);
                loadedMap.Update(gameTime, test);

                sceneCamera = sceneCamera.ToPoint().ToVector2();
                CameraScaleMatrix = Matrix.CreateTranslation(new Vector3(sceneCamera.X, sceneCamera.Y, 1)) * Matrix.CreateScale(new Vector3(zoom, zoom, 1));
                CameraScaleMatrix.M41 = (int)CameraScaleMatrix.M41;
                CameraScaleMatrix.M42 = (int)CameraScaleMatrix.M42;
                CameraScaleMatrix.M43 = (int)CameraScaleMatrix.M43;
                #endregion
            }
            else if (BattleGUI.bIsRunning)
            {
                #region BGUI processor
                PlayerController.Update(gameTime);
                BattleGUI.UpdateBattleLogic(gameTime);
                #endregion
            }
            else if (bGameUpdateIsPaused)
            {

            }

            if (ScriptProcessor.bIsRunning)
            {
                // PlayerController.Update(gameTime);
                ScriptProcessor.Update(gameTime);
                loadedMap.MinimalUpdate(gameTime, sceneCamera);
            }
            else if (LUA.LuaExecutionList.DemandOverride())
            {
                PlayerController.Update(gameTime);
                LUA.LuaExecutionList.Update(gameTime);
            }

            if (bInGameMenu)
            {
                var beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;
                GameMenuHandler.Update(gameTime, beep);

                //if (Keyboard.GetState().IsKeyDown(Keys.P) && !KeyboardMouseUtility.AnyButtonsPressed())
                //{
                //    DisableMenuStage();
                //}
            }

            if (GameScreenEffect.bWeatherEffect)
            {
                //GameScreenEffect.WeatherEffect.spawnPosition = (-sceneCamera.Location.ToVector2() - new Vector2(500)).ToPoint();
                GameScreenEffect.WeatherEffect.spawnPosition = (-sceneCamera + new Vector2(-2000, -800)).ToPoint();
                GameScreenEffect.WeatherEffect.Update(gameTime);
            }
            sceneCamera = sceneCamera.ToPoint().ToVector2();
            CameraScaleMatrixNoZooming = Matrix.CreateTranslation(new Vector3(sceneCamera.X, sceneCamera.Y, 1));

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8) && Game1.bIsDebug && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                if (popUpRenders.OfType<PickUpScreen>().Count() != 0)
                {
                    popUpRenders.Clear();
                    NonCombatCtrl.currentControls = NonCombatCtrl.ControlSetup.Standard;
                }
                else
                {
                    loadedMap.mapSaveInfo.mapPUEntities.Add(new PickUpEntity(PlayerSaveData.playerInventory.localInventory, (PlayerController.selectedSprite as BaseCharacter).positionToMapCoords().ToPoint()));
                }

            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9) && Game1.bIsDebug && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                GameScreenEffect.Reset();
            }


        }

        public static RenderTarget2D mapRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        public static RenderTarget2D activeElementLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        public static RenderTarget2D finalLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static int lining = 2;
        static Color textColor = Color.White;
        static Color textLiningColor = Color.Black;

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (SettingsMenu.bIsRunning)
            {
                SettingsMenu.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);


                spriteBatch.Draw(SettingsMenu.getRender(), new Rectangle(0, 0, 1366, 768), Color.White);

                spriteBatch.End();
                return;
            }

            if (OptionsMenu.bIsRunning)
            {
                OptionsMenu.Draw(spriteBatch);
                spriteBatch.End();
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);


                spriteBatch.Draw(OptionsMenu.getRender(), new Rectangle(0, 0, 1366, 768), Color.White);

                spriteBatch.End();
                return;
            }

            //DayLightHandler.bPaused = false;
            foreach (var item in popUpRenders)
            {
                item.Generate(spriteBatch);
            }

            foreach (var item in loadedMap.testBuildings.FindAll(b => b.Contains(new Rectangle((int)-sceneCamera.X, (int)-sceneCamera.Y, 1366, 768), zoom) && b.reGenerate))
            {
                item.Draw(spriteBatch);
            }

            if (LUA.LuaExecutionList.DemandOverride())
            {
                LUA.LuaExecutionList.Draw(spriteBatch, null);
            }

            TestEnvironment.GenerateRainRender(spriteBatch);

            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(mapRender);
            #region MAP DRAWING AND OVERLAY
            spriteBatch.GraphicsDevice.Clear(Color.Black);
            if (bStartCombatZoom)
            {

            }

            //Overworld?
            if (!BattleGUI.bIsRunning && !BattleGUI.bBGUISoloModifierRunning && !bInGameMenu)
            {

                #region

                if (bInCombat && !bInGameMenu)
                {
                    //DONE
                    if (!CombatProcessor.bAfterBattleScreen)
                    {
                        CombatProcessor.GenerateCombatGroundRender(spriteBatch);
                    }
                }



                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);
                //OVERWORLD DRAW USE OW RENDER         

                loadedMap.DrawMap(spriteBatch, gcDB, mapRender, zoom);

                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(lastDrawnMapRender);
                spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                spriteBatch.Draw(mapRender, new Rectangle(0, 0, 1366, 768), Color.White);
                spriteBatch.End();

                spriteBatch.GraphicsDevice.SetRenderTarget(gameRender);
                spriteBatch.GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                if (!CombatProcessor.bIsRunning)
                {
                    spriteBatch.Draw(mapRender, mapRender.Bounds, Color.White);
                }
                GameScreenEffect.GenerateOpacitySteps(100, 0);


                if (bInCombat && !bInGameMenu)
                {
                    //DONE
                    if (!CombatProcessor.bLostBattle)
                    {
                        if (!CombatProcessor.bAfterBattleScreen)
                        {
                            CombatProcessor.Draw(spriteBatch);
                        }
                        else
                        {
                            CombatProcessor.DrawFinalScreen(spriteBatch);
                        }
                    }
                    else
                    {
                        CombatProcessor.DrawLostBattle(spriteBatch);
                    }


                }

                //OVERWORLD DRAW USE OW RENDER  
                if (GameScreenEffect.bWeatherEffect)
                {
                    GameScreenEffect.WeatherEffect.Draw(spriteBatch);
                }

                spriteBatch.End();




                #endregion
            } //Battle
            else if (BattleGUI.BattleGUITexture != null && BattleGUI.bIsRunning)
            {
                BattleGUI.bDrawFrames = false;
                //UI RENDER
                BattleGUI.QuickDraw(spriteBatch);
                //END UI RENDER
            }

            if (BattleGUI.bBGUISoloModifierRunning)
            {
                BattleGUI.CharacterSoloDraw(spriteBatch);
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            UIElements.ForEach(UIel => UIel.Draw(spriteBatch));

            String timeString = "";
            int hour = DayLightHandler.timeIndex / 60;
            int minutes = DayLightHandler.timeIndex - hour * 60;
            if (hour < 10)
            {
                timeString += "0" + hour + ":";
            }
            else
            {
                timeString += hour + ":";
            }

            if (minutes < 10)
            {
                timeString += "0" + minutes;
            }
            else
            {
                timeString += minutes;
            }

            timeString += "\n";

            switch (DayLightHandler.currentTimeBlock.timeBlockName)
            {
                case DayLightHandler.TimeBlocksNames.pm12am4:
                    timeString += "Late night";
                    break;
                case DayLightHandler.TimeBlocksNames.am4am7:
                    timeString += "Early morning";
                    break;
                case DayLightHandler.TimeBlocksNames.am7am12:
                    timeString += "Morning / Before noon";
                    break;
                case DayLightHandler.TimeBlocksNames.am12am13:
                    timeString += "Noon";
                    break;
                case DayLightHandler.TimeBlocksNames.am13am14:
                    timeString += "Past noon";
                    break;
                case DayLightHandler.TimeBlocksNames.am14am19:
                    timeString += "Afternoon";
                    break;
                case DayLightHandler.TimeBlocksNames.am19am22:
                    timeString += "Evening";
                    break;
                case DayLightHandler.TimeBlocksNames.am22pm12:
                    timeString += "Late evening";
                    break;
                default:
                    break;
            }

            if (!DayLightHandler.bPaused)
            {
                spriteBatch.DrawString(Game1.defaultFont, timeString, new Vector2(50, 100), Color.Gold, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
            }
            //spriteBatch.Draw(gameRender, gameRender.Bounds, Color.White);
            //spriteBatch.Draw(groundRender, groundRender.Bounds, Color.White);
            //spriteBatch.Draw(objectRender, objectRender.Bounds, Color.White);
            spriteBatch.End();

            if (bInGameMenu && !BattleGUI.bBGUISoloModifierRunning)
            {
                spriteBatch.GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                //UI RENDER
                UIRenders.Add(GameMenuHandler.Draw(spriteBatch));

                //END UI RENDER
                spriteBatch.End();
            }


            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrixNoZooming);
            var beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale + new Vector2(-sceneCamera.X, -sceneCamera.Y);
            //UI RENDER
            BattleGUI.DrawCursor(spriteBatch, beep, zoom);

            //END UI RENDER
            spriteBatch.End();

            //UI RENDER
            foreach (var item in BattleGUI.TurnEffectsList)
            {
                if (item.bMustShow)
                {
                    item.Draw(spriteBatch);
                }
            }
            //END UI RENDER

            #endregion

            #region active element layer




            List<RenderTarget2D> elements = new List<RenderTarget2D>();
            elements.Add(NiceText.DrawAll(spriteBatch));


            spriteBatch.GraphicsDevice.SetRenderTarget(activeElementLayer);
            spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            if (!bInGameMenu || !BattleGUI.bBGUISoloModifierRunning)
            {
                spriteBatch.Draw(gameRender, gameRender.Bounds, Color.White);
            }
            //

            foreach (var item in elements)
            {
                spriteBatch.Draw(item, item.Bounds, Color.White);
            }

            spriteBatch.End();

            #endregion

            #region FINAL layer
            spriteBatch.GraphicsDevice.SetRenderTarget(finalLayer);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            spriteBatch.Draw(activeElementLayer, activeElementLayer.Bounds, Color.White);
            spriteBatch.End();

            if (bStartCombatZoom)
            {
                UIRenders.Add(RenderCombatZoomTest(spriteBatch));
            }



            #endregion

            spriteBatch.GraphicsDevice.SetRenderTarget(Game1.gameRender);
            #region RENDER DRAWING
            spriteBatch.GraphicsDevice.Clear(Color.Gray);

            //OW RENDER
            GameScreenEffect.DrawWithEffects(spriteBatch, finalLayer);

            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);

            foreach (var item in UIRenders)
            {
                //spriteBatch.Draw(item, item.Bounds, Color.White);
                GameScreenEffect.DrawWithNoEffects(spriteBatch, item);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
            if (BattleGUI.bIsRunning || BattleGUI.bBGUISoloModifierRunning)
            {
                spriteBatch.Draw(activeElementLayer, activeElementLayer.Bounds, Color.White);
            }
            UIRenders.Clear();

            foreach (var item in popUpRenders)
            {
                item.Draw(spriteBatch);
            }

            if (LUA.LuaExecutionList.DemandOverride())
            {
                spriteBatch.Draw(LUA.LuaExecutionList.GetRender(), new Rectangle(0, 0, 1366, 768), Color.White);
            }


            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);


            //         GameScreenEffect.DrawWithNoEffects(spriteBatch, gameRender);
            //END OW RENDER
            spriteBatch.End();

            if (ResolutionUtility.stdScale == new Vector2(1))
            {

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
                // spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearClamp, null, null, null, SceneUtility.transform);
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
            }

            //UI RENDER
            GameScreenEffect.Draw(spriteBatch, finalLayer);
            //END UI RENDER



            // spriteBatch.Draw(gameRender, gameRender.Bounds, Color.White);
            spriteBatch.End();

            if (ScriptProcessor.bIsRunning)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
                //UI RENDER
                ScriptProcessor.Draw(spriteBatch);

                //END UI RENDER
                spriteBatch.End();
                //ScriptProcessor.script;
            }





            #endregion

        }

        public static RenderTarget2D combatIntroLayerEffect = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        public static RenderTarget2D combatIntroLayer = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static Texture2D combatIntroSide = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\BGS1");
        static Texture2D combatIntroSideBG = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\BGS2BG");
        static Effect combatIntroEffect = Game1.contentManager.Load<Effect>(@"FX\Combat\Intro");
        static float horizontalOffSet = 0;
        private static RenderTarget2D RenderCombatZoomTest(SpriteBatch spriteBatch)
        {
            horizontalOffSet += 0.7f;
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(combatIntroLayerEffect);
            spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            spriteBatch.Draw(combatIntroSide, new Rectangle(0, 0, 1366, 768), Color.White);
            spriteBatch.Draw(BattleGUI.BattleGUIBG, new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, 1366, 768), Color.Gray * .9f);
            int portraitSize = 128 * 3;
            if (battleCaster != battleTarget)
            {
                spriteBatch.Draw(combatIntroSide, new Rectangle(0, 0, 1366, 768), Color.White);
                spriteBatch.Draw(BattleGUI.BattleGUIBG, new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, 1366, 768), Color.Gray * .9f);
                if (!bCasterIsLeft)
                {

                    battleCaster.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Angry].Draw(spriteBatch, new Rectangle((int)(-10 + horizontalOffSet), 10, portraitSize, portraitSize));
                    battleTarget.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Angry].Draw(spriteBatch, new Rectangle(1366 - portraitSize - ((int)(-10 + horizontalOffSet)), 786 - portraitSize - 10, portraitSize, portraitSize), 1.0f, SpriteEffects.FlipHorizontally);
                }
                else
                {
                    battleTarget.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Angry].Draw(spriteBatch, new Rectangle((int)(-10 + horizontalOffSet), 10, portraitSize, portraitSize));
                    battleCaster.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Angry].Draw(spriteBatch, new Rectangle(1366 - portraitSize - ((int)(-10 + horizontalOffSet)), 786 - portraitSize - 10, portraitSize, portraitSize), 1.0f, SpriteEffects.FlipHorizontally);
                }
            }
            else
            {
                spriteBatch.Draw(BattleGUI.BattleGUIBG, new Rectangle(0, 0, 1366, 768), new Rectangle(0, 0, 1366, 768), Color.Gray * .9f);
                if (!bCasterIsLeft)
                {

                    battleCaster.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Angry].Draw(spriteBatch, new Rectangle((int)(-10 + horizontalOffSet), 10, portraitSize, portraitSize));
                    // battleTarget.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Angry].Draw(spriteBatch, new Rectangle(1366 - portraitSize - ((int)(-10 + horizontalOffSet)), 786 - portraitSize - 10, portraitSize, portraitSize), 1.0f, SpriteEffects.FlipHorizontally);
                }
                else
                {
                    // battleTarget.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral].Draw(spriteBatch, new Rectangle((int)(-10 + horizontalOffSet), 10, portraitSize, portraitSize));
                    battleCaster.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Angry].Draw(spriteBatch, new Rectangle(1366 - portraitSize - ((int)(-10 + horizontalOffSet)), 786 - portraitSize - 10, portraitSize, portraitSize), 1.0f, SpriteEffects.FlipHorizontally);
                }
            }

            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(combatIntroLayer);
            spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
            combatIntroEffect.Parameters["ground"].SetValue(combatIntroSide);
            combatIntroEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(combatIntroLayerEffect, new Rectangle(0, 0, 1366, 768), Color.White);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            return combatIntroLayer;
        }

        internal static void UpdateCamera()
        {
            if (cameraFollowTarget != null && !bStartCombatZoom)
            {

                GenerateCamera(cameraFollowTarget, .06f, zoom);
                if (cameraStepsDone < cameraStepsNeedToDo && cameraMoveAmount != Vector2.Zero)
                {
                    cameraStepsDone++;
                    sceneCamera += cameraMoveAmount;
                    zoom += cameraZoomAmount;
                    CameraScaleMatrix = Matrix.CreateTranslation(new Vector3(sceneCamera.X, sceneCamera.Y, 1)) * Matrix.CreateScale(new Vector3(zoom, zoom, 1));
                    //bUpdateShadows = true;
                }
                else if (bUpdateOnceMore && Vector2.Zero == cameraMoveAmount)
                {
                    bUpdateOnceMore = false;
                    //bUpdateShadows = true;

                }

            }
        }

        internal static void AttemptStartRandomZoneFight()
        {
            if (loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)) != null && loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)).getZone(PlayerController.selectedSprite) != null && loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)).getZone(PlayerController.selectedSprite).Contains(PlayerController.selectedSprite.positionToMapCoords() * 64))
            {
                int amountOfChars = PlayerSaveData.heroParty.Count() + loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)).getZone(PlayerController.selectedSprite).zoneEncounterInfo.packSizeMax;
                var tempZone = loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)).getZone(PlayerController.selectedSprite);
                GameProcessor.GenerateCameraInstant(PlayerController.selectedSprite);
                var test = new Vector2(-sceneCamera.X, -sceneCamera.Y);
                Rectangle trueCamera = new Rectangle((int)(test.X) - (int)(100 / zoom), (int)(test.Y) - (int)(100 / zoom), (int)(1366 / zoom) + (int)(200 / zoom), (int)(768 / zoom) + (int)(200 / zoom));
                loadedMap.ForceCheckChunksToConsider(trueCamera);

                if (amountOfChars < loadedMap.possibleTilesGameZone(tempZone.returnBoundingZone()).Count)
                {
                    Console.WriteLine("Left");
                    EnableCombatStage();
                    CombatProcessor.Initialize(PlayerController.selectedSprite, loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)));
                    mainControllerBeforeCombat = PlayerController.selectedSprite;
                    loadedMap.mapRegions.Find(r => r.Contains(PlayerController.selectedSprite)).EnterCombat();
                }
                else
                {
                    CombatProcessor.Reset();
                    Console.WriteLine("Error, combatzone not large enough. Skipping fight.");
                }

            }
            else
            {
                CombatProcessor.Reset();
            }
        }

        static Effect shadowEffect = null;

        static List<KeyValuePair<RenderTarget2D, List<int>>> uniqueObjectRenders = new List<KeyValuePair<RenderTarget2D, List<int>>>();

        public static SamplerState shadowSampler = SamplerState.PointClamp;
        public static float shadowQualityPercentage = 0.5f;
        public static void DrawShadows(SpriteBatch spriteBatch, List<objectInfo> mapSpriteInfoBase)
        {

            bUpdateShadows = false;


            spriteBatch.End();

            if (shadowEffect == null)
            {
                shadowEffect = Game1.contentManager.Load<Effect>(@"FX\Environment\NewShadows");
                //  shadowEffect = Game1.contentManager.Load<Effect>(@"FX\Environment\ThunderShadow");
            }

            // SamplerState shadowSampler = SamplerState.PointClamp;

            var mapSpriteInfo = mapSpriteInfoBase.FindAll(msi => msi.bIsVisible);

            if (CombatProcessor.bIsRunning)
            {
                mapSpriteInfo.AddRange(BasicMap.getCombatObjectInfo());
            }

            //sprites.RemoveAll(s => !s.bIsVisible);
            // var allUniqueObjs
            List<BaseSprite> tempList = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.Sprite).Select(oi => oi.obj).Cast<BaseSprite>().ToList().GroupBy(x => x.shapeID).Select(g => g.First()).ToList();
            List<BaseSprite> allUniqueObjs = new List<BaseSprite>();

            for (int i = 0; i < tempList.Count; i++)
            {
                if (tempList[i].GetType() == typeof(BaseSprite))
                {
                    allUniqueObjs.Add(GameProcessor.gcDB.gameObjectObjects.Find(o => o.shapeID == tempList[i].shapeID));
                }
                else if (tempList[i].GetType() == typeof(SpriteLight))
                {
                    allUniqueObjs.Add(GameProcessor.gcDB.gameLights.Find(o => o.shapeID == tempList[i].shapeID));
                }

            }

            var allCharacters = mapSpriteInfo.FindAll(oi => oi.obj.GetType() != typeof(NPC) && oi.objectType == objectInfo.type.Character).Select(oi => oi.obj).Cast<BaseCharacter>().ToList();
            var allNPCs = mapSpriteInfo.FindAll(oi => oi.obj.GetType() == typeof(NPC)).Select(oi => (oi.obj as NPC).baseCharacter).Cast<BaseCharacter>().ToList();
            var allObjGroups = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.ObjectGroup).Select(oi => oi.obj).Cast<ObjectGroup>().ToList();
            var allUniqueObjGroups = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.ObjectGroup).Select(oi => oi.obj).Cast<ObjectGroup>().ToList().GroupBy(x => x.groupID).Select(g => g.First()).ToList();
            var allBuildings = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.Building).Select(oi => oi.obj).Cast<Building>().ToList();

            List<BaseSprite> sprites = new List<BaseSprite>();
            sprites.AddRange(mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.Sprite).Select(oi => oi.obj).Cast<BaseSprite>().ToList());
            sprites.AddRange(allCharacters);
            allCharacters.AddRange(allNPCs);
            sprites.AddRange(allNPCs);
            allUniqueObjs.AddRange(allCharacters.GroupBy(x => x.shapeID).Select(g => g.First()).ToList());

            List<BaseSprite> uniqueObjectsToAdd = new List<BaseSprite>();

            foreach (var item in allUniqueObjs)
            {
                if (item.GetType() == typeof(BaseCharacter))
                {
                    foreach (var character in allCharacters)
                    {
                        if (character != item && character.shapeID == item.shapeID)
                        {
                            if (character.rotationIndex != item.rotationIndex || character.animationIndex != item.animationIndex)
                            {
                                var found = uniqueObjectsToAdd.Find(obj => obj.shapeID == character.shapeID && obj.rotationIndex == character.rotationIndex && obj.animationIndex == character.animationIndex);
                                if (found == null)
                                {
                                    uniqueObjectsToAdd.Add(character);
                                    //uniqueObjectsToAdd.Add(GameProcessor.gcDB.gameCharacters.Find(gc => gc.shapeID == character.shapeID));
                                    //break;
                                }
                            }
                        }
                    }
                }
            }

            //for (int i = 0; i < uniqueObjectsToAdd.Count; i++)
            //{
            //    var t = GameProcessor.gcDB.gameCharacters.Find(o => o.shapeID == uniqueObjectsToAdd[i].shapeID);
            //    if (t != null)
            //    {
            //        // allUniqueObjs.AddRange(uniqueObjectsToAdd);
            //        allUniqueObjs.Add(t);
            //    }
            //    else
            //    {

            //    }
            //}
            allUniqueObjs.AddRange(uniqueObjectsToAdd);

            List<KeyValuePair<RenderTarget2D, List<int>>> usedRenders = new List<KeyValuePair<RenderTarget2D, List<int>>>();
            List<RenderTarget2D> usedOGRenders = new List<RenderTarget2D>();
            List<KeyValuePair<RenderTarget2D, List<int>>> uniqueObjectRenders = new List<KeyValuePair<RenderTarget2D, List<int>>>();

            foreach (var item in allUniqueObjs)
            {
                if (uniqueObjectRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex).Equals(default(KeyValuePair<RenderTarget2D, List<int>>)))
                {
                    //var t = sprites.Find(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex);
                    var testSprite = sprites.Find(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex);
                    if (testSprite.spriteGameSize.Size == new Point())
                    {
                        testSprite.spriteGameSize.Size = new Point(64);
                    }
                    uniqueObjectRenders.Add(new KeyValuePair<RenderTarget2D, List<int>>(new RenderTarget2D(Game1.graphics.GraphicsDevice, testSprite.spriteGameSize.Height * 3, testSprite.spriteGameSize.Height), new List<int> { item.shapeID, item.rotationIndex, item.animationIndex }));
                }

                if (item == PlayerController.selectedSprite)
                {

                }

                // usedRenders.Add(uniqueObjectRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex));
                var infoRender = uniqueObjectRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex);

                usedRenders.Add(new KeyValuePair<RenderTarget2D, List<int>>(new RenderTarget2D(Game1.graphics.GraphicsDevice, infoRender.Key.Width, infoRender.Key.Height), infoRender.Value));
            }

            foreach (var item in allUniqueObjGroups)
            {
                if (item.getRender() == null)
                {
                    item.GenerateRender(spriteBatch);
                }
                usedOGRenders.Add(new RenderTarget2D(Game1.graphics.GraphicsDevice, (int)(item.getRendeSize().X), (int)(item.getRendeSize().Y)));
            }

            int uniqueIndex = 0;
            foreach (var item in allUniqueObjs)
            {
                var sprite = sprites.Find(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex).ShallowCopy();
                sprite.position = Vector2.Zero;
                //sprite.position = new Vector2(-64);

                sprite.UpdatePositioNonMovement();
                spriteBatch.GraphicsDevice.SetRenderTarget(null);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);

                var tempRender = allUniqueObjs[uniqueIndex].getFXRender(spriteBatch);
                if (tempRender == null)
                { goto skip; }
                int width = tempRender.Width;
                int height = tempRender.Height;
                var searchedRender = usedRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex).Key;
                //var searchedRender = usedRenders[uniqueIndex].Key;
                //  var searchIndex = usedRenders.IndexOf(usedRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex));
                spriteBatch.GraphicsDevice.SetRenderTarget(searchedRender);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, shadowSampler, null, null, null, null);


                //shadowEffect.Parameters["alpha"].SetValue(DayLightHandler.ShadowOpacity);
                shadowEffect.Parameters["alpha"].SetValue(EnvironmentHandler.ShadowOpacity());
                shadowEffect.Parameters["divider"].SetValue(DayLightHandler.ShadowDivider);
                shadowEffect.Parameters["horizontalModifier"].SetValue(DayLightHandler.ShadowOffset);

                shadowEffect.Parameters["size"].SetValue(new Vector2(width, height));
                shadowEffect.CurrentTechnique.Passes[0].Apply();

                //sprite.Draw(spriteBatch);
                //spriteBatch.Draw(tempRender, tempRender.Bounds, Color.White);
                spriteBatch.Draw(tempRender, new Rectangle(0, 0, (int)(tempRender.Width * shadowQualityPercentage), (int)(tempRender.Height * shadowQualityPercentage)), Color.White);

                spriteBatch.End();
                sprite = null;
                tempRender = null;
                uniqueIndex++;
            skip: { }
            }

            foreach (var item in allUniqueObjGroups)
            {
                spriteBatch.GraphicsDevice.SetRenderTarget(null);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);

                //// var tempRender = allUniqueObjs[uniqueIndex].getFXRender(spriteBatch);
                // int width = tempRender.Width;
                // int height = tempRender.Height;
                var searchedRender = usedOGRenders[allUniqueObjGroups.IndexOf(item)];

                spriteBatch.GraphicsDevice.SetRenderTarget(searchedRender);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, shadowSampler, null, null, null, null);


                shadowEffect.Parameters["alpha"].SetValue(DayLightHandler.ShadowOpacity);
                shadowEffect.Parameters["divider"].SetValue(DayLightHandler.ShadowDivider);
                shadowEffect.Parameters["horizontalModifier"].SetValue(DayLightHandler.ShadowOffset);

                shadowEffect.Parameters["size"].SetValue(new Vector2(item.getRendeSize().X, item.getRendeSize().Y));
                shadowEffect.CurrentTechnique.Passes[0].Apply();

                //   spriteBatch.Draw(item.getRender(), item.getRender().Bounds, Color.White);

                item.DrawAtOrigin(spriteBatch);

                spriteBatch.End();
            }

            List<RenderTarget2D> buildingRenders = new List<RenderTarget2D>();
            List<RenderTarget2D> buildingShadowRenders = new List<RenderTarget2D>();
            List<int> buildingIndexes = new List<int>();
            foreach (var item in allBuildings)
            {
                buildingRenders.Add(item.buildingCompleteRender);
                buildingIndexes.Add(loadedMap.testBuildings.IndexOf(item));
            }

            int index = 0;
            foreach (var item in buildingRenders)
            {
                buildingShadowRenders.Add(new RenderTarget2D(Game1.graphics.GraphicsDevice, (int)(item.Width * shadowQualityPercentage), (int)(item.Height * shadowQualityPercentage)));
                spriteBatch.GraphicsDevice.SetRenderTarget(buildingShadowRenders[index]);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, shadowSampler, null, null, null, null);

                shadowEffect.Parameters["size"].SetValue(new Vector2(item.Width, item.Height));
                shadowEffect.Parameters["alpha"].SetValue(DayLightHandler.ShadowOpacity);
                shadowEffect.Parameters["divider"].SetValue(DayLightHandler.ShadowDivider);
                shadowEffect.Parameters["horizontalModifier"].SetValue(DayLightHandler.ShadowOffset);
                // shadowEffect.Parameters["bUsesEffect"].SetValue((bool)item.bIsAffectedByWind);

                shadowEffect.CurrentTechnique.Passes[0].Apply();

                spriteBatch.Draw(item, buildingShadowRenders[index].Bounds, Color.White);

                spriteBatch.End();
                index++;
            }




            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.GraphicsDevice.SetRenderTarget(shadowRender);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, shadowSampler, null, null, null, CameraScaleMatrix);
            spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            foreach (var item in allUniqueObjs)
            {
                List<BaseSprite> spritesToDraw = sprites.FindAll(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex);
                var renderForItem = usedRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex).Key;

                foreach (var sprite in spritesToDraw)
                {
                    sprite.AssignShadowRender(renderForItem);

                    Rectangle r = new Rectangle(new Point((int)(sprite.position - new Vector2(sprite.spriteGameSize.Height, 0)).X, (int)sprite.position.Y), renderForItem.Bounds.Size);
                    //if (sprite.scaleVector == new Vector2(1))
                    //{
                    //    spriteBatch.Draw(renderForItem, r, new Rectangle(0, 0, (int)(renderForItem.Width * percentage), (int)(renderForItem.Height * percentage)), Color.White);
                    //}
                    //else

                    if (!sprite.scaleVector.Equals(new Vector2(1)))
                    {

                    }

                    {
                        int adjustX = (int)(((renderForItem.Width * sprite.scaleVector.X) - (sprite.spriteGameSize.Height * sprite.scaleVector.X)) / 2);
                        Rectangle r2 = new Rectangle(sprite.spriteGameSize.Location - new Point(adjustX, 0), (r.Size.ToVector2() * sprite.scaleVector.X).ToPoint());

                        spriteBatch.Draw(renderForItem, r2, new Rectangle(0, 0, (int)(renderForItem.Width * shadowQualityPercentage), (int)(renderForItem.Height * shadowQualityPercentage)), Color.White);
                    }
                }
            }

            foreach (var item in allUniqueObjGroups)
            {
                List<ObjectGroup> spritesToDraw = allObjGroups.FindAll(s => s.groupID == item.groupID);
                var renderForItem = usedOGRenders[allUniqueObjGroups.IndexOf(item)];

                foreach (var sprite in spritesToDraw)
                {
                    if (sprite.scaleVector != new Vector2(1))
                    {

                    }
                    sprite.AssignShadowRender(renderForItem);
                    //    spriteBatch.Draw(renderForItem, new Rectangle(new Point((int)(sprite.position - new Vector2(sprite.spriteGameSize.Height, 0)).X, (int)sprite.position.Y), renderForItem.Bounds.Size), renderForItem.Bounds, Color.White);
                    var r = new Rectangle(new Point((int)(sprite.getFXRenderDawLoc().Location.ToVector2()).X, (int)sprite.getFXRenderDawLoc().Location.ToVector2().Y), item.getFXRenderDawLoc().Size);
                    spriteBatch.Draw(renderForItem, r, new Rectangle(0, 0, (int)(renderForItem.Width * shadowQualityPercentage), (int)(renderForItem.Height * shadowQualityPercentage)), Color.White);

                }
            }

            index = 0;
            foreach (var item in buildingShadowRenders)
            {

                int id = buildingIndexes[index];
                loadedMap.testBuildings[id].AssignShadowRender(item);

                // spriteBatch.Draw(item, loadedMap.testBuildings[id].boundingZone.Location.ToVector2() - loadedMap.testBuildings[id].boundingDrawZone.Location.ToVector2(), new Rectangle(0, 0, (int)(item.Bounds.Width * percentage), (int)(item.Bounds.Height * percentage)), Color.White);
                Rectangle tempR = new Rectangle(loadedMap.testBuildings[id].boundingZone.X - loadedMap.testBuildings[id].boundingDrawZone.X, loadedMap.testBuildings[id].boundingZone.Y - loadedMap.testBuildings[id].boundingDrawZone.Location.Y, (int)(item.Bounds.Width / shadowQualityPercentage), (int)(item.Bounds.Height / shadowQualityPercentage));
                spriteBatch.Draw(item, tempR, item.Bounds, Color.White);
                // spriteBatch.Draw(item, item.Bounds, Color.White);
                index++;

            }

            foreach (var item in buildingRenders)
            {
                //  item.Dispose();
            }
            foreach (var item in buildingShadowRenders)
            {
                // item.Dispose();
            }
            foreach (var item in usedOGRenders)
            {
                //    item.Dispose();
            }


            //foreach (var item in sprites)
            //{

            //    var renderForItem = usedRenders.Find(kv => kv.Value == item.shapeID).Key;
            //    spriteBatch.Draw(renderForItem, item.position, renderForItem.Bounds, Color.White);


            //}

            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);

            // usedOGRenders.ForEach(o => o.Dispose());
            //usedRenders.ForEach(o => o.Key.Dispose());
            uniqueObjectRenders.ForEach(o => o.Key.Dispose());
            // buildingShadowRenders.ForEach(o => o.Dispose());

            allUniqueObjs.Clear();
            sprites.Clear();
            allCharacters.Clear();
            uniqueObjectsToAdd.Clear();
            usedRenders.Clear();
            allUniqueObjs = null;
            sprites = null;
            allCharacters = null;
            uniqueObjectsToAdd = null;
            usedRenders = null;
        }

        public static void DrawWaterShader(SpriteBatch spriteBatch, RenderTarget2D r2D, List<BasicTile> reflectionTiles, List<objectInfo> mapSpriteInfoBase)
        {

            try
            {
                spriteBatch.End();

                if (heightMapEffect == null)
                {
                    heightMapEffect = Game1.contentManager.Load<Effect>(@"FX\Environment\2DHeightMap");
                }

                var mapSpriteInfo = new List<objectInfo>(mapSpriteInfoBase.FindAll(msi => msi.bIsVisible && (msi.bNeedsReflection || msi.objectType == objectInfo.type.Character)));

                if (CombatProcessor.bIsRunning)
                {
                    mapSpriteInfo.AddRange(BasicMap.getCombatObjectInfo());
                }

                mapSpriteInfo.Reverse();
                //sprites.RemoveAll(s => !s.bIsVisible);
                var allUniqueObjs = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.Sprite).Select(oi => oi.obj).Cast<BaseSprite>().ToList().GroupBy(x => x.shapeID).Select(g => g.First()).ToList();
                var allCharacters = mapSpriteInfo.FindAll(oi => oi.obj.GetType() != typeof(NPC) && oi.objectType == objectInfo.type.Character).Select(oi => oi.obj).Cast<BaseCharacter>().ToList();
                var allNPCs = mapSpriteInfo.FindAll(oi => oi.obj.GetType() == typeof(NPC)).Select(oi => (oi.obj as NPC).baseCharacter).Cast<BaseCharacter>().ToList();
                var allObjGroups = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.ObjectGroup).Select(oi => oi.obj).Cast<ObjectGroup>().ToList();
                var allUniqueObjGroups = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.ObjectGroup).Select(oi => oi.obj).Cast<ObjectGroup>().ToList().GroupBy(x => x.groupID).Select(g => g.First()).ToList();

                allObjGroups.ForEach(og => og.GenerateRender(spriteBatch));
                allUniqueObjGroups.ForEach(og => og.GenerateRender(spriteBatch));

                List<BaseSprite> sprites = new List<BaseSprite>();
                sprites.AddRange(mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.Sprite).Select(oi => oi.obj).Cast<BaseSprite>().ToList());
                sprites.AddRange(allCharacters);
                allCharacters.AddRange(allNPCs);
                sprites.AddRange(allNPCs);
                allUniqueObjs.AddRange(allCharacters.GroupBy(x => x.shapeID).Select(g => g.First()).ToList());

                List<BaseSprite> uniqueObjectsToAdd = new List<BaseSprite>();


                List<BaseSprite> tempList = new List<BaseSprite>();
                foreach (var item in sprites)
                {
                    var r = new Rectangle(item.trueMapSize().X, item.trueMapSize().Y + item.trueMapSize().Height, item.trueMapSize().Width, item.trueMapSize().Height);
                    foreach (var rTile in reflectionTiles)
                    {
                        if (rTile.mapPosition.Contains(r) || rTile.mapPosition.Intersects(r))
                        {
                            tempList.Add(item);
                            break;
                        }
                    }
                }
                sprites = tempList;
                allUniqueObjs = sprites.GroupBy(x => x.shapeID).Select(g => g.First()).ToList(); //Make unique IDs based on reflection



                List<ObjectGroup> objectGroups = mapSpriteInfo.FindAll(oi => oi.objectType == objectInfo.type.ObjectGroup).Select(oi => oi.obj).Cast<ObjectGroup>().ToList();
                List<ObjectGroup> tempList2 = new List<ObjectGroup>();
                foreach (var item in objectGroups)
                {
                    var r = new Rectangle(item.trueMapSize.X, item.trueMapSize.Y + item.trueMapSize.Height, item.trueMapSize.Width, item.trueMapSize.Height);
                    foreach (var rTile in reflectionTiles)
                    {
                        if (rTile.mapPosition.Contains(r) || rTile.mapPosition.Intersects(r))
                        {
                            tempList2.Add(item);
                            break;
                        }
                    }
                }
                objectGroups = tempList2;
                allUniqueObjGroups = objectGroups.GroupBy(x => x.groupID).Select(g => g.First()).ToList();

                foreach (var item in allUniqueObjs)
                {
                    if (item.GetType() == typeof(BaseCharacter))
                    {
                        foreach (var character in allCharacters)
                        {
                            if (character != item && character.shapeID == item.shapeID)
                            {
                                if (character.rotationIndex != item.rotationIndex || character.animationIndex != item.animationIndex)
                                {
                                    var found = uniqueObjectsToAdd.Find(obj => obj.shapeID == character.shapeID && obj.rotationIndex == character.rotationIndex && obj.animationIndex == character.animationIndex);
                                    if (found == null)
                                    {
                                        uniqueObjectsToAdd.Add(character);
                                        //break;
                                    }
                                }
                            }
                        }
                    }
                }

                allUniqueObjs.AddRange(uniqueObjectsToAdd);
                allUniqueObjs.RemoveAll(o => !sprites.Contains(o));

                List<KeyValuePair<RenderTarget2D, List<int>>> usedRenders = new List<KeyValuePair<RenderTarget2D, List<int>>>();
                List<RenderTarget2D> usedOGRenders = new List<RenderTarget2D>();

                foreach (var item in allUniqueObjs)
                {
                    if (uniqueObjectRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex).Equals(default(KeyValuePair<RenderTarget2D, List<int>>)))
                    {
                        uniqueObjectRenders.Add(new KeyValuePair<RenderTarget2D, List<int>>(new RenderTarget2D(Game1.graphics.GraphicsDevice, sprites.Find(s => s.shapeID == item.shapeID).spriteGameSize.Width, sprites.Find(s => s.shapeID == item.shapeID).spriteGameSize.Height), new List<int> { item.shapeID, item.rotationIndex, item.animationIndex }));
                    }

                    usedRenders.Add(uniqueObjectRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex));
                    //usedRenders.Add(uniqueObjectRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex));
                }

                foreach (var item in allUniqueObjGroups)
                {
                    usedOGRenders.Add(new RenderTarget2D(Game1.graphics.GraphicsDevice, (int)(item.getFXRenderDawLoc().Width), (int)(item.getFXRenderDawLoc().Height)));
                }


                foreach (var item in allUniqueObjs)
                {
                    spriteBatch.GraphicsDevice.SetRenderTarget(usedRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex).Key);
                    spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
                    var sprite = sprites.Find(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex).ShallowCopy();
                    sprite.position = Vector2.Zero;

                    sprite.UpdatePositioNonMovement();

                    heightMapEffect.Parameters["texHeight"].SetValue((float)sprite.returnProperTexBounds().Height);
                    heightMapEffect.Parameters["sourcePosY"].SetValue((float)sprite.returnTexSource().Y);
                    heightMapEffect.Parameters["sourceHeight"].SetValue((float)sprite.returnTexSource().Height);

                    heightMapEffect.CurrentTechnique.Passes[0].Apply();
                    sprite.Draw(spriteBatch, default(Color), -1, true);
                    // sprite.Draw(spriteBatch,new Rectangle(sprite.spriteGameSize.X,sprite.spriteGameSize.Y,sprite.spriteGameSize.Width/3,sprite.spriteGameSize.Height), default(Color), -1, true);

                    spriteBatch.End();
                    sprite = null;
                }

                foreach (var item in allUniqueObjGroups)
                {
                    var searchedRender = usedOGRenders[allUniqueObjGroups.IndexOf(item)];

                    spriteBatch.GraphicsDevice.SetRenderTarget(searchedRender);
                    spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);

                    heightMapEffect.Parameters["texHeight"].SetValue((float)item.getRender().Height);
                    heightMapEffect.Parameters["sourcePosY"].SetValue((float)0);
                    heightMapEffect.Parameters["sourceHeight"].SetValue((float)item.getRender().Height);

                    heightMapEffect.CurrentTechnique.Passes[0].Apply();

                    item.DrawAtOrigin2(spriteBatch);

                    spriteBatch.End();
                }


                List<RenderTarget2D> buildingRenders = new List<RenderTarget2D>();
                List<RenderTarget2D> buildingShadowRenders = new List<RenderTarget2D>();
                List<int> buildingIndexes = new List<int>();
                foreach (var item in loadedMap.testBuildings.FindAll(b => b.Contains(new Rectangle((int)-sceneCamera.X, (int)-sceneCamera.Y, 1366, 768), zoom)))
                {
                    buildingRenders.Add(item.buildingCompleteRender);
                    buildingIndexes.Add(loadedMap.testBuildings.IndexOf(item));
                }

                int index = 0;
                foreach (var item in buildingRenders)
                {
                    buildingShadowRenders.Add(new RenderTarget2D(Game1.graphics.GraphicsDevice, item.Width, item.Height));
                    spriteBatch.GraphicsDevice.SetRenderTarget(buildingShadowRenders[index]);
                    spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);

                    heightMapEffect.Parameters["texHeight"].SetValue((float)item.Bounds.Height);
                    heightMapEffect.Parameters["sourcePosY"].SetValue((float)item.Bounds.Y);
                    heightMapEffect.Parameters["sourceHeight"].SetValue((float)item.Bounds.Height);

                    heightMapEffect.CurrentTechnique.Passes[0].Apply();
                    // shadowEffect.Parameters["bUsesEffect"].SetValue((bool)item.bIsAffectedByWind);

                    spriteBatch.Draw(item, item.Bounds, Color.White);

                    spriteBatch.End();
                    index++;
                }


                spriteBatch.GraphicsDevice.SetRenderTarget(null);
                spriteBatch.GraphicsDevice.SetRenderTarget(r2D);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);
                spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));

                foreach (var item in allUniqueObjs)
                {
                    List<BaseSprite> spritesToDraw = sprites.FindAll(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex);
                    var renderForItem = usedRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex).Key;

                    foreach (var sprite in spritesToDraw)
                    {
                        //if (sprite.groundTileType == TileSource.TyleType.Fluid)
                        //{
                        //    spriteBatch.Draw(renderForItem, new Vector2(sprite.position.X, sprite.position.Y + sprite.returnTexSource().Height - 15), new Rectangle(0, 0, renderForItem.Width, renderForItem.Height), Color.White);

                        //}
                        //else
                        //{
                        //    spriteBatch.Draw(renderForItem, new Vector2(sprite.position.X, sprite.position.Y + sprite.returnTexSource().Height - 15), new Rectangle(0, 0, renderForItem.Width, renderForItem.Height), Color.White);
                        //}

                        Rectangle r = new Rectangle(new Point((int)(sprite.position).X, (int)(sprite.position.Y + (sprite.returnTexSource().Height * sprite.scaleVector.Y) - 15 * sprite.scaleVector.Y)), (renderForItem.Bounds.Size.ToVector2() * sprite.scaleVector).ToPoint());

                        spriteBatch.Draw(renderForItem, r, new Rectangle(0, 0, (int)(renderForItem.Width), (int)(renderForItem.Height)), Color.White);
                    }


                }

                foreach (var item in allUniqueObjGroups)
                {
                    List<ObjectGroup> spritesToDraw = objectGroups.FindAll(s => s.groupID == item.groupID);
                    var renderForItem = usedOGRenders[allUniqueObjGroups.IndexOf(item)];

                    foreach (var sprite in spritesToDraw)
                    {
                        // var r = new Rectangle(new Point((int)(sprite.getFXRenderDawLoc().Location.ToVector2()).X, (int)sprite.getFXRenderDawLoc().Location.ToVector2().Y), item.getFXRenderDawLoc().Size);
                        Rectangle r = new Rectangle(new Point((int)(sprite.trueMapSize).X, (int)(sprite.trueMapSize.Y + sprite.trueMapSize.Height * sprite.scaleVector.Y) - 15), renderForItem.Bounds.Size);

                        spriteBatch.Draw(renderForItem, r, new Rectangle(0, 0, (int)(renderForItem.Width), (int)(renderForItem.Height)), Color.White);

                    }
                }


                index = 0;
                foreach (var item in buildingShadowRenders)
                {
                    int id = buildingIndexes[index];
                    spriteBatch.Draw(item, loadedMap.testBuildings[id].boundingZone.Location.ToVector2() - loadedMap.testBuildings[id].boundingDrawZone.Location.ToVector2() + new Vector2(0, loadedMap.testBuildings[id].boundingDrawZone.Height - 15), item.Bounds, Color.White);
                    // spriteBatch.Draw(item, item.Bounds, Color.White);
                    index++;

                }

                foreach (var item in usedRenders)
                {
                    //  item.Key.Dispose();
                }

                foreach (var item in buildingRenders)
                {
                    //  item.Dispose();
                }
                foreach (var item in buildingShadowRenders)
                {
                    item.Dispose();
                }
                foreach (var item in usedOGRenders)
                {
                    item.Dispose();
                }

                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error in water shader");
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);
            }

        }

        public static void DrawObjHeight(SpriteBatch spriteBatch, RenderTarget2D target, Effect fx, Matrix m)
        {

            try
            {
                spriteBatch.End();
                float percentage = 1f;

                var sprites = loadedMap.returnAllObjectsInCamera(new Rectangle((int)-sceneCamera.X, (int)-sceneCamera.Y, 1366, 768), zoom);
                //   sprites.Reverse();

                foreach (var item in sprites)
                {
                    var r = new Rectangle(item.spriteGameSize.X, item.spriteGameSize.Y + item.spriteGameSize.Height, item.spriteGameSize.Width, item.spriteGameSize.Height);
                }


                var allUniqueObjs = sprites.GroupBy(x => x.shapeID).Select(g => g.First()).ToList();

                var allCharacters = sprites.FindAll(obj => obj.GetType() == typeof(BaseCharacter));
                List<BaseSprite> uniqueObjectsToAdd = new List<BaseSprite>();

                foreach (var item in allUniqueObjs)
                {
                    if (item.GetType() == typeof(BaseCharacter))
                    {
                        foreach (var character in allCharacters)
                        {
                            if (character != item && character.shapeID == item.shapeID)
                            {
                                if (character.rotationIndex != item.rotationIndex || character.animationIndex != item.animationIndex)
                                {
                                    var found = uniqueObjectsToAdd.Find(obj => obj.shapeID == character.shapeID && obj.rotationIndex == character.rotationIndex && obj.animationIndex == character.animationIndex);
                                    if (found == null)
                                    {
                                        uniqueObjectsToAdd.Add(character);
                                        //break;
                                    }
                                }
                            }
                        }
                    }
                }

                allUniqueObjs.AddRange(uniqueObjectsToAdd);

                List<KeyValuePair<RenderTarget2D, List<int>>> usedRenders = new List<KeyValuePair<RenderTarget2D, List<int>>>();

                foreach (var item in allUniqueObjs)
                {
                    if (uniqueObjectRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex).Equals(default(KeyValuePair<RenderTarget2D, List<int>>)))
                    {
                        uniqueObjectRenders.Add(new KeyValuePair<RenderTarget2D, List<int>>(new RenderTarget2D(Game1.graphics.GraphicsDevice, sprites.Find(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex).spriteGameSize.Height * 3, sprites.Find(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex).spriteGameSize.Height), new List<int> { item.shapeID, item.rotationIndex, item.animationIndex }));
                    }

                    usedRenders.Add(uniqueObjectRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex));
                }

                int uniqueIndex = 0;
                var usedSampler = SamplerState.PointClamp;
                foreach (var item in allUniqueObjs)
                {
                    var sprite = sprites.Find(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex).ShallowCopy();
                    sprite.position = Vector2.Zero;
                    //sprite.position = new Vector2(-64);

                    sprite.UpdatePositioNonMovement();
                    var tempRender = allUniqueObjs[uniqueIndex].getFXRender(spriteBatch);
                    int width = tempRender.Width;
                    int height = tempRender.Height;
                    var searchedRender = usedRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex).Key;
                    //var searchedRender = usedRenders[uniqueIndex].Key;
                    //  var searchIndex = usedRenders.IndexOf(usedRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex));
                    spriteBatch.GraphicsDevice.SetRenderTarget(searchedRender);
                    spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, usedSampler, null, null, null, null);

                    int h = sprite.returnTexSource().Height;
                    fx.Parameters["gameHeight"].SetValue((float)(sprite.spriteGameSize.Height));
                    fx.CurrentTechnique.Passes[0].Apply();

                    //sprite.Draw(spriteBatch);
                    //spriteBatch.Draw(tempRender, tempRender.Bounds, Color.White);
                    spriteBatch.Draw(tempRender, new Rectangle(0, 0, (int)(tempRender.Width * percentage), (int)(tempRender.Height * percentage)), Color.White);

                    spriteBatch.End();
                    sprite = null;
                    tempRender = null;
                    uniqueIndex++;
                }


                List<RenderTarget2D> buildingRenders = new List<RenderTarget2D>();
                List<RenderTarget2D> buildingShadowRenders = new List<RenderTarget2D>();
                List<int> buildingIndexes = new List<int>();
                foreach (var item in loadedMap.testBuildings.FindAll(b => b.Contains(new Rectangle((int)-sceneCamera.X, (int)-sceneCamera.Y, 1366, 768), zoom)))
                {
                    buildingRenders.Add(item.buildingCompleteRender);
                    buildingIndexes.Add(loadedMap.testBuildings.IndexOf(item));
                }

                int index = 0;
                foreach (var item in buildingRenders)
                {
                    buildingShadowRenders.Add(new RenderTarget2D(Game1.graphics.GraphicsDevice, (int)(item.Width * percentage), (int)(item.Height * percentage)));
                    spriteBatch.GraphicsDevice.SetRenderTarget(buildingShadowRenders[index]);
                    spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                    spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);

                    fx.Parameters["gameHeight"].SetValue((float)(buildingRenders[index].Height));
                    fx.CurrentTechnique.Passes[0].Apply();
                    // shadowEffect.Parameters["bUsesEffect"].SetValue((bool)item.bIsAffectedByWind);

                    spriteBatch.Draw(item, item.Bounds, Color.White);

                    spriteBatch.End();
                    index++;
                }


                spriteBatch.GraphicsDevice.SetRenderTarget(null);
                spriteBatch.GraphicsDevice.SetRenderTarget(target);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, usedSampler, null, null, null, m);
                spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
                foreach (var item in allUniqueObjs)
                {
                    List<BaseSprite> spritesToDraw = sprites.FindAll(s => s.shapeID == item.shapeID && s.rotationIndex == item.rotationIndex && s.animationIndex == item.animationIndex);
                    var renderForItem = usedRenders.Find(kv => kv.Value[0] == item.shapeID && kv.Value[1] == item.rotationIndex && kv.Value[2] == item.animationIndex).Key;

                    foreach (var sprite in spritesToDraw)
                    {
                        //    spriteBatch.Draw(renderForItem, new Rectangle(new Point((int)(sprite.position - new Vector2(sprite.spriteGameSize.Height, 0)).X, (int)sprite.position.Y), renderForItem.Bounds.Size), renderForItem.Bounds, Color.White);
                        spriteBatch.Draw(renderForItem, new Rectangle(new Point((int)(sprite.position - new Vector2(sprite.spriteGameSize.Height, 0)).X, (int)sprite.position.Y), renderForItem.Bounds.Size), new Rectangle(0, 0, (int)(renderForItem.Width * percentage), (int)(renderForItem.Height * percentage)), Color.White);

                    }
                }

                index = 0;
                foreach (var item in buildingShadowRenders)
                {
                    int id = buildingIndexes[index];
                    // spriteBatch.Draw(item, loadedMap.testBuildings[id].boundingZone.Location.ToVector2() - loadedMap.testBuildings[id].boundingDrawZone.Location.ToVector2(), new Rectangle(0, 0, (int)(item.Bounds.Width * percentage), (int)(item.Bounds.Height * percentage)), Color.White);
                    Rectangle tempR = new Rectangle(loadedMap.testBuildings[id].boundingZone.X - loadedMap.testBuildings[id].boundingDrawZone.X, loadedMap.testBuildings[id].boundingZone.Y - loadedMap.testBuildings[id].boundingDrawZone.Location.Y, (int)(item.Bounds.Width / percentage), (int)(item.Bounds.Height / percentage));
                    spriteBatch.Draw(item, tempR, item.Bounds, Color.White);
                    // spriteBatch.Draw(item, item.Bounds, Color.White);
                    index++;

                }

                foreach (var item in buildingRenders)
                {
                    //  item.Dispose();
                }
                foreach (var item in buildingShadowRenders)
                {
                    item.Dispose();
                }



                //foreach (var item in sprites)
                //{

                //    var renderForItem = usedRenders.Find(kv => kv.Value == item.shapeID).Key;
                //    spriteBatch.Draw(renderForItem, item.position, renderForItem.Bounds, Color.White);


                //}

                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(null);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, m);


            }
            catch (Exception)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, m);
            }

        }

        static Effect renderEffect = null;
        public static void TestDrawSomeEffects(SpriteBatch spriteBatch, RenderTarget2D r2d)
        {
            //renderEffect = Game1.contentManager.Load<Effect>(@"FX\Environment\TestRenderEffect");
            if (renderEffect == null)
            {
                renderEffect = Game1.contentManager.Load<Effect>(@"FX\Environment\TestRenderEffect");
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);


            //renderEffect.Parameters["hMod"].SetValue(200f);


            //Color[] color = new Color[1366 * 768];
            //r2d.GetData<Color>(color);
            //renderEffect.CurrentTechnique.Passes[0].Apply();


            spriteBatch.Draw(r2d, new Rectangle(0, 0, 1366, 768), Color.White);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
        }

        static Effect heightMapEffect = Game1.contentManager.Load<Effect>(@"FX\Environment\2DHeightMap");
        //public static void TestDrawHeightMap(SpriteBatch spriteBatch)
        //{
        //    //spriteBatch.End();
        //    //spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);



        //    //foreach (var item in list)
        //    //{
        //    //    int var1 = item.returnProperTexBounds().Height;
        //    //    int var2 = item.returnProperTexBounds().Y;
        //    //    int var3 = item.returnTexSource().Height;
        //    //    heightMapEffect.Parameters["texHeight"].SetValue((float)item.returnProperTexBounds().Height);
        //    //    heightMapEffect.Parameters["sourcePosY"].SetValue((float)item.returnTexSource().Y);
        //    //    heightMapEffect.Parameters["sourceHeight"].SetValue((float)item.returnTexSource().Height);
        //    //    heightMapEffect.CurrentTechnique.Passes[0].Apply();
        //    //    item.Draw(spriteBatch);
        //    //}
        //    //spriteBatch.End();
        //    //spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);




        //    try
        //    {
        //        spriteBatch.End();

        //        var sprites = loadedMap.returnAllObjectsInCamera(new Rectangle((int)-sceneCamera.X, (int)-sceneCamera.Y, 1366, 768), zoom);
        //        var allUniqueObjs = sprites.GroupBy(x => x.shapeID).Select(g => g.First()).ToList();
        //        List<KeyValuePair<RenderTarget2D, int>> usedRenders = new List<KeyValuePair<RenderTarget2D, int>>();

        //        foreach (var item in allUniqueObjs)
        //        {
        //            if (uniqueObjectRenders.Find(kv => kv.Value == item.shapeID).Equals(default(KeyValuePair<RenderTarget2D, int>)))
        //            {
        //                uniqueObjectRenders.Add(new KeyValuePair<RenderTarget2D, int>(new RenderTarget2D(Game1.graphics.GraphicsDevice, sprites.Find(s => s.shapeID == item.shapeID).spriteGameSize.Width, sprites.Find(s => s.shapeID == item.shapeID).spriteGameSize.Height), item.shapeID));
        //            }

        //            usedRenders.Add(uniqueObjectRenders.Find(kv => kv.Value == item.shapeID));
        //        }


        //        foreach (var item in allUniqueObjs)
        //        {
        //            spriteBatch.GraphicsDevice.SetRenderTarget(usedRenders.Find(kv => kv.Value == item.shapeID).Key);
        //            spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
        //            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);
        //            var sprite = sprites.Find(s => s.shapeID == item.shapeID).ShallowCopy();
        //            sprite.position = Vector2.Zero;
        //            //sprite.position = new Vector2(-64);

        //            sprite.UpdatePosition();

        //            heightMapEffect.Parameters["texHeight"].SetValue((float)sprite.returnProperTexBounds().Height);
        //            heightMapEffect.Parameters["sourcePosY"].SetValue((float)sprite.returnTexSource().Y);
        //            heightMapEffect.Parameters["sourceHeight"].SetValue((float)sprite.returnTexSource().Height);
        //            heightMapEffect.CurrentTechnique.Passes[0].Apply();

        //            heightMapEffect.CurrentTechnique.Passes[0].Apply();

        //            sprite.Draw(spriteBatch);

        //            spriteBatch.End();
        //        }

        //        spriteBatch.GraphicsDevice.SetRenderTarget(null);
        //        spriteBatch.GraphicsDevice.SetRenderTarget(shadowRender);
        //        spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);
        //        spriteBatch.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
        //        foreach (var item in allUniqueObjs)
        //        {
        //            List<BaseSprite> spritesToDraw = sprites.FindAll(s => s.shapeID == item.shapeID);
        //            var renderForItem = usedRenders.Find(kv => kv.Value == item.shapeID).Key;

        //            foreach (var sprite in spritesToDraw)
        //            {
        //                spriteBatch.Draw(renderForItem, sprite.position, renderForItem.Bounds, Color.White);
        //            }
        //        }
        //        //foreach (var item in sprites)
        //        //{

        //        //    var renderForItem = usedRenders.Find(kv => kv.Value == item.shapeID).Key;
        //        //    spriteBatch.Draw(renderForItem, item.position, renderForItem.Bounds, Color.White);


        //        //}

        //        spriteBatch.End();
        //        spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);

        //    }
        //    catch (Exception)
        //    {
        //        spriteBatch.End();
        //        spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);
        //    }
        //}

        public static void TestDrawSomeEffects(SpriteBatch spriteBatch, List<BaseSprite> list)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);



            foreach (var item in list)
            {

                grassEffect.Parameters["texWidth"].SetValue((float)list[0].returnProperTexBounds().Width);
                grassEffect.Parameters["texHeight"].SetValue((float)list[0].returnProperTexBounds().Height);
                grassEffect.Parameters["sourcePosX"].SetValue((float)list[0].returnTexSource().X);
                grassEffect.Parameters["sourcePosY"].SetValue((float)list[0].returnTexSource().Y);
                grassEffect.Parameters["sourceWidth"].SetValue((float)list[0].returnTexSource().Width);
                grassEffect.Parameters["sourceHeight"].SetValue((float)list[0].returnTexSource().Height);


                grassEffect.Parameters["horizontalModifier"].SetValue((item.horizontalGrassModifier) / 3);
                //grassEffect.Parameters["bUsesEffect"].SetValue((horizontalGrassModifier+ GamePlayUtility.ExpertRandomizeSmallNumbers(-0.03f, 0.03f)) / 3);
                //grassEffect.Parameters["horizontalModifier"].SetValue(GamePlayUtility.ExpertRandomize(0, 6));

                grassEffect.CurrentTechnique.Passes[0].Apply();

                item.Draw(spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);
        }

        public static void TestDrawWind(SpriteBatch spriteBatch, RenderTarget2D r2D)
        {
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, null);

            grassEffect.Parameters["texWidth"].SetValue((float)r2D.Bounds.Width);
            grassEffect.Parameters["texHeight"].SetValue((float)r2D.Bounds.Height);
            grassEffect.Parameters["sourcePosX"].SetValue((float)r2D.Bounds.X);
            grassEffect.Parameters["sourcePosY"].SetValue((float)r2D.Bounds.Y);
            grassEffect.Parameters["sourceWidth"].SetValue((float)r2D.Bounds.Width);
            grassEffect.Parameters["sourceHeight"].SetValue((float)r2D.Bounds.Height);


            grassEffect.Parameters["horizontalModifier"].SetValue(loadedMap.mapObjectGroups[0].groupItems[1].horizontalGrassModifier);
            //grassEffect.Parameters["horizontalModifier"].SetValue((horizontalGrassModifier+ GamePlayUtility.ExpertRandomizeSmallNumbers(-0.03f, 0.03f)) / 3);
            //grassEffect.Parameters["horizontalModifier"].SetValue(GamePlayUtility.ExpertRandomize(0, 6));

            grassEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(r2D, r2D.Bounds, Color.White);


            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, CameraScaleMatrix);
        }

        static bool bStartBattle = false;
        public static bool bMayEndCharacterTurn = false;
        static public bool bStartCombatZoom = false;
        static public BaseCharacter battleCaster;
        static public BaseCharacter battleTarget;
        static int battleCastIndex;
        static bool bCasterIsLeft = true;


        public static void StartBattleSoon(BaseCharacter caster, BaseCharacter target, int index = -1)
        {
            if (bStartBattle)
            {
                if (EncounterInfo.currentTurn().bIsPlayerTurnSet && EncounterInfo.currentTurn().charactersInGroup.Contains(caster))
                {

                    BattleGUI.Start(caster, target);
                }
                else
                {
                    BattleGUI.AIStart(caster, target, index);
                }
                bMayEndCharacterTurn = true;
                bStartBattle = false;
                bStartCombatZoom = false;
            }
            else
            {
                bStartCombatZoom = true;
                CombatProcessor.GetRegion().regionBGinfo.SwitchLayer();

                horizontalOffSet = 0;
                preBattleTimePassed = 0;
                bStartCombatZoom = true;
                GameScreenEffect.InitiateZoom(0.8f, 3f, zoom);
                bMayEndCharacterTurn = false;
                battleCaster = caster;
                battleTarget = target;
                battleCastIndex = index;
                BattleGUI.surroundingRbcChars = GetSurroundingChars(target);
                BattleGUI.surroundingGbcChars = GetSurroundingChars(caster);

                var xDistance = caster.position.X - target.position.X;
                var yDistance = caster.position.Y - target.position.Y;

                if (caster.position.X <= target.position.X)
                {
                    bCasterIsLeft = false;
                }
                else if (caster.position.X > target.position.X)
                {
                    bCasterIsLeft = true;
                }

                if (Math.Abs(xDistance) >= Math.Abs(yDistance))
                {
                    if (caster.position.X <= target.position.X)
                    {
                        caster.rotationIndex = (int)BaseCharacter.Rotation.Right;
                        target.rotationIndex = (int)BaseCharacter.Rotation.Left;
                    }
                    else if (caster.position.X > target.position.X)
                    {
                        caster.rotationIndex = (int)BaseCharacter.Rotation.Left;
                        target.rotationIndex = (int)BaseCharacter.Rotation.Right;
                    }
                }
                else if (Math.Abs(xDistance) < Math.Abs(yDistance))
                {
                    if (caster.position.Y <= target.position.Y)
                    {
                        caster.rotationIndex = (int)BaseCharacter.Rotation.Down;
                        target.rotationIndex = (int)BaseCharacter.Rotation.Up;
                    }
                    else if (caster.position.Y > target.position.Y)
                    {
                        caster.rotationIndex = (int)BaseCharacter.Rotation.Up;
                        target.rotationIndex = (int)BaseCharacter.Rotation.Down;
                    }
                }

                Vector2 cameraWidth = new Vector2(1366 / GameProcessor.zoom, 768 / GameProcessor.zoom);
                Vector2 cameraPosition = sceneCamera;

                float minX = 0;
                float width = 0;
                if (caster.position.X <= target.position.X)
                {
                    minX = caster.position.X;
                    width = -caster.position.X + target.position.X;
                }
                else if (caster.position.X <= target.position.X)
                {
                    minX = target.position.X;
                    width = -target.position.X + caster.position.X;
                }
                minX += -16;
                width += 16 * 2 + 64;

                float minY = 0;
                float height = 0;
                if (caster.position.Y <= target.position.Y)
                {
                    minY = caster.position.Y;
                    height = -caster.position.Y + target.position.Y;
                }
                else if (caster.position.Y <= target.position.Y)
                {
                    minY = target.position.Y;
                    height = -target.position.Y + caster.position.Y;
                }
                minY += -16;
                height += 16 * 2 + 64;

                float xMod = 1366f / width;
                float yMod = 768f / height;
                Vector2 adjustedCameraPos = Vector2.Zero;
                Vector2 adjustedCameraSize = Vector2.Zero;

                if (xMod <= yMod)
                {
                    adjustedCameraSize = new Vector2(1366f, 768f) / xMod;
                    adjustedCameraPos = -((caster.position + target.position) / 2 - adjustedCameraSize / 2 + new Vector2(32));
                }

                sceneCamera = -((caster.position + target.position) / 2 - new Vector2(1366 / GameProcessor.zoom / 2, 768 / GameProcessor.zoom / 2) + new Vector2(32));
            }

        }

        internal static List<KeyValuePair<BaseCharacter, bool>> GetSurroundingChars(BaseCharacter target)
        {
            List<BaseCharacter> temp = new List<BaseCharacter>();

            foreach (var group in EncounterInfo.encounterGroups)
            {
                foreach (var item in group.charactersInGroup)
                {
                    if (item.distanceFrom(target) == 1 && item != battleCaster && item != battleTarget)
                    {
                        temp.Add(item);
                    }
                }
            }

            List<KeyValuePair<BaseCharacter, bool>> temp2 = new List<KeyValuePair<BaseCharacter, bool>>();
            var test = EncounterInfo.friendlyTurnSetsFor(target);
            foreach (var item in temp)
            {
                bool bIsFriendly = false;
                foreach (var group in test)
                {
                    bIsFriendly = group.charactersInGroup.Contains(item);
                }
                temp2.Add(new KeyValuePair<BaseCharacter, bool>(item, bIsFriendly));
            }

            return temp2;
        }
    }

    public static class CombatProcessor
    {
        internal static TacticalTextManager tacTextManager;
        static public bool bIsRunning = false;
        static MapRegion region;
        public static MapZone zone;
        static int screenAdjuster = 60 * 2;// (60fps, adjusting over 2 seconds, for 3 seconds would be 60*3 = 180)
        static int screenAdjustCount = 0;
        static float zoomFinal = 0f;
        static Point locationFinal = Point.Zero;
        static public bool bStartPhase1 = false;
        static bool bStartNormalCombat = false;
        static public bool bMainCombat = false;
        public static List<BaseCharacter> heroCharacters = new List<BaseCharacter>();
        public static List<BasicTile> zoneTiles = new List<BasicTile>();
        public static List<Node> allPossibleNodes = new List<Node>();
        public static List<BasicTile> radiusTiles = new List<BasicTile>();
        static public List<BaseCharacter> encounterEnemies = new List<BaseCharacter>();

        static List<TurnSet> groupTurns = new List<TurnSet>();
        static Vector2 newCharPos = new Vector2();
        static int newCharRot = (int)BaseSprite.Rotation.Down;
        static BaseCharacter selectedNonPartyMember = null;
        static public bool bAfterBattleScreen = false;
        public enum afterBattleScreen { Loot = 0, Exp }
        static internal afterBattleScreen currentAfterBattleScreen = afterBattleScreen.Loot;
        public static bool bLostBattle = false;
        static StatDisplayChart statDisplayChart = null;

        public static void Reset()
        {

            EncounterGenerator.cSpawnAmount = default(EncounterGenerator.customSpawnAmount);
            bLostBattle = false;
            EncounterInfo.currentGroupIndex = 0;
            foreach (var item in BattleGUI.TurnEffectsList)
            {
                item.frameIndex = 0;
                item.bMustShow = false;
                item.timePassed = 0;
            }
            EncounterInfo.encounterGroups.Clear();

            bAfterBattleScreen = false;
            PlayerSaveData.heroParty.ForEach(h => h.postCombatLogic());
            region = null;
            zone = null;
            screenAdjustCount = 0;
            zoomFinal = 0f;
            locationFinal = Point.Zero;
            bStartPhase1 = false;
            bStartNormalCombat = false;
            bMainCombat = false;
            heroCharacters.Clear();
            zoneTiles.Clear();
            allPossibleNodes.Clear();
            radiusTiles.Clear();
            encounterEnemies.Clear();
            groupTurns.Clear();
            newCharPos = new Vector2();
            what.Clear();
            test.Clear();
            selectedNonPartyMember = null;
            bIsRunning = false;
            DayLightHandler.bPaused = false;
        }

        public static void InitiateVictory()
        {
            EncounterGenerator.cSpawnAmount = default(EncounterGenerator.customSpawnAmount);
            BattleScriptHandler.Reset();
            bAfterBattleScreen = true;
            currentAfterBattleScreen = afterBattleScreen.Loot;
            //More stuff here
            LootScreen.Start();
        }

        public static void Initialize(BaseSprite controller, MapRegion r)
        {
            IntializeAssets();
            LUA.LuaCombatInfo.GenerateInfo();
            DayLightHandler.bPaused = true;
            bIsRunning = true;
            PlayerController.selectedSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
            PlayerSaveData.heroParty.ForEach(h => h.preCombatLogic());
            //Main controller set to nearest tile
            controller.position = new Vector2(((int)controller.position.X / 64) * 64, ((int)controller.position.Y / 64) * 64);
            controller.spriteGameSize.Location = controller.position.ToPoint();
            controller.UpdatePosition();
            bStartPhase1 = false;
            bStartNormalCombat = false;
            bMainCombat = false;
            //   heroCharacters.Clear();

            region = r;
            region.regionBGinfo.StartCombat();
            zone = r.getZone(controller);
            screenAdjustCount = 0;
            if (zone.zoneTiles.Count == 0)
            {
                zoneTiles = GameProcessor.loadedMap.possibleTilesWithController(zone.returnBoundingZone(), controller);
                zone.zoneTiles = zoneTiles;
            }
            else
            {
                zoneTiles = zone.zoneTiles;
            }


            KeyValuePair<Rectangle, float> camerInfo = zone.returnOptimalCameraAndAngle();
            zoomFinal = (camerInfo.Value - GameProcessor.zoom) / screenAdjuster;
            locationFinal = ((camerInfo.Key.Location.ToVector2() - GameProcessor.sceneCamera) / screenAdjuster).ToPoint();
            encounterEnemies.Clear();
            var temp_loc = MapListUtility.returnMapRadius(2, zoneTiles, PlayerController.selectedSprite);
            EncounterGenerator.Start(region, zone, temp_loc);
            AssignRandomPersonalityToEnemies(encounterEnemies);
            AssignRandomEquipmentToEnemies(encounterEnemies);

            heroCharacters.Clear();
            heroCharacters = new List<BaseCharacter>(PlayerSaveData.heroParty.FindAll(h => h.IsAlive()));

            heroCharacters[0].spriteGameSize = PlayerController.selectedSprite.spriteGameSize;



            //Places all other heroes in a radius near the player controller
            var temp = heroCharacters.FindAll(c => c != PlayerController.selectedSprite);

            List<BasicTile> properLocs = new List<BasicTile>(temp_loc);

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                properLocs.RemoveAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize));
                if (temp.Count != 0)
                {
                    properLocs.RemoveAll(t => t.mapPosition.Intersects(temp[0].spriteGameSize) || t.mapPosition.Contains(temp[0].spriteGameSize));
                }
            }


            PlayerController.selectedSprite.rotationIndex = newCharRot;

            foreach (var item in temp)
            {
                BasicTile randomTile = new BasicTile();
                do
                {
                    randomTile = properLocs[GamePlayUtility.Randomize(0, properLocs.Count - 1)];
                    if (properLocs.Count <= heroCharacters.Count)
                    {
                        break;
                    }
                } while (heroCharacters.Find(h => h.spriteGameSize == randomTile.mapPosition) != null);

                item.spriteGameSize = randomTile.mapPosition;
                item.position = randomTile.mapPosition.Location.ToVector2();
                item.rotationIndex = newCharRot;
                item.UpdatePosition();
            }

            BattleStats.Start(heroCharacters);
            PlayerSetupPhase.Start(MapListUtility.returnValidMapRadius(5, zoneTiles, PlayerController.selectedSprite.position));
            BattleGUI.UpdateGUIElements();
            EncounterInfo.UpdateAllStats();

            BasicMap.GenerateCombatInfo(encounterEnemies, heroCharacters);
            GameProcessor.bUpdateShadows = true;
        }

        private static void IntializeAssets()
        {
            tacTextManager = new TacticalTextManager();

        }

        public static void InitializeFromScript(Vector2 pos, MapRegion r, MapZone mz, LUA.LuaScriptBattle lsb)
        {
            #region setup
            IntializeAssets();
            LUA.LuaCombatInfo.GenerateInfo();
            DayLightHandler.bPaused = true;
            bIsRunning = true;
            PlayerController.selectedSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
            PlayerSaveData.heroParty.ForEach(h => h.preCombatLogic());
            //Main controller set to nearest tile
            BaseSprite controller = PlayerController.selectedSprite;

            controller.position = new Vector2(((int)pos.X * 64), ((int)pos.Y) * 64);
            controller.spriteGameSize.Location = controller.position.ToPoint();
            controller.UpdatePosition();
            bStartPhase1 = false;
            bStartNormalCombat = false;
            bMainCombat = false;
            //   heroCharacters.Clear();

            region = r;
            zone = mz;
            screenAdjustCount = 0;
            if (zone.zoneTiles.Count == 0)
            {
                zoneTiles = GameProcessor.loadedMap.possibleTilesWithController(zone.returnBoundingZone(), controller);
                zone.zoneTiles = zoneTiles;
            }
            else
            {
                zoneTiles = zone.zoneTiles;
            }


            KeyValuePair<Rectangle, float> camerInfo = zone.returnOptimalCameraAndAngle();
            zoomFinal = (camerInfo.Value - GameProcessor.zoom) / screenAdjuster;
            locationFinal = ((camerInfo.Key.Location.ToVector2() - GameProcessor.sceneCamera) / screenAdjuster).ToPoint();
            #endregion

            region.regionBGinfo.StartCombat();
            //encounterEnemies.Clear();
            //controller.position = new Vector2(((int)pos.X / 64) * 64, ((int)pos.Y / 64) * 64);
            //  controller.spriteGameSize.Location = controller.position.ToPoint();
            var temp_loc = MapListUtility.returnMapRadius(2, zoneTiles, PlayerController.selectedSprite);

            //EncounterGenerator.Start(region, zone, temp_loc);
            // AssignRandomPersonalityToEnemies(encounterEnemies);
            //AssignRandomEquipmentToEnemies(encounterEnemies);

            heroCharacters.Clear();
            heroCharacters = new List<BaseCharacter>(PlayerSaveData.heroParty.FindAll(h => h.IsAlive()));

            heroCharacters[0].spriteGameSize = PlayerController.selectedSprite.spriteGameSize;



            //Places all other heroes in a radius near the player controller
            var temp = heroCharacters.FindAll(c => c != PlayerController.selectedSprite);

            List<BasicTile> properLocs = new List<BasicTile>(temp_loc);

            foreach (var item in CombatProcessor.encounterEnemies)
            {
                properLocs.RemoveAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize));
                if (temp.Count != 0)
                {
                    properLocs.RemoveAll(t => t.mapPosition.Intersects(temp[0].spriteGameSize) || t.mapPosition.Contains(temp[0].spriteGameSize));
                }
            }


            PlayerController.selectedSprite.rotationIndex = newCharRot;

            foreach (var item in temp)
            {
                BasicTile randomTile = new BasicTile();
                do
                {
                    randomTile = properLocs[GamePlayUtility.Randomize(0, properLocs.Count - 1)];
                    if (properLocs.Count <= heroCharacters.Count)
                    {
                        break;
                    }
                } while (heroCharacters.Find(h => h.spriteGameSize == randomTile.mapPosition) != null);

                item.spriteGameSize = randomTile.mapPosition;
                item.position = randomTile.mapPosition.Location.ToVector2();
                item.rotationIndex = newCharRot;
                item.UpdatePosition();
            }

            BattleStats.Start(heroCharacters);
            PlayerSetupPhase.Start(MapListUtility.returnValidMapRadius(5, zoneTiles, PlayerController.selectedSprite.position));
            BattleGUI.UpdateGUIElements();
            EncounterInfo.UpdateAllStats();

            BasicMap.GenerateCombatInfo(encounterEnemies, heroCharacters);
            GameProcessor.bUpdateShadows = true;
        }

        private static void AssignRandomEquipmentToEnemies(List<BaseCharacter> group)
        {
            foreach (var item in group)
            {
                int randomInt = GamePlayUtility.Randomize(0, item.enemyWeaponArray.Count);
                item.weapon = GameProcessor.gcDB.gameItems.Find(i => i.itemID == item.enemyWeaponArray[randomInt]) as BaseEquipment;
            }
        }

        private static void AssignRandomPersonalityToEnemies(List<BaseCharacter> group)
        {
            foreach (var bc in group)
            {
                int randomNum = GamePlayUtility.Randomize(0, Enum.GetNames(typeof(AIBehaviour.AIBehaviourType)).Length);
                (bc).Behaviour = (AIBehaviour.AIBehaviourType)randomNum;
            }
        }

        public static void Update(GameTime gameTime)
        {
            //    MapOverlay.Initialize(GameProcessor.overlayBasicTile);
            //  MapOverlay.Update(gameTime);

            //if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            //{
            //    var temp = LootGenerator.GenerateLoot();
            //}
            //GameProcessor.EnableCombatStage();
            //Microsoft.Xna.Framework.Media.MediaPlayer.Volume = 1.0f;

            if (screenAdjustCount < screenAdjuster)
            {
                GameProcessor.sceneCamera += locationFinal.ToVector2();
                GameProcessor.zoom += zoomFinal;
                if (Math.Abs(GameProcessor.zoom - zoomFinal) < .05 && screenAdjustCount < 10)
                {
                    screenAdjustCount = screenAdjuster;
                }
                screenAdjustCount++;
            }
            else if (!bStartPhase1 && !bMainCombat)
            {
                bStartPhase1 = true;
            }

            //if (Keyboard.GetState().IsKeyDown(Keys.H))
            //{
            //    GameProcessor.zoom += 0.02f;
            //    if (PlayerController.selectedSprite != null)
            //    {
            //        GameProcessor.sceneCamera = ((new Vector2(-(PlayerController.selectedSprite.position.X + 32 - 1366 / GameProcessor.zoom / 2), -(PlayerController.selectedSprite.position.Y + 32 - 768 / GameProcessor.zoom / 2))));
            //    }
            //}
            //if (Keyboard.GetState().IsKeyDown(Keys.J))
            //{
            //    GameProcessor.zoom -= 0.02f;
            //    if (PlayerController.selectedSprite != null)
            //    {
            //        GameProcessor.sceneCamera = ((new Vector2(-(PlayerController.selectedSprite.position.X + 32 - 1366 / GameProcessor.zoom / 2), -(PlayerController.selectedSprite.position.Y + 32 - 768 / GameProcessor.zoom / 2))));
            //    }
            //}

            if (bStartPhase1)
            {
                //if (Keyboard.GetState().GetPressedKeys().Length > 0)
                //{
                //    foreach (var key in Game1.actionKeyList)
                //    {
                //        if (!KeyboardMouseUtility.AnyButtonsPressed() && Keyboard.GetState().IsKeyDown(key.assignedActionKey) && key.actionIndentifierString.Equals(Game1.confirmString))
                //        {

                //            groupTurns.Add(new TurnSet(heroCharacters, zone, zoneTiles, true));

                //            groupTurns.Add(new TurnSet(CombatProcessor.encounterEnemies, zone, zoneTiles));
                //            groupTurns[0].targetGroups.Add(groupTurns[1]);
                //            groupTurns[1].targetGroups.Add(groupTurns[0]);

                //            groupTurns[groupTurns.Count - 1].bIsEnemyTurnSet = true;
                //            // groupTurns[groupTurns.Count - 1].targetGroups.Add(groupTurns.Find(gt => gt.bIsPlayerTurnSet));
                //            //AIWarTest();
                //            EncounterInfo.Start(groupTurns);
                //            EncounterInfo.encounterGroups[0].ReGenerateTurn(PlayerController.selectedSprite as BaseCharacter);
                //        }
                //    }
                //}
                PlayerSetupPhase.Update();
            }

            if (PathMoveHandler.bIsBusy)
            {
                PathMoveHandler.Update(gameTime);
            }

            if (!bMainCombat)
            {


                foreach (var item in heroCharacters)
                {
                    item.Update(gameTime);
                    if (!PathMoveHandler.bIsBusy && item.animationIndex != (int)BaseCharacter.CharacterAnimations.Idle)
                    {
                        if (item.smh == null || (item.smh != null && !item.smh.bIsBusy))
                        {
                            item.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                        }
                    }
                }

                foreach (var item in encounterEnemies)
                {
                   // bool bTTT = encounterEnemies[4].statChart == encounterEnemies[9].statChart;
                    item.Update(gameTime);
                    if (!PathMoveHandler.bIsBusy && item.animationIndex != (int)BaseCharacter.CharacterAnimations.Idle)
                    {
                        if (item.smh == null || (item.smh != null && !item.smh.bIsBusy))
                        {
                            item.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                        }
                    }
                }
            }
            else
            {
                DangerTile.Update(gameTime);

                foreach (var item in EncounterInfo.encounterGroups)
                {
                    foreach (var character in item.charactersInGroup)
                    {
                        character.Update(gameTime);
                        if (!PathMoveHandler.bIsBusy && character.animationIndex != (int)BaseCharacter.CharacterAnimations.Idle)
                        {
                            if (character.smh == null || (character.smh != null && !character.smh.bIsBusy))
                            {
                                character.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                            }
                        }
                    }
                }
            }


            tacTextManager.Update(gameTime);



            if (bMainCombat)
            {
                EncounterInfo.Update(gameTime);

                foreach (var item in BattleGUI.TurnEffectsList)
                {
                    if (item.bMustShow)
                    {
                        item.Update(gameTime);
                    }
                }

                if ((selectedNonPartyMember == null || (encounterEnemies.Find(c => c.spriteGameSize.Contains(GameProcessor.EditorCursorPos)) != selectedNonPartyMember)) && encounterEnemies.Find(c => c.spriteGameSize.Contains(GameProcessor.EditorCursorPos)) != null)
                {
                    var temp = encounterEnemies.Find(c => c.spriteGameSize.Contains(GameProcessor.EditorCursorPos));
                    temp.UpdateRadius();
                    statDisplayChart = new StatDisplayChart(temp, temp.position + new Vector2(64, 16));
                }
                selectedNonPartyMember = encounterEnemies.Find(c => c.spriteGameSize.Contains(GameProcessor.EditorCursorPos));
                if (selectedNonPartyMember == null)
                {
                    statDisplayChart = null;
                }

                if (CombatArrowLayout.bMustDraw)
                {
                    CombatArrowLayout.Update(gameTime);
                }
            }

            //if (EncounterInfo.bIsRunning)
            //{
            //    try
            //    {
            //        if (EncounterInfo.encounterGroups[EncounterInfo.currentGroupIndex].bIsPlayerTurnSet)
            //        {

            //        }
            //    }
            //    catch
            //    {
            //    }

            //}

        }

        private static void AIWarTest()
        {
            //groupTurns.Clear();
            //groupTurns.Add(new TurnSet(heroCharacters, zone, zoneTiles, true));
            //groupTurns.Add(new TurnSet(new List<BaseCharacter>(CombatProcessor.encounterEnemies), zone, zoneTiles));
            //var temp = EncounterGenerator.Start(region, zone);
            //AssignRandomEquipmentToEnemies(temp);
            //AssignRandomPersonalityToEnemies(temp);
            //groupTurns.Add(new TurnSet(temp, zone, zoneTiles));
            //groupTurns[groupTurns.Count - 1].bIsEnemyTurnSet = true;
            //groupTurns[groupTurns.Count - 1].targetGroups.Add((groupTurns[groupTurns.Count - 2]));
            //groupTurns[groupTurns.Count - 2].bIsEnemyTurnSet = true;
            //groupTurns[groupTurns.Count - 2].targetGroups.Add((groupTurns[groupTurns.Count - 1]));
            //BattleStats.AddRange(groupTurns[groupTurns.Count - 2].charactersInGroup);
            //BattleStats.AddRange(groupTurns[groupTurns.Count - 1].charactersInGroup);
            //encounterEnemies.AddRange(temp);
        }

        static public List<Node> what = new List<Node>();
        static public List<SpotNode> test = new List<SpotNode>();
        static RenderTarget2D tempRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D UIRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D CombatRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static internal RenderTarget2D CombatGroundRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

        public static void GenerateCombatGroundRender(SpriteBatch spriteBatch)
        {

            RenderGeneration(spriteBatch);


            spriteBatch.GraphicsDevice.SetRenderTarget(CombatGroundRender);
            spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
            //END UI RENDER

            //OW RENDER
            if (bStartPhase1)
            {
                PlayerSetupPhase.Draw(spriteBatch);

                //foreach (var item in heroCharacters.FindAll(h => h.IsAlive()))
                //{
                //    item.Draw(spriteBatch);
                //}

            }
            else if (bMainCombat && !GameProcessor.bStartCombatZoom)
            {
                if (EncounterInfo.currentTurn().selectedCharTurn != null)// && selectedNonPartyMember == null)
                {
                    CharacterTurn ct = EncounterInfo.currentTurn().selectedCharTurn;
                    BattleGUI.DrawCharacterArea(spriteBatch, ct);

                    if (!TurnSet.bSelectingArea)
                    {
                        BattleGUI.DrawCharacterAttackArea(spriteBatch, ct.character.returnOffenseAbilityRange(zoneTiles));

                        if (CombatArrowLayout.bMustDraw)
                        {
                            CombatArrowLayout.Draw(spriteBatch);
                        }

                        if (ct.character.HasAvailableSupportAbilities())
                        {
                            BattleGUI.DrawCharacterSupportArea(spriteBatch, ct.character.returnSupportAbilityRange(zoneTiles));
                        }
                    }

                }

                if (selectedNonPartyMember != null)
                {
                    float opacity = 0.85f;
                    if (EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn().bIsPlayerTurnSet)
                    {
                        opacity = 0.3f;
                    }
                    if (selectedNonPartyMember.latestStatChart.currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB] != 0)
                    {
                        BattleGUI.DrawCharacterArea(spriteBatch, GameProcessor.loadedMap.possibleTilesGameZone(selectedNonPartyMember.maxRadius), opacity);
                    }

                    BattleGUI.DrawCharacterAttackArea(spriteBatch, selectedNonPartyMember.returnOffenseAbilityRange(zoneTiles), opacity);
                    if (selectedNonPartyMember.HasAvailableSupportAbilities())
                    {
                        BattleGUI.DrawCharacterSupportArea(spriteBatch, selectedNonPartyMember.returnSupportAbilityRange(zoneTiles), opacity);
                    }


                }
            }

            if (TurnSet.bSelectingArea && BattleGUI.castAbilityGBC.lama != null)
            {
                BattleGUI.castAbilityGBC.lamaTiles.Clear();
                // spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
                for (int i = 0; i < BattleGUI.castAbilityGBC.lama.areaRelLocs.Count; i++)
                {
                    Point os = new Point();

                    if (BattleGUI.gbc.rotationIndex == (int)BaseCharacter.Rotation.Up)
                    {
                        os = new Point(BattleGUI.castAbilityGBC.lama.areaRelLocs[i].X, BattleGUI.castAbilityGBC.lama.areaRelLocs[i].Y);
                    }
                    else if (BattleGUI.gbc.rotationIndex == (int)BaseCharacter.Rotation.Down)
                    {
                        os = new Point(-BattleGUI.castAbilityGBC.lama.areaRelLocs[i].X, -BattleGUI.castAbilityGBC.lama.areaRelLocs[i].Y);
                    }
                    else if (BattleGUI.gbc.rotationIndex == (int)BaseCharacter.Rotation.Left)
                    {
                        os = new Point(BattleGUI.castAbilityGBC.lama.areaRelLocs[i].Y, -BattleGUI.castAbilityGBC.lama.areaRelLocs[i].X);
                    }
                    else if (BattleGUI.gbc.rotationIndex == (int)BaseCharacter.Rotation.Right)
                    {
                        os = new Point(-BattleGUI.castAbilityGBC.lama.areaRelLocs[i].Y, BattleGUI.castAbilityGBC.lama.areaRelLocs[i].X);
                    }

                    var tile = zoneTiles.Find(t => t.isPointDistanceFrom(BattleGUI.gbc.position, os));
                    if (tile != null)
                    {
                        BattleGUI.castAbilityGBC.lamaTiles.Add(tile);
                        spriteBatch.Draw(Game1.WhiteTex, tile.mapPosition, Color.Red);
                    }
                }
                // spriteBatch.End();
            }

            if (bMainCombat)
            {
                DangerTile.Draw(spriteBatch);
            }


            //tacTextManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        private static void RenderGeneration(SpriteBatch spriteBatch)
        {
            tacTextManager.GenerateRender(spriteBatch);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if (statDisplayChart != null && selectedNonPartyMember != null)
            {
                statDisplayChart.Draw(spriteBatch);

                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(GameProcessor.gameRender);
                spriteBatch.GraphicsDevice.Clear(Color.Black);
            }


            //END OW RENDER       
            //RENDER UI
            if (BattleGUI.bIsRunning)
            {
                spriteBatch.GraphicsDevice.SetRenderTarget(GameProcessor.gameRender);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                BattleGUI.QuickDraw(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);

            }

            //UI PART NO DRAWING
            if (bMainCombat && EncounterInfo.currentTurn().selectedCharTurn != null && !GameProcessor.bStartCombatZoom)
            {
                if (EncounterInfo.currentTurn().bPlayerMustSelectAction && !PathMoveHandler.bIsBusy)
                {
                    if (BattleGUI.returnCurrentPopUpMenuSelection() == (int)BattleGUI.PopUpChoices.Attack)
                    {
                        List<BaseCharacter> test = encounterEnemies.FindAll(c => MapListUtility.ContainsCharacter(EncounterInfo.currentTurn().selectedCharTurn.character.returnOffenseAbilityRange(zoneTiles), c));
                        if (EncounterInfo.currentTurn().selectedCharTurn.character.HasAvailableSupportAbilities())
                        {
                            var charPool = EncounterInfo.currentTurn().charactersInGroup;
                            var bCanTargetSelf = EncounterInfo.currentTurn().selectedCharTurn.character.AbilityList().Find(abi => abi.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT && abi.CanTargetSelf()) != null;
                            test.AddRange(charPool.FindAll(c => (c != EncounterInfo.currentTurn().selectedCharTurn.character || bCanTargetSelf) && MapListUtility.ContainsCharacter(EncounterInfo.currentTurn().selectedCharTurn.character.returnSupportAbilityRange(zoneTiles), c)));

                        }
                        if (EncounterInfo.currentTurn().selectedCharTurn.character.HasAvailableAOEAbilities())
                        {
                            test.Add(EncounterInfo.currentTurn().selectedCharTurn.character);
                        }
                        BattleGUI.selectableTargetsWithinRange = test;
                    }
                }
            }






            if (false && !GameProcessor.bStartCombatZoom && bMainCombat)
            {
                foreach (var item in heroCharacters)
                {
                    if (item.bIsAlive && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsPlayerTurnSet)
                    {
                        if (EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == item).bIsCompleted)
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


                    BattleGUI.DrawSmallBars(spriteBatch, item, heroCharacters.IndexOf(item), BattleGUI.characterType.Hero);
                    if (item.bIsAlive && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsPlayerTurnSet)
                    {
                        if (EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == item).bIsCompleted)
                        {
                            spriteBatch.DrawString(Game1.defaultFont, "E", item.position + new Vector2(45, 45), Color.White);
                        }
                    }
                }

                foreach (var enemy in encounterEnemies)
                {

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

                    BattleGUI.DrawSmallBars(spriteBatch, enemy, encounterEnemies.IndexOf(enemy), BattleGUI.characterType.Enemy);

                    if (enemy.bIsAlive && EncounterInfo.encounterGroups.Count != 0 && EncounterInfo.currentTurn() != null && EncounterInfo.currentTurn().bIsEnemyTurnSet)
                    {
                        if (EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == enemy) != null && EncounterInfo.currentTurn().groupTurnSet.Find(gts => gts.character == enemy).bIsCompleted)
                        {
                            spriteBatch.DrawString(Game1.defaultFont, "E", enemy.position + new Vector2(45, 45), Color.White);
                        }
                    }
                }
            }




            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(tempRender);
            spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            // spriteBatch.Draw(CombatRender, CombatRender.Bounds, Color.White);


            spriteBatch.End();

            if (bMainCombat && !BattleGUI.bIsRunning && !GameProcessor.bStartCombatZoom)
            {

                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(UIRender);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                BattleGUI.DrawMapOverlay(spriteBatch);
                spriteBatch.End();

                if (statDisplayChart != null && selectedNonPartyMember != null)
                {
                    statDisplayChart.FinalDraw(spriteBatch, GameProcessor.CameraScaleMatrixNoZooming);
                }

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);

            }



            if (bMainCombat && EncounterInfo.currentTurn().selectedCharTurn != null && !GameProcessor.bStartCombatZoom && !TurnSet.bSelectingArea)
            {
                if (EncounterInfo.currentTurn().bPlayerMustSelectAction && !PathMoveHandler.bIsBusy)
                {
                    if (BattleGUI.returnCurrentPopUpMenuSelection() == (int)BattleGUI.PopUpChoices.Attack)
                    {
                        try
                        {
                            BattleGUI.DrawVerticalPointerAbove(spriteBatch);
                        }
                        catch
                        {
                        }

                    }

                    BattleGUI.DrawPopUpMenu(spriteBatch);

                }
            }

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrixNoZooming);
            var beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale + new Vector2(-GameProcessor.sceneCamera.X, -GameProcessor.sceneCamera.Y);
            //UI RENDER
            BattleGUI.DrawCursor(spriteBatch, beep, GameProcessor.zoom);

            //END UI RENDER
            spriteBatch.End();

            if (!GameProcessor.bStartCombatZoom && bMainCombat)
            {
                //GameProcessor.UIRenders.Add(tacTextManager.getRender());
                GameProcessor.UIRenders.Add(UIRender);
            }


            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(GameProcessor.gameRender);
            spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(GameProcessor.mapRender, GameProcessor.mapRender.Bounds, Color.White);

            UIDraw(spriteBatch);
            //spriteBatch.Draw(tempRender, tempRender.Bounds, Color.White);
            //spriteBatch.Draw(CombatGroundRender, CombatGroundRender.Bounds, Color.White);
            spriteBatch.End();


            //END UI

        }

        private static void UIDraw(SpriteBatch spriteBatch)
        {
            if (bStartPhase1)
            {
                GameText startBattleText = new GameText("Press confirm to start battle!");
                TextUtility.Draw(spriteBatch, startBattleText.getText(), Game1.defaultFont, new Rectangle(150, 500, 400, 80), TextUtility.OutLining.Center, Color.White, 1f, true, default(Matrix), Color.Black, false);
            }

            if (bMainCombat)
            {
                if (EncounterInfo.currentTurn().bPlayerMustSelectAction && BattleGUI.returnCurrentPopUpMenuSelection() == (int)BattleGUI.PopUpChoices.Attack)
                {

                }
            }
        }

        internal static RenderTarget2D getTacticalTextPopUpRender()
        {
            return tacTextManager.getRender();
        }

        internal static void StartMainCombat()
        {

            groupTurns.Add(new TurnSet(heroCharacters, zone, zoneTiles, true));

            groupTurns.Add(new TurnSet(CombatProcessor.encounterEnemies, zone, zoneTiles));
            groupTurns[0].targetGroups.Add(groupTurns[1]);
            groupTurns[1].targetGroups.Add(groupTurns[0]);

            groupTurns[groupTurns.Count - 1].bIsEnemyTurnSet = true;
            // groupTurns[groupTurns.Count - 1].targetGroups.Add(groupTurns.Find(gt => gt.bIsPlayerTurnSet));
            //AIWarTest();
            EncounterInfo.Start(groupTurns);
            EncounterInfo.encounterGroups[0].ReGenerateTurn(PlayerController.selectedSprite as BaseCharacter);


            PlayerSetupPhase.End();
            bStartPhase1 = false;
            bMainCombat = true;
        }


        internal static void UpdateFinalScreen(GameTime gameTime)
        {

            switch (currentAfterBattleScreen)
            {
                case afterBattleScreen.Loot:
                    if (LootScreen.IsRunning())
                    {
                        LootScreen.Update(gameTime);
                    }
                    break;
                case afterBattleScreen.Exp:
                    ExpGainScreen.Update(gameTime);
                    break;
                default:
                    break;
            }
        }

        internal static void DrawFinalScreen(SpriteBatch spriteBatch)
        {
            spriteBatch.End();

            switch (currentAfterBattleScreen)
            {
                case afterBattleScreen.Loot:
                    if (LootScreen.IsRunning())
                    {
                        GameProcessor.UIRenders.Clear();
                        GameProcessor.UIRenders.Add(LootScreen.Draw(spriteBatch));
                    }
                    break;
                case afterBattleScreen.Exp:
                    GameProcessor.UIRenders.Clear();
                    GameProcessor.UIRenders.Add(ExpGainScreen.Draw(spriteBatch));
                    break;
                default:
                    break;
            }


            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, GameProcessor.CameraScaleMatrix);
        }

        internal static void LeaveCombat()
        {
            region.LeaveCombat();
            CombatProcessor.Reset();
            GameProcessor.EnableNonCombatStage();
            if (PlayerController.selectedSprite == null)
            {
                if ((GameProcessor.mainControllerBeforeCombat is BaseCharacter) && (GameProcessor.mainControllerBeforeCombat as BaseCharacter).IsAlive())
                {
                    PlayerController.selectedSprite = GameProcessor.mainControllerBeforeCombat;
                }
                else
                {
                    PlayerController.selectedSprite = PlayerSaveData.heroParty[GamePlayUtility.Randomize(0, PlayerSaveData.heroParty.Count - 1)];
                }
            }
            GameProcessor.GenerateCamera(PlayerController.selectedSprite);

            try
            {
                Vector2 pos = PlayerController.selectedSprite.position;
                var oi = MapChunk.consideredSprites.FindAll(s => s.objectType == objectInfo.type.Character).Find(s => s.obj == GameProcessor.mainControllerBeforeCombat);
                var oil = MapChunk.consideredSprites.FindAll(s => s.objectType == objectInfo.type.Character);
                if (oi == null)
                {
                    var tempTryFind = MapChunk.consideredSprites.Find(cs => cs.obj == GameProcessor.mainControllerBeforeCombat);
                    GameProcessor.loadedMap.ForceCheckChunksToConsider();
                    oi = MapChunk.consideredSprites.FindAll(s => s.objectType == objectInfo.type.Character).Find(s => s.obj == PlayerController.selectedSprite);
                }
                PlayerController.selectedSprite = oi.obj as BaseSprite;
                oi.obj = PlayerController.selectedSprite;
                PlayerController.selectedSprite.position = pos;
                PlayerController.selectedSprite.UpdatePosition();
                GameProcessor.cameraFollowTarget = PlayerController.selectedSprite;
            }
            catch
            {

            }
        }

        internal static void UpdateBattleLost(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Game1.startScreen = new StartScreen();
                Reset();
                bIsRunning = false;
            }
        }

        internal static void DrawLostBattle(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(tempRender);
            spriteBatch.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred);
            TextUtility.Draw(spriteBatch, "Lost Battle", BattleGUI.testSF48, new Rectangle((1366 - 600) / 2, 100, 600, 220), TextUtility.OutLining.Center, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            GameProcessor.UIRenders.Add(tempRender);
        }

        internal static MapRegion GetRegion()
        {
            return region;
        }
    }
}
