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
    public class UICollection
    {
        public UIScreen startMainElement = null;

        public int ElementIDCount = 0;
        public int CollectionID = 0;
        public int mainElementID = 0;

        RenderTarget2D uiCollectionRender { get; set; }
        public RenderTarget2D UICollectionRender
        {
            get
            {
                if (uiCollectionRender == null || uiCollectionRender.IsDisposed)
                {
                    uiCollectionRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
                }
                return uiCollectionRender;
            }
        }

        public BaseUIElement getElementByName(String name)
        {
            BaseUIElement temp = elementsInCollection.Find(ele=>ele.name.Equals(name,StringComparison.OrdinalIgnoreCase));
            return temp;
        }

        public List<BaseUIElement> elementsInCollection;

        public void Close()
        {
            startMainElement.Close();
        }

        public void AddElement(BaseUIElement be)
        {
            if (elementsInCollection == null)
            {
                elementsInCollection = new List<BaseUIElement>();
                if (be.GetType() != typeof(UIScreen))
                {
                    elementsInCollection.Add(new UIScreen());
                    elementsInCollection.Last().ElementID = ElementIDCount++;
                    elementsInCollection.Last().size = new Point(500, 300);
                    elementsInCollection.Last().initialSize = new Point(500, 300);
                    if (startMainElement == null)
                    {
                        startMainElement = elementsInCollection.Last() as UIScreen;
                    }

                }
            }

            if (elementsInCollection.Count == 0 && be.GetType() != typeof(UIScreen))
            {
                elementsInCollection.Add(new UIScreen());
                elementsInCollection.Last().ElementID = ElementIDCount++;
                if (startMainElement == null)
                {
                    startMainElement = elementsInCollection.Last() as UIScreen;
                }
            }

            elementsInCollection.Add(be);
            elementsInCollection.Last().ElementID = ElementIDCount++;

            if (startMainElement == null && elementsInCollection.Count == 1 && elementsInCollection.Last().GetType() == typeof(UIScreen))
            {
                startMainElement = elementsInCollection.Last() as UIScreen;
            }
        }

        public void Update(GameTime gt, Point mouse)
        {
            BaseUIElement.UIMousePos = mouse - startMainElement.position;
            elementsInCollection.ForEach(ele => ele.Update(gt));
        }

        public Point trueOffSet() { return startMainElement.position; }

        public RenderTarget2D Draw(SpriteBatch sb)
        {


            if (!elementsInCollection.Contains(startMainElement))
            {
                startMainElement = elementsInCollection.Find(ele => ele.GetType() == typeof(UIScreen)) as UIScreen;
            }

            if (uiCollectionRender != null && uiCollectionRender.Bounds != startMainElement.UIElementRender.Bounds)
            {
                uiCollectionRender.Dispose();
                uiCollectionRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, startMainElement.UIElementRender.Width, startMainElement.UIElementRender.Height);
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            elementsInCollection.ForEach(element => element.Draw(sb));

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(uiCollectionRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);
            foreach (var item in elementsInCollection)
            {
                if (item == startMainElement)
                {
                    sb.Draw(item.UIElementRender,Vector2.Zero, Color.White);
                }
                else
                {
                    sb.Draw(item.UIElementRender, Vector2.Zero + item.position.ToVector2(), Color.White);
                }

                if (item.GetType() == typeof(Utilities.UIGrid))
                {
                    //sb.Draw(item.UIElementRender, startMainElement.position.ToVector2() + item.position.ToVector2(), Color.White);
                }

            }
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
            return uiCollectionRender;
        }

        internal UICollection Clone()
        {
            UICollection temp = new UICollection();
            temp = (UICollection)this.MemberwiseClone();
            temp.elementsInCollection = new List<BaseUIElement>();
            temp.uiCollectionRender = new RenderTarget2D(temp.UICollectionRender.GraphicsDevice, temp.UICollectionRender.Width, temp.UICollectionRender.Height);
            temp.elementsInCollection.Clear();
            temp.startMainElement = this.startMainElement.Clone(null, null, temp) as UIScreen;
            temp.elementsInCollection.Add(temp.startMainElement);

            foreach (var item in this.elementsInCollection)
            {
                if (item.ElementID != temp.startMainElement.ElementID)
                {
                    temp.elementsInCollection.Add(item.Clone(null, temp.startMainElement, temp));
                }
            }

            return temp;
        }
    }

    [XmlRoot("UI Collection Save")]
    public class UICollectionSave
    {
        [XmlElement("UI save elements")]
        public List<UIElementLayout> listUIElementsSaves = new List<UIElementLayout>();
        [XmlElement("Initial Position")]
        public Point initialPosition = new Point(0);
        [XmlElement("Start element ID")]
        public int startElementID = 0;

        [XmlIgnore]
        public UICollection parent = new UICollection();

        public UICollectionSave()
        {

        }

        public void GenerateSave()
        {
            listUIElementsSaves.Clear();
            startElementID = parent.startMainElement.ElementID;
            initialPosition = parent.startMainElement.initialPosition;
            listUIElementsSaves.Add(parent.startMainElement.GenerateSaveInfo(null));
            foreach (var item in parent.elementsInCollection.FindAll(ele => ele != parent.startMainElement))
            {
                listUIElementsSaves.Add(item.GenerateSaveInfo(null));
            }
        }

        public void Reload()
        {
            parent = new UICollection();
            parent.elementsInCollection = new List<BaseUIElement>();
            foreach (var item in listUIElementsSaves)
            {
                if (item.GetType() == typeof(UIScreenSaveLayout))
                {
                    UIScreen uis = new UIScreen();
                    uis.Reload(parent, item);
                    parent.elementsInCollection.Add(uis);
                }
                else if (item.GetType() == typeof(UIScreenSliderSaveLayout))
                {
                    UIScreenSlider uisl = new UIScreenSlider();
                    uisl.Reload(parent, item);
                    parent.elementsInCollection.Add(uisl);
                }
                else if (item.GetType() == typeof(UINumericInputLayout))
                {
                    UINumericInput uinum = new UINumericInput();
                    uinum.Reload(parent, item);
                    parent.elementsInCollection.Add(uinum);
                }
                else if (item.GetType() == typeof(UIButtonSaveLayout))
                {
                    GButton uib = new GButton();
                    uib.Reload(parent, item);
                    parent.elementsInCollection.Add(uib);
                }
                else if (item.GetType() == typeof(UITextElementLayout))
                {
                    UITextElement uite = new UITextElement();
                    uite.Reload(parent, item);
                    parent.elementsInCollection.Add(uite);
                }


            }

            parent.startMainElement = parent.elementsInCollection.FindAll(e => e.GetType() == typeof(UIScreen)).Cast<UIScreen>().ToList().Find(uis => uis.ElementID == startElementID);
        }
    }
}

namespace LUA
{
    public class LuaUICollection
    {
        internal static NLua.Lua luaStateConversion = new NLua.Lua();

        public List<LuaUIElementInfo> listLuaElements = new List<LuaUIElementInfo>();
        public LuaUIScreen mainScreen = new LuaUIScreen();



        public LuaUICollection() { }

        public UICollection ConvertFromLua(NLua.Lua state)
        {
            luaStateConversion = state;

            UICollection UIC = new UICollection();
            UIC.elementsInCollection = new List<BaseUIElement>();
            try
            {
                UIC.startMainElement = mainScreen.convertFromLuaData(null) as UIScreen;
            }
            catch (Exception)
            {
                UIC = null;
                LUAUtilities.errorList.Add("***Error Begin***");
                LUAUtilities.errorList.Add("No (suitable) 'mainScreen' in collection found!");
                LUAUtilities.errorList.Add("***Error End***");
            }

            if (UIC != null)
            {
                foreach (var item in listLuaElements)
                {
                    UIC.AddElement(item.convertFromLuaData(null));
                    //UIC.elementsInCollection.Add(item.convertFromLuaData(null));
                }
            }

            return UIC;
        }
    }
}
