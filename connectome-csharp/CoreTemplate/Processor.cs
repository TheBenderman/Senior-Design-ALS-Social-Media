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
    public abstract class Processor<T,C> : IProcessable<T> {
        public Processor(C parameter)
        {
            c = parameter;
        }
        public C c;
        public abstract IProcessable<C>[] Children { get; set; }

        #region IEmotivProcessor Public Methods
        /// <summary>
        /// Logic to determine when to execute the actions tied to this processor.
        /// </summary>
        public bool Process(T t)
        {
            bool suc = false; 
            for (int i = 0; i < Children.Length; i++)
            {
                if (Children[i].Process(this.c))
                {
                    suc = true;
                    OnChildExecute?.Invoke(t, i);
                }
            }

            return suc; 
        }

        public event Action<T, int> OnChildExecute;
        #endregion
    }
}
