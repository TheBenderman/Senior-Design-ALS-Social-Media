
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.UI
{
    public class DisplayPanel : DisplayObject
    {
        public override void Displayed()
        {
            base.Displayed();
            //TODO stretch to parent 
            gameObject.SetActive(true); 
        }

        public override void Dismissed()
        {
            base.Dismissed();
            gameObject.SetActive(false);
        }
    }
}
