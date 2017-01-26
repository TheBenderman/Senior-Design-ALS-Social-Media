using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Twitter;
using Connectome.Twitter.API;

public class TwitterController : MonoBehaviour {

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
    public Text homeTimeLineText;
    #endregion

    private static TwitterAPI api;

	// Use this for initialization
	void Start () {
        if (api == null)
            api = TwitterAPI.Instance;

        homeTimeLineObjects.SetActive(false);
        loginObjects.SetActive(true);

        authenticationURL.text = "The authorization URL has been copied to your clipboard! Please visit this url to authenticate twitter.";

        TextEditor te = new TextEditor();
        te.text = api.getAuthorizationURL();
        te.SelectAll();
        te.Copy();

        userMessage.text = "Please input the pin code that you received from Twitter: ";
	}

    public void onPinEnter()
    {
        string pinCode = userInput.text;

        if (string.IsNullOrEmpty(pinCode))
        {
            errorMessage.text = "Please input a value for the pin code!";
        }

        api.enterPinCode(pinCode);

        ///
        /// NEED TO VALIDATE THAT THE PIN CODE WAS ENTERED CORRECTLY HERE.
        ///

        addHomeTimeLine();
    }

    public void addHomeTimeLine()
    {
        homeTimeLineObjects.SetActive(true);
        loginObjects.SetActive(false);

        homeTimeLineText.text = api.getTop5HomeTimeLineTweets();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
