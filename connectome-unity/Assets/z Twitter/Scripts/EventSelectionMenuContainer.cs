using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Connectome.Twitter.API;
using UnityEngine.UI;
using Connectome.Unity.Menu;
using UnityEngine.Events;


public class EventSelectionMenuContainer : SelectionMenuContainer
{
	public UnityEvent OnPushed;
	public UnityEvent OnPopped;
	public UnityEvent OnResumed;
	public UnityEvent OnPaused;

	public override void Popped()
	{
		base.Popped ();
		if (OnPopped != null) {
			OnPopped.Invoke ();
		}
	}

	public override void Pushed()
	{
		base.Pushed ();
		if (OnPushed != null) {
			OnPushed.Invoke ();
		}	
	}

	public override void Paused()
	{
		base.Paused ();
		if (OnPaused != null) {
			OnPaused.Invoke ();
		}	
	}

	public override void Resumed()
	{
		base.Resumed ();
		if (OnResumed != null) {
			OnResumed.Invoke ();
		}
	}


}

