using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.Menu
{
    /// <summary>
    /// A Selection menu of buttons. If a button contains a ButtonSelectionMenu component, the menu will appended.
    /// </summary>
    public class ButtonSelectionMenu : SelectionMenu
    {
        #region Private Attributes
        /// <summary>
        /// Currently Pointed 
        /// </summary>
        private int Pointer;
        #endregion
        #region Inspecter Attributes
        /// <summary>
        /// List of buttons 
        /// </summary>
        public Button[] Selection;
        #endregion
        #region SelectionMenu Overrides
        /// <summary>
        /// Invoked button at pointer. 
        /// </summary>
        /// <returns>A ButtonSelectionMenu within invoked button if exist</returns>
        public override ISelectionMenu InvokeSelected()
        {
            Selection[Pointer].onClick.Invoke();

            return Selection[Pointer].GetComponent<ButtonSelectionMenu>();
        }

        /// <summary>
        /// Selection next button on the list.  
        /// </summary>
        /// <param name="h"></param>
        public override void SelectNext(ISelectionHighlighter h)
        {
            Pointer++;
            if (ShouldReset())
            {
                ResetSelection();
                Pointer++;
            }

            h.Highlight(Selection[Pointer].gameObject);
        }

        /// <summary>
        /// Reset pointer to the beginning. 
        /// </summary>
        public override void ResetSelection()
        {
            Pointer = -1;
        }
        #endregion
        #region Virtual Methods
        /// <summary>
        /// True if pointer reachs the end of the list. 
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldReset()
        {
            return Pointer >= Selection.Length;
        }
        #endregion
    }
}
