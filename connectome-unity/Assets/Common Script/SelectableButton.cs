using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableButton : SelectableObject
{
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
    }

    public override void TriggerClick(SelectionManager manager)
    {
        button.onClick.Invoke();
    }

    public override void Select(SelectableObject previous)
    {
        button.Select();
    }

    public override ColorBlock GetColor()
    {
        throw new NotImplementedException();
    }
}
