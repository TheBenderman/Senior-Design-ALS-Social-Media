using Connectome.Emotiv.Interface;
using Connectome.Unity.Expection;
using Connectome.Unity.Template;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Core.Interface;

/// <summary>
/// Holds Device, and Reader as well as running interpetation. 
/// </summary>
public class EmotivDeviceManager : DeviceManager<IEmotivState>
{
    #region Public Inspector Attributes 
    [Header("Emotiv Requirements")]
    /// <summary>
    /// Hold Device
    /// </summary>
    public EmotivDevicePlugin DevicePlugin;
    /// <summary>
    /// Holds Redaer 
    /// </summary>
    public EmotivReaderPlugin ReaderPlugin;

    [Header("Data Process Requirements")]
    /// <summary>
    /// Holds Samplers with creates samples to interpret
    /// </summary>
    public EmotivSampler Sampler;

    /// <summary>
    /// Holds data sample interpreters 
    /// </summary>
    public EmotivInterpreter[] Interpreters;  

    #endregion
    #region DeviceManager Overrides
    protected override IConnectomeDevice<IEmotivState> GetDevice()
    {
        DevicePlugin.Setup();
        return DevicePlugin; 
    }

    protected override IConnectomeReader<IEmotivState> GetReader()
    {
        ReaderPlugin.SetUp(DevicePlugin);
        return ReaderPlugin; 
    }

    protected override DataSampler<IEmotivState> GetSampler()
    {
        return Sampler; 
    }

    protected override DataInterpreter<IEmotivState>[] GetInterpreters()
    {
        return Interpreters; 
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

