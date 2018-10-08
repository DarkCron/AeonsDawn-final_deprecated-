using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.AI;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    static public class MapListUtility
    {
        static public List<BasicTile> returnMapRadius(int r, List<BasicTile> bt, BaseSprite bs)
        {
            Point loc = ((bs.position / 64).ToPoint().ToVector2() * 64).ToPoint();
            List<Rectangle> temp = new List<Rectangle>();
            List<BasicTile> tempTiles = new List<BasicTile>();

            for (int i = 0; i < r + 1; i++)
            {
                for (int j = -(r - i); j < (r - i) + 1; j++)
                {
                    temp.Add(new Rectangle(loc.X + i * 64, loc.Y + j * 64, 64, 64));
                }
            }

            for (int i = -r; i != 0; i++)
            {
                for (int j = -(i + r); j < i + r + 1; j++)
                {
                    temp.Add(new Rectangle(loc.X + i * 64, loc.Y + j * 64, 64, 64));
                }
            }

            foreach (var item in temp)
            {
                BasicTile tempTile = bt.Find(t => t.mapPosition == item);
                if (tempTile != null)
                {
                    tempTiles.Add(tempTile);
                }
            }

            return tempTiles;
        }

        static public List<BasicTile> returnMapRadius(int r, int ro, List<BasicTile> bt, BaseSprite bs)
        {
            if (r == 0)
            {
                return returnMapRadius(ro, bt, bs); ;
            }
            List<BasicTile> temp_1 = returnMapRadius(r, bt, bs);
            List<BasicTile> temp_2 = returnMapRadius(ro, bt, bs);
            temp_2.RemoveAll(t => temp_1.Contains(t));

            return temp_2;
        }

        static public List<BasicTile> returnMapRadius(int r, List<BasicTile> bt, Vector2 bs)
        {
            Point loc = ((bs / 64).ToPoint().ToVector2() * 64).ToPoint();
            List<Rectangle> temp = new List<Rectangle>();
            List<BasicTile> tempTiles = new List<BasicTile>();

            for (int i = 0; i < r + 1; i++)
            {
                for (int j = -(r - i); j < (r - i) + 1; j++)
                {
                    temp.Add(new Rectangle(loc.X + i * 64, loc.Y + j * 64, 64, 64));
                }
            }

            for (int i = -r; i != 0; i++)
            {
                for (int j = -(i + r); j < i + r + 1; j++)
                {
                    temp.Add(new Rectangle(loc.X + i * 64, loc.Y + j * 64, 64, 64));
                }
            }

            foreach (var item in temp)
            {
                BasicTile tempTile = bt.Find(t => t.mapPosition == item);
                if (tempTile != null)
                {
                    tempTiles.Add(tempTile);
                }
            }

            return tempTiles;
        }

        static public List<BasicTile> returnMapRadius(int r, int ro, List<BasicTile> bt, Vector2 bs)
        {
            if (r == 0)
            {
                return returnMapRadius(ro, bt, bs); ;
            }
            List<BasicTile> temp_1 = returnMapRadius(r, bt, bs);
            List<BasicTile> temp_2 = returnMapRadius(ro, bt, bs);
            temp_2.RemoveAll(t => temp_1.Contains(t));

            return temp_2;
        }

        static public List<BasicTile> returnValidMapRadius(int r1, int r2, List<BasicTile> bt, Vector2 bs)
        {
            List<BasicTile> temp1 = returnMapRadius(r2, bt, bs);
            if (r1 > 1)
            {
                List<BasicTile> temp2 = returnMapRadius(r1 - 1, bt, bs);
                temp1.RemoveAll(t => temp2.Contains(t) == true);
            }
            return temp1;
        }

        static public List<BasicTile> returnValidMapRadius(int r, List<BasicTile> bt, Vector2 bs)
        {
            Point loc = ((bs / 64).ToPoint().ToVector2() * 64).ToPoint();
            List<Rectangle> temp = new List<Rectangle>();
            List<BasicTile> tempTiles = new List<BasicTile>();

            for (int i = 0; i < r + 1; i++)
            {
                for (int j = -(r - i); j < (r - i) + 1; j++)
                {
                    temp.Add(new Rectangle(loc.X + i * 64, loc.Y + j * 64, 64, 64));
                }
            }

            for (int i = -r; i != 0; i++)
            {
                for (int j = -(i + r); j < i + r + 1; j++)
                {
                    temp.Add(new Rectangle(loc.X + i * 64, loc.Y + j * 64, 64, 64));
                }
            }

            foreach (var item in temp)
            {
                BasicTile tempTile = bt.Find(t => t.mapPosition == item);
                if (tempTile != null)
                {
                    tempTiles.Add(tempTile);
                }
            }

            List<SpotNode> openNodes = new List<SpotNode>();
            List<SpotNode> closedNodes = new List<SpotNode>();
            SpotNode startNode = new SpotNode(0, ((bs / 64).ToPoint().ToVector2() * 64).ToPoint());
            SpotNode selectedNode;
            openNodes.AddRange(returnURDL(openNodes, closedNodes, tempTiles, startNode));
            while (openNodes.Count != 0)
            {
                for (int i = 1; i < r + 1; i++)
                {
                    selectedNode = openNodes.Find(on => on.NodeCost == i);
                    if (selectedNode != null)
                    {
                        openNodes.Remove(selectedNode);
                        closedNodes.Add(selectedNode);
                        if (i != r)
                        {

                            openNodes.AddRange(returnURDL(openNodes, closedNodes, tempTiles, selectedNode));
                        }
                        break;
                    }
                }
            }
            CombatProcessor.test = closedNodes;
            List<BasicTile> toDeleteTiles = new List<BasicTile>();
            foreach (var tile in tempTiles)
            {
                bool bCanReach = false;
                foreach (var cn in closedNodes)
                {
                    if (tile.mapPosition.Location == cn.nodeLoc)
                    {
                        bCanReach = true;
                    }

                }

                if (!bCanReach)
                {
                    toDeleteTiles.Add(tile);
                }
            }

            foreach (var item in toDeleteTiles)
            {
                tempTiles.Remove(item);
            }

            CombatProcessor.radiusTiles = tempTiles;

            return tempTiles;
        }

        static public List<BasicTile> returnValidMapRadius2(int r, List<BasicTile> bt, Vector2 bs)
        {


            Point loc = ((bs / 64).ToPoint().ToVector2() * 64).ToPoint();
            List<Rectangle> temp = new List<Rectangle>();
            List<BasicTile> tempTiles = new List<BasicTile>();

            for (int i = 0; i < r + 1; i++)
            {
                for (int j = -(r - i); j < (r - i) + 1; j++)
                {
                    temp.Add(new Rectangle(loc.X + i * 64, loc.Y + j * 64, 64, 64));
                }
            }

            for (int i = -r; i != 0; i++)
            {
                for (int j = -(i + r); j < i + r + 1; j++)
                {
                    temp.Add(new Rectangle(loc.X + i * 64, loc.Y + j * 64, 64, 64));
                }
            }

            foreach (var item in temp)
            {
                BasicTile tempTile = bt.Find(t => t.mapPosition == item);
                if (tempTile != null)
                {
                    tempTiles.Add(tempTile);
                }
            }

            List<SpotNode> openNodes = new List<SpotNode>();
            List<SpotNode> closedNodes = new List<SpotNode>();
            SpotNode startNode = new SpotNode(0, ((bs / 64).ToPoint().ToVector2() * 64).ToPoint());
            SpotNode selectedNode;
            openNodes.AddRange(returnURDL(openNodes, closedNodes, tempTiles, startNode));
            while (openNodes.Count != 0)
            {
                for (int i = 1; i < r + 1; i++)
                {
                    selectedNode = openNodes.Find(on => on.NodeCost == i);
                    if (selectedNode != null)
                    {
                        openNodes.Remove(selectedNode);
                        closedNodes.Add(selectedNode);
                        if (i != r)
                        {

                            openNodes.AddRange(returnURDL(openNodes, closedNodes, tempTiles, selectedNode));
                        }
                        break;
                    }
                }
            }

            List<BasicTile> toDeleteTiles = new List<BasicTile>();
            foreach (var tile in tempTiles)
            {
                bool bCanReach = false;
                foreach (var cn in closedNodes)
                {
                    if (tile.mapPosition.Location == cn.nodeLoc)
                    {
                        bCanReach = true;
                    }

                }

                if (!bCanReach)
                {
                    toDeleteTiles.Add(tile);
                }
            }

            foreach (var item in toDeleteTiles)
            {
                tempTiles.Remove(item);
            }

            return tempTiles;
        }

        static public List<SpotNode> returnURDL(List<SpotNode> openNodes, List<SpotNode> closedNodes, List<BasicTile> tiles, SpotNode selectedNode)
        {
            BasicTile tileAbove = tiles.Find(t => t.mapPosition.Location == new Point(selectedNode.nodeLoc.X, selectedNode.nodeLoc.Y - 64));
            BasicTile tileBelow = tiles.Find(t => t.mapPosition.Location == new Point(selectedNode.nodeLoc.X, selectedNode.nodeLoc.Y + 64));
            BasicTile tileLeft = tiles.Find(t => t.mapPosition.Location == new Point(selectedNode.nodeLoc.X - 64, selectedNode.nodeLoc.Y));
            BasicTile tileRight = tiles.Find(t => t.mapPosition.Location == new Point(selectedNode.nodeLoc.X + 64, selectedNode.nodeLoc.Y));
            List<SpotNode> temp = new List<SpotNode>();

            if (tileAbove != null)
            {

                temp.Add(new SpotNode(selectedNode.NodeCost + 1, new Point(selectedNode.nodeLoc.X, selectedNode.nodeLoc.Y - 64)));
                var temp1 = openNodes.Find(on => on.nodeLoc == tileAbove.mapPosition.Location);
                var temp2 = closedNodes.Find(on => on.nodeLoc == tileAbove.mapPosition.Location);
                if (temp1 != null || temp2 != null)
                {
                    temp.RemoveAt(temp.Count - 1);
                }
            }

            if (tileBelow != null)
            {
                temp.Add(new SpotNode(selectedNode.NodeCost + 1, new Point(selectedNode.nodeLoc.X, selectedNode.nodeLoc.Y + 64)));
                var temp1 = openNodes.Find(on => on.nodeLoc == tileBelow.mapPosition.Location);
                var temp2 = closedNodes.Find(on => on.nodeLoc == tileBelow.mapPosition.Location);
                if (temp1 != null || temp2 != null)
                {
                    temp.RemoveAt(temp.Count - 1);
                }
            }

            if (tileLeft != null)
            {
                temp.Add(new SpotNode(selectedNode.NodeCost + 1, new Point(selectedNode.nodeLoc.X - 64, selectedNode.nodeLoc.Y)));
                var temp1 = openNodes.Find(on => on.nodeLoc == tileLeft.mapPosition.Location);
                var temp2 = closedNodes.Find(on => on.nodeLoc == tileLeft.mapPosition.Location);
                if (temp1 != null || temp2 != null)
                {
                    temp.RemoveAt(temp.Count - 1);
                }
            }

            if (tileRight != null)
            {
                temp.Add(new SpotNode(selectedNode.NodeCost + 1, new Point(selectedNode.nodeLoc.X + 64, selectedNode.nodeLoc.Y)));
                var temp1 = openNodes.Find(on => on.nodeLoc == tileRight.mapPosition.Location);
                var temp2 = closedNodes.Find(on => on.nodeLoc == tileRight.mapPosition.Location);
                if (temp1 != null || temp2 != null)
                {
                    temp.RemoveAt(temp.Count - 1);
                }
            }

            return temp;
        }

        static public List<Node> convertSpotNodeToNode(List<SpotNode> snl)
        {
            List<Node> nl = new List<Node>();

            foreach (var item in snl)
            {
                Node n = new Node();
                n.coord = (item.nodeLoc.ToVector2() / 64).ToPoint();
                nl.Add(n);
            }

            return nl;
        }

        static public List<Node> findEqualNodesToTileList(List<Node> nodes, List<BasicTile> tiles)
        {
            List<Node> tempList = new List<Node>();
            foreach (var item in tiles)
            {
                Node n = new Node();
                n.coord = (item.mapPosition.Location.ToVector2() / 64).ToPoint();
                tempList.Add(n);
            }

            List<Node> returnList = new List<Node>();
            foreach (var item in nodes)
            {
                var n = tempList.Find(sn => sn.coord == item.coord);
                if (n != null)
                {
                    returnList.Add(n);
                }
            }

            if (returnList.Count > 1)
            {
                DeleteSoloNodes(returnList);
            }


            return returnList;
        }

        private static void DeleteSoloNodes(List<Node> returnList)
        {
            List<Node> toDeleteNodes = new List<Node>();
            foreach (var item in returnList)
            {
                var n = returnList.Find(node => node != item && (node.coord == item.coord + new Point(0, 1) ||
                node.coord == item.coord + new Point(0, -1) ||
                node.coord == item.coord + new Point(1, 0) ||
                node.coord == item.coord + new Point(-1, 0)));
                if (n == null)
                {
                    toDeleteNodes.Add(item);
                }
            }

            foreach (var item in toDeleteNodes)
            {
                returnList.Remove(item);
            }
        }

        static public List<BasicTile> returnZoneTilesWithoutCharacters(List<BasicTile> zt)
        {
            return zt;
        }

        static public bool ContainsCharacter(List<BasicTile> zt, BaseCharacter bc)
        {
            foreach (var item in zt)
            {
                if (item.mapPosition.Contains(bc.spriteGameSize) || item.mapPosition.Intersects(bc.spriteGameSize))
                {
                    return true;
                }
            }
            return false;
        }

        static public int distanceBetweenCharacters(BaseCharacter caster, BaseCharacter target)
        {
            Point cPos = (caster.position / 64).ToPoint();
            Point tPos = (target.position / 64).ToPoint();
            int xDiff = Math.Abs(cPos.X - tPos.X);
            int yDiff = Math.Abs(cPos.Y - tPos.Y);
            return xDiff + yDiff;
        }
    }


    public class SpotNode
    {
        public int NodeCost = 0;
        public Point nodeLoc = new Point();

        public SpotNode(int cost, Point loc)
        {
            NodeCost = cost;
            nodeLoc = loc;
        }
    }

    [Serializable]
    [XmlType(TypeName = "Serializable KeyValuePair, because why not, amiright?")]
    public struct KeyValuePair<K, V>
    {
        public K Key
        { get; set; }

        public V Value
        { get; set; }

        public KeyValuePair(K key, V value) {
            this.Key = key;
            this.Value = value;
        }
    }
}
