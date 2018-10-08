using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;

namespace TBAGW
{

    internal static class EnvironmentHandler
    {
        static bool bRaining = false;
        static bool bThunder = false;

        internal static void Update(GameTime gt)
        {
            if (false)
            {
                bRaining = true;
                bThunder = true;
            }else
            {
                bRaining = false;
                bThunder = false;
            }
            if (bRaining)
            {
                TestEnvironment.UpdateRain(gt);
            }

            if (bThunder)
            {
                ThunderEffect.Update(gt.ElapsedGameTime.Milliseconds);
            }
         
            
        }

        internal static void GenerateRenders(SpriteBatch sb)
        {
            TestEnvironment.GenerateRainRender(sb);
        }

        internal static void DrawEnvironmentPreLightning(SpriteBatch sb)
        {
            if (bRaining)
            {
                sb.Draw(TestEnvironment.rainRender, TestEnvironment.rainRender.Bounds, Color.White);
            }
        }

        internal static float ShadowOpacity()
        {
            if (bThunder)
            {
                return ThunderEffect.ShadowOpacity();
            }else
            {
                return DayLightHandler.ShadowOpacity;
            }
        }
    }
}
