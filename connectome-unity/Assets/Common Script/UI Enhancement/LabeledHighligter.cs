using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connectome.Unity.UI
{

    public class LabeledHighligter : SelectionHighlighter
    {
        public string HighlightedName { get; private set; }
        public SelectionHighlighter Highlighter;  

        private void Start()
        {
            HighlightedName = Highlighter.transform.parent.name; 
        }

        public override void Highlight(GameObject go)
        {
            Highlighter.Highlight(go);
            HighlightedName = go.name;
        }

        public override void DisableHighlight()
        {
            Highlighter.DisableHighlight();
        }

        public override void EnableHighlight()
        {
            Highlighter.EnableHighlight();
        }

        private void OnValidate()
        {
            if(Highlighter == this)
            {
                Highlighter = null; 
                Debug.LogError("LabeledFlashingHighligter cannot be set to self."); 
            }
        }

    }
}
