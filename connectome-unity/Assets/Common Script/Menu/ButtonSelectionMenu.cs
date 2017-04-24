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
        /// Selection next button on the list. Ignores noninteractable buttons. 
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
            } while (!Selection[Pointer].IsInteractable());
            
            h.Highlight(Selection[Pointer].gameObject);
        }

        /// <summary>
        /// Reset pointer to the beginning. 
        /// </summary>
        public void ResetSelection()
        {
            Pointer = -1;
        }
        public override void Pushed()
        {
            base.Pushed();
            ResetSelection(); 
        }

        public override void Resumed()
        {
            base.Resumed();
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

        private void OnValidate()
        {
            ///Check if Selection is set 
            if(Selection == null || Selection.Length == 0)
            {
                Debug.LogError(name + "'s Selection is empty", this);
                return; 
            }

            ///Check of a selection is null
            foreach (Button butt in Selection)
            {
                 if(butt == null)
                {
                    Debug.LogError("Some elements in Selection are null", this);
                    return; 
                }
            }
            ///Check if at least 1 button is interabtable 
            bool interactabe = false; 
            foreach(Button butt in Selection)
            {
                interactabe = interactabe | butt.IsInteractable(); 
            }

            if(interactabe == false)
            {
                Debug.LogError("All " + name +"'s selection are not interactable", this);
            }
        }
    }
}
