using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    internal class AbilityLineupInfo
    {
        internal enum controlType { Normal }


        internal controlType currentType = controlType.Normal;

        static Rectangle rightPanelPosition = new Rectangle(700, 300, 616, 418);
        static Rectangle rightPanelAbilityLineup;
        static Rectangle leftPanelPosition = new Rectangle(700, 300, 616, 418);
        static Rectangle leftPanelAbilityLineup;
        static TexPanel bgPanelRight;
        static TexPanel bgPanelLeft;
        static GameText rightPanelTitle;
        static Rectangle rightPanelTitlePosition;
        static GameText leftPanelTitle;
        static Rectangle leftPanelTitlePosition;

        static bool bInitialize = false;
        static Texture2D guiTex;
        static SpriteFont TitleFont;

        static RenderTarget2D render = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);

        AbilitySelectTab selectedAbility = null;
        List<AbilitySelectTab> equippedAbilities = new List<AbilitySelectTab>();
        List<AbilitySelectTab> availableAbilities = new List<AbilitySelectTab>();
        float rightPanelVertOS = 0f;
        BaseCharacter bc;
        TimingUtility selectionTimer;
        bool bBuildUp = true;

        internal bool bCursorFocusRight = true;

        internal AbilityLineupInfo(BaseCharacter bc)
        {
            if (!bInitialize)
            {
                Initialize();
            }
            AbilitySelectTab.SetScrollOffSetLeft(0f);
            AbilitySelectTab.SetScrollOffSetRight(0f);
            this.bc = bc;

            var tempList = bc.CCC.possibleAbilities();
            var equippedIDs = bc.CCC.possibleAbilities().Select(abi => abi.abilityIdentifier).ToList();
            for (int i = 0; i < tempList.Count; i++)
            {
                equippedAbilities.Add(new AbilitySelectTab(tempList[i], rightPanelAbilityLineup, i));
            }
            AbilitySelectTab.InitializeRight(rightPanelAbilityLineup, equippedAbilities);

            tempList = bc.CCC.allCharacterAbilities().FindAll(abi => !equippedIDs.Contains(abi.abilityIdentifier));
            for (int i = 0; i < tempList.Count; i++)
            {
                availableAbilities.Add(new AbilitySelectTab(tempList[i], leftPanelAbilityLineup, i));
            }
            AbilitySelectTab.InitializeLeft(leftPanelAbilityLineup, availableAbilities);

            bCursorFocusRight = false;
            SwitchLeftRight();
        }

        private void Initialize()
        {
            bInitialize = true;

            TitleFont = BattleGUI.testSF48;
            guiTex = GameMenuHandler.menuTextureSheet;

            rightPanelPosition = new Rectangle(700, 300, 616, 418);
            rightPanelAbilityLineup = new Rectangle(710, 368, 596, 340);
            rightPanelTitlePosition = new Rectangle(710, 310, 596, 48);
            rightPanelTitle = new GameText();
            rightPanelTitle.AddText("Equipped abilities: ", GameText.Language.English);

            leftPanelPosition = new Rectangle(50, 300, 616, 418);
            leftPanelAbilityLineup = new Rectangle(60, 368, 596, 340);
            leftPanelTitlePosition = new Rectangle(60, 310, 596, 48);
            leftPanelTitle = new GameText();
            leftPanelTitle.AddText("Available abilities: ", GameText.Language.English);

            bgPanelRight = new TexPanel(guiTex, rightPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            bgPanelLeft = new TexPanel(guiTex, leftPanelPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));


        }

        internal void Update(GameTime gt)
        {
            for (int i = 0; i < equippedAbilities.Count; i++)
            {
                equippedAbilities[i].Update(gt);
            }

            for (int i = 0; i < availableAbilities.Count; i++)
            {
                availableAbilities[i].Update(gt);
            }

            if (selectedAbility != null && selectedAbility.asd != null)
            {
                selectedAbility.asd.Update(gt);
            }

            selectionTimer.Tick(gt);
            if (bBuildUp)
            {
                if (!selectionTimer.IsActive())
                {
                    bBuildUp = false;
                    selectionTimer = new TimingUtility(30, true, StopSelectionTimerWhen);
                    selectionTimer.SetStepTimer(40);
                }
            }
            else
            {
                if (!selectionTimer.IsActive())
                {
                    bBuildUp = true;
                    selectionTimer = new TimingUtility(30, true, StopSelectionTimerWhen);
                    selectionTimer.SetStepTimer(40);
                }
            }
        }

        internal void GenerateRenders(SpriteBatch sb)
        {
            Draw(sb);

            if (selectedAbility != null && selectedAbility.asd != null)
            {
                selectedAbility.asd.GenerateRender(sb);
            }

            for (int i = 0; i < equippedAbilities.Count; i++)
            {
                equippedAbilities[i].Draw(sb);
            }
            for (int i = 0; i < availableAbilities.Count; i++)
            {
                availableAbilities[i].Draw(sb);
            }
            AbilitySelectTab.GenerateCompleteRenderLeft(sb, availableAbilities);
            AbilitySelectTab.GenerateCompleteRenderRight(sb, equippedAbilities);

        }

        internal void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            float opacityMod = 0.8f;

            if (bCursorFocusRight)
            {
                bgPanelLeft.Draw(sb, Color.White);
                if (bBuildUp)
                {
                    bgPanelRight.Draw(sb, Color.White * ((selectionTimer.percentageDone() + opacityMod) * opacityMod));
                }
                else
                {
                    bgPanelRight.Draw(sb, Color.White * ((1.0f + opacityMod - selectionTimer.percentageDone()) * opacityMod));
                }
            }
            else
            {
                bgPanelRight.Draw(sb, Color.White);
                if (bBuildUp)
                {
                    bgPanelLeft.Draw(sb, Color.White * ((selectionTimer.percentageDone() + opacityMod) * opacityMod));
                }
                else
                {
                    bgPanelLeft.Draw(sb, Color.White * ((1.0f + opacityMod - selectionTimer.percentageDone()) * opacityMod));
                }
            }


            TextUtility.Draw(sb, rightPanelTitle.getText() + " (" + bc.CCC.abiEquipList.abilities.Count + @"/" + bc.CCC.abiEquipList.amount + ")", TitleFont, rightPanelTitlePosition, TextUtility.OutLining.Left, Color.White, 1f, true, default(Matrix), Color.Silver, false);
            // sb.Draw(Game1.WhiteTex, rightPanelAbilityLineup, Color.Red);

            sb.Draw(AbilitySelectTab.GetRenderRight(), new Rectangle(0, 0, 1366, 768), Color.White);


            TextUtility.Draw(sb, leftPanelTitle.getText(), TitleFont, leftPanelTitlePosition, TextUtility.OutLining.Left, Color.White, 1f, true, default(Matrix), Color.Silver, false);

            sb.Draw(AbilitySelectTab.GetRenderLeft(), new Rectangle(0, 0, 1366, 768), Color.White);

            if (selectedAbility != null && selectedAbility.asd != null)
            {
                selectedAbility.asd.Draw(sb);
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
        }

        internal void SwitchLeftRight()
        {
            bCursorFocusRight = !bCursorFocusRight;
            selectedAbility = null;
            if (selectionTimer == null)
            {
                selectionTimer = new TimingUtility(30, true, StopSelectionTimerWhen);
                selectionTimer.SetStepTimer(40);
            }

        }

        internal void HandleUpDown(bool pressedDown)
        {
            if (bCursorFocusRight)
            {
                if (equippedAbilities.Count != 0)
                {
                    int selectedIndex = equippedAbilities.IndexOf(selectedAbility);
                    if (pressedDown)
                    {
                        selectedIndex++;
                        if (selectedIndex == equippedAbilities.Count)
                        {
                            selectedIndex = 0;
                        }
                    }
                    else
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                        {
                            selectedIndex = equippedAbilities.Count - 1;
                        }
                    }

                    Select(equippedAbilities[selectedIndex]);

                }
            }
            else
            {
                if (availableAbilities.Count != 0)
                {
                    int selectedIndex = availableAbilities.IndexOf(selectedAbility);
                    if (pressedDown)
                    {
                        selectedIndex++;
                        if (selectedIndex == availableAbilities.Count)
                        {
                            selectedIndex = 0;
                        }
                    }
                    else
                    {
                        selectedIndex--;
                        if (selectedIndex < 0)
                        {
                            selectedIndex = availableAbilities.Count - 1;
                        }
                    }

                    Select(availableAbilities[selectedIndex]);

                }
            }
        }

        internal RenderTarget2D getRender()
        {
            return render;
        }

        internal void Close()
        {
            for (int i = 0; i < equippedAbilities.Count; i++)
            {
                equippedAbilities[i].getRender().Dispose();
            }
            for (int i = 0; i < availableAbilities.Count; i++)
            {
                availableAbilities[i].getRender().Dispose();
            }

            if (selectedAbility != null)
            {
                selectedAbility.asd.Close();
            }
        }

        internal AbilitySelectTab getSelectedAbility()
        {
            return selectedAbility;
        }

        internal bool HandleMouseClick()
        {
            if (selectedAbility == null)
            {
                AbilitySelectTab temp = equippedAbilities.Find(tab => tab.containsMouseRight(KeyboardMouseUtility.uiMousePos));
                if (temp != default(AbilitySelectTab))
                {
                    bCursorFocusRight = false;
                    SwitchLeftRight();
                    Select(temp);
                    return true;
                }

                temp = availableAbilities.Find(tab => tab.containsMouseLeft(KeyboardMouseUtility.uiMousePos));
                if (temp != default(AbilitySelectTab))
                {
                    bCursorFocusRight = true;
                    SwitchLeftRight();
                    Select(temp);
                    return true;
                }
            }
            else
            {

                if (bCursorFocusRight)
                {
                    if (selectedAbility.containsMouseRight(KeyboardMouseUtility.uiMousePos))
                    {
                        bc.CCC.abiEquipList.Remove(selectedAbility.getParent());
                        GameMenuHandler.selectedCharacterContext.abilityLineupInfo = new AbilityLineupInfo(GameMenuHandler.selectedCharacterContext.abilityLineupInfo.bc);
                        return false;
                    }
                    else
                    {
                        selectedAbility = null;
                        return HandleMouseClick();
                    }

                }
                else
                {
                    if (selectedAbility.containsMouseLeft(KeyboardMouseUtility.uiMousePos))
                    {
                        if (bc.CCC.abiEquipList.CanAddAbility(selectedAbility.getParent()))
                        {
                            bc.CCC.abiEquipList.Add(selectedAbility.getParent());
                            GameMenuHandler.selectedCharacterContext.abilityLineupInfo = new AbilityLineupInfo(GameMenuHandler.selectedCharacterContext.abilityLineupInfo.bc);
                        }
                    }
                    else
                    {
                        selectedAbility = null;
                        return HandleMouseClick();
                    }
                }

            }

            return false;
        }

        internal bool HandleConfirm()
        {
            if (selectedAbility != null)
            {
                if (bCursorFocusRight)
                {

                    bc.CCC.abiEquipList.Remove(selectedAbility.getParent());
                    GameMenuHandler.selectedCharacterContext.abilityLineupInfo = new AbilityLineupInfo(GameMenuHandler.selectedCharacterContext.abilityLineupInfo.bc);
                    return true;


                }
                else
                {

                    if (bc.CCC.abiEquipList.CanAddAbility(selectedAbility.getParent()))
                    {
                        bc.CCC.abiEquipList.Add(selectedAbility.getParent());
                        GameMenuHandler.selectedCharacterContext.abilityLineupInfo = new AbilityLineupInfo(GameMenuHandler.selectedCharacterContext.abilityLineupInfo.bc);
                    }
                    return true;
                }
            }
            return false;
        }

        internal void Select(AbilitySelectTab ast)
        {
            ast.Select();
        }

        internal void setSelectedAbility(AbilitySelectTab abilitySelectTab)
        {
            if (selectedAbility!=null)
            {
                selectedAbility.asd.Close();
            }

            selectedAbility = abilitySelectTab;

            if (selectedAbility != null)
            {
                selectedAbility.asd = new AbilitySelectDescription(bc, selectedAbility.getParent(), new Rectangle(450, selectedAbility.distanceY()-300, 250, 400));
            }
          
        }

        internal bool StopSelectionTimerWhen()
        {
            if (selectionTimer.percentageDone() == 1.0f)
            {

                return true;
            }
            return false;
        }
    }

    internal class AbilitySelectTab
    {
        internal AbilitySelectDescription asd = null;
        internal Rectangle positionOnscreen;
        internal Rectangle drawPositionOnscreen;
        RenderTarget2D render;
        int index;
        BasicAbility parent;

        static RenderTarget2D sideRenderRight;
        static RenderTarget2D completeRenderRight = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static RenderTarget2D sideRenderLeft;
        static RenderTarget2D completeRenderLeft = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        static Matrix mRight;
        static Matrix mLeft;
        static internal bool bGenerateMatrixRight = true;
        static internal bool bGenerateMatrixLeft = true;

        static SpriteFont abilityNameFont;
        static Rectangle iconBox;
        static Rectangle abilityTitleBox;
        static Rectangle abilityCostBox;

        static bool bInitialize = false;
        static Rectangle renderSize;
        static int verticalSpacing;
        static float scrollOffSetRight = 0f;
        static float scrollOffSetLeft = 0f;
        static Rectangle containerLeft;
        static Rectangle containerRight;


        static float maxScrollOffSetLeft = 0f;
        static float maxScrollOffSetRight = 0f;

        TimingUtility selectionTimer;
        bool bBuildup = true;

        static internal void SetScrollOffSetLeft(float f)
        {
            scrollOffSetRight = f;
            bGenerateMatrixLeft = true;
        }

        static internal void AddScrollOffSetLeft(float f)
        {
            if (scrollOffSetRight + f >= 0 && scrollOffSetRight + f <= maxScrollOffSetLeft)
            {
                scrollOffSetRight += f;
                SetScrollOffSetLeft(scrollOffSetRight);
            }
            else if (scrollOffSetRight + f < 0)
            {
                SetScrollOffSetLeft(0);
            }
            else if (scrollOffSetRight + f > maxScrollOffSetLeft)
            {
                SetScrollOffSetLeft(maxScrollOffSetLeft);
            }

        }

        static internal void SetScrollOffSetRight(float f)
        {
            scrollOffSetRight = f;
            bGenerateMatrixRight = true;
        }

        static internal void AddScrollOffSetRight(float f)
        {
            if (scrollOffSetRight + f >= 0 && scrollOffSetRight + f <= maxScrollOffSetRight)
            {
                scrollOffSetRight += f;
                SetScrollOffSetRight(scrollOffSetRight);
            }
            else if (scrollOffSetRight + f < 0)
            {
                SetScrollOffSetRight(0);
            }
            else if (scrollOffSetRight + f > maxScrollOffSetRight)
            {
                SetScrollOffSetRight(maxScrollOffSetRight);
            }

        }

        internal void Initialize()
        {


            bInitialize = true;
            renderSize = new Rectangle(0, 0, 340, 60);
            verticalSpacing = 10;
            iconBox = new Rectangle(0, 0, renderSize.Height, renderSize.Height);

            abilityNameFont = BattleGUI.testSF25;
            int hSpacingAbiBox = 5;
            int titleHeight = 40;
            abilityTitleBox = new Rectangle(hSpacingAbiBox + iconBox.Width, 0, renderSize.Width - iconBox.Width - hSpacingAbiBox, titleHeight);
            abilityCostBox = new Rectangle(hSpacingAbiBox + iconBox.Width, abilityTitleBox.Height, renderSize.Width - iconBox.Width - hSpacingAbiBox, renderSize.Height - abilityTitleBox.Height);


        }

        static internal void InitializeLeft(Rectangle cl, List<AbilitySelectTab> tabs)
        {
            containerLeft = cl;
            sideRenderLeft = new RenderTarget2D(Game1.graphics.GraphicsDevice, cl.Width, cl.Height);

            int index = tabs.Count;
            if (index > 1)
            {
                maxScrollOffSetLeft = (index - 1) * renderSize.Height + (index - 2) * verticalSpacing;
            }
            else
            {
                maxScrollOffSetLeft = 0f;
            }


        }

        static internal void InitializeRight(Rectangle cr, List<AbilitySelectTab> tabs)
        {
            containerRight = cr;
            sideRenderRight = new RenderTarget2D(Game1.graphics.GraphicsDevice, cr.Width, cr.Height);

            int index = tabs.Count;
            if (index > 1)
            {
                maxScrollOffSetRight = (index - 1) * renderSize.Height + (index - 2) * verticalSpacing;
            }
            else
            {
                maxScrollOffSetRight = 0f;
            }

        }


        internal AbilitySelectTab(BasicAbility ba, Rectangle containerPanel, int index)
        {
            if (!bInitialize)
            {
                Initialize();
            }



            parent = ba;
            this.index = index;

            positionOnscreen = new Rectangle(containerPanel.X, containerPanel.Y + index * renderSize.Height + index * verticalSpacing, renderSize.Width, renderSize.Height);
            drawPositionOnscreen = new Rectangle(0, 0 + index * renderSize.Height + index * verticalSpacing, renderSize.Width, renderSize.Height);
            render = new RenderTarget2D(Game1.graphics.GraphicsDevice, renderSize.Width, renderSize.Height);
        }

        internal void Update(GameTime gt)
        {
            if (bGenerateMatrixRight)
            {
                mRight = Matrix.CreateTranslation(0, scrollOffSetRight, 1);
                bGenerateMatrixRight = false;
            }

            if (bGenerateMatrixLeft)
            {
                mLeft = Matrix.CreateTranslation(0, scrollOffSetLeft, 1);
                bGenerateMatrixLeft = false;
            }

            if (selectionTimer != null)
            {
                selectionTimer.Tick(gt);
            }

            if (GameMenuHandler.selectedCharacterContext.abilityLineupInfo.getSelectedAbility() == this)
            {
                if (!selectionTimer.IsActive() && bBuildup)
                {
                    bBuildup = false;
                    selectionTimer = new TimingUtility(32, true, StopSelectionTimerWhen);
                    selectionTimer.SetStepTimer(30);
                }
                else if (!selectionTimer.IsActive() && !bBuildup)
                {
                    bBuildup = true;
                    selectionTimer = new TimingUtility(32, true, StopSelectionTimerWhen);
                    selectionTimer.SetStepTimer(30);
                }

            }
            else if (selectionTimer != null && !selectionTimer.IsActive() && bBuildup)
            {
                selectionTimer = new TimingUtility(8, true, StopSelectionTimerWhen);
                selectionTimer.SetStepTimer(30);
                bBuildup = false;


            }
            else if (selectionTimer != null && !selectionTimer.IsActive() && !bBuildup)
            {

                selectionTimer = null;
                bBuildup = true;

            }

            parent.abilityIcon.UpdateAnimationForItems(gt);
        }

        internal void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            parent.abilityIcon.Draw(sb, iconBox);
            TextUtility.Draw(sb, parent.GetName(), abilityNameFont, abilityTitleBox, TextUtility.OutLining.Left, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);
            TextUtility.Draw(sb, parent.GetCosts(), abilityNameFont, abilityCostBox, TextUtility.OutLining.Left, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);

            sb.End();

        }

        internal static void GenerateCompleteRenderRight(SpriteBatch sb, List<AbilitySelectTab> tabs)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(sideRenderRight);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, mRight);
            for (int i = 0; i < tabs.Count; i++)
            {
                tabs[i].DrawRender(sb);
            }

            sb.End();
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(completeRenderRight);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            sb.Draw(sideRenderRight, containerRight, sideRenderRight.Bounds, Color.White);

            sb.End();
        }

        internal static RenderTarget2D GetRenderRight()
        {
            return completeRenderRight;
        }

        internal static void GenerateCompleteRenderLeft(SpriteBatch sb, List<AbilitySelectTab> tabs)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(sideRenderLeft);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, mLeft);
            for (int i = 0; i < tabs.Count; i++)
            {
                tabs[i].DrawRender(sb);
            }

            sb.End();
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(completeRenderLeft);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            //sb.Draw(Game1.WhiteTex, containerLeft,Color.Red);
            sb.Draw(sideRenderLeft, containerLeft, sideRenderLeft.Bounds, Color.White);
            //sb.Draw(sideRenderLeft,sideRenderLeft.Bounds,Color.White);

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
        }

        internal static RenderTarget2D GetRenderLeft()
        {
            return completeRenderLeft;
        }

        internal RenderTarget2D getRender()
        {
            return render;
        }

        internal bool containsMouseRight(Point p)
        {
            if (!containerRight.Contains(p))
            {
                return false;
            }
            Point temp = new Point(p.X, (int)(p.Y - scrollOffSetRight));
            if (positionOnscreen.Contains(temp))
            {
                return true;
            }
            return false;
        }

        internal bool containsMouseLeft(Point p)
        {
            if (!containerLeft.Contains(p))
            {
                return false;
            }
            Point temp = new Point(p.X, (int)(p.Y - scrollOffSetRight));
            if (positionOnscreen.Contains(temp))
            {
                return true;
            }
            return false;
        }

        internal void DrawRender(SpriteBatch sb)
        {
            var temp = GameMenuHandler.selectedCharacterContext.abilityLineupInfo.getSelectedAbility();

            if (selectionTimer != null)
            {
                if (bBuildup)
                {
                    sb.Draw(Game1.WhiteTex, drawPositionOnscreen, Color.DarkGray * selectionTimer.percentageDone() * .4f);
                }
                else if (!bBuildup)
                {
                    sb.Draw(Game1.WhiteTex, drawPositionOnscreen, Color.DarkGray * (1.0f - selectionTimer.percentageDone()) * .4f);
                }
            }

            if (temp != null)
            {
                if (temp == this)
                {
                    sb.Draw(getRender(), drawPositionOnscreen, Color.White);
                }
                else
                {
                    sb.Draw(getRender(), drawPositionOnscreen, Color.White * .5f);
                }
            }
            else
            {
                sb.Draw(getRender(), drawPositionOnscreen, Color.White);
            }

        }

        internal BasicAbility getParent()
        {
            return parent;
        }

        internal void Select()
        {
            GameMenuHandler.selectedCharacterContext.abilityLineupInfo.setSelectedAbility(this);
            selectionTimer = new TimingUtility(32, true, StopSelectionTimerWhen);
            selectionTimer.SetStepTimer(30);
        }

        internal bool StopSelectionTimerWhen()
        {
            if (selectionTimer.percentageDone() == 1.0f)
            {

                return true;
            }
            return false;
        }

        internal int distanceY()
        {
            float os = 0f;
            if (GameMenuHandler.selectedCharacterContext.abilityLineupInfo.bCursorFocusRight) { os = scrollOffSetRight; }else { os = scrollOffSetLeft; }
            return (int)(positionOnscreen.Y - os);
        }
    }

    internal class AbilitySelectDescription
    {
        TimingUtility startUpTimer;
        static int startUpStepTimer = 90;
        BasicAbility parent;
        Rectangle pos;
        Rectangle drawOnScreen;
        float scroll = 0.0f;
        Matrix m;
        bool bGenerateCamera = true;
        RenderTarget2D render;
        TexPanel panel;
        Rectangle titleBox;
        Rectangle descriptionBox;
        Rectangle aoeBox;
        String potencyText;

        static SpriteFont font;
        static Texture2D guiTex;
        static bool bInitialize = true;
        static int offsetPanel = 10;
        static int offsetBoxes = 10;
        static int titleBoxHeight = 40;
        static GameText expectedDMGText;


        internal AbilitySelectDescription(BaseCharacter bc, BasicAbility ba, Rectangle screenPos)
        {
            if (bInitialize)
            {
                Initialize();
            }
            parent = ba;
           // GameProcessor.gcDB
            drawOnScreen = screenPos;
            pos = new Rectangle(new Point(), screenPos.Size);

            render = new RenderTarget2D(Game1.graphics.GraphicsDevice, pos.Width, pos.Height);
            panel = new TexPanel(guiTex, pos, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

            startUpTimer = new TimingUtility(16, true, StartUpTimer);
            startUpTimer.SetStepTimer(startUpStepTimer);

            titleBox = new Rectangle(offsetPanel, offsetPanel, render.Width - 2 * offsetPanel, titleBoxHeight);
            descriptionBox = new Rectangle(titleBox.Location + new Point(0, titleBoxHeight + offsetBoxes), new Point(render.Width - 2 * offsetPanel));
            aoeBox = new Rectangle(new Point(0, descriptionBox.Y + descriptionBox.Height + offsetBoxes), new Point(render.Width - 2 * offsetPanel));
            potencyText = ba.GetEstimatedPotency(bc);
        }

        static internal void Initialize()
        {
            font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");//48,32,25,20
            bInitialize = false;
            guiTex = GameMenuHandler.menuTextureSheet;
            offsetPanel = 10;
            offsetBoxes = 10;
            expectedDMGText = new GameText("Estimated Potency: ");
        }

        internal void Update(GameTime gt)
        {
            if (bGenerateCamera)
            {
                m = Matrix.CreateTranslation(0, -scroll, 1f);
            }

            startUpTimer.Tick(gt);
        }

        internal void Draw(SpriteBatch sb)
        {
            sb.Draw(render, drawOnScreen, Color.White * startUpTimer.percentageDone());
        }

        internal void GenerateRender(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred,null,SamplerState.PointClamp);

            panel.Draw(sb, Color.White);
            TextUtility.Draw(sb, expectedDMGText.getText() + potencyText, font, titleBox, TextUtility.OutLining.Left, Color.White, 1f, true, default(Matrix), Color.Silver, false);
            if (!parent.abilityDescription.Equals("",StringComparison.OrdinalIgnoreCase))
            {
                TextUtility.DrawComplex(sb, parent.abilityDescription, font, descriptionBox, TextUtility.OutLining.Center, Color.LightGray, 1f, true, default(Matrix), Color.Silver);
            }
          

            sb.End();
        }

        internal bool StartUpTimer()
        {
            if (startUpTimer.percentageDone() >= 1.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void Close()
        {
            render.Dispose();
            startUpTimer.Stop();
        }
    }
}
