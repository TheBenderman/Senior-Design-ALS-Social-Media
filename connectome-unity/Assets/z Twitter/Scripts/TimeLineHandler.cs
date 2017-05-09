﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Unity.Menu;
using UnityEngine.UI;
using CoreTweet;
using System;
using Connectome.Twitter.API;
using System.Text.RegularExpressions;
using Connectome.Unity.Keyboard;
using Fabric.Crashlytics;

public class TimeLineHandler : TwitterObjects
{
    #region timelineOverviewObjects
    public GameObject homeTimelineOverviewObjects;
    private Status firstTweetObject;
    private Status secondTweetObject;
    private Status thirdTweetObject;
    private Status fourthTweetObject;

    public Image firstTweetProfilePic;
    public Image secondTweetProfilePic;
    public Image thirdTweetProfilePic;
    public Image fourthTweetProfilePic;

    public Text firstTweetRealName;
    public Text secondTweetRealName;
    public Text thirdTweetRealName;
    public Text fourthTweetRealName;

    public Text firstTweetTwitterHandle;
    public Text secondTweetTwitterHandle;
    public Text thirdTweetTwitterHandle;
    public Text fourthTweetTwitterHandle;

    public Text firstTweetBodyText;
    public Text secondTweetBodyText;
    public Text thirdTweetBodyText;
    public Text fourthTweetBodyText;

    public Text firstTweetTimeStamp;
    public Text secondTweetTimeStamp;
    public Text thirdTweetTimeStamp;
    public Text fourthTweetTimeStamp;

    public Image firstTweetImage;
    public Image secondTweetImage;
    public Image thirdTweetImage;
    public Image fourthTweetImage;

    public Button overviewNextTweetButton;
    public Button overviewPreviousTweetButton;
    public Button overviewSelectTweetButton;
    public Button overviewBackButton;
    public Button selectTweetBackButton;
    #endregion

    #region Twitter Home Timeline Members

    public GameObject homeTimeLineObjects;
    public Image profilePic;
    public Text realName;
    public Text twitterHandle;
    public Text timeStamp;
    public Text bodyText;
    public Button homeButton;
    public Button favoriteButton;
    public Button retweetButton;
    public Button replyButton;
    public Button imagesButton;
    public Button privateMessageButton;
    public string timelineTitle = "Timeline";
    public string convoTitle = "Conversation";
    public string homeTimeLineObjectsString = "homeTimeLineObjects";

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
    private List<string> imageURLs;
    private int currentImageIndex;
    #endregion

    // This function populates the user homepage.
    public void addHomeTimeLine()
    {
        // Set all objects to be invisible except those related to the timeline.
        setActiveObject("homeTimelineOverviewObjects");
		// Reset the current home time line tweet that is selected
        authHandler.Interactor.getHomeTimelineNavigatable().resetCurrentObject();
       	
        setNextFourTweets();

        TitleView.text = timelineTitle;
    }

    public void selectTweetBack()
    {
        // hide back button, show tweet navigation buttons
    }

    public void selectTweet()
    {
        // hide tweet navigation buttons, hide select tweet back button
    }

    public void setPreviousFourTweets()
    {
        var tempFirstObj = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getHomeTimelineNavigatable().getNewerObject());
        if (tempFirstObj == null)
            return;

        clearOverviewObjects();

        var tempSecondObj = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getHomeTimelineNavigatable().getNewerObject());
        if (tempSecondObj == null)
        {
            fourthTweetObject = thirdTweetObject;
            thirdTweetObject = secondTweetObject;
            secondTweetObject = firstTweetObject;
            firstTweetObject = tempFirstObj;

            setOverViewObjects();
            return;
        }

        var tempThirdObj = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getHomeTimelineNavigatable().getNewerObject());
        if (tempThirdObj == null)
        {
            fourthTweetObject = secondTweetObject;
            thirdTweetObject = firstTweetObject;
            secondTweetObject = tempSecondObj;
            firstTweetObject = tempFirstObj;

            setOverViewObjects();
            return;
        }
        
        var tempFourthObj = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getHomeTimelineNavigatable().getNewerObject());
        if (tempFourthObj == null)
        {
            fourthTweetObject = firstTweetObject;
            thirdTweetObject = tempThirdObj;
            secondTweetObject = tempSecondObj;
            firstTweetObject = tempFirstObj;

            setOverViewObjects();
            return;
        }
        else
        {
            fourthTweetObject = tempFourthObj;
            thirdTweetObject = tempThirdObj;
            secondTweetObject = tempSecondObj;
            firstTweetObject = tempFirstObj;

            setOverViewObjects();
            return;
        }
    }

    public void setNextFourTweets()
    {
        clearOverviewObjects();

        firstTweetObject = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getHomeTimelineNavigatable().getOlderObject());
        secondTweetObject = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getHomeTimelineNavigatable().getOlderObject());
        thirdTweetObject = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getHomeTimelineNavigatable().getOlderObject());
        fourthTweetObject = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getHomeTimelineNavigatable().getOlderObject());

        setOverViewObjects();
    }

    public void setOverViewObjects()
    {
        if (firstTweetObject != null)
        {
            StartCoroutine(setImage(firstTweetProfilePic, firstTweetObject.User.ProfileImageUrl));
            firstTweetRealName.text = firstTweetObject.User.Name;
            firstTweetTwitterHandle.text = "@" + firstTweetObject.User.ScreenName;
            firstTweetBodyText.text = firstTweetObject.Text;
            firstTweetTimeStamp.text = Utilities.ElapsedTime(firstTweetObject.CreatedAt.Date);

            if (firstTweetObject.Entities != null && firstTweetObject.Entities.Media != null && firstTweetObject.Entities.Media.Length > 0)
            {
                StartCoroutine(setImage(firstTweetImage, firstTweetObject.Entities.Media[0].MediaUrl));
            }
        }

        if (secondTweetObject != null)
        {
            StartCoroutine(setImage(secondTweetProfilePic, secondTweetObject.User.ProfileImageUrl));
            secondTweetRealName.text = secondTweetObject.User.Name;
            secondTweetTwitterHandle.text = "@" + secondTweetObject.User.ScreenName;
            secondTweetBodyText.text = secondTweetObject.Text;
            secondTweetTimeStamp.text = Utilities.ElapsedTime(secondTweetObject.CreatedAt.Date);

            if (secondTweetObject.Entities != null && secondTweetObject.Entities.Media != null && secondTweetObject.Entities.Media.Length > 0)
            {
                StartCoroutine(setImage(secondTweetImage, secondTweetObject.Entities.Media[0].MediaUrl));
            }
        }

        if (thirdTweetObject != null)
        {
            StartCoroutine(setImage(secondTweetProfilePic, thirdTweetObject.User.ProfileImageUrl));
            thirdTweetRealName.text = thirdTweetObject.User.Name;
            thirdTweetTwitterHandle.text = "@" + thirdTweetObject.User.ScreenName;
            thirdTweetBodyText.text = thirdTweetObject.Text;
            thirdTweetTimeStamp.text = Utilities.ElapsedTime(thirdTweetObject.CreatedAt.Date);

            if (thirdTweetObject.Entities != null && thirdTweetObject.Entities.Media != null && thirdTweetObject.Entities.Media.Length > 0)
            {
                StartCoroutine(setImage(thirdTweetImage, thirdTweetObject.Entities.Media[0].MediaUrl));
            }
        }

        if (fourthTweetObject != null)
        {
            StartCoroutine(setImage(fourthTweetProfilePic, fourthTweetObject.User.ProfileImageUrl));
            fourthTweetRealName.text = fourthTweetObject.User.Name;
            fourthTweetTwitterHandle.text = "@" + fourthTweetObject.User.ScreenName;
            fourthTweetBodyText.text = fourthTweetObject.Text;
            fourthTweetTimeStamp.text = Utilities.ElapsedTime(fourthTweetObject.CreatedAt.Date);

            if (fourthTweetObject.Entities != null && fourthTweetObject.Entities.Media != null && fourthTweetObject.Entities.Media.Length > 0)
            {
                StartCoroutine(setImage(fourthTweetImage, fourthTweetObject.Entities.Media[0].MediaUrl));
            }
        }
    }

    public void clearOverviewObjects()
    {
        firstTweetRealName.text = "";
        firstTweetTwitterHandle.text = "";
        firstTweetBodyText.text = "";
        firstTweetTimeStamp.text = "";
        firstTweetImage.sprite = null;
        
        secondTweetRealName.text = "";
        secondTweetTwitterHandle.text = "";
        secondTweetBodyText.text = "";
        secondTweetTimeStamp.text = "";
        secondTweetImage.sprite = null;

        thirdTweetRealName.text = "";
        thirdTweetTwitterHandle.text = "";
        thirdTweetBodyText.text = "";
        thirdTweetTimeStamp.text = "";
        thirdTweetImage.sprite = null;

        fourthTweetRealName.text = "";
        fourthTweetTwitterHandle.text = "";
        fourthTweetBodyText.text = "";
        fourthTweetTimeStamp.text = "";
        fourthTweetImage.sprite = null;
    }

    public IEnumerator setImage(Image image, string url)
    {
        WWW www = new WWW(url);
        yield return www;
        image.sprite = Sprite.Create(
            www.texture,
            new Rect(0, 0, www.texture.width, www.texture.height),
            new Vector2(0, 0));
    }

    public void selectFirstTweetObject()
    {
        setActiveObject(homeTimeLineObjectsString);
        currentTweet = firstTweetObject;
        setTweet(currentTweet);
    }

    public void selectSecondTweetObject()
    {
        setActiveObject(homeTimeLineObjectsString);
        currentTweet = secondTweetObject;
        setTweet(currentTweet);
    }

    public void selectThirdTweetObject()
    {
        setActiveObject(homeTimeLineObjectsString);
        currentTweet = thirdTweetObject;
        setTweet(currentTweet);
    }

    public void selectFourthTweetObject()
    {
        setActiveObject(homeTimeLineObjectsString);
        currentTweet = fourthTweetObject;
        setTweet(currentTweet);
    }

    public void backToTweetOverViewScreen()
    {
        setActiveObject("homeTimelineOverviewObjects");
        
        // Hide select tweet back button, show tweet navigation buttons
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
		try {
			authHandler.makeTwitterAPICallNoReturnVal (() => authHandler.Interactor.createDM (currentTweet.User.ScreenName, msg)); // send the message to the user
			connectomeErrorText.text = "Messaged!";
		} catch (Exception e) {
			connectomeErrorText.text = "Failed to message: Connection error. Please check your internet.";
				
			Debug.Log ("Failed to message " + e);
			Crashlytics.RecordCustomException ("Twitter Exception", "thrown exception", e.StackTrace);
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
			
		authHandler.makeTwitterAPICallNoReturnVal( () => authHandler.Interactor.replyToTweet(currentStatus.Id, msg));
		connectomeErrorText.text = "Replied to user!";
	}

	// This function brings the user to a screen that allows them to reply to a tweet.
	public void replyToImage()
	{
		KeyboardManager.GetInputFromKeyboard(ReplyImage);
	}
}