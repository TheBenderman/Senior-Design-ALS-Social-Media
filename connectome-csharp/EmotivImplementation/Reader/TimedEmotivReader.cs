using Connectome.Core.Interface;
using Connectome.Emotiv.Enum;
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
        private IConnectomeReader<IEmotivState> reader;
        #endregion
        #region Constructor
        /// <summary>
        /// Keeps reading from device for duration. 
        /// </summary>
        /// <param name="reader">reader to read from</param>
        /// <param name="second">duration to read</param>
        public TimedEmotivReader(IConnectomeReader<IEmotivState> reader, int second)
        {
            this.reader = reader; 
            timer = new Timer(1000 * second);

            timer.Elapsed += (o, e) => reader.StopReading(); 
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
        public IConnectomeDevice<IEmotivState> Device
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

        public event Action<IEmotivState> OnRead
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
        public void StartReading()
        {
            timer.Enabled = true;
            reader.StartReading();
        }

        public void StopReading()
        {
            reader.StopReading();
            timer.Stop(); 
        }

        public void PlugDevice(IConnectomeDevice<IEmotivState> Device)
        {
            reader.PlugDevice(Device); 
        }
        #endregion
    }
}
