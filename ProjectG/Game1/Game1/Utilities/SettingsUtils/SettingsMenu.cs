using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;
using TBAGW.Utilities.Control.Player;

namespace TBAGW
{
    public static class SettingsMenu
    {
        internal enum subMenu { None, Save, Load, Settings, Exit }
        static internal subMenu currentSubMenu = subMenu.None;

        static SpriteFont font;
        static Texture2D sourceTex;
        static TexPanel source;
        static TexPanel bigBGPanel;

        static TexPanel savePanel;
        static TexPanel loadPanel;
        static TexPanel SettingsPanel;
        static TexPanel exitPanel;
        static List<TexPanel> panels;
        static List<GameText> texts;

        static GameText savePanelText = new GameText("Save Game");
        static GameText loadPanelText = new GameText("Load Game");
        static GameText settingsText = new GameText("Settings");
        static GameText exitText = new GameText("Exit");

        static public bool bIsRunning = false;
        static PlayerController.Controllers previousController = PlayerController.Controllers.NonCombat;
        static int selectIndex = -1;
        static internal bool bSubMenu = false;

        static bool bInitialize = true;

        static LoadGameScreen loadGS;
        static ConfirmExit cExit;
        static RenderTarget2D render;

        internal static void Initialize()
        {
            render = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            bInitialize = false;
            sourceTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
            int bigWidth = 300;
            int bigHeight = 470;
            Rectangle bigPosition = new Rectangle((1366 - bigWidth) / 2, (768 - bigHeight) / 2, bigWidth, bigHeight);
            source = new TextTexPanel(sourceTex, bigPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

            bigBGPanel = source.positionCopy(bigPosition);
            font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");

            int buttonAmount = 4;
            int offSetY = 45;
            int offSetX = 30;
            int buttonHeight = ((bigHeight - 2 * offSetY) - (buttonAmount - 1) * offSetY) / buttonAmount;
            Rectangle panelsize = new Rectangle(offSetX, offSetY, bigWidth - 2 * offSetX, buttonHeight);
            panelsize.Location += bigPosition.Location;
            savePanel = bigBGPanel.positionCopy(panelsize);
            panelsize.Location += new Point(0, buttonHeight + offSetY);
            loadPanel = bigBGPanel.positionCopy(panelsize);
            panelsize.Location += new Point(0, buttonHeight + offSetY);
            SettingsPanel = bigBGPanel.positionCopy(panelsize);
            panelsize.Location += new Point(0, buttonHeight + offSetY);
            exitPanel = bigBGPanel.positionCopy(panelsize);
            panelsize.Location += new Point(0, buttonHeight + offSetY);

            panels = new List<TexPanel> { savePanel, loadPanel, SettingsPanel, exitPanel };
            texts = new List<GameText> { savePanelText, loadPanelText, settingsText, exitText };
        }

        internal static void HandleCancel()
        {
            if (!bSubMenu)
            {
                Close();
                return;
            }
            else
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Save:
                        break;
                    case subMenu.Load:
                        loadGS.Close();
                        CloseSubMenu();
                        break;
                    case subMenu.Settings:
                        break;
                    case subMenu.Exit:
                        CloseSubMenu();
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Start()
        {
            if (bInitialize) { Initialize(); }
            if (previousController != PlayerController.Controllers.GameSettingsMenu && previousController != PlayerController.Controllers.GameOptions)
            {
                previousController = PlayerController.currentController;
            }

            bIsRunning = true;
            PlayerController.currentController = PlayerController.Controllers.GameSettingsMenu;
            bSubMenu = false;
        }

        public static void Close()
        {
            bIsRunning = false;
            PlayerController.currentController = previousController;
        }

        public static void Update(GameTime gt)
        {
            PlayerController.Update(gt);
            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        bSubMenu = false;
                        break;
                    case subMenu.Save:
                        break;
                    case subMenu.Load:
                        loadGS.Update(gt);
                        break;
                    case subMenu.Settings:
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Draw(SpriteBatch sb, RenderTarget2D bgRender = null)
        {

            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Save:
                        break;
                    case subMenu.Load:
                        loadGS.GenerateRenders(sb);
                        break;
                    case subMenu.Settings:
                        break;
                    default:
                        break;
                }
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            sb.Draw(GameProcessor.lastDrawnMapRender, new Rectangle(0, 0, 1366, 768), Color.White * .5f);

            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Save:
                        break;
                    case subMenu.Load:
                        sb.Draw(loadGS.getRender(), new Vector2(0), Color.White);

                        break;
                    case subMenu.Settings:
                        break;
                    case subMenu.Exit:
                        cExit.Draw(sb);
                        break;
                    default:
                        break;
                }
                BattleGUI.DrawCursor(sb, KeyboardMouseUtility.uiMousePos.ToVector2(), 1f);
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(null);
                return;
            }


            bigBGPanel.Draw(sb, Color.Gray);

            float opacity = 0.5f;

            for (int i = 0; i < panels.Count; i++)
            {
                if (i == selectIndex)
                {
                    panels[i].Draw(sb, Color.White);
                }
                else
                {
                    panels[i].Draw(sb, Color.White * opacity);
                }
            }

            for (int i = 0; i < texts.Count; i++)
            {
                if (i == selectIndex)
                {
                    TextUtility.Draw(sb, texts[i].getText(), font, panels[i].Position(), TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                }
                else
                {
                    TextUtility.Draw(sb, texts[i].getText(), font, panels[i].Position(), TextUtility.OutLining.Center, Color.Gold * opacity, 1f, false, default(Matrix), Color.Silver * opacity, false);
                }
            }

            BattleGUI.DrawCursor(sb, KeyboardMouseUtility.uiMousePos.ToVector2(), 1f);

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.Clear(Color.Red);
        }

        public static void HandleMouseMove()
        {

            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Save:
                        break;
                    case subMenu.Load:
                        loadGS.HandleMouseMove();
                        break;
                    case subMenu.Settings:
                        break;
                    case subMenu.Exit:
                        cExit.HandleMouseMove();
                        break;
                    default:
                        break;
                }
                return;
            }

            selectIndex = panels.IndexOf(panels.Find(p => p.ContainsMouse(KeyboardMouseUtility.uiMousePos)));
        }

        public static void HandleUpDown(bool bDown)
        {
            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Save:
                        break;
                    case subMenu.Load:
                        if (!bDown) { loadGS.HandleUp(); } else { loadGS.HandleDown(); }

                        break;
                    case subMenu.Settings:
                        break;
                    case subMenu.Exit:
                        break;
                    default:
                        break;
                }
                return;
            }

            if (bDown)
            {
                selectIndex++;
                if (selectIndex == panels.Count)
                {
                    selectIndex = 0;
                }
            }
            else
            {
                selectIndex--;
                if (selectIndex < 0)
                {
                    selectIndex = panels.Count - 1;
                }
            }
        }

        public static void HandleConfirmOrClick()
        {
            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Save:
                        if (KeyboardMouseUtility.bMouseButtonPressed)
                        {
                            loadGS.HandleMouseClick();
                        }
                        else
                        {
                            loadGS.HandleConfirm();
                        }
                        break;
                    case subMenu.Load:

                        if (KeyboardMouseUtility.bMouseButtonPressed)
                        {
                            loadGS.HandleMouseClick();
                        }
                        else
                        {
                            loadGS.HandleConfirm();
                        }

                        break;
                    case subMenu.Settings:
                        break;
                    case subMenu.Exit:
                        cExit.HandleConfirm();
                        break;
                    default:
                        break;
                }
                return;
            }

            if (selectIndex == -1)
            {
                return;
            }
            switch (selectIndex)
            {
                case 0: //Save
                    LoadGameScreen.HasSaveFiles(SaveDataProcessor.saveFolder);
                    LoadGameScreen lgs = new LoadGameScreen(SaveDataProcessor.saveFolder, HandleSaveLoc);
                    bSubMenu = true;
                    loadGS = lgs;

                    currentSubMenu = subMenu.Load;
                    break;
                case 1: //Load
                    if (LoadGameScreen.HasSaveFiles(SaveDataProcessor.saveFolder))
                    {
                        lgs = new LoadGameScreen(SaveDataProcessor.saveFolder);
                        bSubMenu = true;
                        loadGS = lgs;

                        currentSubMenu = subMenu.Load;
                    }
                    break;
                case 2: //Settings
                    Close();
                    OptionsMenu.Start(BackFromOptions);
                    break;
                case 3: //Exit
                    cExit = new ConfirmExit();
                    cExit.Start(CloseSubMenu);
                    bSubMenu = true;
                    currentSubMenu = subMenu.Exit;
                    break;
            }
        }

        public static void CloseSubMenu()
        {
            currentSubMenu = subMenu.None;
            bSubMenu = false;
        }

        public static void HandleLeftRight(bool bRight)
        {
            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Save:
                        break;
                    case subMenu.Load:
                        break;
                    case subMenu.Settings:
                        break;
                    case subMenu.Exit:
                        cExit.HandleLeftRight();
                        break;
                    default:
                        break;
                }
            }
        }

        internal static RenderTarget2D getRender()
        {
            return render;
        }

        internal static void HandleSaveLoc(String dataName)
        {
            SaveDataProcessor.OverwriteSave(dataName);
            if (bSubMenu) { CloseSubMenu(); }
        }

        internal static void BackFromOptions()
        {
            SettingsMenu.Close();
            Start();
            CloseSubMenu();
        }
    }

    public class ConfirmExit
    {
        static SpriteFont font;
        static bool bInitialize = true;
        static TexPanel source;
        static TexPanel exitTextPanel;
        static TexPanel confirmPanel;
        static TexPanel cancelPanel;

        static Rectangle ExitTextBox;
        static GameText confirmText;
        static GameText cancelText;
        static GameText LeaveText;

        int selectIndex = -1;

        public delegate void cancelFunction();
        cancelFunction cancelFunc;

        internal static void Initialize()
        {
            font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");
            bInitialize = false;
            Texture2D sourceTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
            int bigWidth = 300;
            int bigHeight = 200;
            Rectangle bigPosition = new Rectangle((1366 - bigWidth) / 2, (768 - bigHeight) / 2, bigWidth, bigHeight);
            source = new TexPanel(sourceTex, bigPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            int wOS = 10;
            int hOS = 10;
            int heightTB = 55;
            ExitTextBox = new Rectangle(bigPosition.Location + new Point(wOS, hOS), new Point(bigPosition.Width, heightTB) - new Point(2 * wOS, 0));

            LeaveText = new GameText("Press Exit to exit, the game\nwill autosave. (separate)");
            confirmText = new GameText("Exit");
            cancelText = new GameText("Cancel");

            confirmPanel = source.positionCopy(new Rectangle(bigPosition.X + wOS, bigPosition.Y + heightTB + 2 * hOS, (bigWidth - 3 * wOS) / 2, bigHeight - heightTB - 3 * hOS));
            cancelPanel = source.positionCopy(new Rectangle(bigPosition.X + wOS * 2 + (bigWidth - 3 * wOS) / 2, bigPosition.Y + heightTB + 2 * hOS, (bigWidth - 3 * wOS) / 2, bigHeight - heightTB - 3 * hOS));
            exitTextPanel = source.positionCopy(bigPosition);
        }

        public void Start(cancelFunction cf)
        {
            if (bInitialize) { Initialize(); }
            cancelFunc = cf;
        }

        internal void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            source.Draw(sb, Color.White);
            exitTextPanel.Draw(sb, Color.White);
            float opacity = 0.5f;

            if (selectIndex == -1)
            {
                confirmPanel.Draw(sb, Color.White * opacity);
                cancelPanel.Draw(sb, Color.White * opacity);
            }
            else if (selectIndex == 0)
            {
                confirmPanel.Draw(sb, Color.White);
                cancelPanel.Draw(sb, Color.White * opacity);
            }
            else if (selectIndex == 1)
            {
                confirmPanel.Draw(sb, Color.White * opacity);
                cancelPanel.Draw(sb, Color.White);
            }


            TextUtility.Draw(sb, LeaveText.getText(), font, ExitTextBox, TextUtility.OutLining.Center, Color.Gold, 1f, true, default(Matrix), Color.Silver, false);


            if (selectIndex == -1)
            {
                TextUtility.Draw(sb, confirmText.getText(), font, confirmPanel.Position(), TextUtility.OutLining.Center, Color.Gold * opacity, 1f, false, default(Matrix), Color.Silver * opacity, false);
                TextUtility.Draw(sb, cancelText.getText(), font, cancelPanel.Position(), TextUtility.OutLining.Center, Color.Gold * opacity, 1f, false, default(Matrix), Color.Silver * opacity, false);
            }
            else if (selectIndex == 0)
            {
                TextUtility.Draw(sb, confirmText.getText(), font, confirmPanel.Position(), TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                TextUtility.Draw(sb, cancelText.getText(), font, cancelPanel.Position(), TextUtility.OutLining.Center, Color.Gold * opacity, 1f, false, default(Matrix), Color.Silver * opacity, false);
            }
            else if (selectIndex == 1)
            {
                TextUtility.Draw(sb, confirmText.getText(), font, confirmPanel.Position(), TextUtility.OutLining.Center, Color.Gold * opacity, 1f, false, default(Matrix), Color.Silver * opacity, false);
                TextUtility.Draw(sb, cancelText.getText(), font, cancelPanel.Position(), TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
            }
        }

        internal void HandleMouseMove()
        {
            if (confirmPanel.ContainsMouse(KeyboardMouseUtility.uiMousePos))
            {
                selectIndex = 0;
                return;
            }

            if (cancelPanel.ContainsMouse(KeyboardMouseUtility.uiMousePos))
            {
                selectIndex = 1;
                return;
            }

            selectIndex = -1;
        }

        internal void HandleLeftRight()
        {
            selectIndex++;
            if (selectIndex > 1)
            {
                selectIndex = 0;
            }
        }

        internal void HandleConfirm()
        {
            if (selectIndex == -1)
            {
                return;
            }

            if (selectIndex == 0)
            {
                SaveDataProcessor.QuickSave();
                Game1.ExitGame();
                return;
            }

            if (selectIndex == 1)
            {
                cancelFunc();
                return;
            }
        }
    }

    public static class OptionsMenu
    {
        internal enum subMenu { None, Audio, Display, Visual, Controls }
        static internal subMenu currentSubMenu = subMenu.None;

        static SpriteFont font;
        static Texture2D sourceTex;
        static TexPanel source;
        static TexPanel bigBGPanel;

        static TexPanel savePanel;
        static TexPanel loadPanel;
        static TexPanel SettingsPanel;
        static TexPanel exitPanel;
        static List<TexPanel> panels;
        static List<GameText> texts;

        static GameText savePanelText = new GameText("Audio");
        static GameText loadPanelText = new GameText("Display");
        static GameText settingsText = new GameText("Visual");
        static GameText exitText = new GameText("Controls");

        static public bool bIsRunning = false;
        static PlayerController.Controllers previousController = PlayerController.Controllers.NonCombat;
        static int selectIndex = -1;
        static internal bool bSubMenu = false;

        static bool bInitialize = true;

        static RenderTarget2D render;
        public delegate void backFunction();
        public static backFunction backFunc;

        static DisplayPage dp = null;
        static AudioPage ap = null;
        static ControlsPage cp = null;

        internal static void Initialize()
        {
            render = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
            bInitialize = false;
            sourceTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
            int bigWidth = 300;
            int bigHeight = 470;
            Rectangle bigPosition = new Rectangle((1366 - bigWidth) / 2, (768 - bigHeight) / 2, bigWidth, bigHeight);
            source = new TextTexPanel(sourceTex, bigPosition, new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

            bigBGPanel = source.positionCopy(bigPosition);
            font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test32");

            int buttonAmount = 4;
            int offSetY = 45;
            int offSetX = 30;
            int buttonHeight = ((bigHeight - 2 * offSetY) - (buttonAmount - 1) * offSetY) / buttonAmount;
            Rectangle panelsize = new Rectangle(offSetX, offSetY, bigWidth - 2 * offSetX, buttonHeight);
            panelsize.Location += bigPosition.Location;
            savePanel = bigBGPanel.positionCopy(panelsize);
            panelsize.Location += new Point(0, buttonHeight + offSetY);
            loadPanel = bigBGPanel.positionCopy(panelsize);
            panelsize.Location += new Point(0, buttonHeight + offSetY);
            SettingsPanel = bigBGPanel.positionCopy(panelsize);
            panelsize.Location += new Point(0, buttonHeight + offSetY);
            exitPanel = bigBGPanel.positionCopy(panelsize);
            panelsize.Location += new Point(0, buttonHeight + offSetY);

            panels = new List<TexPanel> { savePanel, loadPanel, SettingsPanel, exitPanel };
            texts = new List<GameText> { savePanelText, loadPanelText, settingsText, exitText };
        }

        internal static void HandleCancel()
        {
            if (!bSubMenu)
            {
                Close();
                return;
            }
            else
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Audio:
                        ap.HandleCancel();
                        break;
                    case subMenu.Display:
                        dp.HandleCancel();
                        break;
                    case subMenu.Visual:
                        break;
                    case subMenu.Controls:
                        cp.HandleCancel();
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Start(backFunction f)
        {
            if (bInitialize) { Initialize(); }
            if (previousController != PlayerController.Controllers.GameSettingsMenu && previousController != PlayerController.Controllers.GameOptions)
            {
                previousController = PlayerController.currentController;
            }
            bIsRunning = true;
            PlayerController.currentController = PlayerController.Controllers.GameOptions;
            bSubMenu = false;
            backFunc = f;
        }

        public static void Close()
        {
            bIsRunning = false;
            PlayerController.currentController = previousController;
            if (dp != null) { dp.Reset(); }
            if (ap != null) { ap.Reset(); }
            Utilities.ReadWrite.EditorFileWriter.SaveSettingsWriter();
        }

        public static void Update(GameTime gt)
        {
            PlayerController.Update(gt);
            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        bSubMenu = false;
                        break;
                    case subMenu.Audio:
                        if (ap != null)
                        {
                            ap.Update(gt);
                            return;
                        }
                        break;
                    case subMenu.Display:
                        if (dp != null)
                        {
                            dp.Update(gt);
                            return;
                        }
                        break;
                    case subMenu.Visual:
                        break;
                    case subMenu.Controls:
                        if (cp != null)
                        {
                            cp.Update(gt);
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public static void Draw(SpriteBatch sb, RenderTarget2D bgRender = null)
        {
            //Generate renders here
            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Audio:
                        if (ap != null)
                        {
                            ap.GenerateRenders(sb);
                            ap.Draw(sb);
                        }
                        break;
                    case subMenu.Display:
                        if (dp != null)
                        {
                            dp.GenerateRenders(sb);
                            dp.Draw(sb);
                            // return;
                        }
                        break;
                    case subMenu.Visual:
                        break;
                    case subMenu.Controls:
                        if (cp != null)
                        {
                            cp.GenerateRenders(sb);
                            cp.Draw(sb);
                            // return;
                        }
                        break;
                    default:
                        break;
                }
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(render);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            if (bgRender != null)
            {
                sb.Draw(bgRender, new Rectangle(0, 0, 1366, 768), Color.White * .5f);
            }


            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Audio:
                        if (ap != null)
                        {
                            sb.Draw(ap.getRender(), ap.getDrawPos(), Color.White);
                        }
                        break;
                    case subMenu.Display:
                        if (dp != null)
                        {
                            sb.Draw(dp.getRender(), dp.getDrawPos(), Color.White);
                        }
                        break;
                    case subMenu.Visual:
                        break;
                    case subMenu.Controls:
                        if (cp != null)
                        {
                            sb.Draw(cp.getRender(), cp.getDrawPos(), Color.White);
                        }
                        break;
                    default:
                        break;
                }
                BattleGUI.DrawCursor(sb, KeyboardMouseUtility.uiMousePos.ToVector2(), 1f);
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(null);
                return;
            }


            bigBGPanel.Draw(sb, Color.Gray);

            float opacity = 0.5f;

            for (int i = 0; i < panels.Count; i++)
            {
                if (i == selectIndex)
                {
                    panels[i].Draw(sb, Color.White);
                }
                else
                {
                    panels[i].Draw(sb, Color.White * opacity);
                }
            }

            for (int i = 0; i < texts.Count; i++)
            {
                if (i == selectIndex)
                {
                    TextUtility.Draw(sb, texts[i].getText(), font, panels[i].Position(), TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                }
                else
                {
                    TextUtility.Draw(sb, texts[i].getText(), font, panels[i].Position(), TextUtility.OutLining.Center, Color.Gold * opacity, 1f, false, default(Matrix), Color.Silver * opacity, false);
                }
            }

            BattleGUI.DrawCursor(sb, KeyboardMouseUtility.uiMousePos.ToVector2(), 1f);

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.Clear(Color.Red);
        }

        public static void HandleMouseMove()
        {

            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Audio:
                        if (ap != null)
                        {
                            ap.HandleMouseMove();
                        }
                        break;
                    case subMenu.Display:
                        if (dp != null)
                        {
                            dp.HandleMouseMove();
                        }
                        break;
                    case subMenu.Visual:
                        break;
                    case subMenu.Controls:
                        if (cp != null)
                        {
                            cp.HandleMouseMove();
                        }
                        break;
                    default:
                        break;
                }
                return;
            }

            selectIndex = panels.IndexOf(panels.Find(p => p.ContainsMouse(KeyboardMouseUtility.uiMousePos)));
        }

        public static void HandleUpDown(bool bDown)
        {
            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Audio:
                        if (ap != null)
                        {
                            if (bDown)
                            {
                                ap.HandleDown();
                            }
                            else
                            {
                                ap.HandleUp();
                            }
                        }
                        break;
                    case subMenu.Display:
                        if (dp != null)
                        {
                            if (bDown)
                            {
                                dp.SelectPrevious();
                            }
                            else
                            {
                                dp.SelectNext();
                            }
                        }
                        break;
                    case subMenu.Visual:
                        break;
                    case subMenu.Controls:
                        if (cp != null)
                        {
                            if (bDown)
                            {
                                cp.SelectPrevious();
                            }
                            else
                            {
                                cp.SelectNext();
                            }
                        }
                        break;
                    default:
                        break;
                }
                return;
            }

            if (bDown)
            {
                selectIndex++;
                if (selectIndex == panels.Count)
                {
                    selectIndex = 0;
                }
            }
            else
            {
                selectIndex--;
                if (selectIndex < 0)
                {
                    selectIndex = panels.Count - 1;
                }
            }
        }

        public static void HandleConfirmOrClick()
        {
            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Audio:
                        ap.HandleConfirmAndClick();
                        break;
                    case subMenu.Display:
                        dp.HandleConfirmAndClick();
                        break;
                    case subMenu.Visual:
                        break;
                    case subMenu.Controls:
                        cp.HandleConfirmAndClick();
                        break;
                    default:
                        break;
                }
                return;
            }

            if (selectIndex == -1)
            {
                return;
            }
            switch (selectIndex)
            {
                case 0: //AUDIO
                    if (ap == null)
                    {
                        ap = new AudioPage();
                    }

                    OptionsMenu.currentSubMenu = subMenu.Audio;
                    OptionsMenu.bSubMenu = true;
                    break;
                case 1: //DISPLAY
                    if (dp == null)
                    {
                        dp = new DisplayPage();
                    }

                    OptionsMenu.currentSubMenu = subMenu.Display;
                    OptionsMenu.bSubMenu = true;
                    break;
                case 2: //VISUAL
                    break;
                case 3: //Controls
                    if (cp == null)
                    {
                        cp = new ControlsPage();
                    }

                    OptionsMenu.currentSubMenu = subMenu.Controls;
                    OptionsMenu.bSubMenu = true;
                    break;
            }
        }

        public static void CloseSubMenu()
        {
            currentSubMenu = subMenu.None;
            bSubMenu = false;
        }

        public static void HandleLeftRight(bool bRight)
        {
            if (bSubMenu)
            {
                switch (currentSubMenu)
                {
                    case subMenu.None:
                        break;
                    case subMenu.Audio:
                        if (ap != null)
                        {
                            if (bRight)
                            {
                                ap.SelectPrevious();
                            }
                            else
                            {
                                ap.SelectNext();
                            }
                        }
                        break;
                    case subMenu.Display:
                        break;
                    case subMenu.Visual:
                        break;
                    case subMenu.Controls:
                        if (cp != null)
                        {
                            if (bRight)
                            {
                                cp.HandleRight();
                            }
                            else
                            {
                                cp.HandleLeft();
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        internal static RenderTarget2D getRender()
        {
            return render;
        }

        internal static void HandleSaveLoc(String dataName)
        {
            SaveDataProcessor.OverwriteSave(dataName);
            if (bSubMenu) { CloseSubMenu(); }
        }

        public abstract class OptionsPage
        {
            protected static int width = 1000;
            protected static int height = 500;
            protected RenderTarget2D render = new RenderTarget2D(Game1.graphics.GraphicsDevice, width, height);
            protected static int xOS = (1366 - width) / 2;
            protected static int yOS = (768 - height) / 2;
            static Rectangle renderPos = new Rectangle(0, 0, width, height);
            static Rectangle drawPos = new Rectangle(xOS, yOS, width, height);

            protected static TexPanel bgPanel = source.positionCopy(renderPos);

            protected List<SelectableElement> elements = new List<SelectableElement>();
            protected List<KeyValuePair<RenderTarget2D, Rectangle>> elementRenders = new List<KeyValuePair<RenderTarget2D, Rectangle>>();
            internal int selectIndex = -1;

            internal SelectableElement selectedElement = null;

            public OptionsPage() { }

            internal virtual void Update(GameTime gt)
            {
                elements.ForEach(e => e.Update(gt));
            }

            internal virtual void GenerateRenders(SpriteBatch sb)
            {
                elementRenders.Clear();
                elements.ForEach(e => elementRenders.Add(e.DrawReturnRender(sb)));
            }

            internal virtual void Draw(SpriteBatch sb)
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(render);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

                bgPanel.Draw(sb, Color.Gray);

                if (elements.Count != 0 && selectIndex != -1)
                {
                    sb.Draw(Game1.WhiteTex, elements[selectIndex].SelectionPosition(), Color.LightYellow * .8f);
                }
                //elements.ForEach(e => e.Draw(sb));
            }

            internal virtual void Reset()
            {
                elements.ForEach(e => e.bIsSelected = false);
                selectIndex = -1;
            }

            internal virtual RenderTarget2D getRender()
            {
                return render;
            }

            internal virtual Rectangle getDrawPos()
            {
                return drawPos;
            }

            internal virtual void SelectNext()
            {
                selectIndex--;
                if (selectIndex < 0)
                {
                    selectIndex = elements.Count - 1;
                }
                if (elements.Count != 0)
                {
                    selectedElement = elements[selectIndex].bFocusOnSelect ? elements[selectIndex] : null;
                }
            }

            internal virtual void SelectPrevious()
            {
                selectIndex++;
                if (selectIndex >= elements.Count)
                {
                    selectIndex = 0;
                }
                if (elements.Count != 0)
                {
                    selectedElement = elements[selectIndex].bFocusOnSelect ? elements[selectIndex] : null;
                }
            }

            internal virtual void HandleConfirmAndClick()
            {
                if (selectedElement == null)
                {
                    if (selectIndex != -1)
                    {
                        selectedElement = elements[selectIndex];
                        elements.ForEach(e => e.bIsSelected = false);
                        selectedElement.bIsSelected = true;

                        return;
                    }
                }
            }

            internal virtual void HandleCancel()
            {
                if (selectedElement != null)
                {
                    bool bCloseMenu = selectedElement.bFocusOnSelect;
                    selectedElement.bIsSelected = false;
                    selectedElement = null;
                    if (bCloseMenu) { CloseSubMenu(); }
                    return;
                }
                else
                {
                    CloseSubMenu();
                }
            }

            internal virtual void HandleMouseMove()
            {

            }
        }

        public class DisplayPage : OptionsPage
        {
            public DisplayPage() : base()
            {
                int widthOffset = 20;
                int scrollPanelWidth = width - widthOffset * 2;
                int scrollPanelHeight = 250;
                elements.Add(new ScrollPanel(new Point(widthOffset), scrollPanelWidth, scrollPanelHeight, 40, 5, "640x480", "800x480", "1024x600", "1200x900", "1280x720", "1280x1024", "1360x768", "1366x768", "1440x900", "1680x1050", "1600x900", "1600x1200", "1920x1080", "1920x1200", "2560x1600", "2560x1440", "2560x1600"));
                elements.Add(new SelectableButton(new Rectangle(widthOffset, widthOffset * 2 + scrollPanelHeight, scrollPanelWidth, 50), "Change Resolution"));
                (elements.Last() as SelectableButton).SetFunction(AttemptChangeResolutionFunction);
                elements.Add(new SelectableButton(new Rectangle(widthOffset, widthOffset * 3 + scrollPanelHeight + 50, scrollPanelWidth, 50), "Toggle Fullscreen"));
                (elements.Last() as SelectableButton).SetFunction(() => ResolutionUtility.toggleFullscreen());
                SelectPanelItemBasedOnResolution();
            }

            internal override void Draw(SpriteBatch sb)
            {
                base.Draw(sb);
                elements[1].Draw(sb);
                elements[2].Draw(sb);
                sb.Draw(elementRenders[0].Key, elementRenders[0].Value, Color.White);
            }

            internal void SelectPanelItemBasedOnResolution()
            {
                String current = Game1.graphics.PreferredBackBufferWidth + "x" + Game1.graphics.PreferredBackBufferHeight;
                (elements[0] as ScrollPanel).selectIndex = (elements[0] as ScrollPanel).items.ToList().IndexOf(current);
                (elements[0] as ScrollPanel).ChangeIndex((elements[0] as ScrollPanel).items.ToList().IndexOf(current));
            }

            internal override void SelectNext()
            {
                if (selectedElement != null && selectedElement == elements[0] && selectedElement.bIsSelected)
                {
                    (selectedElement as ScrollPanel).HandleUp();
                    return;
                }


                base.SelectNext();


            }

            internal override void SelectPrevious()
            {
                if (selectedElement != null && selectedElement == elements[0] && selectedElement.bIsSelected)
                {
                    (selectedElement as ScrollPanel).HandleDown();
                    return;
                }


                base.SelectPrevious();

            }

            internal override void HandleConfirmAndClick()
            {
                if (selectedElement == null)
                {
                    base.HandleConfirmAndClick();
                    return;
                }

                if (selectedElement == elements[0])
                {
                    (elements[1] as SelectableButton).ExecuteFunction();
                }

                if (selectedElement == elements[1])
                {
                    (elements[1] as SelectableButton).ExecuteFunction();
                }

                if (selectedElement == elements[2])
                {
                    (elements[2] as SelectableButton).ExecuteFunction();
                }
            }

            internal void AttemptChangeResolutionFunction()
            {
                if ((elements[0] as ScrollPanel).selectIndex != -1)
                {
                    String[] args = (elements[0] as ScrollPanel).items[(elements[0] as ScrollPanel).selectIndex].Split('x');
                    int x = int.Parse(args[0]);
                    int y = int.Parse(args[1]);

                    ResolutionUtility.AdjustResolution(x, y, Game1.graphics);
                }
            }

            internal override void HandleMouseMove()
            {
                int i = elements.IndexOf(elements.Find(e => e.elementLoc.Contains(KeyboardMouseUtility.uiMousePos - getDrawPos().Location)));
                selectIndex = i;
                if (selectIndex == -1)
                {
                    selectedElement = null;
                }
                else if (selectIndex != -1)
                {
                    selectedElement = elements[selectIndex];
                }

                if (selectedElement == elements[0])
                {
                    (elements[0] as ScrollPanel).HandleMouse(new Point(xOS, yOS));
                }
            }

            internal override void Update(GameTime gt)
            {
                base.Update(gt);

                if (selectedElement == elements[0])
                {
                    if (KeyboardMouseUtility.ScrollingDown())
                    {
                        (elements[0] as ScrollPanel).HandleScroll(20);
                    }
                    if (KeyboardMouseUtility.ScrollingUp())
                    {
                        (elements[0] as ScrollPanel).HandleScroll(-20);
                    }
                }

            }
        }

        public class AudioPage : OptionsPage
        {
            Rectangle UpcenterTextMaster;
            Rectangle UpcenterTextMusic;
            Rectangle UpcenterTextSoundEffect;

            GameText UpperMaster = new GameText("Master Volume");
            GameText UpperMusic = new GameText("Music Volume");
            GameText UpperSE = new GameText("SE Volume");

            Rectangle centerTextMaster;
            Rectangle centerTextMusic;
            Rectangle centerTextSoundEffect;

            public AudioPage() : base()
            {
                int scale = 2;
                int baseWidth = 18;
                int wOS = 150;
                int s1 = wOS;
                int s2 = (width) / 2;
                int s3 = width - wOS;

                int hOS = 30;
                int hText = 50;
                int heightScroll = height - 4 * hOS - hText;

                int wText = 300;
                centerTextMaster = new Rectangle(s1 - wText / 2, hOS * 3 + heightScroll, wText, hText);
                centerTextMusic = new Rectangle(s2 - wText / 2, hOS * 3 + heightScroll, wText, hText);
                centerTextSoundEffect = new Rectangle(s3 - wText / 2, hOS * 3 + heightScroll, wText, hText);

                UpcenterTextMaster = new Rectangle(s1 - wText / 2, hOS / 2, wText, hText);
                UpcenterTextMusic = new Rectangle(s2 - wText / 2, hOS / 2, wText, hText);
                UpcenterTextSoundEffect = new Rectangle(s3 - wText / 2, hOS / 2, wText, hText);

                elements.Add(new ScrollWheel(new Point(s1 - scale * baseWidth / 2, hOS * 2), heightScroll, baseWidth, scale));
                elements.Add(new ScrollWheel(new Point(s2 - scale * baseWidth / 2, hOS * 2), heightScroll, baseWidth, scale));
                elements.Add(new ScrollWheel(new Point(s3 - scale * baseWidth / 2, hOS * 2), heightScroll, baseWidth, scale));
                (elements[0] as ScrollWheel).bInvertedScroll = true;
                (elements[1] as ScrollWheel).bInvertedScroll = true;
                (elements[2] as ScrollWheel).bInvertedScroll = true;


            }

            internal override void Draw(SpriteBatch sb)
            {
                base.Draw(sb);
                elements[0].Draw(sb);
                elements[1].Draw(sb);
                elements[2].Draw(sb);

                TextUtility.Draw(sb, UpperMaster.getText(), font, UpcenterTextMaster, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                TextUtility.Draw(sb, UpperMusic.getText(), font, UpcenterTextMusic, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                TextUtility.Draw(sb, UpperSE.getText(), font, UpcenterTextSoundEffect, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);

                TextUtility.Draw(sb, (elements[0] as ScrollWheel).getPercentageHundred().ToString(), font, centerTextMaster, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                TextUtility.Draw(sb, (elements[1] as ScrollWheel).getPercentageHundred().ToString(), font, centerTextMusic, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                TextUtility.Draw(sb, (elements[2] as ScrollWheel).getPercentageHundred().ToString(), font, centerTextSoundEffect, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
            }

            internal override void SelectNext()
            {


                base.SelectNext();


            }

            internal override void SelectPrevious()
            {


                base.SelectPrevious();

            }

            internal override void HandleConfirmAndClick()
            {
                if (selectedElement == null)
                {
                    base.HandleConfirmAndClick();
                    return;
                }
            }

            internal override void HandleMouseMove()
            {
                int i = elements.IndexOf(elements.Find(e => e.elementLoc.Contains(KeyboardMouseUtility.uiMousePos - getDrawPos().Location)));
                selectIndex = i;
                if (selectIndex == -1)
                {
                    selectedElement = null;
                }
                else if (selectIndex != -1)
                {
                    selectedElement = elements[selectIndex];
                }
            }

            internal override void Update(GameTime gt)
            {
                base.Update(gt);

                if (selectedElement != null)
                {
                    if (KeyboardMouseUtility.ScrollingDown())
                    {
                        HandleDown();
                    }
                    if (KeyboardMouseUtility.ScrollingUp())
                    {
                        HandleUp();
                    }
                }

                (elements[0] as ScrollWheel).SetScrollBarPos((float)SceneUtility.masterVolume / 100f);
                (elements[1] as ScrollWheel).SetScrollBarPos((float)SceneUtility.musicVolume / 100f);
                (elements[2] as ScrollWheel).SetScrollBarPos((float)SceneUtility.soundEffectsVolume / 100f);

            }

            internal void HandleDown()
            {
                if (selectedElement != null)
                {
                    if (selectedElement == elements[0])
                    {
                        SceneUtility.masterVolume -= 5;
                        if (SceneUtility.masterVolume < 0)
                        {
                            SceneUtility.masterVolume = 0;
                        }
                    }
                    else if (selectedElement == elements[1])
                    {
                        SceneUtility.musicVolume -= 5;
                        if (SceneUtility.musicVolume < 0)
                        {
                            SceneUtility.musicVolume = 0;
                        }
                    }
                    else if (selectedElement == elements[2])
                    {
                        SceneUtility.soundEffectsVolume -= 5;
                        if (SceneUtility.soundEffectsVolume < 0)
                        {
                            SceneUtility.soundEffectsVolume = 0;
                        }
                    }
                }
            }

            internal void HandleUp()
            {
                if (selectedElement != null)
                {
                    if (selectedElement == elements[0])
                    {
                        SceneUtility.masterVolume += 5;
                        if (SceneUtility.masterVolume > 100)
                        {
                            SceneUtility.masterVolume = 100;
                        }
                    }
                    else if (selectedElement == elements[1])
                    {
                        SceneUtility.musicVolume += 5;
                        if (SceneUtility.musicVolume > 100)
                        {
                            SceneUtility.musicVolume = 100;
                        }
                    }
                    else if (selectedElement == elements[2])
                    {
                        SceneUtility.soundEffectsVolume += 5;
                        if (SceneUtility.soundEffectsVolume > 100)
                        {
                            SceneUtility.soundEffectsVolume = 100;
                        }
                    }
                }
            }
        }

        public class ControlsPage : OptionsPage
        {
            Matrix m = Matrix.CreateTranslation(0, 0, 1);
            int pageScroll = 0;
            int minScroll = 0;
            int maxScroll = 0;

            public ControlsPage() : base()
            {
                int h = 80;
                int hOs = 10;
                int index = 0;
                foreach (var act in Game1.actionList.FindAll(a => a.bUsed))
                {
                    elements.Add(new KeyRebindDesign(new Point(50) + new Point(0, (hOs + h) * index), 900, h, 30, act));
                    index++;
                }

                maxScroll = elements.Last().position.Y - 50;
            }

            internal override void Draw(SpriteBatch sb)
            {
                sb.End();
                sb.GraphicsDevice.SetRenderTarget(render);
                sb.GraphicsDevice.Clear(Color.TransparentBlack);
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

                bgPanel.Draw(sb, Color.Gray);


                if (KeyRebindDesign.bRebindNow)
                {
                    if (selectedElement is KeyRebindDesign)
                    {
                        (selectedElement as KeyRebindDesign).DrawRebind(sb, this);
                    }
                    sb.End();
                    return;
                }
                sb.End();
                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, m);


                if (elements.Count != 0 && selectIndex != -1)
                {
                    sb.Draw(Game1.WhiteTex, elements[selectIndex].SelectionPosition(), Color.LightYellow * .8f);
                }

                if (elements.Count != 0 && selectIndex != -1)
                {
                    sb.Draw(Game1.WhiteTex, elements[selectIndex].SelectionPosition(), Color.LightYellow * .8f);
                }

                foreach (var item in elements)
                {
                    if (item.GetType() == typeof(KeyRebindDesign))
                    {
                        (item as KeyRebindDesign).Draw(sb, m);
                    }
                    else
                    {
                        item.Draw(sb);
                    }

                }

                sb.End();
                sb.GraphicsDevice.SetRenderTarget(null);
            }

            internal override void SelectNext()
            {
                if (KeyRebindDesign.bRebindNow)
                {
                    return;
                }

                base.SelectNext();

                if (selectedElement is KeyRebindDesign)
                {
                    (selectedElement as KeyRebindDesign).Select();
                    elements.FindAll(e => e != selectedElement && e is KeyRebindDesign).ForEach(e => (e as KeyRebindDesign).UnSelect());
                }
                else
                {
                    elements.FindAll(e => e != selectedElement && e is KeyRebindDesign).ForEach(e => (e as KeyRebindDesign).UnSelect());
                }


                if (selectedElement != null)
                {
                    pageScroll = selectedElement.elementLoc.Y - 170;
                }

                if (pageScroll > maxScroll)
                {
                    pageScroll = maxScroll;
                }

                if (pageScroll < minScroll)
                {
                    pageScroll = 0;
                }
            }

            internal override void SelectPrevious()
            {
                if (KeyRebindDesign.bRebindNow)
                {
                    return;
                }


                base.SelectPrevious();

                if (selectedElement is KeyRebindDesign)
                {
                    (selectedElement as KeyRebindDesign).Select();
                    elements.FindAll(e => e != selectedElement && e is KeyRebindDesign).ForEach(e => (e as KeyRebindDesign).UnSelect());
                }
                else
                {
                    elements.FindAll(e => e != selectedElement && e is KeyRebindDesign).ForEach(e => (e as KeyRebindDesign).UnSelect());
                }

                if (selectedElement != null)
                {
                    pageScroll = selectedElement.elementLoc.Y - 170;
                }

                if (pageScroll > maxScroll)
                {
                    pageScroll = maxScroll;
                }

                if (pageScroll < minScroll)
                {
                    pageScroll = 0;
                }
            }

            internal override void HandleConfirmAndClick()
            {
                if (KeyRebindDesign.bRebindNow && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    (selectedElement as KeyRebindDesign).ConfirmRebind();
                    return;
                }
                else if (KeyRebindDesign.bRebindNow)
                {
                    return;
                }

                if (selectedElement == null)
                {
                    base.HandleConfirmAndClick();
                    return;
                }
                else if (selectedElement is KeyRebindDesign)
                {
                    (selectedElement as KeyRebindDesign).BeginRebind();
                }
            }

            internal override void HandleMouseMove()
            {
                if (KeyRebindDesign.bRebindNow)
                {
                    return;
                }

                int i = elements.IndexOf(elements.Find(e => e.elementLoc.Contains(KeyboardMouseUtility.uiMousePos - getDrawPos().Location + new Point(0, pageScroll))));

                if (i != selectIndex)
                {
                    selectIndex = i;
                    if (selectIndex == -1)
                    {
                        selectedElement = null;
                    }
                    else if (selectIndex != -1)
                    {
                        selectedElement = elements[selectIndex];
                    }

                    if (selectedElement is KeyRebindDesign)
                    {
                        (selectedElement as KeyRebindDesign).Select();
                        elements.FindAll(e => e != selectedElement && e is KeyRebindDesign).ForEach(e => (e as KeyRebindDesign).UnSelect());
                    }
                    else
                    {
                        elements.FindAll(e => e != selectedElement && e is KeyRebindDesign).ForEach(e => (e as KeyRebindDesign).UnSelect());
                    }
                }
                else if (selectedElement is KeyRebindDesign)
                {
                    (selectedElement as KeyRebindDesign).HandleMouse(KeyboardMouseUtility.uiMousePos - getDrawPos().Location + new Point(0, pageScroll));
                }



            }

            internal override void Update(GameTime gt)
            {
                base.Update(gt);


                if (KeyRebindDesign.bRebindNow)
                {

                    KeyboardState kbs = Keyboard.GetState();
                    MouseState bbs = Mouse.GetState();

                    if (kbs.GetPressedKeys().Length > 0)
                    {
                        KeyRebindDesign.newKey = kbs.GetPressedKeys()[0];
                    }

                    return;
                }

                if (KeyboardMouseUtility.ScrollingDown())
                {
                    pageScroll += 30;

                    if (pageScroll > maxScroll)
                    {
                        pageScroll = maxScroll;
                    }

                    if (pageScroll < minScroll)
                    {
                        pageScroll = 0;
                    }
                }
                if (KeyboardMouseUtility.ScrollingUp())
                {
                    pageScroll -= 30;
                    if (pageScroll < minScroll)
                    {
                        pageScroll = 0;
                    }
                }



                m = Matrix.CreateTranslation(0, -pageScroll, 1);
            }

            internal void HandleRight()
            {
                if (KeyRebindDesign.bRebindNow)
                {
                    return;
                }

                if (selectedElement is KeyRebindDesign)
                {
                    (selectedElement as KeyRebindDesign).SelectNext();
                }
            }

            internal void HandleLeft()
            {
                if (KeyRebindDesign.bRebindNow)
                {
                    return;
                }

                if (selectedElement is KeyRebindDesign)
                {
                    (selectedElement as KeyRebindDesign).SelectPrevious();
                }
            }

            internal override void HandleCancel()
            {
                if (KeyRebindDesign.bRebindNow && Mouse.GetState().RightButton == ButtonState.Pressed)
                {
                    (selectedElement as KeyRebindDesign).CancelRebind();
                    return;
                }
                else if (KeyRebindDesign.bRebindNow)
                {
                    return;
                }

                base.HandleCancel();
            }
        }
    }


}
