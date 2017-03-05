using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Emotiv.Interface
{
    /// <summary>
    /// Interpreters every time Interpret is called...
    /// </summary>
    public interface IEmotivInterpreter
    {
        /// <summary>
        /// Sets up Interpreter
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Reader"></param>
        void Setup(IEmotivDevice Device, IEmotivReader Reader);

        /// <summary>
        /// Interpreter collected data 
        /// </summary>
        void Interpret(); 
    }
}
