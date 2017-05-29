using Connectome.Emotiv.Common;
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
        private Random Random;
        private EmotivCommandType[] States;
        float MinPower;
        float MaxPower;
        #endregion
        #region Constructors
        public RandomEmotivDevice()
        {
            Random = new Random();
            States = new EmotivCommandType[]{ EmotivCommandType.NEUTRAL, EmotivCommandType.PUSH};
            MinPower = 0;
            MaxPower = 1;
        }

        public RandomEmotivDevice(float minPower, float maxPower) : this()
        {
            MinPower = minPower;
            MaxPower = maxPower; 
        }

        public RandomEmotivDevice(float minPower, float maxPower, params EmotivCommandType[] states) : this(minPower, maxPower)
        {
            States = states;
        }

        public RandomEmotivDevice(params EmotivCommandType[] states) : this()
        {
            States = states;
        }
        #endregion
        #region Overrides
        public override IEmotivState AttemptRead(long time)
        {
           return new EmotivState(States[Random.Next(States.Length)], MinPower +((float)Random.NextDouble() * (MaxPower - MinPower)), time);
        }

        protected override bool ConnectionSetUp(out string errorMessage)
        {
            errorMessage = string.Empty;
            IsConnected = true; 
            return true; 
        }

        protected override bool DisconnectionSetUp(out string msg)
        {
            msg = string.Empty;
            IsConnected = false; 
            return true; 
        }
        #endregion
    }
}
