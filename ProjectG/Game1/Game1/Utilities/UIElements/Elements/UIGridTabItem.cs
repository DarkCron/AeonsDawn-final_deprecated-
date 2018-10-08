using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TBAGW;
using TBAGW.Utilities;
using LUA;

namespace TBAGW.Utilities
{
    public class UIGridTabItem : BaseUIElement
    {
        public UIGridTabItem() : base() { }

        UIGrid parent;
        internal UICollection TabItemContents = new UICollection();
        internal int gridIndex = 0;

        public LUADataElement dataElement = null;

        internal override BaseUIElement Clone(BaseUIElement bue, BaseUIElement parent, UICollection parentCollection)
        {
            UIGridTabItem temp = (UIGridTabItem)this.MemberwiseClone();
            temp.TabItemContents = TabItemContents.Clone();

            return base.Clone(temp, parent, parentCollection);
        }

        internal void AssignFromLayout(UIGrid p, UIGridTabItem layout, int index)
        {
            parent = p;
            TabItemContents = layout.TabItemContents.Clone();

            position = TabItemContents.startMainElement.position;
            initialPosition = TabItemContents.startMainElement.initialPosition;

            size = TabItemContents.startMainElement.size;
            initialSize = TabItemContents.startMainElement.initialSize;

            gridIndex = index;
        }

        public void Reload(UICollection uic, UIElementLayout uiel, UIGrid parent)
        {
            base.Reload(uic, uiel);
            this.parent = parent;
        }

        public override void Draw(SpriteBatch sb)
        {
            //sb.End();
            //sb.GraphicsDevice.SetRenderTarget(UIElementRender);
            //sb.GraphicsDevice.Clear(Color.TransparentBlack);
            //sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp,null,null,null,null);
            //sb.Draw(Game1.hitboxHelp, new Rectangle(Point.Zero, size), Color.White);
            //sb.End();

            TabItemContents.Draw(sb);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            var temp = BaseUIElement.UIMousePos;
            TabItemContents.Update(gt, BaseUIElement.UIMousePos -new  Point(0, (size.Y + 20) * gridIndex));
            BaseUIElement.UIMousePos = temp;
        }
    }

    [XmlRoot("Tab Item Layout")]
    public class UIGridTabItemSaveLayout : UIElementLayout
    {
        public UIGridTabItemSaveLayout() : base() { }
    }
}

namespace LUA
{
    public class LuaUIGridTabItem
    {
        public LuaUICollection elements = new LuaUICollection();

        public LuaUIGridTabItem() { }

        internal UIGridTabItem convertFromLuaData()
        {
            UICollection uic = null;
            try
            {
                uic = elements.ConvertFromLua(LuaUICollection.luaStateConversion);
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong converting UIGrid from LUA layout!");
            }

            UIGridTabItem tabItem = null;

            if (uic != null)
            {
                tabItem = new UIGridTabItem();
                tabItem.TabItemContents = uic;

            }
            else
            {
                tabItem = null;
            }

            return tabItem;
        }
    }

}
