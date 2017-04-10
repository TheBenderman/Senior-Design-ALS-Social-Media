using System.Collections;
using System.Collections.Generic;
using Connectome.Core.Common;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using UnityEngine;
using UnityEngine.UI;
using System;
using Connectome.Calibration.API;
using Connectome.Calibration.API.Interfaces;
using Connectome.Calibration.API.Loggers;

public class Streak : BaseTrainingScreen
{

    public Text resultText;
    public Text highscoreText;
    public Text resultTitleText;
    public Text averageStreakText;

    private int streakCounter;
    private long highscore;
    private long startTime;
    private long endTime;
    private Boolean run;
    private Boolean secondRun = false;

    private ArrayList intervals;

    // Use this for initialization
    void Start () {

        //Creates the device
        deviceSetup(Start_Screen.deviceValue);
        setup();

    }

    private void OnEnable()
    {
        if (secondRun)
        {
            deviceSetup(Start_Screen.deviceValue);
            setup();
        }
    }

    private void OnApplicationQuit()
    {
        reader.Stop();
    }
	
	// Update is called once per frame
	void Update () {

        if (run)
        {
            incrementSlider();
            resultText.text = resultsInSeconds(getCurrentTime());
            updateHighscore();

            if (getCurrentTime() > 1)
            {
                highlightButton();
            }
            else
            {
                dehighlightButton();
            }
        }
    }

    void counter(IEmotivState e)
    {

        if (e.Command == EmotivCommandType.PUSH)
        {
            if (streakCounter == 0)
            {
                startTime = e.Time;
                endTime = startTime;
                streakCounter++;
            }
            else
            {
                endTime = e.Time;
            }

        }
        else
        {

            if (getCurrentTime() > 1)
            {
                intervals.Add(getCurrentTime());
            }
            streakCounter = 0;
            startTime = 0;
            endTime = 0;
        }
    }

    String resultsInSeconds(long time)
    {
        long seconds = time / 1000;
        long tenths = (time % 1000) / 100;
        return seconds.ToString() + "." + tenths.ToString();
    }

    void incrementSlider()
    {
        if (slider.value != slider.maxValue)
        {
            slider.value += Time.deltaTime;
        }
        else
        {
            reader.Stop();
            intervals.Add(getCurrentTime());
            displayResults();
            run = false;
            dehighlightButton();
        }
    }

    void updateHighscore()
    {
        if(Convert.ToInt64(highscore) < getCurrentTime())
        {
            highscore = getCurrentTime();
            highscoreText.text = resultsInSeconds(highscore);
        }
    }

    long getCurrentTime()
    {
        return (endTime - startTime);
    }

    void displayResults()
    {
        LoggerInterface logger = new CsvLogger("Streak.csv");
        logger.add(Start_Screen.profile);
        resultTitleText.text = "Results";
        averageStreakText.text = "Average: " + resultsInSeconds(getAverageStreak());
        logger.add(resultsInSeconds(highscore));
        logger.add(resultsInSeconds(getAverageStreak()));
        logger.write();
    }

    long getAverageStreak()
    {
        long totals = 0;

        foreach(long x in intervals)
        {
            totals += x;
        }

        return totals / Convert.ToInt64(intervals.Count);
    }

    void highlightButton()
    {
        updateButtonColor(Color.cyan);
    }

    void dehighlightButton()
    {
        updateButtonColor(Color.white);
    }


    public override void reset()
    {
        dehighlightButton();
        resultTitleText.text = "";
        averageStreakText.text = "";
        highscoreText.text = "0";
        slider.value = 0;
        reader = null;
        device = null;
        intervals = null;
        secondRun = true;
        mainMenu.SetActive(true);
        currentPanel.SetActive(false);
    }

    void setup()
    {
        slider.maxValue = Start_Screen.sliderLength;
        streakCounter = 0;
        highscore = 0;
        run = true;
        intervals = new ArrayList();

        reader = new BasicEmotivReader(device, false);

        reader.OnRead += (e) => counter(e.State);
        reader.Start();
    }

}

