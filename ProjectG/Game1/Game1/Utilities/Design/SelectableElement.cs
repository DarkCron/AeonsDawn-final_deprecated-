using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    public class SelectableElement
    {
        internal Point position = new Point(0);
        internal bool bReturnsRender = false;
        internal Rectangle elementLoc = new Rectangle();
        internal bool bIsSelected = false;
        internal bool bFocusOnSelect = false;
        internal bool bRequiresUpDown = false;

        public virtual void Update(GameTime gt) { }

        public virtual void Draw(SpriteBatch sb) { }

        public virtual KeyValuePair<RenderTarget2D, Rectangle> DrawReturnRender(SpriteBatch sb) { return new KeyValuePair<RenderTarget2D, Rectangle>(); }

        public virtual void Close() { }

        public virtual Rectangle SelectionPosition()
        {
            int offset = 3;
            return new Rectangle(elementLoc.X - offset, elementLoc.Y - offset, elementLoc.Width + 2 * offset, elementLoc.Height + 2 * offset);
        }
    }

    public class SelectableButton : SelectableElement
    {
        internal delegate void ButtonFunction();
        ButtonFunction bf;

        static bool bInitialize = true;
        static TexPanel source;
        TexPanel buttonPanel;
        String ButtonText = "";
        SpriteFont font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test25");

        static void Initialize()
        {
            bInitialize = false;
            source = new TexPanel(Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096"), new Rectangle(0, 0, 200, 200), new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

        }

        internal SelectableButton(Rectangle pos, String bs = "") : base()
        {
            if (bInitialize) { Initialize(); }
            ButtonText = bs;
            elementLoc = pos;
            position = pos.Location;
            buttonPanel = source.positionCopy(pos);
            bFocusOnSelect = true;
        }

        internal void SetFunction(ButtonFunction bf)
        {
            this.bf = bf;
        }

        internal void ExecuteFunction()
        {
            if (bf != default(ButtonFunction))
            {
                bf();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            buttonPanel.Draw(sb, Color.White);
            if (!ButtonText.Equals(""))
            {
                TextUtility.Draw(sb, ButtonText, font, buttonPanel.Position(), TextUtility.OutLining.Center, Color.Gold, 1f, false, default(Matrix), Color.Silver, false);
            }

        }
    }
}
