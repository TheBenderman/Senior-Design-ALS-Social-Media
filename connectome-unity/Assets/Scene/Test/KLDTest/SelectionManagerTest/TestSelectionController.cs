using Connectome.Unity.Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.KLD.SelectionTest
{
    public class TestSelectionController : MonoBehaviour
    {
        public void CallKey(Text text)
        {
            KeyboardManager.GetInputFromKeyboard(s => text.text = s); 
        }
    }
}
