using EmotivWrapper.Core;
using System.Collections.Generic;
using System.Linq;
using EmotivWrapper;
using System.Timers;
using EmotivWrapperInterface;

namespace EmotivImpl
{
    /// <summary>
    /// Calls OnRead every given interval and returns a state with power of the percentage 
    /// </summary>
    public class EmotivAnalyticReader : EmotivReader
    {
        /// <summary>
        /// target command 
        /// </summary>
        private EmotivStateType targetCommand;

        /// <summary>
        /// Time in ms 
        /// </summary>
        private long interval;

        /// <summary>
        /// Time of which reading started 
        /// </summary>
        private Timer intervalTimer; 
       // private Stopwatch intervalTimer;

        /// <summary>
        ///  thresh hold of target powe 
        /// </summary>
        private float threshHold;

        /// <summary>
        /// locks and unlock 'ReadingState' loop to return and compute a value 
        /// </summary>
        private bool shouldReturn;

        /// <summary>
        /// Holds list of states read in a interval 
        /// </summary>
        private LinkedList<IEmotivState> intervalStatesList;

        /// <summary>
        /// Default  
        /// </summary>
        /// <param name="device"></param>
        private EmotivAnalyticReader(IEmotivDevice device) : base (device)
        {
            shouldReturn = false;
        }

        /// <summary>
        /// Creates a reader with a target command, interval, and threshhold to report average accuracry per interval 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="target"></param>
        /// <param name="intervalMs"></param>
        /// <param name="threshHold"></param>
        public EmotivAnalyticReader(IEmotivDevice device, EmotivStateType target, long intervalMs, float threshHold) : this(device)
        {
            this.targetCommand = target;

            this.interval = intervalMs; 
            this.threshHold = threshHold;
           
            intervalTimer = new Timer(intervalMs);
            intervalTimer.AutoReset = true;

            //reset list and enables  'ReadingState' to return a value 
            intervalTimer.Elapsed += (o,e) =>
            {
                //unlock loop in 'ReadingState'
                shouldReturn = true; 
            };

            //trigger the timer 
            OnStart += () => { intervalTimer.Enabled = true;  }; 
        }

        /// <summary>
        /// Returns n average state after every intervals 
        /// </summary>
        /// <param name="device"></param>
        /// <returns>State with target command average power</returns>
        protected override IEmotivState ReadingState(IEmotivDevice device)
        {
            intervalStatesList = new LinkedList<IEmotivState>();
            while (!shouldReturn)
            {
                intervalStatesList.AddFirst(device.Read()); 
            }

            shouldReturn = false;

            return ComputeState(intervalStatesList); 
        }

        /// <summary>
        /// Computes average target states aboce threshhold 
        /// </summary>
        /// <param name="list">list of read states</param>
        /// <returns>tate with target command average power</returns>
        protected virtual IEmotivState ComputeState(IEnumerable<IEmotivState> list)
        {
            IEnumerable<IEmotivState> validStatesList = list.Where(l => l.command == targetCommand &&  l.power >= threshHold ).ToArray();

            float validStateCount = validStatesList.Count(); 
            float stateCount  = intervalStatesList.Count();
            float powerPercent = (validStateCount / stateCount);
           
            return new EmotivState() { command = targetCommand, power = powerPercent, time = intervalStatesList.Max(t => t.time)}; 
        }
    }
}
