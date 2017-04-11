
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// A panel that allows user to input emotiv credintials 
    /// </summary>
    public class EmotivLoginDisplayPanel : DisplayPanel
    {
        #region Public Inspecter Attributes 
        public Text ErrorMessage; 

        public InputField UsernameInput;
        public InputField PasswordInput;
        public InputField ProfileInput;
        #endregion
        #region Public Events  
        public event Action<IEmotivDevice> OnLoginedIn;
        #endregion
        #region Public Methods  
        /// <summary>
        /// Attempt to create a device using given credintials 
        /// </summary>
        public void Login()
        {
            IEmotivDevice device = new EPOCEmotivDevice(UsernameInput.text, PasswordInput.text, ProfileInput.text);
            
            string msg = "";
            bool suc = true;

            device.OnConnectAttempted += (b, m) => {msg=m; suc = b; };

            device.Connect(); 

            if (suc) 
            {
                if (OnLoginedIn != null)
                {
                    OnLoginedIn(device);
                }

                Dismissed(); 
            }
            else
            {
                ErrorMessage.text = msg; 
            }
        }
        #endregion
    }
}

