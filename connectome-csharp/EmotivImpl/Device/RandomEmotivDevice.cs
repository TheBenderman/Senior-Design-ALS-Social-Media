using EmotivWrapper.Core;
using System;
using EmotivWrapper;
using EmotivWrapperInterface;

namespace EmotivImpl
{
    public class RandomEmotivDevice : EmotivDevice
    {
        private Random random;

        public RandomEmotivDevice()
        {
            random = new Random();
        }

        public override IEmotivState Read()
        {
            return new EmotivState() { command = (random.Next(2) == 1? EmotivStateType.NEUTRAL : EmotivStateType.PUSH), power = (float)random.NextDouble(), time =0  };

        }

        protected override bool ConnectionSetUp(out string errorMessage)
        {
            errorMessage = string.Empty; 
            return true; 
        }

        protected override bool DisconnectionSetUp()
        {
            return true; 
        }
    }
}
