using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Connectome.Unity.Common
{
    /// <summary>
    /// Allows the GameObject to be dragged, or to drag another game object upon dragging this object. 
    /// </summary>
    public class Draggable : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        /// <summary>
        /// Object to drag 
        /// </summary>
        [Header("Optional")]
        public GameObject DragTarget;

        /// <summary>
        /// Enhance dragging accuracy 
        /// </summary>
        private Vector3? Offset;

        /// <summary>
        /// Drag action 
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            float x = eventData.position.x - Offset.Value.x;
            float y = eventData.position.y - Offset.Value.y;

            DragTarget.transform.position = new Vector3(x, y);
        }

        /// <summary>
        /// resetting offset before everydrag 
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
           Offset = new Vector3(eventData.pressPosition.x - DragTarget.transform.position.x, eventData.pressPosition.y - DragTarget.transform.position.y);
        }

        /// <summary>
        /// Drags self in no target is set. 
        /// </summary>
        void Start()
        {
            DragTarget = DragTarget == null ? this.gameObject : DragTarget;
        }
    }
}
