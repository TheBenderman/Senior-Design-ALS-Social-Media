using SciterSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Connectome.Src
{
    class ButtonTimer
    {
        public static void startTimer()
        {
            System.Timers.Timer myTimer = new System.Timers.Timer();
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(onTimedEvent);
            myTimer.Interval = 2000;
            myTimer.Enabled = true;
        }

        private static void onTimedEvent(object source, ElapsedEventArgs e)
        {
            Program.HostInstance.InvokePost(() =>
            {
                Program.HostInstance.CallFunction("NextButton", new SciterValue(""));
            });
        }
    }
}
