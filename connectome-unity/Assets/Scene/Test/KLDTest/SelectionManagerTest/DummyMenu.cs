using Connectome.Unity.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Connectome.KLD.Test
{

    public class DummyMenu : ButtonSelectionMenu
    {




        

        public override void SelectNext(ISelectionHighlighter h)
        {
            
        }


        private void Start()
        {
            var menu = GetComponent<SelectionMenu>();

            if (menu == null)
            {
                Debug.Log(name + " doesn't have a menu.");
            }
        }
    }
}
