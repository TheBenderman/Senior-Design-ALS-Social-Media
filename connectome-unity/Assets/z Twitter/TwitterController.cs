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

    

	// Use this for initialization
	void Start ()
    {
        try
        {
            if (api == null)
                api = TwitterAPI.Instance;

            // See if the accesstoken and access secret have already been entered before
            string accesstoken = PlayerPrefs.GetString("Access Token");
            string accessSecret = PlayerPrefs.GetString("Access Secret");

            // If the access token and access secret have been set before, then load them back into the API
            if (Remember && !string.IsNullOrEmpty(accesstoken) && !string.IsNullOrEmpty(accessSecret))
            {
                // Set the tokens to the previously received tokens
                makeTwitterAPICallNoReturnVal(() => api.setTokens(accesstoken, accessSecret));

                // Hacky way to do this, verify that the credentials are fine.
                makeTwitterAPICall(() => api.getHomeTimeLine());

                navigateToTwitterHome();
            }
            else // Otherwise, we need to authenticate the user.
            {
                navigateToTwitterAuthPage();
            }
        }
        catch(Exception e)
        {
            TweetStatusText.text = "error: " + e;
            navigateToTwitterAuthPage();
        }
	}

    // This function allows us to make a call to the API for functions that do not have a return value.
    // This is used to catch any twitter authentication exceptions that may arise, and then navigate the user back to the authentication page if it fails.
    public void makeTwitterAPICallNoReturnVal(Action apiFunction)
    {
        try
        {
            apiFunction();
        }
        catch(TwitterException te)
        {
            PlayerPrefs.SetString("Access Token", "");
            PlayerPrefs.SetString("Access Secret", "");

            errorMessage.text = "Something went wrong with your authorization. Please authorize this application for Twitter again.";

            navigateToTwitterAuthPage();
        }
    }

    // This function allows us to make calls to a twitter api function that DOES have a return value.
    // The purpose of this function is to catch any exceptions that may occur due to authentication, and if they do, reroute the user to the authentication page.
    public T makeTwitterAPICall<T>(Func<T> apiFunction)
    {
        try
        {
            return apiFunction();
        }
        catch (TwitterException te)
        {
            PlayerPrefs.SetString("Access Token", "");
            PlayerPrefs.SetString("Access Secret", "");

            errorMessage.text = "Something went wrong with your authorization. Please authorize this application for Twitter again.";

            navigateToTwitterAuthPage();
        }

        return default(T);
    }

	public void navigateToConversation() 
	{
		TitleView.text = "Coversation";
		populateConversation ();
		// Hide all elements except those related to authentication
		homeTimeLineObjects.SetActive(true);
		homeObjects.SetActive(false);
		loginObjects.SetActive(false);
		composeTweetObjects.SetActive(false);
	}

	public void populateConversation() {
		Status currentStatus = HomeTimeLine[currentTweet];
		Debug.Log ("ID FOR THIS TWEET: " + currentStatus.Id.ToString());
		List<Status> convo = api.getConversation (currentStatus.User.ScreenName, currentStatus.Id.ToString());
		if (convo.Count > 0) {
			HomeTimeLine = convo;
			currentTweet = 0;
			inConversation = true;
			inTimeLine = false;
			seeConversation.gameObject.SetActive (false);
			setTweet (currentTweet);
		} else {
			Debug.Log ("No Conversation for this post");
		}
	}

    // This function navigates the user back to the user authentication page. Here the user will need to open the link that is copied to their clipboard in a browser, and then enter the PIN code shown.
    public void navigateToTwitterAuthPage()
    { 
        SelectionManager.Instance.Deactivate();
        // Hide all elements except those related to authentication
        homeTimeLineObjects.SetActive(false);
        homeObjects.SetActive(false);
        loginObjects.SetActive(true);
        composeTweetObjects.SetActive(false);
        authenticationURL.text = "The authorization URL has been copied to your clipboard! Please visit this url to authenticate twitter."; 

        // Copy the authentication URL to the user's clipboard.
        TextEditor te = new TextEditor();
        te.text = makeTwitterAPICall(() => api.getAuthorizationURL());
        te.SelectAll();
        te.Copy();

        userMessage.text = "Please input the pin code that you received from Twitter: ";
    }

    // This is the on click event for when the user enters their pin code that they received from the twitter website.
    public void onPinEnter()
    {
        string pinCode = userInput.text;

        // make sure the user has entered a pin code
        if (string.IsNullOrEmpty(pinCode))
        {
            errorMessage.text = "Please input a value for the pin code!";
            return;
        }

		if (makeTwitterAPICall(() => api.enterPinCode(pinCode))) {
            string accessToken = makeTwitterAPICall(() => api.getAccessToken());
            string accessSecret = makeTwitterAPICall(() => api.getAccessTokenSecret());

            // Save the access token so they do not have to authenticate themselves again.
            PlayerPrefs.SetString("Access Token", accessToken);
            PlayerPrefs.SetString("Access Secret", accessSecret);

            navigateToTwitterHome();
		} else {
			errorMessage.text = "Please input a value for the pin code!";
		}
    }

    // This function navigates the user to the twitter home page. From this page, the user can choose whether they wish to navigate to their timeline, their own profile, messages...
    public void navigateToTwitterHome()
    {
        // Set all objects to be invisible except those related to the home page.
        homeTimeLineObjects.SetActive(false);
        homeObjects.SetActive(true);
        loginObjects.SetActive(false);
        composeTweetObjects.SetActive(false);

        SelectionManager.Instance.Activate();
    }

	public void backButton() 
	{
		if (inTimeLine) {
			homeTimeLineObjects.SetActive(false);
			homeObjects.SetActive(true);
			loginObjects.SetActive(false);
			composeTweetObjects.SetActive(false);
		} else if (inConversation) {
			addHomeTimeLine ();
			currentTweet = 0;
			Debug.Log ("Current Tweet in Back button: " + currentTweet);
			seeConversation.gameObject.SetActive (true);
		}
	}

	public void cancelTweetReplyButton() {
		homeTimeLineObjects.SetActive (true);
		composeTweetObjects.SetActive (false);
	}

    // This function populates the user homepage.
    public void addHomeTimeLine()
    {
		inTimeLine = true;
		inConversation = false;
		TitleView.text = "Timeline";
        // Set all objects to be invisible except those related to the timeline.
        homeTimeLineObjects.SetActive(true);
        loginObjects.SetActive(false);
        homeObjects.SetActive(false);
        composeTweetObjects.SetActive(false);

        // Get the tweets for the user.
        HomeTimeLine = makeTwitterAPICall(() => api.getHomeTimeLine());
		setTweet (currentTweet);
    }

    // This function sets the current tweet for the user.
	public void setTweet(int index)
	{
		twitterHandle.text = HomeTimeLine [index].User.ScreenName;
		realName.text = HomeTimeLine [index].User.Name;
        bodyText.text = HomeTimeLine[index].Text;

        // Populate the profile picture for the user, requires a separate thread to run.
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
    public void nextTweet() {

		if (currentTweet < HomeTimeLine.Count - 1) {
			currentTweet += 1;
		} else {
            // SHOULD HAVE SOME ANIMATION HERE SHOWING THAT IT IS BEING REFRESHED
			if (inTimeLine) {	
				HomeTimeLine = makeTwitterAPICall (() => api.getHomeTimeLine ());
			}
		}

		setTweet (currentTweet);
	}

    // This function populates the timeline ui with the last tweet in the list.
	public void previousTweet() {
		if (currentTweet > 0) {
			currentTweet--;
		}
		
		setTweet (currentTweet);
	}

	public void refreshTimeline() {
		
	}

    // This function brings the user to a screen that allows them to reply to a tweet.
    public void replyToTweet()
    {
        // Make all objects except those related to replying to a tweet to be hidden.
        homeTimeLineObjects.SetActive(false);
        homeObjects.SetActive(false);
        loginObjects.SetActive(false);
        composeTweetObjects.SetActive(true);

        Status currentStatus = HomeTimeLine[currentTweet];
		Debug.Log ("Current in reply " + currentTweet);

        tweetTitle.text = "Reply to " + currentStatus.User.ScreenName;
		StartCoroutine(setReplyToProfilePic(currentStatus.User.ProfileImageUrl));
        replyToText.text = currentStatus.Text;
        replyToUsername.text = currentStatus.User.ScreenName;
        tweetText.text = "@" + currentStatus.User.ScreenName + " ";


       
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
                api.publishTweet(msg);
                TweetStatusText.text = "Tweeted!";
                attempts = 0; 
            }
            catch (Exception e)
            {
                if(attempts == 0)
                {
                    if(e.Message.Contains("Status is a duplicate"))
                    {
                        TweetStatusText.text = "Failed to tweet: Duplicate status!";
                    }
                    else
                    {
                        TweetStatusText.text = "Failed to tweet: Connection error: "+ e;
                    }
                    Debug.Log("Failed to tweet " + e);
                }
            }
        }
    }

    public void TweetFromKeyboard()
    {
        DisplayManager.GetInputFromKeyboard(Tweet);
    }

	// Update is called once per frame
	void Update () {
		
	}
}