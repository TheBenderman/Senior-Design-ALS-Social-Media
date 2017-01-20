using Connectome.Emotiv.Enum;

namespace Connectome.Emotiv.Interface
{
    //TODO require IEquality for comparing. 
    /// <summary>
    /// State to be read from device. 
    /// </summary>
    public interface IEmotivState
    {
        /// <summary>
        /// Command.
        /// </summary>
        EmotivCommandType Command { get; }
        /// <summary>
        /// Power 0.0 to 1.0.
        /// </summary>
        float Power { get; }
        /// <summary>
        /// Time in ms in which the state was read. 
        /// </summary>
        long Time { get; }
    }
}
