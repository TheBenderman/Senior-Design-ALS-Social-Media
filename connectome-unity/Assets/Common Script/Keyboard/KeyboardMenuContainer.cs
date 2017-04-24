using Connectome.Unity.Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Connectome.Unity.Menu;
using Connectome.Unity.UI;

public class KeyboardMenuContainer : KeyboardTemplate
{
    #region PInspector Attributes
    /// <summary>
    /// The text field used to display/hold the text.
    /// </summary>
    public InputField InputField;

    public SelectionMenu Menu; 
    #endregion
    #region KeyboardTemplate Override
    public override string SubmissionText
    {
        get
        {
            return InputField.text; 
        }
    }
    public override void Submit()
    {
        InvokeOnSubmit();
        InputField.text = ""; 
    }
    #endregion
    #region SelectionMenu Override 
    public override ISelectionMenu InvokeSelected()
    {
        return Menu.InvokeSelected(); 
    }

    public override void SelectNext(ISelectionHighlighter h)
    {
        Menu.SelectNext(h); 
    }

    public override void Pushed()
    {
        base.Pushed();
        Menu.Pushed(); 
        InputField.text = ""; 
    }
    #endregion
    #region Validate 
    private void OnValidate()
    {
        if(Menu == this)
        {
            Menu = null;
            Debug.LogError("Menu cannot be set to itself!", this);
        }
    }
    #endregion
}

