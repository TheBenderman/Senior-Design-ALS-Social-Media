using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EmotivWrapperInterface;
using System;
using System.IO;
using UnityEngine.Events;
using EmotivImpl.Device;

public class DeviceConnectionSetup : MonoBehaviour {

    [HideInInspector()]
    public IEmotivDevice device;

    public Text text;
    public UnityEvent OnSucuess;

    public InputField username;
    public InputField password;
    public InputField profile;

    private bool isRandom = false; 

    public void Connect()
    {
        try
        {
            if (isRandom)
            {
                device = new RandomEmotivDevice();
            }
            else
            {
                device = new EPOCEmotivDevice(username.text, password.text, profile.text);
            }

            string error;
            bool suc = device.ConnectionSetUp(out error);

            if (suc)
            {
                text.text = "Connected!";
                OnSucuess.Invoke(); 
            }
            else
            {
                text.text = error;
            }

            
        }
        catch (Exception e)
        {
            text.text = e.ToString(); 
        }
    }

    public void SetEnableRandomDevice(bool b)
    {
        isRandom = b; 
    }
}



/* Fuck this dlls
 * void Awake()
   {
       String currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
       String dllPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Assets" + Path.DirectorySeparatorChar + "Plugins";
       Debug.Log("added " + currentPath + Path.PathSeparator + dllPath);
       if (!currentPath.Contains(dllPath))
       {
           Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
           //Debug.Log("added " + currentPath + Path.PathSeparator + dllPath);
       }

        dllPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Assets" + Path.DirectorySeparatorChar + "lib";
       Debug.Log("added " + currentPath + Path.PathSeparator + dllPath);
       if (!currentPath.Contains(dllPath))
       {
           Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
           //Debug.Log("added " + currentPath + Path.PathSeparator + dllPath);
       }

       Debug.Log(Environment.CurrentDirectory);
   } */
