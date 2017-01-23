using System;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;

namespace Connectome.Emotiv.Event
{
    /// <summary>
    /// Contains arguments for reader OnRead event. 
    /// </summary>
    public class EmotivReadArgs
    {
        #region Properties
        /// <summary>
        /// read state
        /// </summary>
        public IEmotivState State { get; set; }
        #endregion
        #region Constructor
        public EmotivReadArgs(IEmotivState state)
        {
            this.State = state;
        }
        #endregion  
    }
}
