using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Enum;
using Connectome.Unity.Template;

namespace Connectome.Unity.Plugin
{
    /// <summary>
    /// Wraps Random device as a GameObject
    /// </summary>
    public class RandomDevicePlugin : EmotivDeviceContainer
    {
        #region Public Inspecter Attributes 
        public EmotivCommandType[] States;
        public float MinPower;
        public float MaxPower;
        #endregion
        #region EmotivDeviceContainer Override
        /// <summary>
        /// Create device 
        /// </summary>
        /// <returns></returns>
        protected override IEmotivDevice CreateDevice()
        {
            return new RandomEmotivDevice(MinPower, MaxPower, States);
        }
        #endregion
    }
}
