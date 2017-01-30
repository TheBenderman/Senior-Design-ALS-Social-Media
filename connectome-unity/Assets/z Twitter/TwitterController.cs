using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Twitter;
using Connectome.Twitter.API;
using CoreTweet;
using CoreTweet.Core;
using CoreTweet.Rest;

public class TwitterController : MonoBehaviour {

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

    private static TwitterAPI api;

	// Use this for initialization
	void Start () {
        if (api == null)
            api = TwitterAPI.Instance;

        homeTimeLineObjects.SetActive(false);
        homeObjects.SetActive(false);
        loginObjects.SetActive(true);

        authenticationURL.text = "The authorization URL has been copied to your clipboard! Please visit this url to authenticate twitter.";

        TextEditor te = new TextEditor();
        te.text = api.getAuthorizationURL();
        te.SelectAll();
        te.Copy();

        userMessage.text = "Please input the pin code that you received from Twitter: ";
	}

    public void onPinEnter()
    {
        string pinCode = userInput.text;

        if (string.IsNullOrEmpty(pinCode))
        {
            errorMessage.text = "Please input a value for the pin code!";
        }

		if (api.enterPinCode (pinCode)) {
            //addHomeTimeLine();
            navigateToTwitterHome();
		} else {
			errorMessage.text = "Please input a value for the pin code!";
		}

    }

    public void navigateToTwitterHome()
    {
        homeTimeLineObjects.SetActive(false);
        homeObjects.SetActive(true);
        loginObjects.SetActive(false);
    }

    public void addHomeTimeLine()
    {
        homeTimeLineObjects.SetActive(true);
        loginObjects.SetActive(false);
        homeObjects.SetActive(false);

		HomeTimeLine = api.getHomeTimeLine ();
		setTweet (currentTweet);
    }

	public void setTweet(int index)
	{
		twitterHandle.text = HomeTimeLine [index].User.ScreenName;
		realName.text = HomeTimeLine [index].User.Name;
        bodyText.text = HomeTimeLine[index].Text;

		StartCoroutine (setProfilePic (HomeTimeLine [index].User.ProfileImageUrl));
	}

	public IEnumerator setProfilePic(string url) {
		WWW www = new WWW(url);
		yield return www;
		profilePic.sprite = Sprite.Create(
			www.texture, 
			new Rect(0, 0, www.texture.width, www.texture.height), 
			new Vector2(0, 0));
	}

	public void nextTweet() {

		if (currentTweet < HomeTimeLine.Count - 1) {
			currentTweet += 1;
		} else {
            // SHOULD HAVE SOME ANIMATION HERE SHOWING THAT IT IS BEING REFRESHED
			HomeTimeLine = api.getHomeTimeLine ();
		}

		setTweet (currentTweet);
	}

	public void previousTweet() {
		if (currentTweet > 0) {
			currentTweet--;
		}
		
		setTweet (currentTweet);
	}

	public void refreshTimeline() {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}