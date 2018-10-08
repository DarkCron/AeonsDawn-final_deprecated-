using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;

namespace TBAGW
{
    internal static class ThunderEffect
    {
        static bool bShow = true;
        static List<Flash> flashes = new List<Flash>();
        static int timePassed = 0;
        static int timer = 3000;

        internal static void Update(int t)
        {

            if (flashes.Count > 0)
            {
                flashes[0].Update(t);

                if (flashes[0].Remove())
                {
                    flashes.RemoveAt(0);
                    Flash.timePassed = 0;
                }
            }
            else
            {
                timePassed += t;
                if (timePassed >= timer)
                {
                    Generate();
                    timePassed = 0;
                }
            }

        }

        internal static void Generate(int rMax = 6)
        {
            flashes.Clear();
            int amount = GamePlayUtility.Randomize(2, rMax);
            for (int i = 0; i < amount - 1; i++)
            {
                int l = GamePlayUtility.Randomize(128, 256);
                int ttnf = GamePlayUtility.Randomize(96, 1048);
                flashes.Add(new Flash(l, ttnf, false, Color.LightGray));
            }
            flashes.Add(new Flash(GamePlayUtility.Randomize(640, 1200), 0, true, Color.LightYellow));
        }

        internal static bool IsShowing()
        {
            return bShow;
        }

        internal static Color GetColor()
        {
            if (flashes.Count > 0)
            {
                return flashes[0].FlashColor();
            }
            return Color.TransparentBlack;
        }

        internal static float ShadowOpacity()
        {
            if (flashes.Count == 0)
            {
                return DayLightHandler.ShadowOpacity;
            }
            else
            {
                if (flashes.Count == 1)
                {
                    return flashes[0].GetOpacity();
                }
                else
                {
                    return flashes[0].GetOpacity() / 4;
                }
               // return flashes[0].GetOpacity();
            }
        }
    }

    class Flash
    {
        internal static int timePassed = 0;
        int maxL;
        int length;
        int timeTillNextFlash;
        bool bDone;
        bool bFade;
        Color c;
        float opacity;

        internal Flash(int l, int ttnf, bool bf = false, Color c = default(Color))
        {
            length = l;
            timeTillNextFlash = ttnf;
            bDone = false;
            this.c = c == default(Color) ? Color.LightYellow : c;
            bFade = bf;
            maxL = l + ttnf;
            opacity = 1f;
        }

        internal void Update(int t)
        {
            timePassed += t;
            if (timePassed >= maxL)
            {
                bDone = true;
            }
            if (timePassed >= length)
            {
                opacity = 0f;
            }
            else
            {
                opacity = 1f - (float)(timePassed) / (float)length;

                GameProcessor.bUpdateShadows = true;

                if (bFade)
                {

                    // opacity = 1f-(float)(timePassed) / (float)length;
                }
            }
        }

        internal bool Remove()
        {
            return bDone;
        }

        internal float GetOpacity()
        {
            return opacity;
        }

        internal Color FlashColor()
        {
            //if (timePassed >= length)
            //{
            //    return c * 0f;
            //}
            return c * opacity;
        }
    }
}
