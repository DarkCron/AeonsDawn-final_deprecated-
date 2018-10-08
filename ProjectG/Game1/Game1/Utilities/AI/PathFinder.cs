using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.Sprite;

namespace TBAGW.AI
{
    static public class PathFinder
    {
        public enum Directions { Up = 0, Down, Left, Right }
        public struct Step
        {
            public int directionIndex;
            public int currentGridPosX;
            public int currentGridPosY;
            public Rectangle mapPosition;
            public List<int> positionsPossible;

            public Step(int directionIndex, int currentGridPosX, int currentGridPosY)
            {
                this.directionIndex = directionIndex;
                this.currentGridPosX = currentGridPosX;
                this.currentGridPosY = currentGridPosY;
                mapPosition = new Rectangle(currentGridPosX * 64, currentGridPosY * 64, 64, 64);
                positionsPossible = new List<int>();
            }

            public void assignGrid(int currentGridPosX, int currentGridPosY)
            {
                this.currentGridPosX = currentGridPosX;
                this.currentGridPosY = currentGridPosY;
                mapPosition = new Rectangle(currentGridPosX * 64, currentGridPosY * 64, 64, 64);
            }
        }
        public static List<Step> stepsCalculated = new List<Step>();
        public static bool bRestartSearch = false;
        public static int StepsPerUpdate = 5;
        public static bool bNextIteration = false;
        static int iteration = 0;
        public static Vector2 finish = new Vector2(0);

        const int GCost = 10;
        static public List<Node> NewPathSearch(Vector2 start, Vector2 finish, List<BasicTile> bt)
        {
            bt.FindAll(tile => tile.bMustReload).ForEach(tile => tile.Reload());
            var tempTileStart = bt.Find(tile => tile.mapPosition.Contains(start));
            var tempTileEnd = bt.Find(tile => tile.mapPosition.Contains(finish));

            return NewPathSearch(tempTileStart, tempTileEnd, bt);

        }

        static public List<Node> NewPathSearch(BasicTile tempTileStart, BasicTile tempTileEnd, List<BasicTile> bt)
        {
            if (tempTileStart != null && tempTileEnd != null && tempTileStart != tempTileEnd)
            {
                var tempList = GenerateNodeInfo(tempTileStart, tempTileEnd, bt);
                var tempNodeList = new List<Node>();
                if (tempList.Count == 0)
                {
                    return new List<Node>();
                }
                tempList.ForEach(n => tempNodeList.Add(new Node(n.tileRef.positionGrid.ToPoint())));
                return tempNodeList;
            }
            return new List<Node>();
        }

        private static List<Node> GetNearestClosedNodes(List<Node> closedList, List<Node> nodePath, Node testNode)
        {
            Node nodeAbove = closedList.Find(n => n.coord == new Point(testNode.coord.X, testNode.coord.Y - 1) && !nodePath.Contains(n));
            Node nodeBelow = closedList.Find(n => n.coord == new Point(testNode.coord.X, testNode.coord.Y + 1) && !nodePath.Contains(n));
            Node nodeLeft = closedList.Find(n => n.coord == new Point(testNode.coord.X - 1, testNode.coord.Y) && !nodePath.Contains(n));
            Node nodeRight = closedList.Find(n => n.coord == new Point(testNode.coord.X + 1, testNode.coord.Y) && !nodePath.Contains(n));

            List<Node> temp = new List<Node>();
            if (nodeAbove != null)
            {
                temp.Add(nodeAbove);
            }
            if (nodeBelow != null)
            {
                temp.Add(nodeBelow);
            }
            if (nodeLeft != null)
            {
                temp.Add(nodeLeft);
            }
            if (nodeRight != null)
            {
                temp.Add(nodeRight);
            }

            return temp;
        }

        private static bool FoundEnd(Node n)
        {
            if (n.totalHCost == 0)
            {


                return true;
            }
            return false;
        }

        private static List<Node> getPossibleNeighbourNode(List<Node> nodes, Node selectedNode, List<Node> closed, List<Node> open)
        {
            Node nodeAbove = nodes.Find(n => n.coord == new Point(selectedNode.coord.X, selectedNode.coord.Y - 1) && !closed.Contains(n) && !open.Contains(n));
            Node nodeBelow = nodes.Find(n => n.coord == new Point(selectedNode.coord.X, selectedNode.coord.Y + 1) && !closed.Contains(n) && !open.Contains(n));
            Node nodeLeft = nodes.Find(n => n.coord == new Point(selectedNode.coord.X - 1, selectedNode.coord.Y) && !closed.Contains(n) && !open.Contains(n));
            Node nodeRight = nodes.Find(n => n.coord == new Point(selectedNode.coord.X + 1, selectedNode.coord.Y) && !closed.Contains(n) && !open.Contains(n));

            List<Node> temp = new List<Node>();
            if (nodeAbove != null)
            {
                temp.Add(nodeAbove);
            }
            if (nodeBelow != null)
            {
                temp.Add(nodeBelow);
            }
            if (nodeLeft != null)
            {
                temp.Add(nodeLeft);
            }
            if (nodeRight != null)
            {
                temp.Add(nodeRight);
            }

            return temp;
        }

        internal static List<NewNode> GenerateNodeInfo(BasicTile startTile, BasicTile endTile, List<BasicTile> tileList)
        {
            bool bAtEnd = false;
            List<NewNode> AllNodes = new List<NewNode>();
            List<NewNode> OpenNodes = new List<NewNode>();
            List<NewNode> ClosedNodes = new List<NewNode>();
            foreach (var tile in tileList)
            {
                AllNodes.Add(new NewNode(tile.TileDistance(endTile), tile));
            }

            var startNode = AllNodes.Find(n => n.tileRef == startTile);
            startNode.AssignDelta(0, startNode);
            ClosedNodes.Add(startNode);

            var surroundNodes = AllNodes.Except(ClosedNodes).ToList().FindAll(n => n.tileRef.TileDistance(startNode.tileRef) == 1);
            surroundNodes.ForEach(n => n.AssignDelta(NewNode.deltaGCost4, startNode));
            OpenNodes.AddRange(surroundNodes);
            var cheapestGNode = OpenNodes.Find(n => n.FCost == surroundNodes.Min(gn => gn.FCost));
            if (OpenNodes.Count == 0 || cheapestGNode == null)
            {
                return new List<NewNode>(ClosedNodes);
            }
            if (endTile == cheapestGNode.tileRef)
            {
                ClosedNodes.Add(cheapestGNode);
                goto skip;
            }

            try
            {
                while (!bAtEnd)
                {
                    ClosedNodes.Add(cheapestGNode);
                    OpenNodes.Remove(cheapestGNode);
                    surroundNodes = AllNodes.Except(ClosedNodes).ToList().FindAll(n => n != cheapestGNode && n.tileRef.TileDistance(cheapestGNode.tileRef) == 1);
                    surroundNodes.Except(OpenNodes).ToList().ForEach(n => n.AssignDelta(NewNode.deltaGCost4, cheapestGNode));
                    var NodesToCheckForLowerGCost = surroundNodes.FindAll(sn => OpenNodes.Contains(sn));
                    var NodesToUpdateGCost = NodesToCheckForLowerGCost.FindAll(n => n.CanGCostBeLowerThanCurrent(NewNode.deltaGCost4, cheapestGNode));
                    NodesToUpdateGCost.ForEach(n => n.AssignDelta(NewNode.deltaGCost4, cheapestGNode));
                    OpenNodes.AddRange(surroundNodes.Except(OpenNodes).ToList());
                    cheapestGNode = OpenNodes.Find(n => n.FCost == OpenNodes.Min(gn => gn.FCost));
                    if (cheapestGNode.HCost == 1 || ClosedNodes.Last().HCost == 1) //AKA Node next to finish
                    {
                        bAtEnd = true;
                        if(cheapestGNode.HCost == 1)
                        {
                         ClosedNodes.Add(cheapestGNode);
                        }
                       
                        ClosedNodes.Add(AllNodes.Find(n => n.HCost == 0));
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: probable no path found, skipping...");
                Console.WriteLine("Nodes open: " + OpenNodes.Count + "Nodes close: " + ClosedNodes.Count);
                goto skip;
            }

            ClosedNodes.Reverse();
            List<NewNode> pathNodes = new List<NewNode>();
            int pathNodeIndex = 0;
            pathNodes.Add(ClosedNodes[pathNodeIndex]);
            pathNodeIndex++;
            while (pathNodes.Last() != startNode)
            {
                var tl = ClosedNodes.Except(ClosedNodes.GetRange(0, pathNodeIndex)).ToList().FindAll(n => n.tileRef.TileDistance(pathNodes.Last().tileRef) == 1);
                var t = tl.Find(ti => ti.FCost == tl.Min(t2 => t2.FCost));
                pathNodeIndex = ClosedNodes.IndexOf(t);
                pathNodes.Add(t);
            }
            pathNodes.Reverse();

            return pathNodes;
        skip: { }
            return new List<NewNode>(ClosedNodes);
        }


    }

    public class Node
    {
        public Point coord = new Point();
        public int totalGCost = 0;
        public int totalHCost = 0;
        public int totalFCost = 0;
        public Node parentNode;
        public BasicTile pos = new BasicTile();
        public static int count = 0;
        public int number = 0;

        public Node()
        {

        }

        public Node(Point p)
        {
            coord = p;
        }

        public Node(Point p, Point pEnd)
        {
            coord = p;
            totalHCost = (Math.Abs(pEnd.X - p.X) + Math.Abs(pEnd.Y - p.Y));
            totalGCost = totalHCost * 10;
            totalFCost = totalGCost + totalHCost;
            count++;
            number = count;
        }

        public int getFCost()
        {
            return totalGCost + totalHCost * 10;
        }

        public override string ToString()
        {
            return coord + " , cost:" + (totalGCost + totalHCost * 10).ToString();
        }

        public int distanceFrom(Node n)
        {
            int x1 = Math.Abs(coord.X);
            int x2 = Math.Abs(n.coord.X);
            int y1 = Math.Abs(coord.Y);
            int y2 = Math.Abs(n.coord.Y);

            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }
    }

    public class NewNode
    {
        internal int GCost = 0;
        internal int HCost = 0;
        internal int FCost = 0;
        internal BasicTile tileRef;

        internal static int deltaGCost4 = 10;
        internal static int deltaGCost8 = 14;

        internal NewNode(int HCost, BasicTile t)
        {
            GCost = 0;
            FCost = 0;
            this.HCost = HCost;
            tileRef = t;
        }

        internal void AssignDelta(int delta, NewNode node)
        {
            GCost = delta + node.GCost;
            FCost = HCost + GCost;
        }

        internal bool CanGCostBeLowerThanCurrent(int delta, NewNode neighbour)
        {
            if ((neighbour.GCost + delta) < GCost)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return "H: " + HCost + ", G: " + GCost + ", F: " + FCost;
        }
    }
}
