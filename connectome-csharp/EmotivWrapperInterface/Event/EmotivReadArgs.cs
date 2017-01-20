using System;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;

namespace Connectome.Emotiv.Event
{
    /// <summary>
    /// Contains arguments for reader OnRead event. 
    /// </summary>
    public class EmotivReadArgs : IEmotivState
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
        #region Interface
        EmotivCommandType IEmotivState.Command
        {
            get
            {
                return State.Command; 
            }
        }

        float IEmotivState.Power
        {
            get
            {
                return State.Power;
            }
        }

        long IEmotivState.Time
        {
            get
            {
                return State.Time;
            }
        }
        #endregion  
    }
}
