using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Twitter.API;
using CoreTweet;
using System;
using UnityEngine.UI;

public class profileHomeHandler : TwitterObjects {


	public GameObject profileHomeObjects;

	public AuthenticationHandler authHandler;
	public TwitterInteractor Interactor;
	public string profileObjects = "profileObjects";


	public void showProfileHome() {
		setActiveObject(profileObjects);
	}

	public void backToTwitterHome() {
		setActiveObject(authHandler.homePage);
	}
}
