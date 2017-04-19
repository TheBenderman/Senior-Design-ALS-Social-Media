using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Unity.UI;

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
        /// <returns>A SelectionMenu within invoked button if exist</returns>
        public override ISelectionMenu InvokeSelected()
        {
            if(Pointer == -1)
            {
                return null; 
            }

            Selection[Pointer].onClick.Invoke();

            return Selection[Pointer].GetComponent<SelectionMenu>();
        }

        /// <summary>
        /// Selection next button on the list.  
        /// </summary>
        /// <param name="h"></param>
        public override void SelectNext(ISelectionHighlighter h)
        {
            do
            {
                Pointer++;
                if (ShouldReset())
                {
                    ResetSelection();
                    Pointer++;
                }
            } while (!Selection[Pointer].IsInteractable());//Loop until we reach an object that can be selected
            //Note, I wouldn't recommend having a button selection menu of buttons that are ALL not interactable

            h.Highlight(Selection[Pointer].gameObject);
        }

        /// <summary>
        /// Reset pointer to the beginning. 
        /// </summary>
        public override void ResetSelection()
        {
            Pointer = -1;
        }
        public override void Pushed()
        {
            ResetSelection(); 
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
