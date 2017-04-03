using Connectome.Unity.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopSelectionMenu : SelectionMenu
{
    /// <summary>
    /// Used as a scripted in a game object that will pop the menu. 
    /// </summary>
    public override ISelectionMenu InvokeSelected()
    {
        throw new NotImplementedException();
    }

    public override void OnPush()
    {
        SelectionManager.Instance.Pop();
        SelectionManager.Instance.Pop();
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
