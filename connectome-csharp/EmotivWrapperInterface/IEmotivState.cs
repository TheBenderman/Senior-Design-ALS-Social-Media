using Connectome.Emotiv.Enum;

namespace Connectome.Emotiv.Interface
{
    //TODO require IEquality & IComparable for comparing. 
    /// <summary>
    /// State to be read from device. 
    /// </summary>
    public interface IEmotivState : ITime
    {
        #region Get Properties
        /// <summary>
        /// Command.
        /// </summary>
        EmotivCommandType Command { get; }
        /// <summary>
        /// Power 0.0 to 1.0.
        /// </summary>
        float Power { get; }
        #endregion
    }
}
