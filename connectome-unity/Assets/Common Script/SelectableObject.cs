using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// A wrapper for the various items in the scene that can be cycled through
/// via SelectionManager.
/// </summary>
public abstract class SelectableObject : MonoBehaviour {
    /// <summary>
    /// What happens when the user has pushed enough to "click" this object.
    /// </summary>
    /// <param name="manager"></param>
    public abstract void TriggerClick(SelectionManager manager);
    /// <summary>
    /// What happens when this object is highlighted.
    /// </summary>
    /// <param name="previous"></param>
    public abstract void Select(SelectableObject previous);
    /// <summary>
    /// The wrapper for the color, as different UI components handle color differently.
    /// </summary>
    public abstract Color CurrentColor { get; set; }
    /// <summary>
    /// Some UI components have to manually reset the button color.
    /// </summary>
    public virtual void ResetColor() { }
}
