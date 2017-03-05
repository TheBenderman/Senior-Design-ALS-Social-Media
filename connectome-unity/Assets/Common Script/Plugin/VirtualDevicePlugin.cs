using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;
using Connectome.Unity.Template;

namespace Connectome.Unity.Plugin
{
    /// <summary>
    /// Pops up virtual device 
    /// <see cref="BasicVirtualUnityDevice"/>
    /// </summary>
    public class VirtualDevicePlugin : EmotivDeviceContainer
    {
        protected override IEmotivDevice CreateDevice()
        {
            return DisplayManager.PopUpVirtualUnityDevice();
        }

        private void OnValidate()
        {
            if(DisplayManager.Instance == null)
            {
              //  Debug.LogWarning("VirtualDevicePlugin is created from PopupManager. Make sure a PopupManager exists.");
            }
        }

    }
}
