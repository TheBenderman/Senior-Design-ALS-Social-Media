using Connectome.Emotiv.Interface;
using Connectome.Unity.Template;
using System.Collections;
using System.Collections.Generic;
using Connectome.Unity.EmotivStateExtensions;
using Connectome.Unity.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEngine;
#endif
using System.IO;

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
    public LabeledHighligter LabeledFlashingHighligter; 
    public EmotivReaderPlugin Reader;

    private List<string> StatesCSV; 

    private bool IsRecording;

    [Header("Events")]
    public UnityEvent OnFinish;

    private void Start()
    {
        IsRecording = false;
    }

    public void StartRecording(int time)
    {
        StatesCSV = new List<string>();
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
    }


    private long initTime; 
    /// <summary>
    /// Auto generation for the win. 
    /// </summary>
    /// <param name="state"></param>
    private void ReaderOnRead(IEmotivState state)
    {
        if (IsRecording)
        {
            if (initTime == -1)
            {
                initTime = state.Time; 
            }
            StatesCSV.Add(string.Format("{0},{1},{2},{3}", LabeledFlashingHighligter.HighlightedName, state.Command, state.Time - initTime, state.Power));
        }
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
        string previousLine = StatesCSV[0].Substring(0, StatesCSV[0].IndexOf(','));

        ///csv header
        writer.WriteLine("Location,Command,Time,Power");

        foreach (var line in StatesCSV)
        {
            ///add empty line for each set a states records for each highlighed 
            if(previousLine != line.Substring(0, line.IndexOf(',')))
            {
                previousLine = line.Substring(0, line.IndexOf(','));
                writer.WriteLine(",,,,");     
            }
            writer.WriteLine(line); 
        }

        writer.Flush();
        writer.Close();

        Debug.Log("Exported");
    }
}
