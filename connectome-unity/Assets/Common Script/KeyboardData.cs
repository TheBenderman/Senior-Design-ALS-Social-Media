using Connectome.Unity.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Boomlagoon.JSON;
using System.IO;
using System.Linq;

namespace Connectome.Unity.Keyboard
{
    //TODO this is slipt into two: Part of it is now KeyboardManager. Have this extend ConnectomeKeyboard 
    public class KeyboardData : KeyboardTemplate
    {
        public List<GameObject> KeyPanels;
        public bool GenerateKeysFromJSON;
        /// <summary>
        /// Called when this keyboard is loaded
        /// </summary>
        private void Start()
        {
            if (GenerateKeysFromJSON)
            {
                PopulateButtonText();
            }
        }
        /// <summary>
        /// In case we want to include this functionality.
        /// </summary>
        public bool IsCaps;
        private bool SymToggle = false;
        public void UpdateString(string text)
        {
            InputField.ConcatToCurrentText(IsCaps ? text.ToUpper() : text);
            //Having trouble with cursor not showing in the input field
            InputField.Select();
            InputField.caretPosition = InputField.selectionFocusPosition;
        }

        /// <summary>
        /// Updates the text using a text component (The text displayed on the button, for example).
        /// </summary>
        /// <param name="buttonText"></param>
        public void UpdateText(Text buttonText)
        {
            UpdateString(buttonText.text);
            if (SymToggle) ToggleSymbols();//Return to beginning after entering a symbol
        }

        /// <summary>
        /// Insert a space into the text field
        /// </summary>
        public void Space()
        {
            UpdateString(" ");
        }

        /// <summary>
        /// Do we want to backspace by word or by letter?
        /// </summary>
        public void BackspaceText()
        {
            if (InputField.text.Length > 0)
            {
                InputField.BackSpaceCurrentText();
            }
            //Having trouble with cursor not showing in the input field
            InputField.Select();
            InputField.caretPosition = InputField.selectionFocusPosition;
        }

        public override void Show()
        {
            transform.SetParent(DisplayManager.Instance.transform);
        }

        public override void Hide()
        {
            if (SymToggle) ToggleSymbols();//Reset back to default if the keyboard is exited while Symbols are up
            transform.SetParent(KeyboardManager.Instance.transform);
        }

        public void PopulateButtonText(string file)
        {
            //JSON files must have the same name as the corresponding keyboard prefab.
            JSONObject obj = JSONObject.Parse(File.ReadAllText("Assets/Resources/" + file + ".json"));
            //Populate the button text
            int i = 0;
            foreach (JSONValue row in obj.GetArray("data")){
                Button[] children = KeyPanels[i].GetComponentsInChildren<Button>();
                PopulateButtonText(children, row.Obj.GetArray("paneldata"));
                i++;
            }
        }

        public void PopulateButtonText()
        {
            PopulateButtonText(gameObject.name);
        }
        /// <summary>
        /// Recursively navigate the keyboard button heirarchy to set the text from the JSON file.
        /// Index default value is 1 because the button at children[0] is the root button, so ignore it.
        /// </summary>
        /// <param name="children"></param>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <returns>The index of the last child button that was set</returns>
        public int PopulateButtonText(Button[] children, JSONArray panel, int index = 1)
        {
            int j = index;
            foreach (JSONValue button in panel)
            {
                if (button.Obj.ContainsKey("buttonname"))
                {
                    string name = button.Obj.GetString("buttonname");
                    children[j].SetButtonText(name);
                    //Debug.Log(name);
                    j++;
                }
                else
                {
                    //The button at index j at this point is the one containing the children
                    j = PopulateButtonText(children, button.Obj.GetArray("paneldata"), j + 1);
                }
            }
            return j;
        }

        public void ToggleSymbols()
        {
            if (SymToggle)
            {
                PopulateButtonText(gameObject.name);
                SymToggle = false;
            }
            else
            {
                PopulateButtonText(gameObject.name + "Sym");
                SymToggle = true;
            }
        }
    }
}