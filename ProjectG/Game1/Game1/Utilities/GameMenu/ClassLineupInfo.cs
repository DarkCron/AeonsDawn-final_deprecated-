using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;

namespace TBAGW.Utilities.GameMenu
{
    internal class ClassLineupInfo
    {

        static bool bInitialize = false;
        internal CharacterClassCollection CCC;
        internal BaseCharacter bc;

        static Texture2D guiTex;
        static SpriteFont titleFont;

        static TexPanel rightTexPanel;
        static Rectangle rightPanelPosition;
        static TexPanel leftTexPanel;
        static Rectangle leftPanelPosition;
        static Rectangle leftPanelRenderLoc;

        static RenderTarget2D render;

        internal bool bFocusLeftPanel = true;
        internal ClassPanelLayout cpl;
        internal ClassPanelLayout.ClassPanel cp = null;

        internal ClassLineupInfo(BaseCharacter bc)
        {
            if (!bInitialize)
            {
                Initialize();
            }
            this.bc = bc;
            CCC = bc.CCC;
            cpl = new ClassPanelLayout(leftPanelRenderLoc, CCC,CCC.allClassesExceptEquipped());
        }

        static void Initialize()
        {
            bInitialize = false;
            render = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            titleFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test48");//48,32,25
            guiTex = GameMenuHandler.menuTextureSheet;

            rightPanelPosition = new Rectangle(700, 300, 616, 418);


            leftPanelPosition = new Rectangle(50, 300, 616, 418);
            int offset = 2;
            leftPanelRenderLoc = new Rectangle(leftPanelPosition.X + offset, leftPanelPosition.Y + offset, leftPanelPosition.Width - offset * 2, leftPanelPosition.Height - offset * 2);


            rightTexPanel = new TexPanel(guiTex, rightPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            leftTexPanel = new TexPanel(guiTex, leftPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

        }

        internal void Update(GameTime gt)
        {
            cpl.Update(gt);
        }

        private void GenerateRenders(SpriteBatch sb)
        {
            cpl.GenerateRender(sb);
        }

        internal void Draw(SpriteBatch sb)
        {
            GenerateRenders(sb);

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            leftTexPanel.Draw(sb, Color.White);
            sb.Draw(cpl.getRender(), new Rectangle(0, 0, 1366, 768), Color.White);

            sb.End();
        }

        internal void Close()
        {
            GameMenuHandler.selectedCharacterContext.currentCharContext = CharacterContextMenu.CharacterContextMenuType.Select;
            cpl.Close();
        }

        internal RenderTarget2D getRender()
        {
            return render;
        }

        internal void HandleUpDown(bool bUp)
        {
            if (bFocusLeftPanel)
            {
                HandleUpDownLeftPanel(bUp);
            }
            else
            {

            }
        }

        private void HandleUpDownLeftPanel(bool bUp)
        {
            if (cpl.panels.Count != 0)
            {
                int index = cpl.panels.IndexOf(cp);
                if (bUp)
                {
                    index++;
                }
                else
                {
                    index--;
                }
                if (index >= cpl.panels.Count)
                {
                    index = 0;
                }
                else if (index < 0)
                {
                    index = cpl.panels.Count - 1;
                }
                Select(cpl.panels[index], cp);
            }
        }

        private void Select(ClassPanelLayout.ClassPanel newPanel, ClassPanelLayout.ClassPanel oldPanel)
        {
            if (newPanel != oldPanel)
            {
                if (newPanel != null)
                {
                    newPanel.Select();
                }

                if (oldPanel != null)
                {
                    oldPanel.Unselect();
                }

                cp = newPanel;
            }
        }

        internal void HandleScrollUpDown(float os)
        {
            if (bFocusLeftPanel)
            {
                HandleScrollUpDownLeftPanel(os);
            }
            else
            {

            }
        }

        private void HandleScrollUpDownLeftPanel(float os)
        {
            cpl.Scroll(os);
        }

        internal void HandleMouseButtonPress(Point m)
        {
            if (cpl.panelContainsMouse(m))
            {
                bFocusLeftPanel = true;
                var temp = cpl.panels.Find(p => p.Contains(m, cpl.scrollOffset));
                if (temp != null)
                {
                    Console.WriteLine("Selected: "+cpl.panels.IndexOf(temp));
                    Select(temp, cp);
                }
            }
        }
    }

    internal class ClassPanelLayout
    {
        static RenderTarget2D completeRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D render;
        static bool bInitialize = false;
        internal float scrollOffset = 0.0f;
        float minScroll = 0f;
        float maxScroll = 0f;
        bool bUpdateMatrix = true;
        Matrix scrollMatrix;
        Rectangle renderPos;
        CharacterClassCollection CCC;

        internal List<ClassPanel> panels = new List<ClassPanel>();

        internal ClassPanelLayout(Rectangle pos, CharacterClassCollection ccc, List<BaseClass> lbc)
        {
            renderPos = pos;
            this.CCC = ccc;
            if (!bInitialize)
            {
                Initialize(pos);
            }

            for (int i = 0; i < lbc.Count; i++)
            {
                panels.Add(new ClassPanel(i, pos.Location, CCC.charClassList[i],this));
            }

            if (panels.Count > 2)
            {
                maxScroll = panels[panels.Count - 2].heightIndicator();
            }else
            {
                maxScroll = 0;
            }
           
        }

        internal void SetScroll(float f)
        {
            if (f >= minScroll && f <= maxScroll)
            {
                scrollOffset = f;
            }
            else if (f < minScroll)
            {
                scrollOffset = minScroll;
            }
            else if (f > maxScroll)
            {
                scrollOffset = maxScroll;
            }
            bUpdateMatrix = true;

        }

        internal void Scroll(float f)
        {
            SetScroll(scrollOffset + f);
        }

        private void Initialize(Rectangle pos)
        {
            bInitialize = true;
            render = new RenderTarget2D(Game1.graphics.GraphicsDevice, pos.Width, pos.Height);
        }

        internal void Update(GameTime gt)
        {
            if (bUpdateMatrix)
            {
                bUpdateMatrix = false;
                scrollMatrix = Matrix.CreateTranslation(0, -scrollOffset, 1);
            }

            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].Update(gt);
            }
        }

        internal void GenerateRender(SpriteBatch sb)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].GenerateRender(sb);
            }
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, scrollMatrix);
            for (int i = 0; i < panels.Count; i++)
            {
                sb.Draw(Game1.WhiteTex, panels[i].getRenderLoc(), Color.Firebrick);
                sb.Draw(panels[i].getRender(), panels[i].getRenderLoc(), Color.White);
                var t = panels[i].getRenderLoc();

            }
            sb.End();
            Draw(sb);

        }

        internal void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(completeRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            // sb.Draw(Game1.WhiteTex, renderPos, Color.Firebrick);
            sb.Draw(render, renderPos, Color.White);

            sb.End();
        }

        internal RenderTarget2D getRender()
        {
            return completeRender;
        }

        internal void Close()
        {
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].Close();
            }
        }

        internal bool panelContainsMouse(Point p)
        {
            return renderPos.Contains(p);
        }

        internal class ClassPanel
        {
            //612, 414
            static Rectangle drawPos;
            Rectangle onScreen;
            Rectangle renderDrawPos;
            static bool bInitialize = false;
            static int frameSize;
            static int offSetPanels;
            static Rectangle classFramePos;
            static TexPanel panel;
            static Texture2D guiTex;
            RenderTarget2D render;
            static SpriteFont font;
            static Rectangle classNameLoc;
            static Rectangle classArmourLoc;
            String className;
            String classArmour;

            BaseClass parent;
            ClassPanelLayout cpl;

            internal ClassPanel(int index, Point renderPosBigPanel, BaseClass c,ClassPanelLayout cpl)
            {
                if (!bInitialize)
                {
                    Initialize();
                }

                this.cpl = cpl;
                int yMod = index * classFramePos.Height + index * offSetPanels;
                renderDrawPos = new Rectangle(0, yMod, drawPos.Width, drawPos.Height);
                onScreen = new Rectangle(renderPosBigPanel.X, renderPosBigPanel.Y + yMod, drawPos.Width, drawPos.Height);
                render = new RenderTarget2D(Game1.graphics.GraphicsDevice, drawPos.Width, drawPos.Height);
                parent = c;
                className = c.ClassName + " Lvl. " + c.classEXP.classLevel+1;
                classArmour = c.classTypeArmour.ToString();
            }

            static void Initialize()
            {
                frameSize = 88;
                offSetPanels = 20;
                int offsetClassFramePanel = 4;
                drawPos = new Rectangle(0, 0, 612, frameSize + offsetClassFramePanel * 2);
                classFramePos = new Rectangle(offsetClassFramePanel, offsetClassFramePanel, frameSize, frameSize);
                guiTex = GameMenuHandler.menuTextureSheet;
                panel = new TexPanel(guiTex, drawPos, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
                bInitialize = true;

                font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");//48,32,25

                int textBoxHeight = 24;
                classNameLoc = new Rectangle(classFramePos.X + classFramePos.Width + offsetClassFramePanel, offsetClassFramePanel, drawPos.Width - 3 * offsetClassFramePanel, textBoxHeight);
                classArmourLoc = new Rectangle(classFramePos.X + classFramePos.Width + offsetClassFramePanel, offsetClassFramePanel * 2 + textBoxHeight, drawPos.Width - 3 * offsetClassFramePanel, textBoxHeight);
            }

            internal void Update(GameTime gt)
            {

            }

            internal void GenerateRender(SpriteBatch sb)
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(render);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

                panel.Draw(sb, Color.White);
                sb.Draw(Game1.WhiteTex, classFramePos, Color.Purple);
                TextUtility.Draw(sb, className, font, classNameLoc, TextUtility.OutLining.Left, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);
                TextUtility.Draw(sb, classArmour, font, classArmourLoc, TextUtility.OutLining.Left, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);

                sb.End();
            }

            internal RenderTarget2D getRender()
            {
                return render;
            }

            internal Rectangle getRenderLoc()
            {
                return renderDrawPos;
            }

            internal void Close()
            {
                render.Dispose();
            }

            internal bool Contains(Point p, float scroll)
            {
                Point m = new Point(p.X, (int)(p.Y + scroll));
                return onScreen.Contains(m);
            }

            internal int heightIndicator()
            {
                return renderDrawPos.Y;
            }

            internal void Select()
            {
                cpl.SetScroll(heightIndicator());
            }

            internal void Unselect() { }
        }
    }
}
