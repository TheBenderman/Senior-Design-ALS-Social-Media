using System.Collections;
using System.Collections.Generic;
using Connectome.Unity.UI;
using UnityEngine;

namespace Connectome.Unity.Menu
{

    public class SelectionMenuContainer : DisplayObject, ISelectionMenu 
    {
        public SelectionMenu SelectionMenu;

        #region ISelectionMenu Interface
        public ISelectionMenu InvokeSelected()
        {
            return SelectionMenu.InvokeSelected();
        }

        public void OnPop()
        {
            SelectionMenu.OnPop();
        }

        public void OnPush()
        {
            SelectionMenu.OnPush();
        }

        public void ResetSelection()
        {
            SelectionMenu.ResetSelection();
        }

        public void SelectNext(ISelectionHighlighter h)
        {
            SelectionMenu.SelectNext(h);
        }
        #endregion

        #region Validation 

        private void OnValidate()
        {
            if(SelectionMenu == null)
            {
                SelectionMenu = GetComponent<SelectionMenu>(); 
                if(SelectionMenu == null)
                {
                    Debug.LogError("Container contains no menu nor in children.", this);
                }
            }
        }
        #endregion


    }
}