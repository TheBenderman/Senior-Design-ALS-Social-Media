using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Start_Screen : MonoBehaviour {

    public Text sliderValue;
    public Slider slider;
    public Dropdown deviceDropdown;
    public Button button;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public InputField profileNameInputField;
    public Toggle ssvepToggle;
    public static System.Boolean ssvepIsOn;
    public static string username;
    public static string password;
    public static string profile;
    public static float sliderLength;
    public static int deviceValue;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        sliderValue.text = slider.value.ToString();
        sliderLength = slider.value;
        deviceValue = deviceDropdown.value;
        setupDevice(deviceValue);
        ssvepIsOn = ssvepToggle.isOn;
		
	}

    void setupDevice(int profileID)
    {
        if(profileID == 1)
        {
            username = "emotiv123";
            password = "Emotivbci123";
            profile = "SSVEP_profile";
        }
        else if(profileID == 2)
        {
            username = "emotiv123";
            password = "Emotivbci123";
            profile = "KLD_Blink";
        }
        else if(profileID == 3)
        {
                username = usernameInputField.text;
                password = passwordInputField.text;
                profile = profileNameInputField.text;
        }
        else
        {
            username = null;
            password = null;
            profile = null;
        }
    }

    void ReadValues()
    {

    }
}
