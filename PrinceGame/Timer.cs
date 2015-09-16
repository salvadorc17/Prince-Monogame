using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PrinceGame
{
    public class Timer
    {
        private int startCount;
        private int endCount;
        public String displayValue
        {
            get { return m_displayValue; }
            private set { m_displayValue = value; }
        }
        private String m_displayValue;
        public Boolean isActive
        {
            get { return m_isActive; }
            private set { m_isActive = value; }
        }
        private Boolean m_isActive;
        public Boolean isComplete
        {
            get { return m_isComplete; }
            private set { m_isComplete = value; }
        }

        private Boolean m_isComplete;
        public Timer()
        {
            this.isActive = false;
            this.isComplete = false;
            this.displayValue = "None";
            this.startCount = 0;
            this.endCount = 0;
        }

        public void @set(GameTime gameTime, int seconds)
        {
            this.startCount = gameTime.TotalGameTime.Seconds;
            this.endCount = this.startCount + seconds;
            this.isActive = true;
            this.displayValue = this.endCount.ToString();
        }
        public Boolean checkTimer(GameTime gameTime)
        {
            if (this.isComplete == false)
            {
                if (gameTime.TotalGameTime.Seconds > this.startCount)
                {
                    this.startCount = gameTime.TotalGameTime.Seconds;
                    this.endCount = this.endCount - 1;
                    this.displayValue = this.endCount.ToString();
                    if (this.endCount < 0)
                    {
                        this.endCount = 0;
                        this.isComplete = true;
                        this.displayValue = "Game Over";
                    }
                }
            }
            else
            {
                this.displayValue = "Game Over";
            }
            return this.isComplete;
        }
        public void reset()
        {
            this.isActive = false;
            this.isComplete = false;
            this.displayValue = "None";
            this.startCount = 0;
            this.endCount = 0;
        }
    }
}
