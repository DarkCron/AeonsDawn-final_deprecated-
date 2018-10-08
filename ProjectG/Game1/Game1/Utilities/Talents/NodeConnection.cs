using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TBAGW
{

    public class NodeConnection
    {
        enum Connection { H, V, DR, DL, UR, UL }
        Rectangle[] sources = { new Rectangle(64, 64, 64, 64), new Rectangle(128, 0, 64, 64), new Rectangle(0, 0, 64, 64), new Rectangle(64, 0, 64, 64), new Rectangle(0, 64, 64, 64), new Rectangle(128, 64, 64, 64) };

        internal TalentNode[] connection = new TalentNode[2];
        Point delta = new Point();
        List<KeyValuePair<Rectangle, Connection>> connectionPathPos = new List<KeyValuePair<Rectangle, Connection>>();

        static Texture2D sourceTex;
        static bool bInitialize = true;


        static void Initialize()
        {
            bInitialize = false;
            sourceTex = Game1.contentManager.Load<Texture2D>(@"Graphics\GUI\NodeConnection");
        }



        public NodeConnection(TalentNode n1, TalentNode n2)
        {
            if (bInitialize) { Initialize(); }

            connection[0] = n1;
            connection[1] = n2;
            delta = n2.nodePos - n1.nodePos;

            bool bSameCollumnNeighBours = n1.nodePos.X == n2.nodePos.X && Math.Abs(n2.nodePos.Y - n1.nodePos.Y) == 1;
            bool bSameRowNeighbours = n1.nodePos.Y == n2.nodePos.Y && Math.Abs(n2.nodePos.X - n1.nodePos.X) == 1;

            if (bSameCollumnNeighBours)
            {
                Rectangle pos = new Rectangle(n1.nodePos.X * TalentNode.nodeSize, delta.Y * TalentNode.nodeSpace + n1.nodePos.Y * TalentNode.nodeSpace, TalentNode.nodeSize, TalentNode.nodeSize);
                connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                goto end;
            }

            if (bSameRowNeighbours)
            {
                Rectangle pos = new Rectangle(delta.X * TalentNode.nodeSpace + n1.nodePos.X * TalentNode.nodeSize, n1.nodePos.Y * TalentNode.nodeSpace, TalentNode.nodeSize, TalentNode.nodeSize);
                connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                goto end;
            }

            if (delta.X == 0)
            {
                if (delta.Y % 2 == 0)
                {
                    Rectangle pos = new Rectangle(delta.X * TalentNode.nodeSpace - 1 * TalentNode.nodeSize, n1.nodePos.Y * TalentNode.nodeSpace, TalentNode.nodeSize, TalentNode.nodeSize);
                    pos.Location += new Point(TalentNode.nodeSpace / 2, 0);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, delta.Y > 0 ? Connection.DR : Connection.UR));

                    int y = 1;
                    if (delta.Y < 0)
                    {
                        y = -1;
                    }

                    for (int i = 0; i < Math.Abs(delta.Y); i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y + y * TalentNode.nodeSize, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                    }

                    for (int i = 0; i < (Math.Abs(delta.Y) / 2) - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y + y * TalentNode.nodeSize, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                    }

                    pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y + y * TalentNode.nodeSize, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, delta.Y > 0 ? Connection.UR : Connection.DR));
                    goto end;
                }
                else
                {
                    Rectangle pos = new Rectangle(delta.X * TalentNode.nodeSpace + 1 * TalentNode.nodeSize, n1.nodePos.Y * TalentNode.nodeSpace, TalentNode.nodeSize, TalentNode.nodeSize);
                    pos.Location += new Point(-TalentNode.nodeSpace / 2, 0);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, delta.Y > 0 ? Connection.DL : Connection.UL));

                    int y = 1;
                    if (delta.Y < 0)
                    {
                        y = -1;
                    }

                    for (int i = 0; i < Math.Abs(delta.Y); i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y + y * TalentNode.nodeSize, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));

                    }

                    for (int i = 0; i < (Math.Abs(delta.Y) / 2) - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y + y * TalentNode.nodeSize, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                    }

                    pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y + y * TalentNode.nodeSpace, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));

                    pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y + y * TalentNode.nodeSize, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, delta.Y > 0 ? Connection.UL : Connection.DL));
                    goto end;

                }
            }
            else if (delta.Y == 0)
            {
                if (delta.X % 2 == 0)
                {
                    Rectangle pos = new Rectangle(delta.X * TalentNode.nodeSpace * 0, n1.nodePos.Y * TalentNode.nodeSpace, TalentNode.nodeSize, TalentNode.nodeSize);
                    pos.Location += new Point(0, -TalentNode.nodeSize + TalentNode.nodeSpace / 2);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, delta.X > 0 ? Connection.DR : Connection.DL));

                    int x = 1;
                    if (delta.X < 0)
                    {
                        x = -1;
                    }

                    for (int i = 0; i < Math.Abs(delta.X); i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                    }

                    for (int i = 0; i < (Math.Abs(delta.X) / 2) - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                    }

                    pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, delta.X > 0 ? Connection.DL : Connection.DR));
                    goto end;
                }
                else
                {
                    Rectangle pos = new Rectangle((delta.X * 0) * TalentNode.nodeSpace, n1.nodePos.Y * TalentNode.nodeSpace, TalentNode.nodeSize, TalentNode.nodeSize);
                    pos.Location += new Point(0, TalentNode.nodeSize - TalentNode.nodeSpace / 2);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, delta.X > 0 ? Connection.UR : Connection.UL));

                    int x = 1;
                    if (delta.X < 0)
                    {
                        x = -1;
                    }

                    for (int i = 0; i < Math.Abs(delta.X); i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));

                    }

                    for (int i = 0; i < (Math.Abs(delta.X) / 2) - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                    }

                    pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSpace, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));

                    pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, delta.X > 0 ? Connection.UL : Connection.UR));

                    goto end;
                }
            }

            bool bLongerWidth = Math.Abs(delta.X) >= Math.Abs(delta.Y);

            if (bLongerWidth || true)
            {
                bool bGoingUp = delta.Y < 0;

                Rectangle pos = new Rectangle(delta.X * TalentNode.nodeSpace * 0, n1.nodePos.Y * TalentNode.nodeSpace, TalentNode.nodeSize, TalentNode.nodeSize);
                pos.Location += new Point(0, -TalentNode.nodeSize + TalentNode.nodeSpace / 2);
                connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, delta.X > 0 ? Connection.DR : Connection.DL));

                int intW = Math.Abs(delta.X) / 2;
                int remainderWidth = Math.Abs(delta.X - (delta.X / 2));
                int x = 1;
                if (delta.X < 0)
                {
                    x = -1;
                }


                for (int i = 0; i < intW; i++)
                {
                    pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                }

                for (int i = 0; i < intW; i++)
                {
                    pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize / 2, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                }





                if (bGoingUp)
                {
                    int y = 1;
                    if (delta.Y > 0)
                    {
                        y = -1;
                    }

                    if (Math.Abs(delta.Y) != 1)
                    {
                        //if (Math.Abs(delta.X) != 1)
                        //{
                        //    pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        //}
                        //else
                        {
                            pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize * 3 / 4, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        }

                        if (delta.X < 0)
                        {
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.UR));
                        }
                        else
                        {
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.UL));
                        }
                    }


                    for (int i = 0; i < Math.Abs(delta.Y) - 2; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * TalentNode.nodeSize, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                    }


                    for (int i = 0; i < (Math.Abs(delta.Y) / 2) - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * TalentNode.nodeSize, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                    }

                    if (Math.Abs(delta.Y) != 1)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * (TalentNode.nodeSize / 2 + TalentNode.nodeSpace / 2), connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));

                        if (delta.Y % 2 != 0)
                        {
                            pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * (TalentNode.nodeSpace + TalentNode.nodeSpace / 2), connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                        }
                        else
                        {
                            pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * (TalentNode.nodeSpace / 2), connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                        }

                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * TalentNode.nodeSpace, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);

                        if (delta.X < 0)
                        {
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.DL));
                        }
                        else
                        {
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.DR));
                        }
                    }
                    else
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * (TalentNode.nodeSize) * 3 / 4, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                    }






                    for (int i = 0; i < remainderWidth - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                    }

                    for (int i = 0; i < remainderWidth - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize / 2, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                    }



                    //if (Math.Abs(delta.X) != 1)
                    //{
                    //    pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize / 2, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    //}
                    //else
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize * 3 / 4, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    }

                    if (delta.X < 0)
                    {
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.UR));
                    }
                    else
                    {
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.UL));
                    }
                }
                else
                {
                    int y = -1;

                    if (delta.Y != -1)
                    {
                        //if (Math.Abs(delta.X) != 1)
                        //{
                        //    pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        //}
                        //else
                        {
                            pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize * 3 / 4, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        }

                        if (delta.X < 0)
                        {
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.DR));
                        }
                        else
                        {
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.DL));
                        }
                    }


                    for (int i = 0; i < Math.Abs(delta.Y) - 2 + 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * TalentNode.nodeSize, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                    }


                    for (int i = 0; i < (Math.Abs(delta.Y)) - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * TalentNode.nodeSize / 2, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                    }

                    if (Math.Abs(delta.Y) != -1)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * (TalentNode.nodeSize / 2 + TalentNode.nodeSpace / 2), connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));

                        if (Math.Abs(delta.Y) != 1)
                        {
                            if (delta.Y % 2 != 0)
                            {
                                pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * (TalentNode.nodeSpace + TalentNode.nodeSpace / 2), connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                                connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                            }
                            else
                            {
                                pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * (TalentNode.nodeSpace / 2), connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                                connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                            }
                        }

                        if (Math.Abs(delta.Y) == 1)
                        {
                            pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * (TalentNode.nodeSize) * 1 / 4, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.V));
                        }

                        pos = new Rectangle(connectionPathPos.Last().Key.X, connectionPathPos.Last().Key.Y - y * TalentNode.nodeSpace, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);

                        if (delta.X < 0)
                        {
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.UL));
                        }
                        else
                        {
                            connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.UR));
                        }
                    }
                    else
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * (TalentNode.nodeSize), connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                    }






                    for (int i = 0; i < remainderWidth - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                    }

                    for (int i = 0; i < remainderWidth - 1; i++)
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize / 2, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.H));
                    }



                    //if (Math.Abs(delta.X) != 1)
                    //{
                    //    pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize / 2, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    //}
                    //else
                    {
                        pos = new Rectangle(connectionPathPos.Last().Key.X + x * TalentNode.nodeSize * 3 / 4, connectionPathPos.Last().Key.Y, connectionPathPos.Last().Key.Width, connectionPathPos.Last().Key.Height);
                    }

                    if (delta.X < 0)
                    {
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.DR));
                    }
                    else
                    {
                        connectionPathPos.Add(new KeyValuePair<Rectangle, Connection>(pos, Connection.DL));
                    }
                }

            }
            else
            {
                bool bGoingRight = delta.X > 0;
            }

        end: { }

            for (int i = 0; i < connectionPathPos.Count; i++)
            {
                var r = connectionPathPos[i].Key;
                connectionPathPos[i] = new KeyValuePair<Rectangle, Connection>(new Rectangle(r.X + (n1.nodePos.X) * 64 + (n1.nodePos.X) * 32, r.Y + n1.nodePos.Y * 64, r.Width, r.Height), connectionPathPos[i].Value);
            }
        }

        internal void Draw(SpriteBatch sb,Color dc)
        {
            foreach (var item in connectionPathPos)
            {
                sb.Draw(sourceTex, item.Key, sources[(int)item.Value], dc);
            }
        }
    }
}
