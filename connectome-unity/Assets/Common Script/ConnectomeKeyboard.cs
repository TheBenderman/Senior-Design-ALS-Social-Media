using Connectome.Unity.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.Keyboard
{
    /// <summary>
    /// A keyboard that invoke OnClick for every key press. Each key can be converted to a string 
    /// </summary>
    public abstract class ConnectomeKeyboard : SelectionMenu, IKeyboard<GameObject>
    {
        #region Private Attributes
        /// <summary>
        /// Menu the keyboard wraps around 
        /// </summary>
        public SelectionMenu Menu;

        /// <summary>
        /// Onvoked when a keyboard key is pressed 
        /// </summary>
        public event Action<GameObject> OnClick;

        #endregion
        #region Unity Overrides
        /// <summary>
        /// Sets a menu from gameobject is one was not set 
        /// </summary>
        private void Awake()
        {
            if (Menu == null)
            {
                Menu = GetComponent<SelectionMenu>();
            }
        }
        #endregion
        #region SelectionMenu Overrides
        /// <summary>
        /// wrap menu 
        /// </summary>
        /// <returns></returns>
        public override ISelectionMenu InvokeSelected()
        {
            return Menu.InvokeSelected();
        }

        /// <summary>
        /// wrap menu 
        /// </summary>
        /// <returns></returns>
        public override void ResetSelection()
        {
            Menu.ResetSelection();
        }

        /// <summary>
        /// wrap menu 
        /// </summary>
        /// <returns></returns>
        public override void SelectNext(ISelectionHighlighter h)
        {
            Menu.SelectNext(h);
        }
        #endregion
        #region IKeyboard<GameObject> Overrides
        /// <summary>
        /// Invokes OnClick
        /// </summary>
        /// <param name="key"></param>
        public virtual void KeyClicked(GameObject key)
        {
            if (OnClick != null)
            {
                OnClick.Invoke(key);
            }
        }

        /// <summary>
        /// Converts key into string 
        /// </summary>
        /// <returns></returns>
        public virtual string KeyToString(GameObject key)
        {
            return key.name;
        }

        #endregion
        #region Validation
        /// <summary>
        /// Insures a SelectionMenu is set 
        /// </summary>
        private void OnValidate()
        {
            if (Menu == null && GetComponent<SelectionMenu>() == null)
            {
                Debug.LogError(name + " does not have a SelectionMenu script attached nor amenu object set");
            }
        }
        #endregion
    }
}
