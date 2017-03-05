using Connectome.Unity.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.Keyboard
{
    /// <summary>
    /// Represent a keyboard 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IKeyboard<T> : ISelectionMenu
    {
        /// <summary>
        /// Invoked when key is clicked 
        /// </summary>
        event Action<T> OnClick;

        /// <summary>
        /// Should be called when key is clicked. This invokes OnClick
        /// </summary>
        /// <param name="go"></param>
        void KeyClicked(T go);

        /// <summary>
        /// Convert key into string 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string KeyToString(T key);
    }
}
