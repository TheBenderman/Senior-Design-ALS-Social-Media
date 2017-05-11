using Connectome.Core.Common;
using Connectome.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// DataSampler holds data and samples the data upon request. 
/// </summary>
/// <typeparam name="T">Data type which will be held</typeparam>
public abstract class DataSampler<T> : MonoBehaviour
{
    #region Default Values 
    /// <summary>
    /// Default capacity 
    /// </summary>
    public static int DEFAULT_CAPACITY = 5000;
    #endregion
    #region Inspector Attributes 
    [Header("Collection Size (0 for uncapped)")]
    [Range(0, 10000)]
    /// <summary>
    /// Maximum help number of data
    /// </summary>
    public int Capacity = DEFAULT_CAPACITY;
    #endregion
    #region Private Attributes

    /// <summary>
    /// Stored data 
    /// </summary>
    private T[] RawData { set; get; }
    #endregion
    #region Protected Read-Only Attributes 
    /// <summary>
    /// Non-null data used for children to sample 
    /// </summary>
    protected T[] Data { get { return RawData.Where(d => d != null).ToArray(); } }

    /// <summary>
    /// Index of the last added data.
    /// </summary>
    protected int Index { private set; get; }

    /// <summary>
    /// Total number of data that have been added. 
    /// </summary>
    protected int Added { private set; get; }

    /// <summary>
    /// Last added data.
    /// </summary>
    protected T LastAdded {private set; get;}
    #endregion
    #region Unity-Methods
    /// <summary>
    /// Prepares Data entry 
    /// </summary>
    private void Start()
    {
        Clear(); 
    }
    #endregion
    #region Public Methods
    /// <summary>
    /// Add data to list
    /// </summary>
    /// <param name="dataElement"></param>
    public void Register(T dataElement)
    {
        RawData[Index = (Index+1) % RawData.Length] = dataElement;

        Added++; 

        LastAdded = dataElement;
    }

    /// <summary>
    /// Clear recorded data 
    /// </summary>
    public void Clear()
    {
        Index = 0;
        RawData = new T[(Capacity == 0) ? int.MaxValue : Capacity];
    }
    #endregion
    #region  Public Abstract Methods
    /// <summary>
    /// Sample Data. Use the variable Data to sample. 
    /// <see cref="Data"/>
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerable<T> GetSample();
    #endregion
}
