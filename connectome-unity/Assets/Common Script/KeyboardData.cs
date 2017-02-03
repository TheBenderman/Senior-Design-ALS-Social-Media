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
