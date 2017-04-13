using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Unity.UI;

namespace Connectome.Unity.Menu
{
    /// <summary>
    /// GameObject representing a ISelectionMenu
    /// </summary>
    public abstract class SelectionMenu : DisplayObject, ISelectionMenu
    {
        /// <summary>
        /// Invoked when menu is pushed into stack. 
        /// </summary>
        public event Action OnPush;

        /// <summary>
        /// Invoked when menu is popped from stack. 
        /// </summary>
        public event Action OnPop;

        #region ISelectionMenu Abstract
        /// <summary>
        /// Invokes current pointed selection 
        /// </summary>
        /// <returns>Next sub menu, or null to pop</returns>
        public abstract ISelectionMenu InvokeSelected();

        /// <summary>
        /// Resets selection pointer
        /// </summary>
        public abstract void ResetSelection();

        /// <summary>
        /// Moves pointer to next selection 
        /// </summary>
        /// <param name="h"></param>
        public abstract void SelectNext(ISelectionHighlighter h);

        #endregion
        #region ISelectionMenu Virtual
        /// <summary>
        /// Called after menu is popped
        /// </summary>
        public virtual void Popped()
        {
            if(OnPush != null)
            {
                OnPush(); 
            }
        }
        /// <summary>
        /// Called after menu is pushed
        /// </summary>
        public virtual void Pushed()
        {
            if (OnPop != null)
            {
                OnPop();
            }
        }
        #endregion
    }
}
