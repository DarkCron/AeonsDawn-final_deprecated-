using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{


    public class ScrollPanel : SelectableElement
    {
        RenderTarget2D scrollRender;
        RenderTarget2D completeRender;
        Rectangle completeRenderPos;

        Point size;

        static TexPanel sourcePanel;
        TexPanel bg;
        TexPanel scrollPanel;

        Texture2D scrollElementsSource;
        static SpriteFont font;

        int widthScrollPanel;
        int heightScrollPanel;
        int itemHeight;
        int itemOffSetVertical;
        Rectangle renderDrawPos;

        TexPanel[] itemPanels;
        Rectangle[] itemPositions;
        internal String[] items;

        bool bNeedsScrollbarActive;
        internal int selectIndex;
        int scrollAmount;
        Point drawPos;
        ScrollWheel wheel;
        Matrix scrollMatrix = Matrix.CreateTranslation(0, 0, 1);

        static bool bInitialize = true;

        static void Initialize()
        {
            bInitialize = false;

            Texture2D guiTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
            sourcePanel = new TexPanel(guiTex, new Rectangle(0, 0, 200, 200), new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));
            font = Game1.contentManager.Load<SpriteFont>(@"Fonts\Design\BGUI\test25"); //20,25,32,48
        }

        public ScrollPanel(Point drawPos, int width, int height, int h, int hOS, params String[] items) : base()
        {
            bRequiresUpDown = true;
            position = drawPos;
            bReturnsRender = true;
            if (bInitialize)
            { Initialize(); }
            this.drawPos = drawPos;
            widthScrollPanel = width;
            heightScrollPanel = height;
            itemHeight = h;
            itemOffSetVertical = hOS;
            this.items = items;
            GeneratePanel();
            position = drawPos;
        }

        private void GeneratePanel()
        {

            int offset = 6;
            int itemWidth = widthScrollPanel - 18 - 3 * offset;
            int startX = offset;
            int startY = offset;
            itemPositions = new Rectangle[items.Length];
            itemPanels = new TexPanel[items.Length];

            wheel = new ScrollWheel(new Point(2 * offset + itemWidth, offset), heightScrollPanel - 2 * offset);

            int h = 0;
            for (int i = 0; i < items.Length; i++)
            {
                itemPositions[i] = new Rectangle(startX, startY + itemHeight * i + itemOffSetVertical * i, itemWidth, itemHeight);
                // itemPositions[i].Location += drawPos;
                itemPanels[i] = sourcePanel.positionCopy(itemPositions[i]);
                h += startY + itemHeight * i + itemOffSetVertical * i;
            }

            if (h + 2 * offset < heightScrollPanel) { bNeedsScrollbarActive = false; } else { bNeedsScrollbarActive = true; }

            bg = sourcePanel.positionCopy(new Rectangle(new Point(0), new Point(widthScrollPanel, heightScrollPanel)));
            renderDrawPos = new Rectangle(new Point(0), new Point(widthScrollPanel - 2 * offset, heightScrollPanel - offset));
            scrollRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, renderDrawPos.Width, renderDrawPos.Height);
            completeRenderPos = new Rectangle(drawPos, new Point(widthScrollPanel, heightScrollPanel));
            completeRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, completeRenderPos.Width, completeRenderPos.Height);
            elementLoc = completeRenderPos;
        }

        public void HandleMouse(Point relOS = default(Point))
        {
            Point m = Utilities.KeyboardMouseUtility.uiMousePos;
            m -= relOS;
            if (renderDrawPos.Contains(m))
            {
                m -= drawPos;
                m += new Point(0, scrollAmount);
                selectIndex = itemPositions.ToList().IndexOf(itemPositions.ToList().Find(r => r.Contains(m)));
            }
        }

        internal void HandleScroll(float amount)
        {
            if (items.Length != 0 && bNeedsScrollbarActive)
            {
                int min = itemPositions[0].Y - itemPositions[0].Height * 2;
                int max = itemPositions[items.Length - 1].Y - itemPositions[0].Height * 2;
                scrollAmount += (int)amount;
                if (scrollAmount < min)
                {
                    scrollAmount = min;
                }
                else if (scrollAmount > max)
                {
                    scrollAmount = max;
                }

                float percentage = ((float)(scrollAmount - min) / (float)(max - min));
                wheel.SetScrollBarPos(percentage);
                scrollMatrix = Matrix.CreateTranslation(0, -scrollAmount, 1);
            }

        }

        public override void Update(GameTime gt)
        {

        }

        public override KeyValuePair<RenderTarget2D, Rectangle> DrawReturnRender(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(scrollRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, scrollMatrix);

            float opacity = 0.6f;

            for (int i = 0; i < items.Length; i++)
            {
                if (i != selectIndex)
                {
                    itemPanels[i].Draw(sb, Color.LightYellow * opacity);
                }
                else
                {
                    itemPanels[i].Draw(sb, Color.LightYellow);
                }
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (i != selectIndex)
                {
                    TextUtility.Draw(sb, items[i], font, itemPositions[i], TextUtility.OutLining.Center, Color.Silver * opacity, 1f, false, scrollMatrix, Color.Gray * opacity, false);
                }
                else
                {
                    TextUtility.Draw(sb, items[i], font, itemPositions[i], TextUtility.OutLining.Center, Color.Silver, 1f, false, scrollMatrix, Color.Gray, false);
                }
            }


            sb.End();
            sb.GraphicsDevice.SetRenderTarget(completeRender);
            sb.GraphicsDevice.Clear(Color.TransparentBlack);
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            bg.Draw(sb, Color.Gray);

            sb.Draw(scrollRender, renderDrawPos, Color.White);

            if (bNeedsScrollbarActive)
            {
                wheel.Draw(sb);
            }
            else
            {
                wheel.DrawNotActive(sb);
            }
            sb.End();

            return new KeyValuePair<RenderTarget2D, Rectangle>(completeRender, completeRenderPos);
        }

        public override void Close()
        {
            scrollRender.Dispose();
            completeRender.Dispose();
        }

        internal void HandleUp()
        {
            selectIndex--;
            if (selectIndex == -1)
            {
                selectIndex++;
            }
            ChangeIndex(selectIndex);
        }

        internal void HandleDown()
        {
            selectIndex++;
            if (items.Length == selectIndex)
            {
                selectIndex--;
            }
            ChangeIndex(selectIndex);
        }

        internal void ChangeIndex(int v)
        {


            if (bNeedsScrollbarActive)
            {
                float percentage = ((float)(v / (float)(items.Length - 1)));
                wheel.SetScrollBarPos(percentage);
                //if (v > 3 && v < items.Length - 4)
                //{
                scrollMatrix = Matrix.CreateTranslation(0, -(float)(itemPositions[v].Y - itemPositions[0].Height * 2), 1);
                scrollAmount = itemPositions[v].Y - itemPositions[0].Height * 2;
                //}
                //else if (v <= 3)
                //{
                //    scrollMatrix = Matrix.CreateTranslation(0, -(float)itemPositions[0].Y, 1);
                //}
                //else if (v >= items.Length - 4)
                //{
                //    scrollMatrix = Matrix.CreateTranslation(0, -(float)itemPositions[itemPositions.Length - 1].Y, 1);
                //}
            }

        }
    }

    public class ScrollWheel : SelectableElement
    {
        Texture2D source = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\ScrollPanel");
        Rectangle upperArrSource = new Rectangle(1, 47, 14, 16);
        Rectangle lowerArrSource = new Rectangle(1, 1, 14, 16);
        Rectangle middleScrollPart = new Rectangle(16, 1, 18, 46);

        Rectangle barUpperSource = new Rectangle(35, 1, 6, 5);
        Rectangle barMiddleSource = new Rectangle(35, 7, 6, 4);
        Rectangle barLowerSource = new Rectangle(35, 12, 6, 5);

        int height;
        internal int width = 18;
        int barHeight;
        Rectangle scrollPos;
        Point scrollMinMax;

        Rectangle posUpArr;
        Rectangle posDownArr;
        Rectangle posMiddle;

        Rectangle posUpBar;
        Rectangle posMidBar;
        Rectangle posLowBar;

        internal bool bInvertedScroll = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="h"></param>
        /// <param name="w">Width of the widest element</param>
        private void GenerateScrollLocs(Point p, int h, int w = 18, int scale = 1)
        {
            height = h;
            width = w * scale;
            if (h < 90) { return; }
            posUpArr = new Rectangle(p.X + (w * scale - upperArrSource.Width * scale) / 2, p.Y + 0, upperArrSource.Width * scale, upperArrSource.Height * scale);
            posUpBar = new Rectangle(p.X + (w * scale - barUpperSource.Width * scale) / 2, posUpArr.Height + posUpArr.Y, barUpperSource.Width * scale, barUpperSource.Height * scale);
            int heightMiddle = h - upperArrSource.Height * scale - barUpperSource.Height * scale - barLowerSource.Height * scale - lowerArrSource.Height * scale;
            posMidBar = new Rectangle(p.X + (w * scale - barLowerSource.Width * scale) / 2, posUpBar.Height + posUpBar.Y, barMiddleSource.Width * scale, heightMiddle);
            posLowBar = new Rectangle(p.X + (w * scale - barLowerSource.Width * scale) / 2, posMidBar.Height + posMidBar.Y, barLowerSource.Width * scale, barLowerSource.Height * scale);
            posDownArr = new Rectangle(p.X + (w * scale - lowerArrSource.Width * scale) / 2, posLowBar.Y + posLowBar.Height, lowerArrSource.Width * scale, lowerArrSource.Height * scale);

            barHeight = h - upperArrSource.Height * scale - lowerArrSource.Height * scale;
            scrollMinMax = new Point(upperArrSource.Height * scale, upperArrSource.Height * scale + barHeight - (middleScrollPart.Height * scale));
            scrollMinMax += new Point(p.Y);
            scrollPos = new Rectangle(0, 0, scale * middleScrollPart.Width, scale * middleScrollPart.Height);
            scrollPos.X = p.X + (width - middleScrollPart.Width * scale) / 2;

            elementLoc = new Rectangle(p, new Point(w * scale, h));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">Relative</param>
        /// <param name="h"></param>
        public ScrollWheel(Point p, int h, int w = 18, int scale = 1)
        {
            GenerateScrollLocs(p, h, w, scale);
            scrollPos.Y = scrollMinMax.X;
            bFocusOnSelect = true;
            position = p;

        }

        public override void Update(GameTime gt) { }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(source, posUpArr, upperArrSource, Color.White);
            sb.Draw(source, posUpBar, barUpperSource, Color.White);
            sb.Draw(source, posMidBar, barMiddleSource, Color.White);
            sb.Draw(source, posLowBar, barLowerSource, Color.White);
            sb.Draw(source, posDownArr, lowerArrSource, Color.White);
            sb.Draw(source, scrollPos, middleScrollPart, Color.White);
        }

        internal void DrawNotActive(SpriteBatch sb)
        {
            sb.Draw(source, posUpArr, upperArrSource, Color.DarkGray * .4f);
            sb.Draw(source, posUpBar, barUpperSource, Color.DarkGray * .4f);
            sb.Draw(source, posMidBar, barMiddleSource, Color.DarkGray * .4f);
            sb.Draw(source, posLowBar, barLowerSource, Color.DarkGray * .4f);
            sb.Draw(source, posDownArr, lowerArrSource, Color.DarkGray * .4f);
            //sb.Draw(source, scrollPos, middleScrollPart, Color.White);
        }

        internal void SetScrollBarPos(float percentage)
        {
            if (!bInvertedScroll)
            {
                scrollPos.Y = (int)(scrollMinMax.X * (1.0f - percentage) + (scrollMinMax.Y) * percentage);
            }
            else
            {
                percentage = 1.0f - percentage;
                scrollPos.Y = (int)(scrollMinMax.X * (1.0f - percentage) + (scrollMinMax.Y) * percentage);
            }

        }

        internal int getPercentageHundred()
        {
            if (!bInvertedScroll)
            {
                int val = (int)(((float)(scrollPos.Y - scrollMinMax.X) / (float)(scrollMinMax.Y - scrollMinMax.X)) * 100) + 1;
                if (val==-1)
                {
                    val = 0;
                }else if(val == 99)
                {
                    val = 100;
                }
                else if (val == 49)
                {
                    val = 50;
                }
                return val;
            }
            else
            {
                int val = 100 - ((int)(((float)(scrollPos.Y - scrollMinMax.X) / (float)(scrollMinMax.Y - scrollMinMax.X)) * 100) + 1);
                if (val == -1)
                {
                    val = 0;
                }
                else if (val == 99)
                {
                    val = 100;
                }
                else if (val == 49)
                {
                    val = 50;
                }
                return val;
            }
        }
    }
}
