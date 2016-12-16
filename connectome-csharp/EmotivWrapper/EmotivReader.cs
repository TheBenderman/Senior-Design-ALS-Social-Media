using EmotivWrapperInterface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmotivWrapper.Core
{
    /// <summary>
    /// Reads from 
    /// </summary>
   public class EmotivReader : IEmotivReader
    {
        #region private attributes 
        /// <summary>
        /// Emotiv device to read states from. 
        /// </summary>
        private IEmotivDevice device;
        
        /// <summary>
        /// State reading thread. 
        /// </summary>
        private Thread readingThread;

        private Stopwatch timer; 

        private long lastTime; 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public bool isRunning { set; get; }

        #region events

        public Action<IEmotivState> OnRead { set; get; }

        /// <summary>
        /// State from, to
        /// </summary>
        public Action<EmotivStateType?, EmotivStateType> OnStateChange { set; get; }

        /// <summary>
        /// Getes invoked when Start is called 
        /// </summary>
        public Action OnStart { set; get; }

        /// <summary>
        /// Gets invoked when Stop is called 
        /// </summary>
        public Action OnStop { set; get; }
        #endregion

        #region constructor 
        /// <summary>
        /// Creates an EmotivReader for device 
        /// </summary>
        /// <param name="device"></param>
        public EmotivReader(IEmotivDevice device)
        {
            this.device = device;
            isRunning = false;
            lastTime = -1; 
        }
        #endregion

        #region public methods  
        /// <summary>
        /// Starts a thread calling Read from device. 
        /// </summary>
        public void Start()
        {
            string error; 
           if(!device.Connect(out error))
            {
                throw new Exception("Connection Failed"); //create custom exception 
            }
            //start reading 
            isRunning = true; 
            readingThread = new Thread(ReadThreadLoop);
            readingThread.Start();

            OnStart?.Invoke(); 
        }

        /// <summary>
        /// Disconnect device and aborts reading thread. 
        /// </summary>
        public void Stop()
        {
            device.Disconnect();

            OnStop?.Invoke();
        }
        #endregion

        #region privates 

        /// <summary>
        /// Looks when Reader isRunning. 
        /// </summary>
        private void ReadThreadLoop()
        {
            EmotivStateType? previousState = null;
            InsureNewTime();
            while (isRunning)
            {
                IEmotivState stateRead = ReadingState(device);
                stateRead.time = lastTime; 
                    
                OnRead?.Invoke(stateRead);

                if (previousState != stateRead.command)
                {
                    OnStateChange?.Invoke(previousState, stateRead.command);
                }

                previousState = stateRead.command;
                InsureNewTime();
            }

            Stop(); 
        }

        /// <summary>
        /// Reads next state in device 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="prevSate"></param>
        /// <returns></returns>
        protected virtual IEmotivState ReadingState(IEmotivDevice device)
        {
            return device.Read(); 
        }
        #endregion 

        /// <summary>
        /// Insures at least 1 ms have passed 
        /// </summary>
        private void InsureNewTime()
        {
            if(timer == null)
            {
                timer = Stopwatch.StartNew(); 
            }

            while (lastTime == timer.ElapsedMilliseconds ) ;
            lastTime = timer.ElapsedMilliseconds; 
        }

    }
}
