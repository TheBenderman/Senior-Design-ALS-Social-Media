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
using Fabric.Crashlytics;

namespace Connectome.KLD.Test
{
    //worst than clorox in the eyes - this sux. 
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
        public bool OverideDurations;
        public int RecordDuration;
        public int RestDuration;
        public bool OveridePrompts;
        public EmotivCommandType[] Prompts; 

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
            EmotivCommandType[] prompts = TestingSet.Prompts;

            if (OverideDurations)
            {
                record = RecordDuration;
                rest = RestDuration;
            }
            if (OveridePrompts)
            {
                prompts = Prompts;
            }
            LastTestingSessions = prompts; 

            try
            {
                Title.text = "";
                States = new List<IEmotivState>[prompts.Length];

                for (int i = 0; i < States.Length; i++)
                {
                    States[i] = new List<IEmotivState>();
                }

                IsBreak = true;
                SessionIndex = 0;

                DeviceManager.ReaderPlugin.OnRead -= ListenToState;
                DeviceManager.ReaderPlugin.OnRead += ListenToState;

                

                StartCoroutine(StartReading(prompts, record, rest));
            }
            catch (Exception e)
            {
                Debug.Log(e);
				Crashlytics.RecordCustomException ("Profile Exception", "thrown exception", e.StackTrace);
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
				Crashlytics.RecordCustomException ("Profile Exception", "thrown exception", x.StackTrace);
            }
        }

        IEnumerator StartReading(EmotivCommandType[] Sessions, int RecordDuration, int RestDuration)
        {
            while (SessionIndex < Sessions.Length)
            {
                Title.text = "Break - Target " + Sessions[SessionIndex];

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
                   
                /* useless shit
                 * foreach(var d in Metrix)
                {
                    textToDisplay += "Total " + d.Key.ToString() + " = " + (((float)d.Value.TP) / d.Value.size).ToString("0.00") +
                                "\n Max = " + d.Value.max.ToString("0.00") +
                                " Min = " + d.Value.min.ToString("0.00") +
                                " Avg = " + (d.Value.totalPower / d.Value.size).ToString("0.00") + "\n";                
                }*/

                //std? i failed stat 

                Dictionary<EmotivCommandType, float> variance = new Dictionary<EmotivCommandType, float>();
                Dictionary<EmotivCommandType, List<float>> AllowedVariance = new Dictionary<EmotivCommandType, List<float>>();

                Dictionary<EmotivCommandType, float> averages = new Dictionary<EmotivCommandType, float>();

                float allTotalVariance = 0;
                float ValidTotalVariance = 0;

                float OverlayLimit = .20f;

                Dictionary<EmotivCommandType, int> OutliersCount = new Dictionary<EmotivCommandType, int>();

                int totalOutliers = 0; 

                float MinPushRateOnPush = 1.01f;
                float MaxPushRateOnNeutral = -0.01f;

                float AllMinPushRateOnPush = 1.01f;
                float AllMaxPushRateOnNeutral = -0.01f;


                foreach (EmotivCommandType uc in uniqueList)
                {
                    variance.Add(uc, 0);
                    AllowedVariance.Add(uc, new List<float>());
                    OutliersCount.Add(uc, 0); 
                    averages.Add(uc, 0); 

                    //calculate average
                    float averageAvg = 0;

                    foreach (float avg in Metrix[uc].Rates)
                    {
                        averageAvg += avg;
                    }

                    averageAvg /= Metrix[uc].Rates.Count;

                    averages[uc] = averageAvg;


                    textToDisplay += uc.ToString() + " ("+ averageAvg.ToString("0.00") + "),";
 
                    //calculate variance 
                    foreach (float avg in Metrix[uc].Rates)
                    {
                        textToDisplay += avg.ToString("0.00");
                        if (uc == EmotivCommandType.PUSH)
                        {
                            AllMinPushRateOnPush = Math.Min(AllMinPushRateOnPush, avg);
                        }
                        else if (uc == EmotivCommandType.NEUTRAL)
                        {
                            AllMaxPushRateOnNeutral = Math.Max(AllMaxPushRateOnNeutral, (1 - avg));
                        }

                        variance[uc] += Math.Abs(averageAvg - avg);

                        if (Math.Abs(avg - averageAvg) >= OverlayLimit)
                        {
                            textToDisplay += "*,";
                            OutliersCount[uc]++;
                            totalOutliers++;  //terribale 
                            continue;
                        }

                        textToDisplay += ",";

                        AllowedVariance[uc].Add(Math.Abs(averageAvg - avg)); 

                        if (uc == EmotivCommandType.PUSH)
                        {
                            MinPushRateOnPush = Math.Min(MinPushRateOnPush, avg);
                        }
                        else if (uc == EmotivCommandType.NEUTRAL)
                        {
                            MaxPushRateOnNeutral = Math.Max(MaxPushRateOnNeutral, (1 - avg));
                        }
                    }

                    textToDisplay += "lul";

                    textToDisplay = textToDisplay.Replace(",lul", "\n"); // lul

                    //variance[uc] /= Metrix[uc].Rates.Count;

                    allTotalVariance += variance[uc];

                    ValidTotalVariance += AllowedVariance[uc].Sum(); 

                    //all 
                    /*textToDisplay += "All - MinFP: " + AllMinPushRateOnPush.ToString("0.00") + ", MaxFP: " + AllMaxPushRateOnNeutral.ToString("0.00")
                                        + ", V[" + uc.ToString() + "]: " + variance[uc].ToString("0.00") + "\n"; 
                    // valid (exclude outliers) 
                    textToDisplay += "Valid - MinFP: " + MinPushRateOnPush.ToString("0.00") + ", MaxFP: " + MaxPushRateOnNeutral.ToString("0.00")
                                       + ", V[" + uc.ToString() + "]: " + AllowedVariance[uc].Sum().ToString("0.00") + " Out: "  + OutliersCount[uc] +  "\n\n";
                   */
                }

                summary = textToDisplay; 

                //float AllValidity = AllMinPushRateOnPush - AllMaxPushRateOnNeutral;
                //float valifValidity = MinPushRateOnPush - MaxPushRateOnNeutral;

                Title.text = "Task 1 is completed! (=^◡^=)"; 
                    
                    
               /* string summary1 = textToDisplay
                        + "All - Average V:" + (allTotalVariance / uniqueList.Count).ToString("0.00") 
                        + ", Gap: " + AllValidity.ToString("0.00") + "\n"
                        + "Valid - Average V:" + (ValidTotalVariance / uniqueList.Count).ToString("0.00")
                        + ", Gap: " + valifValidity.ToString("0.00") + " Total Out: "  + totalOutliers + "\n"; 
                */
                if (OnFinished != null)
                {
                    OnFinished.Invoke();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
				Crashlytics.RecordCustomException ("Profile Exception", "thrown exception", e.StackTrace);
            }
        }//end func 


        string summary = ""; //

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
            /*else if (!filePath.EndsWith(".csv"))
            {
                Debug.Log("illegal");
                return;
            }*/

            if(filePath.Contains("."))
            {
                filePath =  filePath.Substring(0, filePath.LastIndexOf("."));
            }

            //TODO Show exporting window 
            Debug.Log("Exporting Raw");
            FileStream fstreamRaw = File.Create(filePath + " - Raw.csv");
            StreamWriter writerRaw = new StreamWriter(fstreamRaw);

            writerRaw.WriteLine("Test,Target,Command,Time,Power");
            for (int t = 0; t < LastTestingSessions.Length; t++)
            {
                if (States[t].Count > 0)
                {
                    long initTime = States[t][0].Time;
                    for (int s = 0; s < States[t].Count; s++)
                    {
                        writerRaw.WriteLine("{0},{1},{2},{3},{4}", (t + 1), LastTestingSessions[t].ToString(), States[t][s].Command.ToString(), States[t][s].Time - initTime, States[t][s].Power);
                    }
                }
                writerRaw.WriteLine(",,,,");
            }

            writerRaw.Flush(); 
            writerRaw.Close(); 

            Debug.Log("Exported Raw");

            //TODO Show exporting window 
            Debug.Log("Exporting Raw");
            FileStream fstreamSummary = File.Create(filePath + " - Summary.csv");
            StreamWriter writerSummary = new StreamWriter(fstreamSummary);

            writerSummary.WriteLine(summary);

            /*for (int t = 0; t < LastTestingSessions.Length; t++)
            {
                if (States[t].Count > 0)
                {
                    long initTime = States[t][0].Time;
                    for (int s = 0; s < States[t].Count; s++)
                    {
                        writerSummary.WriteLine("{0},{1},{2},{3},{4}", (t + 1), LastTestingSessions[t].ToString(), States[t][s].Command.ToString(), States[t][s].Time - initTime, States[t][s].Power);
                    }
                }
                writerSummary.WriteLine(",,,,");
            }*/

            writerSummary.Flush();
            writerSummary.Close();

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

