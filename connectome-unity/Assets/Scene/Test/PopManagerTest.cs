using Connectome.Unity.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Connectome.Unity.KLD
{
    public class PopManagerTest : MonoBehaviour
    {
        public PopupManager pop; 

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void popIt()
        {
           var window =  PopupManager.PopUpVirtualUnityDevice(); 
            
        }

		public void reconnectPopUp() 
		{
			var window = PopupManager.ReconnectDevice (); 
		}
    }
}
