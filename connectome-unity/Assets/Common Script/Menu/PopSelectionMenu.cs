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
    [Range(1, 10)]
    public int PopIndex = 1;

   /// <summary>
   /// Pops itself and then the number of times desired. 
   /// </summary>
    public override void OnPush()
    {
        for(int i = 0; i < PopIndex + 1; i++)
        {
            SelectionManager.Instance.Pop();
        }
    }

    #region Should not be called.
    public override ISelectionMenu InvokeSelected()
    {
        throw new NotImplementedException();
    }
    public override void ResetSelection()
    {
        throw new NotImplementedException();
    }

    public override void SelectNext(ISelectionHighlighter h)
    {
        throw new NotImplementedException();
    }
    #endregion
}
