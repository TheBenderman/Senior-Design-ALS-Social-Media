using EmotivWrapperInterface;

namespace EmotivWrapper
{
    public class EmotivState : IEmotivState
    {
        /// <summary>
        /// Command type 
        /// </summary>
        public EmotivStateType command { set; get; }
        /// <summary>
        /// State power 0.0 to 1.0 
        /// </summary>     
        public float power { set; get; }
        /// <summary>
        /// Time of catpured state 
        /// </summary>
        public long time { set; get; }

        /// <summary>
        /// Time, Command, Power
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Time: {0}, Command: {1}, Power: {2}", time,  command.ToString(), power);
        }
    }
}
