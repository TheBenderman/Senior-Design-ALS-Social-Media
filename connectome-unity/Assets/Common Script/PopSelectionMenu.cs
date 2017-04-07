using Connectome.Unity.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Unity.UI;

public class PopSelectionMenu : SelectionMenu
{
    /// <summary>
    /// The number of times to pop
    /// </summary>
    [Tooltip("The number of times the selection manager will pop")]
    [Range(2, int.MaxValue)]
    public int PopIndex;

    /// <summary>
    /// Used as a scripted in a game object that will pop the menu. 
    /// </summary>
    public override ISelectionMenu InvokeSelected()
    {
        throw new NotImplementedException();
    }

    public override void OnPush()
    {
        for(int i = 0; i < PopIndex; i++)
        {
            SelectionManager.Instance.Pop();
        }
    }

    public override void ResetSelection()
    {
        throw new NotImplementedException();
    }

    public override void SelectNext(ISelectionHighlighter h)
    {
        throw new NotImplementedException();
    }
}
