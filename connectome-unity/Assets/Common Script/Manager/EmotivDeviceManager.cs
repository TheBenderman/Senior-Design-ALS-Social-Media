using Connectome.Unity.Expection;
using Connectome.Unity.Template;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds Device, and Reader as well as running interpetation. 
/// </summary>
public class EmotivDeviceManager : MonoBehaviour
{
    #region Singleton
    public static EmotivDeviceManager Instance { get { return currentInstance;  } }

    public bool AutoSetup;

    private static EmotivDeviceManager currentInstance;
    #endregion
    #region Public Inspector Attributes 
    /// <summary>
    /// Hold Device
    /// </summary>
    public EmotivDevicePlugin DevicePlugin;
    /// <summary>
    /// Holds Redaer 
    /// </summary>
    public EmotivReaderPlugin ReaderPlugin;

    /// <summary>
    /// Hold Interpreters.
    /// </summary>
    public EmotivInterpreterPlugin[] Interpreters;
    #endregion
    #region Unity Methods 
    /// <summary>
    /// Sets up Device, Reader and interpeters then start interpreter coroutine 
    /// </summary>
    public void Setup()
    {
        DevicePlugin.Setup();

        ReaderPlugin.SetUp(DevicePlugin);

        currentInstance = this; 
      
        foreach (var Intepreter in Interpreters)
        {
            Intepreter.Setup(DevicePlugin, ReaderPlugin);
        }

        ReaderPlugin.StartReading(); 

        StartCoroutine(InterpetationProcess()); 
    }

    private void Start()
    {
        if(AutoSetup)
        {
            Setup(); 
        }
    }

    /// <summary>
    /// Insures readers is disabled
    /// </summary>
    void OnApplicationQuit()
    {
        if (ReaderPlugin != null && ReaderPlugin.IsReading)
        {
            ReaderPlugin.StopReading();
        }
    }
    #endregion
    #region Coroutine 
    /// <summary>
    /// Coroutine that Interpret every intepreter
    /// </summary>
    /// <returns></returns>
    IEnumerator InterpetationProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(0); 
            foreach (var Intepreter in Interpreters)
            {
                Intepreter.Interpret();
            }
        }
    }
    #endregion
    #region Validation 
    /// <summary>
    /// Validate required component 
    /// </summary>
    private void OnValidate()
    {
        ValidateDevice();
        ValidateReader();
        ValidateInterpreters();
    }

    /// <summary>
    ///  warns when device is null 
    /// </summary>
    private void ValidateDevice()
    {
        if (DevicePlugin == null)
        {
            Debug.LogWarning("Device is null", this);
        }
    }

    /// <summary>
    ///  warns when reader is null 
    /// </summary>
    private void ValidateReader()
    {
        if (ReaderPlugin == null)
        {
            Debug.LogWarning("Reader is null", this);
        }
    }

    /// <summary>
    ///  insure no interpreter in null
    /// </summary>
    private void ValidateInterpreters()
    {
        for (int i = 0; i < Interpreters.Length; i++)
        {
            if(Interpreters[i] == null)
                Debug.LogError("Interpreter at index " + i + " is null");
        }
    }
    #endregion
}

