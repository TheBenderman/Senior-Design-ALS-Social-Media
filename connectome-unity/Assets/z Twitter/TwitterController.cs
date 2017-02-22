using Connectome.Twitter.API;
using CoreTweet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class TwitterController : TwitterObjects
{

	public string homeObjectsString = "homeObjects";

    #region Twitter Home Members
    public GameObject homeObjects;
    public Button timelineButton;
    public Button profileButton;
    public Button messagesButton;
    public Button settingsButton;
    public Button searchButton;
    #endregion

    #region Twitter Compose Tweet Members
    public Button seeConversation;
    #endregion

    #region Not sure
    public Text TweetStatusText;
    #endregion

	public AuthenticationHandler authHandler;
	public TimeLineHandler timelineHandler;
	public ConversationHandler convoHandler;
	public DirectMessageHandler dmHandler;
	public TweetReplyHandler tweetreplyHandler;

    public TwitterInteractor Interactor;

	void Start() 
	{
		//makeTwitterAPICallNoReturnVal(() => authHandler.initializeAuthComponent ());
		authHandler.initializeAuthComponent ();
	}
		
    public void backButton()
    {
		Debug.Log (timelineHandler.TitleView.text);
		if (timelineHandler.TitleView.text.Equals(timelineHandler.timelineTitle))
        {
			Debug.Log ("Hello");
			setActiveObject(homeObjectsString);
        }
		else if (timelineHandler.TitleView.text.Equals(timelineHandler.convoTitle))
        {
			//setActiveObject (timelineHandler.homeTimeLineObjectsString);
			timelineHandler.setTweet (timelineHandler.getCurrentTweet());
			timelineHandler.TitleView.text = timelineHandler.timelineTitle;
            seeConversation.gameObject.SetActive(true);
        }
    }
}