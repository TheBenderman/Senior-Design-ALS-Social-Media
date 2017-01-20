using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.Common
{
    /// <summary>
    /// Allows highlighting a selection of game objects and surfing* between them.
    /// </summary>
    public class SelectionManager : MonoBehaviour
    {
        #region Public attributes
        /// <summary>
        /// Hold selectable game objects 
        /// </summary>
        public GameObject[] SelectionList;
        /// <summary>
        /// The time, in seconds, to wait before the selection changes.
        /// </summary>
        public int WaitInterval = 2;
        /// <summary>
        /// Hilights currect selection. 
        /// </summary>
        public GameObject Highlighter;
        #endregion
        #region Private attributes
        /// <summary>
        /// Hold currently selected element. 
        /// </summary>
        private int SelectedIndex = 0;
        /// <summary>
        /// The current interval
        /// </summary>
        private float CurrentWait = 0;
        #endregion
        #region Public methods
        /// <summary>
        /// Hilights to next selection 
        /// </summary>
        public void Next()
        {
            SelectedIndex = (SelectedIndex + 1) % SelectionList.Length;
            Select(SelectedIndex);
        }

        /// <summary>
        /// Hilights to previous selection 
        /// </summary>
        public void Previous()
        {
            SelectedIndex = (SelectedIndex - 1 + SelectionList.Length) % SelectionList.Length;
            Select(SelectedIndex);
        }
        #endregion
        #region Private methods
        /// <summary>
        /// Hilights game object at a given index
        /// </summary>
        /// TODO validate index. 
        /// <param name="index"></param>
        private void Select(int index)
        {
            SelectedIndex = index;
            Highlighter.transform.SetParent(SelectionList[index].transform);
            Highlighter.transform.localPosition = new Vector2(0, 0);
            Highlighter.SetActive(true);
            Debug.Log("The selected element = " + SelectionList[index].name);
        }

        /// <summary>
        /// Unity's built-in Update method.
        /// Keep track of the application's time, and
        /// change the selection after the interval time passes.
        /// </summary>
        private void Update()
        {
            CurrentWait += Time.deltaTime;
            if(CurrentWait >= WaitInterval)
            {
                Next();
                ResetInterval();
            }
        }

        /// <summary>
        /// Sets the current wait time back to 0.
        /// Can be used to refresh the waiting time when the user
        /// tries to select an option.
        /// </summary>
        private void ResetInterval()
        {
            CurrentWait = 0;
        }
        #endregion
    }
}

