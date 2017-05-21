using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Unity.Menu;
using Connectome.Twitter.API;
using CoreTweet;
using System;
using System.Threading;
using Fabric.Crashlytics;
using Connectome.Unity.UI;
using System.Net;

public class AuthenticationHandler : TwitterObjects
{
	#region AuthenticationMembers
	public bool Remember = true;
	public GameObject loginObjects;
	public Text authenticationURL;
	public Text userMessage;
	public InputField userInput;
	public Button submitButton;
	public string loginPage = "loginObjects";
	public string homePage = "homeObjects";
	public string loggedInUser = "";
	public TwitterAuthenticator Authenticator;
	public TwitterInteractor Interactor;
	#endregion

	public void initializeAuthComponent()
	{
		if (Authenticator == null) {
            try
            {
                Authenticator = TwitterAuthenticator.Instance;
            }
			catch (Exception e)
			{
                Debug.Log(e.StackTrace);
                checkErrorCodes(e);
            }
        }

		if (Authenticator == null)
		{
            DisplayManager.PushNotification("Something went wrong with your twitter authentication. If this persists please contact the development team.");
            navigateToTwitterAuthPage("Authenticator is null");
        }

		// See if the accesstoken and access secret have already been entered before
		string accesstoken = PlayerPrefs.GetString("Access Token");
		string accessSecret = PlayerPrefs.GetString("Access Secret");

		// If the access token and access secret have been set before, then load them back into the API
		if (Remember && !string.IsNullOrEmpty(accesstoken) && !string.IsNullOrEmpty(accessSecret))
		{
			// Set the tokens to the previously received tokens
			makeTwitterAPICallNoReturnVal(() => Authenticator.setTokens(accesstoken, accessSecret));
            try
            {
                Interactor = new TwitterInteractor(Authenticator);
            }
			catch(Exception e)
			{
				Debug.Log(e.StackTrace);
                checkErrorCodes(e);
			}

            if (Interactor == null)
			{
                DisplayManager.PushNotification("Something went wrong with your twitter authentication. If this persists please contact the development team.");
            	navigateToTwitterAuthPage("Interactor is null");
			}

            // Recover from errors having to do with exceptions in home timeline thread
		    Interactor.getHomeTimelineNavigatable().OnExp = exception =>
		    {
                Debug.Log(exception.Message.ToString());
				Crashlytics.RecordCustomException("Twitter Exception", "thrown exception", exception.StackTrace);

                DisplayManager.PushNotification("Something went wrong with your twitter implementation.");
                navigateToTwitterHome ();
		    };

            // Recover from errors having to do with exceptions in dm users thread
		    Interactor.getDmUsersNavigatable().OnExp = exception =>
		    {
                Debug.Log(exception.Message.ToString());
				Crashlytics.RecordCustomException("Twitter Exception", "thrown exception", exception.StackTrace);

                DisplayManager.PushNotification("Something went wrong with your twitter implementation.");
                navigateToTwitterHome ();
		    };

			navigateToTwitterHome ();
		}
		else // Otherwise, we need to authenticate the user.
		{
			navigateToTwitterAuthPage();
		}
	}

	// This is the on click event for when the user enters their pin code that they received from the twitter website.
	public void onPinEnter()
	{
		string pinCode = userInput.text;

		// make sure the user has entered a pin code
		if (string.IsNullOrEmpty(pinCode))
		{
            DisplayManager.PushNotification("Please input a value for the pin code!");
            return;
		}

		if (makeTwitterAPICall(() => Authenticator.enterPinCode(pinCode)))
		{
			string accessToken = Authenticator.getAccessToken();
			string accessSecret = Authenticator.getAccessTokenSecret();

			// Save the access token so they do not have to authenticate themselves again.
			PlayerPrefs.SetString("Access Token", accessToken);
			PlayerPrefs.SetString("Access Secret", accessSecret);

			if (Interactor == null)
				Interactor = new TwitterInteractor (Authenticator);

			if (Interactor == null)
			{
                DisplayManager.PushNotification("Something went wrong with your twitter authentication. If this persists please contact the development team.");
                navigateToTwitterAuthPage("Interactor is null");
			}

			navigateToTwitterHome ();
		}
		else
		{
			DisplayManager.PushNotification("Please input a value for the pin code!");
        }
	}

	// This function navigates the user to the twitter home page. From this page, the user can choose whether they wish to navigate to their timeline, their own profile, messages...
	public void navigateToTwitterHome()
	{
		setActiveObject(homePage);
		SelectionManager.Instance.Activate();
		SelectionManager.Instance.AllowSelection = true;
        SelectionManager.Instance.Highlighter.EnableHighlight();
	}


	// This function navigates the user back to the user authentication page. Here the user will need to open the link that is copied to their clipboard in a browser, and then enter the PIN code shown.
	public void navigateToTwitterAuthPage(string errorCode = null)
	{
        hideAllGameObjects();

        if (errorCode != null)
		{
            Fabric.Crashlytics.Crashlytics.RecordCustomException("Navigated to Twitter Auth Page", errorCode, "");
        }

        SelectionManager.Instance.AllowSelection = false;
        SelectionManager.Instance.Highlighter.DisableHighlight();
        SelectionManager.Instance.Deactivate();

		setActiveObject(loginPage);

		authenticationURL.text = "The authorization URL has been copied to your clipboard! Please visit this url to authenticate twitter.";

		// Copy the authentication URL to the user's clipboard.
		TextEditor te = new TextEditor();
		te.text = makeTwitterAPICall( () => Authenticator.getAuthorizationURL());
		te.SelectAll();
		te.Copy();

		userMessage.text = "Please input the pin code that you received from Twitter: ";
	}

	#region TwitterAPIFunctions
	// This function allows us to make a call to the API for functions that do not have a return value.
	// This is used to catch any twitter authentication exceptions that may arise, and then navigate the user back to the authentication page if it fails.
	public void makeTwitterAPICallNoReturnVal(Action apiFunction)
	{
		try
		{
			apiFunction();
		}
		catch (Exception e)
		{
			PlayerPrefs.SetString("Access Token", "");
			PlayerPrefs.SetString("Access Secret", "");

			checkErrorCodes (e);
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
		catch (Exception e)
		{
			PlayerPrefs.SetString("Access Token", "");
			PlayerPrefs.SetString("Access Secret", "");

			checkErrorCodes (e);
		}

		return default(T);
	}

	private void checkErrorCodes(Exception e)
	{
		Crashlytics.RecordCustomException("Twitter Exception", "thrown exception", e.StackTrace);
        Debug.LogError(e.StackTrace);

        if (e.Message.Contains ("Status is a duplicate")) {
			DisplayManager.PushNotification("Failed to tweet: Duplicate status!");
			return;
		} else if (e.Message.Contains ("Could not authenticate you")) {
			DisplayManager.PushNotification("Unable to authenticate you. Please try to log in again.");
		} else if (e.Message.Contains ("User has been suspended")) {
			DisplayManager.PushNotification("Your account has been temporarily suspended. Please try again later.");
		} else if (e.Message.Contains ("Rate limit exceeded.")) {
			DisplayManager.PushNotification("Sorry! We are experiencing heavy traffic. Please try again in a bit.");
			return;
		} else if (e.Message.Contains ("Invalid or expired token")) {
			DisplayManager.PushNotification("Your session timed out. Please log back in.");
		} else if (e.Message.Contains ("Unable to verify your credentials")) {
			DisplayManager.PushNotification("Invalid credentials. Please try again.");
		} else if (e.Message.Contains ("Over capacity")) {
			DisplayManager.PushNotification("Twitter is temporarily over capacity. Please try again later.");
		} else if (e.Message.Contains ("You are unable to follow more people at this time")) {
			DisplayManager.PushNotification("Sorry you can't follow this person at this time.");
			return;
		} else if (e.Message.Contains ("User is over daily status update limit")) {
			DisplayManager.PushNotification("Sorry you have posted too many times today. Please try again tomorrow.");
			return;
		} else if (e.Message.Contains ("The text of your direct message is over the max character limit")) {
			DisplayManager.PushNotification("Your message is too long! Please try a shorter status.");
			return;
		}
		else {
			if (e is WebException)
			{
                DisplayManager.PushNotification("Something is wrong with your internet connection, please fix it and try again.");
            }
            else if (e is TwitterException)
            {
                DisplayManager.PushNotification("Something went wrong with your authorization. Please authorize this application for Twitter again.");
            }
			else
            {
					DisplayManager.PushNotification("An unknown error has occurred. If this persists, please contact the dev team.");
                	return;
			}
		}

		navigateToTwitterAuthPage(e.Message.ToString());
	}

	public void logOut()
	{
		PlayerPrefs.SetString("Access Token", "");
		PlayerPrefs.SetString("Access Secret", "");

        DisplayManager.PushNotification("You have successfully been logged out!");
        navigateToTwitterAuthPage();
	}
}
#endregion