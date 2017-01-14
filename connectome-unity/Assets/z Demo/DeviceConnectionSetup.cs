using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using EmotivWrapperInterface;
using System;
using System.IO;
using UnityEngine.Events;
using EmotivImpl.Device;

namespace Connectome.Unity.Demo
{
    public class DeviceConnectionSetup : MonoBehaviour
    {
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
}
