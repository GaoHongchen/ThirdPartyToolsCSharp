using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GHCLibs
{
    class TimerUser
    {
        private System.Timers.Timer timer;

        public TimerUser()
        {
            timer = new System.Timers.Timer();
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        public void SetTimer(double interval)
        {

            timer.Interval = interval;      
            //timer.AutoReset = true;
            //System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            timer.Start();
            //timer.Enabled = true;
        }

        public void KillTimer()
        {
            timer.Stop();
        }
    }
}
