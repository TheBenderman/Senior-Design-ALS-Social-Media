using Connectome.Unity.Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMenuBuilder : MenuBuilder {

    public KeyboardData keyboard;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        if (!keyboard)
        {
            keyboard = GetComponent<KeyboardData>();
        }
        keyboard.OnToggle += PopulateButtonText;
    }
}
