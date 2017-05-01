using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;
using UnityEngine.Events;
using Connectome.Emotiv.Enum;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Interperts EmotivStates based on rate of target command appearance relative to scalled sample size.
/// The given sample is scalled down based on maximum sample size. 
/// </summary>
public class CommandRateEmotivInterpreter : EmotivInterpreter
{
    #region Inspecter Attributes 
    [Header("Control")]
    [Range(0, 1)]
    /// <summary>
    /// Rate when analized data pass, OnReached is triggered. 
    /// </summary>
    public float ReachRate;

    /// <summary>
    /// Target command type to count it's appearance rate. 
    /// </summary>
    public EmotivCommandType TargetCommand;

    [Header("Events")]
    /// <summary>
    /// Invoked when analized data reach ReachRate. 
    /// </summary>
    public UnityEvent OnReached;

    [Header("Optional")]
    [Tooltip("Updates slider value to calculated rate")]
    /// <summary>
    /// Used to display analized rate 
    /// </summary>
    public Slider ActivitySlider;

    [Tooltip("Sets slider value to ReachRate")]
    /// <summary>
    /// Used to display ReachRate
    /// </summary>
    public Slider ReachThreshholdSlider;
    #endregion
    #region Private Attributes 
    /// <summary>
    /// Holds and keeps track of maximum sample size. 
    /// </summary>
    private int MaxSampleSizeScaler = 40;
    #endregion
    #region DataInterpreter Override  
    /// <summary>
    /// Interprets sample based on ratio of command type apperance in sample relative to sample size. 
    /// </summary>
    /// <param name="sample">Collection of EmotivStates</param>
    public override void Interpeter(IEnumerable<IEmotivState> sample)
    {
        if (ReachThreshholdSlider != null)
        {
            ReachThreshholdSlider.value = ReachRate;
        }
        float Rate = ((float)sample.Where(s => s.Command == TargetCommand).Count()) / sample.Count();
        
        if (sample.Count() < MaxSampleSizeScaler)
        {
            Rate *= ((float)sample.Count()) / MaxSampleSizeScaler;
        }
        else
        {
            MaxSampleSizeScaler = sample.Count(); 
        }

        if (ActivitySlider != null)
        {
            ActivitySlider.value = Rate;
        }
        if (OnReached != null && ReachRate <= Rate) ///sry
        {
            OnReached.Invoke(); 
        }
    }
    #endregion
    #region Private Methods 
    /// <summary>
    /// Updates ReachRate on it's Slider when running scene. 
    /// </summary>
    private void Start()
    {
        if (ReachThreshholdSlider != null)
        {
            ReachThreshholdSlider.value = ReachRate;
        }
    }

    /// <summary>
    /// Updates ReachRate on it's Slider within the scene. 
    /// </summary>
    private void OnValidate()
    {
        if (ReachThreshholdSlider != null)
        {
            ReachThreshholdSlider.value = ReachRate;
        }
    }
    #endregion
}
