using Connectome.Unity.Menu;
using Connectome.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.Keyboard
{
    /// <summary>
    /// Managers Keyboard's clicks and holds a string representatino of the output
    /// </summary>
    public class KeyboardManager : MonoBehaviour
    {
        #region Public Attributes
        public static KeyboardManager Instance;
        /// <summary>
        /// The loaded Keyboard prefab
        /// </summary>
        [HideInInspector]
        public KeyboardTemplate KeyboardGameObject;

        #endregion
        #region Private Attributes
        /// <summary>
        /// TODO? This value will eventually come from some fixed spot, based on the social media we are using.
        /// </summary>
        private int TextLimit;
        #endregion
        #region Unity Overrides
        /// <summary>
        /// Connects keyboard onclick event to manager.
        /// </summary>
        private void Awake()
        {
            Instance = this;
            SetKeyboard(UserSettings.CurrentKeyboardName);//Guarantee a keyboard is set 
        }
        #endregion
        #region Memory

        public void SetKeyboard(string keyboardtype)
        {
            //Change to making the keyboard appear?
            KeyboardGameObject = (Instantiate(Resources.Load(keyboardtype), this.transform) as GameObject).GetComponent<KeyboardTemplate>();
            KeyboardGameObject.name = keyboardtype;
            //Do this here in case we have apps with different character limits, so just this value has to change to change the keyboards.
            KeyboardGameObject.InputField.characterLimit = TextLimit;
        }

        public void RemoveKeyboard()
        {
            Destroy(KeyboardGameObject.gameObject);
        }
        #endregion

        public static void GetInputFromKeyboard(Action<string> onSubmit)
        {
            Instance.KeyboardGameObject.OnPop += () => Instance.KeyboardGameObject.transform.SetParent(Instance.transform);
            
            Instance.KeyboardGameObject.AddSubmitAction(onSubmit);
            DisplayManager.AlignDisplay(Instance.KeyboardGameObject); 
            SelectionManager.Instance.Push(Instance.KeyboardGameObject);
        }
    }
}