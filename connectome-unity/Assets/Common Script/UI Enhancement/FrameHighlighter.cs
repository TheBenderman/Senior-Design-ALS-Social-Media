using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Connectome.Unity.UI
{
    /// <summary>
    /// A frame-like highlighter  
    /// </summary>
    public class FrameHighlighter : SelectionHighlighter
    {
        /// <summary>
        /// Frame color 
        /// </summary>
        public Color FrameColor;

        //TODO set frame size 
        //public float FrameSize;

        void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Image>().color = FrameColor;
            }
        }

        //TODO add validators 
    }
}
