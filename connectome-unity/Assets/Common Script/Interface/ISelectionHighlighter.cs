using UnityEngine;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// Represnts a highlighter that can highlight elements in a selection menu.
    /// <seealso cref="ISelectionMenu"/>
    /// </summary>
    public interface ISelectionHighlighter
    {
        /// <summary>
        /// Highlight game object.
        /// </summary>
        /// <param name="go"></param>
        void Highlight(GameObject go);

        /// <summary>
        /// Enable highlighting.
        /// </summary>
        void EnableHighlight();

        /// <summary>
        /// Disable highlighting.
        /// </summary>
        void DisableHighlight();
    }
}
