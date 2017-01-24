using System;

namespace Connectome.Emotiv.Interface
{
    /// <summary>
    /// A Device that can read states. 
    /// </summary>
    public interface IEmotivDevice
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
        /// <param name="msg"></param>
        /// <returns></returns>
        bool Connect(out string msg);

        /// <summary>
        /// Disconnects device. Invokes OnDisconnect. 
        /// </summary>
        /// <param name="msg"></param>
        bool Disconnect(out string msg);

        /// <summary>
        /// Reads current state from device. 
        /// </summary>
        /// <returns></returns>
        IEmotivState Read(long time);
        #endregion
    }
}
