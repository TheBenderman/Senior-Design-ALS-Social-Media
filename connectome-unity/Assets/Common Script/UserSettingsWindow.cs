using Connectome.Unity.Keyboard;
using Connectome.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserSettingsWindow : MonoBehaviour {
    public Dropdown ProfileDrop;

    public Slider DurationSlider;
    public Slider PassThresholdSlider;
    public Slider TargetPowerSlider;
    public Slider RefreshSlider;

    public InputField DurationText;
    public InputField PassThresholdText;
    public InputField TargetPowerText;
    public InputField RefreshText;

    public Dropdown KeyboardDrop;

    public Toggle FlashingToggle;
    public Slider FreqSlider;
    public InputField FreqText;

    public Image BackgroundColor;
    public Image FrameColor;
    public Image ParentFrameColor;

    private Image SelectedColorToEdit;

    public void SetSelectedColor(Image CurrentSelection)
    {
        SelectedColorToEdit = CurrentSelection;
    }

    public void UpdateColorSelection(Color newColor)
    {
        try
        {
            SelectedColorToEdit.color = newColor;
            if(SelectedColorToEdit == FrameColor)
            {
                ((FrameHighlighter)SelectionManager.Instance.Highlighter).FrameColor = newColor;
            }
        }
        catch (NullReferenceException)
        {
            //Initialization
        }
    }

    public void PreviewBackground(Image canvasBackground)
    {
        canvasBackground.color = BackgroundColor.color;
    }

    public void PreviewFrame(FrameHighlighter frame)
    {
        frame.FrameColor = FrameColor.color;
    }

    /// <summary>
    /// Calls the config highlighter in ConnectomeScene
    /// </summary>
    /// <param name="scene"></param>
    public void PreviewFlashing(ConnectomeScene scene)
    {
        scene.PreviewHighlighter(FlashingToggle.isOn);
    }
    
	// Use this for initialization
	void Start () {
        KeyboardDrop.ClearOptions();
        foreach (KeyboardType type in Enum.GetValues(typeof(KeyboardType)))
        {
            KeyboardDrop.options.Add(new Dropdown.OptionData() { text = type.ToString() });
        }
        LoadProfile();
    }

    public void LoadProfile()
    {
        //Set slider values
        DurationSlider.value = UserSettings.Duration;
        PassThresholdSlider.value = UserSettings.PassThreshold * 100;
        TargetPowerSlider.value = UserSettings.TargetPower;
        RefreshSlider.value = UserSettings.RefreshRate;
        FreqSlider.value = UserSettings.Frequency;
        //Set keyboard settings
        KeyboardDrop.value = UserSettings.CurrentKeyboard;
        KeyboardDrop.RefreshShownValue();
        //Set flashing settings
        FlashingToggle.isOn = UserSettings.UseFlashingButtons;
        OnSSVEPToggleClicked();
        //Set text values
        SetPassThresholdTextValue();
        SetDurationTextValue();
        SetTargetPowerTextValue();
        SetRefreshRateTextValue();
        SetFreqTextValue();
        UpdateCurrentKeyboard();
        LoadColors();
        SetSelectedColor(BackgroundColor);
    }
    
    public void OnSSVEPToggleClicked()
    {
        FreqSlider.interactable = FlashingToggle.isOn;
        FreqText.interactable = FlashingToggle.isOn;
        ParentFrameColor.GetComponent<Button>().interactable = !FlashingToggle.isOn;
        FrameColor.GetComponent<Button>().interactable = !FlashingToggle.isOn;
        SetSelectedColor(BackgroundColor);
    }

    public void SaveProfile()
    {
        SetTargetPowerValue();
        SetDurationValue();
        SetPassThresholdValue();
        SetKeyboardType();
        SetFlashingOption();
        SetFreqValue();
        SetRefreshRateValue();
        SaveColors();
    }
    #region Private Methods
    private void SetTargetPowerValue()
    {
        UserSettings.TargetPower = TargetPowerSlider.value;
    }

    private void SetDurationValue()
    {
        UserSettings.Duration = DurationSlider.value;
    }

    private void SetPassThresholdValue()
    {
        UserSettings.PassThreshold = PassThresholdSlider.value/100;
    }

    private void SetRefreshRateValue()
    {
        UserSettings.RefreshRate = RefreshSlider.value;
    }

    private void SetFreqValue()
    {
        UserSettings.Frequency = (int)FreqSlider.value;
    }
    #endregion
    #region Public Methods


    public void SaveColors()
    {
        UserSettings.BackgroundColor = BackgroundColor.color;
        UserSettings.FrameColor = FrameColor.color;
        UserSettings.ParentFrameColor = ParentFrameColor.color;
    }

    public void LoadColors()
    {
        BackgroundColor.color = UserSettings.BackgroundColor;
        FrameColor.color = UserSettings.FrameColor;
        ParentFrameColor.color = UserSettings.ParentFrameColor;

    }
    public void SetDurationTextValue()
    {
        DurationText.text = DurationSlider.value.ToString();
    }

    public void SetDurationSliderValue()
    {
        DurationSlider.value = float.Parse(DurationText.text);
    }

    public void SetPassThresholdTextValue()
    {
        PassThresholdText.text = PassThresholdSlider.value.ToString();
    }

    public void SetPassThresholdSliderValue()
    {
        PassThresholdSlider.value = float.Parse(PassThresholdText.text);
    }

    public void SetTargetPowerTextValue()
    {
        TargetPowerText.text = TargetPowerSlider.value.ToString();
    }

    public void SetTargetPowerSliderValue()
    {
        TargetPowerSlider.value = float.Parse(TargetPowerText.text);
    }

    public void CloseUserSettingsWindow()
    {
        Destroy(gameObject);
    }

    public void SetKeyboardType()
    {
        UserSettings.CurrentKeyboard = KeyboardDrop.value;
    }

    public void SetFlashingOption()
    {
        UserSettings.UseFlashingButtons = FlashingToggle.isOn;
    }

    public void SetRefreshRateTextValue()
    {
        RefreshText.text = RefreshSlider.value.ToString();
    }

    public void SetRefreshSliderValue()
    {
        RefreshSlider.value = float.Parse(RefreshText.text);
    }

    public void SetFreqTextValue()
    {
        FreqText.text = FreqSlider.value.ToString();
    }

    public void SetFreqSliderValue()
    {
        FreqSlider.value = float.Parse(FreqText.text);
    }

    public void UpdateCurrentKeyboard()
    {
        if (!KeyboardManager.Instance.KeyboardGameObject.name.Equals(((KeyboardType)KeyboardDrop.value).ToString()))
        {
            KeyboardManager.Instance.RemoveKeyboard();
            KeyboardManager.Instance.SetKeyboard(UserSettings.CurrentKeyboardName);
        }
    }
    #endregion

}
