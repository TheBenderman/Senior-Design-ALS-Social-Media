using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.KLD.PromptTest
{
    public class PromptPopup : MonoBehaviour
    {
        /// <summary>
        /// Contains inputted text 
        /// </summary>
        public InputField InputField;

        /// <summary>
        /// Invoked when submitting popup 
        /// </summary>
        Action<string> OnSubmit;

        /// <summary>
        /// Called from outside of class 
        /// </summary>
        /// <param name="onSubmit"></param>
        public void Popup(Action<string> onSubmit)
        {
            OnSubmit = onSubmit;
            Display();
        }

        /// <summary>
        /// Called by SubmitButton within Popup
        /// </summary>
        public void Submit()
        {
            Hide();
            OnSubmit(InputField.text);
        }

        /// <summary>
        /// Display popup 
        /// </summary>
        private void Display()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Hide popup 
        /// </summary>
        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
