using Connectome.Emotiv.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Interface;
using System;
using Connectome.Unity.Template;
using Connectome.Unity.UI;

namespace Connectome.Unity.Plugin
{
    /// <summary>
    /// EPOCDevicePlugin contained in a GameObject
    /// </summary>
    public class EPOCDevicePlugin : EmotivDeviceContainer
    {
        #region Private 
        private static EPOCEmotivDevice DeviceInstance;
        #endregion
        #region Public Inspector Attributes 
       
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
            if (DeviceInstance == null)
            { 
                DeviceInstance = new EPOCEmotivDevice(Username, Password, Profile);

                DeviceInstance.OnConnectAttempted += DebugStatus; 
            }
            return DeviceInstance;
        }
        #endregion
        #region Private Functions 
        private void DebugStatus(bool suc, string msg)
        {
            Debug.Log("EPOC Connection Status: " + suc + ". Reason: " + msg);
        }
        #endregion
    }
}
