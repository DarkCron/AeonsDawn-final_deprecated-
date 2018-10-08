using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    public class TexPanel
    {
        Texture2D tex;
        Rectangle finalPos;
        #region source
        Rectangle upperMiddleCenter = new Rectangle();
        Rectangle lowerMiddleCenter = new Rectangle();

        Rectangle leftMiddleCenter = new Rectangle();
        Rectangle rightMiddleCenter = new Rectangle();

        Rectangle upperLeftCorner = new Rectangle();
        Rectangle upperRightCorner = new Rectangle();
        Rectangle lowerLeftCorner = new Rectangle();
        Rectangle lowerRightCorner = new Rectangle();

        Rectangle middle = new Microsoft.Xna.Framework.Rectangle();
        #endregion

        #region positions
        Rectangle upperMiddleCenterPos = new Rectangle();
        Rectangle lowerMiddleCenterPos = new Rectangle();

        Rectangle leftMiddleCenterPos = new Rectangle();
        Rectangle rightMiddleCenterPos = new Rectangle();

        Rectangle upperLeftCornerPos = new Rectangle();
        Rectangle upperRightCornerPos = new Rectangle();
        Rectangle lowerLeftCornerPos = new Rectangle();
        Rectangle lowerRightCornerPos = new Rectangle();

        Rectangle middlePos = new Rectangle();
        #endregion

        public TexPanel(Texture2D tex, Rectangle finalPos, Rectangle upperMiddleCenter, Rectangle lowerMiddleCenter, Rectangle leftMiddleCenter, Rectangle rightMiddleCenter,
            Rectangle upperLeftCorner, Rectangle upperRightCorner, Rectangle lowerLeftCorner, Rectangle lowerRightCorner, Rectangle middle)
        {
            this.tex = tex;
            this.finalPos = finalPos;

            this.upperMiddleCenter = upperMiddleCenter;
            this.lowerMiddleCenter = lowerMiddleCenter;
            this.leftMiddleCenter = leftMiddleCenter;
            this.rightMiddleCenter = rightMiddleCenter;

            this.upperLeftCorner = upperLeftCorner;
            this.upperRightCorner = upperRightCorner;
            this.lowerLeftCorner = lowerLeftCorner;
            this.lowerRightCorner = lowerRightCorner;

            this.middle = middle;

            upperLeftCornerPos = new Rectangle(new Point(finalPos.X, finalPos.Y), upperLeftCorner.Size);
            upperMiddleCenterPos = new Rectangle(new Point(finalPos.X + upperLeftCorner.Width, finalPos.Y), new Point(finalPos.Width - upperLeftCorner.Width - upperRightCorner.Width, upperMiddleCenter.Height));
            upperRightCornerPos = new Rectangle(new Point(finalPos.X + finalPos.Width - upperRightCorner.Width, finalPos.Y), upperRightCorner.Size);

            rightMiddleCenterPos = new Rectangle(new Point(finalPos.X + finalPos.Width - upperRightCorner.Width, finalPos.Y + upperRightCorner.Height), new Point(rightMiddleCenter.Width, finalPos.Height - upperRightCorner.Height - lowerRightCorner.Height));
            leftMiddleCenterPos = new Rectangle(new Point(finalPos.X, finalPos.Y + upperRightCorner.Height), new Point(leftMiddleCenter.Width, finalPos.Height - upperLeftCorner.Height - lowerLeftCorner.Height));

            lowerLeftCornerPos = new Rectangle(new Point(finalPos.X, finalPos.Y + finalPos.Height - lowerLeftCorner.Height), lowerLeftCorner.Size);
            lowerMiddleCenterPos = new Rectangle(new Point(finalPos.X + lowerLeftCorner.Width, finalPos.Y + finalPos.Height - lowerMiddleCenter.Height), new Point(finalPos.Width - lowerLeftCorner.Width - lowerRightCorner.Width, lowerMiddleCenter.Height));
            lowerRightCornerPos = new Rectangle(new Point(finalPos.X + finalPos.Width - lowerLeftCorner.Width, finalPos.Y + finalPos.Height - lowerRightCorner.Height), lowerRightCorner.Size);

            middlePos = new Rectangle(new Point(finalPos.X + leftMiddleCenter.Width, finalPos.Y + upperMiddleCenter.Height), new Point(finalPos.Width - rightMiddleCenter.Width - leftMiddleCenter.Width, finalPos.Height - upperMiddleCenter.Height - lowerMiddleCenter.Height));
        }

        public virtual void Update(GameTime gt) { }

        public virtual void Draw(SpriteBatch sb, Color c)
        {
            sb.Draw(tex, upperLeftCornerPos, upperLeftCorner, c);
            sb.Draw(tex, upperMiddleCenterPos, upperMiddleCenter, c);
            sb.Draw(tex, upperRightCornerPos, upperRightCorner, c);
            //sb.Draw(Game1.WhiteTex,upperRightCornerPos,Color.Green);

            sb.Draw(tex, leftMiddleCenterPos, leftMiddleCenter, c);
            sb.Draw(tex, rightMiddleCenterPos, rightMiddleCenter, c);

            sb.Draw(tex, lowerLeftCornerPos, lowerLeftCorner, c);
            sb.Draw(tex, lowerMiddleCenterPos, lowerMiddleCenter, c);
            sb.Draw(tex, lowerRightCornerPos, lowerRightCorner, c);
            //sb.Draw(Game1.WhiteTex, lowerRightCornerPos, Color.Green);

            sb.Draw(tex, middlePos, middle, c);
        }

        public Rectangle Position()
        {
            return middlePos;
        }

        public TexPanel positionCopy(Rectangle finalPos)
        {
            TexPanel tp = new TexPanel(this.tex, finalPos, this.upperMiddleCenter, this.lowerMiddleCenter, this.leftMiddleCenter, this.rightMiddleCenter, this.upperLeftCorner, this.upperRightCorner, this.lowerLeftCorner, this.lowerRightCorner, this.middle);
            return tp;
        }

        internal bool ContainsMouse(Point p)
        {
            if (finalPos.Contains(p))
            {
                return true;
            }
            return false;
        }
    }

    public class TextTexPanel : TexPanel
    {
        String TextAddition = "";
        GameText text;
        SpriteFont sf;
        Color tc;
        Color lc;
        TextUtility.OutLining ol;
        bool bUpScaling;

        int stepsTaken = 0;
        int steps = 20;
        bool bSelected = false;
        bool bWasSelected = false;
        Color sc;
        TimingUtility tu;

        public TextTexPanel(Texture2D tex, Rectangle finalPos, Rectangle upperMiddleCenter, Rectangle lowerMiddleCenter, Rectangle leftMiddleCenter, Rectangle rightMiddleCenter,
    Rectangle upperLeftCorner, Rectangle upperRightCorner, Rectangle lowerLeftCorner, Rectangle lowerRightCorner, Rectangle middle) : base(tex, finalPos, upperMiddleCenter, lowerMiddleCenter, leftMiddleCenter, rightMiddleCenter,
    upperLeftCorner, upperRightCorner, lowerLeftCorner, lowerRightCorner, middle)
        {
        }

        public void Setup(GameText text, SpriteFont sf, Color tc, Color lc, TextUtility.OutLining ol, bool bUpScaling)
        {
            this.text = text;
            this.sf = sf;
            this.tc = tc;
            this.lc = lc;
            this.bUpScaling = bUpScaling;
            this.ol = ol;
        }

        public override void Update(GameTime gt)
        {
            base.Update(gt);
            if (bSelected || bWasSelected)
            {
                tu.Tick(gt);
                while (tu.Ding())
                {
                    stepsTaken++;
                }
            }
        }

        public void Draw(SpriteBatch sb, Color c, float opacityText = 1f)
        {
            base.Draw(sb, c);
            if (text != null)
            {
                if (!bSelected && !bWasSelected)
                {
                    TextUtility.Draw(sb, text.getText() + TextAddition, sf, Position(), ol, tc * opacityText, 1f, bUpScaling, default(Matrix), lc * opacityText, false);
                }

                if (bSelected && !bWasSelected)
                {
                    float otherOpacity = (float)(stepsTaken / (float)steps);
                    if (otherOpacity != 0)
                    {
                        TextUtility.Draw(sb, text.getText() + TextAddition, sf, Position(), ol, sc * opacityText * (otherOpacity), 1f, bUpScaling, default(Matrix), lc * opacityText * (otherOpacity), false);
                    }
                    if (otherOpacity != 1.0f)
                    {
                        TextUtility.Draw(sb, text.getText() + TextAddition, sf, Position(), ol, tc * opacityText * (1.0f - otherOpacity), 1f, bUpScaling, default(Matrix), lc * opacityText * (1.0f - otherOpacity), false);
                    }


                }

                if (!bSelected && bWasSelected)
                {
                    float otherOpacity = (float)(stepsTaken / (float)steps);
                    if (otherOpacity != 0f)
                    {
                        TextUtility.Draw(sb, text.getText() + TextAddition, sf, Position(), ol, tc * opacityText * (otherOpacity), 1f, bUpScaling, default(Matrix), lc * opacityText * (otherOpacity), false);
                    }
                    if (otherOpacity != 1.0f)
                    {
                        TextUtility.Draw(sb, text.getText() + TextAddition, sf, Position(), ol, sc * opacityText * (1.0f - otherOpacity), 1f, bUpScaling, default(Matrix), lc * opacityText * (1.0f - otherOpacity), false);
                    }
                }
            }

        }

        internal bool StopTimerWhen()
        {
            if (stepsTaken >= steps)
            {
                if (bWasSelected)
                {
                    bWasSelected = false;
                    stepsTaken = 0;
                }
                return true;
            }
            return false;
        }

        internal void ContainsMouse(Point p, Color c)
        {
            if (Position().Contains(p))
            {
                if (!bSelected)
                {
                    sc = c;
                    bSelected = true;
                    tu = new TimingUtility(30, true, StopTimerWhen);
                    stepsTaken = steps - stepsTaken;
                    bWasSelected = false;
                }

            }
            else if (bSelected)
            {
                bSelected = false;
                bWasSelected = true;
                tu = new TimingUtility(30, true, StopTimerWhen);
                stepsTaken = steps - stepsTaken;
            }
            else
            {
                bSelected = false;
            }

        }

        internal bool IsSelected()
        {
            return bSelected;
        }

        internal void SetTextAddition(String s)
        {
            TextAddition = s;
        }
    }
}
