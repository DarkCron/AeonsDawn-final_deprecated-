using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    public class TimingUtility
    {
        int timer = 16;
        int timePassed = 0;
        bool bMultiTick = true;
        bool bStop = false;
        public delegate bool stopCondition();
        stopCondition stopCheck;

        bool bUsingStepTimer = false;
        int steps = 60;
        int stepsTaken = 0;

        public TimingUtility(int timer, bool muliTick = true, stopCondition sc = null)
        {
            this.timer = timer;
            this.bMultiTick = muliTick;
            this.stopCheck = sc;
        }

        public void SetStepTimer(int steps, int stepsTaken = 0)
        {
            this.steps = steps;
            this.stepsTaken = stepsTaken;
            bUsingStepTimer = true;
        }

        public int stepsRemaining()
        {
            return steps - stepsTaken;
        }

        public float percentageDone()
        {
            return ((float)stepsTaken / (float)steps);
        }

        public void Tick(GameTime gt)
        {
            if (!bStop && stopCheck!=null)
            {
                bStop = stopCheck();
            }
            if (!bStop)
            {
                timePassed += gt.ElapsedGameTime.Milliseconds;
            }

            if (bUsingStepTimer)
            {
                while (Ding())
                {

                }
            }
        }

        public bool Ding()
        {
            if (bStop) { return false; }
            if (timePassed >= timer)
            {
                if (bUsingStepTimer)
                {
                    stepsTaken++;
                }
                timePassed -= timer;

                if (timer == 0)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        public void Stop()
        {
            bStop = true;
        }

        public void Start() { bStop = false; }

        public bool IsActive()
        {
            return !bStop;
        }

        public bool IsDone()
        {
            return bStop;
        }
    }
}
