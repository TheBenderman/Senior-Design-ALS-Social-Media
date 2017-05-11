using Connectome.Emotiv.Implementation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Interface;
using System;
using Connectome.Unity.Template;
using Connectome.Core.Interface;

namespace Connectome.Unity.Plugin
{
    /// <summary>
    /// Wraps EmotivBasicReader in an EmotivReaderContainer
    /// </summary>
    public class BasicReaderPlugin : EmotivReaderContainer
    {
        #region Inspector Attributes
        /// <summary>
        /// Allows or ignore null states. 
        /// </summary>
        public bool AllowNull;
        #endregion
        #region EmotivReaderContainer Override
        /// <summary>
        /// Create device 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        protected override IEmotivReader CreateReader(IEmotivDevice device)
        {
            return new BasicEmotivReader(device, AllowNull);
        }
        #endregion
    }
}
