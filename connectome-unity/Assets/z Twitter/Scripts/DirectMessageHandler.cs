using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CoreTweet;
using System;

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
	public string selectConvoObjectsString = "SelectConvObjects";
    public string viewConvObjectsString = "ViewConvObjects";
    #endregion

    public void handleTwitterConversations()
	{
		authHandler.makeTwitterAPICallNoReturnVal (() => authHandler.Interactor.getDmUsersNavigatable().resetCurrentObject());

		setActiveObject(selectConvoObjectsString);

		currentUser = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getDmUsersNavigatable().getNewerObject());
		if (currentUser != null)
			setUser (currentUser);
		else
			throw new Exception ("No next user.");
	}

	// This function sets the current tweet for the user.
	public void setUser(User user)
	{
		StartCoroutine(setDMUserProfilePic(user.ProfileImageUrl));
		DMUsername.text = user.ScreenName;
		DMName.text = user.Name;

		string currentUser = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getCurrentUser());

		Debug.Log("Current user : " + currentUser);

		string otherUser = user.ScreenName;

		Debug.Log("Other User : " + otherUser);

		List<DirectMessage> dms = authHandler.makeTwitterAPICall(
			() => authHandler.Interactor.buildDMConversation(otherUser));

		DirectMessage latestMessage = dms.OrderByDescending((arg) => arg.CreatedAt).First();

		LastMessageText.text = latestMessage.Sender.ScreenName + " - (" + Utilities.ElapsedTime(latestMessage.CreatedAt.DateTime)
			+ ") - " + latestMessage.Text;

		LastDMUser.enabled = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getDmUsersNavigatable().hasNewerObject());
		NextDMUser.enabled = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getDmUsersNavigatable().hasOlderObject());
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

	// This function populates the timeline ui with the next tweet in the list.
	public void nextUser()
	{
		currentUser = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getDmUsersNavigatable().getOlderObject());
		if (currentUser != null)
			setUser (currentUser);
		else
			throw new Exception ("No next user.");
	}

	// This function populates the timeline ui with the last tweet in the list.
	public void previousUser()
	{
		currentUser = authHandler.makeTwitterAPICall (() => authHandler.Interactor.getDmUsersNavigatable().getNewerObject());
		if (currentUser != null)
			setUser (currentUser);
		else
			throw new Exception ("No previous user.");
	}

    public void messageUser()
    {
        setActiveObject(viewConvObjectsString);

        string currentUserName = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getCurrentUser());

        Debug.Log("Current user : " + currentUserName);

		string otherUser = currentUser.ScreenName;

        Debug.Log("Other User : " + otherUser);

        List<DirectMessage> dms = authHandler.makeTwitterAPICall(
            () => authHandler.Interactor.buildDMConversation(otherUser));

        directMessages = dms;
        currentDMPage = 0;

        DMTitle.text = "Conversation with @" + otherUser;
        setDMPage(currentDMPage);
    }

    public void setDMPage(int dmPage)
    {
        if ((dmPage*5) <= directMessages.Count)
        {
            // Destroy all children
            foreach (GameObject messageObject in messageObjects)
            {
                try
                {
                    GameObject.Destroy(messageObject);
                }
                catch (System.Exception ex)
                {
                    
                }
            }

            string currentUserName = authHandler.makeTwitterAPICall(
                () => authHandler.Interactor.getCurrentUser());
            int numToStart = dmPage*5;
            int YPoint = -125;

            for (int i = numToStart; (i < numToStart + 5) && (i < directMessages.Count); i++)
            {
                DirectMessage currentDM = directMessages[i];
                if (currentDM.Sender.ScreenName == currentUserName)
                {
                    GameObject go = Instantiate(Resources.Load("currentUserMessage")) as GameObject;
                    go.transform.SetParent(DMsHolder.transform, false);

                    Text messageText = go.transform.Find("messageText").GetComponent<Text>();
                    messageText.text = currentDM.Text;

                    Text username = go.transform.Find("username").GetComponent<Text>();
                    username.text = currentDM.Sender.ScreenName;

                    Text time = go.transform.Find("time").GetComponent<Text>();
                    time.text = currentDM.CreatedAt.ToString("MM/dd/yy H:mm:ss");

                    go.transform.localPosition = new Vector3(0, YPoint);

                    messageObjects.Add(go);
                }
                else if (currentDM.Recipient.ScreenName == currentUserName)
                {
                    GameObject go = Instantiate(Resources.Load("otherUserMessage")) as GameObject;
                    go.transform.SetParent(DMsHolder.transform, false);

                    Text messageText = go.transform.Find("messageText").GetComponent<Text>();
                    messageText.text = currentDM.Text;

                    Text username = go.transform.Find("username").GetComponent<Text>();
                    username.text = currentDM.Sender.ScreenName;

                    Text time = go.transform.Find("time").GetComponent<Text>();
                    time.text = currentDM.CreatedAt.ToString("MM/dd/yy H:mm:ss");

                    go.transform.localPosition = new Vector3(0, YPoint);

                    messageObjects.Add(go);
                }

                YPoint += 63;
            }
        }
    }

    public void BackToUsersPage()
    {
        currentUser = null;
        handleTwitterConversations();
    }

    public void olderDMs()
    {
        if (currentDMPage < directMessages.Count - 1)
        {
            currentDMPage += 1;
        }

        setDMPage(currentDMPage);
    }

    // This function populates the timeline ui with the last tweet in the list.
    public void newerDMs()
    {
        if (currentDMPage > 0)
        {
            currentDMPage--;
        }

        setDMPage(currentDMPage);
    }
}
