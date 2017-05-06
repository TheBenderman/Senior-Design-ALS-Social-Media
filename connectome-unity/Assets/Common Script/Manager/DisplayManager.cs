using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Enum;
using Connectome.Unity.Plugin;
using System;
using UnityEngine.UI;
using Connectome.Unity.Keyboard;
using UnityEngine.Events;
using Connectome.Unity.Menu;


namespace Connectome.Unity.UI
{
    /// <summary>
    /// Pops up windows such as a Virtual Device. 
    /// </summary>
    public class DisplayManager : MonoBehaviour
    {
        #region Private Attributes
        private const int DEFAULT_DURATION = 5;   
        #endregion
        #region Singleton 
        private static DisplayManager instance;

        public static DisplayManager Instance
        {
            get
            {
                return instance;
            }
        }
        #endregion
        #region Inspecter Attributse
        /// <summary>
        /// Notification component to display notifications 
        /// </summary>
        public Notification Notification;
        #endregion
        #region Unity Built-in
        void Awake()
        {
            instance = this;
        }
        #endregion
        #region Public Static Methods 
        public static void Display(DisplayObject disObj)
        {
            disObj.Displayed();
        }

        public static void Dismiss(DisplayObject disObj)
        {
            disObj.Dismissed();
        }

        public static void AlignDisplay(DisplayObject disObj)
        {
            //move under DisplayManager 
            disObj.transform.SetParent(Instance.transform);

            //Display 
            Display(disObj);
        }

        public static void PushNotification(string msg, int duration = DEFAULT_DURATION)
        {
            Instance.Notification.PushNotification(msg, duration); 
        }

        public static void PushNotification(string msg)
        {
            Instance.Notification.PushNotification(msg, DEFAULT_DURATION);
        }
        #endregion
    }
}

