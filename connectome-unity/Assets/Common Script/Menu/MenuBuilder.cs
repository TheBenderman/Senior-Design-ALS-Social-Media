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
        JSONObject obj;

        try
        {
            obj = JSONObject.Parse(File.ReadAllText("Assets/Resources/" + file + ".json"));
        }
        catch
        {
            obj = GetDefaultJson();
        }
        
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


    private JSONObject GetDefaultJson()
    {
        //string grid json =  "{\"keyboardtitle\":\"GridKeyboard\",\"data\":[{\"panelname\":\"Row1\",\"paneldata\":[{\"panelname\":\"Row1Group1\",\"paneldata\":[{\"buttonname\":\"A\"},{\"buttonname\":\"C\"},{\"buttonname\":\"N\"},{\"buttonname\":\"Y\"}]},{\"panelname\":\"Row1Group2\",\"paneldata\":[{\"buttonname\":\"F\"},{\"buttonname\":\"L\"},{\"buttonname\":\"T\"},{\"buttonname\":\"U\"}]},{\"buttonname\":\"Space\"},{\"buttonname\":\"Backspace\"}]},{\"panelname\":\"Row2\",\"paneldata\":[{\"panelname\":\"Row2Group1\",\"paneldata\":[{\"buttonname\":\"B\"},{\"buttonname\":\"H\"},{\"buttonname\":\"R\"},{\"buttonname\":\"S\"}]},{\"panelname\":\"Row2Group2\",\"paneldata\":[{\"buttonname\":\"D\"},{\"buttonname\":\"E\"},{\"buttonname\":\"P\"},{\"buttonname\":\"W\"}]},{\"buttonname\":\"Phrases\"},{\"buttonname\":\"123/Sym\"}]},{\"panelname\":\"Row3\",\"paneldata\":[{\"panelname\":\"Row3Group1\",\"paneldata\":[{\"buttonname\":\"G\"},{\"buttonname\":\"I\"},{\"buttonname\":\"J\"},{\"buttonname\":\"M\"},{\"buttonname\":\"Z\"}]},{\"panelname\":\"Row3Group2\",\"paneldata\":[{\"buttonname\":\"K\"},{\"buttonname\":\"O\"},{\"buttonname\":\"Q\"},{\"buttonname\":\"V\"},{\"buttonname\":\"X\"}]},{\"buttonname\":\"Exit\"},{\"buttonname\":\"Submit\"}]}]}";
        string json = "{\"keyboardtitle\":\"PhraseKeyboard\",\"data\":[{\"panelname\":\"Utlity\",\"paneldata\":[{\"buttonname\":\"Exit\"},{\"buttonname\":\"Submit\"}]},{\"panelname\":\"Top\",\"paneldata\":[{\"buttonname\":\"WOW!\"},{\"buttonname\":\"AMAZING!\"},{\"buttonname\":\"COOL!\"}]},{\"panelname\":\"Middle\",\"paneldata\":[{\"buttonname\":\"GOOD\"},{\"buttonname\":\"MEH\"},{\"buttonname\":\"YOU TRIED\"}]},{\"panelname\":\"Bottom\",\"paneldata\":[{\"buttonname\":\"YIKES!\"},{\"buttonname\":\"DISASTER!\"},{\"buttonname\":\"AY CARAMBA!\"}]}]}";

        return JSONObject.Parse(json);
    }
}
