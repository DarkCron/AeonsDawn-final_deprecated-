using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Actions;

namespace TBAGW
{
    public class KeyRebindDesign : SelectableElement
    {
        static bool bInitialize = true;
        static SpriteFont font;
        static TexPanel sourcePanel;

        int totalW;
        int widthPrompts;
        int widthText;

        TexPanel textBGPanel;
        TexPanel prompt1Panel;
        TexPanel prompt2Panel;
        TexPanel prompt3Panel;

        int selectIndex = 0;

        static internal bool bRebindNow = false;
        static internal Microsoft.Xna.Framework.Input.Keys newKey = default(Microsoft.Xna.Framework.Input.Keys);
        static internal int collumn = 1;

        Actions keyAction;

        static void Initialize()
        {
            bInitialize = false;

            Texture2D guiTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
            sourcePanel = new TexPanel(guiTex, new Rectangle(0, 0, 200, 200), new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test25"); //20,25,32,48
        }

        public KeyRebindDesign(Point p, int w, int h, int wOS, Actions a) : base()
        {
            if (bInitialize) { Initialize(); }
            keyAction = a;
            bFocusOnSelect = true;
            position = p;
            totalW = w;
            widthText = (w - 3 * wOS) / 2;
            widthPrompts = (w - 3 * wOS) / 2 / 3;

            textBGPanel = sourcePanel.positionCopy(new Rectangle(p, new Point(widthText, h)));
            prompt1Panel = sourcePanel.positionCopy(new Rectangle(p + new Point(widthText + wOS, 0), new Point(widthPrompts, h)));
            prompt2Panel = sourcePanel.positionCopy(new Rectangle(p + new Point(widthText + wOS * 2 + widthPrompts, 0), new Point(widthPrompts, h)));
            prompt3Panel = sourcePanel.positionCopy(new Rectangle(p + new Point(widthText + wOS * 3 + 2 * widthPrompts, 0), new Point(widthPrompts, h)));

            elementLoc = new Rectangle(p, new Point(w, h));
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public void DrawRebind(SpriteBatch sb, OptionsMenu.OptionsPage op)
        {
            Rectangle titlePos = new Rectangle(0, 30, op.getDrawPos().Width, 60);
            Rectangle textPos = new Rectangle(0, 100, op.getDrawPos().Width, 300);
            Rectangle textUnderPos = new Rectangle(0, 420, op.getDrawPos().Width, 70);
            String title = "Default '" + keyAction.actionIndentifierString + "' button is: " + keyAction.defaultKey;
            String under = "LMB to confirm change, RMB to cancel change.\nPress LMB without a key to unassign a key.";
            String text = "";
            String warning = "";
            if (newKey != (default(Microsoft.Xna.Framework.Input.Keys)))
            {
                text = newKey.ToString();
                var replaceKey = Game1.actionKeyList.Find(ak => ak.assignedActionKey == newKey);
                if (replaceKey != null && Game1.actionList.Find(a => a.actionIndentifierString.Equals(replaceKey.actionIndentifierString)).bUsed)
                {
                    warning += "\n\n\nWarning will replace '" + replaceKey.actionIndentifierString + "' Keybind " + (replaceKey.column + 1);
                }
            }
            if (!text.Equals(""))
            {
                TextUtility.Draw(sb, text, font, textPos, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                if (!warning.Equals(""))
                {
                    TextUtility.Draw(sb, warning, font, textPos, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
                }
            }

            TextUtility.Draw(sb, title, font, titlePos, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
            TextUtility.Draw(sb, under, font, textUnderPos, TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
        }

        public void Draw(SpriteBatch sb, Matrix m)
        {
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, m);

            textBGPanel.Draw(sb, Color.White);

            if (bIsSelected)
            {
                Rectangle selected = new Rectangle();
                int os = 4;
                if (selectIndex == 0)
                {
                    selected = new Rectangle(prompt1Panel.Position().X - os, prompt1Panel.Position().Y - os, prompt1Panel.Position().Width + os * 2, prompt1Panel.Position().Height + os * 2);
                }
                else if (selectIndex == 1)
                {
                    selected = new Rectangle(prompt2Panel.Position().X - os, prompt2Panel.Position().Y - os, prompt2Panel.Position().Width + os * 2, prompt2Panel.Position().Height + os * 2);
                }
                else if (selectIndex == 2)
                {
                    selected = new Rectangle(prompt3Panel.Position().X - os, prompt3Panel.Position().Y - os, prompt3Panel.Position().Width + os * 2, prompt3Panel.Position().Height + os * 2);
                }

                sb.Draw(Game1.WhiteTex, selected, Color.Orange * .75f);
            }

            prompt1Panel.Draw(sb, Color.White);
            prompt2Panel.Draw(sb, Color.White);
            prompt3Panel.Draw(sb, Color.White);

            TextUtility.Draw(sb, keyAction.actionIndentifierString, font, textBGPanel.Position(), TextUtility.OutLining.Center, Color.Gold, 1f, false, m, Color.Silver, false);

            if (keyAction.whatKeysIsActionAssignedTo[0].assignedActionKey != default(Microsoft.Xna.Framework.Input.Keys))
            {
                String keyName = keyAction.whatKeysIsActionAssignedTo[0].assignedActionKey.ToString();
                TextUtility.Draw(sb, keyName, font, prompt1Panel.Position(), TextUtility.OutLining.Center, Color.Gold, 1f, false, m, Color.Silver, false);
            }

            if (keyAction.whatKeysIsActionAssignedTo[1].assignedActionKey != default(Microsoft.Xna.Framework.Input.Keys))
            {
                String keyName = keyAction.whatKeysIsActionAssignedTo[1].assignedActionKey.ToString();
                TextUtility.Draw(sb, keyName, font, prompt2Panel.Position(), TextUtility.OutLining.Center, Color.Gold, 1f, false, m, Color.Silver, false);
            }

            if (keyAction.whatKeysIsActionAssignedTo[2].assignedGamePadButton != default(Microsoft.Xna.Framework.Input.Buttons))
            {
                String keyName = keyAction.whatKeysIsActionAssignedTo[2].assignedGamePadButton.ToString();
                TextUtility.Draw(sb, keyName, font, prompt3Panel.Position(), TextUtility.OutLining.Center, Color.Gold, 1f, false, m, Color.Silver, false);
            }

        }

        public void Select()
        {
            selectIndex = 0;
            bIsSelected = true;
        }

        public void UnSelect()
        {
            selectIndex = 0;
            bIsSelected = false;
        }

        public void SelectNext()
        {
            selectIndex++;
            if (selectIndex > 2)
            {
                selectIndex = 0;
            }
        }

        public void SelectPrevious()
        {
            selectIndex--;
            if (selectIndex < 0)
            {
                selectIndex = 2;
            }
        }

        internal void HandleMouse(Point point)
        {
            if (prompt1Panel.ContainsMouse(point))
            {
                selectIndex = 0;
            }

            if (prompt2Panel.ContainsMouse(point))
            {
                selectIndex = 1;
            }

            if (prompt3Panel.ContainsMouse(point))
            {
                selectIndex = 2;
            }
        }

        internal void BeginRebind()
        {
            if (selectIndex == 0 || selectIndex == 1)
            {
                newKey = default(Microsoft.Xna.Framework.Input.Keys);
                bRebindNow = true;
                collumn = selectIndex;
            }

        }

        internal void CancelRebind()
        {
            newKey = default(Microsoft.Xna.Framework.Input.Keys);
            bRebindNow = false;
        }

        internal void ConfirmRebind()
        {
            var replaceKey = Game1.actionKeyList.Find(ak => ak.assignedActionKey == newKey);
            if (newKey != Microsoft.Xna.Framework.Input.Keys.None && replaceKey != null)
            {
                replaceKey.assignedActionKey = Microsoft.Xna.Framework.Input.Keys.None;
                Actions ac = Game1.actionList.Find(a=>a.actionIndentifierString.Equals(replaceKey.actionIndentifierString));
                ac.whatKeysIsActionAssignedTo[replaceKey.column].assignedActionKey = default(Microsoft.Xna.Framework.Input.Keys);
                Game1.actionKeyList.Find(ak => ak.actionIndentifierString.Equals(replaceKey.actionIndentifierString) && ak.column == replaceKey.column).assignedActionKey = default(Microsoft.Xna.Framework.Input.Keys);
            }
            keyAction.whatKeysIsActionAssignedTo[collumn].assignKey(newKey, keyAction.actionIndentifierString, collumn, false);
            Game1.actionKeyList.Find(ak => ak.actionIndentifierString.Equals(keyAction.actionIndentifierString) && ak.column == collumn).assignedActionKey = newKey;


            newKey = default(Microsoft.Xna.Framework.Input.Keys);
            bRebindNow = false;
        }
    }
}
