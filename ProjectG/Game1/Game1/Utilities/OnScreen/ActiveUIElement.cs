/*
 * Created by SharpDevelop.
 * User: r0382279
 * Date: 10/17/2016
 * Time: 2:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("Active UI Element")]
    [XmlInclude(typeof(TextElement))]
    public class ActiveUIElement
    {
        [XmlElement("UI Element Timer")]
        public ElementTimer timer = new ElementTimer();
        [XmlElement("UI Element draw pos")]
        public Vector2 drawPos = new Vector2(100, 150);
        [XmlElement("UI Element draw Rectangle")]
        public Rectangle drawRectangle = default(Rectangle);
        [XmlElement("UI Element opacity")]
        public float elementOpacity = 0f;
        [XmlElement("UI Element fade in time")]
        public int elementFadeIn = 500;
        [XmlElement("UI Element fade out time")]
        public int elementFadeOut = 500;

        [XmlIgnore]
        public int elementFadeTimePassed = 0;
        [XmlIgnore]
        public bool bFadedIn = false;
        [XmlIgnore]
        public bool bRemove = false;

        public ActiveUIElement()
        {

        }

        public void SetTimer(int timerTime)
        {
            timer.timer = timerTime;
            timer.Activate();
        }

        public virtual void Update(GameTime gt)
        {

            if (!bFadedIn && elementFadeIn != 0)
            {
                elementFadeTimePassed += gt.ElapsedGameTime.Milliseconds;
                elementOpacity = (float)((float)elementFadeTimePassed / (float)elementFadeIn);
                if (elementFadeTimePassed > elementFadeIn)
                {
                    bFadedIn = true;
                    elementOpacity = 1f;
                    elementFadeTimePassed = 0;
                }
            }
            else if (elementFadeIn == 0)
            {
                elementOpacity = 1f;
                bFadedIn = true;
            }else if(bFadedIn && !timer.bIsActive)
            {
                elementFadeTimePassed += gt.ElapsedGameTime.Milliseconds;
                elementOpacity = 1f- ((float)elementFadeTimePassed / (float)elementFadeIn);
                if (elementFadeTimePassed > elementFadeIn)
                {
                    timer.bIsDone = true;
                    elementOpacity = 0f;
                    elementFadeTimePassed = 0;
                }
            }

            if (timer.bIsActive)
            {
                timer.Update(gt);
            }

        }

        public bool IsDoneRemove()
        {
            if (timer.bIsDone && !timer.bIsActive || bRemove)
            {
                return true;
            }
            return false;
        }

        public virtual void Draw(SpriteBatch sb)
        {

        }

    }

    [XmlRoot("UI Element Timer Main")]
    public class ElementTimer
    {
        [XmlElement("Time to tick")]
        public int timer = 0;
        [XmlElement("Time passed")]
        public int timePassed = 0;
        [XmlElement("Is timer active")]
        public bool bIsActive = false;
        [XmlElement("Is timer done")]
        public bool bIsDone = false;
        [XmlElement("Report remaining time")]
        //if not showing remaining time, it's supposed to show the time passed since start instead.
        public bool bShowRemainingTime = false;

        public ElementTimer() { }

        public void Update(GameTime gt)
        {
            if (bIsActive)
            {
                timePassed += gt.ElapsedGameTime.Milliseconds;

                if (timePassed > timer)
                {
                    Deactivate();
                }
            }
        }

        public void Activate()
        {
            bIsActive = true;
        }

        public void Deactivate()
        {
            bIsActive = false;
        }

        public void convertTimeToTimer(int hours, int minutes, int seconds)
        {
            timer = (hours * 60 * 60 + minutes * 60 + seconds) * 1000;
        }

        public List<int> reportRemainingTime()
        {
            int timeRemaining = timer - timePassed;
            int hours = timeRemaining / 3600 / 1000;
            timeRemaining -= hours * 3600 * 1000;
            int minutes = timeRemaining / 60 / 1000;
            timeRemaining -= minutes * 60 * 1000;
            int seconds = timeRemaining / 1000;

            return new List<int> { seconds, minutes, hours };
        }

        public List<int> reportTimePassed()
        {
            int timeRemaining = timePassed;
            int hours = timeRemaining / 3600 / 1000;
            timeRemaining -= hours * 3600 * 1000;
            int minutes = timeRemaining / 60 / 1000;
            timeRemaining -= minutes * 60 * 1000;
            int seconds = timeRemaining / 1000;

            return new List<int> { seconds, minutes, hours };
        }

        public int reportTimePassedMS()
        {
            return timePassed;
        }

        public int reportTimeRemainingMS()
        {
            return timer - timePassed;
        }
    }
}
