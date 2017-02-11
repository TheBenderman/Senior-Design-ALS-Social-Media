using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace Connectome.Unity.UI
{
    /// <summary>
    /// Makes a selection flash different colors.
    /// TODO: possibly merge into SelectionManager? or Change name to something like SelectionColorHandler?
    /// </summary>
    public class ButtonFlicker : MonoBehaviour
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
        #endregion
        #region GameObject overrides
        /// <summary>
        /// Built in Start method
        /// </summary>
        void Start()
        {
            FlickIndex = 0;
        }
        /// <summary>
        /// Built in Update Method
        /// </summary>
        private void Update()
        {
            if (!FlickActivated && UserSettings.GetFlashingSetting())
            {
                StartFlicker();
                FlickActivated = true;
            }
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
            SetColor(SelectionManager.Instance.DefaultSelectColor);
        }
        /// <summary>
        /// Call the coroutine to start flashing
        /// </summary>
        public void StartFlicker()
        {
            StartCoroutine(flick());
        }
        /// <summary>
        /// Sets the highlighted color of the button 
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            SelectionManager.Instance.CurrentSelection.CurrentColor = color;
        }
        #endregion
        #region Coroutines
        private IEnumerator flick()
        {
            while (UserSettings.GetFlashingSetting())
            {
                yield return new WaitForSeconds((float)Interval / 1000);


                //flick 
                if (SelectionManager.Instance.CurrentSelection != null)
                {
                    FlickIndex = ++FlickIndex % Flicks.Length;
                    SetColor(Flicks[FlickIndex]);
                }

            }
            //The flashing was turned off, so reset
            SetColor(SelectionManager.Instance.DefaultSelectColor);
            FlickActivated = false;

        }
        #endregion
    }
}