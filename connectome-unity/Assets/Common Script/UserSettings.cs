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
    #region Login
    /// <summary>
    /// Set the username, password, and profile in
    /// the PlayerPrefs
    /// </summary>
    /// <param name="userInfo"></param>
    public static void SetLogin(LoginInfo userInfo)
    {
        PlayerPrefs.SetString("username", userInfo.Username);
        PlayerPrefs.SetString("password", userInfo.Password);
        PlayerPrefs.SetString("profile", userInfo.Profile);
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
    public static float PassThreshold { get { return PlayerPrefs.GetFloat("PassThreshold", 0.89f); } set { PlayerPrefs.SetFloat("PassThreshold", value); } }
    /// <summary>
    /// Attribute for Duration
    /// </summary>
    public static float Duration { get { return PlayerPrefs.GetFloat("Duration", 2f); } set { PlayerPrefs.SetFloat("Duration", value); } }
    /// <summary>
    /// Attribute for Target Power
    /// </summary>
    public static float TargetPower { get { return PlayerPrefs.GetFloat("TargetPower", 0); } set { PlayerPrefs.SetFloat("TargetPower", value); } }

    /// <summary>
    /// Attribute for the Flashing Setting
    /// </summary>
    public static bool UseFlashingButtons { get { return PlayerPrefs.GetInt("UseFlashing", 1) == 0; } set { PlayerPrefs.SetInt("UseFlashing", value ? 0 : 1); } }

    /// <summary>
    /// Attribute for Current Keyboard
    /// </summary>
    public static int CurrentKeyboard { get { return PlayerPrefs.GetInt("Keyboard", 0); } set { PlayerPrefs.SetInt("Keyboard", value); } }

    public static string CurrentKeyboardName { get { return ((KeyboardType)CurrentKeyboard).ToString(); } }

    /// <summary>
    /// Attribute for Refresh Rate
    /// </summary>
    public static float RefreshRate { get { return PlayerPrefs.GetFloat("RefreshRate", 0.3f); } set { PlayerPrefs.SetFloat("RefreshRate", value); } }

    /// <summary>
    /// Attribute for Flashing Frequency
    /// </summary>
    public static int Frequency { get { return PlayerPrefs.GetInt("Frequency", 15); } set { PlayerPrefs.SetInt("Frequency", value); } }

    public static Color BackgroundColor
    {
        get { return new Color(PlayerPrefs.GetFloat("BGR", 1), PlayerPrefs.GetFloat("BGG", 1), PlayerPrefs.GetFloat("BGB", 1), 1); }
        set { PlayerPrefs.SetFloat("BGR", value.r); PlayerPrefs.SetFloat("BGG", value.g); PlayerPrefs.SetFloat("BGB", value.b); }
    }
    public static Color HighlighterColor
    {
        get { return new Color(PlayerPrefs.GetFloat("HR", 1), PlayerPrefs.GetFloat("HG", 1), PlayerPrefs.GetFloat("HB", 1), 1); }
        set { PlayerPrefs.SetFloat("HR", value.r); PlayerPrefs.SetFloat("HG", value.g); PlayerPrefs.SetFloat("HB", value.b); }
    }
    public static Color FlashingColor
    {
        get { return new Color(PlayerPrefs.GetFloat("FR", 1), PlayerPrefs.GetFloat("FG", 1), PlayerPrefs.GetFloat("FB", 1), 1); }
        set { PlayerPrefs.SetFloat("FR", value.r); PlayerPrefs.SetFloat("FG", value.g); PlayerPrefs.SetFloat("FB", value.b); }
    }
        #endregion

    #region Social
        #endregion
    }

