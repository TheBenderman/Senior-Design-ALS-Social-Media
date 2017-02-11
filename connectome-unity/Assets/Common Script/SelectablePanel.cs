using Connectome.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// A SelectablePanel is a SelectableObject that contains SelectableObjects.
/// </summary>
public class SelectablePanel : SelectableObject
{
    /// <summary>
    /// The list of objects that will get pushed onto the stack when this object is selected.
    /// </summary>
    public SelectableObject[] SelectableList;
    /// <summary>
    /// Used to change the color when this object is selected.
    /// </summary>
    public Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    public override Color CurrentColor
    {
        get
        {
            return image.color;
        }

        set
        {
            image.color = value;
        }
    }

    public override void Select(SelectableObject previous)
    {
        CurrentColor = SelectionManager.Instance.DefaultSelectColor;//Color change is handled in the Button flicker, but do it here in case.
        if(previous != null)
        {
            previous.ResetColor();//We have to manually change the color back to the unselected one for Images.
        }
    }

    public override void TriggerClick(SelectionManager manager)
    {
        manager.PushSelections(SelectableList);
    }

    /// <summary>
    /// Used for "deselecting" a panel.
    /// </summary>
    public override void ResetColor()
    {
        image.color = SelectionManager.Instance.DefaultUnselectColor;
    }
}
