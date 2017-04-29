using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Template;
using Connectome.Emotiv.Common;
using Connectome.Core.Interface;

namespace Connectome.Emotiv.Implementation
{
    /// <summary>
    /// NOT USED ANYWHERE!!!
    /// Calls OnRead every given interval and returns a state with power of the percentage 
    /// </summary>
    public class EmotivAnalyticReader : EmotivReader
    {
        /// <summary>
        /// target command 
        /// </summary>
        private EmotivCommandType TargetCommand;

        /// <summary>
        /// Time in ms 
        /// </summary>
        private long Interval;

        /// <summary>
        /// Time of which reading started 
        /// </summary>
        private Timer IntervalTimer; 
       // private Stopwatch intervalTimer;

        /// <summary>
        ///  thresh hold of target powe 
        /// </summary>
        private float ThreshHold;

        /// <summary>
        /// locks and unlock 'ReadingState' loop to return and compute a value. TPDO There are better way to 'lock' for threading
        /// </summary>
        private bool ShouldReturn;

        /// <summary>
        /// Holds list of states read in a interval 
        /// </summary>
        private LinkedList<IEmotivState> IntervalStatesList;

        /// <summary>
        /// Default  
        /// </summary>
        /// <param name="device"></param>
        private EmotivAnalyticReader(IEmotivDevice device) : base (device)
        {
            ShouldReturn = false;
        }

        /// <summary>
        /// Creates a reader with a target command, interval, and threshhold to report average accuracry per interval 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="target"></param>
        /// <param name="intervalMs"></param>
        /// <param name="threshHold"></param>
        public EmotivAnalyticReader(IEmotivDevice device, EmotivCommandType target, long intervalMs, float threshHold) : this(device)
        {
            this.TargetCommand = target;

            this.Interval = intervalMs; 
            this.ThreshHold = threshHold;
           
            IntervalTimer = new Timer(intervalMs);
            IntervalTimer.AutoReset = true;

            //reset list and enables  'ReadingState' to return a value 
            IntervalTimer.Elapsed += (o,e) =>
            {
                //unlock loop in 'ReadingState'
                ShouldReturn = true; 
            };

            //trigger the timer 
            OnStart += () => { IntervalTimer.Enabled = true;  }; 
        }

        /// <summary>
        /// Returns n average state after every intervals 
        /// </summary>
        /// <param name="device"></param>
        /// <returns>State with target command average power</returns>
        protected override IEmotivState ReadingState(IConnectomeDevice<IEmotivState> device, long time)
        {
            IntervalStatesList = new LinkedList<IEmotivState>();
            while (!ShouldReturn)
            {
                IntervalStatesList.AddFirst(device.Read(time)); 
            }

            ShouldReturn = false;

            return ComputeState(IntervalStatesList); 
        }

        /// <summary>
        /// Computes average target states aboce threshhold 
        /// </summary>
        /// <param name="list">list of read states</param>
        /// <returns>tate with target command average power</returns>
        protected virtual IEmotivState ComputeState(IEnumerable<IEmotivState> list)
        {
            IEnumerable<IEmotivState> validStatesList = list.Where(l => l.Command == TargetCommand &&  l.Power >= ThreshHold ).ToArray();

            float validStateCount = validStatesList.Count(); 
            float stateCount  = IntervalStatesList.Count();
            float powerPercent = (validStateCount / stateCount);
           
            return new EmotivState(TargetCommand,powerPercent,IntervalStatesList.Max(t => t.Time)); 
        }
    }
}
