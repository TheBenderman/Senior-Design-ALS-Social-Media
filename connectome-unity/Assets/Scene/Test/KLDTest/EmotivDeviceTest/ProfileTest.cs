using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Manager;
using Connectome.Unity.Template;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Connectome.KLD.Test
{
    //worst than clorox in the eyes 
    public class ProfileTest : MonoBehaviour
    {
        /// <summary>
        /// Used for exporting
        /// </summary>
        private EmotivCommandType[] LastTestingSessions; 
        
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
        public bool ShouldOverideDurations;
        public int RecordDuration;
        public int RestDuration;
        public EmotivCommandType[] TestSessions; 

        [Header("Object Refrences")]
        public Text Title;
        public EmotivDeviceManager DeviceManager;
        public EmotivRateCalculator Calculator;
        public EmotivCalculatorConfiguration Config; 

        public UnityEvent OnFinished;

        public void StartRecording(TestingSet TestingSet)
        {
            int record = TestingSet.RecordDuration;
            int rest = TestingSet.RestDuration;
            EmotivCommandType[] sessions = TestingSet.Sessions;

            if (ShouldOverideDurations)
            {
                record = RecordDuration;
                rest = RestDuration;
                sessions = TestSessions;
            }

            LastTestingSessions = sessions; 

            try
            {
                Title.text = "";
                States = new List<IEmotivState>[sessions.Length];

                for (int i = 0; i < States.Length; i++)
                {
                    States[i] = new List<IEmotivState>();
                }

                IsBreak = true;
                SessionIndex = 0;

                DeviceManager.ReaderPlugin.OnRead -= ListenToState;
                DeviceManager.ReaderPlugin.OnRead += ListenToState;

                

                StartCoroutine(StartReading(sessions, record, rest));
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void ListenToState(IEmotivState e)
        {
            try
            {


                if (IsBreak)
                    return;

                //Debug.Log(e.State);

                States[SessionIndex].Add(e);
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

                Dictionary<EmotivCommandType, Metrix> Metrix = new Dictionary<EmotivCommandType, Metrix>();

                //Dictionary<EmotivCommandType, List<float>> TypeAvergaeAccuracy = new Dictionary<EmotivCommandType, List<float>>();

                foreach (EmotivCommandType c in Sessions)
                {
                    if (!uniqueList.Contains(c))
                    {
                        uniqueList.Add(c);
                        Metrix.Add(c, new Metrix());
                        //TypeAvergaeAccuracy.Add(c,new List<float>());
                    }
                }

                for (int i = 0; i < Sessions.Length; i++)
                {
                    int TP = 0;
                    int size = 0;

                    Metrix met = Metrix[Sessions[i]];

                    for (int j = 0; j < States[i].Count; j++)
                    {
                        //count 
                        size++;

                        //correct 
                        if (Sessions[i] == States[i][j].Command)
                        {
                            TP++;
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
                    met.TP += TP;
                    met.size += size;

                    Config.TargetCommand = Sessions[i]; 
                    met.Rates.Add(Calculator.Calculate(States[i], Config));

                    //TypeAvergaeAccuracy[Sessions[i]].Add(((float)TP) / size);
                }

                //printing results 
                //Displaying result 
                string textToDisplay = "";
                   
                foreach(var d in Metrix)
                {
                    textToDisplay += "Total " + d.Key.ToString() + " = " + (((float)d.Value.TP) / d.Value.size).ToString("0.00") +
                                "\n Max = " + d.Value.max.ToString("0.00") +
                                " Min = " + d.Value.min.ToString("0.00") +
                                " Avg = " + (d.Value.totalPower / d.Value.size).ToString("0.00") + "\n";                
                }

                //std? i failed stat 

                Dictionary<EmotivCommandType, float> difference = new Dictionary<EmotivCommandType, float>();
                string diffBuild = "";
                float totalD = 0; 

                foreach (EmotivCommandType uc in uniqueList)
                {
                    difference.Add(uc, 0); 

                    float averageAvg = 0;

                    foreach (float avg in Metrix[uc].Rates)
                    {
                        averageAvg += avg; 
                    }

                    averageAvg /= Metrix[uc].Rates.Count;
                   
                    foreach (float avg in Metrix[uc].Rates)
                    {
                        difference[uc] += Math.Abs(averageAvg - avg); 
                    }

                    difference[uc] /= Metrix[uc].Rates.Count;

                    totalD = difference[uc]; 

                    diffBuild += "C["+uc.ToString()+"]: " + difference[uc] + "\n"; 
                }
                   
                Title.text = textToDisplay + diffBuild + "Average C:" + totalD/ uniqueList.Count; 

                if (OnFinished != null)
                {
                    OnFinished.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }//end func 

        public void ExportCollectedData()
        {
            string filePath = "data.csv"; 
            #if UNITY_EDITOR
            filePath = UnityEditor.EditorUtility.SaveFilePanel("File Destination", "", "data", "csv");
            #endif

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

            writer.WriteLine("Test,Target,Command,Time,Power");
            for (int t = 0; t < LastTestingSessions.Length; t++)
            {
                if (States[t].Count > 0)
                {
                    long initTime = States[t][0].Time;
                    for (int s = 0; s < States[t].Count; s++)
                    {
                        writer.WriteLine("{0},{1},{2},{3},{4}", (t + 1), LastTestingSessions[t].ToString(), States[t][s].Command.ToString(), States[t][s].Time - initTime, States[t][s].Power);
                    }
                }
                writer.WriteLine(",,,,");
            }

            writer.Flush(); 
            writer.Close(); 

            Debug.Log("Exported");
        }


        private void OnValidate()
        {
            if(DeviceManager == null)
            {
                // DeviceManager = EmotivDeviceManager.Instance; //rip singleton 
            }
        }
    }//end class 

    public class Metrix
    {
        public float max = -0.01f;
        public float min = 1.01f;
        public float totalPower = 0f;
        public int TP = 0;
        public int size = 0;
        public List<float> Rates = new List<float>(); 
    }
}//end namespace

