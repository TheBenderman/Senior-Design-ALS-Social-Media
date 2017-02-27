using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.Keyboard
{
    /// <summary>
    /// A keyboard with a target InputField to store text
    /// </summary>
    public class BasicKeyboardPrompt : KeyboardManager
    {
        #region Inspecter Attributes

        
        /// <summary>
        /// Holds Target Keyboard
        /// </summary>
        public ConnectomeKeyboard ConnectomeKeyboard;
        #endregion
        #region KeyboardPrompt Overrides

        /// <summary>
        /// Gets keyboard value from keyboard set in Inspector.
        /// </summary>
        public override IKeyboard<GameObject> Keyboard
        {
            get
            {
                return ConnectomeKeyboard;
            }
        }

        /// <summary>
        /// Append Text to input field 
        /// </summary>
        /// <param name="text"></param>
        public override void AppendText(string text)
        {
            InputField.text += text;
        }

        /// <summary>
        /// Resets Text in unput Field 
        /// </summary>
        public override void ResetText()
        {
            InputField.text = "";
        }
        #endregion
    }
}
