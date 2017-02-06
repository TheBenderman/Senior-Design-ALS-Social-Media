using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


namespace Connectome.Unity.UI
{
    /// <summary>
    /// Makes a button flash different colors.
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
        /// <summary>
        /// The Associated Selection Manager.
        /// Used to get the current button.
        /// </summary>
        public SelectionManager Manager;

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

        /// <summary>
        /// Holds refrence to image element. 
        /// </summary>
        private Button button;
        /// <summary>
        /// Use this to set the button to the correct colors
        /// </summary>
        public Button CurrentButton { get { return button; } }
        /// <summary>
        /// The button's current color block. Needs to be saved because we can't reference it in a single line.
        /// </summary>
        private ColorBlock ButtonColor;
        /// <summary>
        /// The default color we want buttons to be(if flash is turned off)
        /// </summary>
        public Color defaultColor;
        #endregion
        #region GameObject overrides
        void Start()
        {
            FlickIndex = 0;
        }

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
            button = Manager.CurrentSelection;
            ButtonColor = CurrentButton.colors;
            SetColor(defaultColor);
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
            ButtonColor.highlightedColor = color;
            CurrentButton.colors = ButtonColor;
        }
        #endregion
        #region Coroutines
        private IEnumerator flick()
        {
            while (UserSettings.GetFlashingSetting())
            {
                yield return new WaitForSeconds((float)Interval / 1000);


                //flick 
                if (CurrentButton != null)
                {
                    FlickIndex = ++FlickIndex % Flicks.Length;
                    SetColor(Flicks[FlickIndex]);
                }

            }
            //The flashing was turned off, so reset
            SetColor(defaultColor);
            FlickActivated = false;

        }
        #endregion
    }
}