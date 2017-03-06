using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Twitter.API;
using CoreTweet;
using System;

public class profileHomeHandler : TwitterObjects {


	public GameObject profileHomeObjects;

	public TimeLineHandler timeline;
	public AuthenticationHandler authHandler;
	public TwitterInteractor Interactor;

	private List<Status> UserTimeline;
	public List<Status> usertimelineStatuses {
		get { return UserTimeline; }
		set { UserTimeline = value; }
	}

	private int currentTweet = 0;
	public string userTimeLineTitle = "Profile";
	public string profileObjects = "profileObjects";


	public void showProfileHome() {
		setActiveObject(profileObjects);
	}

	public void backToTwitterHome() {
		setActiveObject(authHandler.homePage);
	}

	public void showUserTimeLine() {
		try {
			// Set all objects to be invisible except those related to the timeline.
			setActiveObject(timeline.homeTimeLineObjectsString);
			if (authHandler.Interactor == null) {
			}
			//Debug.Log();
			// Get the tweets for the user.
			usertimelineStatuses = authHandler.makeTwitterAPICall( () => authHandler.Interactor.getLoggedInUserTimeline());
			currentTweet = 0;
			setTweet(currentTweet);
			timeline.TitleView.text = userTimeLineTitle;
		}
		catch (Exception e) {
			Debug.Log ("Something went wrong!");
		}
	}

	// This function sets the current tweet for the user.
	public void setTweet(int index)
	{
		timeline.twitterHandle.text = usertimelineStatuses[index].User.ScreenName;
		timeline.realName.text = usertimelineStatuses[index].User.Name;
		timeline.bodyText.text = usertimelineStatuses[index].Text;

		// Populate the profile picture for the user, requires a separate thread to run.
		StartCoroutine(setProfilePic(usertimelineStatuses[index].User.ProfileImageUrl));
	}

	public IEnumerator setProfilePic(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		timeline.profilePic.sprite = Sprite.Create(
			www.texture,
			new Rect(0, 0, www.texture.width, www.texture.height),
			new Vector2(0, 0));
	}

	// This function populates the timeline ui with the next tweet in the list.
	public void nextTweet()
	{
		// Skip this onclick if the scene is on something other than the usertimeline
		if (!timeline.TitleView.text.Equals (userTimeLineTitle))
			return;

		if (currentTweet < usertimelineStatuses.Count - 1)
		{
			currentTweet += 1;
		}
		else
		{
			usertimelineStatuses = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getLoggedInUserTimeline());
		}

		setTweet(currentTweet);
	}

	// This function populates the timeline ui with the last tweet in the list.
	public void previousTweet()
	{
		// Skip this onclick if the scene is on something other than the usertimeline
		if (!timeline.TitleView.text.Equals (userTimeLineTitle))
			return;

		if (currentTweet > 0)
		{
			currentTweet--;
		}

		setTweet(currentTweet);
	}
}
