using Boomlagoon.JSON;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MenuBuilder : MonoBehaviour {
    /// <summary>
    /// The panels that will be populated by the JSON file
    /// </summary>
    public List<GameObject> KeyPanels;
    // Use this for initialization
    protected virtual void Start () {
        PopulateButtonsFromFile(gameObject.name);
    }

    /// <summary>
    /// Read in a JSON file and begin populating the menu.
    /// </summary>
    /// <param name="file"></param>
    public void PopulateButtonsFromFile(string file)
    {
        //JSON files must have the same name as the corresponding keyboard prefab.
        JSONObject obj = JSONObject.Parse(File.ReadAllText("Assets/Resources/" + file + ".json"));
        //Populate the button text
        int i = 0;
        foreach (JSONValue row in obj.GetArray("data"))
        {
            Button[] children = KeyPanels[i].GetComponentsInChildren<Button>(true);
            PopulateButton(children, row.Obj.GetArray("paneldata"));
            i++;
        }
    }
    
    /// <summary>
    /// Recursively navigate the keyboard button heirarchy to set the text from the JSON file.
    /// Index default value is 1 because the button at children[0] is the root button, so ignore it.
    /// </summary>
    /// <param name="children"></param>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <returns>The index of the last child button that was set</returns>
    public int PopulateButton(Button[] children, JSONArray panel, int index = 1)
    {
        int j = index;
        foreach (JSONValue button in panel)
        {
            if (button.Obj.ContainsKey("buttonname"))
            {
                string name = button.Obj.GetString("buttonname");
                children[j].SetButtonText(name);
                children[j].gameObject.name = name;
                //Debug.Log(name);
                j++;
            }
            else
            {
                //The button at index j at this point is the one containing the children
                children[j].gameObject.name = button.Obj.GetString("panelname");
                j = PopulateButton(children, button.Obj.GetArray("paneldata"), j + 1);
            }
        }
        return j;
    }
}
