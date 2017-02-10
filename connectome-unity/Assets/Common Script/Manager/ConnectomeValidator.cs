using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Validates a connectome scene for having required elements to operate. 
/// </summary>
public class ConnectomeValidator : MonoBehaviour
{
    private void OnValidate()
    {
        ValidateClassSingleExistence(typeof(SelectionManager));
        ValidateClassSingleExistence(typeof(EmotivDeviceManager));
        //ValidateClassSingleExistence(typeof(KeyboardManager));
    }

    /// <summary>
    /// Errors of a class type exists more than once within children. 
    /// </summary>
    /// <param name="t"></param>
    private void ValidateClassSingleExistence(Type t)
    {
        Component[] components = GetComponentsInChildren(t);

        if(components.Length != 1)
        {
            //Debug.LogError("Only a sinlge " + t.FullName + " must exist. There are: " + components.Length); 
        }
    }

}

