using Connectome.Unity.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Boomlagoon.JSON;
using System.IO;

namespace Connectome.Unity.Keyboard
{
    //TODO this is slipt into two: Part of it is now KeyboardManager. Have this extend ConnectomeKeyboard 
    public class KeyboardData : KeyboardTemplate
    {
        public List<GameObject> PhraseTypes;
        /// <summary>
        /// Called when this keyboard is loaded
        /// </summary>
        private void Start()
        {
            PopulateButtonText();
        }
        /// <summary>
        /// In case we want to include this functionality.
        /// </summary>
        public bool IsCaps;
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
            transform.SetParent(KeyboardManager.Instance.transform);
        }

        public void PopulateButtonText()
        {
            //JSON files must have the same name as the corresponding keyboard prefab.
            JSONObject obj = JSONObject.Parse(File.ReadAllText("Assets/Resources/" + gameObject.name + ".json"));
            //Populate the button text
            int i = 0;
            foreach (JSONValue panel in obj.GetArray("data")){
                Button[] phrases = PhraseTypes[i].GetComponentsInChildren<Button>(true);
                JSONArray paneldata = panel.Obj.GetArray("paneldata");
                int j = 1;//Start from 1 because GetComponentsInChildren includes the parent object as well, so filter that out.
                foreach (JSONValue button in paneldata)
                {
                    phrases[j].SetButtonText(button.Obj.GetString("buttonname"));
                    j++;
                }
                i++;
            }

        }
    }
}