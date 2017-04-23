using Connectome.Unity.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.UI
{

    public enum HighlighterType
    {
        Frame,
        Flashing
    }

    public class HighlighterFactory : MonoBehaviour
    {
        private static HighlighterFactory Instence;

        public SelectionHighlighter[] Highlighters;
        public SelectionHighlighter[] InstantiatedHighlighters;
        public void Start()
        {
            Instence = this;
            if (InstantiatedHighlighters.Length == 0) InstantiatedHighlighters = new SelectionHighlighter[Highlighters.Length];
        }

        /// <summary>
        /// Create a Highlighter from type using generics 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T CreateHighlighter<T>(HighlighterType t) where T : SelectionHighlighter
        {
            if (!Instence.InstantiatedHighlighters[(int)t]) 
                Instence.InstantiatedHighlighters[(int)t] = Instantiate((T)Instence.Highlighters[(int)t]);
            return (T)Instence.InstantiatedHighlighters[(int)t];
        }

        /// <summary>
        /// Creates a highlighter from type. 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static SelectionHighlighter CreateHighlighter(HighlighterType t)
        {
            return Instantiate(Instence.Highlighters[(int)t]);
        }

    }
}
