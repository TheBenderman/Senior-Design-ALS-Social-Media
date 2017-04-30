using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Unity.UI;

namespace Connectome.Unity.UI {
	public class DualSelectionHighlighter : SelectionHighlighter {

		public SelectionHighlighter Highlighter;  
		public SelectionHighlighter ParentHighlighter;  

		public override void Highlight(GameObject go) 
		{
			Highlighter.Highlight(go);
			// Highlight the parent of the current object.
			ParentHighlighter.Highlight (go.transform.parent.gameObject);
		}

		public override void EnableHighlight() 
		{
			Highlighter.EnableHighlight();
			ParentHighlighter.EnableHighlight ();
		}

		public override void DisableHighlight() 
		{
			Highlighter.DisableHighlight();
			ParentHighlighter.DisableHighlight ();
		}

		private void OnValidate()
		{
			if(ParentHighlighter == null || Highlighter == null)
			{
				Debug.LogError("Both highlighters cannot be null."); 
			}
			if(Highlighter == this)
			{
				Highlighter = null; 
				Debug.LogError("DualSelectionHighlighter cannot be set to self."); 
			}
			if(ParentHighlighter == this)
			{
				Debug.LogError("DualSelectionHighlighter cannot be set to self."); 
			}
			if(ParentHighlighter == Highlighter)
			{
				Debug.LogError("Parent highlighter cannot be the same as the current highlighter."); 
			}

		}


	}
}
