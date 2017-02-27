using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Connectome.Unity.KLD
{
    public class PopManagerTest : MonoBehaviour
    {
        public DisplayManager pop; 

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
           DisplayManager.PopUpVirtualUnityDevice(); 
            
        }

		public void reconnectPopUp() 
		{
			DisplayManager.ReconnectDevice (); 
		}
    }
}
