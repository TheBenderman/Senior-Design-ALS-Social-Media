using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Unity.Menu;
using UnityEngine.UI;
using CoreTweet;
using Connectome.Twitter.API;
using System;
using System.Text.RegularExpressions;
using Connectome.Unity.Keyboard;

public class TimeLineHandler : TwitterObjects
{
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
    public Button imagesButton;
    public Button privateMessageButton;
    public Button nextTweetButton;
    public string timelineTitle = "Timeline";
    public string convoTitle = "Conversation";
    public string homeTimeLineObjectsString = "homeTimeLineObjects";
	public Text timelineErrorText;

    private Status currentTweet = null;
    public Text TitleView;
    public AuthenticationHandler authHandler;

    #endregion

    #region image members
	public GameObject ImageObjects;
    public Button lastImageButton;
    public Button backImageButton;
    public Button replyImageButton;
    public Button nextImageButton;
    public Image userImage;
    private List<String> imageURLs;
    private int currentImageIndex;
    #endregion

    // This function populates the user homepage.
    public void addHomeTimeLine()
    {
        try
        {
			lastTweetButton.onClick.AddListener(() => {
				timelineErrorText.text = "";
			});
			homeButton.onClick.AddListener(() => {
				timelineErrorText.text = "";
			});
			/*favoriteButton.onClick.AddListener(() => {
				timelineErrorText.text = "";
			});*/
			retweetButton.onClick.AddListener(() => {
				timelineErrorText.text = "";
			});
			imagesButton.onClick.AddListener(() => {
				timelineErrorText.text = "";
			});
			privateMessageButton.onClick.AddListener(() => {
				timelineErrorText.text = "";
			});
			nextTweetButton.onClick.AddListener(() => {
				timelineErrorText.text = "";
			});

            // Set all objects to be invisible except those related to the timeline.
            setActiveObject(homeTimeLineObjectsString);
            if (authHandler.Interactor == null)
            {
                Debug.Log("IT'S BADD!");
            }

            authHandler.Interactor.getHomeTimelineNavigatable().resetCurrentObject();
            currentTweet = authHandler.makeTwitterAPICall(
                    () => authHandler.Interactor.getHomeTimelineNavigatable().getNewerObject());
            if (currentTweet != null)
                setTweet(currentTweet);
            else
                throw new Exception("No first tweet.");

            TitleView.text = timelineTitle;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    // This function sets the current tweet for the user.
    public void setTweet(Status tweet)
    {
        twitterHandle.text = "@" + tweet.User.ScreenName;
        realName.text = tweet.User.Name;
        bodyText.text = tweet.Text;

		if (tweet.FullText != null && string.IsNullOrEmpty(tweet.FullText)) {
			bodyText.text = tweet.FullText;
		} else {
			bodyText.text = tweet.Text;
		}

        timeStamp.text = Utilities.ElapsedTime(tweet.CreatedAt.DateTime);

        lastTweetButton.GetComponentInChildren<Text>().text = "< (" +
                                                              authHandler.makeTwitterAPICall(
                                                                  () =>
                                                                      authHandler.Interactor.getHomeTimelineNavigatable()
                                                                          .getNumNewerObjects()) + ") newer";
        nextTweetButton.GetComponentInChildren<Text>().text = "(" +
                                                              authHandler.makeTwitterAPICall(
                                                                  () =>
                                                                      authHandler.Interactor.getHomeTimelineNavigatable()
                                                                          .getNumOlderObjects()) + ") older >";

        
		Boolean lastButtonEnabled = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getHomeTimelineNavigatable().hasNewerObject());
		lastTweetButton.enabled = lastButtonEnabled;
		lastTweetButton.interactable = lastButtonEnabled;

		Boolean nextButtonEnabled = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getHomeTimelineNavigatable ().hasOlderObject ());
		nextTweetButton.enabled = nextButtonEnabled;
		nextTweetButton.interactable = nextButtonEnabled;
        
		Boolean imageButtonEnabled = tweet.Entities != null && tweet.Entities.Media != null && tweet.Entities.Media.Length > 0;
		imagesButton.enabled = imageButtonEnabled;
		imagesButton.interactable = imageButtonEnabled;

        // Populate the profile picture for the user, requires a separate thread to run.
        StartCoroutine(setProfilePic(tweet.User.ProfileImageUrl));
    }

    public IEnumerator setProfilePic(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        profilePic.sprite = Sprite.Create(
            www.texture,
            new Rect(0, 0, www.texture.width, www.texture.height),
            new Vector2(0, 0));
    }

    // This function populates the timeline ui with the next tweet in the list.
    public void nextTweet()
    {
        // Skip this onclick if the scene is on the Timeline
        if (!TitleView.text.Equals(timelineTitle))
            return;

        currentTweet = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getHomeTimelineNavigatable().getOlderObject());
        if (currentTweet != null)
            setTweet(currentTweet);
        else
            throw new Exception("No next tweet.");
    }

    // This function populates the timeline ui with the last tweet in the list.
    public void previousTweet()
    {
        if (!TitleView.text.Equals(timelineTitle))
            return;

        currentTweet = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getHomeTimelineNavigatable().getNewerObject());
        if (currentTweet != null)
            setTweet(currentTweet);
        else
            throw new Exception("No previous tweet.");
    }

    public Status getCurrentTweet()
    {
        return this.currentTweet;
    }

    public void populateImages()
    {
        Status currentTweet = getCurrentTweet();
		MediaEntity [] currentTweetMedia = currentTweet.Entities.Media;

        imageURLs = new List<string>();

		if (currentTweetMedia == null || currentTweetMedia.Length == 0) {
			Debug.Log ("Uh oh. Shouldn't be here.");
			return;
		}
        
		foreach (MediaEntity media in currentTweetMedia) {
			imageURLs.Add (media.MediaUrl);
		}

		currentImageIndex = 0;

		setCurrentImage();
        setActiveObject("ImageObjects");
    }

	public void imagesBackToCurrentTweet()
	{
		Destroy (userImage.sprite);
		setActiveObject (homeTimeLineObjectsString);
	}

    public void setCurrentImage()
    {
		StartCoroutine(setUserImage(imageURLs[currentImageIndex]));

		Boolean nextButtonEnabled = currentImageIndex < imageURLs.Count - 1;
		nextImageButton.enabled = nextButtonEnabled;
		nextImageButton.interactable = nextButtonEnabled;

		Boolean lastButtonEnabled = currentImageIndex > 0;
		lastImageButton.enabled = lastButtonEnabled;
		lastImageButton.interactable = lastButtonEnabled;
    }

	public void nextImage()
	{
		currentImageIndex++;
		setCurrentImage ();
	}

	public void lastImage()
	{
		currentImageIndex--;
		setCurrentImage();
	}

    public IEnumerator setUserImage(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        userImage.sprite = Sprite.Create(
            www.texture,
            new Rect(0, 0, www.texture.width, www.texture.height),
            new Vector2(0, 0));
    }

	public void Message(string msg)
	{
		int attempts = 10;
		while (attempts-- > 0) {
			try {
				authHandler.makeTwitterAPICallNoReturnVal (() => authHandler.Interactor.createDM (currentTweet.User.ScreenName, msg));
				timelineErrorText.text = "Messaged!";
				attempts = 0;
			} catch (Exception e) {
				if (attempts == 0) {
					timelineErrorText.text = "Failed to message: Connection error. Please check your internet.";
						
					Debug.Log ("Failed to message " + e);
				}
			}
		}
	}

	public void messageUser()
	{
		KeyboardManager.GetInputFromKeyboard (Message);
	}
}