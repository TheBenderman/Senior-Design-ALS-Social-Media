using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CoreTweet;
using UnityEditor;

public class DirectMessageHandler : TwitterObjects {
	#region DM Members
	public GameObject DMObjects;
	public GameObject SelectConvObjects;
	public GameObject ViewConvObjects;
	public Image DMConvoProfilePic;
	public Text DMName;
	public Text DMUsername;
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

	private List<User> conversationUsers;
    private List<DirectMessage> directMessages;
	private int currentUser = 0;
    private int currentDMPage = 0;
	public AuthenticationHandler authHandler;
	public string selectConvoObjectsString = "SelectConvObjects";
    public string viewConvObjectsString = "ViewConvObjects";
    #endregion

    public void handleTwitterConversations()
	{
		List<User> users = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getUniqueDMs());

		setActiveObject(selectConvoObjectsString);

		conversationUsers = users;
		setUser(currentUser);
	}

	// This function sets the current tweet for the user.
	public void setUser(int index)
	{
		StartCoroutine(setDMUserProfilePic(conversationUsers[index].ProfileImageUrl));
		DMUsername.text = conversationUsers[index].ScreenName;
		DMName.text = conversationUsers[index].Name;
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
		if (currentUser < conversationUsers.Count - 1)
		{
			currentUser += 1;
		}

		setUser(currentUser);
	}

	// This function populates the timeline ui with the last tweet in the list.
	public void previousUser()
	{
		if (currentUser > 0)
		{
			currentUser--;
		}

		setUser(currentUser);
	}

    public void messageUser()
    {
        setActiveObject(viewConvObjectsString);

        string currentUser = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getCurrentUser());

        Debug.Log("Current user : " + currentUser);

        string otherUser = conversationUsers[this.currentUser].ScreenName;

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
        currentUser = 0;
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
