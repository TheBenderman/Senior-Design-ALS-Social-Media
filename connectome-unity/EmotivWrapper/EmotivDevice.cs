using EmotivWrapperInterface;
using System;

namespace EmotivWrapper.Core
{
    /// <summary>
    /// Emotiv device that connects and reads states. 
    /// </summary>
    public abstract class EmotivDevice : IEmotivDevice
    {
        #region delegates
        /// <summary>
        /// Gets invoked after device is succussfully connected. 
        /// </summary>
        public Action OnConnect;
       
        /// <summary>
        /// Gets invoked after device failed to connect. 
        /// </summary>
        public Action OnFailedConnect;

        /// <summary>
        /// Gets invoked after device is succussfully disconnected. 
        /// </summary>
        public Action OnDisconnect;

        /// <summary>
        /// Gets invoked after device failed to disconnect. 
        /// </summary>
        public Action OnFailedDisconnect;
        //TODO logger 
        #endregion

        #region methods connect/disconnect 
        /// <summary>
        /// Connects device by calling it's setup 
        /// </summary>
        /// <returns></returns>
        public bool Connect(out string errorMsg)
        {
            bool suc = ConnectionSetUp(out errorMsg);

            //TODO log msg 
            if (suc)
                OnConnect?.Invoke();
            else
                OnFailedConnect?.Invoke(); 
            
            return suc; 
        }
        
        /// <summary>
        /// Calls Disconnection and invokes OnDisconnect
        /// </summary>
        public void Disconnect()
        {
           bool suc =  DisconnectionSetUp();

            if (suc)
                OnDisconnect?.Invoke();
            else
                OnFailedDisconnect?.Invoke(); 
        }
        #endregion

        #region abstract
        /// <summary>
        /// Connect device 
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        protected abstract bool ConnectionSetUp(out string errorMessage);

        /// <summary>
        /// Disconnect device 
        /// </summary>
        /// <returns></returns>
        protected abstract bool DisconnectionSetUp();
       
        /// <summary>
        /// Read current state from device 
        /// </summary>
        /// <returns></returns>
        public abstract IEmotivState Read();

        #endregion
        #region interface satisfaction 
        bool IEmotivDevice.ConnectionSetUp(out string errorMsg)
        {
            return ConnectionSetUp(out errorMsg); 
        }

        bool IEmotivDevice.DisconnectionSetUp()
        {
          return DisconnectionSetUp(); 
        }
        #endregion
    }
}
