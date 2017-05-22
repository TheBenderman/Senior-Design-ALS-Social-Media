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
    public GameObject sceneObjects;
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

	public AuthenticationHandler authHandler;
	public TimeLineHandler timelineHandler;
	public ConversationHandler convoHandler;
	public DirectMessageHandler dmHandler;
	public TweetReplyHandler tweetreplyHandler;

	public TwitterInteractor Interactor;

	void Start() 
	{
		// Disable all button clicks
        IEnumerable<Component> all_Objs = sceneObjects.GetComponentsInChildren<Button>(true);
		foreach (Button b in all_Objs)
		{
            //Debug.Log("Button: " + b.name);
            if (!b.name.Equals("submitButton"))
				b.enabled = false;
        }

		authHandler.initializeAuthComponent ();
	}

    void OnApplicationQuit()
    {
		AppDomain.CurrentDomain.ProcessExit += (e, o) => { 
			authHandler.Interactor.getHomeTimelineNavigatable().stopThread();
			authHandler.Interactor.getDmUsersNavigatable().stopThread();
		};
    }

    void OnDestroy()
    {
		AppDomain.CurrentDomain.ProcessExit += (e, o) => { 
			authHandler.Interactor.getHomeTimelineNavigatable().stopThread();
			authHandler.Interactor.getDmUsersNavigatable().stopThread();
		};
    }

	public void backButton()
	{
		if (timelineHandler.TitleView.text.Equals(timelineHandler.timelineTitle))
		{
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