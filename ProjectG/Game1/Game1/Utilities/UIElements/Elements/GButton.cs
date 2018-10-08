using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using TBAGW.Utilities.Sprite;
using LUA;
using TBAGW;

namespace TBAGW
{
    public class GButton : BaseUIElement
    {
        public enum ButtonState { NON_CLICKED, CLICKED, HOVER }

        ButtonState currentState { get; set; } = ButtonState.NON_CLICKED;

        ShapeAnimation ButtonAnim_RestState;
        ShapeAnimation ButtonAnim_HoverState;
        ShapeAnimation ButtonAnim_ClickState;

        public LuaFRef executeFunction = new LuaFRef();

        internal override BaseUIElement Clone(BaseUIElement bue, BaseUIElement parent, UICollection parentCollection)
        {
            GButton temp = (GButton)this.MemberwiseClone();
            temp.ButtonAnim_ClickState = temp.ButtonAnim_ClickState.Clone();
            temp.ButtonAnim_HoverState = temp.ButtonAnim_HoverState.Clone();
            temp.ButtonAnim_ClickState = temp.ButtonAnim_ClickState.Clone();

            return base.Clone(temp, parent, parentCollection);
        }

        public GButton() : base()
        {
            name = "Button";
            description = "A simple button with 2 states, 'Pressed' and 'Non Pressed' (and 'hover')";
        }

        public override void Reload(UICollection uic, UIElementLayout uiel)
        {
            base.Reload(uic, uiel);
            var temp = uiel as UIButtonSaveLayout;

            if (temp.ButtonAnim_ClickState == null)
            {
                temp.ButtonAnim_ClickState = new ShapeAnimation();
            }
            if (temp.ButtonAnim_RestState == null)
            {
                temp.ButtonAnim_RestState = new ShapeAnimation();
            }
            if (temp.ButtonAnim_HoverState == null)
            {
                temp.ButtonAnim_HoverState = new ShapeAnimation();
            }

            temp.ButtonAnim_ClickState.ReloadTexture();
            temp.ButtonAnim_RestState.ReloadTexture();
            temp.ButtonAnim_HoverState.ReloadTexture();

            ButtonAnim_RestState = temp.ButtonAnim_RestState;
            ButtonAnim_HoverState = temp.ButtonAnim_HoverState;
            ButtonAnim_ClickState = temp.ButtonAnim_ClickState;
        }

        public override UIElementLayout GenerateSaveInfo(UIElementLayout UIEL)
        {
            var UIBEL = new UIButtonSaveLayout();
            UIBEL.ButtonAnim_ClickState = ButtonAnim_ClickState;
            UIBEL.ButtonAnim_HoverState = ButtonAnim_HoverState;
            UIBEL.ButtonAnim_RestState = ButtonAnim_RestState;
            return base.GenerateSaveInfo(UIBEL);
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            switch (currentState)
            {
                case ButtonState.NON_CLICKED:
                    if (new Rectangle(position, size).Contains(BaseUIElement.UIMousePos))
                    {
                        currentState = ButtonState.HOVER;
                    }
                    break;
                case ButtonState.CLICKED:
                    break;
                case ButtonState.HOVER:
                    if (!new Rectangle(position, size).Contains(BaseUIElement.UIMousePos))
                    {
                        currentState = ButtonState.NON_CLICKED;

                    }


                    if (currentState == ButtonState.HOVER)
                    {
                        if (!Utilities.KeyboardMouseUtility.AnyButtonsPressed() &&!Utilities.KeyboardMouseUtility.bMouseButtonPressed && Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                        {
                            Execute();
                        }
                    }
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

            switch (currentState)
            {
                case ButtonState.NON_CLICKED:
                    if (ButtonAnim_RestState == null) { ButtonAnim_RestState = new ShapeAnimation(); }
                    ButtonAnim_RestState.Draw(sb, new Rectangle(Point.Zero, size));
                    break;
                case ButtonState.CLICKED:
                    if (ButtonAnim_ClickState == null) { ButtonAnim_ClickState = new ShapeAnimation(); }
                    ButtonAnim_ClickState.Draw(sb, new Rectangle(Point.Zero, size));
                    break;
                case ButtonState.HOVER:
                    if (ButtonAnim_HoverState == null) { ButtonAnim_HoverState = new ShapeAnimation(); }
                    ButtonAnim_HoverState.Draw(sb, new Rectangle(Point.Zero, size));
                    break;
                default:
                    break;
            }

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Dispose()
        {
            base.Dispose();
            currentState = ButtonState.NON_CLICKED;

            ButtonAnim_RestState.SimpleReset();
            ButtonAnim_HoverState.SimpleReset();
            ButtonAnim_ClickState.SimpleReset();
        }

        internal void AssignBaseButtonAnimations(params ShapeAnimation[] sas)
        {
            ButtonAnim_RestState = sas[0];
            ButtonAnim_HoverState = sas[1];
            ButtonAnim_ClickState = sas[2];
        }

        public override void Execute()
        {
            base.Execute();
            try
            {
                executeFunction.F.Call(UICollectionParent, this);
            }
            catch (Exception e)
            {
                try
                {
                    executeFunction.F.Call(UICollectionParent, this);
                }
                catch (Exception)
                {

                }

            }

        }
    }

    [XmlRoot("UI Button Save Layout")]
    public class UIButtonSaveLayout : UIElementLayout
    {
        [XmlElement("Animation Rest State")]
        public ShapeAnimation ButtonAnim_RestState;
        [XmlElement("Animation Hover State")]
        public ShapeAnimation ButtonAnim_HoverState;
        [XmlElement("Animation Click State")]
        public ShapeAnimation ButtonAnim_ClickState;

        public UIButtonSaveLayout() { }
    }
}

namespace LUA
{
    public class LuaUIButtonInfo : LuaUIElementInfo
    {
        public LuaShapeAnimationInfo ButtonAnim_RestState = new LuaShapeAnimationInfo();
        public LuaShapeAnimationInfo ButtonAnim_HoverState = new LuaShapeAnimationInfo();
        public LuaShapeAnimationInfo ButtonAnim_ClickState = new LuaShapeAnimationInfo();

        ShapeAnimation _ButtonAnim_RestState;
        ShapeAnimation _ButtonAnim_HoverState;
        ShapeAnimation _ButtonAnim_ClickState;

        public LuaFRef exFunction = new LuaFRef();

        public void Reload()
        {
            try
            {
                _ButtonAnim_RestState = ShapeAnimation.animFromLuaInfo(ButtonAnim_RestState);
            }
            catch (Exception)
            {
                Console.WriteLine("Error in 'LuaUIButtonInfo' for 'ShapeAnimation.animFromLuaInfo(ButtonAnim_RestState)' ");
                _ButtonAnim_RestState = new ShapeAnimation();
            }

            try
            {
                _ButtonAnim_HoverState = ShapeAnimation.animFromLuaInfo(ButtonAnim_HoverState);
            }
            catch (Exception)
            {
                Console.WriteLine("Error in 'LuaUIButtonInfo' for 'ShapeAnimation.animFromLuaInfo(ButtonAnim_HoverState)' ");
                _ButtonAnim_HoverState = new ShapeAnimation();
            }

            try
            {
                _ButtonAnim_ClickState = ShapeAnimation.animFromLuaInfo(ButtonAnim_ClickState);
            }
            catch (Exception)
            {
                Console.WriteLine("Error in 'LuaUIButtonInfo' for 'ShapeAnimation.animFromLuaInfo(ButtonAnim_RestState)' ");
                _ButtonAnim_ClickState = new ShapeAnimation();
            }
        }

        internal override BaseUIElement convertFromLuaData(BaseUIElement bue)
        {
            GButton uib;
            if (bue == null)
            {
                uib = new GButton();
            }
            else
            {
                try
                {
                    uib = bue as GButton;
                }
                catch (Exception)
                {
                    uib = new GButton();
                }
            }

            try
            {
                Reload();
            }
            catch (Exception)
            {
            }

            uib.AssignBaseButtonAnimations(_ButtonAnim_RestState, _ButtonAnim_HoverState, _ButtonAnim_ClickState);

            try
            {
                exFunction.Process(LuaUICollection.luaStateConversion);
            }
            catch (Exception)
            {
                exFunction = new LuaFRef();
            }
            uib.executeFunction = exFunction;

            return base.convertFromLuaData(uib);
        }
    }

}


