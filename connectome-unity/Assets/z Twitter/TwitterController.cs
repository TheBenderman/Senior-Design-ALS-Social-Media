using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Twitter;
using Connectome.Twitter.API;
using CoreTweet;
using CoreTweet.Core;
using CoreTweet.Rest;
using System;
using System.Net; 

public class TwitterController : MonoBehaviour {

    public bool Remember; 
    #region Twitter Login Members
    public GameObject loginObjects;
    public Text authenticationURL;
    public Text userMessage;
    public Text errorMessage;
    public InputField userInput;
    public Button submitButton;
    #endregion

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
	private List<Status> HomeTimeLine;
	private int currentTweet = 0;
    #endregion

    #region Twitter Home Members
    public GameObject homeObjects;
    public Button timelineButton;
    public Button profileButton;
    public Button messagesButton;
    public Button settingsButton;
    public Button searchButton;
    #endregion

    #region Twitter Compose Tweet Members
    public GameObject composeTweetObjects;
    public Text tweetTitle;
    public Image replyToProfilePic;
    public Text replyToUsername;
    public Text replyToText;
	public Text TitleView;
    public InputField tweetText;
    public Button tweetButton;
    public Button cancelTweetButton;
	public Button seeConversation;
    #endregion

	#region Twitter States
	public bool inTimeLine = false;
	public bool inConversation = false;
    #endregion

    #region Not sure
    public Text TweetStatusText; 
    #endregion

    private static TwitterAPI api;

    public TwitterInteractor Interactor;

	void Start() 
	{
		//makeTwitterAPICallNoReturnVal(() => authHandler.initializeAuthComponent ());
		authHandler.initializeAuthComponent ();
	}
		
    public void backButton()
    {
        try
        {
            apiFunction();
        }
        catch(TwitterException te)
        {
			//setActiveObject (timelineHandler.homeTimeLineObjectsString);
			timelineHandler.setTweet (timelineHandler.getCurrentTweet());
			timelineHandler.TitleView.text = timelineHandler.timelineTitle;
            seeConversation.gameObject.SetActive(true);
        }
    }
}
