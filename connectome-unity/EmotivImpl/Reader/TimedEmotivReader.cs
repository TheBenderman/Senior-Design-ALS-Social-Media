using System;
using EmotivWrapperInterface;
using System.Timers;

namespace EmotivImpl
{
    /// <summary>
    /// A Reader that termanites itself after duration
    /// </summary>
    public class TimedEmotivReader : IEmotivReader
    {
        private Timer timer;
        private IEmotivReader reader; 

        /// <summary>
        /// Keeps reading from device for duration. 
        /// </summary>
        /// <param name="reader">reader to read from</param>
        /// <param name="second">duration to read</param>
        public TimedEmotivReader(IEmotivReader reader, int second)
        {
            this.reader = reader; 
            timer = new Timer(1000 * second);

            timer.Elapsed += (o, e) => reader.isRunning = false;
        }

        public bool isRunning
        {
            get
            {
                return reader.isRunning; 
            }

            set
            {
                reader.isRunning = value; 
            }
        }

        public Action<IEmotivState> OnRead
        {
            set
            {
                reader.OnRead = value;
            }
        }

        public Action OnStart
        {
            set
            {
                reader.OnStart = value; 
            }
        }

        public Action<EmotivStateType?, EmotivStateType> OnStateChange
        {
            set
            {
                reader.OnStateChange = value;
            }
        }

        public Action OnStop
        {
            set
            {
                reader.OnStop = value;
            }
        }

        public void Start()
        {
            timer.Enabled = true;
            reader.Start();
        }

        public void Stop()
        {
            reader.Start();
        }

    }
}
