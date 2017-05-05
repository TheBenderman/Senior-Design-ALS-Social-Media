using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A calculator that calculates sampled emotiv states using Emotiv specific configration. 
/// </summary>
public abstract class EmotivRateCalculator : DataRateCalculator<IEmotivState, EmotivCalculatorConfiguration>
{
    
}
