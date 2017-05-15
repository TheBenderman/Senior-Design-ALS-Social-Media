using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.Menu
{
    /// <summary>
    /// Functions similar to a ButtonSelectionMenu, except when this menu is selected, resize itself and
    /// children. Assumes a horizontal layout group is being used for the children.
    /// </summary>
    [RequireComponent(typeof(UnityEngine.UI.HorizontalLayoutGroup))]
    public class ExpandingButtonSelectionMenu : ButtonSelectionMenu
    {
        #region Private Variables
        private float DefaultWidth;
        private float ExpandedWidth;
        private float DefaultChildrenWidth;
        private float ExpandedChildrenWidth;
        #endregion
        #region Public Variables
        /// <summary>
        /// After this button is clicked/popped, hide/show the gameObjects in this list.
        /// </summary>
        public GameObject[] ButtonsToHide;
        #endregion
        #region Unity Methods
        private void Start()
        {
            DefaultWidth = GetComponent<RectTransform>().rect.width;
            ExpandedWidth = Screen.width - 10;
            DefaultChildrenWidth = Selection[0].GetComponent<RectTransform>().rect.width;
            ExpandedChildrenWidth = DefaultChildrenWidth + 40; //There is possibly a better way to get the buttons to scale correctly.
        }
        #endregion
        #region SelectionMenu Overrides
        public override void Pushed()
        {
            base.Pushed();
            ConfigureButtonSizes(ExpandedWidth, ExpandedChildrenWidth);
            //Hide the parent's siblings to give more room to this object's children on the screen.
            foreach (GameObject obj in ButtonsToHide)
            {
                obj.SetActive(false);
            }
        }

        public override void Popped()
        {
            base.Popped();
            ConfigureButtonSizes(DefaultWidth, DefaultChildrenWidth);
            //Bring the hidden siblings back to revert the look to what it was before.
            foreach (GameObject obj in ButtonsToHide)
            {
                obj.SetActive(true);
            }
        }
        #endregion
        #region private Methods
        /// <summary>
        /// Adjust the width of the parent and children object widths.
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="ChildWidth"></param>
        /// <param name="show"></param>
        private void ConfigureButtonSizes(float Width, float ChildWidth)
        {
            RectTransform rt = GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(Width, rt.rect.height);
            foreach (UnityEngine.UI.Button b in Selection)
            {
                RectTransform brt = b.GetComponent<RectTransform>();
                brt.sizeDelta = new Vector2(ChildWidth, brt.rect.height);
            }
        }
#endregion
    }
}
