using Connectome.Unity.Keyboard;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardDemoController : MonoBehaviour {
    public Text text;

    public void SetText(string msg)
    {
        text.text = msg;
    }
    
    public void DisplayText()
    {
        KeyboardManager.GetInputFromKeyboard(SetText);
    }
}
