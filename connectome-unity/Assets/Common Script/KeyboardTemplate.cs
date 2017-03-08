using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Connectome.Unity.Keyboard
{
    public abstract class KeyboardTemplate : MonoBehaviour
    {
        private Action<string> onSubmit;
        /// <summary>
        /// The text field used to display/hold the text.
        /// </summary>
        public InputField InputField;
        public string SubmissionText { get { return InputField.text; } }
        public virtual void AddSubmitAction(Action<string> onSubmit)
        {
            onSubmit += onSubmit;
        }
        public virtual void Submit()
        {
            onSubmit(SubmissionText);
        }
        public abstract void Show();
        public abstract void Hide();
    }
}
