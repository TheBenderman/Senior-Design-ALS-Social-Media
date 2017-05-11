using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Connectome.Twitter.API;
using UnityEngine.UI;
using Connectome.Unity.Menu;


public class MultipleTweetsTwitterMenu : ButtonSelectionMenu
{

	public override void Popped()
	{
		// Don't hide the base object when popped because it contains the
		// list of 4 tweets as well as the back button.
		base.Popped ();	
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
}

