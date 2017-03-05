using Connectome.Unity.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Connectome.Unity.Menu
{
    /// <summary>
    /// Used as a return value for InvokeSelected() that will pop the selection menu. 
    /// </summary>
    public class PopMenu : ISelectionMenu
    {
        public ISelectionMenu InvokeSelected()
        {
            throw new NotImplementedException();
        }

        public void OnPop()
        {
            
        }

        public void OnPush()
        {
            SelectionManager.Instance.Pop();
            SelectionManager.Instance.Pop();
        }

        public void ResetSelection()
        {
            throw new NotImplementedException();
        }

        public void SelectNext(ISelectionHighlighter h)
        {
            throw new NotImplementedException();
        }
    }
}
