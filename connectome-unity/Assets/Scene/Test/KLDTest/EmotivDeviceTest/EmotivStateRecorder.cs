using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Template;
using Connectome.Unity.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EmotivStateRecorder : MonoBehaviour {

    public SelectionManager SelectionManager;
    public CommandRateEmotivInterpreter ClickInterpreter;
    public CommandRateEmotivInterpreter RefreshInterpreter;

    public FlashingHighlighter FlasgingHighlighter;
    public LabeledHighligter LabeledHighligter;

    public EmotivReaderPlugin Reader; 


    public List<ControledEmotivState> States;

    private void Start()
    {
        States = new List<ControledEmotivState>();
    }

    public void StartRecording()
    {
        Reader.OnRead -= OnRead;
        Reader.OnRead += OnRead; 
    }

    public void StopAndExport()
    {
        Reader.OnRead -= OnRead;

        string filePath = "data.csv";
        #if UNITY_EDITOR
        filePath = UnityEditor.EditorUtility.SaveFilePanel("File Destination", "", "data", "csv");
        #endif

        if (string.IsNullOrEmpty(filePath))
        {
            Debug.Log("empty path - bye");
            return;
        }
        else if (!filePath.EndsWith(".csv"))
        {
            Debug.Log("illegal name (doesn't end with .csv)");
            return;
        }

        Debug.Log("Exporting");
        FileStream fstream = File.Create(filePath);
        StreamWriter writer = new StreamWriter(fstream);

        writer.WriteLine("Test,Target,Command,Time,Power");

        long initTime = States[0].Time;

        ///Header
        writer.WriteLine("Time,Command,Power,Location,ClickRate,RefreshRate,Duration,CalcRate,Status");
        foreach (ControledEmotivState c in States)
        {
            writer.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7},{8}", 
                c.Time - initTime,
                c.Command.ToString(),
                c.Power.ToString("0.00"),
                c.Location,
                c.ClickRate.ToString("0.00"),
                c.RefreshRate.ToString("0.00"),
                c.Duration.ToString("0.00"),
                c.CalcRate.ToString("0.00"),
                c.ClickStatus
                );
        }

        writer.Flush();
        writer.Close();

        Debug.Log("Exported");
    }

    public void OnRead(IEmotivState s)
    {
        States.Add(new ControledEmotivState()
        {
            Command = s.Command,
            Power = s.Power,
            Time = s.Time,
            Location = LabeledHighligter.HighlightedName,
            RefreshRate = RefreshInterpreter.ReachRate,
            ClickRate = ClickInterpreter.ReachRate,
            CalcRate = ClickInterpreter.ActivitySlider.value,
            ClickStatus = DidClick ? "Clicked" : "",
            Duration = SelectionManager.WaitInterval
        }
        );

        DidClick = false; 
    }

    private bool DidClick; 

    public void Clicked()
    {
        DidClick = true; 
    }
}

public class ControledEmotivState
{
    public EmotivCommandType Command;

    public float Power;

    public long Time;

    public string Location;

    public float ClickRate;

    public float RefreshRate;

    public float CalcRate;

    public string ClickStatus;

    public float Duration; 

    /*public override string ToString() good idea, but it's more clear knowning the header. 
    {
        return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",Time, ); 
    }*/
}
