using Connectome.Unity.Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// KeyboardMenuBuilder is used when The keyboard has a feature for toggling between multiple JSON layouts
/// </summary>
public class KeyboardMenuBuilder : MenuBuilder {

    public KeyboardData keyboard;
    // Use this for initialization
    protected override void Start () {
        base.Start();
        if (!keyboard)
        {
            keyboard = GetComponent<KeyboardData>();
        }
        keyboard.OnToggle += PopulateButtonsFromFile;
    }
}
