using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Event;
using Connectome.Emotiv.Interface;
using System;
using System.Diagnostics;
using System.Threading;

namespace Connectome.Emotiv.Template
{
    /// <summary>
    /// Basic device reader that read a state every millisecond. 
    /// </summary>
    public abstract class EmotivReader : IEmotivReader, IDisposable
    {
        #region Private Attributes 
        /// <summary>
        /// Emotiv device to read states from. 
        /// </summary>
        public IEmotivDevice Device { set; get; }
        
        /// <summary>
        /// A thread that keeps reading states. 
        /// </summary>
        private Thread ReadingThread;

        /// <summary>
        /// Holds timer to tell when each state is read after starting. 
        /// </summary>
        private Stopwatch ReadingTimer; 

        /// <summary>
        /// Holds last time a state was read after starting. 
        /// </summary>
        private long LastTime;

        /// <summary>
        /// Keeps reading loop alive. 
        /// </summary>
        private bool KeepReading;
        #endregion
        #region Constructors
        /// <summary>
        /// Creates an EmotivReader for device 
        /// </summary>
        /// <param name="device"></param>
        public EmotivReader(IEmotivDevice device)
        {
            this.Device = device;
            KeepReading = false;
            LastTime = -1;

            ReadingThread = new Thread(ReadThreadLoop);
        }
        #endregion
        #region Abstract Methods
        /// <summary>
        /// Reads next state in device 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="prevSate"></param>
        /// <returns></returns>
        protected abstract IEmotivState ReadingState(IEmotivDevice device, long time);
        #endregion
        #region Private Methods
        /// <summary>
        /// Keeps readings states from device 
        /// </summary>
        private void ReadThreadLoop()
        {
            EmotivCommandType? previousState = null;
            InsureMillisecondPassed();

            try
            {
                while (IsReading)
                {
                    IEmotivState stateRead = ReadingState(Device, LastTime);

                    OnRead?.Invoke(new EmotivReadArgs(stateRead));

                    if (previousState != stateRead.Command)
                    {
                        OnCommandChange?.Invoke(previousState, stateRead.Command);
                    }

                    previousState = stateRead.Command;
                    InsureMillisecondPassed();
                }
            }
            catch (Exception e)
            {
                OnStop?.Invoke(e.ToString());
                ExceptionHandler?.Invoke( new Exception("Something went wrong while reading. ", e));
                return; 
            }

            OnStop?.Invoke("Normal");
        }

        /// <summary>
        /// Attempt to disconnects device, throws exception when failing. 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="expMsg"></param>
        private void TryDisconnecting(IEmotivDevice device, string expMsg = "Unable to disconnect device: ")
        {
            bool didDisconnect = true;
            string disconnectMsg = "";

            device.OnDisconnectAttempted += (b, m) => { didDisconnect = b; disconnectMsg = m;  };

            device.Disconnect(); 
  
            if (didDisconnect == false)
            {
                ExceptionHandler?.Invoke(new Exception(expMsg + disconnectMsg));
            }
        }

        /// <summary>
        /// Attempt to connects device, throws exception when failing. 
        /// </summary>
        private void TryConnecting(IEmotivDevice device, string expMsg = "Unable to connect device: ")
        {
            bool didConnect = true;
            string connectMsg = "";

            device.OnConnectAttempted += (b, m) => { didConnect = b; connectMsg = m; };

            device.Connect();
           
            if (didConnect == false)
            {
                ExceptionHandler?.Invoke(new Exception(expMsg + connectMsg));
            }
        }
        #endregion
        #region Protected Methods
        /// <summary>
        /// Insures at least 1 ms have passed 
        /// </summary>
        protected void InsureMillisecondPassed()
        {
            if (ReadingTimer == null)
            {
                ReadingTimer = Stopwatch.StartNew();
            }

            while (LastTime == ReadingTimer.ElapsedMilliseconds) ;
                LastTime = ReadingTimer.ElapsedMilliseconds;
        }
        #endregion
        #region IEmotivReader Public Get Property
        /// <summary>
        /// Determans if 
        /// </summary>
        public bool IsReading { get { return KeepReading && ReadingThread.IsAlive;  } }
        #endregion
        #region IEmotivReader Events

        /// <summary>
        /// Invoked when a state is read. 
        /// </summary>
        public event Action<EmotivReadArgs> OnRead;

        /// <summary>
        /// Invoked when command type changes 
        /// </summary>
        public event Action<EmotivCommandType?, EmotivCommandType> OnCommandChange;

        /// <summary>
        /// Invoked when Start is called 
        /// </summary>
        public event Action OnStart;

        /// <summary>
        /// Invoked when Stop is called 
        /// </summary>
        public event Action<string> OnStop;
     
        /// <summary>
        /// Handles expections 
        /// </summary>
        public event Action<Exception> ExceptionHandler;
        #endregion
        #region IEmotivReader Public Methods  
        /// <summary>
        /// Starts a thread calling Read from device. 
        /// </summary>
        public void Start()
        {
            if(!Device.IsConnected)
                TryConnecting(Device);

            KeepReading = true; 
            
            ReadingThread.Start();
            OnStart?.Invoke(); 
        }

        /// <summary>
        /// Stops reading 
        /// </summary>
        public void Stop()
        {
            KeepReading = false; 
        }

        /// <summary>
        /// Replaces old device with a new one and starts it. Old device is stopped and disconnected.
        /// </summary>
        /// <param name="Device"></param>
        public void PlugDevice(IEmotivDevice Device)
        {
            if(IsReading)
            {
                Stop();
                //wait until thread is dead. 
                ReadingThread.Join();
            }

            if (Device != null)
            {
                OnStop += (e) =>  { TryDisconnecting(this.Device, "Unable to disconnect previous device: "); }; 
                Stop();
            }

            //old device is disconnected and reading is off 

            this.Device = Device;

            Start();
        }
        #endregion
       
        #region IDispose Public Method
        /// <summary>
        /// Dispose thread
        /// </summary>
        public void Dispose()
        {
           if(ReadingThread.IsAlive)
            {
                ReadingThread.Abort(); 
            }
        }
        #endregion 
    }
}
