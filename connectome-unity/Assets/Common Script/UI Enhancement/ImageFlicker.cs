using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// Flickers an Image element using colours. 
    /// </summary>
    public class ImageFlicker : MonoBehaviour
    {
        #region Public attributes
        /// <summary>
        /// Set of colors to flicker.
        /// </summary>
        public Color[] Flicks;

        /// <summary>
        /// Time in ms to be waited betweeen a flick.
        /// </summary>
        [Tooltip("Flicking interval in millisecond")]
        [Range(0, 1000)]
        public int Interval = 0;

        #endregion
        #region Private attributes
        /// <summary>
        /// Holds current color index. 
        /// </summary>
        private int FlickIndex;

        /// <summary>
        /// Holds refrence to image element. 
        /// </summary>
        private Image Image;
        #endregion
        #region GameObject overrides
        void Start()
        {
            FlickIndex = 0;
            Image = GetComponent<Image>();

            StartCoroutine(flick());
        }

        void OnValidate()
        {
            //colours check 
            if (Flicks == null)
            {
                Debug.LogError("Flicker colors are null", this);
            }
            else if (Flicks.Length == 0)
            {
                Debug.LogError("Flicker colors are empty", this);
            }

            //image comp check
            if (GetComponent<Image>() == null)
            {
                Debug.LogError("Missing 'Image' component", this);
            }

        }
        #endregion
        #region Coroutines
        private IEnumerator flick()
        {
            while (true)
            {
                yield return new WaitForSeconds((float)Interval / 1000);


                //flick 
                FlickIndex = ++FlickIndex % Flicks.Length;
                Image.color = Flicks[FlickIndex];

            }


        }
        #endregion
    }
}
