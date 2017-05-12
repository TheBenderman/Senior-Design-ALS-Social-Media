using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyboardType
{
    PhraseKeyboard,
    QWERTYKeyboard,
    GridKeyboard
}
public static class UserSettings
{
    /// <summary>
    /// The default value for the keyboard when Connectome is first run on a computer
    /// </summary>
    public const KeyboardType DEFAULT_KEYBOARD = KeyboardType.GridKeyboard;
    /// <summary>
    /// The default duration
    /// </summary>
    public const int DEFAULT_DURATION = 5;
    /// <summary>
    /// The default pass threshold
    /// </summary>
    public const float DEFAULT_PASS = 0.89f;
    /// <summary>
    /// The default refresh rate
    /// </summary>
    public const float DEFAULT_REFRESH = 0.3f;
    /// <summary>
    /// The default frequency of SSVEP flashing
    /// </summary>
    public const int DEFAULT_FREQ = 15;
    /// <summary>
    /// The default power
    /// </summary>
    public const int DEFAULT_POWER = 0;
    /// <summary>
    /// The default setting for whether SSVEP should be on
    /// 0 = USE SSVEP
    /// 1 = DO NOT USE SSVEP
    /// </summary>
    public const int DEFAULT_SSVEP = 0;
    public static string Username { get { return GetLoginInfo("username");  } }
    public static string Password { get { return GetLoginInfo("password"); } }
    public static string Profile { get { return GetLoginInfo("profile"); } }
    #region Login
    /// <summary>
    /// Set the username, password, and profile in
    /// the PlayerPrefs
    /// </summary>
    /// <param name="userInfo"></param>
    public static void SetLogin(string username, string password, string profile)
    {
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("password", password);
        PlayerPrefs.SetString("profile", profile);
    }

    /// <summary>
    /// Returns the login value from the PlayerPrefs
    /// </summary>
    /// <param name="key">The name of the value to get</param>
    /// <returns></returns>
    public static string GetLoginInfo(string key)
    {
        return PlayerPrefs.GetString(key);
    }
    #endregion

    #region Settings
    /// <summary>
    /// Attribute for Pass Threshold
    /// </summary>
    public static float PassThreshold { get { return PlayerPrefs.GetFloat("PassThreshold", DEFAULT_PASS); } set { PlayerPrefs.SetFloat("PassThreshold", value); } }
    /// <summary>
    /// Attribute for Duration
    /// </summary>
    public static float Duration { get { return PlayerPrefs.GetFloat("Duration", DEFAULT_DURATION); } set { PlayerPrefs.SetFloat("Duration", value); } }
    /// <summary>
    /// Attribute for Target Power
    /// </summary>
    public static float TargetPower { get { return PlayerPrefs.GetFloat("TargetPower", DEFAULT_POWER); } set { PlayerPrefs.SetFloat("TargetPower", value); } }

    /// <summary>
    /// Attribute for the Flashing Setting
    /// </summary>
    public static bool UseFlashingButtons { get { return PlayerPrefs.GetInt("UseFlashing", DEFAULT_SSVEP) == 0; } set { PlayerPrefs.SetInt("UseFlashing", value ? 0 : 1); } }

    /// <summary>
    /// Attribute for Current Keyboard
    /// </summary>
    public static int CurrentKeyboard { get { return PlayerPrefs.GetInt("Keyboard", (int)DEFAULT_KEYBOARD); } set { PlayerPrefs.SetInt("Keyboard", value); } }

    public static string CurrentKeyboardName { get { return ((KeyboardType)CurrentKeyboard).ToString(); } }

    /// <summary>
    /// Attribute for Refresh Rate
    /// </summary>
    public static float RefreshRate { get { return PlayerPrefs.GetFloat("RefreshRate", DEFAULT_REFRESH); } set { PlayerPrefs.SetFloat("RefreshRate", value); } }

    /// <summary>
    /// Attribute for Flashing Frequency
    /// </summary>
    public static int Frequency { get { return PlayerPrefs.GetInt("Frequency", DEFAULT_FREQ); } set { PlayerPrefs.SetInt("Frequency", value); } }

    public static Color BackgroundColor
    {
        get { return new Color(PlayerPrefs.GetFloat("BGR", 0.75f), PlayerPrefs.GetFloat("BGG", 0.87f), PlayerPrefs.GetFloat("BGB", 0.93f), 1); }
        set { PlayerPrefs.SetFloat("BGR", value.r); PlayerPrefs.SetFloat("BGG", value.g); PlayerPrefs.SetFloat("BGB", value.b); }
    }
    public static Color FrameColor
    {
        get { return new Color(PlayerPrefs.GetFloat("HR", 0.87f), PlayerPrefs.GetFloat("HG", 0.99f), PlayerPrefs.GetFloat("HB", 0.102f), 0.867f); }
        set { PlayerPrefs.SetFloat("HR", value.r); PlayerPrefs.SetFloat("HG", value.g); PlayerPrefs.SetFloat("HB", value.b); }
    }

    public static Color ParentFrameColor
    {
        get { return new Color(PlayerPrefs.GetFloat("PHR", 0.973f), PlayerPrefs.GetFloat("PHG", 0), PlayerPrefs.GetFloat("PHB", 0.059f), 0.867f); }
        set { PlayerPrefs.SetFloat("PHR", value.r); PlayerPrefs.SetFloat("PHG", value.g); PlayerPrefs.SetFloat("PHB", value.b); }
    }

    /// <summary>
    /// USE WITH EXTREME CAUTION
    /// </summary>
    public static void ResetUserSettings()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion

    #region Social
    #endregion
}

