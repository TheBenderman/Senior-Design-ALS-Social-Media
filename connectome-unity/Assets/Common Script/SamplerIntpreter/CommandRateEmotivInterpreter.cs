using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;
using UnityEngine.Events;
using Connectome.Emotiv.Enum;
using System.Linq;
using UnityEngine.UI;

public class CommandRateEmotivInterpreter : EmotivInterpreter
{
    public static int MaxSampleSizeScaler = 40;

    [Header("Control")]
    [Range(0, 1)]
    public float ReachRate;

    public EmotivCommandType TargetCommand;

    [Header("Events")]
    public UnityEvent OnReached;

    [Header("Optional")]
    [Tooltip("Updates slider value to calculated rate")]
    public Slider ActivitySlider;

    [Tooltip("Sets slider value to ReachRate")]
    public Slider ReachThreshholdSlider;

    public override void Interpeter(IEnumerable<IEmotivState> sample)
    {
        //Debug.Log(sample.Count());
        if (ReachThreshholdSlider != null)
        {
            ReachThreshholdSlider.value = ReachRate;
        }
        float Rate = ((float)sample.Where(s => s.Command == TargetCommand).Count()) / sample.Count();
        Debug.Log(sample.Count());
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

    private void Start()
    {
        if (ReachThreshholdSlider != null)
        {
            ReachThreshholdSlider.value = ReachRate;
        }
    }

    private void OnValidate()
    {
        if (ReachThreshholdSlider != null)
        {
            ReachThreshholdSlider.value = ReachRate;
        }
    }
}
