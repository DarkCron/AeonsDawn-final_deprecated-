using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace TBAGW
{
    class InputControl
    {

        public double elapsedMilliseconds = 0;
        public double elapsedSeconds = 0;
        public double elapsedMinutes = 0;

        int currentSecondTick = 0;
        int currentMillisecondTick = 0;
        int currentMinuteTick = 0;

        public bool secondTimer(GameTime gametime, double secondTimer)
        {
            elapsedSeconds += gametime.ElapsedGameTime.TotalSeconds;

            if(elapsedSeconds>secondTimer){
                
                elapsedSeconds = 0;
                Reset();
                return true;
            }

            return false;
        }

        public bool minuteTimer(GameTime gametime, double minuteTimer)
        {
            elapsedMinutes += gametime.ElapsedGameTime.TotalMinutes;

            if (elapsedMinutes > minuteTimer)
            {
               
                elapsedMinutes = 0;
                Reset();
                return true;
            }

            return false;
        }

        public bool millisecondTimer(GameTime gametime, double millisecondTimer)
        {
            elapsedMilliseconds += gametime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedMilliseconds > millisecondTimer)
            {
                
                elapsedMilliseconds = 0;
                Reset();
                return true;
            }

            return false;
        }

        /*int times represents how many times a tick should be given
         * In proper English *ehem¨* A countdown Timer
         * 
         */
        public bool chronoSecondTimer(GameTime gametime, double secondTimer, int times)
        {
            if (currentSecondTick < times)
            {
                elapsedSeconds += gametime.ElapsedGameTime.TotalSeconds;

                if (elapsedSeconds > secondTimer)
                {
                    currentSecondTick++;
                    elapsedSeconds = 0;
                    return true;
                    
                }

                
            }
            else if(currentSecondTick>=times)
            {
                currentSecondTick = 0;
                return true;
            }


            return false;
        }

        public bool chronoMillisecondTimer(GameTime gametime, double millisecondTimer, int times)
        {
            if (currentMillisecondTick < times)
            {
                elapsedMilliseconds += gametime.ElapsedGameTime.TotalMilliseconds;

                if (elapsedMilliseconds > millisecondTimer)
                {
                    currentMillisecondTick++;
                    elapsedMilliseconds = 0;
                    return true;
                }
            }
            else if (currentMillisecondTick >= times)
            {
                currentMillisecondTick = 0;
                return true;
            }
            return false;
        }

        public bool chronoMinuteTimer(GameTime gametime, double minuteTimer, int times)
        {
            if (currentMinuteTick < times)
            {
                elapsedMinutes += gametime.ElapsedGameTime.TotalMinutes;

                if (elapsedMinutes > minuteTimer)
                {
                    currentMinuteTick++;
                    elapsedMinutes = 0;
                    return true;
                }
            }
            else if (currentMinuteTick >= times)
            {
                currentMinuteTick = 0;
                return true;
            }
            return false;
        }

        public void Reset()
        {
             elapsedMilliseconds = 0;
             elapsedSeconds = 0;
             elapsedMinutes = 0;

             currentSecondTick = 0;
             currentMillisecondTick = 0;
             currentMinuteTick = 0;
        }
    }
}
