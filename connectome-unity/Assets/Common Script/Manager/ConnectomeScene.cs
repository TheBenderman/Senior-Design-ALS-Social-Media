using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Expection;
using Connectome.Unity.Keyboard;
using Connectome.Unity.Manager;
using Connectome.Unity.Plugin;
using Connectome.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fabric.Crashlytics;

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
    public LoginEPOCDevicePlugin LoginEPOCDevicePlugin;
    //public ClickRefreshInterperter ClickRefreshInterperter; 
    public CommandRateEmotivInterpreter ClickInterpreter;
    public CommandRateEmotivInterpreter RefreshInterpreter;
    public IntervalEmotivSampler Sampler; 

    [Header("Factories")]
    public HighlighterFactory HighlighterFactory;

    [Header("UI Components")]
    public GameObject HighlighterContainer; 
    public EmotivLoginDisplayPanel LoginPanel;
    public LabeledHighligter LabledHighlighter; 

    [Header("Main Canvas")]
    public Canvas ConnectomeCanvas;


    public void Start()
    {
        ///Login in will set profile type 
        try
        {
            EmotivDeviceManager.Setup();
        }
        catch (NullEmotivDeviceException e)
        {
            LoginPanel.OnDismiss -= Start;
            LoginPanel.OnDismiss += Start; 
            DisplayManager.Display(LoginPanel); 

			Crashlytics.RecordCustomException ("Emotiv Exception", "thrown exception", e.StackTrace);
            return; 
        }

       
        ///Configure layout 
        if (ApplyUserSettings)
        {
            MainConfig();
        }
        ///Start selecting 
        SelectionManager.AllowSelection = true; 
    }

    #region Configurations 

    public void MainConfig()
    {
        ConfigureLayout();
        ConfigureSelectionManager(SelectionManager);
        ConfigureDeviceInterperter();
        ConfigureHighlighter(UserSettings.UseFlashingButtons);
	    //ConfigureEmotivLogin(); 
    }

    /// <summary>
    /// Moves the current highlighter back to the Highlighter Container
    /// </summary>
    public void ReturnHighlighter()
    {
        SelectionManager.Highlighter.DisableHighlight();
        SelectionManager.Highlighter.transform.SetParent(HighlighterContainer.transform);
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
            LabledHighlighter.Highlighter = flashing;
            flashing.transform.SetParent(HighlighterContainer.transform);
        }
        else
        {
            FrameHighlighter frame = HighlighterFactory.CreateHighlighter<FrameHighlighter>(HighlighterType.Frame);
            LabledHighlighter.Highlighter = frame;
            frame.FrameColor = UserSettings.FrameColor;
            frame.transform.SetParent(HighlighterContainer.transform);
        }

        this.SelectionManager.Highlighter = LabledHighlighter; 
    }

    private void ConfigureEmotivLogin()
    {
        LoginEPOCDevicePlugin.Username = UserSettings.Username;
        LoginEPOCDevicePlugin.Password = UserSettings.Password;
        LoginEPOCDevicePlugin.Profile = UserSettings.Profile;

    }

    private void ConfigureDeviceInterperter()
    {
        Sampler.Interval = (int)  UserSettings.Duration*1000;
        ClickInterpreter.ReachRate = UserSettings.PassThreshold;//These values in the window are percents
        RefreshInterpreter.ReachRate = UserSettings.RefreshRate;
    }

    #endregion
    #region Validation 
    private void OnValidate()
    { 
        //validate and/or get from children
        ValidateType(ref SelectionManager);
        ValidateType(ref EmotivDeviceManager);
        ValidateType(ref KeyboardManager);

        if(ConnectomeCanvas == null)
        {
            Debug.LogWarning("A canvas must be attached before building", this);
        }

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

