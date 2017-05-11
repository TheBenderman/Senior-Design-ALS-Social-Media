using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Unity.UI;
using System;

namespace Connectome.Unity.Menu
{
    /// <summary>
    /// Represent a selection menu that can iterate an invokable selection 
    /// </summary>
    public interface ISelectionMenu
    {
        event Action OnPush;
        event Action OnPop;
        event Action OnPause;
        event Action OnResume; 

        /// <summary>
        /// Invokes current pointed selection 
        /// </summary>
        /// <returns>Next sub menu, or null to pop</returns>
        ISelectionMenu InvokeSelected();

        /// <summary>
        /// Moves pointer to next selection 
        /// </summary>
        /// <param name="h"></param>
        void SelectNext(ISelectionHighlighter h);

        /// <summary>
        /// Called after menu is popped
        /// </summary>
        void Popped();

        /// <summary>
        /// Called after menu is pushed
        /// </summary>
        void Pushed();

        /// <summary>
        /// Invoked when another menu is pushed on top if this.
        /// </summary>
        void Paused();

        /// <summary>
        /// Invoked when another menu pops into this. 
        /// </summary>
        void Resumed();
    }
}
