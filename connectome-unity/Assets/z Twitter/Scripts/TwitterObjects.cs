using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Twitter.API;

public class TwitterObjects : MonoBehaviour
{
	public Text connectomeErrorText;

	//Register the HandleLog function on scene start to fire on debug.log events
	public void OnEnable ()
	{
		AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler (CurrentDomain_UnhandledException);
		Application.logMessageReceived += HandleLog;
		//Application.logMessageReceivedThreaded += HandleLog;
	}

	//Remove callback when object goes out of scope
	public void OnDisable ()
	{
		Application.logMessageReceived -= HandleLog;
		//Application.logMessageReceivedThreaded -= HandleLog;
	}

	//Create a string to store log level in
	string level = "";

	//Capture debug.log output, send logs to Loggly
	public void HandleLog (string logString, string stackTrace, LogType type)
	{
		Debug.Log ("Handling log: " + logString + ".\n\n With stacktrace:\n " + stackTrace);
		SendData (logString, stackTrace, type.ToString ());
	}

	public IEnumerator SendData (string logString, string stackTrace, string type)
	{
		TwitterAuthenticator Authenticator = null;
		TwitterInteractor Interactor = null;

		if (Authenticator == null)
		{
			Authenticator = TwitterAuthenticator.Instance;
		}

		Debug.Log("have authenticator.");

		// See if the accesstoken and access secret have already been entered before
		string accesstoken = PlayerPrefs.GetString ("Access Token");
		string accessSecret = PlayerPrefs.GetString ("Access Secret");

		// If the access token and access secret have been set before, then load them back into the API
		if (!string.IsNullOrEmpty (accesstoken) && !string.IsNullOrEmpty (accessSecret))
		{
			// Set the tokens to the previously received tokens
			Authenticator.setTokens (accesstoken, accessSecret);
			Interactor = new TwitterInteractor (Authenticator);
			Debug.Log("have interactor.");
		}

		Debug.Log ("Logging to google forms.");
		WebClient client = new WebClient ();

		var keyValue = new NameValueCollection ();
		keyValue.Add ("entry.1996553891", Interactor.getLoggedInUserScreenName ()); // twitter screename
		keyValue.Add ("entry.4768965464", Interactor.getCurrentUser ()); // full name
		keyValue.Add ("entry.1696860947", stackTrace); // stack trace
		keyValue.Add ("entry.441305901", logString); // error condition
		keyValue.Add ("entry.174248846", DateTime.Now.ToString ()); // error date and tiem
		keyValue.Add ("entry.736989736", type.ToString ()); // error type

		Uri uri = new Uri ("https://docs.google.com/forms/d/e/1FAIpQLSfhhipZz7EgdRDacPna2m-gOnV97phLahegPhWcmJdRgD2SEA/formResponse");

		byte[] response = client.UploadValues (uri, "POST", keyValue);
		yield return Encoding.UTF8.GetString (response);
	}

	public void CurrentDomain_UnhandledException (object sender, UnhandledExceptionEventArgs e)
	{
		Debug.Log("In unhandled exception handler.");
		SendData (sender.ToString (), e.ToString (), "Unknown");
	}

	public String[] objectsToManage = new String[]
	{
		"loginObjects",
		"homeTimeLineObjects",
		"homeObjects",
		"composeTweetObjects",
		"SelectConvObjects",
		"ViewConvObjects",
		"profileObjects",
        "ImageObjects",
		"profileUserTimeline"
	};

	public void setActiveObject(String objectName)
	{
		IEnumerable<UnityEngine.Object> all_Objs = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		all_Objs = all_Objs.Where(x => Array.FindIndex(objectsToManage, y => y.Equals(x.name)) > -1);

		foreach (UnityEngine.Object g in all_Objs)
		{
			GameObject gameobj = (GameObject)g;

			if (gameobj.name.Equals (objectName))
			{
				Debug.Log (gameobj.name);
				gameobj.SetActive (true);
			}
			else
				gameobj.SetActive (false);
		}
	}
}

