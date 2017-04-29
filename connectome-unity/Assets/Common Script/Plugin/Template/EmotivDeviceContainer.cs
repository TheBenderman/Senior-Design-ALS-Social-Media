using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;


namespace Connectome.Unity.Template
{
    /// <summary>
    /// Wraps an IEmotivDevice in a container used as an EmotivDevicePlugin
    /// </summary>
    public abstract class EmotivDeviceContainer : EmotivDevicePlugin
    {
        #region Public Attribute
        public IEmotivDevice Content;
        #endregion
        #region IEmotivDevice Override
        public override bool IsConnected
        {
            get
            {
                return Content.IsConnected;
            }
        }

        public override event Action OnConnectAttempt
        {
            add
            {
                Content.OnConnectAttempt += value;
            }
            remove
            {
                Content.OnConnectAttempt -= value;
            }
        }

        public override event Action<bool, string> OnConnectAttempted
        {
            add
            {
                Content.OnConnectAttempted += value;
            }
            remove
            {
                Content.OnConnectAttempted -= value;
            }
        }
        public override event Action OnDisconnectAttempt
        {
            add
            {
                Content.OnDisconnectAttempt += value;
            }
            remove
            {
                Content.OnDisconnectAttempt -= value;
            }
        }
        public override event Action<bool, string> OnDisconnectAttempted
        {
            add
            {
                Content.OnDisconnectAttempted += value;
            }
            remove
            {
                Content.OnDisconnectAttempted -= value;
            }
        }

        public override void Connect()
        {
            Content.Connect();
        }

        public override void Disconnect()
        {
            Content.Disconnect();
        }

        public override IEmotivState Read(long time)
        {
             return Content.Read(time);
        }
        #endregion
        #region EmotivDevicePlugin Override
        /// <summary>
        /// Sets up device from creation. 
        /// <see cref="CreateDevice"/>
        /// </summary>
        public override void Setup()
        {
            Content = CreateDevice();
        }
        #endregion
        #region Abstract Method
        /// <summary>
        /// Creates device. 
        /// </summary>
        /// <returns>a created device</returns>
        protected abstract IEmotivDevice CreateDevice();
        #endregion
    }
}

