using Connectome.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// DeviceManager managers a device and a reader by making sure they run, and display proper notifications when any stops working. 
/// It also controls Interprepter that recieve a data sample read from device by reader. 
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class DeviceManager<T> : MonoBehaviour 
{
    #region Inspecter Attributes 
    /// <summary>
    /// Allows DeviceManager to auto start 
    /// </summary>
    public bool AutoSetup;
    #endregion
    #region Private Attributes 
    /// <summary>
    /// Holds device 
    /// </summary>
    private IConnectomeDevice<T> Device;
    
    /// <summary>
    /// Holds reader  
    /// </summary>
    private IConnectomeReader<T> Reader;

    /// <summary>
    /// Holds read data and samples it for interpeters
    /// </summary>
    private DataSampler<T> Sampler;

    /// <summary>
    /// Holds interpreters to be invoked
    /// </summary>
    private DataInterpreter<T>[] Interpeters;

    /// <summary>
    /// Counts total states read 
    /// </summary>
    private int TotalStatesRead; 
    #endregion
    #region Virtual Methods 
    /// <summary>
    /// Sets up Device, Reader, and Sampler then start interpreters coroutine 
    /// </summary>
    public virtual void Setup()
    {
        Device = GetDevice();
        Reader = GetReader();
        Sampler = GetSampler();
        Interpeters = GetInterpreters(); 

        Reader.OnRead += Sampler.Register;
        Reader.OnRead += (s) => TotalStatesRead++;

        Reader.StartReading(); 

        StartCoroutine(InterpetationProcess());
    }
    #endregion
    #region Unity Methods 
    /// <summary>
    /// Auto runs Manager if  AutoSetup is checked  
    /// </summary>
    private void Start()
    {
        if (AutoSetup)
        {
            Setup();
        }
    }

    /// <summary>
    /// Insures reader is disabled
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
    /// Coroutine that Interpret every intepreter with an allocated sample. 
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
    /// <summary>
    /// Gets a Device 
    /// </summary>
    /// <returns>An initilized device</returns>
    protected abstract IConnectomeDevice<T> GetDevice();
    /// <summary>
    /// Gets a Reader 
    /// </summary>
    /// <returns>An initilized reader</returns>
    protected abstract IConnectomeReader<T> GetReader();
    /// <summary>
    /// Gets a DataSampler 
    /// </summary>
    /// <returns>An initilized DataSampler</returns>
    protected abstract DataSampler<T> GetSampler();
    /// <summary>
    /// Gets Interpreters  
    /// </summary>
    /// <returns>Initilized set fo interpreters</returns>
    protected abstract DataInterpreter<T>[] GetInterpreters();
    #endregion
    #region Validate 
    /// <summary>
    /// Checks of AutoSetup is enabled. 
    /// </summary>
    private void OnValidate()
    {
        if(AutoSetup)
        {
            Debug.LogWarning("Remember to disable AutoRun before building.");
        }
    }
    #endregion
}
