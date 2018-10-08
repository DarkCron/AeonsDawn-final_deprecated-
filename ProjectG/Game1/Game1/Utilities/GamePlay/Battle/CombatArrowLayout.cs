using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.AI;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    internal static class CombatArrowLayout
    {

        static Texture2D arrowTexSheet = Game1.contentManager.Load<Texture2D>(@"Design\BattleScreen\GUI_Sheet_2048x2048");

        static ShapeAnimation horizontalL = new ShapeAnimation();
        static ShapeAnimation rightL = new ShapeAnimation();
        static ShapeAnimation leftL = new ShapeAnimation();
        static ShapeAnimation verticalL = new ShapeAnimation();
        static ShapeAnimation downL = new ShapeAnimation();
        static ShapeAnimation upL = new ShapeAnimation();
        static ShapeAnimation leftUpL = new ShapeAnimation();
        static ShapeAnimation rightUpL = new ShapeAnimation();
        static ShapeAnimation leftDownL = new ShapeAnimation();
        static ShapeAnimation rightDownL = new ShapeAnimation();

        static bool bMustInitialize = true;
        static internal bool bMustDraw = false;

        static BasicTile lastCheckedTile;
        static List<arrowInfo> lai = new List<arrowInfo>();

        static internal void Initialize()
        {
            int index = 0;

            horizontalL.animationTexture = arrowTexSheet;
            horizontalL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            rightL.animationTexture = arrowTexSheet;
            rightL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            leftL.animationTexture = arrowTexSheet;
            leftL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            verticalL.animationTexture = arrowTexSheet;
            verticalL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            downL.animationTexture = arrowTexSheet;
            downL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            upL.animationTexture = arrowTexSheet;
            upL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            leftUpL.animationTexture = arrowTexSheet;
            leftUpL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            rightUpL.animationTexture = arrowTexSheet;
            rightUpL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            leftDownL.animationTexture = arrowTexSheet;
            leftDownL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            rightDownL.animationTexture = arrowTexSheet;
            rightDownL.animationFrames.Add(new Rectangle(192 + index++ * 64, 1171, 64, 64));

            bMustInitialize = false;
        }

        static internal void Start(BasicTile t, CharacterTurn ct)
        {
            if (bMustInitialize)
            {
                Initialize();
            }
            lai.Clear();
            CalculatePossiblePath(ct);
            lastCheckedTile = t;
        }

        static internal bool CanClear()
        {
            return lai.Count > 0 ? true : false;
        }

        static internal void Clear()
        {
            lai.Clear();
        }

        static internal void CalculatePossiblePath(CharacterTurn ct)
        {
            BaseCharacter selectedChar = ct.character;
            Vector2 temp = new Vector2(-1);
            if (ct.returnCompleteArea().Find(t => t.mapPosition.Contains(GameProcessor.EditorCursorPos)) != null)
            {
                temp = (GameProcessor.EditorCursorPos / 64).ToPoint().ToVector2() * 64;
            }



            if (temp != new Vector2(-1))
            //    if (temp != new Vector2(-1) && !selectedChar.spriteGameSize.Contains(temp))
            {
                List<Node> nodes = new List<Node>();

                if (CombatProcessor.zone.Contains(GameProcessor.EditorCursorPos) && !PathMoveHandler.bIsBusy)
                {
                    nodes = PathFinder.NewPathSearch(selectedChar.position, GameProcessor.EditorCursorPos, ct.returnCompleteArea());
                }

                if (nodes.Count > 2)
                {
                    for (int i = 0; i < nodes.Count - 1; i++)
                    {
                        Node pn = nodes[i];
                        Node n = nodes[i + 1];
                        Node na = nodes.Count > i + 2 ? nodes[i + 2] : null;
                        lai.Add(new arrowInfo(pn, n, na));
                    }
                }
                else if (nodes.Count == 2)
                {
                    Point pos = selectedChar.positionToMapCoords().ToPoint();
                    if (pos.X + 1 == nodes[1].coord.X)
                    {
                        lai.Add(new arrowInfo(rightL,new Rectangle((nodes[1].coord.ToVector2()*64).ToPoint(), new Point(64))));
                    }
                    else if (pos.X - 1 == nodes[1].coord.X)
                    {
                        lai.Add(new arrowInfo(leftL, new Rectangle((nodes[1].coord.ToVector2() * 64).ToPoint(), new Point(64))));
                    }else
                    if (pos.Y + 1 == nodes[1].coord.Y)
                    {
                        lai.Add(new arrowInfo(downL, new Rectangle((nodes[1].coord.ToVector2() * 64).ToPoint(), new Point(64))));
                    }else
                    if (pos.Y - 1 == nodes[1].coord.Y)
                    {
                        lai.Add(new arrowInfo(upL, new Rectangle((nodes[1].coord.ToVector2() * 64).ToPoint(), new Point(64))));
                    }
                }
            }


        }

        static internal void Update(GameTime gt)
        {
            if (bMustDraw)
            {
                lai.ForEach(ai => ai.Update(gt));
            }
        }

        static internal void Draw(SpriteBatch sb)
        {
            if (bMustDraw)
            {
                lai.ForEach(ai => ai.Draw(sb));
            }
        }

        internal struct arrowInfo
        {

            ShapeAnimation anim;
            Rectangle loc;

            internal arrowInfo(ShapeAnimation sa, Rectangle l)
            {
                anim = sa;
                loc = l;
            }

            internal arrowInfo(Node pn, Node n, Node na)
            {
                loc = new Rectangle((n.coord.ToVector2() * 64).ToPoint(), new Point(64));
                if (na != null)
                {
                    if (IsHorizontal(pn, n, na))
                    {
                        anim = horizontalL;
                    }
                    else if (IsVertical(pn, n, na))
                    {
                        anim = verticalL;
                    }
                    else if (IsLeftUp(pn, n, na))
                    {
                        anim = leftUpL;
                    }
                    else if (IsRightUp(pn, n, na))
                    {
                        anim = rightUpL;
                    }
                    else if (IsRightDown(pn, n, na))
                    {
                        anim = rightDownL;
                    }
                    else //if (IsLeftDown(pn, n, na))
                    {
                        anim = leftDownL;
                    }
                }
                else //if (na == null)
                {
                    if (isArrowLeft(pn, n))
                    {
                        anim = leftL;
                    }
                    else if (isArrowRight(pn, n))
                    {
                        anim = rightL;
                    }
                    else if (isArrowDown(pn, n))
                    {
                        anim = downL;
                    }
                    else
                    {
                        anim = upL;
                    }
                }


            }

            internal void Update(GameTime gt)
            {
                anim.UpdateAnimationForItems(gt);
            }

            internal void Draw(SpriteBatch sb)
            {
                anim.Draw(sb, loc, 1.0f, SpriteEffects.None, Color.White);
                //sb.Draw(arrowTexSheet, new Rectangle(192,1171,64,64),Color.Red);
            }

            static bool isArrowLeft(Node pn, Node n)
            {
                if (n.coord.X + 1 == pn.coord.X)
                {
                    return true;
                }

                return false;
            }

            static bool isArrowRight(Node pn, Node n)
            {
                if (n.coord.X - 1 == pn.coord.X)
                {
                    return true;
                }

                return false;
            }

            static bool isArrowDown(Node pn, Node n)
            {
                if (n.coord.Y - 1 == pn.coord.Y)
                {
                    return true;
                }

                return false;
            }




            static bool IsHorizontal(Node pn, Node n, Node na)
            {
                if (pn.coord.X + 2 == na.coord.X)
                {
                    return true;
                }
                else if (pn.coord.X - 2 == na.coord.X)
                {
                    return true;
                }

                return false;
            }

            static bool IsVertical(Node pn, Node n, Node na)
            {
                if (pn.coord.Y + 2 == na.coord.Y)
                {
                    return true;
                }
                else if (pn.coord.Y - 2 == na.coord.Y)
                {
                    return true;
                }

                return false;
            }

            static bool IsLeftUp(Node pn, Node n, Node na)
            {
                if (pn.coord.X + 1 == n.coord.X && n.coord.Y - 1 == na.coord.Y)
                {
                    return true;
                }
                else if (pn.coord.Y + 1 == n.coord.Y && n.coord.X - 1 == na.coord.X)
                {
                    return true;
                }

                return false;
            }

            static bool IsRightUp(Node pn, Node n, Node na)
            {
                if (pn.coord.Y + 1 == n.coord.Y && n.coord.X + 1 == na.coord.X)
                {
                    return true;
                }
                else if (pn.coord.X - 1 == n.coord.X && n.coord.Y - 1 == na.coord.Y)
                {
                    return true;
                }

                return false;
            }

            static bool IsLeftDown(Node pn, Node n, Node na)
            {
                if (pn.coord.X + 1 == n.coord.X && n.coord.Y + 1 == na.coord.Y)
                {
                    return true;
                }
                else if (pn.coord.Y - 1 == n.coord.Y && n.coord.X - 1 == na.coord.X)
                {
                    return true;
                }

                return false;
            }

            static bool IsRightDown(Node pn, Node n, Node na)
            {
                if (pn.coord.Y - 1 == n.coord.Y && n.coord.X + 1 == na.coord.X)
                {
                    return true;
                }
                else if (pn.coord.X - 1 == n.coord.X && n.coord.Y + 1 == na.coord.Y)
                {
                    return true;
                }

                return false;
            }
        }

        static internal bool bShouldRecheck(BasicTile t)
        {
            bMustDraw = true;
            if (lastCheckedTile == null || t.mapPosition != lastCheckedTile.mapPosition)
            {
                return true;
            }
            return false;
        }
    }
}
