using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTweet;
using UnityEngine.UI;
using System;
using Connectome.Unity.Keyboard;
using Connectome.Unity.UI;

public class TweetReplyHandler : TwitterObjects {

	#region Handlers
	public AuthenticationHandler authHandler;
	public TimeLineHandler timelineHandler;
	public ConversationHandler convoHandler;
	#endregion

	#region Twitter Compose Tweet Members
	public GameObject composeTweetObjects;
	public Text tweetTitle;
	public Image replyToProfilePic;
	public Text replyToUsername;
	public Text replyToText;
	public InputField tweetText;
	public Button tweetButton;
	public Button cancelTweetButton;
	public Button seeConversation;
	public string composeTweetObjectsString = "composeTweetObjects";
	#endregion

	// Function to contact the twitter api to reply to a tweet
	public void ReplyTweet(string msg)
	{
		Status currentStatus;

		// Determine if we are on the conversations page or the home timeline
		if (timelineHandler.TitleView.text.Equals (timelineHandler.timelineTitle))
        {
			currentStatus = timelineHandler.getCurrentTweet ();
		} else
        {
			currentStatus = convoHandler.conversationtimelineStatuses [convoHandler.currentTweet];
		}
			
		Debug.Log("Current tweet id: " + currentStatus.Id);
		authHandler.makeTwitterAPICallNoReturnVal( () => authHandler.Interactor.replyToTweet(currentStatus.Id, msg));
		DisplayManager.PushNotification("Replied to user!");
	}

	// This function brings the user to a screen that allows them to reply to a tweet.
	public void replyToTweet()
	{
		KeyboardManager.GetInputFromKeyboard(ReplyTweet);
	}

	// Don't think this is used anymore
	public void cancelTweetReplyButton()
	{
		setActiveObject(timelineHandler.homeTimeLineObjectsString);
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
		
	// Contacts the twitter api to tweet a message
	public void Tweet(string msg)
	{
		authHandler.makeTwitterAPICallNoReturnVal( () => authHandler.Interactor.publishTweet(msg));
		DisplayManager.PushNotification("Tweeted!");
	}

	// Pull up the on screen keyboard to tweet
	public void TweetFromKeyboard()
	{
		KeyboardManager.GetInputFromKeyboard(Tweet);
	}
}
