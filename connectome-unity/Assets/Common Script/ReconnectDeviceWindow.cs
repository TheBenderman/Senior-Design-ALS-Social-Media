using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Implementation;
using System;
using UnityEngine.UI;



public class ReconnectDeviceWindow: MonoBehaviour
{
	public int Timer = 0;
	public Text AttemptText;
	public Text ErrorText;

	/// <summary>
	/// Starts a thread to attempt to reconnect to a device every 2 seconds.
	/// </summary>
	void Start() 
	{
		StartCoroutine (AttemptReconnect ());
	}

	/// <summary>
	/// Attempts to reconnect to a device once connection is lost.
	/// </summary>
	/// <returns>IEnumerator</returns>
	public IEnumerator AttemptReconnect() 
	{
		while (!Connect())
		{
			Timer++;
			AttemptText.text = Timer.ToString();
			yield return new WaitForSeconds (2f);
		}
				
		AttemptText.text = "Connected!";
		yield return new WaitForSeconds (4f);
		Destroy (this.gameObject);
		yield return null;
	}

	/// <summary>
	/// Connect to the device or show an error message.
	/// </summary>
	/// <returns>bool</returns>
	public bool Connect() 
	{ 
		//string err;
		//EmotivDeviceManager.Instance.DevicePlugin.Connect (); jess... -KLD
        return true;  //TODO fix this -KLD
	}


}


