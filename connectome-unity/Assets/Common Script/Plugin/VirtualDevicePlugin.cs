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
    /// </summary>
    public class VirtualDevicePlugin : EmotivDeviceContainer
    {
        protected override IEmotivDevice CreateDevice()
        {
            return PopupManager.PopUpVirtualUnityDevice();
        }

    }
}
