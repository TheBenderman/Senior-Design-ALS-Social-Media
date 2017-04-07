using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Connectome.Unity.UI;


namespace Connectome.Unity.UI
{
    /// <summary>
    /// Makes a selection flash different colors.
    /// TODO: possibly merge into SelectionManager? or Change name to something like SelectionColorHandler?
    /// </summary>
    public class ButtonFlicker : SelectionHighlighter
    {
        #region Public attributes
        /// <summary>
        /// Set of colors to cycle through.
        /// </summary>
        public Color[] Flicks;

        /// <summary>
        /// Time in ms to be waited betweeen a flash.
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
        /// Checks if we called the coroutine yet.
        /// </summary>
        private bool FlickActivated = false;

        private Button CurrentButton;
        private Color DefaultColor; 

        #endregion
        #region GameObject overrides
        /// <summary>
        /// Built in Start method
        /// </summary>
        void Start()
        {
            FlickIndex = 0;
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

        }
        #endregion
        #region Public Methods
        /// <summary>
        /// Updates the current button and sets the color(in case the flashing stopped on a wrong color
        /// </summary>
        public void UpdateSelection()
        {
            //SetColor(SelectionManager.Instance.DefaultSelectColor);
        }

        /// <summary>
        /// Sets the highlighted color of the button 
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            CurrentButton.image.color = color; 
        }

        public override void Highlight(GameObject go)
        {
            if(CurrentButton != null)
            {
                CurrentButton.image.color = DefaultColor; 
            }

            CurrentButton =  go.GetComponent<Button>();
            DefaultColor = CurrentButton.image.color; 
        }
        #endregion
        #region Coroutines
        private IEnumerator flick()
        {
            while (true)
            {
                yield return new WaitForSeconds((float)Interval / 1000);


                if (CurrentButton == null)
                    continue; 

                FlickIndex = ++FlickIndex % Flicks.Length;
                SetColor(Flicks[FlickIndex]);

            }
        }
        #endregion
    }
}