using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBAGW.Utilities;
using TBAGW.Scenes;
using TBAGW.Utilities.Actions;
using System;
using System.Collections.Generic;
using TBAGW.Scenes.MainGame;
using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.GUI;
using TBAGW.Utilities.GamePlay.Spells;
using TBAGW.Utilities.Sound.BG;
using TBAGW.Utilities.GamePlay.Characters;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.SriptProcessing;
using Microsoft.Xna.Framework.Content;
using TBAGW.Utilities.Particles;
using System.Threading;
using TBAGW.Utilities.Editor.IO;
using System.Linq;

namespace TBAGW
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        static public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static public SpriteFont defaultFont;
        static public List<ActionKey> actionKeyList = new List<ActionKey>();
        static public List<Actions> actionList = new List<Actions>();
        public enum Screens { MainMenu = 0, MainGameStart, MainGameContinue, OWGame, BGame, Options, ExitGame, Editor };
        List<Scene> scenes = new List<Scene>();
        MainMenu mainMenu = new MainMenu();
        static public Texture2D hitboxHelp;
        static public Texture2D selectionTexture;
        static public Texture2D mapBorderHelp;
        static public bool bIsActive = true;
        static public ContentManager contentManager;
        static public String root = Environment.CurrentDirectory;
        static public String rootContent = Environment.CurrentDirectory + @"\Content\";
        static public String rootContentExtra = Environment.CurrentDirectory + @"\Content\Mods\";
        static public String rootTBAGW = Environment.CurrentDirectory + @"\TBAGW\";
        static public String rootGClasses = rootTBAGW + @"Characters\Classes\";
        static public bool bIsDebug = false;
        static public RenderTarget2D gameRender;
        static public RenderTarget2D UIRender;
        static public Vector2 monitorSize;
        static public bool bUsingKeyboardMouse = true;
        static public StartScreen startScreen;
        static public bool bMainGame = true;
        static internal SongManager songManager;
        static internal Game1 gameRef;

        //test
        //test2
        //test3
        public Game1()
            : base()
        {
            if (true)
            {
#if STEAMWORKS_WIN
                try
                {
                    try
                    {
                        ///Ignore any System.EntryPointNotFoundException
                        ///or System.DllNotFoundException exceptions here
                        NVClass.NvAPI_Initialize();
                    }
                    catch
                    { }
                    goto ignoreSW;
                    if (!Steamworks.SteamAPI.IsSteamRunning() || Steamworks.SteamAPI.RestartAppIfNecessary((Steamworks.AppId_t)480))
                    {
                        Console.WriteLine("Requires restart? " + Steamworks.SteamAPI.RestartAppIfNecessary((Steamworks.AppId_t)480));
                        System.Windows.Forms.MessageBox.Show("This version requires Steam to be running, please restart with Steam active. Thank you.");
                        Dispose();
                        Exit();
                    }
                    Steamworks.SteamAPI.Init();
                    Steamworks.SteamAPI.RestartAppIfNecessary((Steamworks.AppId_t)480);
                    Console.WriteLine("Requires restart? " + Steamworks.SteamAPI.RestartAppIfNecessary((Steamworks.AppId_t)480));
                    // Console.WriteLine(Steamworks.SteamAPI.InitSafe());
                    // Steamworks.SteamAPI.Shutdown();
                    // Steamworks.SteamAPI.Init();

                    Console.WriteLine("You are working in: STEAMWORKS_WIN");

                    Console.WriteLine("Is Steam running? " + Steamworks.SteamAPI.IsSteamRunning());
                    //  Steamworks.SteamAPI.InitSafe();
                    Console.WriteLine("Trying: " + Steamworks.SteamFriends.GetPersonaName());
                }
                catch (Exception e)
                {
                    Console.WriteLine("BEEP\n" + e);
                }
#endif
            }
        ignoreSW: { }
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            contentManager = this.Content;
            songManager = new SongManager(this);
#if DEBUG
            bIsDebug = true;
#endif

            Console.WriteLine(bIsDebug);
            Console.WriteLine(-1 / 25);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            //  Console.WriteLine(SteamFriends.GetPersonaName());


            base.Initialize();
            initiateActions();

            ResolutionUtility.bMouseIsVisible = this.IsMouseVisible;
            hitboxHelp = Content.Load<Texture2D>(@"Graphics\HitBoxHelp");
            selectionTexture = Content.Load<Texture2D>(@"Graphics\MainMenu\TestRectangle");
            mapBorderHelp = Content.Load<Texture2D>(@"Graphics\Tiles\Basic\BorderTest");
            WhiteTex = Content.Load<Texture2D>(@"White");
            scenes.Insert((int)(Screens.MainMenu), new MainMenu());
            scenes.Insert((int)(Screens.MainGameStart), new NewGameScene());
            scenes.Insert((int)(Screens.MainGameContinue), new ContinueGameScene());
            scenes.Insert((int)(Screens.OWGame), new OWGame());
            scenes.Insert((int)(Screens.BGame), new MainGame());
            scenes.Insert((int)(Screens.Options), new Options());
            scenes.Insert((int)(Screens.ExitGame), new Scene());
            scenes.Insert((int)(Screens.Editor), new Editor());


            CursorUtility.Initialize(this);
            MainGUI.Initialize(this);


            if (!bRunEditMode)
            {
                scenes[SceneUtility.currentScene].Initialize(this);
            }




            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferWidth = this.GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = this.GraphicsDevice.DisplayMode.Height;
            //   this.Window.Position = new Point(100, 100);
            //graphics.IsFullScreen = true;
            this.IsMouseVisible = false;
            graphics.ApplyChanges();
            LoadAssets();
            this.Window.AllowUserResizing = false;
            //Window.IsBorderless = true;
            Window.AllowAltF4 = true;
            //Window.Location = Point.Zero;
            //900,506
            ResolutionUtility.WindowSizeBeforeFullScreen = new Vector2(1366, 768);
            ResolutionUtility.AdjustResolution(1366, 768, graphics);
            gameRender = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            gameRender = new RenderTarget2D(GraphicsDevice, 1366, 768);
            UIRender = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            monitorSize = new Vector2(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            Console.WriteLine("Primary Monitor Size: " + monitorSize);

            EditorFileWriter.LoadSaveSettings();
            startScreen = new StartScreen();
#if !DEBUG
            // GameProcessor.Launch();
#endif
            // GameProcessor.Launch();
        }

        private void LoadAssets()
        {
            //SpellsAssetLoader.loadAssets(this);
            //CharacterAssetsLoader.loadAssets(this);
        }

        static public String openMenuString = "Menu";
        static public String confirmString = "Confirm";
        static public String cancelString = "Cancel";
        static public String moveUpString = "Up";
        static public String moveDownString = "Down";
        static public String moveLeftString = "Left";
        static public String moveRightString = "Right";
        static public String cameraZoomIn = "Camera zoom in";
        static public String cameraZoomOut = "Camera zoom out";
        static public String cameraDefaultZoom = "Camera zoom reset";
        static public String pauseString = "Pause";
        static public String debugInfoString = "Debug";
        static public String cameraMoveUpString = "Camera up";
        static public String cameraMoveDownString = "Camera down";
        static public String cameraMoveLeftString = "Camera left";
        static public String cameraMoveRightString = "Camera right";
        static public String tabKeyString = "Tab Function";
        static public String QKey1String = "Hotkey 1";
        static public String QKey2String = "Hotkey 2";
        static public String QKey3String = "Hotkey 3";
        static public String QKey4String = "Hotkey 4";
        static public String QKey5String = "Hotkey 5";
        static public String QKey6String = "Hotkey 6";
        static public String QKey7String = "Hotkey 7";
        static public String QKey8String = "Hotkey 8";
        static public String QKey9String = "Hotkey 9";
        static public String EditorString = "GoToEditor";
        static public String SettingsMenu = "Settings";
        static internal Texture2D WhiteTex;

        private void initiateActions()
        {
            List<Actions> orderedList = new List<Actions>();

            Actions zoomInAction = new Actions();
            zoomInAction.Initialize(cameraZoomIn, actionKeyList);
            actionList.Add(zoomInAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.LeftShoulder;
            actionList[actionList.Count - 1].defaultKey = Keys.Z;
            actionList[actionList.Count - 1].bUsed = true;

            Actions zoomOutAction = new Actions();
            zoomOutAction.Initialize(cameraZoomOut, actionKeyList);
            actionList.Add(zoomOutAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.RightShoulder;
            actionList[actionList.Count - 1].defaultKey = Keys.C;
            actionList[actionList.Count - 1].bUsed = true;

            Actions zoomDefaultAction = new Actions();
            zoomDefaultAction.Initialize(cameraDefaultZoom, actionKeyList);
            actionList.Add(zoomDefaultAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.LeftStick;
            actionList[actionList.Count - 1].defaultKey = Keys.X;
            actionList[actionList.Count - 1].bUsed = true;

            Actions confirmAction = new Actions();
            confirmAction.Initialize(confirmString, actionKeyList);
            actionList.Add(confirmAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.A;
            actionList[actionList.Count - 1].defaultKey = Keys.Enter;
            actionList[actionList.Count - 1].bUsed = true;

            Actions cancelAction = new Actions();
            cancelAction.Initialize(cancelString, actionKeyList);
            actionList.Add(cancelAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.B;
            actionList[actionList.Count - 1].defaultKey = Keys.Back;
            actionList[actionList.Count - 1].bUsed = true;

            Actions moveDownAction = new Actions();
            moveDownAction.Initialize(moveDownString, actionKeyList);
            actionList.Add(moveDownAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.LeftThumbstickDown;
            actionList[actionList.Count - 1].defaultKey = Keys.S;
            actionList[actionList.Count - 1].bUsed = true;

            Actions moveLeftAction = new Actions();
            moveLeftAction.Initialize(moveLeftString, actionKeyList);
            actionList.Add(moveLeftAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.LeftThumbstickLeft;
            actionList[actionList.Count - 1].defaultKey = Keys.A;
            actionList[actionList.Count - 1].bUsed = true;

            Actions moveRightAction = new Actions();
            moveRightAction.Initialize(moveRightString, actionKeyList);
            actionList.Add(moveRightAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.LeftThumbstickRight;
            actionList[actionList.Count - 1].defaultKey = Keys.D;
            actionList[actionList.Count - 1].bUsed = true;

            Actions moveUpAction = new Actions();
            moveUpAction.Initialize(moveUpString, actionKeyList);
            actionList.Add(moveUpAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.LeftThumbstickUp;
            actionList[actionList.Count - 1].defaultKey = Keys.W;
            actionList[actionList.Count - 1].bUsed = true;

            Actions pauseAction = new Actions();
            pauseAction.Initialize(pauseString, actionKeyList);
            actionList.Add(pauseAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.Y;
            actionList[actionList.Count - 1].defaultKey = Keys.P;
            actionList[actionList.Count - 1].bUsed = true;

            Actions debugAction = new Actions();
            debugAction.Initialize(debugInfoString, actionKeyList);
            actionList.Add(debugAction);


            Actions cameraMoveUpAction = new Actions();
            cameraMoveUpAction.Initialize(cameraMoveUpString, actionKeyList);
            actionList.Add(cameraMoveUpAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.RightThumbstickUp;
            actionList[actionList.Count - 1].defaultKey = Keys.Up;
            actionList[actionList.Count - 1].bUsed = true;

            Actions cameraMoveDownAction = new Actions();
            cameraMoveDownAction.Initialize(cameraMoveDownString, actionKeyList);
            actionList.Add(cameraMoveDownAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.RightThumbstickDown;
            actionList[actionList.Count - 1].defaultKey = Keys.Down;
            actionList[actionList.Count - 1].bUsed = true;

            Actions cameraMoveLeftAction = new Actions();
            cameraMoveLeftAction.Initialize(cameraMoveLeftString, actionKeyList);
            actionList.Add(cameraMoveLeftAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.RightThumbstickLeft;
            actionList[actionList.Count - 1].defaultKey = Keys.Left;
            actionList[actionList.Count - 1].bUsed = true;

            Actions cameraMoveRightAction = new Actions();
            cameraMoveRightAction.Initialize(cameraMoveRightString, actionKeyList);
            actionList.Add(cameraMoveRightAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.RightThumbstickRight;
            actionList[actionList.Count - 1].defaultKey = Keys.Right;
            actionList[actionList.Count - 1].bUsed = true;

            Actions openMenuAction = new Actions();
            openMenuAction.Initialize(openMenuString, actionKeyList);
            actionList.Add(openMenuAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.Start;
            actionList[actionList.Count - 1].defaultKey = Keys.Space;
            actionList[actionList.Count - 1].bUsed = true;

            Actions cameraMoveCenterAction = new Actions();
            cameraMoveCenterAction.Initialize(tabKeyString, actionKeyList);
            actionList.Add(cameraMoveCenterAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.LeftTrigger;
            actionList[actionList.Count - 1].defaultKey = Keys.Tab;
            actionList[actionList.Count - 1].bUsed = true;

            Actions QKey1Action = new Actions();
            QKey1Action.Initialize(QKey1String, actionKeyList);
            actionList.Add(QKey1Action);

            Actions QKey2Action = new Actions();
            QKey2Action.Initialize(QKey2String, actionKeyList);
            actionList.Add(QKey2Action);

            Actions QKey3Action = new Actions();
            QKey3Action.Initialize(QKey3String, actionKeyList);
            actionList.Add(QKey3Action);

            Actions QKey4Action = new Actions();
            QKey4Action.Initialize(QKey4String, actionKeyList);
            actionList.Add(QKey4Action);

            Actions QKey5Action = new Actions();
            QKey5Action.Initialize(QKey5String, actionKeyList);
            actionList.Add(QKey5Action);

            Actions QKey6Action = new Actions();
            QKey6Action.Initialize(QKey6String, actionKeyList);
            actionList.Add(QKey6Action);

            Actions QKey7Action = new Actions();
            QKey7Action.Initialize(QKey7String, actionKeyList);
            actionList.Add(QKey7Action);

            Actions QKey8Action = new Actions();
            QKey8Action.Initialize(QKey8String, actionKeyList);
            actionList.Add(QKey8Action);

            Actions QKey9Action = new Actions();
            QKey9Action.Initialize(QKey9String, actionKeyList);
            actionList.Add(QKey9Action);

            Actions EditorAction = new Actions();
            EditorAction.Initialize(EditorString, actionKeyList);
            actionList.Add(EditorAction);

            Actions SettingsMenuAction = new Actions();
            SettingsMenuAction.Initialize(SettingsMenu, actionKeyList);
            actionList.Add(SettingsMenuAction);
            actionList[actionList.Count - 1].defaultButton = Buttons.Back;
            actionList[actionList.Count - 1].defaultKey = Keys.Enter;
            actionList[actionList.Count - 1].bUsed = true;

            orderedList.Add(confirmAction);
            orderedList.Add(cancelAction);
            orderedList.Add(openMenuAction);
            orderedList.Add(SettingsMenuAction);
            orderedList.Add(pauseAction);

            orderedList.Add(moveUpAction);
            orderedList.Add(moveDownAction);
            orderedList.Add(moveLeftAction);
            orderedList.Add(moveRightAction);

            orderedList.Add(cameraMoveUpAction);
            orderedList.Add(cameraMoveDownAction);
            orderedList.Add(cameraMoveLeftAction);
            orderedList.Add(cameraMoveRightAction);

            orderedList.Add(zoomInAction);
            orderedList.Add(zoomOutAction);
            orderedList.Add(zoomDefaultAction);

            orderedList.Add(cameraMoveCenterAction);

            actionList.Except(orderedList).ToList().ForEach(a => orderedList.Add(a));
            actionList = orderedList;

            foreach (ActionKey key in actionKeyList)
            {
                key.assignUnadjustableKey(Keys.Enter, confirmString, 0, true);
                key.assignUnadjustableKey(Keys.Escape, SettingsMenu, 0, true);
                key.assignUnadjustableKey(Keys.Back, cancelString, 0, true);
                key.assignUnadjustableKey(Keys.P, pauseString, 0, true);
                key.assignUnadjustableKey(Keys.W, moveUpString, 0, true);
                key.assignUnadjustableKey(Keys.S, moveDownString, 0, true);
                key.assignUnadjustableKey(Keys.A, moveLeftString, 0, true);
                key.assignUnadjustableKey(Keys.D, moveRightString, 0, true);

                key.assignUnadjustableKey(Keys.NumPad8, moveUpString, 1, true);
                key.assignUnadjustableKey(Keys.NumPad2, moveDownString, 1, true);
                key.assignUnadjustableKey(Keys.NumPad4, moveLeftString, 1, true);
                key.assignUnadjustableKey(Keys.NumPad6, moveRightString, 1, true);

                key.assignUnadjustableKey(Keys.I, debugInfoString, 0, true);
                key.assignUnadjustableKey(Keys.Up, cameraMoveUpString, 0, true);
                key.assignUnadjustableKey(Keys.Down, cameraMoveDownString, 0, true);
                key.assignUnadjustableKey(Keys.Left, cameraMoveLeftString, 0, true);
                key.assignUnadjustableKey(Keys.Right, cameraMoveRightString, 0, true);
                key.assignUnadjustableKey(Keys.Space, openMenuString, 0, true);
                key.assignUnadjustableKey(Keys.Tab, tabKeyString, 0, true);
                key.assignUnadjustableKey(Keys.D1, QKey1String, 0, true);
                key.assignUnadjustableKey(Keys.D2, QKey2String, 0, true);
                key.assignUnadjustableKey(Keys.D3, QKey3String, 0, true);
                key.assignUnadjustableKey(Keys.D4, QKey4String, 0, true);
                key.assignUnadjustableKey(Keys.D5, QKey5String, 0, true);
                key.assignUnadjustableKey(Keys.D6, QKey6String, 0, true);
                key.assignUnadjustableKey(Keys.D7, QKey7String, 0, true);
                key.assignUnadjustableKey(Keys.D8, QKey8String, 0, true);
                key.assignUnadjustableKey(Keys.D9, QKey9String, 0, true);
                key.assignUnadjustableKey(Keys.F1, EditorString, 0, true);

                key.assignUnadjustableKey(Keys.Z, cameraZoomIn, 0, true);
                key.assignUnadjustableKey(Keys.C, cameraZoomOut, 0, true);
                key.assignUnadjustableKey(Keys.X, cameraDefaultZoom, 0, true);


                key.assignButton(Buttons.A, confirmString, true);
                key.assignButton(Buttons.Y, pauseString, true);
                key.assignButton(Buttons.LeftThumbstickUp, moveUpString, true);
                key.assignButton(Buttons.LeftThumbstickDown, moveDownString, true);
                key.assignButton(Buttons.LeftThumbstickLeft, moveLeftString, true);
                key.assignButton(Buttons.LeftThumbstickRight, moveRightString, true);
                key.assignButton(Buttons.B, cancelString, true);
                key.assignButton(Buttons.RightThumbstickUp, cameraMoveUpString, true);
                key.assignButton(Buttons.RightThumbstickDown, cameraMoveDownString, true);
                key.assignButton(Buttons.RightThumbstickLeft, cameraMoveLeftString, true);
                key.assignButton(Buttons.RightThumbstickRight, cameraMoveRightString, true);
                key.assignButton(Buttons.Start, openMenuString, true);
                key.assignButton(Buttons.LeftTrigger, tabKeyString, true);
                key.assignButton(Buttons.Back, SettingsMenu, true);
                key.assignButton(Buttons.LeftShoulder, cameraZoomIn, true);
                key.assignButton(Buttons.RightShoulder, cameraZoomOut, true);
                key.assignButton(Buttons.LeftStick, cameraDefaultZoom, true);
                key.assignButton(Buttons.LeftTrigger, tabKeyString, true);
                key.assignButton(Buttons.RightTrigger, tabKeyString, true);
            }
        }

        static public Effect testEffect;
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            testEffect = Content.Load<Effect>(@"FX\ShapeEffect");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            defaultFont = Content.Load<SpriteFont>("Fonts\\DefaultFont");


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        internal static void ExitGame()
        {
            bExited = true;
        }

        bool bRunEditMode = false;
        public object ScriptTestingScene { get; private set; }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            gameRef = this;
            if (bExited)
            {
                Exit();
            }

            if (bIsDebug && TestEnvironment.bDoTest && false)
            {
                TestEnvironment.Test();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.M) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                ToggleMouse();
            }

            //Console.WriteLine("Hello from Update");

            if (Keyboard.GetState().IsKeyDown(Keys.End) && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                Console.WriteLine(Steamworks.SteamAPI.InitSafe());
            }

            SceneUtility.UpdateTransform();
            SceneUtility.Update(gameTime);
            SceneUtility.scenes = scenes;

            ResolutionUtility.bMouseIsVisible = this.IsMouseVisible;
            ResolutionUtility.mousePos.X = Mouse.GetState().X;
            ResolutionUtility.mousePos.Y = Mouse.GetState().Y;
            ResolutionUtility.KeyHandler(graphics, gameTime);
            //MouseMustStayWithinGame();
            buttonPressUtility.Update(gameTime);

            if (ResolutionUtility.stdScale != new Vector2(1))
            {
                this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            }

            bIsActive = this.IsActive;
            //if (!bRunEditMode)
            //{
            //    MusicBGPlayer.Update(gameTime);
            //}

            //CursorUtility.update(gameTime);


            if (!bRunEditMode && !StartScreen.bIsRunning)
            {
                scenes[SceneUtility.currentScene].Update(gameTime, this);
            }
            else if (StartScreen.bIsRunning)
            {
                startScreen.Update(gameTime);
            }
            //else
            //{
            //    ScripTestingScene.Update(gameTime, this);
            //}

            if (SceneUtility.currentScene != (int)Screens.Editor)
            {
                buttonPressUtility.bPressed = true;
                Console.Out.WriteLine("You called the editor!");
                (scenes[(int)Screens.Editor] as Editor).preScreen = SceneUtility.prevScene;
                SceneUtility.ChangeScene((int)Screens.Editor);
                // SceneUtility.prevScene = SceneUtility.currentScene;
                //  SceneUtility.currentScene = (int)Screens.Editor;
                (scenes[(int)Screens.Editor] as Editor).scenes = scenes;
                (scenes[(int)Screens.Editor] as Editor).Start(this);
            }

            KeyboardMouseUtility.Update(this);

            //if (!bRunEditMode)
            //{

            //    if (SceneUtility.prevScene != SceneUtility.currentScene)
            //    {
            //        if (!scenes[SceneUtility.currentScene].bIsInitialized)
            //        {
            //            Console.Out.WriteLine("Called for: " + scenes[SceneUtility.currentScene]);
            //            scenes[SceneUtility.currentScene].Initialize(this);
            //        }
            //        else
            //        {
            //            scenes[SceneUtility.currentScene].Reload();
            //        }

            //    }

            //    if (SceneUtility.currentScene == (int)(Game1.Screens.ExitGame))
            //    {

            //        foreach (Scene scene in scenes)
            //        {
            //            scene.UnloadContent(this);
            //        }

            //        Exit();
            //    }

            //    if (buttonPressUtility.isPressedSub(EditorString) && !buttonPressUtility.bPressed && SceneUtility.currentScene != (int)Screens.Editor)
            //    {
            //        buttonPressUtility.bPressed = true;
            //        Console.Out.WriteLine("You called the editor!");
            //        (scenes[(int)Screens.Editor] as Editor).preScreen = SceneUtility.prevScene;
            //        SceneUtility.ChangeScene((int)Screens.Editor);
            //        // SceneUtility.prevScene = SceneUtility.currentScene;
            //        //  SceneUtility.currentScene = (int)Screens.Editor;
            //        (scenes[(int)Screens.Editor] as Editor).scenes = scenes;
            //        (scenes[(int)Screens.Editor] as Editor).Start(this);
            //    }
            //    else if (buttonPressUtility.isPressedSub(EditorString) && !buttonPressUtility.bPressed && SceneUtility.currentScene == (int)Screens.Editor)
            //    {
            //        buttonPressUtility.bPressed = true;
            //        Console.Out.WriteLine("You left the editor!");
            //        //TODO set the actual prevscreen before entering the editor
            //        SceneUtility.currentScene = SceneUtility.prevScene;
            //        SceneUtility.prevScene = (scenes[(int)Screens.Editor] as Editor).preScreen;
            //        (scenes[(int)Screens.Editor] as Editor).ResetBackToGame(this);
            //    }
            //}




            base.Update(gameTime);

        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //ResolutionUtility.toggleFullscreen();
            GraphicsDevice.Clear(Color.Red);
            GraphicsDevice.SetRenderTarget(gameRender);
            //   spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            if (!bRunEditMode && !StartScreen.bIsRunning || (StartScreen.bIsRunning && startScreen.bFinishedLoading))
            {

                scenes[SceneUtility.currentScene].Draw(gameTime, spriteBatch);

                TextUtility.Draw(spriteBatch, "Pre-alpha combat demo v0.1.6", defaultFont, new Rectangle(1000, 650, 300, 100), TextUtility.OutLining.Left, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                //if ((int)Screens.Editor != SceneUtility.currentScene)
                //{
                //    CursorUtility.Draw(spriteBatch);
                //}
            }
            if (StartScreen.bIsRunning)
            {
                startScreen.Draw(spriteBatch, gameRender);
                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(gameRender);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null);

                if (!OptionsMenu.bIsRunning)
                {
                    spriteBatch.Draw(startScreen.getRender(), gameRender.Bounds, Color.White);
                }
                else
                {
                    spriteBatch.Draw(OptionsMenu.getRender(), gameRender.Bounds, Color.White);
                }
                BattleGUI.DrawCursor(spriteBatch, KeyboardMouseUtility.uiMousePos.ToVector2(), 1f);

                TextUtility.Draw(spriteBatch, "Pre-alpha combat demo v0.1.6", defaultFont, new Rectangle(1000, 650, 300, 100), TextUtility.OutLining.Left, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);

                spriteBatch.End();

            }

            spriteBatch.End();

            //if (bRunEditMode)
            //{
            //    ScripTestingScene.Draw(spriteBatch);
            //}
            //ResolutionUtility.toggleFullscreen();
            GraphicsDevice.SetRenderTarget(null);
            Matrix m = Matrix.CreateTranslation(0, 0, 1) * Matrix.CreateScale(ResolutionUtility.stdScale.X, ResolutionUtility.stdScale.Y, 1);
            m.M11 = 1.0f;
            m.M22 = 1.0f;
            if (graphics.IsFullScreen)
            {
                Rectangle targetDrawInFullScreen = new Rectangle(((int)monitorSize.X - (int)ResolutionUtility.WindowSizeBeforeFullScreen.X) / 2, ((int)monitorSize.Y - (int)ResolutionUtility.WindowSizeBeforeFullScreen.Y) / 2, (int)ResolutionUtility.WindowSizeBeforeFullScreen.X, (int)ResolutionUtility.WindowSizeBeforeFullScreen.Y);
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null);
                // spriteBatch.Draw(gameRender, targetDrawInFullScreen, gameRender.Bounds, Color.White);
                spriteBatch.Draw(gameRender, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
                spriteBatch.End();

                //spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, SceneUtility.transform);
                //spriteBatch.Draw(gameRender, new Rectangle(0,0,1366,768), Color.White);
                //spriteBatch.End();

                //spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
                //spriteBatch.Draw(gameRender, gameRender.Bounds, Color.White);
                //spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null);
                // spriteBatch.Draw(gameRender, gameRender.Bounds, Color.White);
                // Rectangle targetDrawInFullScreen = new Rectangle(((int)monitorSize.X - (int)ResolutionUtility.WindowSizeBeforeFullScreen.X) / 2, ((int)monitorSize.Y - (int)ResolutionUtility.WindowSizeBeforeFullScreen.Y) / 2, (int)ResolutionUtility.WindowSizeBeforeFullScreen.X, (int)ResolutionUtility.WindowSizeBeforeFullScreen.Y);
                spriteBatch.Draw(gameRender, new Rectangle(0, 0, (int)ResolutionUtility.WindowSizeBeforeFullScreen.X, (int)ResolutionUtility.WindowSizeBeforeFullScreen.Y), Color.White);
                spriteBatch.End();
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ToggleMouse()
        {
            IsMouseVisible = !IsMouseVisible;
        }

        public GraphicsDeviceManager getGDM()
        {
            return graphics;
        }

        public void setGDM(GraphicsDeviceManager gdm)
        {
            graphics = gdm;
        }

        static internal bool bExited = false;
        protected override void Dispose(bool disposing)
        {
            CleanUpResources();
            base.Dispose(disposing);

            bExited = true;
        }

        private static void CleanUpResources()
        {
            Microsoft.Xna.Framework.Media.MediaPlayer.Stop();
            SoundEffectSong.End();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
        }

        protected override void EndRun()
        {
            base.EndRun();
        }

        internal bool GameHasMouse()
        {
            return Window.ClientBounds.Contains(Mouse.GetState().Position + Window.Position);
        }
    }

    internal class SongManager : ContentManager
    {
        internal SongManager(Game1 g) : base(g.Content.ServiceProvider)
        {

        }



        public Microsoft.Xna.Framework.Media.Song Load(string assetName)
        {

            String s = Encrypter.DecryptFileSong(System.IO.Path.Combine(Game1.rootContent, assetName + ".cwma"));
            String scontent = s.Replace(Game1.rootContent, "").Substring(0, s.Replace(TBAGW.Game1.rootContent, "").LastIndexOf("."));
            var temp = base.Load<Microsoft.Xna.Framework.Media.Song>(scontent);
            if (System.IO.File.Exists(s))
            {
                System.IO.File.Delete(s);
            }

            return temp;

        }
    }
}
