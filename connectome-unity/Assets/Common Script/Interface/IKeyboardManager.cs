using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.Keyboard
{
    /// <summary>
    /// IKeyboardPrompt represent a prompt that listens to keyboard clicks. 
    /// </summary>
    public interface IKeyboardManager
    {
        #region Property
        IKeyboard<GameObject> Keyboard { get; }
        #endregion 
        #region Methods
        /// <summary>
        /// Resets Text. 
        /// </summary>
        void ResetText();

        /// <summary>
        /// Submits Text. 
        /// </summary>
        void Submit();

        /// <summary>
        /// Appends Text. 
        /// </summary>
        /// <param name="text"></param>
        void AppendText(string text);

        /// <summary>
        /// Prompts the manager.
        /// </summary>
        /// <param name="onSubmit"></param>
        void SetOnSubmit(Action<string> onSubmit);

        /// <summary>
        /// Collects Text data for submition. 
        /// </summary>
        /// <returns></returns>
        string SubmissionText();
        #endregion
    }
}
