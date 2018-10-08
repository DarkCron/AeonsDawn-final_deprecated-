using LUA;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW;
using TBAGW.Utilities;

namespace TBAGW.Utilities
{
    public class UIGrid : BaseUIElement
    {
        public UIGrid() : base() { }

        public LuaDataCollection ldc = new LuaDataCollection();

        public enum GridType { Vertical, Matrix }
        public GridType currentGridType = GridType.Vertical;

        internal UIGridTabItem itemLayout = new UIGridTabItem();

        internal List<UIGridTabItem> gridItemsList { get { return gridItems; } }
        List<UIGridTabItem> gridItems = new List<UIGridTabItem>();

        Matrix gridMatrix = Matrix.CreateTranslation(new Vector3(0, 0, 1));
        RenderTarget2D gridCompleteRender { get; set; }
        public RenderTarget2D GridCompleteRender
        {
            get
            {
                if (gridCompleteRender == null || gridCompleteRender.IsDisposed)
                {
                    gridCompleteRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, UIElementRender.Width, UIElementRender.Height);
                }
                return gridCompleteRender;
            }
        }

        Point gridMatrixOffSet = new Point(0);

        internal Point elementSize = new Point(100);

        List<Object> tabItemData = new List<object>();

        internal override BaseUIElement Clone(BaseUIElement bue, BaseUIElement parent, UICollection parentCollection)
        {
            UIGrid temp = (UIGrid)this.MemberwiseClone();
            temp.ldc = ldc.Clone();
            temp.gridItems = new List<UIGridTabItem>(temp.gridItems);
            temp.gridCompleteRender = new RenderTarget2D(gridCompleteRender.GraphicsDevice, gridCompleteRender.Width, gridCompleteRender.Height);

            return base.Clone(temp, parent, parentCollection);
        }

        public virtual void RecalculateTabItemSizes()
        {
            switch (currentGridType)
            {
                case GridType.Vertical:
                    elementSize.X = size.X;
                    break;
                case GridType.Matrix:
                    break;
                default:
                    break;
            }
        }

        public void ReloadDataFromLuaString(String luaCommands)
        {
            NLua.Lua lua = new NLua.Lua();
        }

        public void ReloadDataFromLuaStringFile(String luaLoc)
        {
            NLua.Lua lua = new NLua.Lua();
            lua.DoFile(luaLoc);

        }

        public override void Reload(UICollection uic, UIElementLayout uiel)
        {
            base.Reload(uic, uiel);
            var temp = uiel as UIGridSaveLayout;
            //foreach (var item in temp.listTabItemLayouts)
            //{
            //    gridItems.Add(new UIGridTabItem());
            //    gridItems.Last().Reload(uic, item, this);
            //}

            if (gridCompleteRender == null || gridCompleteRender.IsDisposed)
            {
                gridCompleteRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, size.X, size.Y);
            }
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            gridMatrix = Matrix.CreateTranslation(new Vector3(gridMatrixOffSet.X, gridMatrixOffSet.Y, 0));
            gridItems.ForEach(item => item.Update(gt));
        }

        public override void Draw(SpriteBatch sb)
        {
            // gridMatrix = Matrix.CreateTranslation(new Vector3(0, 0, 1));
            if (GridCompleteRender == UIElementRender)
            { }
            gridItems.ForEach(i => i.Draw(sb));

            // gridCompleteRender = new RenderTarget2D(Game1.graphics.GraphicsDevice,500,500);
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(GridCompleteRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, gridMatrix);
            foreach (var item in gridItems)
            {
                var r = new Rectangle(Point.Zero + new Point(0, (item.size.Y + 20) * item.gridIndex), item.size);

                // sb.Draw(item.UIElementRender, new Rectangle(Point.Zero + new Point(0, (item.size.Y + 20) * item.gridIndex), item.size), Color.White);

                sb.Draw(item.TabItemContents.UICollectionRender, new Rectangle(item.position + new Point(0, (item.size.Y + 20) * item.gridIndex), item.size), Color.White);

            }
            sb.End();

            sb.GraphicsDevice.SetRenderTarget(UIElementRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            sb.Draw(GridCompleteRender, new Rectangle(Point.Zero, size), Color.White);
            sb.End();
        }

        public override void Dispose()
        {
            base.Dispose();
            gridItems.ForEach(item => item.Dispose());
        }
    }

    [XmlRoot("UI Grid Save Layout")]
    public class UIGridSaveLayout : UIElementLayout
    {
        //[XmlElement("Grid Items layout")]
        // public List<UIGridTabItemSaveLayout> listTabItemLayouts = new List<UIGridTabItemSaveLayout>();


        public UIGridSaveLayout() { }
    }
}

namespace LUA
{
    public class LuaUIGrid : LuaUIElementInfo
    {
        public LuaUIGridTabItem tabLayout = new LuaUIGridTabItem();

        public LuaDataCollection ldc = new LuaDataCollection();

        public LuaUIGrid() : base() { }

        internal override BaseUIElement convertFromLuaData(BaseUIElement bue)
        {
            UIGrid uib;
            if (bue == null)
            {
                uib = new UIGrid();
            }
            else
            {
                try
                {
                    uib = bue as UIGrid;
                }
                catch (Exception)
                {
                    uib = new UIGrid();
                }
            }

            UIGridTabItem layout = tabLayout.convertFromLuaData();

            if (layout != null)
            {
                HandleGridItems(layout, uib);
            }
            else
            {
                throw new Exception("UIGridTabItem layout is null--");
            }


            return base.convertFromLuaData(uib);
        }

        private void HandleGridItems(UIGridTabItem layout, UIGrid uib)
        {
            uib.itemLayout = layout;
            ldc.ConvertCheck();

            foreach (var item in ldc.data)
            {
                UIGridTabItem temp = new UIGridTabItem();
                temp.AssignFromLayout(uib, layout, ldc.data.IndexOf(item));
                uib.gridItemsList.Add(temp);
            }
        }
    }

}

