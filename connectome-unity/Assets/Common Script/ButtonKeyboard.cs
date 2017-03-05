using Connectome.Unity.Menu;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.Keyboard
{
    /// <summary>
    /// A keyboard of buttons where the string value for every key is within it's child text. 
    /// </summary>
    [RequireComponent(typeof(ButtonSelectionMenu))]
    public class ButtonKeyboard : ConnectomeKeyboard
    {
        #region  ConnectomeKeyboard Override
        /// <summary>
        /// Gets string from child text 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override string KeyToString(GameObject key)
        {
            return key.GetComponentInChildren<Text>().text;
        }
        #endregion
    }
}
