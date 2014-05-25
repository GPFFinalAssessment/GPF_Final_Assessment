using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace GPF_Final_Assessment
{
    public class Timer
    {
        //=================================================================
        //TIMER CODE
        public int TimerMax;
        public float TimerElapsed;
        public bool TimerEnd;
        //=================================================================

        public Timer(int TimerBegin, int TimerMax)
        {
            this.TimerElapsed = (float)TimerBegin;
            this.TimerMax = TimerMax;
            this.TimerEnd = false;
        }

        //=================================================================
        //TIMER CODE
        public void GameTimer(GameTime gameTime)
        {
            if (!this.TimerEnd)
            {
                this.TimerElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if ((int)this.TimerElapsed >= this.TimerMax)
                    this.TimerEnd = true;
            }
        }
        //=================================================================
    }
}

