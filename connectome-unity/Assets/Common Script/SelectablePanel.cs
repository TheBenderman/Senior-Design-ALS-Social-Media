using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectablePanel : SelectableObject
{
    public SelectableObject[] SelectableList;
    public Image image;
    public Color SelectedColor;
    public Color DefaultColor;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    public override ColorBlock GetColor()
    {
        throw new NotImplementedException();
    }

    public override void Select(SelectableObject previous)
    {
        image.color = new Color(SelectedColor.r, SelectedColor.g, SelectedColor.b, SelectedColor.a);
        if(previous != null)
        {
            previous.ResetColor();
        }
    }

    public override void TriggerClick(SelectionManager manager)
    {
        manager.PushSelections(SelectableList);
    }

    public override void ResetColor()
    {
        image.color = new Color(DefaultColor.r, DefaultColor.g, DefaultColor.b, DefaultColor.a);
    }
}
