using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTweet;
using UnityEngine.UI;

public class ConversationHandler : TwitterObjects {

	#region Handlers
	public AuthenticationHandler authHandler;
	public TimeLineHandler timelineHandler;
	#endregion

	#region Conversation Members
	private List<Status> ConversationTimeline;
	public List<Status> conversationtimelineStatuses
	{
		get { return ConversationTimeline; }
		set { ConversationTimeline = value; }
	}

	private int currentTweetConvo = 0;
	public int currentTweet
	{
		get { return currentTweetConvo; }
	}

	public Image replyToProfilePic;
	public Button seeConversation;
	#endregion

	public void navigateToConversation()
	{
		populateConversation();

		setActiveObject(timelineHandler.homeTimeLineObjectsString);
	}

	public void populateConversation()
	{
		Status currentStatus = timelineHandler.hometimelineStatuses[timelineHandler.getCurrentTweet()];
		Debug.Log("ID FOR THIS TWEET: " + currentStatus.Id.ToString());
		List<Status> convo = authHandler.makeTwitterAPICall(
			() => authHandler.Interactor.getConversation(currentStatus.User.ScreenName, currentStatus.Id.ToString())
		);
		if (convo.Count > 0)
		{
			conversationtimelineStatuses = convo;
			currentTweetConvo = 0;
			seeConversation.gameObject.SetActive (false);
			setTweet(currentTweetConvo);
			timelineHandler.TitleView.text = timelineHandler.convoTitle;
		}
		else
		{
			Debug.Log("No Conversation for this post");
		}
	}

	// This function sets the current tweet for the user.
	public void setTweet(int index)
	{
		timelineHandler.twitterHandle.text = conversationtimelineStatuses[index].User.ScreenName;
		timelineHandler.realName.text = conversationtimelineStatuses[index].User.Name;
		timelineHandler.bodyText.text = conversationtimelineStatuses[index].Text;

		// Populate the profile picture for the user, requires a separate thread to run.
		StartCoroutine(setReplyToProfilePic(conversationtimelineStatuses[index].User.ProfileImageUrl));
	}

	public IEnumerator setReplyToProfilePic(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		replyToProfilePic.sprite = Sprite.Create(
			www.texture,
			new Rect(0, 0, www.texture.width, www.texture.height),
			new Vector2(0, 0));
	}

	// This function populates the timeline ui with the next tweet in the list.
	public void nextTweet()
	{
		// Skip this onclick if the scene is on the Timeline
		if (timelineHandler.TitleView.text.Equals (timelineHandler.timelineTitle))
			return;

		if (currentTweetConvo < conversationtimelineStatuses.Count - 1)
		{
			currentTweetConvo += 1;
		}
		else
		{
			// TODO add code to refresh conversation
			// Conversationtimeline = authHandler.makeTwitterAPICall(authHandler.Interactor.getHomeTimeLine());
		}

		setTweet(currentTweetConvo);
	}

	// This function populates the timeline ui with the last tweet in the list.
	public void previousTweet()
	{
		// Skip this onclick if the scene is on the Timeline
		if (timelineHandler.TitleView.text.Equals (timelineHandler.timelineTitle))
			return;

		if (currentTweetConvo > 0)
		{
			currentTweetConvo--;
		}

		setTweet(currentTweetConvo);
	}
}