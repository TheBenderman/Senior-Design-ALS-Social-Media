using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Template;
using System;


namespace Connectome.Emotiv.Implementation
{
    /// <summary>
    /// Generates random power and random command types between neutral and push.  
    /// </summary>
    public class RandomEmotivDevice : EmotivDevice
    {
        #region Private Attributes
        private Random random;
        #endregion
        #region Constructors
        public RandomEmotivDevice()
        {
            random = new Random();
        }
        #endregion
        #region Overrides
        public override IEmotivState AttemptRead(long time)
        {
           return new EmotivState((random.Next(2) == 1? EmotivCommandType.NULL : EmotivCommandType.PUSH), (float)random.NextDouble(), time);
        }

        protected override bool ConnectionSetUp(out string errorMessage)
        {
            errorMessage = string.Empty; 
            return true; 
        }

        protected override bool DisconnectionSetUp(out string msg)
        {
            msg = string.Empty;
            return true; 
        }
        #endregion
    }
}
