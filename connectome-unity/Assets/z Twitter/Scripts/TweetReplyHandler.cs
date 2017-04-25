using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTweet;
using UnityEngine.UI;
using System;
using Connectome.Unity.Keyboard;

public class TweetReplyHandler : TwitterObjects {

	#region Handlers
	public AuthenticationHandler authHandler;
	public TimeLineHandler timelineHandler;
	public ConversationHandler convoHandler;
	#endregion

	#region Twitter Compose Tweet Members
	public GameObject composeTweetObjects;
	public Text TweetStatusText;
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

	public void ReplyTweet(string msg)
	{
		Status currentStatus;
		if (timelineHandler.TitleView.text.Equals (timelineHandler.timelineTitle))
        {
			currentStatus = timelineHandler.getCurrentTweet ();
		} else
        {
			currentStatus = convoHandler.conversationtimelineStatuses [convoHandler.currentTweet];
		}

		try
		{
			Debug.Log("Current tweet id: " + currentStatus.Id);
			authHandler.makeTwitterAPICallNoReturnVal( () => authHandler.Interactor.replyToTweet(currentStatus.Id, msg));
			timelineHandler.timelineErrorText.text = "Replied to user!";
		}
		catch (Exception e)
		{
			if (e.Message.Contains("Status is a duplicate"))
			{
				timelineHandler.timelineErrorText.text = "Failed to tweet: Duplicate status!";
			}
			else
			{
				timelineHandler.timelineErrorText.text = "Failed to tweet: Connection error. Fix your internet";
			}

			Debug.Log("Failed to tweet " + e);
		}
		

	}

	// This function brings the user to a screen that allows them to reply to a tweet.
	public void replyToTweet()
	{
		// Make all objects except those related to replying to a tweet to be hidden.
		/*setActiveObject(composeTweetObjectsString);

		Status currentStatus;
		if (timelineHandler.TitleView.text.Equals (timelineHandler.timelineTitle)) {
			currentStatus = timelineHandler.getCurrentTweet ();
		} else {
			currentStatus = convoHandler.conversationtimelineStatuses [convoHandler.currentTweet];
		}

		Debug.Log("Current in reply " + timelineHandler.getCurrentTweet());

		tweetTitle.text = "Reply to " + currentStatus.User.ScreenName;
		StartCoroutine(setReplyToProfilePic(currentStatus.User.ProfileImageUrl));
		replyToText.text = currentStatus.Text;
		replyToUsername.text = currentStatus.User.ScreenName;
		tweetText.text = "@" + currentStatus.User.ScreenName + " ";
		*/
		KeyboardManager.GetInputFromKeyboard(ReplyTweet);
	}

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
		
	/// <summary>
	/// Appemts 10 time to tweet a message 
	/// </summary>
	/// <param name="msg"></param>
	public void Tweet(string msg)
	{
		int attempts = 10;
		while (attempts-- > 0)
		{
			try
			{
				authHandler.makeTwitterAPICallNoReturnVal( () => authHandler.Interactor.publishTweet(msg));
				TweetStatusText.text = "Tweeted!";
				attempts = 0;
			}
			catch (Exception e)
			{
				if (attempts == 0)
				{
					if (e.Message.Contains("Status is a duplicate"))
					{
						TweetStatusText.text = "Failed to tweet: Duplicate status!";
					}
					else
					{
						TweetStatusText.text = "Failed to tweet: Connection error. Fix your internet";
					}
					Debug.Log("Failed to tweet " + e);
				}
			}
		}

	}

	public void TweetFromKeyboard()
	{
		KeyboardManager.GetInputFromKeyboard(Tweet);
	}
}
