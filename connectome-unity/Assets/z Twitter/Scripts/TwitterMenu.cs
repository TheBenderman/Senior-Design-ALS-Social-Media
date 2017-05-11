using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Connectome.Twitter.API;
using UnityEngine.UI;
using Connectome.Unity.Menu;


public class TwitterMenu : ButtonSelectionMenu
{
	public bool clone; 
	public override void Popped()
	{
		base.Popped ();	
		this.gameObject.SetActive (false);
	}

	public override void Pushed()
	{
		base.Pushed ();
		this.gameObject.SetActive (true);
	}

	public override void Paused()
	{
		base.Paused ();
		this.gameObject.SetActive (false);
	}

	public override void Resumed()
	{
		base.Resumed ();
		this.gameObject.SetActive (true);
	}

	void OnValidate()
	{
		ButtonSelectionMenu m = GetComponents<ButtonSelectionMenu> ()[0]; 
		if (m != null && clone) 
		{
			this.Selection = new Button[m.Selection.Length];
			int i = 0; 
			foreach (var b in m.Selection)
			{
				this.Selection [i++] = b; 
			}

		}
	}
}

