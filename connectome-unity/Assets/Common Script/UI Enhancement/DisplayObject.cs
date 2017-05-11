using Connectome.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// To be displayed be DisplayManager
    /// </summary>
    public abstract class DisplayObject : MonoBehaviour
    {
        public event Action OnDisplay;
        public event Action OnDismiss;

        /// <summary>
        /// Gets Called after object is displayed
        /// </summary>
        public virtual void Displayed()
        {
            if (OnDisplay != null)
            {
                OnDisplay();
            }
        }

        /// <summary>
        /// Gets called after object is dismissed 
        /// </summary>
        public virtual void Dismissed()
        {
            if (OnDismiss != null)
            {
                OnDismiss();
            }
        }
    }
}
