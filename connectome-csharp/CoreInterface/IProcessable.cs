using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Interface
{
    /// <summary>
    /// The interface for any object that goes through a process
    /// </summary>
    public interface IProcessable<T>
    {
        #region Methods
        /// <summary>
        /// Check if the connected process has been completed.
        /// </summary>
        bool Process(T t);
        #endregion
    }
}
