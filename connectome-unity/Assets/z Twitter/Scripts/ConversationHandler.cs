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

	// Navigate to the conversation between the two users of the current tweet
	public void navigateToConversation()
	{
		populateConversation();

		setActiveObject(timelineHandler.homeTimeLineObjectsString);
	}

	// Populate the unity conversation objects.
	public void populateConversation()
	{
		Status currentStatus = timelineHandler.getCurrentTweet ();

		List<Status> convo = authHandler.makeTwitterAPICall(
			() => authHandler.Interactor.getConversation(currentStatus.User.ScreenName, currentStatus.Id.ToString())
		);

		if (convo.Count > 0)
		{
			conversationtimelineStatuses = convo;
			currentTweetConvo = 0; // set the current conversation message to the first one
			seeConversation.gameObject.SetActive (false);
			setTweet(currentTweetConvo);
			timelineHandler.TitleView.text = timelineHandler.convoTitle;
		}
		else
		{
            connectomeErrorText.text = "There are no posts between these two users!";
            timelineHandler.addHomeTimeLine();
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
		// Skip this onclick if the scene is not on Conversation
		if (!timelineHandler.TitleView.text.Equals (timelineHandler.convoTitle))
			return;

		if (currentTweetConvo < conversationtimelineStatuses.Count - 1)
		{
			currentTweetConvo += 1;
		}
		else
		{
            connectomeErrorText.text = "There is no next tweet.";
            return;
        }

		setTweet(currentTweetConvo);
	}

	// This function populates the timeline ui with the last tweet in the list.
	public void previousTweet()
	{
		// Skip this onclick if the scene is on the Timeline
		if (!timelineHandler.TitleView.text.Equals (timelineHandler.convoTitle))
			return;

		if (currentTweetConvo > 0)
		{
			currentTweetConvo--;
		}
		else
		{
            connectomeErrorText.text = "There is no previous tweet.";
            return;
        }

		setTweet(currentTweetConvo);
	}
}