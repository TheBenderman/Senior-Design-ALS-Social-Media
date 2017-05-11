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

        public override void Popped()
        {
            SelectionMenu.Popped();
        }

        public override void Pushed()
        {
            SelectionMenu.Pushed();
        }

        public override void Paused()
        {
            SelectionMenu.Paused();
        }

        public override void Resumed()
        {
            SelectionMenu.Resumed();
        }
        public override void Dismissed()
        {
            SelectionMenu.Dismissed();
        }

        public override void Displayed()
        {
            SelectionMenu.Displayed();
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
                Debug.LogError("Container contains no menu.", this);
                return; 
            }

            if(SelectionMenu == this)
            {
                SelectionMenu = null;
                Debug.LogError(name + " menu cannot contain itself", this);
                return;
            }
        }
        #endregion
    }
}