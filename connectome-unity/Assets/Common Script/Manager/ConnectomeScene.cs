using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Expection;
using Connectome.Unity.Keyboard;
using Connectome.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Validates a connectome scene for having required elements to operate. 
/// </summary>
public class ConnectomeScene : MonoBehaviour
{
   
    [Header("Managers (Required)")]
    public SelectionManager SelectionManager;
    public EmotivDeviceManager EmotivDeviceManager;
    public KeyboardManager KeyboardManager;

    [Header("Highlighter")]
    public SelectionHighlighter SelectionHighlighter;

    public EmotivLoginDisplayPanel LoginPanel;  

    public void Start()
    {
        try
        {
            EmotivDeviceManager.Setup();
        }
        catch (NullEmotivDeviceException)
        {
            LoginPanel.OnDismiss += Start; 
            DisplayManager.Display(LoginPanel); 
            return; 
        }


        SelectionManager.StartSelecting(); 
    }

    private void OnValidate()
    { 
        ValidateType(ref SelectionManager);
        ValidateType(ref EmotivDeviceManager);
        ValidateType(ref KeyboardManager);
    }

    /// <summary>
    /// Errors of a class type exists more than once within children. 
    /// </summary>
    /// <param name="t"></param>
    private T GetValidatedSingleComponentFromChildren<T>(Action<string, UnityEngine.Object> logger) where T : Component
    {
        T[] components = GetComponentsInChildren<T>();

        if (components.Length == 0)
        {
            logger("Missing " + typeof(T).FullName + "!", this);
            return null;
        }
        else if (components.Length > 1)
        {
            logger("Only a single " + typeof(T).FullName + " must exist in children. There are: " + components.Length, this);
            return null; 
        }
        else
        {
            return components[0];
        }
    }

    /// <summary>
    /// Quack
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    private void ValidateType<T>(ref T go) where T : Component
    {
        if (go != null)
        {
            GetValidatedSingleComponentFromChildren<T>(Debug.LogWarning);
        }
        else
        {
            go = GetValidatedSingleComponentFromChildren<T>(Debug.LogError);
        }

       
    }

}

