using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Enum;

namespace Connectome.Unity.Common
{
    /// <summary>
    /// Pops up windows such as a Virtual Device. 
    /// </summary>
    public class PopupManager : MonoBehaviour
    {
        #region Singleton 
        private static PopupManager instence;
		public IEmotivDevice Device;

        public static PopupManager Instence
        {
            get
            {
                return instence; 
            }
        }
        #endregion


        #region Unity Built-in
        void Start()
        {
            instence = this; 
        }
        #endregion
        #region Popup BasicVirtualUnityDevice
        /// <summary>
        /// Holds Virtual Device prefab
        /// </summary>
        public BasicVirtualUnityDevice VirtualUnityDevicePrefab;

        public static BasicVirtualUnityDevice PopUpVirtualUnityDevice(GameObject config = null)
        {
            BasicVirtualUnityDevice pop = Instantiate(Instence.VirtualUnityDevicePrefab);
			// Closing out of the device triggers an event to reconnect back to it.
			pop.OnDisconnectSucceed += (msg) => ReconnectDevice();
			Instence.Device = pop; 
            pop.transform.SetParent(Instence.transform.parent);
            pop.transform.localPosition = new Vector2(0,0); 
            return pop; 
        }
        #endregion
		#region Popup BasicVirtualUnityDevice
		/// <summary>
		/// Holds Virtual Device prefab
		/// </summary>
		public ReconnectDeviceWindow ReconnectDeviceWindowPrefab;

		public static ReconnectDeviceWindow ReconnectDevice(GameObject config = null)
		{
			ReconnectDeviceWindow recon = Instantiate(Instence.ReconnectDeviceWindowPrefab);
			recon.transform.SetParent(Instence.transform.parent);
			recon.transform.localPosition = new Vector2(0,0); 
			return recon; 
		}
		#endregion
    }
}
