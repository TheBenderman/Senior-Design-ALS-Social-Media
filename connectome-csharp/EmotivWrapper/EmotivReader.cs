using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmotivWrapper.Core
{
    /// <summary>
    /// Reads from 
    /// </summary>
   public class EmotivReader
    {
        #region private attributes 
        /// <summary>
        /// Emotiv device to read states from. 
        /// </summary>
        private EmotivDevice device;
        
        /// <summary>
        /// State reading thread. 
        /// </summary>
        private Thread readingThread;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public bool isRunning;

        #region events

        public Action<EmotivState> OnRead;

        /// <summary>
        /// State from, to
        /// </summary>
        public Action<EmotivStateType?, EmotivStateType> OnStateChange;

        /// <summary>
        /// Getes invoked when Start is called 
        /// </summary>
        public Action OnStart;

        /// <summary>
        /// Gets invoked when Stop is called 
        /// </summary>
        public Action OnStop;
        #endregion

        #region constructor 
        /// <summary>
        /// Creates an EmotivReader for device 
        /// </summary>
        /// <param name="device"></param>
        public EmotivReader(EmotivDevice device)
        {
            this.device = device;
            isRunning = false; 
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

            readingThread.Abort();
        }
        #endregion

        #region privates 

        /// <summary>
        /// Looks when Reader isRunning. 
        /// </summary>
        private void ReadThreadLoop()
        {
            EmotivStateType? previousState = null;
            while (isRunning)
            {
                    EmotivState stateRead = ReadingState(device);

                    OnRead?.Invoke(stateRead);

                    if (previousState != stateRead.command)
                    {
                        OnStateChange?.Invoke(previousState, stateRead.command);
                    }

                    previousState = stateRead.command;
            }

            Stop(); 
        }

        /// <summary>
        /// Reads next state in device 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="prevSate"></param>
        /// <returns></returns>
        protected virtual EmotivState ReadingState(EmotivDevice device)
        {
            return device.Read(); 
        }
        #endregion 
    }
}
