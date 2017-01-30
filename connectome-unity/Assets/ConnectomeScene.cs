using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.Common
{
    public class ConnectomeScene : MonoBehaviour
    {
        void Start()
        {
            UserSettings.Device = PopupManager.PopUpVirtualUnityDevice() ; //new RandomEmotivDevice(EmotivCommandType.NEUTRAL, EmotivCommandType.NEUTRAL, EmotivCommandType.NEUTRAL, EmotivCommandType.PUSH); 
            UserSettings.Reader = new BasicEmotivReader(UserSettings.Device);

            UserSettings.Reader.OnStop += (s) => Debug.Log(s) ;
           // UserSettings.Reader.OnRead += (a) => Debug.Log(a.State);
            UserSettings.Reader.Start();
            Debug.Log("Running!");
        }

        public void OnApplicationQuit()
        {
            if (UserSettings.Reader != null)
            {
                UserSettings.Reader.Stop();
            }
        }

    }

  
}
