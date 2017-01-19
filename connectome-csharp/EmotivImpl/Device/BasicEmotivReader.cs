using Connectome.Emotiv.Template;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Connectome.Emotiv.Interface;

namespace Connectome.Emotiv.Implementation
{
    public class BasicEmotivReader : EmotivReader
    {
        public bool ShouldReadNull { get; set; }
        public BasicEmotivReader(IEmotivDevice device, bool ShouldReadNull = true ) : base(device)
        {
            this.ShouldReadNull = ShouldReadNull; 
        }

        protected override IEmotivState ReadingState(IEmotivDevice device, long time)
        {
            IEmotivState currentState = device.Read(time);
            if (ShouldReadNull == false )
            {
                while (currentState.Command == Enum.EmotivCommandType.NULL)
                { 
                    InsureMillisecondPassed();
                    currentState = device.Read(time);
                }
            }
            return currentState;
        }
    }
}
