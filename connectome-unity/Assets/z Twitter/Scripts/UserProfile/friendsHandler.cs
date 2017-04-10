using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Twitter.API;
using CoreTweet;
using System;
using UnityEngine.UI;

public class friendsHandler : TwitterObjects {

	public GameObject friendObjects;
	public Text Description;
	public Text Title;
	public Image profilePic;
	public Text RealName;
	public Text TwitterHandle;
	// Know if we are browing followers or following.
	public bool isFollowers;

	private List<User> FollowingList;
	public List<User> followingList {
		get { return FollowingList; }
		set { FollowingList = value; }
	}

	private List<User> FollowersList;
	public List<User> followersList {
		get { return FollowersList; }
		set { FollowersList = value; }
	}

	private int currentFriend = 0;

	public AuthenticationHandler authHandler;


	public void showFollowers() {
		Title.text = "Followers";
		isFollowers = true;

		Debug.Log (authHandler.Interactor.getLoggedInUserScreenName ());

		List<User> followers = authHandler.makeTwitterAPICall( () =>
			authHandler.Interactor.getFollowers (
				authHandler.Interactor.getLoggedInUserScreenName()));

		followersList = followers;

		Debug.Log ("Filled followers");
		setFriend (followersList [currentFriend]);
	}

	public void showFollowing() {
		Title.text = "Following";
		isFollowers = false;

		List<User> following = authHandler.makeTwitterAPICall( () =>
			authHandler.Interactor.getFollowing (
				authHandler.Interactor.getLoggedInUserScreenName()));
		
		followingList = following;

		Debug.Log ("The COUNT IS " + followingList.Count);

		Debug.Log ("Filled following");
		setFriend (followingList [currentFriend]);
	}

	// This function sets the current tweet for the user.
	public void setFriend(User friend)
	{
		RealName.text = friend.Name;
		TwitterHandle.text = friend.ScreenName;
		Description.text = friend.Description;

		StartCoroutine(setFriendProfilePic (friend.ProfileImageUrl));
	}

	public IEnumerator setFriendProfilePic(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		profilePic.sprite = Sprite.Create(
			www.texture,
			new Rect(0, 0, www.texture.width, www.texture.height),
			new Vector2(0, 0));
	}

	public void nextFriend()
	{
		if (isFollowers) {
			if (currentFriend < followersList.Count) {
				currentFriend++;
				setFriend (followersList [currentFriend]);
			}
		} else {
			if (currentFriend < followingList.Count) {
				currentFriend++;
				setFriend (followingList [currentFriend]);
			}
		}
	}

	public void previousFriend()
	{
		if (isFollowers) {
			if (currentFriend > 0) {
				currentFriend--;
				setFriend (followersList [currentFriend]);
			}
		} else {
			if (currentFriend > followingList.Count) {
				currentFriend--;
				setFriend (followingList [currentFriend]);
			}
		}
	}
}
