using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Interface
{
    public interface IConnectomeDevice<T>
    {
        #region Get Properties 
        bool IsConnected { get; }
        #endregion
        #region Events 
        /// <summary>
        /// Invoked before connecting.
        /// </summary>
        event Action OnConnectAttempt;

        /// <summary>
        /// Invoked after connection attempt ends.
        /// </summary>
        event Action<bool, string> OnConnectAttempted;

        /// <summary>
        /// Invoked before disconnecting.
        /// </summary>
        event Action OnDisconnectAttempt;

        /// <summary>
        /// Invoked after disconnect attempt ends.
        /// </summary>
        event Action<bool, string> OnDisconnectAttempted;

        #endregion
        #region Methods
        /// <summary>
        /// Connects device. Invokes OnConnect. 
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects device. Invokes OnDisconnect. 
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Reads current state from device. 
        /// </summary>
        /// <returns></returns>
        T Read(long time);
        #endregion
    }
}
