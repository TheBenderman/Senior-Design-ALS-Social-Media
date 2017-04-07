using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// Flickers an Image element using colours. 
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ImageFlicker : SelectionHighlighter
    {
        #region Public attributes
        /// <summary>
        /// Set of colors to flicker.
        /// </summary>
        public Color[] Flicks;

        /// <summary>
        /// Time in ms to be waited betweeen a flick.
        /// </summary>
        [Range(0, 100)]
        public int Frequency = 0;

        #endregion
        #region Private attributes
        /// <summary>
        /// Holds current color index. 
        /// </summary>
        protected int FlickIndex;

        /// <summary>
        /// Holds refrence to image element. 
        /// </summary>
        private Image Image;

        Coroutine FlickRoutine; 
        #endregion
        #region GameObject overrides
        void Start()
        {
            FlickIndex = 0;
            Image = GetComponent<Image>();

            FlickRoutine = StartCoroutine(flick());
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
                yield return new WaitForSeconds((1f / ((float)Frequency)));

                if (!disable)
                {
                    OnFlick();
                }
                   
            }
        }
        #endregion

        bool disable; 

        protected virtual void OnFlick()
        {
            FlickIndex = ++FlickIndex % Flicks.Length;
            Image.color = Flicks[FlickIndex];
        }

        public override void EnableHighlight()
        {
            disable = false; 
        }

        public override void DisableHighlight()
        {
            disable = true;
            //base.DisableHighlight();
            if(Image!= null)
                Image.color = new Color(0, 0, 0, 0);

           
            //StopCoroutine(FlickRoutine); 
        }
    }

}
