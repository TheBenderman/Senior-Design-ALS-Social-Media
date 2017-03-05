using System;
using System.Collections.Generic;
using System.Linq;
using Connectome.Emotiv.Event; 


namespace Connectome.Emotiv.Interface
{
    public interface IEmotivReaderTracker
    {
        void Track(EmotivReadArgs args); 
    }
}
