using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.Common
{
    /// <summary>
    /// Pops up windows such as a Virtual Device. 
    /// </summary>
    public class PopupManager : MonoBehaviour
    {
        #region Singleton 
        private static PopupManager instence;

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

            pop.transform.SetParent(Instence.transform.parent);
            pop.transform.localPosition = new Vector2(0,0); 
            return pop; 
        }
        #endregion
    }
}
