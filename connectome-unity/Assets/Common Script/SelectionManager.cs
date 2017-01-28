using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Template;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        [Range(0.0f, 10.0f)]
        public int WaitInterval = 2;
        /// <summary>
        /// The list of processors attached to the selection manager.
        /// </summary>
        public GameObject ProcessorList;
        /// <summary>
        /// Gets the button of the currently selected object.
        /// </summary>
        public Button CurrentSelection { get { return SelectionList[SelectedIndex].GetComponent<Button>(); } }
        /// <summary>
        /// The amount of time, in seconds, to wait before the processors are updated.
        /// </summary>
        [Range(0.0f, 10.0f)]
        public float ProcessorWaitTime;
        /// <summary>
        /// The bar that will fill in to indicate when 
        /// the current selection will be clicked.
        /// </summary>
        public Slider ProgressBar;
        /// <summary>
        /// Used to get or set the fill value of the ProgressBar.
        /// </summary>
        public float ProgressBarValue { get { return ProgressBar.value; } set { ProgressBar.value = value; } }
        public Button DisplayText;//JUST FOR TESTING PURPOSES
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
            ChangeSelection(SelectedIndex);
        }

        /// <summary>
        /// Hilights to previous selection 
        /// </summary>
        public void Previous()
        {
            SelectedIndex = (SelectedIndex - 1 + SelectionList.Length) % SelectionList.Length;
            ChangeSelection(SelectedIndex);
        }
        /// <summary>
        /// Clicks the currently selected button
        /// </summary>
        public void TriggerClick()
        {
            CurrentSelection.onClick.Invoke();
        }

        public void ResetBar()
        {
            ProgressBarValue = 0;
        }
        #endregion
        #region Private methods
        /// <summary>
        /// Hilights game object at a given index
        /// </summary>
        /// TODO validate index. 
        /// <param name="index"></param>
        private void ChangeSelection(int index)
        {
            SelectionList[index].GetComponent<Button>().Select();
            ResetProgressBar();
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
        /// Unity's built-in OnEnable method.
        /// Start the coroutine to check the attached processors
        /// </summary>
        private void OnEnable()
        {
            ChangeSelection(SelectedIndex);
            StartCoroutine(CheckProcessors());
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
        /// <summary>
        /// Set the ProgressBar back to 0
        /// </summary>
        private void ResetProgressBar()
        {
            ProgressBarValue = 0;
            ProgressBar.gameObject.SetActive(false);
            DisplayText.gameObject.SetActive(false);
        }
        /// <summary>
        /// Go through each processor and check progress
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckProcessors()
        {
            while (true)//For now
            {
                foreach (ProcessorPlugin processor in ProcessorList.GetComponentsInChildren<ProcessorPlugin>())//Potential performance issue
                {
                    Debug.Log(processor.GetPlugin());
                    if (processor.GetPlugin() != null)
                    {
                        processor.GetPlugin().Process(this);
                    }
                }
                yield return new WaitForSeconds(ProcessorWaitTime);
            }
        }
            
        #endregion
    }
}

