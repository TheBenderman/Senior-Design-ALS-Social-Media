using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;
using System.Linq;
using Connectome.Emotiv.Common;

/// <summary>
/// Samples EmotivStates based on lastest given time interval 
/// </summary>
public class IntervalEmotivSampler : EmotivSampler
{
    #region Inspector Attributes 
    [Header("In milliseconds")]
    /// <summary>
    /// Sampling Interval 
    /// </summary>
    public int Interval;
    #endregion
    #region DataSampler Override 
    /// <summary>
    /// Samples latest inteval.
    /// </summary>
    /// <returns></returns>
    public override IEnumerable<IEmotivState> GetSample()
    {
        if(base.LastAdded == null)
        {
           return  new EmotivState[0]; 
        }

       long lastTime = base.LastAdded.Time;
       long startTime = lastTime - Interval;

       return Data.Where(d => d.Time >= startTime && d.Time < lastTime); 
    }
    #endregion
}
