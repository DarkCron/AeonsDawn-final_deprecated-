using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW;

namespace TBAGW
{
    public class UIScreen : BaseUIElement
    {
        public enum ScreenState { Uninitialized, Open, Closed }

        public ScreenState currentState = ScreenState.Uninitialized;

        public UIScreen() : base() { }

        internal override BaseUIElement Clone(BaseUIElement bue, BaseUIElement parent, UICollection parentCollection)
        {
            UIScreen temp = (UIScreen)this.MemberwiseClone();
            return base.Clone(temp, parent, parentCollection);
        }

        public override UIElementLayout GenerateSaveInfo(UIElementLayout UIEL)
        {
            UIScreenSaveLayout UISEL = new UIScreenSaveLayout();
            return base.GenerateSaveInfo(UISEL);
        }

        public virtual void Close()
        {
            Dispose();
            currentState = ScreenState.Closed;
        }

        public virtual void Open()
        {
            switch (currentState)
            {
                case ScreenState.Uninitialized:
                    currentState = ScreenState.Open;
                    //UIElementRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, size.X, size.Y);
                    renderInitializeCheck();
                    Console.WriteLine(this + " succesfully Opened for the 1st time this runtime.");
                    break;
                case ScreenState.Open:
                    break;
                case ScreenState.Closed:
                    currentState = ScreenState.Open;
                    renderInitializeCheck();
                    //screenRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, size.X, size.Y);
                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(UIElementRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            sb.Draw(Game1.hitboxHelp, new Rectangle(Point.Zero, size), Color.White);
            sb.End();
        }
    }

    [XmlRoot("Save layout for UI Screen")]
    public class UIScreenSaveLayout : UIElementLayout
    {
        public UIScreenSaveLayout() { }
    }
}

namespace LUA
{
    public class LuaUIScreen : LuaUIElementInfo
    {

        public LuaUIScreen() { }

        internal override BaseUIElement convertFromLuaData(BaseUIElement bue)
        {
            UIScreen uis;
            if (bue == null)
            {
                uis = new UIScreen();
            }
            else
            {
                try
                {
                    uis = bue as UIScreen;
                }
                catch (Exception)
                {
                    uis = new UIScreen();
                }
            }

            return base.convertFromLuaData(uis);
        }
    }

}



