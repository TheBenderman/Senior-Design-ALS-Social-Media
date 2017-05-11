using Connectome.Emotiv.Interface;
using Connectome.Unity.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Emotiv.Enum;
using System;

public class ManualControlTest : MonoBehaviour
{
    public SelectionManager SelectionManager;
    public IntervalEmotivSampler Sampler; 
    public CommandRateEmotivInterpreter ClickInterpreter;
    public CommandRateEmotivInterpreter RefreshInterpreter;
    public FlashingHighlighter FlasgingHighlighter;

    public Text DurationText;
    public Text FlashingFrequencyText;
    public Text ClickRateText;
    public Text RefreshRateText;


    public void DurationSlider(Slider s)
    {
        SelectionManager.WaitInterval = s.value;
        Sampler.Interval = (int)(s.value * 1000);

        DurationText.text = s.value.ToString("0.00"); 
    }

    public void FlashingSlider(Slider s)
    {
        FlasgingHighlighter.Frequency = (int)s.value; 
        FlashingFrequencyText.text = s.value.ToString("0"); 
    }

    public void ClickRate(Slider s)
    {
        ClickInterpreter.ReachRate = s.value; 
        ClickRateText.text = s.value.ToString("0.00");
    }

    public void RefreshRate(Slider s)
    {
        RefreshInterpreter.ReachRate = s.value;
        RefreshRateText.text = s.value.ToString("0.00");
    }
}


