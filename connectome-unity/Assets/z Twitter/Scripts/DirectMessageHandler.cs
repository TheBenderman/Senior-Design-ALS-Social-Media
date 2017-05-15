using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CoreTweet;
using System;
using Connectome.Unity.Keyboard;
using Fabric.Crashlytics;
using Connectome.Unity.UI;

public class DirectMessageHandler : TwitterObjects {
	#region DM Members
	public GameObject DMObjects;
	public GameObject SelectConvObjects;
	public GameObject ViewConvObjects;
	public Image DMConvoProfilePic;
	public Text DMName;
	public Text DMUsername;
	public Text LastMessageText;
	public Button LastDMUser;
	public Button NextDMUser;
	public Button MessageDMUser;
	public Button DMUserHome;
    public GameObject DMsHolder;
    public Button NewerDMs;
    public Button OlderDMs;
    public Button ReplyToDM;
    public Button BackToUsersDM;
    public Text DMTitle;
    public List<GameObject> messageObjects;

    private List<DirectMessage> directMessages;
	private User currentUser;
    private int currentDMPage = 0;
	public AuthenticationHandler authHandler;
    public TwitterController twitterController;
    public string selectConvoObjectsString = "SelectConvObjects";
    public string viewConvObjectsString = "ViewConvObjects";
    #endregion

	// Make the menu displaying all conversations visible. This is a list of users and their most recent message
    public void handleTwitterConversations()
	{
		// Reset the current twitter DM user that is selected
		authHandler.makeTwitterAPICallNoReturnVal (() => authHandler.Interactor.getDmUsersNavigatable().resetCurrentObject());

		setActiveObject(selectConvoObjectsString);

		// Get the next twitter DM User (newest conversation)
		currentUser = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getDmUsersNavigatable().getNewerObject());
		if (currentUser != null)
			setUser (currentUser);
		else
		{
            DisplayManager.PushNotification( "There are no DMs for you to see! Select a tweet, and hit the message button to start a conversation.");
            authHandler.navigateToTwitterHome();
		}
    }

	// This function sets the current DM user
	public void setUser(User user)
	{
		// Populate the user's profile picture
		StartCoroutine(setDMUserProfilePic(user.ProfileImageUrl));
		DMUsername.text = user.ScreenName;
		DMName.text = user.Name;
 
		string otherUser = user.ScreenName;

		// Load all of the DMs between the current user and the selected user
		List<DirectMessage> dms = authHandler.makeTwitterAPICall(
			() => authHandler.Interactor.buildDMConversation(otherUser));

		// Get the newest message
		DirectMessage latestMessage = dms.OrderByDescending((arg) => arg.CreatedAt).First();
		LastMessageText.text = latestMessage.Sender.ScreenName + " - (" + Utilities.ElapsedTime(latestMessage.CreatedAt.DateTime)
			+ ") - " + latestMessage.Text;

		// Populate the previous and next buttons with the number of users in each direction
        LastDMUser.GetComponentInChildren<Text>().text = "< (" + authHandler.makeTwitterAPICall(() => authHandler.Interactor.getDmUsersNavigatable().getNumNewerObjects()) + ") newer";
        NextDMUser.GetComponentInChildren<Text>().text = "(" + authHandler.makeTwitterAPICall(() => authHandler.Interactor.getDmUsersNavigatable().getNumOlderObjects()) + ") older >";

		// Determine if there are any newer users. If there aren't, disable the button
		Boolean lastButtonEnabled = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getDmUsersNavigatable ().hasNewerObject ());
		LastDMUser.enabled = lastButtonEnabled; // Disable the button for clicking
		LastDMUser.interactable = lastButtonEnabled; // Disable the button for highlighting

		// Determine if there are any older users. If there aren't, disable the button
		Boolean nextButtonEnabled = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getDmUsersNavigatable().hasOlderObject());
		NextDMUser.enabled = nextButtonEnabled; // Disable the button for clicking
		NextDMUser.interactable = nextButtonEnabled; // Disable the button for highlighting
	}

	public IEnumerator setDMUserProfilePic(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		DMConvoProfilePic.sprite = Sprite.Create(
			www.texture,
			new Rect(0, 0, www.texture.width, www.texture.height),
			new Vector2(0, 0));
	}

	// This function populates the DM users page with the next oldest user.
	public void nextUser()
	{
		// Get the older dm user (older message)
		currentUser = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getDmUsersNavigatable().getOlderObject());
		if (currentUser != null)
			setUser (currentUser);
		else
            DisplayManager.PushNotification("There is no next DM, something went wrong!");
	}

	// This function populates the DM users page with the next newest user.
	public void previousUser()
	{
		// Get the newer dm user (newer message)
		currentUser = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getDmUsersNavigatable().getNewerObject());
		if (currentUser != null)
			setUser (currentUser);
		else
            DisplayManager.PushNotification("There is no previous DM, something went wrong!");
	}

	// Populate the screen to view the entire conversation with a user.
    public void messageUser()
    {
        setActiveObject(viewConvObjectsString);

		string otherUser = currentUser.ScreenName;

		// Get all of the dms between the current user and the selected user
        List<DirectMessage> dms = authHandler.makeTwitterAPICall(
            () => authHandler.Interactor.buildDMConversation(otherUser));

        directMessages = dms;
        currentDMPage = 0; // the current page of messages is the first

        DMTitle.text = "Conversation with @" + otherUser;
        setDMPage(currentDMPage); // populate the screen with the first page of messages
    }

	// Populate the screen with the current set of messages
    public void setDMPage(int dmPage)
    {
		// Make sure the current page is within the bounds of the dm array
        if ((dmPage*5) <= directMessages.Count)
        {
            // Destroy all children, which are existing objects
            foreach (GameObject messageObject in messageObjects)
            {
                try
                {
                    GameObject.Destroy(messageObject);
                }
                catch(Exception e)
                {
					Debug.Log ("Error destroying object: " + messageObject.name);
					Crashlytics.RecordCustomException ("Twitter Exception", "thrown exception", e.StackTrace);
                }
            }

			// Get the user name of the current user.
            string currentUserName = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getCurrentUser());
            int numToStart = dmPage*5; // find the starting index of the array, there are 5 messages per page
            int YPoint = -125; // Starting point for the objects

			// Loop over each of the messages for the page and create the UI objects
            for (int i = numToStart; (i < numToStart + 5) && (i < directMessages.Count); i++)
            {
                DirectMessage currentDM = directMessages[i];
                if (currentDM.Sender.ScreenName == currentUserName) // if the message was sent by the current user
                {
                    GameObject go = Instantiate(Resources.Load("currentUserMessage")) as GameObject; // load the prefab
                    go.transform.SetParent(DMsHolder.transform, false); // set the prefab as a child of the list of dms

                    Text messageText = go.transform.Find("messageText").GetComponent<Text>(); //set the text of the message
                    messageText.text = currentDM.Text;

                    Text username = go.transform.Find("username").GetComponent<Text>(); // set the username of the message
                    username.text = currentDM.Sender.ScreenName;

                    Text time = go.transform.Find("time").GetComponent<Text>(); // set the time of the message
                    time.text = currentDM.CreatedAt.ToString("MM/dd/yy H:mm:ss");

                    go.transform.localPosition = new Vector3(0, YPoint); // put the message in the correct location

                    messageObjects.Add(go);
                }
                else if (currentDM.Recipient.ScreenName == currentUserName) // if the message was sent by the other user
                {
                    GameObject go = Instantiate(Resources.Load("otherUserMessage")) as GameObject; // load the prefab
                    go.transform.SetParent(DMsHolder.transform, false); // set the prefab as a child of the list of dms

                    Text messageText = go.transform.Find("messageText").GetComponent<Text>(); // set the text of the message
                    messageText.text = currentDM.Text;

                    Text username = go.transform.Find("username").GetComponent<Text>(); // set the username of the message
                    username.text = currentDM.Sender.ScreenName;

                    Text time = go.transform.Find("time").GetComponent<Text>(); // set the time of the message
                    time.text = currentDM.CreatedAt.ToString("MM/dd/yy H:mm:ss");

                    go.transform.localPosition = new Vector3(0, YPoint); // put the message in the correct location

                    messageObjects.Add(go);
                }

                YPoint += 63; // move the message to the next location for messages
            }
        }

        NewerDMs.enabled = dmPage > 0;
        NewerDMs.interactable = dmPage > 0;

        OlderDMs.enabled = ((dmPage + 1) * 5) <= directMessages.Count;
		OlderDMs.interactable = ((dmPage + 1) * 5) <= directMessages.Count;
    }

	// Go back from the conversation page to the dm users page
    public void BackToUsersPage()
    {
        currentUser = null;
        handleTwitterConversations();
    }

	// On click object to get the next page of older dms
    public void olderDMs()
    {
        if (currentDMPage < directMessages.Count - 1)
        {
            currentDMPage += 1;
        }
		else
		{
            DisplayManager.PushNotification("There are no older DMs, something went wrong!");
            return;
        }

        setDMPage(currentDMPage);
    }

    // On click object to get the next page of newer dms
    public void newerDMs()
    {
        if (currentDMPage > 0)
        {
            currentDMPage--;
        }
		else
		{
            DisplayManager.PushNotification("There are no newer DMs, something went wrong!");
            return;
        }

        setDMPage(currentDMPage);
    }

	// Function to contact the twitter api to message a user.
	public void Message(string msg)
	{
		authHandler.makeTwitterAPICallNoReturnVal( () => authHandler.Interactor.createDM(currentUser.ScreenName, msg));
		DisplayManager.PushNotification("Messaged!");

		messageUser ();
	}

	// Pulls up the on screen keyboard to dm a user.
	public void MessageUser()
	{
		KeyboardManager.GetInputFromKeyboard(Message);
	}
}
