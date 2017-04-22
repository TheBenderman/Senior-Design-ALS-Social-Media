using Connectome.Unity.Menu;
using Connectome.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Connectome.Unity.Keyboard
{
    public abstract class KeyboardTemplate : SelectionMenu
    {
        /// <summary>
        /// Invoked when Keyboard submits text. 
        /// </summary>
        public event Action<string> OnSubmit;
        
        /// <summary>
        /// Gets text that will be submitted 
        /// </summary>
        public abstract string SubmissionText { get; }

        /// <summary>
        /// Submits keyboard 
        /// </summary>
        public abstract void Submit();
       
        /// <summary>
        /// reset Text field and enable game object when pushed by SelectionManager. 
        /// </summary>
        public override void Pushed()
        {
            base.Pushed();

            ///resize
            RectTransform Rect = GetComponent<RectTransform>();
            Rect.anchorMax = new Vector2(1, 1);
            Rect.anchorMin = new Vector2(0f, 0f);
            Rect.sizeDelta = new Vector2(0f, 0f);

            Rect.offsetMin = new Vector2(0, 0);
            Rect.offsetMax = new Vector2(0, 0);
            
            Rect.localScale = new Vector3(1, 1, 1);

            gameObject.SetActive(true);
        }

        /// <summary>
        /// deactivate gameobject when popped. 
        /// </summary>
        public override void Popped()
        {
            base.Popped();
            gameObject.SetActive(false); 
        }

        protected void InvokeOnSubmit()
        {
            if(OnSubmit!= null)
            {
                OnSubmit(SubmissionText); 
            }
        }
    }
}
