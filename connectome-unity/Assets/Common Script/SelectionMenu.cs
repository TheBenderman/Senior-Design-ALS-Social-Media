using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.Menu
{
    /// <summary>
    /// GameObject representing a ISelectionMenu
    /// </summary>
    public abstract class SelectionMenu : MonoBehaviour, ISelectionMenu
    {
        #region ISelectionMenu
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
    }
}
