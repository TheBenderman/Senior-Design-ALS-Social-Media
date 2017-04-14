using Connectome.Unity.Menu;
using Connectome.Unity.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Connectome.Unity.Keyboard
{
    public abstract class KeyboardTemplate : SelectionMenuContainer
    {
        private Action<string> OnSubmit;
        /// <summary>
        /// The text field used to display/hold the text.
        /// </summary>
        public InputField InputField;
        public string SubmissionText { get { return InputField.text; } }
        public virtual void AddSubmitAction(Action<string> submitAction)
        {
            OnSubmit += submitAction;
        }
        public virtual void Submit()
        {
            OnSubmit(SubmissionText);
        }

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

            Rect.offsetMin = new Vector2(Rect.offsetMin.x, 0);
            Rect.offsetMax = new Vector2(Rect.offsetMax.x, 0);
            Rect.offsetMin = new Vector2(Rect.offsetMin.y, 0);
            Rect.offsetMax = new Vector2(Rect.offsetMax.y, 0);

            gameObject.SetActive(true);
            InputField.text = ""; 
        }

        /// <summary>
        /// deactivate gameobject when popped. 
        /// </summary>
        public override void Popped()
        {
            base.Popped();
            gameObject.SetActive(false); 
        }
    }
}
