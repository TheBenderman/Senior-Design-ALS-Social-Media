using Connectome.Core.Common;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Train : BaseTrainingScreen
{

    public Text resultText;
    public Text[] results;

    private string[] testValues;
    public EmotivCommandType[] ect;
    public Timeline<IEmotivState> timeline;
    private long[][] timelineCheckPoints;
    private Boolean incrementSlider;


    // Use this for initialization
    void Start () {

        slider.maxValue = Start_Screen.sliderLength;

        //Creates the device
        deviceSetup(Start_Screen.deviceValue);
        reader = new BasicEmotivReader(device, false);

        createTestArray();

        //Creates the timeline and timeline checkpoints
        timelineCheckPoints = new long[(ect.Length+2)][];
        timeline = new Timeline<IEmotivState>();

        for (int i = 0; i < ect.Length; i++)
        {
            timelineCheckPoints[i] = new long[2];
        }

        //Starts reading from the device
        reader.OnRead += (e) => timeline.Register(e.State);
        reader.Start();
        incrementSlider = true;

        Activate();
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

                int[][] data = analyzeData();
                setAccuracyResultText(data[0], data[1]);
                collectData = false;

            }  
        }

    }

    //Displays the results to the user in the UI for Accuracy
    private void setAccuracyResultText(int[] commandCounter, int[] commandTotal)
    {
        resultText.text = "Accuracy Results";

        for (int i = 0; i < results.Length; i++)
        {
            if(commandTotal[i] > 0)
            {
                results[i].text = ect[i] + ": " + ((commandCounter[i] * 100) / commandTotal[i]) + "%";
            }
            else
            {
                results[i].text = ect[i] + ": " + "0%";
            }
        }
    }

    //Calculates the number of times the user gave the correct command in each interval
    private int[][] analyzeData()
    {
        IEnumerable<IEmotivState>[] states = new IEnumerable<IEmotivState>[ect.Length];

        int[][] data = new int[2][];
        int[] commandCounter = new int[ect.Length];
        int[] commandTotal = new int[ect.Length];

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
                commandTotal[i] += 1;
            }
        }

        data[0] = commandCounter;
        data[1] = commandTotal;


        return data;
    }

    public override void reset()
    {
        throw new NotImplementedException();
    }
}
