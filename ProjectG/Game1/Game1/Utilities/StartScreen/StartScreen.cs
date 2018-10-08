using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW
{
    public class StartScreen
    {
        ShapeAnimation bg = new ShapeAnimation();
        SpriteFont sf;
        internal List<SCButton> mButtons = new List<SCButton>();
        internal RenderTarget2D r2d;
        static internal bool bIsRunning = false;
        static internal StartScreen sc;
        internal SCButton selectedButton = null;
        internal bool bReadyToLoad = false;
        internal bool bFinishedLoading = false;
        internal int postLoadTimer = 3200;
        internal int timePassedLoadTimer = 0;

        internal enum StartScreens { main, load, loading }
        internal enum Screens { start, load }
        internal StartScreens currentStartScreen = StartScreens.main;
        internal Screens currentScreen = Screens.start;

        internal LoadGameScreen lgs;
        TimingUtility loadGameDataTimer;

        public StartScreen()
        {
            BattleGUI.InitializeResources();
            bIsRunning = true;
            r2d = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            bg.texFileLoc = @"Graphics\StartScreen\StartBG";
            bg.animationFrames.Add(new Rectangle(0, 0, 1366, 768));
            bg.ReloadTexture();

            sf = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test48");
            //testSF = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test");
            //testSF20 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test20");
            //testSF25 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test25");
            //testSF32 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");
            //testSF48 = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test48");
            int width = 800;
            int deltaH = 28;
            int height = (400 - deltaH * 5) / 4;

            Rectangle b = new Rectangle((1366 - width) / 2, 400, 800, height);
            mButtons.Add(new SCButton(b, sf, "New Game"));
            b.Y += height + deltaH;
            mButtons.Add(new SCButton(b, sf, "Load Game"));
            b.Y += height + deltaH;
            mButtons.Add(new SCButton(b, sf, "Settings"));
            b.Y += height + deltaH;
            mButtons.Add(new SCButton(b, sf, "Exit Game"));
            sc = this;

            Utilities.Control.Player.PlayerController.InitializeStartScreenControls(this);
        }

        internal void CloseLoadScreen()
        {
            currentScreen = Screens.start;
            lgs.Close();
            lgs = null;
        }

        public void Update(GameTime gt)
        {
            if (OptionsMenu.bIsRunning) //copied from gameprocessor
            {
                Utilities.Control.Player.PlayerController.Update(gt);
                OptionsMenu.Update(gt);
                return;
            }

            if (currentScreen== Screens.start)
            {
                if (currentStartScreen!=StartScreens.loading)
                {
                    Utilities.Control.Player.PlayerController.Update(gt);
                }
            }else
            {
                Utilities.Control.Player.PlayerController.Update(gt);
            }



            switch (currentScreen)
            {
                case Screens.start:
                    #region
                    switch (currentStartScreen)
                    {
                        case StartScreens.main:
                            UpdateMain(gt);
                            break;
                        case StartScreens.load:
                            break;
                        case StartScreens.loading:
                            UpdateLoading(gt);
                            break;
                        default:
                            break;
                    }
                    #endregion
                    break;
                case Screens.load:
                    #region
                    if (lgs != null)
                    {
                        lgs.Update(gt);
                    }
                    if (loadGameDataTimer != null)
                    {
                        loadGameDataTimer.Tick(gt);
                        if (!loadGameDataTimer.IsActive())
                        {
                            loadGameDataTimer = null;
                        }
                    }
                    #endregion
                    break;
                default:
                    break;
            }

        }

        internal void LoadGameButton()
        {
            if (LoadGameScreen.HasSaveFiles(SaveDataProcessor.saveFolder))
            {
                lgs = new LoadGameScreen(SaveDataProcessor.saveFolder,this);
                currentScreen = Screens.load;
                loadGameDataTimer = new TimingUtility(40, false, stopTimer1);
                loadGameDataTimer.SetStepTimer(60, 0);
            }
        }

        internal void OptionsButton()
        {
            OptionsMenu.Start(BackToStartScreen);
        }

        private void UpdateLoading(GameTime gt)
        {
            if (bReadyToLoad && !bFinishedLoading)
            {
                GameProcessor.Launch();
                bFinishedLoading = true;
            }
            if (bFinishedLoading)
            {
                GameProcessor.Update(gt);
                GameProcessor.UpdateCamera();
                if (ScriptProcessor.bIsRunning)
                {
                    //   ScriptProcessor.Update(gt);
                }
                timePassedLoadTimer += gt.ElapsedGameTime.Milliseconds;
                if (timePassedLoadTimer > postLoadTimer)
                {
                    End();
                }
            }
        }

        internal void UpdateMain(GameTime gt)
        {
            for (int i = 0; i < mButtons.Count; i++)
            {
                mButtons[i].Update(gt, this);
            }
        }

        public void Draw(SpriteBatch sb, RenderTarget2D target = null)
        {
            if (OptionsMenu.bIsRunning)
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(r2d);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
                bg.Draw(sb, new Rectangle(0, 0, 1366, 768));
                sb.End();
                OptionsMenu.Draw(sb,r2d);
                return;
            }
            #region Generate Renders
            switch (currentScreen)
            {
                case Screens.start:
                    break;
                case Screens.load:
                    if (lgs != null)
                    {
                        lgs.GenerateRenders(sb);
                    }
                    break;
                default:
                    break;
            }
            #endregion
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(r2d);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            switch (currentScreen)
            {
                case Screens.start:
                    #region
                    switch (currentStartScreen)
                    {
                        case StartScreens.main:
                            DrawMain(sb);
                            break;
                        case StartScreens.load:
                            break;
                        case StartScreens.loading:
                            DrawLoadScreen(sb, target);
                            break;
                        default:
                            break;
                    }
                    #endregion
                    break;
                case Screens.load:
                    #region
                    if (lgs != null)
                    {
                        bg.Draw(sb, new Rectangle(0, 0, 1366, 768));

                        if (loadGameDataTimer != null)
                        {
                            sb.Draw(lgs.getRender(), new Rectangle(0, 0, 1366, 768), Color.White * (loadGameDataTimer.percentageDone()));
                        }
                        else
                        {
                            sb.Draw(lgs.getRender(), new Rectangle(0, 0, 1366, 768), Color.White);
                        }


                    }
                    #endregion
                    break;
                default:
                    break;
            }


            sb.End();
            sb.GraphicsDevice.SetRenderTarget(target);
        }

        private void DrawLoadScreen(SpriteBatch sb, RenderTarget2D target = null)
        {
            float opactiy = (float)(postLoadTimer - timePassedLoadTimer) / postLoadTimer;
            sb.Draw(Game1.WhiteTex, new Rectangle(0, 0, 1366, 768), Color.Black * opactiy);
            TextUtility.Draw(sb, "Loading...", sf, new Rectangle(1000, 600, 300, 80), TextUtility.OutLining.Left, Color.Gold * opactiy, 1f, true, default(Matrix), Color.Silver * opactiy, false);
            bReadyToLoad = true;
            if (bFinishedLoading)
            {
                TextUtility.Draw(sb, "Done", sf, new Rectangle(1000, 680, 300, 40), TextUtility.OutLining.Left, Color.Snow * opactiy, 1f, true, default(Matrix), Color.Silver * opactiy, false);
                sb.Draw(target, new Rectangle(0, 0, 1366, 768), Color.White * (1.0f - opactiy));
            }

        }

        internal void DrawMain(SpriteBatch sb)
        {
            bg.Draw(sb, new Rectangle(0, 0, 1366, 768));
            for (int i = 0; i < mButtons.Count; i++)
            {
                mButtons[i].Draw(sb);
            }

        }

        internal void End()
        {
            //SceneUtility.currentScene = (int)Game1.Screens.Editor;
            StartScreen.bIsRunning = false;
            //Utilities.Control.Player.PlayerController.currentController = Utilities.Control.Player.PlayerController.Controllers.NonCombat;
            bFinishedLoading = false;
            bReadyToLoad = false;
            timePassedLoadTimer = 0;
        }

        internal RenderTarget2D getRender()
        {
            return r2d;
        }

        internal bool NewGameButton()
        {
            PlayerSaveData.Reset();
            currentStartScreen = StartScreens.loading;

            return true;
        }

        internal bool stopTimer1()
        {
            if (loadGameDataTimer.percentageDone() == 1.0f)
            {
                return true;
            }
            return false;
        }

        internal void BackToStartScreen()
        {
            OptionsMenu.Close();
        }
    }

    public class SCButton
    {
        public Rectangle position;
        public SpriteFont sf;
        public GameText gt;
        public Color tc = Color.Black;
        bool bSelected = false;
        bool bGoingDown = false;
        int colorTimer = 10;
        int timePassed = 0;

        public SCButton(Rectangle p, SpriteFont sf, String s, GameText.Language l = GameText.Language.English)
        {
            position = p;
            this.sf = sf;
            gt = new GameText();
            gt.AddText(s, l);
        }

        internal void Unselect()
        {
            tc = Color.Black;
            bSelected = false;
            bGoingDown = false;
            timePassed = 0;
        }

        internal void Select()
        {
            bGoingDown = true;
            timePassed = 0;
            bSelected = true;
        }

        public void Update(GameTime gt, StartScreen sc)
        {
            if (sc.selectedButton != this)
            {
                if (bSelected)
                {
                    Unselect();
                }
            }
            else if (!bSelected && sc.selectedButton == this)
            {
                Select();
            }

            if (bSelected)
            {
                timePassed += gt.ElapsedGameTime.Milliseconds;
                if (timePassed > colorTimer)
                {
                    timePassed = 0;
                    if (bGoingDown)
                    {
                        tc.R += 1;
                        tc.G += 1;
                        tc.B += 1;
                        if (tc.R > 100)
                        {
                            bGoingDown = false;
                        }
                    }
                    else
                    {
                        tc.R -= 1;
                        tc.G -= 1;
                        tc.B -= 1;
                        if (tc.R <= 0)
                        {
                            bGoingDown = true;
                        }
                    }

                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            TextUtility.Draw(sb, gt.getText(), sf, position, TextUtility.OutLining.Center, tc, 1f, true, default(Matrix), Color.LightBlue, false);
        }

        public bool Contains(Point p)
        {
            if (position.Contains(p))
            {
                return true;
            }

            return false;
        }
    }
}
