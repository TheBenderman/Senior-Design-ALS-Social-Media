using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using UnityEngine.UI;
using System;
using Connectome.Unity.UI;

public abstract class BaseTrainingScreen : MonoBehaviour {

    public IEmotivDevice device;
    public IEmotivReader reader;

    public Slider slider;
    public Button button;
    public Button backButton;

    public SelectionHighlighter highlighter;
    public Button flashingButton;
    public Button neutralButton;

    public GameObject mainMenu;
    public GameObject currentPanel;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected void deviceSetup(int a)
    {
        if (Start_Screen.username == null)
        {
            device = new RandomEmotivDevice(); //DisplayManager.PopUpVirtualUnityDevice();
            Start_Screen.profile = "Testing";
        }
        else if (Start_Screen.username != null)
        {
            device = new EPOCEmotivDevice(Start_Screen.username, Start_Screen.password, Start_Screen.profile);
        }


    }


   protected void updateButtonColor(Color c)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = c;
        button.colors = cb;
    }

    protected void setButtonText(String text)
    {
        button.transform.GetChild(0).GetComponent<Text>().text = text;
    }

    public void ssvepOff()
    {
            neutralButton.gameObject.SetActive(false);
            highlighter.gameObject.SetActive(false);
            flashingButton.gameObject.SetActive(false);
    }

    public void ssvepOn()
    {
        if (Start_Screen.ssvepIsOn)
        {
            neutralButton.gameObject.SetActive(true);
            flashingButton.gameObject.SetActive(true);
            highlighter.gameObject.SetActive(true);
        }
        else
        {
            ssvepOff();
        }
    }

    public abstract void reset();
}
