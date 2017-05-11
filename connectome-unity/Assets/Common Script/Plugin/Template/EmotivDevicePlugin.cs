using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Connectome.Unity.Template
{
    /// <summary>
    /// MonoBehaviour EmotivDevice Templete. Enables device to be set to EmotivDeviceManager through Inspecter.
    /// </summary>
    public abstract class EmotivDevicePlugin : MonoBehaviour, IEmotivDevice
    {
        #region IEmotivDevice Abstract
        public abstract bool IsConnected { get; }
        public abstract int BatteryLevel { get; }
        public abstract int WirelessSignalStrength { get; }

        public abstract event Action OnConnectAttempt;
        public abstract event Action<bool, string> OnConnectAttempted;
        public abstract event Action OnDisconnectAttempt;
        public abstract event Action<bool, string> OnDisconnectAttempted;

        public abstract void Connect();
        public abstract void Disconnect();
        public abstract IEmotivState Read(long time);
        #endregion
        #region Abstract Methods
        public abstract void Setup();
        #endregion
    }
}
