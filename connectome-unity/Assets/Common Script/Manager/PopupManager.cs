using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Enum;
using Connectome.Unity.Plugin;
using System;
using UnityEngine.UI;
using Connectome.Unity.Keyboard;

/// <summary>
/// Pops up windows such as a Virtual Device. 
/// </summary>
public class PopupManager : MonoBehaviour
{
    #region Singleton 
    private static PopupManager instance;
		
    public static PopupManager Instance
    {
        get
        {
            return instance; 
        }
    }
    #endregion
    #region 
    public SelectionManager SelectionManager; 
    #endregion

    #region Unity Built-in
    void Awake()
    {
        instance = this; 
    }
    #endregion
    #region Popup BasicVirtualUnityDevice
    /// <summary>
    /// Holds Virtual Device prefab
    /// </summary>
    public BasicVirtualUnityDevice VirtualUnityDevicePrefab;

    /// <summary>
    /// Current popped device. 
    /// </summary>
    private BasicVirtualUnityDevice AvailableDevice; 

    public static BasicVirtualUnityDevice PopUpVirtualUnityDevice()
    {
        if(Instance.AvailableDevice != null)
        {
            return Instance.AvailableDevice; 
        }
        BasicVirtualUnityDevice pop = Instantiate(Instance.VirtualUnityDevicePrefab);
        Instance.AvailableDevice = pop;
        // Closing out of the device triggers an event to reconnect back to it.
        pop.OnDisconnectAttempted += (suc, msg) => { ReconnectDevice(); Instance.AvailableDevice = null; }; 

        pop.transform.SetParent(Instance.transform.parent);
        pop.transform.localPosition = new Vector2(0,0); 
        return pop; 
    }
    #endregion
    #region Popup ReconnectDeviceWindow
    /// <summary>
    /// Holds Virtual Device prefab
    /// </summary>
    public ReconnectDeviceWindow ReconnectDeviceWindowPrefab;

	public static ReconnectDeviceWindow ReconnectDevice(GameObject config = null)
	{
		ReconnectDeviceWindow recon = Instantiate(Instance.ReconnectDeviceWindowPrefab);
		recon.transform.SetParent(Instance.transform.parent);
		recon.transform.localPosition = new Vector2(0,0); 
		return recon; 
	}
    #endregion

    #region 

    public KeyboardManager KeyboardManager;

    public static void GetInputFromKeyboard(Action<string> onSubmit)
    {
        //KeyboardPrompt keyboardPrompt = Instantiate(Instance.PromptKeyboardPrefab, Instance.transform.parent);

        //keyboardPrompt.transform.localPosition = new Vector2(0, 0);
        Instance.KeyboardManager.Show(); 

        onSubmit += (s) => Instance.SelectionManager.Pop();

        Instance.KeyboardManager.Prompt(onSubmit);
        Instance.SelectionManager.Push(Instance.KeyboardManager.Keyboard); 
    }

    #endregion
}

