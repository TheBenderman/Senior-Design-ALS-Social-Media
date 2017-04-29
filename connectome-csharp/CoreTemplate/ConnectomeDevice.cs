using Connectome.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Template
{
    public abstract class ConnectomeDevice<T>  : IConnectomeDevice<T>
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
        public void Connect()
        {
            OnConnectAttempt?.Invoke();
            string msg;

            bool suc = ConnectionSetUp(out msg);

            OnConnectAttempted?.Invoke(suc, msg);

            Connected = suc;
        }

        /// <summary>
        /// Disconnects device.
        /// </summary>
        public void Disconnect()
        {
            OnDisconnectAttempt?.Invoke();
            string msg;

            bool suc = DisconnectionSetUp(out msg);

            OnDisconnectAttempted?.Invoke(suc, msg);

            Connected = !suc;
        }

        #endregion
        #region IEmotivDevice Events 

        /// <summary>
        /// Invoked before device attempts to connect. 
        /// </summary>
        public event Action OnConnectAttempt;

        /// <summary>
        /// Invoked after connect attempt ends.
        /// </summary>
        public event Action<bool, string> OnConnectAttempted;

        /// <summary>
        /// Invoked before device attempts to disconnect.
        /// </summary>
        public event Action OnDisconnectAttempt;

        /// <summary>
        /// Invoked after device succussfully disconnects. 
        /// </summary>
        public event Action<string> OnDisconnectSucceed;

        /// <summary>
        /// Invoked after disconnect attempt ends.
        /// </summary>
        public event Action<bool, string> OnDisconnectAttempted;

        /// <summary>
        /// Returns current read state or Null state if unable to read a state. 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public T Read(long time)
        {
            try
            {
                T state = AttemptRead(time);

                return state;
            }
            catch (Exception e)
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
        public abstract T AttemptRead(long time);
        #endregion
    }
}
