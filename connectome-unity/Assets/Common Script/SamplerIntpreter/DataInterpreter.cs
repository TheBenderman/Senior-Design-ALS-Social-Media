using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interpert a sample of data. 
/// </summary>
/// <typeparam name="T">Data type to be interpert</typeparam>
public abstract class DataInterpreter<T> : MonoBehaviour
{
    #region Public Abstract Methods
    /// <summary>
    /// Interpert rate of calculated data
    /// </summary>
    /// <param name="sample"></param>
    public abstract void Interpeter(float rate);
    #endregion
}
