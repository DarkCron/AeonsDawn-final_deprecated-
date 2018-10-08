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

namespace TBAGW
{
    public abstract class BaseUIElement
    {
        public String name { get; set; } = "UIE";
        public String description { get; set; } = "description";

        public Point initialPosition { get; set; } = new Point(0);//relative on UISCreen
        public Point initialSize { get; set; } = new Point(0);//relative on UISCreen

        public Point position { get; set; } = new Point(0);//relative on UISCreen
        public Point size { get; set; } = new Point(0);//relative on UISCreen

        static Point uiMousePos { get; set; }
        public static Point UIMousePos
        {
            get { return uiMousePos; }
            set { if (uiMousePos == null) { uiMousePos = new Point(); }; uiMousePos = value; }
        }

        RenderTarget2D uiElementRender { get; set; }
        public RenderTarget2D UIElementRender
        {
            get
            {
                if (uiElementRender == null || uiElementRender.IsDisposed)
                {
                    uiElementRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, size.X, size.Y);
                }
                return uiElementRender;
            }
        }

        BaseUIElement parent { get; set; }
        UICollection collectionParent { get; set; }
        public UICollection UICollectionParent
        {
            get { return collectionParent; }
        }

        public enum ActivateType { Click, Hold, ClickHold }
        public ActivateType activateType { get; set; } = ActivateType.Click;

        bool bHoldActivated = false;

        bool bHoldFunctionActivated = false;
        internal int holdEventActivateTimer { get; set; } = 500; //ms
        internal int holdEventTick { get; set; } = 200;

        int holdEventActivateTimePassed = 0; //ms
        int holdEventTickTimePassed = 0;

        int id { get; set; }
        public int ElementID
        {
            get { return id; }
            set { id = value; }
        }

        String luaScriptDataInitialize { get; set; }
        internal String LUAScriptDataInitialize
        {
            get { if (luaScriptDataInitialize == null) { luaScriptDataInitialize = ""; } return luaScriptDataInitialize; }
            set { luaScriptDataInitialize = value; }
        }

        String luaDataProvisionLocation = "";
        public NLua.Lua luaDataProvision;

        Texture2D baseTexture = null;

        public LUADataElement lde = new LUADataElement();

        internal virtual BaseUIElement Clone(BaseUIElement bue, BaseUIElement parent, UICollection parentCollection)
        {
            bue.uiElementRender = new RenderTarget2D(bue.UIElementRender.GraphicsDevice,bue.UIElementRender.Width,bue.UIElementRender.Height);
            bue.parent = parent;
            bue.collectionParent = parentCollection;

            return bue;
        }

        public virtual UIElementLayout GenerateSaveInfo(UIElementLayout UIEL)
        {
            UIEL.activateType = activateType;
            UIEL.description = description;
            UIEL.name = name;
            UIEL.Size = size;
            UIEL.holdEventActivateTimer = holdEventActivateTimer;
            UIEL.holdEventTick = holdEventTick;
            UIEL.ID = ElementID;
            UIEL.Position = position;

            return UIEL;
        }

        public virtual void renderInitializeCheck()
        {
            if (uiElementRender == null || uiElementRender.IsDisposed)
            {
                uiElementRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, size.X, size.Y);
            }
        }

        public virtual void HoldClickReset()
        {
            holdEventActivateTimePassed = 0;
            holdEventTickTimePassed = 0;
            bHoldFunctionActivated = false;
            bHoldActivated = false;
        }

        public BaseUIElement() { }

        public virtual void Reload(UICollection uic, UIElementLayout uiel)
        {
            collectionParent = uic;
            name = uiel.name;
            description = uiel.description;
            id = uiel.ID;
            holdEventActivateTimer = uiel.holdEventActivateTimer;
            holdEventTick = uiel.holdEventTick;
            activateType = uiel.activateType;

            initialPosition = uiel.Position;
            initialSize = uiel.Size;

            position = initialPosition;
            size = initialSize;

            try
            {
                baseTexture = Game1.contentManager.Load<Texture2D>(uiel.baseTexLoc);
            }
            catch (Exception)
            {
                baseTexture = Game1.hitboxHelp;
                uiel.baseTexLoc = baseTexture.Name.Replace(Game1.rootContent, "");
            }

            if (uiElementRender == null || uiElementRender.IsDisposed)
            {
                uiElementRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, size.X, size.Y);
            }
        }

        public virtual void Execute() { }

        public virtual void ClickedOn()
        {
            switch (activateType)
            {
                case ActivateType.Click:
                    break;
                case ActivateType.Hold:
                    if (!bHoldActivated) { HoldClickReset(); }
                    bHoldActivated = true;
                    break;
                case ActivateType.ClickHold:
                    if (!bHoldActivated) { HoldClickReset(); }
                    bHoldActivated = true;
                    break;
                default:
                    break;
            }

            Click();
        }

        public virtual void Click()
        {
        }

        public virtual void OnMouseOver()
        {

        }

        public virtual void OnMouseClickElement()
        {

        }

        public virtual void Update(GameTime gt)
        {
            if (bHoldActivated)
            {
                if (!(KeyboardMouseUtility.bMouseButtonPressed && KeyboardMouseUtility.HoldingLeftClick()))
                {
                    bHoldActivated = false;
                }
            }

            if (bHoldActivated)
            {
                if (!bHoldFunctionActivated)
                {
                    holdEventActivateTimePassed += gt.ElapsedGameTime.Milliseconds;

                    if (holdEventActivateTimePassed >= holdEventActivateTimer)
                    {
                        bHoldFunctionActivated = true;
                    }
                }

                if (bHoldFunctionActivated)
                {
                    holdEventTickTimePassed += gt.ElapsedGameTime.Milliseconds;
                    if (holdEventTickTimePassed >= holdEventActivateTimer)
                    {
                        holdEventTickTimePassed = 0;
                        Click();
                    }
                }
            }


        }

        public virtual void Draw(SpriteBatch sb)
        {
        }

        /// <summary>
        /// Also doubles as reset function
        /// </summary>
        public virtual void Dispose()
        {
            UIElementRender.Dispose();
            position = initialPosition;
            size = initialSize;
            holdEventActivateTimePassed = 0;
            holdEventTickTimePassed = 0;
            bHoldActivated = false;
            bHoldFunctionActivated = false;
        }
    }

    [XmlRoot("UI Element Layout")]
    [XmlInclude(typeof(UITextElementLayout))]
    [XmlInclude(typeof(UIGridSaveLayout))]
    [XmlInclude(typeof(UIScreenSaveLayout))]
    [XmlInclude(typeof(UIGridTabItemSaveLayout))]
    [XmlInclude(typeof(UIButtonSaveLayout))]
    public class UIElementLayout
    {
        [XmlElement("Base Texture Location")]
        public String baseTexLoc = "";
        [XmlElement("Name")]
        public String name = "";
        [XmlElement("Description")]
        public String description = "";
        [XmlElement("ID")]
        public int ID = -1;
        [XmlElement("Timer click to Hold")]
        public int holdEventActivateTimer = 500;
        [XmlElement("Timer Hold tick")]
        public int holdEventTick = 200;
        [XmlElement("Activate type")]
        public BaseUIElement.ActivateType activateType = BaseUIElement.ActivateType.Click;
        [XmlElement("Element Size")]
        public Point Size = new Point(50);
        [XmlElement("Element Position")]
        public Point Position = new Point(0);

        public UIElementLayout() { }
    }
}

namespace LUA
{
    public abstract class LuaUIElementInfo
    {
        public String name = "";
        public int holdEventActivateTimer = 500;
        public int holdEventTick = 200;
        public LuaPoint position = new LuaPoint(0);
        public LuaPoint size = new LuaPoint(50);
        public String activateType = "Click";

        internal BaseUIElement.ActivateType ActivateType = BaseUIElement.ActivateType.Click;
        internal void GetActivateType()
        {
            int index = Enum.GetNames(typeof(BaseUIElement.ActivateType)).ToList().IndexOf(activateType);
            ActivateType = index == -1 ? BaseUIElement.ActivateType.Click : (BaseUIElement.ActivateType)index;
        }

        internal virtual BaseUIElement convertFromLuaData(BaseUIElement bue)
        {
            bue.name = name;
            bue.holdEventActivateTimer = holdEventActivateTimer;
            bue.holdEventTick = holdEventTick;
            bue.position = position.MGConvert();
            bue.initialPosition = position.MGConvert();
            bue.size = size.MGConvert();
            bue.initialSize = size.MGConvert();
            GetActivateType();
            bue.activateType = ActivateType;
            return bue;
        }
    }

}



