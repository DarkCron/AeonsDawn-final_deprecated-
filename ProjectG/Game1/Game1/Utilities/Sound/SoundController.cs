using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace TBAGW
{
    public static class SoundController
    {
        public static SoundEffectInstance loopBG;

        public static void Stop()
        {
            if (loopBG != null)
            {
                loopBG.Stop();
            }
        }

        public static void Resume()
        {
            if (loopBG != null)
            {
                loopBG.Resume();
            }
        }

        static int opacitySteps = 0;
        static float opacityStep = 0;
        static int opacityStepsTaken = 0;
        static public void GenerateOpacitySteps(float fadeEnd, float time)
        {
            if(time!=0)
            {
                opacitySteps = (int)(time * 60);
                opacityStep = (float)(((float)(fadeEnd - loopBG.Volume)) / ((float)opacitySteps));
            }else
            {
                loopBG.Volume = fadeEnd;
            }

            opacityStepsTaken = 0;
        }

        public static void FadeOut(float fadeOutTo)
        {
            if (loopBG != null)
            {
                loopBG.Resume();
            }
        }

        public static void Update()
        {
            if(opacityStepsTaken<opacitySteps)
            {
                opacityStepsTaken++;
                loopBG.Volume += opacityStep;
            }
        }

        public static void Start(SoundEffectInstance newBG, float soundLevel = 1f)
        {
            if (loopBG != null)
            {
                loopBG.Stop();
            }
            if(newBG!=null)
            {
                loopBG = newBG;
                loopBG.IsLooped = true;
                loopBG.Volume = soundLevel;
                loopBG.Play();
            }
            
        }
    }
}
