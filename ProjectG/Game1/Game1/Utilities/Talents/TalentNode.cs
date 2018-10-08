using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace TBAGW
{
    [XmlRoot("Talent Node")]
    public class TalentNode
    {
        [XmlElement("Node pos")]
        public Point nodePos = new Point();
        [XmlElement("Nodes")]
        public List<TalentNode> required = new List<TalentNode>();
        [XmlElement("Acquired Node")]
        public bool bAcquired = false;

        internal TexPanel tp;
        static TexPanel source;
        internal List<NodeConnection> nc = null;
        internal static int nodeSize = 64;
        internal static int nodeSpace = 32;
        internal Rectangle pos;
        internal BaseTalentSlot parent;

        static bool bInitialize = true;

        public TalentNode() { }

        static void Initialize()
        {
            bInitialize = false;

            Texture2D guiTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\Inventory_sheet_4096x4096");
            source = new TexPanel(guiTex, new Rectangle(0, 0, 64, 64), new Rectangle(90, 679, 64, 2), new Rectangle(90, 745, 64, 2), new Rectangle(88, 681, 2, 64), new Rectangle(154, 681, 2, 64), new Rectangle(88, 679, 2, 2), new Rectangle(154, 679, 2, 2), new Rectangle(88, 745, 2, 2), new Rectangle(154, 745, 2, 2), new Rectangle(90, 681, 64, 64));

        }

        public TalentNode(Point p)
        {
            if (bInitialize) { Initialize(); }
            nodePos = p;
            pos = new Rectangle(new Point(nodePos.X * nodeSize + nodePos.X * nodeSpace, nodePos.Y * nodeSize + nodePos.Y * nodeSpace), new Point(nodeSize));
            tp = source.positionCopy(pos);
        }

        internal void DrawGrid(SpriteBatch sb, Color c = default(Color))
        {
            if (nc != null)
            {
                Color dc = Color.Gold;

                if (c != default(Color))
                {
                    dc = c;
                }

                foreach (var item in nc)
                {
                    item.Draw(sb, dc);
                }

            }

        }


        internal void DrawGrid(SpriteBatch sb, TalentNode n1, TalentNode n2, Color c = default(Color))
        {
            if (nc != null)
            {
                Color dc = Color.Gold;

                if (c != default(Color))
                {
                    dc = c;
                }

                foreach (var item in nc)
                {
                    if (item.connection.Contains(n1)&&item.connection.Contains(n2))
                    {
                        item.Draw(sb, dc);
                    }
                }

            }

        }

        internal void DrawNode(SpriteBatch sb, Color c = default(Color))
        {
            if (pos == default(Rectangle))
            {
                if (bInitialize) { Initialize(); }
                pos = new Rectangle(new Point(nodePos.X * nodeSize + nodePos.X * nodeSpace, nodePos.Y * nodeSize + nodePos.Y * nodeSpace), new Point(nodeSize));
                tp = source.positionCopy(pos);

            }
            Color dc = Color.White;

            if (c != default(Color))
            {
                dc = c;
            }

            tp.Draw(sb, dc);

        }

        public override string ToString()
        {
            return parent == null ? parent.name.getText() + " " + nodePos : " " + nodePos;
        }
    }
}
