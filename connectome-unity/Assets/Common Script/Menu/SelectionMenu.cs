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

        /// <summary>
        /// Invoked after popping into this menu. 
        /// </summary>
        public event Action OnResume;

        /// <summary>
        /// Invoked after pushing a new on top of this. 
        /// </summary>
        public event Action OnPause; 

        #region ISelectionMenu Abstract
        /// <summary>
        /// Invokes currentpointed selection 
        /// </summary>
        /// <returns>Next sub menu, or null to pop</returns>
        public abstract ISelectionMenu InvokeSelected();

        /// <summary>
        /// Moves pointer to next selection 
        /// </summary>
        /// <param name="h"></param>
        public abstract void SelectNext(ISelectionHighlighter h);

        #endregion
        #region ISelectionMenu Virtual
        /// <summary>
        /// Called after menu is popped. 
        /// </summary>
        public virtual void Popped()
        {
            if(OnPop != null)
            {
                OnPop(); 
            }
        }
        /// <summary>
        /// Called after menu is pushed
        /// </summary>
        public virtual void Pushed()
        {
            if (OnPush != null)
            {
                OnPush();
            }
        }

        /// <summary>
        /// Invoked when menu is resumed.
        /// </summary>
        public virtual void Resumed()
        {
            if(OnResume != null)
            {
                OnResume(); 
            }
        }

        /// <summary>
        /// Invoked when menu is paused.
        /// </summary>
        public virtual void Paused()
        {
            if(OnPause != null)
            {
                OnPause();
            }
        }
        #endregion
    }
}
