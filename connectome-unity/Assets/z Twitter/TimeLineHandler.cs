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

	private List<Status> Hometimeline;
	public List<Status> hometimelineStatuses {
		get { return Hometimeline; }
		set { Hometimeline = value; }
	}

	private int currentTweet = 0;
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
			//Debug.Log();
			// Get the tweets for the user.
			hometimelineStatuses = authHandler.makeTwitterAPICall( () => authHandler.Interactor.getHomeTimeLine());
			currentTweet = 0;
			setTweet(currentTweet);
			TitleView.text = timelineTitle;
		}
		catch (Exception e) {
			Debug.Log ("Something went wrong!");
		}
	}

	// This function sets the current tweet for the user.
	public void setTweet(int index)
	{
		twitterHandle.text = "@" + hometimelineStatuses[index].User.ScreenName;
		realName.text = hometimelineStatuses[index].User.Name;
		bodyText.text = hometimelineStatuses[index].Text;
		timeStamp.text = hometimelineStatuses[index].CreatedAt.DateTime.ToString("HH:mmtt - dd MMM yyyy");

		// Populate the profile picture for the user, requires a separate thread to run.
		StartCoroutine(setProfilePic(hometimelineStatuses[index].User.ProfileImageUrl));
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
		// Skip this onclick if the scene is on something other than the Timeline
		if (!TitleView.text.Equals (timelineTitle))
			return;
		
		if (currentTweet < hometimelineStatuses.Count - 1)
		{
			currentTweet += 1;
		}
		else
		{
			hometimelineStatuses = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getHomeTimeLine());
		}

		setTweet(currentTweet);
	}

	// This function populates the timeline ui with the last tweet in the list.
	public void previousTweet()
	{
		// Skip this onclick if the scene is on something other than the Timeline
		if (!TitleView.text.Equals (timelineTitle))
			return;
		
		if (currentTweet > 0)
		{
			currentTweet--;
		}

		setTweet(currentTweet);
	}

	public int getCurrentTweet() 
	{
		return this.currentTweet;
	}

}
