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
	public ConversationHandler convoHandler;

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
			addErrorFieldListeners();

            // Set all objects to be invisible except those related to the timeline.
            setActiveObject(homeTimeLineObjectsString);

			// Reset the current home time line tweet that is selected
            authHandler.Interactor.getHomeTimelineNavigatable().resetCurrentObject();
           	
			// Get the next tweet on the home timeline
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
		
	public void addErrorFieldListeners()
	{
		// Whenever these buttons are clicked, clear the text of the error field
		lastTweetButton.onClick.AddListener(() => {
			timelineErrorText.text = "";
		});
		homeButton.onClick.AddListener(() => {
			timelineErrorText.text = "";
		});
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

		// Set the time stamp to be text such as "2 seconds ago"
        timeStamp.text = Utilities.ElapsedTime(tweet.CreatedAt.DateTime);

		// Populate the last tweet button with the number of newer tweets
        lastTweetButton.GetComponentInChildren<Text>().text = "< (" +
                                                              authHandler.makeTwitterAPICall(
                                                                  () =>
                                                                      authHandler.Interactor.getHomeTimelineNavigatable()
                                                                          .getNumNewerObjects()) + ") newer";
        // Populate the next tweet button with the number of older tweets
		nextTweetButton.GetComponentInChildren<Text>().text = "(" +
                                                              authHandler.makeTwitterAPICall(
                                                                  () =>
                                                                      authHandler.Interactor.getHomeTimelineNavigatable()
                                                                          .getNumOlderObjects()) + ") older >";

        
		// Determine if the last tweet button should be disabled
		Boolean lastButtonEnabled = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getHomeTimelineNavigatable().hasNewerObject());
		lastTweetButton.enabled = lastButtonEnabled;
		lastTweetButton.interactable = lastButtonEnabled;

		// Determine if the next tweet button should be disabled
		Boolean nextButtonEnabled = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getHomeTimelineNavigatable ().hasOlderObject ());
		nextTweetButton.enabled = nextButtonEnabled;
		nextTweetButton.interactable = nextButtonEnabled;
        
		// If there are no images in the tweet, disable the button
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

	// Handle the twitter page to display the images for the current post
    public void populateImages()
    {
        Status currentTweet = getCurrentTweet(); // get the current tweet
		MediaEntity [] currentTweetMedia = currentTweet.Entities.Media; // get all of the images for the current tweet

        imageURLs = new List<string>();

		if (currentTweetMedia == null || currentTweetMedia.Length == 0) {
			throw new Exception ("Uh oh. Shouldn't be here.");
		}
        
		// For each of the images, get the urls
		foreach (MediaEntity media in currentTweetMedia) {
			imageURLs.Add (media.MediaUrl);
		}

		currentImageIndex = 0; // get the first image for the tweet

		setCurrentImage();
        setActiveObject("ImageObjects");
    }

	// Go back from the images menu to the twitter timeline
	public void imagesBackToCurrentTweet()
	{
		Destroy (userImage.sprite);
		setActiveObject (homeTimeLineObjectsString);
	}

	// Set the current image being displayed
    public void setCurrentImage()
    {
		StartCoroutine(setUserImage(imageURLs[currentImageIndex]));

		// Enable the next images button to go to the next image
		Boolean nextButtonEnabled = currentImageIndex < imageURLs.Count - 1;
		nextImageButton.enabled = nextButtonEnabled;
		nextImageButton.interactable = nextButtonEnabled;

		// Enable the next images button to go back to previous images
		Boolean lastButtonEnabled = currentImageIndex > 0;
		lastImageButton.enabled = lastButtonEnabled;
		lastImageButton.interactable = lastButtonEnabled;
    }

	// Go to the next image
	public void nextImage()
	{
		currentImageIndex++;
		setCurrentImage ();
	}

	// Go to the last image
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

	// Contact the twitter api to send a message to the user
	public void Message(string msg)
	{
		int attempts = 10;
		while (attempts-- > 0) {
			try {
				authHandler.makeTwitterAPICallNoReturnVal (() => authHandler.Interactor.createDM (currentTweet.User.ScreenName, msg)); // send the message to the user
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

	// Pop up the on screen keyboard to message a user
	public void messageUser()
	{
		KeyboardManager.GetInputFromKeyboard (Message);
	}

	// Contact the twitter api to send the tweet reply for an image
	public void ReplyImage(string msg)
	{
		Status currentStatus;
		// Determine if we are on the conversation page or the home timeline
		if (TitleView.text.Equals (timelineTitle))
		{
			currentStatus = getCurrentTweet ();
		} else
		{
			currentStatus = convoHandler.conversationtimelineStatuses [convoHandler.currentTweet];
		}

		try
		{
			authHandler.makeTwitterAPICallNoReturnVal( () => authHandler.Interactor.replyToTweet(currentStatus.Id, msg));
			timelineErrorText.text = "Replied to user!";
		}
		catch (Exception e)
		{
			if (e.Message.Contains("Status is a duplicate"))
			{
				timelineErrorText.text = "Failed to tweet: Duplicate status!";
			}
			else
			{
				timelineErrorText.text = "Failed to tweet: Connection error. Fix your internet";
			}

			Debug.Log("Failed to tweet " + e);
		}


	}

	// This function brings the user to a screen that allows them to reply to a tweet.
	public void replyToImage()
	{
		KeyboardManager.GetInputFromKeyboard(ReplyImage);
	}
}