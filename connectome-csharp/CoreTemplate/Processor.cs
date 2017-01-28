using Connectome.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Template
{
    /// <summary>
    /// A processor is a process that has children. The children can be processors or processees.
    /// Processors use type T to determine if they are successful, and push Type C to their children.
    /// </summary>
    public abstract class Processor<T,C> : IProcessable<T> {
        #region Constructor
        public Processor(C parameter)
        {
            c = parameter;
        }
        #endregion
        #region Public Attributes
        /// <summary>
        /// The stored obj for the children to process.
        /// </summary>
        public C c;
        #endregion
        #region Abstract Methods
        /// <summary>
        /// Defines the children of this processor
        /// </summary>
        public abstract IProcessable<C>[] Children { get; set; }
        #endregion
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
        /// <summary>
        /// Any events that fire when the appropriate child is successful
        /// </summary>
        public event Action<T, int> OnChildExecute;
        #endregion
    }
}
