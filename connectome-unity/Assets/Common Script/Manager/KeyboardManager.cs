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
    public abstract class KeyboardManager : MonoBehaviour, IKeyboardManager
    {
        #region Private Attributes
        /// <summary>
        /// Holds prompt event.
        /// </summary>
        private Action<string> OnSubmit;
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
        /// <see cref="GetSubmissionText"/>
        /// </summary>
        public virtual void Submit()
        {
            OnSubmit(GetSubmissionText());
            Hide();
        }
        
        /// <summary>
        /// Prompts keyboard for user. 
        /// </summary>
        /// <param name="onSubmit"></param>
        public virtual void Prompt(Action<string> onSubmit)
        {
            OnSubmit += onSubmit;
            Show();
        }

        /// <summary>
        /// Displays keyboard manager
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
            setKeyboard();
        }

        /// <summary>
        /// Hides keyboard manger  
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion
        #region IKeyboardManager Abstract  
        public abstract void ResetText();
        public abstract void AppendText(string text);
        public abstract string GetSubmissionText();
        #endregion
        #region Abstract Properties
        /// <summary>
        /// Keyboard to be listened to. 
        /// </summary>
        public abstract IKeyboard<GameObject> Keyboard { get; }
        #endregion
        #region Memory
        private GameObject KeyboardGameObject;
        private  string KeyboardPrefabName;
        /// <summary>
        /// TODO? This value will eventually come from some fixed spot, based on the social media we are using.
        /// </summary>
        private int TextLimit;

        private void setKeyboard(string keyboardtype)
        {
            KeyboardGameObject = Instantiate(Resources.Load(keyboardtype), this.transform) as GameObject;
  
            //Commenting this out to try calling the same methods from Keyboard Data.
            //This would elminiate the need to find the game object with the Exit tag just to set these methods via code, and just do it in the editor.
            /*GameObject.FindGameObjectWithTag("Exit").GetComponent<Button>().onClick.AddListener(() =>
            {
                Keyboard.GetComponent<KeyboardData>().ActiveField.text = "";
                removeKeyboard();
                SelectionManager.Instance.PopSelections();
            }); */

            //SelectionManager.Instance.PushSelections(Keyboard.GetComponent<KeyboardData>().BaseSelections);

            //Do this here in case we have apps with different character limits, so just this value has to change to change the keyboards.
            KeyboardGameObject.GetComponent<KeyboardData>().ActiveField.characterLimit = TextLimit;
        }

        private void setKeyboard()
        {
            setKeyboard(((KeyboardType)UserSettings.GetKeyboard()).ToString());
        }

        private void removeKeyboard()
        {
            Destroy(KeyboardGameObject.gameObject);
        }
        #endregion
    }
}