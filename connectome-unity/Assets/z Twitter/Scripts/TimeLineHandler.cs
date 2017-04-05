using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Unity.Menu;
using UnityEngine.UI;
using CoreTweet;
using Connectome.Twitter.API;
using System;

public class TimeLineHandler: TwitterObjects
{
	#region Twitter Home Timeline Members
	public GameObject homeTimeLineObjects;
	public Image profilePic;
	public Text realName;
	public Text twitterHandle;
	public Text timeStamp;
	public Text bodyText;
	public Button lastTweetButton;
	public Button homeButton;
	public Button favoriteButton;
	public Button retweetButton;
	public Button replyButton;
	public Button privateMessageButton;
	public Button nextTweetButton;
	public string timelineTitle = "Timeline";
	public string convoTitle = "Conversation";
	public string homeTimeLineObjectsString = "homeTimeLineObjects";

	private Status currentTweet = null;
	public Text TitleView;
	public AuthenticationHandler authHandler;

	#endregion

	// This function populates the user homepage.
	public void addHomeTimeLine()
	{

		try {
			// Set all objects to be invisible except those related to the timeline.
			setActiveObject(homeTimeLineObjectsString);
			if (authHandler.Interactor == null) {
				Debug.Log("IT'S BADD!");
			}

            authHandler.Interactor.getHomeTimelineNavigatable().resetCurrentObject();
			currentTweet = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getHomeTimelineNavigatable().getNewerObject());
			if(currentTweet != null)
				setTweet(currentTweet);
			TitleView.text = timelineTitle;
		}
		catch (Exception e) {
			Debug.Log ("Something went wrong!");
		}
	}

	// This function sets the current tweet for the user.
	public void setTweet(Status tweet)
	{
		twitterHandle.text = tweet.User.ScreenName;
		realName.text = tweet.User.Name;
		bodyText.text = tweet.Text;
	    timeStamp.text = tweet.CreatedAt.ToString();

		lastTweetButton.GetComponentInChildren<Text>().text = "< (" + authHandler.makeTwitterAPICall(() => authHandler.Interactor.getHomeTimelineNavigatable().getNumNewerObjects()) + ") newer tweets.";
		nextTweetButton.GetComponentInChildren<Text>().text = "(" + authHandler.makeTwitterAPICall(() => authHandler.Interactor.getHomeTimelineNavigatable().getNumOlderObjects()) + ") >";

		lastTweetButton.enabled = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getHomeTimelineNavigatable().hasNewerObject());
		nextTweetButton.enabled = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getHomeTimelineNavigatable().hasOlderObject());

		// Populate the profile picture for the user, requires a separate thread to run.
		StartCoroutine(setProfilePic(tweet.User.ProfileImageUrl));
	}

	public IEnumerator setProfilePic(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		profilePic.sprite = Sprite.Create(
			www.texture,
			new Rect(0, 0, www.texture.width, www.texture.height),
			new Vector2(0, 0));
	}

	// This function populates the timeline ui with the next tweet in the list.
	public void nextTweet()
	{
		// Skip this onclick if the scene is on the Timeline
		if (!TitleView.text.Equals (timelineTitle))
			return;

		currentTweet = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getHomeTimelineNavigatable().getOlderObject());
		if (currentTweet != null)
			setTweet (currentTweet);
		else
			throw new Exception ("No next tweet.");
	}

	// This function populates the timeline ui with the last tweet in the list.
	public void previousTweet()
	{
		if (!TitleView.text.Equals (timelineTitle))
			return;
		
		currentTweet = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getHomeTimelineNavigatable().getNewerObject());
		if (currentTweet != null)
			setTweet (currentTweet);
		else
			throw new Exception ("No previous tweet.");
	}

	public Status getCurrentTweet() 
	{
		return this.currentTweet;
	}

}