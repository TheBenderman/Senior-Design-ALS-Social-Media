using Connectome.Unity.Template;
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
    /// Rate to interpert interpreters.
    /// </summary>
    public float InterpretRate;

    /// <summary>
    /// Hold Interpreters.
    /// </summary>
    public EmotivInterpreterPlugin[] Interpreters;
    #endregion
    #region Unity Methods 
    /// <summary>
    /// Sets up Device, Reader and interpeters then start interpreter coroutine 
    /// </summary>
    void Start()
    {
        DevicePlugin.Setup(); 
        ReaderPlugin.SetUp(DevicePlugin); 

        if(currentInstance != null)
        {
            Debug.LogWarningFormat("There are more than one existence of EmotivDeviceManager. Prev: {0} New: {1}", currentInstance.name, this.name);
        }
        currentInstance = this; 
      
        foreach (var Intepreter in Interpreters)
        {
            Intepreter.Setup(DevicePlugin, ReaderPlugin);
        }

        ReaderPlugin.Start(); 

        StartCoroutine(InterpetationProcess()); 
    }

    /// <summary>
    /// Insures readers is disabled
    /// </summary>
    void OnApplicationQuit()
    {
        if (ReaderPlugin != null)
        {
            ReaderPlugin.Stop();
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
            yield return new WaitForSeconds(InterpretRate); 
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
            Debug.LogWarning("Device is null");
        }
    }

    /// <summary>
    ///  warns when reader is null 
    /// </summary>
    private void ValidateReader()
    {
        if (ReaderPlugin == null)
        {
            Debug.LogWarning("Reader is null");
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

