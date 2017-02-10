using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectableObject : MonoBehaviour {

    public abstract void TriggerClick(SelectionManager manager);
    public abstract void Select(SelectableObject previous);
    public abstract ColorBlock GetColor();
    public virtual void ResetColor() { }
}
