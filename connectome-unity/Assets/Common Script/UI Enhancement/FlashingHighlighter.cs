using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// Flickers an Image element using colours. 
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class FlashingHighlighter : SelectionHighlighter
    {
        #region Public attributes
        /// <summary>
        /// Set of colors to flicker.
        /// </summary>
        public Color[] FlashingColors;

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
        protected int FlashIndex;

        /// <summary>
        /// Holds refrence to image element. 
        /// </summary>
        private Image ImageComponent;

        #endregion
        #region GameObject overrides
        void Start()
        {
            FlashIndex = 0;
            ImageComponent = GetComponent<Image>();
    
        }

        private void OnEnable()
        {
            StartCoroutine(flick());
        }

        void OnValidate()
        {
            //colours check 
            if (FlashingColors == null)
            {
                Debug.LogError("Flicker colors are null", this);
            }
            else if (FlashingColors.Length == 0)
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

        public bool disable; 

        protected virtual void OnFlick()
        {
            FlashIndex = ++FlashIndex % FlashingColors.Length;
            ImageComponent.color = FlashingColors[FlashIndex];
        }

        public override void EnableHighlight()
        {
            disable = false; 
        }

        public override void DisableHighlight()
        {
            disable = true;
            //base.DisableHighlight();
            if(ImageComponent!= null)
                ImageComponent.color = new Color(0, 0, 0, 0);


        }
    }

}
