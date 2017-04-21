using Connectome.Unity.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Boomlagoon.JSON;
using System.IO;
using Connectome.Unity.UI;

namespace Connectome.Unity.Keyboard
{
    //TODO this is slipt into two: Part of it is now KeyboardManager. Have this extend ConnectomeKeyboard 
    public class KeyboardData : KeyboardMenuContainer
    {
        /// <summary>
        /// Called when this keyboard is loaded
        /// </summary>
        private void Start()
        {
        }
        /// <summary>
        /// In case we want to include this functionality.
        /// </summary>
        public bool IsCaps;
        private bool SymToggle = false;
        public Action<string> OnToggle;
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
            ResetSymbols();
        }

        /// <summary>
        /// Insert a space into the text field
        /// </summary>
        public void Space()
        {
            UpdateString(" ");
            ResetSymbols();
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
            ResetSymbols();
        }

        public override void Popped()
        {
	        base.Popped();
            ResetSymbols();
        }

        public void ResetSymbols()
        {
            if (SymToggle) ToggleSymbols();//Reset back to default if the keyboard is exited while Symbols are up
        }
       

        public void ToggleSymbols()
        {
            if (SymToggle)
            {
                OnToggle.Invoke(gameObject.name);
                SymToggle = false;
            }
            else
            {
                OnToggle.Invoke(gameObject.name + "Sym");
                SymToggle = true;
            }
        }
    }
}
