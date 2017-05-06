using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Twitter.API;
using CoreTweet;
using System;
using UnityEngine.UI;

public class profileTimeLineHandler : TwitterObjects {
	public string profileTimelineString = "profileUserTimeline";
	#region Twitter Home Timeline Members
	public GameObject profileTimeLineObjects;
	public Text bodyText;
	public Text TitleView;
	public Image profilePic;
	public Text realName;
	public Text twitterHandle;
	public Text timeStamp;
	#endregion

	private List<Status> UserTimeline;
	public List<Status> usertimelineStatuses {
		get { return UserTimeline; }
		set { UserTimeline = value; }
	}

	private int currentTweet = 0;
	public string userTimeLineTitle = "Your Tweets";


	public AuthenticationHandler authHandler;

	public void showUserTimeLine() {
		// Set all objects to be invisible except those related to the timeline.
		setActiveObject("profileUserTimeline");
		Debug.Log("Made obj visible");
		//Debug.Log();
		// Get the tweets for the user.
		usertimelineStatuses = authHandler.makeTwitterAPICall( () => authHandler.Interactor.getLoggedInUserTimeline());
		currentTweet = 0;
		setTweet(currentTweet);
		TitleView.text = userTimeLineTitle;
		Debug.Log(usertimelineStatuses.Count);
		// Populate the profile picture for the user, requires a separate thread to run.
		StartCoroutine(setProfilePic(usertimelineStatuses[currentTweet].User.ProfileImageUrl));
	}

	// This function sets the current tweet for the user.
	public void setTweet(int index)
	{
		twitterHandle.text = usertimelineStatuses[index].User.ScreenName;
		realName.text = usertimelineStatuses[index].User.Name;
		bodyText.text = usertimelineStatuses[index].Text;
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

		if (currentTweet > 0)
		{
			currentTweet--;
			setTweet(currentTweet);

		}
	}

}
