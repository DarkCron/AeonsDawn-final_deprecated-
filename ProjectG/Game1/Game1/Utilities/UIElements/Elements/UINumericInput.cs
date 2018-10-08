using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using TBAGW;

namespace TBAGW
{
    public class UINumericInput : UITextElement
    {
        float input = 0;
        public float Input { get { return input; } }
        public float defaultInput = 0.0f;

        public UINumericInput():base() { }

        internal override BaseUIElement Clone(BaseUIElement bue, BaseUIElement parent, UICollection parentCollection)
        {
            UINumericInput temp = (UINumericInput)this.MemberwiseClone();

            return base.Clone(temp, parent, parentCollection);
        }

        public override void Reload(UICollection uic, UIElementLayout uiel)
        {
            base.Reload(uic, uiel);
            var temp = uiel as UINumericInputLayout;
            defaultInput = temp.defaultInput;
        }

        public override UIElementLayout GenerateSaveInfo(UIElementLayout UIEL)
        {
            UINumericInputLayout UINEL = new UINumericInputLayout();
            UINEL.defaultInput = defaultInput;
            return base.GenerateSaveInfo(UIEL);
        }

        public void AssignNum(float f)
        {
            input = f;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(UIElementRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            TextUtility.Draw(sb, input.ToString(), sf, new Rectangle(Point.Zero, size), TextUtility.OutLining.Center, textC, 1.0f, false);
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
        }
    }

    [XmlRoot("UI Numeric element Layout")]
    public class UINumericInputLayout : UITextElementLayout
    {
        [XmlElement("Default input")]
        public float defaultInput = 0.0f;

        public UINumericInputLayout() : base() { }
    }
}
namespace LUA
{
    public class LuaUINumericInputLayout : LuaUITextElement
    {
        public float defaultInput = 0.0f;

        public LuaUINumericInputLayout() : base() { }

        internal override BaseUIElement convertFromLuaData(BaseUIElement bue)
        {
            UINumericInput uini;
            if (bue == null)
            {
                uini = new UINumericInput();
            }
            else
            {
                try
                {
                    uini = bue as UINumericInput;
                }
                catch (Exception)
                {
                    uini = new UINumericInput();
                }
            }

            try
            {
                Reload();
            }
            catch (Exception)
            {
            }

            uini.defaultInput = defaultInput;
            uini.AssignNum(defaultInput);

            return base.convertFromLuaData(uini);
        }
    }
}
