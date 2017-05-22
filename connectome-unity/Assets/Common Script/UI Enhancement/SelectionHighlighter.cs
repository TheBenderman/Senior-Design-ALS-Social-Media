using UnityEngine;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// A highlighter that abjust it's size and position to the highlighted object. 
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SelectionHighlighter : MonoBehaviour, ISelectionHighlighter
    {
        #region Rect Singleton 
        /// <summary>
        /// GameObject's RectTransform
        /// </summary>
        private RectTransform Rect;
        /// <summary>
        /// Singleton return of rect component. 
        /// </summary>
        /// <returns></returns>
        private RectTransform GetRect()
        {
            if (Rect == null)
            {
                Rect = GetComponent<RectTransform>();
            }

            return Rect;
        }
        #endregion 
        #region ISelectionHighlighter Overrides
        /// <summary>
        /// Move it's position over the gameobject and adjust size while keeping it's relative parent. 
        /// </summary>
        /// <param name="go"></param>
        public virtual void Highlight(GameObject go)
        {
            //remember your father 
            Transform dad = transform.parent;
             
            //clone size 
            transform.SetParent(go.transform);

            RectTransform Rect = GetRect(); 

            Rect.anchorMax = new Vector2(1, 1);
            Rect.anchorMin = new Vector2(0f, 0f);
            Rect.sizeDelta = new Vector2(0f, 0f);
           
            Rect.offsetMin = new Vector2(Rect.offsetMin.x, 0);
            Rect.offsetMax = new Vector2(Rect.offsetMax.x, 0);
            Rect.offsetMin = new Vector2(Rect.offsetMin.y, 0);
            Rect.offsetMax = new Vector2(Rect.offsetMax.y, 0);

            Rect.localScale = new Vector3(1f,1f,1f);

            //go to position 
            transform.SetParent(dad);
        }

        /// <summary>
        /// Enables highlighter 
        /// </summary>
        public virtual void EnableHighlight()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Disable highlighter 
        /// </summary>
        public virtual void DisableHighlight()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
