using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardData : MonoBehaviour {
    /// <summary>
    /// The text field used to display the text.
    /// </summary>
    public InputField ActiveField;
    /// <summary>
    /// In case we want to include this functionality.
    /// </summary>
    public bool IsCaps;
    /// <summary>
    /// The initial selections when the keyboard first pops up.
    /// </summary>
    public SelectableObject[] BaseSelections;
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
    /// <summary>
    /// Remove the keyboard from the scene and return to the previous selection screen.
    /// </summary>
    public void ExitKeyboard()
    {
        SelectionManager.Instance.PopSelections();
        Destroy(this.gameObject);
    }
}
