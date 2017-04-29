using Connectome.Core.Interface;
using Connectome.Core.Template;
using Connectome.Emotiv.Interface;
using System;

namespace Connectome.Emotiv.Template
{
    /// <summary>
    /// Abstract Emotiv device that connects and reads states. 
    /// </summary>
    public abstract class EmotivDevice : ConnectomeDevice<IEmotivState>, IEmotivDevice
    {
        
    }
}
