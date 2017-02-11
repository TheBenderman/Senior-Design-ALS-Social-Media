using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// A SelectableButton is the bottom level of selectable objects. 
/// When selected, some action will occur - whatever is attached to the button onClick.
/// </summary>
public class SelectableButton : SelectableObject
{
    /// <summary>
    /// The reference to the button.
    /// We need to manually set this in the editor to guarantee it isn't null, due to Script Execution Order.
    /// </summary>
    public Button button;

    public override void TriggerClick(SelectionManager manager)
    {
        button.onClick.Invoke();
    }

    public override void Select(SelectableObject previous)
    {
        button.Select();//Highlight the button.
        //We don't need to do anything to previous, unless we want to set it to a different color in the future.
    }

    public override Color CurrentColor
    {
        get
        {
            return button.colors.highlightedColor;
        }

        set
        {
            ColorBlock colors = button.colors;
            colors.highlightedColor = value;//Only dealing with highlighted color in the buttons for now.
            button.colors = colors;
        }
    }
}
