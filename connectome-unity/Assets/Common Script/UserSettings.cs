using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserSettings{
    #region Login
    /// <summary>
    /// Set the username, password, and profile in
    /// the PlayerPrefs
    /// </summary>
    /// <param name="userInfo"></param>
    public static  void SetLogin(LoginInfo userInfo)
    {
        PlayerPrefs.SetString("username", userInfo.Login);
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
    /// Set the Pass Threshold in PlayerPrefs
    /// </summary>
    /// <param name="value"></param>
    public static void SetPassThreshold(float value)
    {
        PlayerPrefs.SetFloat("passThreshold", value);
    }

    /// <summary>
    /// Set the Duration in PlayerPrefs
    /// </summary>
    /// <param name="value"></param>
    public static void SetDuration(float value)
    {
        PlayerPrefs.SetFloat("duration", value);
    }

    /// <summary>
    /// Sets the Trigger Threshold in PlayerPrefs
    /// </summary>
    /// <param name="value"></param>
    public static void SetTriggerThreshold(float value)
    {
        PlayerPrefs.SetFloat("triggerThreshold", value);
    }

    /// <summary>
    /// Returns the settings value from PlayerPrefs
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static float GetSettingsValue(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }
    #endregion

    #region Social
    #endregion
}
