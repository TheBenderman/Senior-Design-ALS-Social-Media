using Connectome.Emotiv.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Interface;
using System;
using Connectome.Unity.Template;

namespace Connectome.Unity.Plugin
{
    /// <summary>
    /// EPOCDevicePlugin contained in a GameObject
    /// </summary>
    public class EPOCDevicePlugin : EmotivDeviceContainer
    {
        #region Inspector Attributes 
        public string Username;
        public string Password;
        public string Profile;
        #endregion
        #region EmotivDeviceContainer Override
        /// <summary>
        /// Creates device 
        /// </summary>
        /// <returns></returns>
        protected override IEmotivDevice CreateDevice()
        {
            return new EPOCEmotivDevice(Username, Password, Profile);
        }
        #endregion
    }
}
