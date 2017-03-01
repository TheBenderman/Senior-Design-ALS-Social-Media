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
        /// <summary>
        /// Grabs Text from InputField
        /// </summary>
        /// <param name="text"></param>
        public virtual string SubmissionText { get { return InputField.text; } }
        /// <summary>
        /// Resets the OnSubmit action.
        /// </summary>
        public virtual Action<string> SubmitAction { set { OnSubmit = value; } }

        #endregion
        #region Private Attributes
        /// <summary>
        /// Holds prompt event.
        /// </summary>
        private Action<string> OnSubmit;

        /// <summary>
        /// Holds Accumelated text 
        /// </summary>
        private InputField InputField;

        /// <summary>
        /// The loaded Keyboard prefab
        /// </summary>
        [HideInInspector]
        public GameObject KeyboardGameObject;

        #endregion
        #region Unity Overrides
        /// <summary>
        /// Connects keyboard onclick event to manager.
        /// </summary>
        private void Awake()
        {
            //Keyboard.OnClick += (go) =>
            //{
            //    AppendText(Keyboard.KeyToString(go));
           // };
        }
        #endregion
        #region IKeyboardManager Overrides
        /// <summary>
        /// Submits text and end keyboard session 
        /// <see cref="SubmissionText"/>
        /// </summary>
        public virtual void Submit()
        {
            OnSubmit(SubmissionText);
            RemoveKeyboard();
        }

        /// <summary>
        /// Displays keyboard manager
        /// </summary>
        public virtual void Show()
        {
            SetKeyboard(((KeyboardType)UserSettings.GetKeyboard()).ToString());
        }

        /// <summary>
        /// Hides keyboard manger  
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion
        #region Memory
        /// <summary>
        /// TODO? This value will eventually come from some fixed spot, based on the social media we are using.
        /// </summary>
        private int TextLimit;

        private void SetKeyboard(string keyboardtype)
        {
            //Change to making the keyboard appear?
            KeyboardGameObject = Instantiate(Resources.Load(keyboardtype), this.transform) as GameObject;
  
            //Commenting this out to try calling the same methods from Keyboard Data.
            //This would elminiate the need to find the game object with the Exit tag just to set these methods via code, and just do it in the editor.
            GameObject.FindGameObjectWithTag("Exit").GetComponent<Button>().onClick.AddListener(() =>
            {
                //Another way to implement this?
                SelectionManager.Instance.Pop();
                SelectionManager.Instance.Pop();
                RemoveKeyboard();
            });

            //Submit Button instantiation
            GameObject.FindGameObjectWithTag("Submit").GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectionManager.Instance.Pop();
                SelectionManager.Instance.Pop();
                Submit();
            });

            //Do this here in case we have apps with different character limits, so just this value has to change to change the keyboards.
            InputField = KeyboardGameObject.GetComponent<KeyboardData>().ActiveField;
            InputField.characterLimit = TextLimit;
        }

        private void RemoveKeyboard()
        {
            Destroy(KeyboardGameObject.gameObject);
        }
        #endregion
    }
}