using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.Template
{
    /// <summary>
    /// MonoBehaviour IEmotivInterpreter Templete. Enables interpreters to be set to EmotivDeviceManager through Inspecter
    /// </summary>
    public abstract class EmotivInterpreterPlugin : MonoBehaviour, IEmotivInterpreter
    {
        #region Abstract
        /// <summary>
        /// Interprets. 
        /// </summary>
        public abstract void Interpret();

        /// <summary>
        /// Sets up interpretation.
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Reader"></param>
        public abstract void Setup(IEmotivDevice Device, IEmotivReader Reader);
        #endregion
    }
}
