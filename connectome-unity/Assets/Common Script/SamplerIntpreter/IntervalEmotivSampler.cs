using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;
using System.Linq;
using Connectome.Emotiv.Common;

public class IntervalEmotivSampler : EmotivSampler
{
    [Header("In milliseconds")]
    public int Interval; 

    public override IEnumerable<IEmotivState> GetSample()
    {
        if(LastAdded == null)
        {
           return  new EmotivState[0]; 
        }

       long lastTime = LastAdded.Time;
       long startTime = lastTime - Interval;

       return Data.Where(d => d != null && d.Time >= (startTime) && d.Time < lastTime); 
    }
}
