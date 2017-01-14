using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace Connectome.Unity.Demo
{
    public class InputGathererButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        public UnityEvent yesAct;

        public UnityEvent noAct;

        private bool shouldTrigger = false;

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

        void Start()
        {
            StartCoroutine(EventTriggerer());
        }

        IEnumerator EventTriggerer()
        {
            while (true)
            {
                yield return new WaitForSeconds(.0f);
                if (shouldTrigger)
                {
                    yesAct.Invoke();
                }
                else
                {
                    noAct.Invoke();
                }
            }
        }

    }

}