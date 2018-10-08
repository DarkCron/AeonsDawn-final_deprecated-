using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    public class TalentGrid
    {
        public List<TalentNode> nodes = new List<TalentNode>();

        internal BaseCharacter parentBC;
        internal CharacterClassCollection parentCCC = null;
        internal RenderTarget2D gridRender;
        static internal Matrix m = Matrix.CreateTranslation((1366 / 2 - 32), (768 / 2 - 32), 1) * Matrix.CreateScale(1f, 1f, 1f);
        static internal Point mPos = new Point(0);
        static internal float mScale = 1f;
        static internal bool bUpdateMatrix = false;
        static internal TalentNode hoverOverNode = null;


        public TalentGrid()
        {

        }

        public void Update(GameTime gt)
        {
            if (bUpdateMatrix)
            {
                m = Matrix.CreateTranslation((1366 / 2 - 32 - mPos.X) * (1f / mScale), (768 / 2 - 32 - mPos.Y) * (1f / mScale), 1) * Matrix.CreateScale(mScale, mScale, 1f);
                bUpdateMatrix = false;
            }

        }

        public TalentGrid(params TalentNode[] nodes)
        {
            bUpdateMatrix = true;
            hoverOverNode = null;
            this.nodes = nodes.ToList();
            if (this.nodes.Count > 1)
            {
                for (int i = 0; i != this.nodes.Count; i++)
                {

                    if (this.nodes[i].parent.requiredTalentIDs.Count != 0)
                    {
                        this.nodes[i].nc = new List<NodeConnection>();
                        foreach (var item in this.nodes[i].parent.requiredTalentIDs)
                        {
                            var temp = this.nodes[i].parent.parentCCC.baseTalentSlot.Find(ts => ts.ID == item);

                            if (temp != null && temp.talentNode != null)
                            {
                                this.nodes[i].nc.Add(new NodeConnection(temp.talentNode, this.nodes[i]));
                            }

                        }
                    }

                }
            }
            gridRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, 1366, 768);
        }

        public TalentGrid(Utilities.Characters.BaseCharacter bc) : this(bc.CCC.getTalentNodesForGrid())
        {
            parentBC = bc;
        }

        public void GenerateRenderEditor(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(gridRender);
            sb.GraphicsDevice.Clear(Color.Black);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, m);

            foreach (var item in nodes)
            {

                item.DrawGrid(sb);

            }

            foreach (var item in nodes)
            {

                item.DrawNode(sb);
                TextUtility.DrawString(sb, Game1.defaultFont, item.ToString(), item.nodePos.ToVector2() * 64 + item.nodePos.ToVector2() * 32, new Vector2(1), Color.White, Color.Black);
            }



            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
        }

        public void GenerateRender(SpriteBatch sb)
        {
            sb.End();
            sb.GraphicsDevice.SetRenderTarget(gridRender);
            sb.GraphicsDevice.Clear(Color.Black);
            sb.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, m);

            if (hoverOverNode == null)
            {
                foreach (var item in nodes)
                {
                    if (item.parent.bUnlocked || item.parent.CanUnlock(parentBC))
                    {
                    }
                    else
                    {
                        item.DrawGrid(sb, Color.Gray);
                    }


                }

                foreach (var item in nodes)
                {
                    if (item.parent.bUnlocked || item.parent.CanUnlock(parentBC))
                    {
                        item.DrawGrid(sb, Color.Gold);
                    }
                }


                foreach (var item in nodes)
                {
                    if (item.parent.bUnlocked)
                    {
                        item.DrawNode(sb, Color.White);
                    }
                    else
                    {
                        item.DrawNode(sb, Color.White * .75f);
                    }
                }
            }
            else
            {
                var nodesThatRequireSelected = nodes.FindAll(n => n.parent.requiredTalents.Contains(hoverOverNode.parent));
                var nodesThatSelectedRequired = hoverOverNode.parent.requiredTalents.Select(t => t.talentNode).ToList();
                nodesThatRequireSelected.AddRange(nodesThatSelectedRequired);
                nodesThatRequireSelected.Add(hoverOverNode);

                float nonPathTransparancy = .35f;

                foreach (var item in nodes.Except(nodesThatRequireSelected))
                {
                    if (item.parent.bUnlocked || item.parent.CanUnlock(parentBC))
                    {
                    }
                    else
                    {
                        item.DrawGrid(sb, Color.Gray * nonPathTransparancy);
                    }


                }

                foreach (var item in nodes)
                {
                    if (item.parent.bUnlocked || item.parent.CanUnlock(parentBC))
                    {
                        item.DrawGrid(sb, Color.Gold * nonPathTransparancy);
                    }
                }


                foreach (var item in nodes)
                {
                    if (item.parent.bUnlocked)
                    {
                        item.DrawNode(sb, Color.White * nonPathTransparancy);
                    }
                    else
                    {
                        item.DrawNode(sb, Color.White * nonPathTransparancy);
                    }
                }

                foreach (var item in nodesThatRequireSelected)
                {
                    {
                        item.DrawGrid(sb, Color.Gray * nonPathTransparancy);
                    }
                }




                foreach (var item in nodesThatRequireSelected)
                {
                    if (item.parent.bUnlocked || item.parent.CanUnlock(parentBC))
                    {
                    }
                    else
                    {
                        item.DrawGrid(sb, hoverOverNode, item, Color.Gray);
                    }


                }

                foreach (var item in nodesThatRequireSelected)
                {
                    if (item.parent.bUnlocked || item.parent.CanUnlock(parentBC))
                    {
                        item.DrawGrid(sb, hoverOverNode, item, Color.Gold);
                    }
                }


                foreach (var item in nodesThatRequireSelected)
                {
                    if (item.parent.bUnlocked)
                    {
                        item.DrawNode(sb, Color.White);
                    }
                    else
                    {
                        item.DrawNode(sb, Color.White * .75f);
                    }
                }
            }




            sb.End();
            sb.GraphicsDevice.SetRenderTarget(null);
        }

        public RenderTarget2D getRender()
        {
            return gridRender;
        }

        public void Close()
        {
            gridRender.Dispose();
            mScale = 1f;
            mPos = new Point(0);
        }

        public bool NodeContainsMouse(Rectangle container)
        {
            if (!container.Contains(KeyboardMouseUtility.uiMousePos))
            {
                hoverOverNode = null;
                return false;
            }

            Point p = Utilities.KeyboardMouseUtility.uiMousePos;
            p += mPos;
            p -= new Point(1366 / 2 - 32, 768 / 2 - 32);

            foreach (var item in nodes)
            {
                if (item.pos.Contains(p))
                {
                    hoverOverNode = item;
                    return true;
                }
            }

            hoverOverNode = null;
            return false;
        }

        internal void SelectRight()
        {
            if (hoverOverNode == null)
            {
                SelectMostCenteredNode();
                return;
            }

            var tempList = new List<TalentNode>(nodes);
            tempList.OrderBy(n => n.nodePos.X);
            var node = tempList.Find(n => n.nodePos.X > hoverOverNode.nodePos.X && hoverOverNode.nodePos.Y == n.nodePos.Y);
            if (node != null)
            {
                hoverOverNode = node;
                mPos = hoverOverNode.pos.Location - new Point(32);
                bUpdateMatrix = true;
                return;
            }
            node = tempList.Find(n => n.nodePos.X > hoverOverNode.nodePos.X && Math.Abs(Math.Abs(hoverOverNode.nodePos.Y) - Math.Abs(n.nodePos.Y)) == 1);
            if (node != null)
            {
                hoverOverNode = node;
                mPos = hoverOverNode.pos.Location - new Point(32);
                bUpdateMatrix = true;
                return;
            }
        }

        internal void SelectUp()
        {
            if (hoverOverNode == null)
            {
                SelectMostCenteredNode();
                return;
            }

            var tempList = new List<TalentNode>(nodes);
            tempList.OrderBy(n => n.nodePos.Y);
            var node = tempList.Find(n => n.nodePos.Y > hoverOverNode.nodePos.Y && hoverOverNode.nodePos.X == n.nodePos.X);
            if (node != null)
            {
                hoverOverNode = node;
                mPos = hoverOverNode.pos.Location - new Point(32); bUpdateMatrix = true;
                return;
            }
            node = tempList.Find(n => n.nodePos.Y > hoverOverNode.nodePos.Y && Math.Abs(Math.Abs(hoverOverNode.nodePos.X) - Math.Abs(n.nodePos.X)) == 1);
            if (node != null)
            {
                hoverOverNode = node;
                mPos = hoverOverNode.pos.Location - new Point(32); bUpdateMatrix = true;
                return;
            }
        }

        internal void SelectLeft()
        {
            if (hoverOverNode == null)
            {
                SelectMostCenteredNode();
                return;
            }

            var tempList = new List<TalentNode>(nodes);
            tempList.OrderBy(n => n.nodePos.X);
            var node = tempList.Find(n => n.nodePos.X < hoverOverNode.nodePos.X && hoverOverNode.nodePos.Y == n.nodePos.Y);
            if (node != null)
            {
                hoverOverNode = node;
                mPos = hoverOverNode.pos.Location - new Point(32); bUpdateMatrix = true;
                return;
            }
            node = tempList.Find(n => n.nodePos.X < hoverOverNode.nodePos.X && Math.Abs(Math.Abs(hoverOverNode.nodePos.Y) - Math.Abs(n.nodePos.Y)) == 1);
            if (node != null)
            {
                hoverOverNode = node;
                mPos = hoverOverNode.pos.Location - new Point(32); bUpdateMatrix = true;
                return;
            }
        }

        internal void SelectDown()
        {
            if (hoverOverNode == null)
            {
                SelectMostCenteredNode();
                return;
            }

            var tempList = new List<TalentNode>(nodes);
            tempList.OrderBy(n => n.nodePos.Y);
            var node = tempList.Find(n => n.nodePos.Y < hoverOverNode.nodePos.Y && hoverOverNode.nodePos.X == n.nodePos.X);
            if (node != null)
            {
                hoverOverNode = node;
                mPos = hoverOverNode.pos.Location - new Point(32); bUpdateMatrix = true;
                return;
            }
            node = tempList.Find(n => n.nodePos.Y < hoverOverNode.nodePos.Y && Math.Abs(Math.Abs(hoverOverNode.nodePos.X) - Math.Abs(n.nodePos.X)) == 1);
            if (node != null)
            {
                hoverOverNode = node;
                mPos = hoverOverNode.pos.Location - new Point(32); bUpdateMatrix = true;
                return;
            }


        }

        private void SelectMostCenteredNode()
        {
            Point middle = new Point((mPos.X), (mPos.Y));
            var node = nodes.Find(n => n.pos.Contains(middle));
            if (node != null)
            {
                hoverOverNode = node;
                return;
            }

            Point distance = new Point(0);
            KeyValuePair<Point, int> closestNodeAndIndex = new KeyValuePair<Point, int>(new Point(0), -1);
            for (int i = 0; i < nodes.Count; i++)
            {
                var delta = new Point(Math.Abs(Math.Abs(nodes[i].pos.X) - Math.Abs(middle.X)), Math.Abs(nodes[i].pos.Y) - Math.Abs(middle.Y));
                if (closestNodeAndIndex.Value == -1 || (Math.Abs(closestNodeAndIndex.Key.X) + Math.Abs(closestNodeAndIndex.Key.Y)) > Math.Abs(delta.X) + Math.Abs(delta.Y))
                {
                    closestNodeAndIndex = new KeyValuePair<Point, int>(delta, i);
                }
            }

            if (closestNodeAndIndex.Value != -1)
            {
                hoverOverNode = nodes[closestNodeAndIndex.Value];
            }
            else if (nodes.Count != 0)
            {
                hoverOverNode = nodes[0];
            }

            mPos = hoverOverNode.pos.Location - new Point(32); bUpdateMatrix = true;
        }
    }
}
