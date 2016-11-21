using EmotivWrapper.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmotivWrapper;

namespace EmotivImpl
{
    /// <summary>
    /// A Reader that termanites itself after reading for a time limit 
    /// </summary>
    public class TimedEmotivReader : EmotivReader
    {
        private Stopwatch timer; 
        private long duration; 

        public TimedEmotivReader(EmotivDevice device, int second) : base(device)
        {
            this.duration = 1000*second;

            OnStart += () => timer = Stopwatch.StartNew(); 
        }

        protected override EmotivState ReadingState(EmotivDevice device)
        {
            EmotivState read = base.ReadingState(device);

            if (timer.ElapsedMilliseconds > this.duration)
            {
                timer.Stop();
                isRunning = false;
            }

            return read;
        }

      
    }
}
