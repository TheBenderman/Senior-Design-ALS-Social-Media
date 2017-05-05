using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Calculates a rating for a set of data
/// </summary>
/// <typeparam name="T">Data type</typeparam>
/// <typeparam name="C">Calculator config type</typeparam>
public abstract class DataRateCalculator<T, C> : MonoBehaviour
{
    /// <summary>
    /// Calculates sampled data
    /// </summary>
    /// <param name="sample">Sampled data</param>
    /// <param name="config">Calculator configoration</param>
    /// <returns></returns>
    public abstract float Calculate(IEnumerable<T> sample, C config); 
}
