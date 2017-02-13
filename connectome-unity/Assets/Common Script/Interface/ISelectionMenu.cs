using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.Menu
{
    /// <summary>
    /// Represent a selection menu that can iterate an invokable selection 
    /// </summary>
    public interface ISelectionMenu
    {
        /// <summary>
        /// Invokes current pointed selection 
        /// </summary>
        /// <returns>Next sub menu, or null to pop</returns>
        ISelectionMenu InvokeSelected();

        /// <summary>
        /// Resets selection pointer
        /// </summary>
        void ResetSelection();

        /// <summary>
        /// Moves pointer to next selection 
        /// </summary>
        /// <param name="h"></param>
        void SelectNext(ISelectionHighlighter h);
    }
}
