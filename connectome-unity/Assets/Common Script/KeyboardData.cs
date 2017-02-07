using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardData : MonoBehaviour {
    private KeyboardManager manager;
    public InputField ActiveField;
    public bool IsCaps;
    public int TextLimit;
    private void Start()
    {
        ActiveField.characterLimit = TextLimit;
    }
    public void UpdateText(string text)
    {
        ActiveField.ConcatToCurrentText(IsCaps ? text.ToUpper() : text);
        //Having trouble with cursor not showing in the input field
        ActiveField.Select();
        ActiveField.caretPosition = ActiveField.selectionFocusPosition;
    }

    /// <summary>
    /// Updates the text using a text component (The text displayed on the button, for example).
    /// </summary>
    /// <param name="buttonText"></param>
    public void UpdateText(Text buttonText)
    {
        UpdateText(buttonText.text);
    }

    /// <summary>
    /// Do we want to backspace by word or by letter?
    /// </summary>
    public void BackspaceText()
    {
        if (ActiveField.text.Length > 0)
        {
            ActiveField.BackSpaceCurrentText();
        }
        //Having trouble with cursor not showing in the input field
        ActiveField.Select();
        ActiveField.caretPosition = ActiveField.selectionFocusPosition;
    }
}
