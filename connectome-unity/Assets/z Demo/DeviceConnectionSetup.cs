using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Events;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Implementation;

namespace Connectome.Unity.Demo
{
    /// <summary>
    /// Able to create a device. 
    /// </summary>
    public class DeviceConnectionSetup : MonoBehaviour
    {
        #region Public attributes

        /// <summary>
        /// Holds a connectable device. 
        /// </summary>
        [HideInInspector()]
        public IEmotivDevice Device;

        /// <summary>
        /// title to display connection failed reasons. 
        /// </summary>
        public Text title;

        /// <summary>
        /// Holds events that will be called when connection succeeds. 
        /// </summary>
        public UnityEvent OnSucuess;

        /// <summary>
        /// Holds username Input Field
        /// </summary>
        public InputField Username;

        /// <summary>
        /// Holds password Input Field
        /// </summary>
        public InputField Password;

        /// <summary>
        /// Holds profile Input Field
        /// </summary>
        public InputField Profile;

        #endregion
        #region Private attributes
        private bool IsRandom = false;
        #endregion
        #region Public mothods

        /// <summary>
        /// Creates device either to random or Epoc device and insures connectivity. 
        /// Call 'OnSuccess' when connection suceesd.
        /// </summary>
        public void Connect()
        {
            try
            {
                if (IsRandom)
                {
                    Device = new Emotiv.Implementation.RandomEmotivDevice();
                }
                else
                {
                    Device = new Emotiv.Implementation.EPOCEmotivDevice(Username.text, Password.text, Profile.text);
                }

                string error;
                bool suc = Device.Connect(out error);

                if (suc)
                {
                    title.text = "Connected!";
                    OnSucuess.Invoke();
                }
                else
                {
                    title.text = error;
                }
            }
            catch (Exception e)
            {
                title.text = e.ToString();
            }
        }

        /// <summary>
        /// sets flag indecating wether random device is needed or not. 
        /// </summary>
        /// <param name="b"></param>
        public void SetEnableRandomDevice(bool b)
        {
            IsRandom = b;
        }
        #endregion
    }
}
