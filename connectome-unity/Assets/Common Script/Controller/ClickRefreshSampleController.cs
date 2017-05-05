using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickRefreshSampleController : MonoBehaviour
{
    [Header("Adjustables")]
    public int Interval;

    [Range(0f, 1f)]
    public float ClickRate;
    [Range(0f, 1f)]
    public float RefreshRate;

    public bool ApplyReduction; 

    public UnityEvent OnClick;
    public UnityEvent OnRefresh;

    [Header("Component Requirments")]
    public IntervalEmotivSampler Sampler;
    public CommandRateEmotivInterpreter ClickInterpreter;
    public CommandRateEmotivInterpreter RefreshInterpreter;
    public CommandTypeCalculator CommandTypeCalculator; 

    private void Start()
    {
        ClickInterpreter.OnReached.AddListener(() => { if (OnClick != null) { OnClick.Invoke(); } });
        RefreshInterpreter.OnReached.AddListener(() => { if (OnRefresh != null) { OnRefresh.Invoke(); } });
    }

    private void Update()
    {
        UpdateWrapped();
    }

    private void OnValidate()
    {
        if(gameObject.activeSelf)
        {
            Debug.LogWarning("CRSController will take over adjusting assigned compoment values.");
            UpdateWrapped();
        }
    }

    /// <summary>
    /// Update wrapped components 
    /// </summary>
    private void UpdateWrapped()
    {
        Sampler.Interval = Interval;

        ClickInterpreter.ReachRate = ClickRate;      
        RefreshInterpreter.ReachRate = RefreshRate;

        ClickInterpreter.UpdateSliders();
        RefreshInterpreter.UpdateSliders();

        CommandTypeCalculator.Reduction = ApplyReduction;
    }

}
