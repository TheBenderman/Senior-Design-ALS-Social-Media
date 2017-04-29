using Connectome.Core.Interface;
using Connectome.Core.Template;
using Connectome.Emotiv.Interface;
using System;
using System.Diagnostics;
using System.Threading;

namespace Connectome.Emotiv.Template
{
    /// <summary>
    /// Basic device reader that read a state every millisecond. 
    /// </summary>
    public abstract class EmotivReader : ConnectomeReader<IEmotivState>, IEmotivReader
    {
        public EmotivReader(IEmotivDevice device) : base(device)
        {
        }
    }
}
