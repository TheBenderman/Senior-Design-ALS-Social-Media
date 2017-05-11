using Connectome.Unity.Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.Test
{
    public class KeyboardTestController : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SummonKeyboard()
        {
            KeyboardManager.GetInputFromKeyboard(Debug.Log);
        }
    }
}
