using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.Menu
{
    /// <summary>
    /// A wrapper for the various items in the scene that can be cycled through
    /// via SelectionManager.
    /// </summary>
    public abstract class SelectableObject : SelectionMenu
    {
        /// <summary>
        /// The list of objects that will get pushed onto the stack when this object is selected.
        /// </summary>
        [Tooltip("The next set of SelectableObjects that are pushed on the SelectionManager stack when this object is clicked. NOTE: if this loads a prefab, the prefab must contain the next set(As you can't set the list here in that case).")]
        public SelectableObject[] SelectableList;
       
        /// <summary>
        /// What happens when this object is highlighted.
        /// </summary>
        /// <param name="previous"></param>
        public abstract void Select();
        /// <summary>
        /// The wrapper for the color, as different UI components handle color differently.
        /// </summary>
        public abstract Color CurrentColor { get; set; }
        /// <summary>
        /// Some UI components have to manually reset the button color.
        /// </summary>
        public virtual void ResetColor() { }

        /// <summary>
        /// Selects next element. 
        /// </summary>
        /// <param name="h"></param>
        public override void SelectNext(ISelectionHighlighter h)
        {
            Select();
        }
    }
}
