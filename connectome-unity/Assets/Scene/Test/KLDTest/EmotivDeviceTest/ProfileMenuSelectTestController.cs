using Connectome.Emotiv.Interface;
using Connectome.Unity.Template;
using System.Collections;
using System.Collections.Generic;
using Connectome.Unity.UI;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEngine;
#endif
using System.IO;

/// <summary>
/// Please don't read. Not pleasent 
/// </summary>
public class ProfileMenuSelectTestController : MonoBehaviour
{
    [Header("README")]
    [Header("To initiate test, invoke StartRecording(int).")]
    [Header("SelectionManager can run independetly unless ForceOverride.")]
    [Header("Overrides")]
    public bool ForceOverride;

    [Tooltip("in seconds")]
    public int Duration;
    public bool EnableSelectionManager;

    [Header("Required Refrences")]
    public SelectionManager SelectionManager;
    public Text DisplayText; 
    public LabeledHighligter LabeledFlashingHighligter; 
    public EmotivReaderPlugin Reader;

    //private List<string> StatesCSV;

    private List<TaggedEmotivState> States; 

    public EmotivRateCalculator Calculator;
    public EmotivCalculatorConfiguration Config;

    private bool IsRecording;

    [Header("Events")]
    public UnityEvent OnFinish;

    private void Start()
    {
        IsRecording = false;
    }

    public void StartRecording(int time)
    {
        //StatesCSV = new List<string>();
        States = new List<TaggedEmotivState>(); 
        initTime = -1;
        
        if (ForceOverride)
        {
            time = Duration;

            if (EnableSelectionManager)
            {
                SelectionManager.Activate();
            }
            else
            {
                SelectionManager.Deactivate();
            }
        }
        StartCoroutine(RecordingFor(time)); 
    }

    private IEnumerator RecordingFor(int seconds)
    {
        Reader.OnRead -= ReaderOnRead;
        Reader.OnRead += ReaderOnRead;

        IsRecording = true;
        yield return new WaitForSeconds(seconds);
        IsRecording = false;

        OnFinish.Invoke();


        //Group data based on Tag 
        List<List<IEmotivState>> list = new List<List<IEmotivState>>();

        Dictionary<string, List<float>> TargettingState = new Dictionary<string, List<float>>();

        List<float> TargetRates = new List<float>();
        float MinTargetRate = 1.01f; 

        List<float> NonTargetRates = new List<float>();
        float MaxNonTargetRate = -0.01f; 

        string previous = null;

        foreach (var state in States)
        {
            if (previous != state.Tag)
            {
                if (previous != null) ///skip first iteration 
                {
                    float rate = Calculator.Calculate(list.Last(), Config);
                    string target = "NonTarget";

                    if (previous == "Option 2") //eh  TODO -What is this? SE101????
                    {
                        target = "Target";
                        TargetRates.Add(rate);
                        if (MinTargetRate > rate)
                        {
                            MinTargetRate = rate;
                        }
                    }
                    else
                    {

                        NonTargetRates.Add(rate);
                        if (MaxNonTargetRate < rate)
                        {
                            MaxNonTargetRate = rate;
                        }
                    }

                    if (!TargettingState.ContainsKey(target))
                    {
                        TargettingState.Add(target, new List<float>());
                    }
                    TargettingState[target].Add(rate);
                }

                previous = state.Tag;
                list.Add(new List<IEmotivState>());
            }

            list.Last().Add(state.EmotivState);
        }
            
        
        //Presenting data? 

        string[] unique = { "Target", "NonTarget" };

        Dictionary<string, float> difference = new Dictionary<string, float>();
        Dictionary<string, float> average = new Dictionary<string, float>();

        string diffBuild = "";
        float totalD = 0;

        foreach (string uc in unique)
        {
            difference.Add(uc, 0);

            float averageAvg = 0;

            foreach (float avg in TargettingState[uc])
            {
                averageAvg += avg;
            }


            averageAvg /= TargettingState[uc].Count;

            average.Add(uc, averageAvg); 

            foreach (float avg in TargettingState[uc])
            {
                difference[uc] += Math.Abs(averageAvg - avg);
            }

            difference[uc] /= TargettingState[uc].Count;

            totalD = difference[uc];

            diffBuild += "\n C[" + uc.ToString() + "]: " + difference[uc].ToString("0.00") + "\n";
        }

        //fk this sucks
        float estRefresh = average["NonTarget"] + ((average["Target"] - average["NonTarget"]) * .25f);

        DisplayText.text = "Avg Target: " + average["Target"].ToString("0.00") + "\nAvg NonTarget: " +  average["NonTarget"].ToString("0.00") +  diffBuild + " Estemated RefreshRate" + estRefresh.ToString("0.00"); 



        //calculate average for each session. 

    }

    private long initTime; 
    /// <summary>
    /// Auto generation for the win. 
    /// </summary>
    /// <param name="state"></param>
    private void ReaderOnRead(IEmotivState state)
    {
        if (!IsRecording)
        {
            return;
        }

        if (initTime == -1)
        {
            initTime = state.Time; 
        }

        States.Add(new TaggedEmotivState() { Tag = LabeledFlashingHighligter.HighlightedName, EmotivState = state });

        //StatesCSV.Add(string.Format("{0},{1},{2},{3}", LabeledFlashingHighligter.HighlightedName, state.Command, state.Time - initTime, state.Power));
    }

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

        Debug.Log("Exporting");
        FileStream fstream = File.Create(filePath);
        StreamWriter writer = new StreamWriter(fstream);
        string previousLine = States[0].Tag; 

        ///csv header
        writer.WriteLine("Location,Command,Time,Power");

        foreach (var state in States)
        {
            ///add empty line for each set a states records for each highlighed 
            if(previousLine != state.Tag)
            {
                previousLine = state.Tag; 
                writer.WriteLine(",,,,");     
            }

            writer.WriteLine(string.Format("{0},{1},{2},{3}", state.Tag, state.EmotivState.Command, state.EmotivState.Time - initTime, state.EmotivState.Power)); 
        }

        writer.Flush();
        writer.Close();

        Debug.Log("Exported");
    }
}

class TaggedEmotivState
{
    public string Tag;
    public IEmotivState EmotivState; 
}

