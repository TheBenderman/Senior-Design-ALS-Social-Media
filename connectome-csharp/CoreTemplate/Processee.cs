using Connectome.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Template
{
    /// <summary>
    /// Defines a processor
    /// </summary>
    public abstract class Processee<T> : IProcessable<T>, IExecutable<T>
    {
        #region IProcessable Public Methods
        /// <summary>
        /// Logic to determine when to execute the actions tied to this processor.
        /// </summary>
        public bool Process(T t)
        {
            if (IsFulfilled(t))
            {
                OnExecute?.Invoke(t);
                return true;
            }
            return false;
        }
        #endregion
        #region Public Delegates
        /// <summary>
        /// Fires off any methods attached to the OnExecute Action.
        /// </summary>
        public event Action<T> OnExecute;
        #endregion
        #region Private Methods
        #endregion
        #region Abstract
        protected abstract bool IsFulfilled(T t);
        #endregion
    }
}
