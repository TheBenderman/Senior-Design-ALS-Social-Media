using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour {
    GameObject Keyboard;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setKeyboard (string keyboardtype)
    {
        Keyboard = Instantiate(Resources.Load(keyboardtype),GameObject.Find("Canvas").transform) as GameObject;
        GameObject.FindGameObjectWithTag("Exit").GetComponent<Button>().onClick.AddListener(() =>
        {
            removeKeyboard();
        });
    }

    public void removeKeyboard()
    {
        Destroy(Keyboard.gameObject);
    }
}
