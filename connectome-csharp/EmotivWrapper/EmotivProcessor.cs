using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Connectome.Emotiv.Interface;

namespace Connectome.Emotiv.Template
{
    /// <summary>
    /// Defines a processor
    /// </summary>
    public abstract class EmotivProcessor : IEmotivProcessor
    {
        #region IEmotivProcessor Public Methods
        public void CheckProgress()
        {
            if (IsFulfilled())
            {

            }
        }
        #endregion
        #region Abstract
        protected abstract bool IsFulfilled();
        #endregion
    }
}
