using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSettingsWindow : MonoBehaviour {

    public Slider DurationSlider;
    public Slider PassThresholdSlider;

    public InputField DurationText;
    public InputField PassThresholdText;
	// Use this for initialization
	void Start () {
        DurationSlider.value = UserSettings.GetDuration();
        PassThresholdSlider.value = UserSettings.GetPassThreshold() * 100;
        SetPassThresholdTextValue();
        SetDurationTextValue();
	}

    public void SetDurationValue()
    {
        UserSettings.SetDuration(DurationSlider.value);
    }

    public void SetDurationTextValue()
    {
        DurationText.text = DurationSlider.value.ToString();
    }

    public void SetPassThresholdValue()
    {
        UserSettings.SetPassThreshold(PassThresholdSlider.value/100);
    }

    public void SetPassThresholdTextValue()
    {
        PassThresholdText.text = PassThresholdSlider.value.ToString();
    }

    public void SetDurationSliderValue()
    {
        DurationSlider.value = float.Parse(DurationText.text);
    }

    public void SetPassThresholdSliderValue()
    {
        PassThresholdSlider.value = float.Parse(PassThresholdText.text);
    }

    public void CloseUserSettingsWindow()
    {
        Destroy(gameObject);
    }
}
