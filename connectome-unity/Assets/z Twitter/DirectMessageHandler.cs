using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoreTweet;

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
	private List<User> conversationUsers;
	private int currentUser = 0;
	public AuthenticationHandler authHandler;
	public string convoObjectsString = "SelectConvObjects";
	#endregion

	public void handleTwitterConversations()
	{
		List<User> users = authHandler.makeTwitterAPICall(() => authHandler.Interactor.getUniqueDMs());

		setActiveObject(convoObjectsString);

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
}
