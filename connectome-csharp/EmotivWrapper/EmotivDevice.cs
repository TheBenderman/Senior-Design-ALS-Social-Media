using Connectome.Emotiv.Interface;
using System;

namespace Connectome.Emotiv.Template
{
    /// <summary>
    /// Abstract Emotiv device that connects and reads states. 
    /// </summary>
    public abstract class EmotivDevice : IEmotivDevice
    {
        #region Private Attributes 
        /// <summary>
        /// Hold connection value. 
        /// </summary>
        private bool Connected;
        #endregion
        #region IEmotivDevice Public Attributes
        public bool IsConnected
        {
            get
            {
                return Connected; 
            }
        }
        #endregion
        #region IEmotivDevice Public Methods
        /// <summary>
        /// Connects device.
        /// </summary>
        /// <returns></returns>
        public bool Connect(out string msg)
        {
            OnConnectAttempt?.Invoke();

            bool suc = ConnectionSetUp(out msg);

            if (suc)
                OnConnectSucceed?.Invoke(msg);
            else
                OnConnectFailed?.Invoke(msg);

            Connected = suc;

            return suc;
        }

        /// <summary>
        /// Disconnects device.
        /// </summary>
        public bool Disconnect(out string msg)
        {
            OnDisconnectAttempt?.Invoke();

            bool suc = DisconnectionSetUp(out msg);

            if (suc)
                OnDisconnectSucceed?.Invoke(msg);
            else
                OnDisconnectFailed?.Invoke(msg);

            Connected = !suc;

            return suc;
        }

        #endregion
        #region IEmotivDevice Events 

        /// <summary>
        /// Invoked before device attempts to connect. 
        /// </summary>
        public event Action OnConnectAttempt;

        /// <summary>
        /// Invoked after device succussfully connects. 
        /// </summary>
        public event Action<string> OnConnectSucceed;

        /// <summary>
        /// Invoked after device fails to connect. 
        /// </summary>
        public event Action<string> OnConnectFailed;

        /// <summary>
        /// Invoked before device attempts to disconnect.
        /// </summary>
        public event Action OnDisconnectAttempt;

        /// <summary>
        /// Invoked after device succussfully disconnects. 
        /// </summary>
        public event Action<string> OnDisconnectSucceed;

        /// <summary>
        /// Invoked after device fails to disconnect. 
        /// </summary>
        public event Action<string> OnDisconnectFailed;


        /// <summary>
        /// Returns current read state or Null state if unable to read a state. 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEmotivState Read(long time)
        {
            try
            {
                IEmotivState state = AttemptRead(time);
            
                return state; 
            }
            catch(Exception e)
            {
                throw new Exception("Unable to read from device", e); 
            }
        }
        #endregion
        #region Abstract
        /// <summary>
        /// Device connection setup  
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected abstract bool ConnectionSetUp(out string msg);

        /// <summary>
        /// Device disconnect setup  
        /// </summary>
        /// <returns></returns>
        protected abstract bool DisconnectionSetUp(out string msg);
       
        /// <summary>
        /// Defines how reader reads a single state from device. 
        /// </summary>
        /// <returns></returns>
        public abstract IEmotivState AttemptRead(long time);
        #endregion
    }
}
