using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace Connectome.Unity.Demo
{
    /// <summary>
    /// Calls respective action: yes or no. 
    /// </summary>
    public class InputGathererButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        #region Public attributes 
        /// <summary>
        /// Hold events to be called when acting upon 'yes'
        /// </summary>
        public UnityEvent yesAct;
        /// <summary>
        /// Hold events to be called when acting upon 'no'
        /// </summary>
        public UnityEvent noAct;
        #endregion
        #region Private attributes 
        /// <summary>
        /// Decides whether to trigger a 'yes' or no'. 
        /// </summary>
        private bool shouldTrigger = false;
        #endregion
        #region Interfaces 
        public void OnPointerDown(PointerEventData eventData)
        {
            shouldTrigger = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            shouldTrigger = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            shouldTrigger = false;
        }
        #endregion
        #region GameObject override  
        /// <summary>
        /// triggers 'Yes' or 'No' based on flag 
        /// </summary>
        void Update()
        {
            if (shouldTrigger)
            {
                yesAct.Invoke();
            }
            else
            {
                noAct.Invoke();
            }
        }
        #endregion
    }
}