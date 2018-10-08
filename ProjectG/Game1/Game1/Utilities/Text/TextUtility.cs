using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW
{
    static public class TextUtility
    {
        public static String bestMatchStringForBox(String s, SpriteFont sf, Rectangle r)
        {
            while (s.IndexOf("\n") != -1)
            {
                s = s.Replace("\n", "");
            }

            String bestMatch = s;
            String temp = "";
            int posLastSpace = -1;

            if (sf.MeasureString(bestMatch).X > r.Width)
            {
                while (sf.MeasureString(bestMatch).X > r.Width)
                {
                    String processedPieceOfString = "";
                    foreach (char letter in bestMatch.ToCharArray())
                    {
                        processedPieceOfString += letter;


                        if (letter == ' ')
                        {
                            if (sf.MeasureString(processedPieceOfString).X > r.Width && posLastSpace != 0)
                            {
                                try
                                {
                                    temp += processedPieceOfString.Substring(0, posLastSpace) + "\n";

                                    bestMatch = bestMatch.Replace(processedPieceOfString.Substring(0, posLastSpace), "");
                                    posLastSpace = -1;

                                    break;
                                }
                                catch
                                {
                                    temp += processedPieceOfString.Substring(0, posLastSpace);
                                    //bestMatch = bestMatch.Replace(processedPieceOfString.Substring(0, posLastSpace) + " ", "");
                                    posLastSpace = -1;
                                    goto doThis;
                                }
                            }
                            else if (sf.MeasureString(processedPieceOfString).X < r.Width && !(processedPieceOfString.Equals(bestMatch)) || (sf.MeasureString(processedPieceOfString).X > r.Width && posLastSpace == 0))
                            {
                                posLastSpace = processedPieceOfString.Length - 1;

                            }
                        }
                        else if (processedPieceOfString.Equals(bestMatch))
                        {
                            temp += processedPieceOfString;
                            goto doThis;
                        }
                    }

                }
                temp += bestMatch;
            }
            else
            {
                temp = bestMatch;
            }


        doThis:
            {

            }

            String lastLine = temp.Substring(temp.LastIndexOf("\n") + 1);
            if (sf.MeasureString(lastLine).X > r.Width)
            {
                temp = temp.Insert(temp.LastIndexOf(" ") + 1, "\n");
            }

            while (temp.Contains("\n "))
            {
                temp = temp.Replace("\n ", "\n");
            }
            return temp;
        }

        public static String bestMatchStringForScaledBox(String s, SpriteFont sf, Rectangle r, float scale)
        {
            while (s.IndexOf("\n") != -1)
            {
                s = s.Replace("\n", "");
            }

            String bestMatch = s;
            String temp = "";
            int posLastSpace = -1;

            if (sf.MeasureString(bestMatch).X * scale > r.Width)
            {
                while (sf.MeasureString(bestMatch).X * scale > r.Width)
                {
                    String processedPieceOfString = "";
                    foreach (char letter in bestMatch.ToCharArray())
                    {
                        processedPieceOfString += letter;

                        if (letter == ' ')
                        {
                            if (sf.MeasureString(processedPieceOfString).X * scale > r.Width && posLastSpace != 0 && posLastSpace != -1)
                            {
                                try
                                {
                                    temp += processedPieceOfString.Substring(0, posLastSpace) + "\n";

                                    bestMatch = bestMatch.Replace(processedPieceOfString.Substring(0, posLastSpace), "");
                                    posLastSpace = -1;

                                    break;
                                }
                                catch
                                {
                                    temp += processedPieceOfString.Substring(0, posLastSpace);
                                    //bestMatch = bestMatch.Replace(processedPieceOfString.Substring(0, posLastSpace) + " ", "");
                                    posLastSpace = -1;
                                    goto doThis;
                                }
                            }
                            else if (sf.MeasureString(processedPieceOfString).X * scale < r.Width && !(processedPieceOfString.Equals(bestMatch)) || (sf.MeasureString(processedPieceOfString).X * scale > r.Width && posLastSpace == 0))
                            {
                                posLastSpace = processedPieceOfString.Length - 1;

                            }
                        }
                        else if (processedPieceOfString.Equals(bestMatch))
                        {
                            temp += processedPieceOfString;
                            goto doThis;
                        }
                    }

                }
                temp += bestMatch;
            }
            else
            {
                temp = bestMatch;
            }


        doThis:
            {

            }

            String lastLine = temp.Substring(temp.LastIndexOf("\n") + 1);
            if (sf.MeasureString(lastLine).X * scale > r.Width)
            {
                temp = temp.Insert(temp.LastIndexOf(" ") + 1, "\n");
            }

            while (temp.Contains("\n "))
            {
                temp = temp.Replace("\n ", "\n");
            }
            return temp;
        }

        public static KeyValuePair<String, int> GenerateBestStringAndRectangle(String s, SpriteFont sf, int w)
        {
            while (s.IndexOf("\n") != -1)
            {
                s = s.Replace("\n", "");
            }

            String bestMatch = s;
            String temp = "";
            int posLastSpace = -1;

            if (sf.MeasureString(bestMatch).X > w)
            {
                while (sf.MeasureString(bestMatch).X > w)
                {
                    String processedPieceOfString = "";
                    foreach (char letter in bestMatch.ToCharArray())
                    {
                        processedPieceOfString += letter;

                        if (letter == ' ')
                        {
                            if (sf.MeasureString(processedPieceOfString).X > w)
                            {
                                try
                                {
                                    temp += processedPieceOfString.Substring(0, posLastSpace) + "\n";
                                    bestMatch = bestMatch.Replace(processedPieceOfString.Substring(0, posLastSpace), "");
                                    posLastSpace = -1;
                                    break;
                                }
                                catch
                                {
                                    temp += processedPieceOfString.Substring(0, posLastSpace);
                                    //bestMatch = bestMatch.Replace(processedPieceOfString.Substring(0, posLastSpace) + " ", "");
                                    posLastSpace = -1;
                                    goto doThis;
                                }
                            }
                            else if (sf.MeasureString(processedPieceOfString).X <= w && !(processedPieceOfString.Equals(bestMatch)))
                            {
                                posLastSpace = processedPieceOfString.Length - 1;

                            }
                        }
                        else if (processedPieceOfString.Equals(bestMatch))
                        {
                            temp += processedPieceOfString;
                            goto doThis;
                        }
                    }

                }
                temp += bestMatch;
            }
            else
            {
                temp = bestMatch;
            }


        doThis:
            {

            }

            int Height = (int)(sf.MeasureString(temp).Y + 1);

            return new KeyValuePair<String, int>(temp, Height);
        }

        public static void ClearCache()
        {
            lsi.Clear();
            Console.WriteLine("Text batch cleared");
        }

        public enum OutLining { Center = 0, Left, Right };
        static List<StringInfo> lsi = new List<StringInfo>();
        static Matrix defaultCamera = Matrix.CreateTranslation(new Vector3(0, 0, 1));
        public static void Draw(SpriteBatch sb, String s, SpriteFont sf, Rectangle box, OutLining ol, Color c, float scale, bool allowUpscaling = true, Matrix camera = default(Matrix), Color bgTColor = default(Color), bool bSaveToCache = true)
        {
            if (camera == default(Matrix))
            {
                camera = defaultCamera;
            }
            //ClearCache();
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearWrap, null, null, null, camera);

            StringInfo test = default(StringInfo);
            if (bSaveToCache)
            {
                test = lsi.Find(si => si.Equals(s, box));
            }

            bool generate = false;

            if (bSaveToCache)
            {
                if (test.s == null)
                {
                    generate = true;
                }
                else
                {
                    generate = ((test.ogString.Equals(s) && test.box.Equals(box))) ? false : true;
                }
            }


            if (generate || !bSaveToCache)
            {
                if (bgTColor == default(Color))
                {
                    bgTColor = Color.Black;
                }

                if (s.Equals("Leather vest", StringComparison.OrdinalIgnoreCase))
                {

                }

                String ogString = (String)s.Clone();
                Vector2 size = sf.MeasureString(s);
                Vector2 offSetCenter = Vector2.Zero;
                Vector2 Location = box.Location.ToVector2();
                bool bScaledX = false;
                bool bScaledY = false;
                bool bScaledUp = true;

                float scaleX = 1f;
                if ((size.ToPoint() + new Point(1)).X > box.Width)
                {
                    scaleX = (float)box.Width / (float)(size.ToPoint() + new Point(1)).X;
                    bScaledX = true;
                }
                else
                {
                    scaleX = (float)box.Width / (float)(size.ToPoint() + new Point(1)).X;
                    bScaledUp = true;
                }

                float scaleY = 1f;
                if ((size.ToPoint() + new Point(1)).Y > box.Height)
                {
                    scaleY = (float)box.Height / (float)(size.ToPoint() + new Point(1)).Y;
                    bScaledY = true;
                }
                else
                {
                    scaleY = (float)box.Height / (float)(size.ToPoint() + new Point(1)).Y;
                    bScaledUp = true;
                }

                if (bScaledUp)
                {
                    if (scaleX >= scaleY)
                    {
                        scaleX = scaleY;
                    }
                    else
                    {
                        scaleY = scaleX;
                    }
                }

                if (!allowUpscaling && scaleX > 1)
                {
                    scaleX = 1.0f;
                    scaleY = 1.0f;
                }

                switch (ol)
                {
                    case OutLining.Center:
                        Vector2 temp = (box.Center - box.Location).ToVector2();
                        temp.X -= size.X / 2 * scaleX;
                        temp.Y -= size.Y / 2 * scaleY;
                        Location += temp;
                        break;
                    case OutLining.Left:
                        temp = (box.Center - box.Location).ToVector2();
                        temp.Y -= size.Y / 2 * scaleY;
                        Location += new Vector2(0, temp.Y);
                        break;
                    case OutLining.Right:
                        temp = (box.Center - box.Location).ToVector2();
                        temp.Y -= size.Y / 2 * scaleY;
                        Location += new Vector2(0, temp.Y);
                        Location.X = (box.Location.ToVector2().X + box.Size.ToVector2().X) - size.X * scaleX;
                        break;
                    default:
                        break;
                }


                //if (Math.Abs(scaleX - 1) < 0.05f)
                //{
                //    scaleX = 1.0f;
                //    scaleY = 1.0f;
                //}

                if (bSaveToCache)
                {
                    lsi.Add(new StringInfo(s, ogString, sf, Location, new Vector2(scaleX, scaleY), c, box, bgTColor));
                    test = lsi[lsi.Count - 1];
                }
                else
                {
                    test = new StringInfo(s, ogString, sf, Location, new Vector2(scaleX, scaleY), c, box, bgTColor);
                }

            }

            sb.DrawString(sf, s, test.Location + new Vector2(1), test.bgTC * .7f, 0f, Vector2.Zero, test.scale, SpriteEffects.None, 0);
            sb.DrawString(sf, s, test.Location, test.c, 0f, Vector2.Zero, test.scale, SpriteEffects.None, 0);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, camera);
        }

        public static void DrawComplex(SpriteBatch sb, String s, SpriteFont sf, Rectangle box, OutLining ol, Color c, float scale, bool allowUpscaling = true, Matrix camera = default(Matrix), Color bgTColor = default(Color))
        {

            if (camera == default(Matrix))
            {
                camera = defaultCamera;
            }

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, null, null, null, null, null, camera);

            var test = lsi.Find(si => si.Equals(s, box));
            bool generate = false;
            if (test.s == null)
            {
                generate = true;
            }
            else
            {
                generate = ((test.ogString.Equals(s) && test.box.Equals(box))) ? false : true;
            }

            if (!generate)
            {
                //s = bestMatchStringForScaledBox(s, sf, box, test.scale.X);
            }

            if (generate)
            {

                if (bgTColor == default(Color))
                {
                    bgTColor = Color.Black;
                }


                String ogString = (String)s.Clone();
                Vector2 size = sf.MeasureString(s);

                //if (size.X > box.Width * 4 && size.X < box.Width * 6)
                //{
                //    float testScale = (float)(box.Width) / (size.X / 4);
                //    s = bestMatchStringForScaledBox(s, sf, box, testScale);
                //    size = sf.MeasureString(s);

                //}



                if (size.X > box.Width * 4)
                {
                    int index = 4;
                    while ((size.X > box.Width * index && !(size.X < box.Width * (index + 2))))
                    {

                        index += 2;
                    }

                    if (index == 4)
                    {
                        index = 6;
                    }

                    float testScale = (float)(box.Width) / (size.X);
                    testScale *= (index / 2);
                    s = bestMatchStringForScaledBox(s, sf, box, testScale);
                    size = sf.MeasureString(s);
                }
                else
                {
                    s = bestMatchStringForScaledBox(s, sf, box, 1f);
                    size = sf.MeasureString(s);
                }



                Vector2 offSetCenter = Vector2.Zero;
                Vector2 Location = box.Location.ToVector2();
                bool bScaledX = false;
                bool bScaledY = false;
                bool bScaledUp = true;

                float scaleX = 1f;
                if ((size.ToPoint() + new Point(1)).X > box.Width)
                {
                    scaleX = (float)box.Width / (float)(size.ToPoint() + new Point(1)).X;
                    bScaledX = true;
                }
                else
                {
                    scaleX = (float)box.Width / (float)(size.ToPoint() + new Point(1)).X;
                    bScaledUp = true;
                }

                float scaleY = 1f;
                if ((size.ToPoint() + new Point(1)).Y > box.Height)
                {
                    scaleY = (float)box.Height / (float)(size.ToPoint() + new Point(1)).Y;
                    bScaledY = true;
                }
                else
                {
                    scaleY = (float)box.Height / (float)(size.ToPoint() + new Point(1)).Y;
                    bScaledUp = true;
                }

                if (bScaledUp)
                {
                    if (scaleX >= scaleY)
                    {
                        scaleX = scaleY;
                    }
                    else
                    {
                        scaleY = scaleX;
                    }
                }

                if (!allowUpscaling && scaleX > 1)
                {
                    scaleX = 1.0f;
                    scaleY = 1.0f;
                }

                switch (ol)
                {
                    case OutLining.Center:
                        Vector2 temp = (box.Center - box.Location).ToVector2();
                        temp.X -= size.X / 2 * scaleX;
                        temp.Y -= size.Y / 2 * scaleY;
                        Location += temp;
                        break;
                    case OutLining.Left:
                        temp = (box.Center - box.Location).ToVector2();
                        temp.Y -= size.Y / 2 * scaleY;
                        Location += new Vector2(0, temp.Y);
                        break;
                    case OutLining.Right:
                        temp = (box.Center - box.Location).ToVector2();
                        temp.Y -= size.Y / 2 * scaleY;
                        Location += new Vector2(0, temp.Y);
                        Location.X = (box.Location.ToVector2().X + box.Size.ToVector2().X) - size.X * scaleX;
                        break;
                    default:
                        break;
                }


                //if (Math.Abs(scaleX - 1) < 0.05f)
                //{
                //    scaleX = 1.0f;
                //    scaleY = 1.0f;
                //}

                lsi.Add(new StringInfo(s, ogString, sf, Location, new Vector2(scaleX, scaleY), c, box, bgTColor));
                test = lsi[lsi.Count - 1];
            }

            sb.DrawString(sf, test.s, test.Location + new Vector2(1), test.bgTC * .7f, 0f, Vector2.Zero, test.scale, SpriteEffects.None, 0);
            sb.DrawString(sf, test.s, test.Location, test.c, 0f, Vector2.Zero, test.scale, SpriteEffects.None, 0);
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, camera);
        }

        public static void DrawString(SpriteBatch sb, SpriteFont font, String s, Vector2 pos, Vector2 scale, Color tc, Color tlc)
        {
            sb.DrawString(font, s, pos + new Vector2(1), tlc * .7f, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
            sb.DrawString(font, s, pos, tc, 0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }




        struct StringInfo
        {
            public String ogString;
            public String s;
            SpriteFont sf;
            public Vector2 Location;
            public Vector2 scale;
            public Color c;
            public Rectangle box;
            public Color bgTC;

            public StringInfo(String text, String ogString, SpriteFont sf, Vector2 Location, Vector2 scale, Color c, Rectangle box, Color bgtc)
            {
                this.ogString = ogString;
                this.s = text;
                this.sf = sf;
                this.Location = Location;
                this.c = c;
                this.scale = scale;
                this.box = box;
                this.bgTC = bgtc;
            }

            public bool Equals(String text, Rectangle box)
            {
                if (ogString.Equals("") || text.Equals(""))
                {
                    return false;
                }
                if (ogString.First() != text.First())
                {
                    return false;
                }
                if (ogString.Equals(text, StringComparison.OrdinalIgnoreCase))
                {
                    if (this.box.Equals(box))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
