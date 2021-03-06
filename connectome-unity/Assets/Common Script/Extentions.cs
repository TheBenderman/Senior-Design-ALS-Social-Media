﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Extentions{
    public static void ConcatToCurrentText(this InputField t, string text)
    {
        //Debug.Log(t);
        //Debug.Log(text); -KLD
        t.text = t.text + text;
    }
    public static void BackSpaceCurrentText(this InputField t)
    {
        t.text = t.text.Substring(0, t.text.Length - 1);
    }


    public static void SetButtonText(this Button t, string text)
    {
        t.GetComponentInChildren<Text>().text = text;
    }
}
