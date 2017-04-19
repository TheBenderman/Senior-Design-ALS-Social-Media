using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Expection;
using Connectome.Unity.Keyboard;
using Connectome.Unity.Plugin;
using Connectome.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Validates a connectome scene for having required elements to operate. 
/// </summary>
public class ConnectomeScene : MonoBehaviour
{
    public bool ApplyUserSettings;

    [Header("Managers")]
    public SelectionManager SelectionManager;
    public EmotivDeviceManager EmotivDeviceManager;
    public KeyboardManager KeyboardManager;

    [Header("Device Interpeter")]
    public ClickRefreshInterperter ClickRefreshInterperter; 

    [Header("Factories")]
    public HighlighterFactory HighlighterFactory;

    [Header("UI Components")]
    public GameObject HighlighterContainer; 
    public EmotivLoginDisplayPanel LoginPanel;

    [Header("Main Canvas")]
    public Canvas ConnectomeCanvas;


    public void Start()
    {
        ///Login in will set profile type 
        try
        {
            EmotivDeviceManager.Setup();
        }
        catch (NullEmotivDeviceException)
        {
            LoginPanel.OnDismiss += Start; 
            DisplayManager.Display(LoginPanel); 
            return; 
        }

        ///Configure layout 
        if (ApplyUserSettings)
        {
            //adjust as proper
            ConfigureLayout();
            ConfigureSelectionManager(SelectionManager);
            //ConfigureHighlighter(SelectionManager, UserConfig.HighlighterType )  HighlighterType is set based on profile type. 
            //ConfigureHighlighter(UserSettings.UseFlashingButtons);//Change to setting highlighter?

            ConfigureDeviceInterperter(ClickRefreshInterperter); 
        }
        ///Start selecting 
        SelectionManager.AllowSelection = true; 
    }

    #region Configurations 

    public void MainConfig()
    {
        ConfigureLayout();
        ConfigureSelectionManager(SelectionManager);
        ConfigureDeviceInterperter(ClickRefreshInterperter);
    }
    
    private void ConfigureLayout()
    {
        //set background color or such 
        ConnectomeCanvas.GetComponent<Image>().color = UserSettings.BackgroundColor;
        
    }

    private void ConfigureSelectionManager(SelectionManager sm)
    {
        //man.Highlighter = -we should do factory from enums 
        sm.WaitInterval = UserSettings.Duration;
    }

    private void ConfigureHighlighter(bool useFlashing)
    {
        if (useFlashing)
        {
            FlashingHighlighter flashing = HighlighterFactory.CreateHighlighter<FlashingHighlighter>(HighlighterType.Flashing);

            flashing.Frequency = UserSettings.Frequency;
            this.SelectionManager.Highlighter = flashing;
            flashing.transform.SetParent(HighlighterContainer.transform);
            flashing.FlashingColors[0] = UserSettings.BackgroundColor;
            flashing.FlashingColors[1] = UserSettings.FlashingColor;
        }
        else
        {
            FrameHighlighter frame = HighlighterFactory.CreateHighlighter<FrameHighlighter>(HighlighterType.Frame);
            this.SelectionManager.Highlighter = frame;
            frame.FrameColor = UserSettings.HighlighterColor;
            frame.transform.SetParent(HighlighterContainer.transform);
        }
    }
    private void ConfigureHighlighter(HighlighterType type)
    {
        //example for flashing 
        FlashingHighlighter flashing = HighlighterFactory.CreateHighlighter<FlashingHighlighter>(type);

        flashing.Frequency = UserSettings.Frequency;
        //flashing.FlashingColors = 

        this.SelectionManager.Highlighter = flashing;

        //move highlighter to scene
        flashing.transform.SetParent(HighlighterContainer.transform);
    }

    private void ConfigureDeviceInterperter(ClickRefreshInterperter ClickRefreshInterperter)
    {
        ClickRefreshInterperter.Interval = (long) UserSettings.Duration*1000;
        ClickRefreshInterperter.ClickThreshhold = UserSettings.PassThreshold/100;//These values in the window are percents
        ClickRefreshInterperter.RefreshThreshhold = UserSettings.RefreshRate/100;
    }

    #endregion
    #region Validation 
    private void OnValidate()
    { 
        //validate and/or get from children
        ValidateType(ref SelectionManager);
        ValidateType(ref EmotivDeviceManager);
        ValidateType(ref KeyboardManager);

        if(ApplyUserSettings == false)
        {
            Debug.LogWarning("Enable ApplyUserSettings before building.", this);
        }
    }

    /// <summary>
    /// Attempts to get the class from it's chilren. 
    /// </summary>
    /// <param name="t"></param>
    private T GetValidatedSingleComponentFromChildren<T>(Action<string, UnityEngine.Object> logger) where T : Component
    {
        T[] components = GetComponentsInChildren<T>();

        if (components.Length == 0)
        {
            logger("Missing " + typeof(T).FullName + "!", this);
            return null;
        }
        else if (components.Length > 1)
        {
            logger("Only a single " + typeof(T).FullName + " must exist in children. There are: " + components.Length, this);
            return null; 
        }
        else
        {
            return components[0];
        }
    }

    /// <summary>
    /// If a given object is null, it tried to fill it from children. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    private void ValidateType<T>(ref T go) where T : Component
    {
        if (go != null)
        {
            GetValidatedSingleComponentFromChildren<T>(Debug.LogWarning);
        }
        else
        {
            go = GetValidatedSingleComponentFromChildren<T>(Debug.LogError);
        }
    }

    #endregion
}

