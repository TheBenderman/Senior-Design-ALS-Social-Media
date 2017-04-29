using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;
using UnityEngine;
using Connectome.Core.Interface;

namespace Connectome.Unity.Template
{
    /// <summary>
    /// MonoBehaviour IEmotivReader Templete. Enables readers to be set to EmotivDeviceManager through Inspecter
    /// </summary>
    public abstract class EmotivReaderPlugin : MonoBehaviour, IEmotivReader
    {
        #region IEmotivReader Abstract
        public abstract IConnectomeDevice<IEmotivState> Device { get; set; }
        public abstract bool IsReading { get; }

        public abstract event Action<Exception> ExceptionHandler;
        public abstract event Action<IEmotivState> OnRead;
        public abstract event Action OnStart;
        public abstract event Action<string> OnStop;

        public abstract void PlugDevice(IConnectomeDevice<IEmotivState> Device);
        public abstract void StartReading();
        public abstract void StopReading();
        #endregion
        #region Abstarct
        /// <summary>
        /// Sets up a reader.
        /// </summary>
        /// <param name="device"></param>
        public abstract void SetUp(IEmotivDevice device);
        #endregion
    }
}
