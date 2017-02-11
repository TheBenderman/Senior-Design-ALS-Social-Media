using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour {
    GameObject Keyboard;
    public string KeyboardPrefabName;
    /// <summary>
    /// TODO? This value will eventually come from some fixed spot, based on the social media we are using.
    /// </summary>
    public int TextLimit;

    public void setKeyboard (string keyboardtype)
    {
        Keyboard = Instantiate(Resources.Load(keyboardtype),GameObject.Find("Canvas").transform) as GameObject;
        //Commenting this out to try calling the same methods from Keyboard Data.
        //This would elminiate the need to find the game object with the Exit tag just to set these methods via code, and just do it in the editor.
        /*GameObject.FindGameObjectWithTag("Exit").GetComponent<Button>().onClick.AddListener(() =>
        {
            Keyboard.GetComponent<KeyboardData>().ActiveField.text = "";
            removeKeyboard();
            SelectionManager.Instance.PopSelections();
        });*/
        SelectionManager.Instance.PushSelections(Keyboard.GetComponent<KeyboardData>().BaseSelections);
        //Do this here in case we have apps with different character limits, so just this value has to change to change the keyboards.
        Keyboard.GetComponent<KeyboardData>().ActiveField.characterLimit = TextLimit;
    }

    public void setKeyboard()
    {
        setKeyboard(KeyboardPrefabName);
    }

    public void removeKeyboard()
    {
        Destroy(Keyboard.gameObject);
    }
}
