using Connectome.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class DeviceManager<T> : MonoBehaviour 
{
    
    /// <summary>
    /// Allows DeviceManager to auto start 
    /// </summary>
    public bool AutoSetup;

    /// <summary>
    /// Holds initialized device 
    /// </summary>
    private IConnectomeDevice<T> Device;
    
    /// <summary>
    /// Holds initlized reader  
    /// </summary>
    private IConnectomeReader<T> Reader;

    /// <summary>
    /// Holds read data and samples it for interpeters
    /// </summary>
    private DataSampler<T> Sampler;

    private DataInterpreter<T>[] Interpeters; 

    #region Unity Methods 
    /// <summary>
    /// Sets up Device, Reader and interpeters then start interpreter coroutine 
    /// </summary>
    public virtual void Setup()
    {
        Device = GetDevice();
        Reader = GetReader();
        Sampler = GetSampler();
        Interpeters = GetInterpreters(); 

        Reader.OnRead += Sampler.Register;

        Reader.StartReading(); 

        StartCoroutine(InterpetationProcess());
    }

    private void Start()
    {
        if (AutoSetup)
        {
            Setup();
        }
    }

    /// <summary>
    /// Insures readers is disabled
    /// </summary>
    void OnApplicationQuit()
    {
        if (Reader != null && Reader.IsReading)
        {
            Reader.StopReading();
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
           yield return new WaitForSeconds(0); ///needed to execute otherwise it'll be stuck. 
           IEnumerable<T> Sample =  Sampler.GetSample();

            foreach (var interpeter in Interpeters)
            {
                interpeter.Interpeter(Sample); 
            }
        }
    }
    #endregion

    #region Abstarct Methods 
    protected abstract IConnectomeDevice<T> GetDevice();
    protected abstract IConnectomeReader<T> GetReader();
    protected abstract DataSampler<T> GetSampler();
    protected abstract DataInterpreter<T>[] GetInterpreters();
    #endregion

    #region Validate 
    private void OnValidate()
    {
        if(AutoSetup)
        {
            Debug.LogWarning("Remember to disable AutoRun before building.");
        }
    }
    #endregion
}
