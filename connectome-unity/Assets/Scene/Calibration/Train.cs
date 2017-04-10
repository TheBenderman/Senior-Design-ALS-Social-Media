using Connectome.Core.Common;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Connectome.Calibration.API.Interfaces;
using Connectome.Calibration.API.Loggers;

public class Train : BaseTrainingScreen
{

    public Text resultText;
    public Text[] results;

    private string[] testValues;
    public EmotivCommandType[] ect;
    public Timeline<IEmotivState> timeline;
    private long[][] timelineCheckPoints;
    private Boolean incrementSlider;
    private Boolean secondTime = false;
    private Boolean complete;
    private LoggerInterface accuracyLogger;
    private LoggerInterface accuracyFalsePositiveLogger;


    // Use this for initialization
    void Start () {
        deviceSetup(Start_Screen.deviceValue);
        setup();
       
    }

    private void OnEnable()
    {
        if (secondTime)
        {
            deviceSetup(Start_Screen.deviceValue);
            setup();
        }
    }

    //Randomizes the array of push and neutral order
    void createTestArray()
    {
        System.Random rnd = new System.Random();
        ect = ect.OrderBy(x => rnd.Next()).ToArray();
       
    }	

	// Update is called once per frame
	void Update () {
        //Increments the slider every time the frame updates
        if(incrementSlider)
        {
            slider.value += Time.deltaTime;
        }
        
        //Changes the button's color if the user is pushing
        if(timeline.Latest().Command.Equals(EmotivCommandType.PUSH))
        {
            updateButtonColor(Color.cyan);
        }
        else
        {
            updateButtonColor(Color.white);
        }


    }

    private void OnApplicationQuit()
    {
        reader.Stop();
    }

    void Activate()
    {
        StartCoroutine(Phases());
    }

    //Run the calibration test
    IEnumerator Phases()
    {
        int pos = 0;
        Boolean collectData = true;

        while (collectData)
        {
            
            yield return new WaitForSeconds(slider.maxValue);

            if (pos < ect.Length)
            {
                if (pos != 0)
                {
                    timelineCheckPoints[pos-1][1] = timeline.Latest().Time;
                }
                setButtonText(ect[pos].ToString());
                incrementSlider = false;
                slider.value = slider.minValue;
                yield return new WaitForSeconds(1);
                timelineCheckPoints[pos][0] = timeline.Latest().Time;
                incrementSlider = true;
                
                pos++;
            }
            else
            {
                timelineCheckPoints[pos-1][1] = timeline.Latest().Time;

                setButtonText("Complete!");
                reader.Stop();
                reader = null;

                int[][] data = analyzeData();
                setAccuracyResultText(data[0], data[1]);
                collectData = false;
                complete = true;

            }  
        }

    }

    //Displays the results to the user in the UI for Accuracy
    private void setAccuracyResultText(int[] commandCounter, int[] commandTotal)
    {
        int percent;
        int[] neutralHolder = new int[results.Length/2];
        int counter = 0;
        resultText.text = "Accuracy Results";

        for (int i = 0; i < results.Length; i++)
        {
            if(commandTotal[i] > 0)
            {
                percent = ((commandCounter[i] * 100) / commandTotal[i]);

                results[i].text = ect[i] + ": " + percent + "%";

                if(ect[i] == EmotivCommandType.NEUTRAL)
                {
                    neutralHolder[counter] = percent;
                    counter++;
                }
                else
                {
                    accuracyLogger.add(percent.ToString());
                }
                
            }
            else
            {
                results[i].text = ect[i] + ": " + "0%";
            }
        }

        foreach(int per in neutralHolder)
        {
            accuracyLogger.add(per.ToString());
        }

        accuracyLogger.write();
        //accuracyFalsePositiveLogger.write();
    }

    //Calculates the number of times the user gave the correct command in each interval
    private int[][] analyzeData()
    {
        IEnumerable<IEmotivState>[] states = new IEnumerable<IEmotivState>[ect.Length];

        int[][] data = new int[2][];
        int[] commandCounter = new int[ect.Length];
        int[] commandTotal = new int[ect.Length];
        float totalFalsePositivePower = 0;
        float falsePostiveCounter = 0;

        for (int i = 0; i < ect.Length; i++)
        {
            states[i] = timeline.GetInterval(timelineCheckPoints[i][0], timelineCheckPoints[i][1]).ToArray();

            commandCounter[i] = 0;
            foreach (IEmotivState x in states[i])
            {
                if (x.Command == ect[i])
                {
                    commandCounter[i] += 1;
                }
                else
                {
                    if(x.Command == EmotivCommandType.NEUTRAL)
                    {
                        if (x.Power > 0)
                        {
                            totalFalsePositivePower += (x.Power * 100);
                            falsePostiveCounter++;
                        }
                    }
                }
                commandTotal[i] += 1;
            }
        }
        accuracyFalsePositiveLogger.add(((falsePostiveCounter)).ToString());
        data[0] = commandCounter;
        data[1] = commandTotal;

        return data;
    }

    public override void reset()
    {

        resultText.text = "";

        for (int i = 0; i < results.Length; i++)
        {
                results[i].text = "";
            
        }

        slider.value = 0;
        secondTime = true;
        device = null;
        setButtonText("Start!");
        mainMenu.SetActive(true);
        currentPanel.SetActive(false);
    }

    void setup()
    {
        slider.maxValue = Start_Screen.sliderLength;
        complete = false;

        reader = new BasicEmotivReader(device, false);

        createTestArray();

        //Creates the timeline and timeline checkpoints
        timelineCheckPoints = new long[(ect.Length + 2)][];
        timeline = new Timeline<IEmotivState>();

        for (int i = 0; i < ect.Length; i++)
        {
            timelineCheckPoints[i] = new long[2];
        }

        //Starts reading from the device
        reader.OnRead += (e) => timeline.Register(e.State);
        reader.Start();
        incrementSlider = true;
        accuracyLogger = new CsvLogger("Accuracy.csv");
        accuracyFalsePositiveLogger = new CsvLogger("AccuracyFalsePositiveLogger.csv");

        Activate();
    }
}
