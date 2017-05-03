using Connectome.Unity.Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// KeyboardMenuBuilder is used when The keyboard has a feature for toggling between multiple JSON layouts
/// </summary>
[RequireComponent(typeof(KeyboardData))]
public class KeyboardMenuBuilder : MenuBuilder {
    // Use this for initialization
    protected override void Start () {
        base.Start();
        GetComponent<KeyboardData>().OnToggle += PopulateButtonsFromFile;
    }
}
