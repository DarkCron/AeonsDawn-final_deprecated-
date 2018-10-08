using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.ReadWrite;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    internal class LoadGameScreen
    {
        static SpriteFont font;
        static Texture2D guiTex;
        static TexPanel bigBG;
        static Rectangle bgPosition;
        static bool bInitialize = false;
        static int offSetBg = 50;
        static int offSetRender = 2;
        static Vector2 renderDrawPos;
        static RenderTarget2D render;
        static RenderTarget2D CompleteRender;

        String loadLocation;
        static List<PlayerSaveData> saveDatas = new List<PlayerSaveData>();
        List<LoadFileTab> lfTabs = new List<LoadFileTab>();
        LoadFileTab selectedTab = null;
        static GameContentDataBase gcdbSaveData;
        StartScreen sc;

        internal delegate void ExecuteFunction(String s);
        internal ExecuteFunction func;
        static Task<List<PlayerSaveData>> tempT;

        internal static bool HasSaveFiles(String loadLocation)
        {
            saveDatas = new List<PlayerSaveData>();
            var t = Task<List<PlayerSaveData>>.Factory.StartNew(() => SaveDataProcessor.saveFilesIn(loadLocation));
            tempT = t;
            // t.Start();
            var res = t.Result;
            bool b = saveDatas.Count > 0;
            return true;
            return b;
        }

        private void Initialize()
        {

            bInitialize = true;

            font = BattleGUI.testSF48; //fix
            guiTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
            offSetBg = 50;
            offSetRender = 2;

            renderDrawPos = new Vector2(offSetBg + offSetRender);
            bgPosition = new Rectangle(offSetBg, offSetBg, 1366 - offSetBg * 2, 768 - offSetBg * 2);
            bigBG = new TexPanel(guiTex, bgPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

            render = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366 - (offSetBg + offSetRender) * 2, 768 - (offSetBg + offSetRender) * 2);
            CompleteRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        }

        internal LoadGameScreen(String loadLoc, StartScreen sc = null)
        {
            func = null;
            this.sc = sc;
            if (!bInitialize)
            {
                Initialize();
            }

            loadLocation = loadLoc;
            LoadFileTab.SetScroll(0f);
            GenerateTabs();

        }

        internal LoadGameScreen(String loadLoc, ExecuteFunction f)
        {
            func = f;
            this.sc = null;
            if (!bInitialize)
            {
                Initialize();
            }

            loadLocation = loadLoc;
            LoadFileTab.SetScroll(0f);
            saveDatas.Add(new PlayerSaveData());
            saveDatas.Last().timeIndex = long.MaxValue - 1;
            GenerateTabs();

        }

        private void GenerateTabs()
        {
            List<PlayerSaveData> saves = saveDatas;
            saves = saves.Distinct().ToList();
            saves = saves.OrderBy(psd => psd.timeIndex).ToList();
            saves.Reverse();

            if (saves.Count != 0 && gcdbSaveData == null)
            {
                if (func == null)
                {
                    gcdbSaveData = EditorFileWriter.gcdbReader(saves[0].databaseLoc);
                }
                else if (saves.Count > 1)
                {
                    gcdbSaveData = EditorFileWriter.gcdbReader(saves[1].databaseLoc);
                }

            }

            for (int i = 0; i < saves.Count; i++)
            {
                lfTabs.Add(new LoadFileTab(saves[i], saves, bgPosition, gcdbSaveData));
            }
        }

        internal void HandleMouseClick()
        {
            var temp = lfTabs.Find(tab => tab.Contains(KeyboardMouseUtility.uiMousePos - renderDrawPos.ToPoint()));
            if (temp != null && temp == selectedTab)
            {
                HandleLoadGame();
            }
            else
            {
                Select(temp, selectedTab);
            }
        }

        internal void HandleConfirm()
        {
            if (selectedTab != null)
            {

                HandleLoadGame();
            }
        }

        private void HandleLoadGame()
        {
            if (selectedTab != null)
            {
                if (func != null) { func(selectedTab.getParentSaveData().saveDataName); return; }

                if (sc != null)
                {
                    sc.End();
                }

                GameProcessor.LaunchFromSaveFile(selectedTab.getParentSaveData(), Game1.rootTBAGW);
            }
        }

        internal void Update(GameTime gt)
        {
            if (tempT != null && tempT.IsCompleted)
            {

                saveDatas = tempT.Result;
                if (func != null)
                {
                    saveDatas.Add(new PlayerSaveData());
                    saveDatas.Last().timeIndex = long.MaxValue - 1;
                }
                tempT = null;
                if (saveDatas.Count == 0) { Close(); return; }
                GenerateTabs();

            }

            for (int i = 0; i < lfTabs.Count; i++)
            {
                lfTabs[i].Update(gt);
            }
        }

        internal void GenerateRenders(SpriteBatch sb)
        {
            for (int i = 0; i < lfTabs.Count; i++)
            {
                lfTabs[i].GenerateRender(sb);
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, LoadFileTab.GetMatrix());

            for (int i = 0; i < lfTabs.Count; i++)
            {
                if (selectedTab == null)
                {
                    sb.Draw(lfTabs[i].getRender(), lfTabs[i].screenPosition, Color.White);
                }
                else
                {
                    if (selectedTab == lfTabs[i])
                    {
                        sb.Draw(lfTabs[i].getRender(), lfTabs[i].screenPosition, Color.White);
                    }
                    else
                    {
                        if (lfTabs[i].getTimer() == null)
                        {
                            sb.Draw(lfTabs[i].getRender(), lfTabs[i].screenPosition, Color.White * .5f);
                        }
                        else
                        {
                            sb.Draw(lfTabs[i].getRender(), lfTabs[i].screenPosition, Color.White * (((1.0f - lfTabs[i].getTimer().percentageDone()) * .5f) + 0.5f));
                        }

                    }
                }
            }


            sb.End();
            sb.GraphicsDevice.SetRenderTarget(CompleteRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            bigBG.Draw(sb, Color.White);
            //sb.Draw(Game1.WhiteTex, new Rectangle((int)renderDrawPos.X, (int)renderDrawPos.Y, render.Width, render.Height), Color.Yellow);
            sb.Draw(render, renderDrawPos, Color.White);

            sb.End();
        }

        internal RenderTarget2D getRender()
        {
            return CompleteRender;
        }

        internal void Close()
        {
            for (int i = 0; i < lfTabs.Count; i++)
            {
                lfTabs[i].Close();
            }
        }

        internal void HandleDown()
        {
            int index = lfTabs.IndexOf(selectedTab);
            int previousIndex = index;
            index++;
            if (index >= lfTabs.Count)
            {
                index = 0;
            }
            if (previousIndex == -1)
            {
                Select(lfTabs[index], null);
            }
            else
            {
                Select(lfTabs[index], lfTabs[previousIndex]);
            }

        }

        internal void HandleUp()
        {
            int index = lfTabs.IndexOf(selectedTab);
            int previousIndex = index;
            index--;
            if (index < 0)
            {
                index = lfTabs.Count - 1;
            }
            if (previousIndex == -1)
            {
                Select(lfTabs[index], null);
            }
            else
            {
                Select(lfTabs[index], lfTabs[previousIndex]);
            }
        }

        private void Select(LoadFileTab loadFileTab, LoadFileTab previous)
        {
            selectedTab = loadFileTab;
            if (selectedTab != null)
            {
                LoadFileTab.SetScroll(selectedTab.screenPosition.Y - bgPosition.Height / 3);
            }
            if (previous != null)
            {
                previous.unSelectTimer();
            }
        }

        internal void HandleMouseMove()
        {

        }
    }

    internal class LoadFileTab
    {
        PlayerSaveData psd;
        static bool bInitialize = false;
        static Texture2D guiTex;
        static SpriteFont font;
        static Rectangle panelPosition;
        static TexPanel panel;

        static int vertHeight;
        static int vertSpacing;
        static int horSpacing;
        static int itemsPerPage;
        static Point renderSize;



        int index = 0;
        internal Rectangle screenPosition;
        internal Rectangle drawPosition;
        RenderTarget2D render;


        static Matrix loadTabM;
        static bool bGenerateMatrix = true;
        static float vertMOffset;
        static float maxScrollOffSet = 0f;

        TimingUtility unselectTimer;

        portraitFrame[] portraitFrames = new portraitFrame[4];

        String dateTimeString;
        static Rectangle dateTimeBox;
        static Rectangle titleTextBox;
        static GameText latestText = new GameText("<Latest save>");
        bool bIsFirst = false;

        static internal Matrix GetMatrix()
        {
            return loadTabM;
        }

        internal LoadFileTab(PlayerSaveData psd, List<PlayerSaveData> saves, Rectangle position, GameContentDataBase gcdb)
        {
            this.psd = psd;
            if (!bInitialize)
            {
                Initialize(position);
            }

            int index = saves.IndexOf(psd);
            this.index = index;
            render = new RenderTarget2D(Game1.graphics.GraphicsDevice, renderSize.X, renderSize.Y);
            drawPosition = new Rectangle(new Point(0), renderSize);

            int Y = horSpacing + index * renderSize.Y + vertSpacing * index;
            screenPosition = new Rectangle(horSpacing, Y, renderSize.X, renderSize.Y);

            int frameDistance = 5;
            int frameOffSet = 25;
            int frameSize = renderSize.Y - 2 * frameOffSet;

            if (saves[0] == psd)
            {
                bIsFirst = true;
            }

            for (int i = 0; i < 4; i++)
            {
                if (psd.heroTeamActive.Count > i)
                {
                    var tempBC = gcdb.gameCharacters.Find(c => c.shapeID == psd.heroTeamActive[i].charID);
                    portraitFrames[i] = new portraitFrame(tempBC.portraitAnimations[(int)BaseCharacter.PortraitExpressions.Neutral], new Rectangle(frameSize * i + drawPosition.X + frameOffSet + frameDistance * i, frameOffSet, frameSize, frameSize));
                }
                else
                {
                    portraitFrames[i] = new portraitFrame(null, new Rectangle(frameSize * i + drawPosition.X + frameOffSet + frameDistance * i, frameOffSet, frameSize, frameSize));
                }
            }

            if (saves.Count > 2)
            {
                if (index == saves.Count - 2)
                {
                    maxScrollOffSet = screenPosition.Y;
                }
            }
            else
            {
                maxScrollOffSet = 0;
            }
            if (psd.timeIndex != long.MaxValue - 1)
            {
                DateTime dt = new DateTime(psd.timeIndex);
                dateTimeString = dt.ToShortDateString().ToString() + " " + dt.ToLongTimeString().ToString();
            }
            else
            {
                dateTimeString = "New Save";
            }


            if (!bInitialize)
            {
                PostInitialize(position);
            }
        }

        private void PostInitialize(Rectangle position)
        {
            bInitialize = true;

            int i = 4;
            int frameDistance = 5;
            int frameOffSet = 25;
            int frameSize = renderSize.Y - 2 * frameOffSet;
            int titleBoxHeight = 35;
            int X = frameSize * i + drawPosition.X + frameOffSet + frameDistance * i;
            titleTextBox = new Rectangle(X, frameOffSet, renderSize.X - X - frameOffSet, titleBoxHeight);
            dateTimeBox = new Rectangle(titleTextBox.X, titleTextBox.Y + titleTextBox.Height + frameDistance, titleTextBox.Width, titleTextBox.Height);

        }

        private void Initialize(Rectangle position)
        {
            font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test25");
            guiTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
            vertSpacing = 10;
            horSpacing = 10;
            itemsPerPage = 3;
            vertHeight = (position.Height - horSpacing * 4 - (itemsPerPage - 1) * vertSpacing) / itemsPerPage;

            renderSize = new Point(position.Width - horSpacing * 2, vertHeight + vertSpacing * 2);

            panelPosition = new Rectangle(horSpacing, vertSpacing, renderSize.X - 2 * horSpacing, vertHeight);
            panel = new TexPanel(guiTex, panelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

        }

        static internal void SetScroll(float f)
        {
            if (f < 0)
            {
                f = 0;
            }
            else if (f > maxScrollOffSet)
            {
                f = (maxScrollOffSet);
            }

            vertMOffset = f;
            bGenerateMatrix = true;
        }

        static internal void AddScrollOffSet(float f)
        {
            if (vertMOffset + f >= 0 && vertMOffset + f <= maxScrollOffSet)
            {
                vertMOffset += f;
                SetScroll(vertMOffset);
            }
            else if (vertMOffset + f < 0)
            {
                SetScroll(0);
            }
            else if (vertMOffset + f > maxScrollOffSet)
            {
                SetScroll(maxScrollOffSet);
            }

        }
        internal void Close()
        {
            render.Dispose();
        }

        internal void Update(GameTime gt)
        {
            if (bGenerateMatrix)
            {
                loadTabM = Matrix.CreateTranslation(0, -vertMOffset, 1);
                bGenerateMatrix = false;
            }

            for (int i = 0; i < portraitFrames.Length; i++)
            {
                portraitFrames[i].Update(gt);
            }

            if (unselectTimer != null)
            {
                unselectTimer.Tick(gt);
                if (!unselectTimer.IsActive())
                {
                    unselectTimer = null;
                }
            }
        }

        internal void GenerateRender(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            sb.Draw(Game1.WhiteTex, panel.Position(), Color.White);
            panel.Draw(sb, Color.White);

            for (int i = 0; i < portraitFrames.Length; i++)
            {
                portraitFrames[i].Draw(sb);
            }

            if (bIsFirst && !psd.databaseLoc.Equals(""))
            {
                // sb.Draw(Game1.WhiteTex,titleTextBox,Color.Yellow);
                TextUtility.Draw(sb, latestText.getText(), font, titleTextBox, TextUtility.OutLining.Center, Color.White, 1f, true, default(Matrix), Color.Silver, false);
            }
            TextUtility.Draw(sb, dateTimeString, font, dateTimeBox, TextUtility.OutLining.Center, Color.White, 1f, true, default(Matrix), Color.Silver, false);

            sb.End();
        }

        internal RenderTarget2D getRender()
        {
            return render;
        }

        internal struct portraitFrame
        {
            Rectangle position;
            ShapeAnimation portrait;

            internal portraitFrame(ShapeAnimation portrait, Rectangle r)
            {
                this.portrait = portrait;
                this.position = new Rectangle(r.X, r.Y, r.Width, r.Height);
            }

            internal void Update(GameTime gt)
            {
                if (portrait != null)
                {
                    portrait.UpdateAnimationForItems(gt);
                }
            }

            internal void Draw(SpriteBatch sb)
            {
                sb.Draw(Game1.WhiteTex, position, Color.DarkGray);
                if (portrait != null)
                {
                    portrait.Draw(sb, position);
                }
                else
                {

                }
            }
        }

        internal bool StopSelectionTimerWhen()
        {
            if (unselectTimer.percentageDone() == 1.0f)
            {

                return true;
            }
            return false;
        }

        internal void unSelectTimer()
        {
            unselectTimer = new TimingUtility(16, true, StopSelectionTimerWhen);
            unselectTimer.SetStepTimer(40);
        }

        internal TimingUtility getTimer()
        {
            return unselectTimer;
        }

        internal PlayerSaveData getParentSaveData()
        {
            return psd;
        }

        internal bool Contains(Point m)
        {
            Point p = new Point(m.X, (int)(m.Y + vertMOffset));

            if (screenPosition.Contains(p))
            {
                return true;
            }
            return false;
        }
    }
}
