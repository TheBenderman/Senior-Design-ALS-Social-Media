using Connectome.Unity.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Connectome.Unity.Keyboard
{
    //TODO this is slipt into two: Part of it is now KeyboardManager. Have this extend ConnectomeKeyboard 
    public class KeyboardData : KeyboardTemplate
    {
        /// <summary>
        /// In case we want to include this functionality.
        /// </summary>
        public bool IsCaps;
        /// <summary>
        /// The initial selections when the keyboard first pops up.
        /// </summary>
        //public SelectableObject[] BaseSelections;
        public void UpdateString(string text)
        {
            InputField.ConcatToCurrentText(IsCaps ? text.ToUpper() : text);
            //Having trouble with cursor not showing in the input field
            InputField.Select();
            InputField.caretPosition = InputField.selectionFocusPosition;
        }

        /// <summary>
        /// Updates the text using a text component (The text displayed on the button, for example).
        /// </summary>
        /// <param name="buttonText"></param>
        public void UpdateText(Text buttonText)
        {
            UpdateString(buttonText.text);
        }

        /// <summary>
        /// Do we want to backspace by word or by letter?
        /// </summary>
        public void BackspaceText()
        {
            if (InputField.text.Length > 0)
            {
                InputField.BackSpaceCurrentText();
            }
            //Having trouble with cursor not showing in the input field
            InputField.Select();
            InputField.caretPosition = InputField.selectionFocusPosition;
        }

        public override void Show()
        {
            transform.SetParent(DisplayManager.Instance.transform);
        }

        public override void Hide()
        {
            transform.SetParent(KeyboardManager.Instance.transform);
        }
    }
}