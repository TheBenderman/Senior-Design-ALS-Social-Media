using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Event;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Template;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Connectome.KLD.Test
{

    public class ProfileTest : MonoBehaviour
    {
        /// <summary>
        /// Used for exporting
        /// </summary>
        private TestingSet LastTestingSet; 
        
        /// <summary>
        /// Holds states red 
        /// </summary>
        private List<IEmotivState>[] States;

        /// <summary>
        /// Keeps track of current session 
        /// </summary>
        private int SessionIndex;

        /// <summary>
        /// Indecates when break is running 
        /// </summary>
        private bool IsBreak;

        [Header("Overriding")]
        public bool OverideDurations;
        public int RecordDuration;
        public int RestDuration;

        [Header("Object Refrences")]
        public Text Title;
        public EmotivReaderPlugin Reader;

        public UnityEvent OnFinished;

        public void StartRecording(TestingSet TestingSet)
        {
            LastTestingSet = TestingSet; 
            try
            {
                Title.text = "";
                States = new List<IEmotivState>[TestingSet.Sessions.Length];

                for (int i = 0; i < States.Length; i++)
                {
                    States[i] = new List<IEmotivState>();
                }

                IsBreak = true;
                SessionIndex = 0;

                Reader.OnRead -= ListenToState;
                Reader.OnRead += ListenToState;

                int record = TestingSet.RecordDuration;
                int rest = TestingSet.RestDuration; 

                if(OverideDurations)
                {
                    record = RecordDuration;
                    rest = RestDuration; 
                }

                StartCoroutine(StartReading(TestingSet.Sessions, record, rest));
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void ListenToState(EmotivReadArgs e)
        {
            try
            {


                if (IsBreak)
                    return;

                //Debug.Log(e.State);

                States[SessionIndex].Add(e.State);
            }
            catch (Exception x)
            {
                Debug.Log(x);
            }
        }

        IEnumerator StartReading(EmotivCommandType[] Sessions, int RecordDuration, int RestDuration)
        {
            while (SessionIndex < Sessions.Length)
            {
                Title.text = "Break: next Target " + Sessions[SessionIndex];

                yield return new WaitForSeconds(RestDuration);

                IsBreak = false;
                Title.text = "Do " + Sessions[SessionIndex];

                yield return new WaitForSeconds(RecordDuration);
                IsBreak = true;
                SessionIndex++;
            }
            try
            {
                List<EmotivCommandType> uniqueList = new List<EmotivCommandType>();
                Dictionary<EmotivCommandType, Metrix> Metrix = new Dictionary<EmotivCommandType, Test.Metrix>();

                foreach (EmotivCommandType c in Sessions)
                {
                    if (!uniqueList.Contains(c))
                    {
                        uniqueList.Add(c);
                        Metrix.Add(c, new Metrix()); 
                    }
                }

                foreach (EmotivCommandType target in uniqueList)
                {
                   

                    for (int i = 0; i < States.Length; i++)
                    {
                        for (int j = 0; j < States[i].Count; j++)
                        {
                            Metrix met = Metrix[Sessions[i]];

                            //count 
                            met.size++; 

                            //correct 
                            if (Sessions[i] == States[i][j].Command)
                            {
                                met.TP++; 
                            }


                            if (States[i][j].Command == EmotivCommandType.PUSH)
                            {

                                if (States[i][j].Power > met.max)
                                {
                                    met.max = States[i][j].Power;
                                }
                                if (States[i][j].Power < met.min)
                                {
                                    met.min = States[i][j].Power;
                                }

                                met.totalPower += States[i][j].Power;
                            }


                            
                        }
                    }

                    //Displaying result 
                    string textToDisplay = "";

                   
                    foreach(var d in Metrix)
                    {
                        textToDisplay += "Total " + d.Key.ToString() + " = " + (((float)d.Value.TP) / d.Value.size).ToString("0.00") +
                                    "\n Max = " + d.Value.max.ToString("0.00") +
                                    " Min = " + d.Value.min.ToString("0.00") +
                                    " Avg = " + (d.Value.totalPower / d.Value.size).ToString("0.00") + "\n";                
                    }
                   
                    Title.text = textToDisplay; 
                }

                OnFinished.Invoke();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public void ExportCollectedData()
        {
            string filePath = UnityEditor.EditorUtility.SaveFilePanel("File Destination", "", "data", "csv");
            

            if (string.IsNullOrEmpty(filePath))
            {
                Debug.Log("bye");
                return;
            }
            else if (!filePath.EndsWith(".csv"))
            {
                Debug.Log("illegal");
                return;
            }

            //Show exporting window 
            Debug.Log("Exporting");
            FileStream fstream = File.Create(filePath);
            StreamWriter writer = new StreamWriter(fstream);

            writer.WriteLine("Test,Target,Time,Command,Power");
            for (int t = 0; t < LastTestingSet.Sessions.Length; t++)
            {
               
                for (int s = 0; s < States[t].Count; s++)
                {
                    writer.WriteLine("{0},{1},{2},{3},{4}", (t+1), LastTestingSet.Sessions[t].ToString(), States[t][s].Time, States[t][s].Command.ToString(), States[t][s].Power);
                }
            }

            writer.Flush(); 
            writer.Close(); 


            Debug.Log("Exported");
        }


       
    }//end class 

    public class Metrix
    {
        public float max = -0.01f;
        public float min = 1.01f;
        public float totalPower = 0f;
        public int TP = 0;
        public int size = 0;
    }

   
}//end namespace

