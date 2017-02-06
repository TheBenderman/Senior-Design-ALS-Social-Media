
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Enum;

namespace Connectome.Emotiv.Common
{
    /// <summary>
    /// EmotivState that holds command, power, and time. 
    /// </summary>
    public class EmotivState : IEmotivState, ITime
    {
        #region Constroctor  
        /// <summary>
        /// Creates an emotiv state
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="pwr"></param>
        /// <param name="time"></param>
        public EmotivState(EmotivCommandType cmd, float pwr, long time)
        {
            Command = cmd;
            Power = pwr;
            Time = time; 
        }
        #endregion 
        #region IEmotivState Interface 
        /// <summary>
        /// Command type 
        /// </summary>
        public EmotivCommandType Command { get; }
        /// <summary>
        /// State power 0.0 to 1.0 
        /// </summary>     
        public float Power { get; }
        #endregion
        #region ITime Interface 
        /// <summary>
        /// Time of catpured state 
        /// </summary>
        public long Time { get; }
        #endregion
        #region Object Override 
        /// <summary>
        /// returns: "Time: {Time}, Command: {Command}, Power: {Power}"
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Time: {0}, Command: {1}, Power: {2}", Time,  Command.ToString(), Power);
        }
        #endregion
    }
}
