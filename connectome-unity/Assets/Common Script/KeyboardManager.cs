using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour {
    GameObject Keyboard;
    public string KeyboardPrefabName;
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
            Keyboard.GetComponent<KeyboardData>().ActiveField.text = "";
            removeKeyboard();
        });
    }

    public void setKeyboard()
    {
        setKeyboard(KeyboardPrefabName);
    }

    public void setActiveTextBox(InputField field)
    {
        Keyboard.GetComponent<KeyboardData>().ActiveField = field;
    }

    public void removeKeyboard()
    {
        Destroy(Keyboard.gameObject);
    }
}
