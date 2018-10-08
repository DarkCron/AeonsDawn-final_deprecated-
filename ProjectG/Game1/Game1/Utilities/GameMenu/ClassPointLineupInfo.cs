using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.GameMenu;

namespace TBAGW
{
    internal class ClassPointLineupInfo
    {
        BaseCharacter parent;

        static RenderTarget2D render;
        static Texture2D guiTex;
        static SpriteFont titleFont;

        static TexPanel rightTexPanel;
        static Rectangle rightPanelPosition;
        static TexPanel leftTexPanel;
        static Rectangle leftPanelPosition;
        static Rectangle leftPanelRenderLoc;

        static bool bInitialize = true;

        internal bool bFocusLeftPanel = true;
        internal ClassPanelLayout cpl;
        internal ClassPanelLayout.ClassPanel cp = null;
        static TalentGrid talentGrid;

        static void Initialize()
        {
            bInitialize = false;

            render = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            titleFont = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test48");//48,32,25
            guiTex = GameMenuHandler.menuTextureSheet;

            rightPanelPosition = new Rectangle(700, 300, 616, 418);


            leftPanelPosition = new Rectangle(67, 50, 1232, 668);
            int offset = 2;
            leftPanelRenderLoc = new Rectangle(leftPanelPosition.X + offset, leftPanelPosition.Y + offset, leftPanelPosition.Width - offset * 2, leftPanelPosition.Height - offset * 2);


            rightTexPanel = new TexPanel(guiTex, rightPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            leftTexPanel = new TexPanel(guiTex, leftPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

        }

        internal ClassPointLineupInfo(BaseCharacter bc)
        {
            if (bInitialize)
            {
                Initialize();
            }
            parent = bc;
            var l = bc.CCC.allClassesExceptEquipped();
            cpl = new ClassPanelLayout(rightPanelPosition, bc.CCC, l);
            talentGrid = new TalentGrid(bc);
            TalentGrid.mPos = new Point(0, -leftPanelPosition.Size.Y * 2 / 4);
        }

        internal void Update(GameTime gt)
        {
            talentGrid.Update(gt);
            cpl.Update(gt);
        }

        internal void GenerateRender(SpriteBatch sb)
        {
            talentGrid.GenerateRender(sb);
            //cpl.GenerateRender(sb);
        }

        internal void Draw(SpriteBatch sb)
        {
            GenerateRender(sb);

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            leftTexPanel.Draw(sb, Color.White);
            sb.Draw(talentGrid.getRender(), leftPanelRenderLoc, leftPanelRenderLoc, Color.White);
            
            sb.End();
        }

        internal void Close()
        {
            GameMenuHandler.selectedCharacterContext.currentCharContext = CharacterContextMenu.CharacterContextMenuType.Select;
            cpl.Close();
            talentGrid.Close();
        }

        internal RenderTarget2D getRender()
        {
            return render;
        }

        internal void HandleUpDown(bool bUp)
        {

            HandleUpDownLeftPanel(bUp);

        }

        private void HandleUpDownLeftPanel(bool bUp)
        {
            TalentGrid.hoverOverNode = null;
            if (bUp)
            {
                TalentGrid.mPos.Y -= 9;
                TalentGrid.bUpdateMatrix = true;
            }
            else
            {
                TalentGrid.mPos.Y += 9;
                TalentGrid.bUpdateMatrix = true;
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
                    Console.WriteLine("Selected: " + cpl.panels.IndexOf(temp));
                    Select(temp, cp);
                }
            }
        }

        internal void HandleMouseMove()
        {
            talentGrid.NodeContainsMouse(leftPanelPosition);
        }

        internal void SelectUpTalentNode()
        {
            talentGrid.SelectUp();
        }

        internal void SelectDownTalentNode()
        {
            talentGrid.SelectDown();
        }

        internal void HandleLeft(bool v)
        {
            TalentGrid.hoverOverNode = null;
            TalentGrid.mPos.X -= 9;
            TalentGrid.bUpdateMatrix = true;
        }

        internal void HandleRight(bool v)
        {
            TalentGrid.hoverOverNode = null;
            TalentGrid.mPos.X += 9;
            TalentGrid.bUpdateMatrix = true;
        }

        internal void SelectRightTalentNode()
        {
            talentGrid.SelectRight();
        }

        internal void SelectLeftTalentNode()
        {
            talentGrid.SelectLeft();
        }
    }
}
