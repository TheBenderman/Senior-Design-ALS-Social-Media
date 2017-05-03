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

	public AuthenticationHandler authHandler;
	public TimeLineHandler timelineHandler;
	public ConversationHandler convoHandler;
	public DirectMessageHandler dmHandler;
	public TweetReplyHandler tweetreplyHandler;

	public TwitterInteractor Interactor;

	void Start() 
	{
		//makeTwitterAPICallNoReturnVal(() => authHandler.initializeAuthComponent ());

		IEnumerable<UnityEngine.Object> all_Objs = Resources.FindObjectsOfTypeAll(typeof(Button));
		all_Objs = all_Objs.Where(x => Array.FindIndex(objectsToManage, y => y.Equals(x.name)) > -1);

		foreach (UnityEngine.Object g in all_Objs)
		{
			Button gameobj = (Button)g;
			gameobj.onClick.AddListener (() => {
				connectomeErrorText.text = "";
			});
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
		Debug.Log (timelineHandler.TitleView.text);
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