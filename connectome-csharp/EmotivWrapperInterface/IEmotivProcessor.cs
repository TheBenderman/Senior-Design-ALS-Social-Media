using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Emotiv.Interface
{
    /// <summary>
    /// Defines an Emotiv Processor
    /// </summary>
    public interface IEmotivProcessor
    {
        #region Methods
        /// <summary>
        /// Check if the connected process has been completed.
        /// </summary>
        void CheckProgress();
        #endregion
    }
}
