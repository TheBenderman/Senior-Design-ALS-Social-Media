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
			ParentHighlighter.Highlight (go.transform.parent.gameObject);
			Highlighter.Highlight(go);
		}

		public override void EnableHighlight() 
		{
			ParentHighlighter.EnableHighlight ();
			Highlighter.EnableHighlight();
		}

		public override void DisableHighlight() 
		{
			ParentHighlighter.DisableHighlight ();
			Highlighter.DisableHighlight();
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
