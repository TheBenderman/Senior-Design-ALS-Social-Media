using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Event;
using Connectome.Emotiv.Interface;
using System;
using System.Timers;

namespace Connectome.Emotiv.Implementation
{
    /// <summary>
    /// A Reader that termanites itself after duration
    /// </summary>
    public class TimedEmotivReader : IEmotivReader
    {
        #region Private Attributes
        private Timer timer;
        private IEmotivReader reader;
        #endregion
        #region Constructor
        /// <summary>
        /// Keeps reading from device for duration. 
        /// </summary>
        /// <param name="reader">reader to read from</param>
        /// <param name="second">duration to read</param>
        public TimedEmotivReader(IEmotivReader reader, int second)
        {
            this.reader = reader; 
            timer = new Timer(1000 * second);

            timer.Elapsed += (o, e) => reader.Stop(); 
        }
        #endregion
        #region IEmotivReader Public Properies 
        public bool IsReading
        {
            get
            {
                return reader.IsReading; 
            }
        }
        public IEmotivDevice Device
        {
            get
            {
                return reader.Device; 
            }

            set
            {
                reader.Device = value; 
            }
        }
        #endregion
        #region IEmotivReader Public Events

        public event Action<EmotivReadArgs> OnRead
        {
            add
            {
                reader.OnRead += value; 
            }
            remove
            {
                reader.OnRead -= value;
            }
        }

        public event Action OnStart
        {
            add
            {
                reader.OnStart += value;
            }
            remove
            {
                reader.OnStart -= value;
            }
        }

        public event Action<string> OnStop
        {
            add
            {
                reader.OnStop += value;
            }
            remove
            {
                reader.OnStop -= value;
            }
        }

        public event Action<EmotivCommandType?, EmotivCommandType> OnCommandChange
        {
            add
            {
                reader.OnCommandChange += value;
            }
            remove
            {
                reader.OnCommandChange -= value;
            }
        }

        public event Action<Exception> ExceptionHandler
        {
            add
            {
                reader.ExceptionHandler += value;
            }
            remove
            {
                reader.ExceptionHandler -= value;
            }
        }

        #endregion
        #region IEmotivReader Public Methods
        public void Start()
        {
            timer.Enabled = true;
            reader.Start();
        }

        public void Stop()
        {
            reader.Stop();
            timer.Stop(); 
        }

        public void PlugDevice(IEmotivDevice Device)
        {
            reader.PlugDevice(Device); 
        }
        #endregion
        #region IDispose Public Method
        /// <summary>
        /// Dispose thread
        /// </summary>
        public void Dispose()
        {
            reader.Dispose();
            timer.Dispose();
        }
        #endregion 
    }
}
