using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Interface
{
    /// <summary>
    /// Simple, Timed interface. Used for Timeline. 
    /// <see cref="ITimeline{T}"/>
    /// </summary>
    public interface ITime
    {
        /// <summary>
        /// Holds time
        /// </summary>
        long Time { get; }
    }
}
