using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Event;
using System;

namespace Connectome.Unity.Template
{
    /// <summary>
    /// Wraps an IEmotivReader in a container used as a EmotivReaderPlugin
    /// </summary>
    public abstract class EmotivReaderContainer : EmotivReaderPlugin
    {
        #region Public Attribute
        public IEmotivReader Content;
        #endregion
        #region IEmotivReader Override
        public override IEmotivDevice Device
        {
            get
            {
                return Content.Device;
            }

            set
            {
                Content.Device = value;
            }
        }

        public override bool IsReading
        {
            get
            {
                return Content.IsReading;
            }
        }

        public override event Action<Exception> ExceptionHandler
        {
            add
            {
                Content.ExceptionHandler += value;
            }
            remove
            {
                Content.ExceptionHandler -= value;
            }
        }

        public override event Action<EmotivCommandType?, EmotivCommandType> OnCommandChange
        {
            add
            {
                Content.OnCommandChange += value;
            }
            remove
            {
                Content.OnCommandChange -= value;
            }
        }
        public override event Action<EmotivReadArgs> OnRead
        {
            add
            {
                Content.OnRead += value;
            }
            remove
            {
                Content.OnRead -= value;
            }
        }

        public override event Action OnStart
        {
            add
            {
                Content.OnStart += value;
            }
            remove
            {
                Content.OnStart -= value;
            }
        }

        public override event Action<string> OnStop
        {
            add
            {
                Content.OnStop += value;
            }
            remove
            {
                Content.OnStop -= value;
            }
        }

        public override void PlugDevice(IEmotivDevice Device)
        {
            Content.PlugDevice(Device);
        }

        public override void Start()
        {
            Content.Start();
        }

        public override void Stop()
        {
            if (Content != null)
            {
                Content.Stop();
            }
        }
        #endregion
        #region EmotivReaderPlugin Override
        /// <summary>
        /// Sets up created device 
        /// </summary>
        /// <see cref="CreateReader(IEmotivDevice)"/>
        /// <param name="device"></param>
        public override void SetUp(IEmotivDevice device)
        {
            Content = CreateReader(device);
        }
        #endregion
        #region Abstract Method
        /// <summary>
        /// Creates device 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        protected abstract IEmotivReader CreateReader(IEmotivDevice device);
        #endregion
    }
}
