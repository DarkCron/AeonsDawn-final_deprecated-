using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;
using TBAGW.Utilities.Control.Player;

namespace TBAGW
{
    public class UIElement
    {
        RenderTarget2D uiRender;
        Rectangle location;
        public bool bIsActive = false;


        public UIElement(Rectangle locationToDraw)
        {
            location = locationToDraw;
            uiRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, location.Width, location.Height);
        }

        public virtual void Generate(SpriteBatch sb)
        {
            sb.End();

            sb.GraphicsDevice.SetRenderTarget(uiRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(uiRender, location, uiRender.Bounds, Color.White);
        }

        public RenderTarget2D getRenderSource()
        {
            return uiRender;
        }

        public Rectangle Location()
        {
            return location;
        }

        public void ChangeLocation(Rectangle r)
        {
            location = r;
        }

        public void Move(Point p)
        {
            location.X += p.X;
            location.Y += p.Y;
        }
    }

    public class PickUpScreen : UIElement
    {
        static Rectangle panelItemFrameSource = new Rectangle(3076, 384, 156, 43);
        static Rectangle panelFrameSource = new Rectangle(2896, 384, 178, 226);
        static Texture2D menuTextureSheet = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");

        Rectangle TitleTextBox = new Rectangle();
        String title = "";
        static Point nameOffSet = new Point(15, 5);
        Rectangle location;

        Rectangle itemCollectionRenderLocation = new Rectangle();
        float scale = 1.0f;

        RenderTarget2D itemPanelRender;
        public Matrix itemPanelScrollMatrix = Matrix.CreateTranslation(0, 0, 1);
        List<itemPanel> lip = new List<itemPanel>();
        public float verticalModifier = 0;
        public float maxVerticalModifier = 0;

        DescriptionScreen ds = null;

        public bool bBeingDragged = false;
        public Vector2 clickMoveOffSet = Vector2.Zero;

        public itemPanel selectedItemPanel;
        PickUpEntity PUEntity;

        public struct itemPanel
        {
            static Vector2 globalOffSet = new Vector2(4, 3);                                                            
            static Vector2 frameSize = new Vector2(156, 43);
            static Vector2 frameOffSet = new Vector2(0, 6 + 43);
            Rectangle frameLocation;
            float scale;

            static Vector2 displayFrameOffSet = new Vector2(3, 3);
            static Vector2 displayFrameSize = new Vector2(37, 37);
            Rectangle displayFrameLocation;

            static Vector2 displayNameOffSet = new Vector2(46, 3);
            static Vector2 displayNameSize = new Vector2(107, 18);
            Rectangle displayNameLocation;

            static Vector2 displayNameTypeOffSet = new Vector2(46, 21);
            static Vector2 displayNameTypeSize = new Vector2(107, 18);
            Rectangle displayNameTypeLocation;

            static Vector2 displayAmountTypeOffSet = new Vector2(28, 30);
            static Vector2 displayAmountTypeSize = new Vector2(15, 15);
            Rectangle displayAmountTypeLocation;

            BaseItem bi;

            public itemPanel(int index, float scale, BaseItem bi)
            {
                this.scale = scale;
                frameLocation = new Rectangle((frameOffSet * scale * index + globalOffSet).ToPoint(), (frameSize * scale).ToPoint());

                this.bi = bi;

                Point tempLoc = (displayFrameOffSet * scale).ToPoint();
                Point tempSize = (displayFrameSize * scale).ToPoint();
                displayFrameLocation = new Rectangle(frameLocation.Location + tempLoc, tempSize);

                tempLoc = (displayNameOffSet * scale).ToPoint();
                tempSize = (displayNameSize * scale).ToPoint();
                displayNameLocation = new Rectangle(frameLocation.Location + tempLoc, tempSize);

                tempLoc = (displayNameTypeOffSet * scale).ToPoint();
                tempSize = (displayNameTypeSize * scale).ToPoint();
                displayNameTypeLocation = new Rectangle(frameLocation.Location + tempLoc, tempSize);

                tempLoc = (displayAmountTypeOffSet * scale).ToPoint();
                tempSize = (displayAmountTypeSize * scale).ToPoint();
                displayAmountTypeLocation = new Rectangle(frameLocation.Location + tempLoc, tempSize);
            }

            public void Draw(SpriteBatch sb, Matrix m)
            {
                sb.Draw(menuTextureSheet, frameLocation, panelItemFrameSource, Color.White);
                sb.Draw(menuTextureSheet, new Rectangle(displayFrameLocation.X - 1, displayFrameLocation.Y - 1, displayFrameLocation.Width + 2, displayFrameLocation.Height + 2), panelItemFrameSource, Color.White);
                bi.itemTexAndAnimation.Draw(sb, displayFrameLocation);
                // sb.Draw(itemPanel., displayFrameLocation, Color.Red);
                // sb.Draw(Game1.hitboxHelp, displayNameLocation, Color.Green);
                TextUtility.Draw(sb, "x" + bi.itemAmount.ToString(), BattleGUI.testSF25, displayAmountTypeLocation, TextUtility.OutLining.Left, bi.itemRarityColour(), 0f, false, m);
                TextUtility.Draw(sb, bi.itemName, BattleGUI.testSF25, displayNameLocation, TextUtility.OutLining.Left, bi.itemRarityColour(), 0f, false, m);
                TextUtility.Draw(sb, bi.itemType.ToString(), BattleGUI.testSF25, displayNameTypeLocation, TextUtility.OutLining.Left, Color.Silver, 0f, false, m);
            }

            public void Draw(SpriteBatch sb, Matrix m, float opacity)
            {
                sb.Draw(menuTextureSheet, frameLocation, panelItemFrameSource, Color.White * opacity);
                sb.Draw(menuTextureSheet, new Rectangle(displayFrameLocation.X - 1, displayFrameLocation.Y - 1, displayFrameLocation.Width + 2, displayFrameLocation.Height + 2), panelItemFrameSource, Color.White * opacity);
                bi.itemTexAndAnimation.Draw(sb, displayFrameLocation, opacity);
                // sb.Draw(itemPanel., displayFrameLocation, Color.Red);
                // sb.Draw(Game1.hitboxHelp, displayNameLocation, Color.Green);
                TextUtility.Draw(sb, "x" + bi.itemAmount.ToString(), BattleGUI.testSF25, displayAmountTypeLocation, TextUtility.OutLining.Left, bi.itemRarityColour() * opacity, 0f, false, m, default(Color), false);
                TextUtility.Draw(sb, bi.itemName, BattleGUI.testSF25, displayNameLocation, TextUtility.OutLining.Left, bi.itemRarityColour() * opacity, 0f, false, m, default(Color), false);
                TextUtility.Draw(sb, bi.itemType.ToString(), BattleGUI.testSF25, displayNameTypeLocation, TextUtility.OutLining.Left, Color.Silver * opacity, 0f, false, m, default(Color), false);
            }

            public int End()
            {
                return frameLocation.Y - 100;
            }

            public void Update(GameTime gt)
            {
                bi.UpdateAnimation(gt);
            }

            public BaseItem getItemFromPanel()
            {
                return bi;
            }

            public bool Contains(Rectangle r)
            {
                if (r.Contains(frameLocation) || r.Intersects(frameLocation))
                {
                    return true;
                }
                return false;
            }

            public Rectangle frameLoc(int verticalMod, Point loc)
            {
                return new Rectangle(frameLocation.X + loc.X, frameLocation.Y + loc.Y - verticalMod, frameLocation.Width, frameLocation.Height);
            }
        }

        internal void TryPickUp()
        {
            if (selectedItemPanel.getItemFromPanel() != null)
            {
                BaseItem bi = selectedItemPanel.getItemFromPanel();
                if (InventoryManager.CanAddItemToInventory(bi))
                {
                    InventoryManager.AddItemToInventory(bi);
                    PUEntity.RemoveItemFromList(bi);
                    int index = lip.IndexOf(lip.Find(li => li.getItemFromPanel() == bi));
                    lip.Remove(lip.Find(li => li.getItemFromPanel() == bi));

                    if (lip.Count == 0)
                    {
                        Close();
                    }
                    else
                    {
                        if (--index < 0)
                        {
                            index = 0;
                        }
                        SelectItem(lip[index]);
                    }

                    InventoryManager.playerInventory.ManageStackableItems();
                }
                else
                {
                    //Cannot add item
                }

            }
        }

        public PickUpScreen(Rectangle locationToDrawRender, String title, PickUpEntity PUEntity) : base(locationToDrawRender)
        {
            this.PUEntity = PUEntity;
            bIsActive = true;
            location = new Rectangle(0, 0, locationToDrawRender.Width, locationToDrawRender.Height);

            TitleTextBox = new Rectangle(nameOffSet, new Point(locationToDrawRender.Width - nameOffSet.X * 2, 50 - nameOffSet.Y));
            this.title = title;

            itemCollectionRenderLocation = new Rectangle(15, 62, locationToDrawRender.Width - 15 * 2, locationToDrawRender.Height - 62 - 4);

            scale = (float)locationToDrawRender.Width / (float)panelFrameSource.Width;
            itemPanelRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, itemCollectionRenderLocation.Width, itemCollectionRenderLocation.Height);

            int index = 0;
            foreach (var item in PUEntity.itemList)
            {
                lip.Add(new itemPanel(index, scale, item));
                index++;
            }

            if (lip.Count != 0)
            {
                maxVerticalModifier = lip.Last().End();
                //  ds = new DescriptionScreen(this, lip[0].getItemFromPanel(), (int)(130 * scale), (int)(190 * scale));
                // GameProcessor.popUpRenders.Add(ds);
            }
            else
            {
                maxVerticalModifier = 0;
            }

        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            //TextUtility.ClearCache();
            for (int i = 0; i < lip.Count; i++)
            {
                lip[i].Update(gt);
            }

            if (!KeyboardMouseUtility.bMouseButtonPressed && bBeingDragged)
            {
                bBeingDragged = false;
            }

        }

        public override void Generate(SpriteBatch sb)
        {
            //if (ds != null)
            //{
            //    ds.Generate(sb);
            //}

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(itemPanelRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, itemPanelScrollMatrix);

            var temp = new Rectangle(new Point(0, (int)verticalModifier - 5), itemPanelRender.Bounds.Size);

            if (!lip.Contains(selectedItemPanel))
            {
                foreach (var item in lip.FindAll(b => b.Contains(temp)))
                {
                    item.Draw(sb, itemPanelScrollMatrix);
                }
            }
            else
            {
                foreach (var item in lip.FindAll(b => b.Contains(temp) && !b.Equals(selectedItemPanel)))
                {
                    item.Draw(sb, itemPanelScrollMatrix, .35f);
                }

                selectedItemPanel.Draw(sb, itemPanelScrollMatrix);
            }




            sb.End();
            sb.GraphicsDevice.SetRenderTarget(getRenderSource());
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);

            sb.Draw(menuTextureSheet, location, panelFrameSource, Color.White);
            // sb.Draw(Game1.hitboxHelp, TitleTextBox, Color.White);
            TextUtility.Draw(sb, title, BattleGUI.testSF32, TitleTextBox, TextUtility.OutLining.Center, Color.White, 0f, false);

            // sb.Draw(Game1.hitboxHelp, itemCollectionRenderLocation, Color.White);
            sb.Draw(itemPanelRender, itemCollectionRenderLocation, itemPanelRender.Bounds, Color.White);

            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

        public void Clicked()
        {

            Rectangle titleActualBox = new Rectangle(Location().X, Location().Y, TitleTextBox.Width, TitleTextBox.Height);
            Vector2 beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;

            if (titleActualBox.Contains(beep) && !bBeingDragged)
            {
                clickMoveOffSet = Location().Location.ToVector2() - beep;
                bBeingDragged = true;
            }
            else
            {
                foreach (var item in lip)
                {
                    if (item.frameLoc((int)(verticalModifier), Location().Location + itemCollectionRenderLocation.Location).Contains(beep))
                    {
                        selectedItemPanel = item;
                        verticalModifier = selectedItemPanel.End();
                        itemPanelScrollMatrix = Matrix.CreateTranslation(0, -verticalModifier, 0);
                        openDifferentDescription();
                        KeyboardMouseUtility.bPressed = true;
                        break;
                    }
                }
            }



        }

        public void Drag()
        {
            Vector2 beep = Mouse.GetState().Position.ToVector2() / ResolutionUtility.stdScale;
            Vector2 delta = -(Location().Location.ToVector2() - (beep + clickMoveOffSet));
            Vector2 pos = Location().Location.ToVector2();
            Move(delta.ToPoint());
            if (ds != null)
            {
                ds.Move(delta.ToPoint());
            }

        }

        public void MoveRenderControl(Vector2 p)
        {
            Move(p.ToPoint());
            if (ds != null)
            {
                ds.Move(p.ToPoint());
            }
        }

        public List<itemPanel> getPanels()
        {
            return lip;
        }

        public void openDifferentDescription()
        {
            GameProcessor.popUpRenders.Remove(ds);
            ds = new DescriptionScreen(this, selectedItemPanel.getItemFromPanel(), (int)(130 * scale), (int)(190 * scale));
            GameProcessor.popUpRenders.Add(ds);
        }

        internal void SelectItem(itemPanel ip)
        {
            selectedItemPanel = ip;
            verticalModifier = selectedItemPanel.End();
            itemPanelScrollMatrix = Matrix.CreateTranslation(0, -verticalModifier, 0);
            openDifferentDescription();
        }

        internal void Close()
        {
            GameProcessor.popUpRenders.Remove(this);
            if (ds != null)
            {
                GameProcessor.popUpRenders.Remove(ds);
            }
            NonCombatCtrl.currentControls = NonCombatCtrl.ControlSetup.Standard;
        }
    }

    public class DescriptionScreen : UIElement
    {
        PickUpScreen pu;
        BaseItem bi;
        Rectangle textBox;
        static Rectangle panelItemFrameSource = new Rectangle(3076, 384, 156, 43);
        static Texture2D menuTextureSheet = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
        bool bRight = false;

        Rectangle frameBox;
        //pu.Location().X - width
        public DescriptionScreen(PickUpScreen pu, BaseItem bi, int width, int height, bool bRight = false) : base(new Microsoft.Xna.Framework.Rectangle(pu.Location().X - width, pu.Location().Y, width, height))
        {
            this.pu = pu;
            this.bi = bi;
            var temp = this.pu.Location();
            this.bRight = bRight;

            if (Location().X < 0)
            {
                ChangeLocation(new Rectangle(pu.Location().X + pu.Location().Width, pu.Location().Y, Location().Width, Location().Height));
                bRight = true;
            }

            frameBox = new Rectangle(0, 0, Location().Width, Location().Height);
            textBox = new Rectangle(0 + 5, 0 + 5, Location().Width - 10, Location().Height - 10);
        }

        public override void Update(GameTime gt)
        {
            if (!bRight)
            {
                if (Location().X < 0)
                {
                    bRight = true;
                    ChangeLocation(new Rectangle(pu.Location().X + pu.Location().Width, pu.Location().Y, Location().Width, Location().Height));
                }
            }
            else
            {
                if (Location().X + Location().Width > 1366)
                {
                    bRight = false;
                    ChangeLocation(new Rectangle(pu.Location().X - Location().Width, pu.Location().Y, Location().Width, Location().Height));
                }
            }
        }

        public override void Generate(SpriteBatch sb)
        {
            base.Generate(sb);
            sb.Draw(menuTextureSheet, frameBox, panelItemFrameSource, Color.White);
            //   sb.Draw(Game1.hitboxHelp, textBox, Color.White);
            TextUtility.DrawComplex(sb, bi.itemDescription, BattleGUI.testSF32, textBox, TextUtility.OutLining.Center, Color.White, 0f, false);

        }
    }
}
