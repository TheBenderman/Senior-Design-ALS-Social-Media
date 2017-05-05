using Connectome.Emotiv.Interface;
using Connectome.Unity.Template;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// not written to be read 
public class ClickAdjusterTest : MonoBehaviour
{
    [Range(0, 1)]
    public float RefreshRate;
    [Range(0, 1)]
    public float InitClickRate; 

    public float RecordDuration;

    public float BreakDuration;

    public Text DisplayText; 

    public CommandRateEmotivInterpreter RefreshInterpreter;
    public CommandRateEmotivInterpreter ClickInterpreter;
    public IntervalEmotivSampler Sampler; 

    public EmotivReaderPlugin Reader;
    public EmotivRateCalculator Calculator;
    public EmotivCalculatorConfiguration Config; 

    private bool HasStarted; 
    private int Sessions;
    private bool HasReached;
    //private List<float> Rates;
    private List<float> Times; 

    private void Start()
    {
        HasStarted = false;
        HasReached = false;
    } 

    public void StartRecording(int sessions)
    {
        Sessions = sessions; 
        
        RefreshInterpreter.ReachRate = RefreshRate;
        RefreshInterpreter.OnReached.AddListener(RefreshReached);
        RefreshInterpreter.UpdateSliders(); 

        ClickInterpreter.ReachRate = InitClickRate;
        ClickInterpreter.OnReached.AddListener(Reached);
        ClickInterpreter.UpdateSliders();

        Sampler.Interval = (int) (RecordDuration * 1000); 

        TotalWaitedTime = 0; 
        waitedTime = 0;

        //Rates = new List<float>();
        Times = new List<float>();

        DisplayText.text = "rest";
        breakTime = BreakDuration; 
        HasStarted = true;
    }

    float TotalWaitedTime; 
    float waitedTime;

    float breakTime; 

    private void Update()
    {
        float dTime = Time.deltaTime;
        if (!HasStarted || ((breakTime -= Time.deltaTime) > 0))
        {
            HasReached = false;
            Sampler.Clear();
            return; 
        }
        else
        {
             DisplayText.text = "CLICK!!!!";
        }


       
        waitedTime += dTime;
        TotalWaitedTime += dTime;

        if (waitedTime >= RecordDuration || HasReached || TotalWaitedTime >= RecordDuration * 2)
        {
            breakTime = BreakDuration;
            DisplayText.text = "rest ";
            //float rate = Calculator.Calculate(Sampler.GetSample(), Config);
            //Debug.Log(Sessions);
            //Debug.Log("T: " + TotalWaitedTime + "t: " + waitedTime);
            //Rates.Add(rate);
            Times.Add(TotalWaitedTime);

            TotalWaitedTime = 0;
            waitedTime = 0;

            if (--Sessions == 0)
            {
                HasStarted = false;
                DisplayResults();
            }
        }
    }


    public void Reached()
    {
        HasReached = true;
        //Debug.Log("REACHED!");
    }

    public void RefreshReached()
    {
        waitedTime = 0; 
    }

    public void DisplayResults()
    {
        /*
        float averageRate = 0; 

       foreach(float rate in Rates)
        {
            averageRate += rate; 
        }
            averageRate /= Rates.Count;
       */
        float averageTime = 0; 
        foreach (float time in Times)
        {
            averageTime += time; 
        }

        averageTime /= Times.Count;

        float consistency = 0;
        foreach (float time in Times)
        {
            consistency += Math.Abs(time - averageTime);
        }


        float timeRate = averageTime / RecordDuration; // range (0.0 -> 2.0)   

        Debug.Log("tr "+ timeRate);
        
        float UpdatedClickRate = InitClickRate;

        Debug.Log("Avg time" + averageTime);

        if (timeRate > 1)
        {
            UpdatedClickRate += ((1 - timeRate) * (InitClickRate - RefreshRate));
        }
        else
        {
            UpdatedClickRate += (timeRate) * (1 - InitClickRate);
        }
       

        DisplayText.text = "C: " + consistency.ToString("0.00") + "\n Suggested ClickRate: " + UpdatedClickRate.ToString("0.00");


    }
}


