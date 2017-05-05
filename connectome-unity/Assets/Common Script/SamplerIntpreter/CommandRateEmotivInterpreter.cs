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
    #region DataInterpreter Override  
    /// <summary>
    /// Interprets sample based on ratio of command type apperance in sample relative to sample size. 
    /// </summary>
    /// <param name="sample">Collection of EmotivStates</param>
    public override void Interpeter(float rate)
    {
        UpdateSliders(rate); 

        if (OnReached != null && rate >= ReachRate)
        {
            OnReached.Invoke(); 
        }
    }
    #endregion
    #region Public Methods 
    /// <summary>
    /// Updates UI sliders to reflect avtivity rate and threshhold. 
    /// </summary>
    /// <param name="rate"></param>
    public void UpdateSliders(float rate = 0)
    {
        if (ReachThreshholdSlider != null)
        {
            ReachThreshholdSlider.value = ReachRate;
        }

        if (ActivitySlider != null)
        {
            ActivitySlider.value = rate;
        }
    }
    #endregion
    #region Private Unity Methods 
    /// <summary>
    /// Updates ReachRate on it's Slider when running scene. 
    /// </summary>
    private void Start()
    {
        UpdateSliders();
    }

    /// <summary>
    /// Updates ReachRate on it's Slider within the scene. 
    /// </summary>
    private void OnValidate()
    {
        UpdateSliders(); 
    }
    #endregion
}
