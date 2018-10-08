using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using TBAGW;

namespace TBAGW
{
    public class UITextElement : BaseUIElement
    {
        String text { get; set; } = "";
        public String Text
        {
            get { return text; }
            set { text = value != null ? value : ""; }
        }
        internal SpriteFont sf { get; set; } = Game1.defaultFont;
        internal Color textC = Color.Black;

        internal override BaseUIElement Clone(BaseUIElement bue, BaseUIElement parent, UICollection parentCollection)
        {
            UITextElement temp = (UITextElement)this.MemberwiseClone();
            return base.Clone(temp, parent, parentCollection);
        }

        public override UIElementLayout GenerateSaveInfo(UIElementLayout UIEL)
        {
            UITextElementLayout UITEL = new UITextElementLayout();
            UITEL.text = Text;

            return base.GenerateSaveInfo(UITEL);
        }

        public override void Reload(UICollection uic, UIElementLayout uiel)
        {
            base.Reload(uic, uiel);
            var temp = uiel as UITextElementLayout;
            try
            {
                sf = Game1.contentManager.Load<SpriteFont>(temp.fontLocation);
            }
            catch (Exception)
            {
                sf = Game1.defaultFont;
            }
            Text = temp.text;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(UIElementRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            TextUtility.Draw(sb, Text, sf, new Rectangle(Point.Zero, size), TextUtility.OutLining.Center, textC, 1.0f, false);
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
        }
    }

    [XmlRoot("UI Text Element Layout")]
    [XmlInclude(typeof(UINumericInputLayout))]
    public class UITextElementLayout : UIElementLayout
    {
        [XmlElement("Font location")]
        public String fontLocation = "";
        [XmlElement("Text")]
        public String text = "";

        public UITextElementLayout() : base() { }
    }
}

namespace LUA
{

    public class LuaUITextElement : LuaUIElementInfo
    {
        public String font = "";
        public String text = "Text";
        public String color = "";

        internal Color c = Color.Black;
        internal SpriteFont sf = TBAGW.Game1.defaultFont;
        internal void getFont()
        {
            try
            {
                sf = TBAGW.Game1.contentManager.Load<SpriteFont>(font);
            }
            catch (Exception)
            {
                sf = TBAGW.Game1.defaultFont;
            }
        }
        internal void getColor()
        {
            if (!color.StartsWith("R:", StringComparison.OrdinalIgnoreCase))
            {
                Color tempColor = Color.Black;
                var prop = typeof(Color).GetProperty(color);
                if (prop != null)
                {
                    tempColor = (Color)prop.GetValue(null, null);
                }

                if (tempColor != null && tempColor != default(Color))
                {
                    c = tempColor;
                }
            }
            else
            {
                try
                {
                    String temp = color.Replace("R:", "");
                    String RVal = temp.Substring(temp.IndexOf("G:", StringComparison.OrdinalIgnoreCase));
                    temp = color.Replace("R:" + RVal + "G:", "");
                    String GVal = temp.Substring(temp.IndexOf("B:", StringComparison.OrdinalIgnoreCase));
                    temp = color.Replace("R:" + RVal + "G:" + GVal + "B:", "");
                    String BVal = temp;

                    int R = int.Parse(RVal);
                    int G = int.Parse(GVal);
                    int B = int.Parse(BVal);

                    while (R < 0)
                    { R += 255; }
                    while (R > 255)
                    { R -= 255; }

                    while (G < 0)
                    { G += 255; }
                    while (G > 255)
                    { G -= 255; }

                    while (B < 0)
                    { B += 255; }
                    while (B > 255)
                    { B -= 255; }

                    c = new Color(R, G, B);
                }
                catch (Exception)
                {
                    c = Color.White;
                }



            }



        }

        internal void Reload()
        {
            getFont();
            getColor();

            if (c == null) { c = Color.Gold; }
            if (sf == null) { sf = TBAGW.Game1.defaultFont; }
        }

        internal override BaseUIElement convertFromLuaData(BaseUIElement bue)
        {
            UITextElement utl;

            if (bue == null)
            {
                utl = new UITextElement();
            }
            else
            {
                try
                {
                    utl = bue as UITextElement;
                }
                catch (Exception)
                {
                    utl = new UITextElement();
                }
            }

            Reload();

            return utl;
        }
    }
}
