using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardData : MonoBehaviour {
    private KeyboardManager manager;
    public InputField ActiveField;
    public bool IsCaps;
    public void UpdateText(string text)
    {
        ActiveField.ConcatToCurrentText(IsCaps ? text.ToUpper() : text);
    }

    public void BackspaceText()
    {
        ActiveField.BackSpaceCurrentText();
    }
}
