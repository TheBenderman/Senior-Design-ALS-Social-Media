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
        /// Invoked after successfully connecting.
        /// </summary>
        event Action<string> OnConnectSucceed;
        /// <summary>
        /// Invoked after faling to  connect.
        /// </summary>
        event Action<string> OnConnectFailed;
        /// <summary>
        /// Invoked after successfully disconnecting.
        /// </summary>
        event Action<string> OnDisconnectSucceed;
        /// <summary>
        /// Invoked after faling to  disconnect.
        /// </summary>
        event Action<string> OnDisconnectFailed;
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
