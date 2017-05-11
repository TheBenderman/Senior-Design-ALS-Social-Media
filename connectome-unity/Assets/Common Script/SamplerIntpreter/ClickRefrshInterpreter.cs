using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClickRefrshInterpreter : CommandRateEmotivInterpreter
{

    [Header("Refresh Properties")]
    [Range(0,1)]
    /// <summary>
    /// Holds Refresh Rate
    /// </summary>
    public float RefreshRate;

    [Header("Refresh Event")]
    /// <summary>
    /// Event triggered when refersh rate is reached 
    /// </summary>
    public UnityEvent OnRefresh;

    [Header("Refresh Optional")]
    /// <summary>
    /// Slider to display Refresh Rate
    /// </summary>
    public Slider RefreshIndicator;

    /// <summary>
    /// Adds Refresh Interpretation 
    /// </summary>
    /// <param name="rate"></param>
    public override void Interpeter(float rate)
    {
        if (OnRefresh != null && rate >= RefreshRate)
        {
            OnRefresh.Invoke();
        }
        base.Interpeter(rate);
    }

    /// <summary>
    /// Updates Refresh Indecator as well 
    /// </summary>
    /// <param name="activityRate"></param>
    public override void UpdateSliders(float activityRate = 0)
    {
        base.UpdateSliders(activityRate);

        if (RefreshIndicator != null && AutoUpdateIndicators)
        {
            RefreshIndicator.value = ScaleSlider ? RefreshRate / ReachRate : RefreshRate;
        }
    }

    /// <summary>
    /// Warns if Refresh Rate is lower than ReachRate (ClickRate)
    /// </summary>
    private void OnValidate()
    {
        if(ReachRate < RefreshRate)
        {
            Debug.LogWarning("Refresh Rate should not be high that Reach", this);
        }

        if(gameObject.activeSelf)
        {
            UpdateSliders(); 
        }
    }

}
