using EmotivWrapper.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmotivWrapper;
using System.Diagnostics;

namespace EmotivImpl.Reader
{
    /// <summary>
    /// Calls OnRead every given interval and returns a state with power of the percentage 
    /// </summary>
    public class EmotivAnalyticReader : EmotivReader
    {
        private EmotivStateType targetCommand;

        /// <summary>
        /// Time in ms 
        /// </summary>
        private long interval;

        /// <summary>
        /// Time of which reading started 
        /// </summary>
        private Stopwatch intervalTimer;

        private float threshHold; 

        /// <summary>
        /// Holds list of states read in a interval 
        /// </summary>
        private LinkedList<EmotivState> intervalStatesList; 


        public EmotivAnalyticReader(EmotivDevice device, EmotivStateType target, long ms, float threshHold) : base(device)
        {
            this.targetCommand = target;

            this.interval = ms; 
            this.threshHold = threshHold;

        }

        protected override EmotivState ReadingState(EmotivDevice device)
        {
            intervalStatesList = new LinkedList<EmotivState>();
            intervalTimer = Stopwatch.StartNew();

            while (intervalTimer.ElapsedMilliseconds < interval)
            {
                intervalStatesList.AddLast(device.Read());
            }

            IEnumerable<EmotivState> validStatesList = intervalStatesList.Where(l => l.command == targetCommand &&  l.power >= threshHold ).ToArray();

            float validStateCount = validStatesList.Count(); 
            float stateCount  = intervalStatesList.Count();
            float powerPercent = (validStateCount / stateCount); 

            return new EmotivState() { command = targetCommand, power = powerPercent, time = intervalStatesList .Last().time}; 
        }
    }
}
