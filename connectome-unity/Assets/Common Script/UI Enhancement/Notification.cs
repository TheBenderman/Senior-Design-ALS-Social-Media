
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// Display a text notification that disapears after time. 
    /// </summary>
    public class Notification : DisplayObject
    {
        /// <summary>
        /// Hold text for display
        /// </summary>
        public Text text;

        /// <summary>
        /// Duration until dismissing notification
        /// </summary>
        private float Duration;

        /// <summary>
        /// Holds the notification displayed regardless of time
        /// </summary>
        private bool HoldDisplay; 
        /// <summary>
        /// Decree duration until reaching 0 to dismiss 
        /// </summary>
        private void Update()
        {
            if(isActiveAndEnabled == false)
            {
                return; 
            }

            if ((Duration -= Time.deltaTime) <= 0 && HoldDisplay == false)
            {
                DisplayManager.Dismiss(this); 
            }
            
        }

        /// <summary>
        /// Pushed 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="duration"></param>
        public virtual void PushNotification(string msg, float duration)
        {
            text.text = msg;
            Duration = duration;

            DisplayManager.Display(this); 
        }

        /// <summary>
        /// Sets notification duration
        /// </summary>
        /// <param name="duration"></param>
        public void SetDuration(float duration)
        {
            Duration = duration;
        }

        #region DisplayObject Overrides
        public override void Dismissed()
        {
            base.Dismissed();
            gameObject.SetActive(false); 
        }

        
        public override void Displayed()
        {
            base.Displayed();
            gameObject.SetActive(true);
        }
        #endregion
        /// <summary>
        /// Holds notification visitable if not dismissed 
        /// </summary>
        public void Hold()
        {
            HoldDisplay = true; 
        }

        /// <summary>
        /// Releases notification if displayed 
        /// </summary>
        public void Release()
        {
            HoldDisplay = false;
            Duration = 0;
        }

    }
}
