using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSettingsWindow : MonoBehaviour {

    public Slider DurationSlider;
    public Slider PassThresholdSlider;
    public Slider TargetPowerSlider;

    public InputField DurationText;
    public InputField PassThresholdText;
    public InputField TargetPowerText;

    public Dropdown KeyboardDrop;
    
	// Use this for initialization
	void Start () {
        DurationSlider.value = UserSettings.GetDuration();
        PassThresholdSlider.value = UserSettings.GetPassThreshold() * 100;
        TargetPowerSlider.value = UserSettings.GetTargetPower();
        SetPassThresholdTextValue();
        SetDurationTextValue();
        SetTargetPowerTextValue();
	}

    public void SetTargetPowerValue()
    {
        UserSettings.SetTargetPower(TargetPowerSlider.value);
    }

    public void SetTargetPowerTextValue()
    {
        TargetPowerText.text = TargetPowerSlider.value.ToString();
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

    public void SetTargetPowerSliderValue()
    {
        TargetPowerSlider.value = float.Parse(TargetPowerText.text);
    }

    public void CloseUserSettingsWindow()
    {
        Destroy(gameObject);
    }


}
