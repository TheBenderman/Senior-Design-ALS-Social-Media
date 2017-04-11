using System.Collections;
using System.Collections.Generic;
using Connectome.Unity.UI;
using UnityEngine;

namespace Connectome.Unity.Menu
{

    public class SelectionMenuContainer : SelectionMenu 
    {
        public SelectionMenu SelectionMenu;

        #region ISelectionMenu Interface
        public override ISelectionMenu InvokeSelected()
        {
            return SelectionMenu.InvokeSelected();
        }

        public override void OnPop()
        {
            SelectionMenu.OnPop();
        }

        public override void OnPush()
        {
            SelectionMenu.OnPush();
        }

        public override void ResetSelection()
        {
            SelectionMenu.ResetSelection();
        }

        public override void SelectNext(ISelectionHighlighter h)
        {
            SelectionMenu.SelectNext(h);
        }
        #endregion

        #region Validation 

        private void OnValidate()
        {
            if(SelectionMenu == null)
            {
                Debug.LogError("Container contains no menu nor in children.", this);
            }
        }
        #endregion


    }
}