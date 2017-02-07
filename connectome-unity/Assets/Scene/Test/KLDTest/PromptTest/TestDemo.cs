using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.KLD.PromptTest
{
    /// <summary>
    /// Demostraim a way to get string from a popup (also keyboard) 
    /// </summary>
    public class TestDemo : MonoBehaviour
    {
        public PromptPopup prompt;

        /// <summary>
        /// Sets text on screen called from PopupButton
        /// </summary>
        /// <param name="text"></param>
        public void PutTextFromPrompt(Text text)
        { 
            //lambda could be replaced with s => TwitterClient.Tweet(s)
            prompt.Popup(s => text.text = s);
        }
    }
}
