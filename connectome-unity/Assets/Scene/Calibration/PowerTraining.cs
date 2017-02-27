using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using UnityEngine.UI;
using System;

public class PowerTraining : BaseTrainingScreen {

    public float[] ect;
    private int currentPoint;

    private float currentPower;
    private float lastPower;
    private float highscore;
    private Boolean passedTest;
    private Boolean started;
    private Boolean secondRun = false;
    private Boolean complete;

    public Text maxPower;

    // Use this for initialization
    void Start () {

        deviceSetup(Start_Screen.deviceValue);
        setup();

    }

    private void OnEnable()
    {
        if(secondRun)
        {
            setup();
        }
    }

    // Update is called once per frame
    void Update () {

        slider.value += Time.deltaTime;
        colorRange(lastPower);

        updateHighcore();


        if (passedTest)
        {
            setButtonText((ect[currentPoint] * 100).ToString() +"%");
            currentPower = ect[currentPoint];
            passedTest = false;
            slider.value = 0;
            currentPoint++;
        }

        if (complete)
        {
            if(slider.value == slider.maxValue)
            {
                reset();
            }
        }

        if (currentPoint >= ect.Length || (slider.value == slider.maxValue && started))
        {
            reader.Stop();
            passedTest = false;
            setButtonText("Complete!");
            lastPower = 0;
            slider.value = 0;
            slider.maxValue = 5;
            complete = true;

        }

    }

    void updateHighcore()
    {
        maxPower.text = "Max: " + Math.Round(highscore*100);
    }

    void activate()
    {
        StartCoroutine(phases());
    }

    IEnumerator phases()
    {

        yield return new WaitForSeconds(slider.maxValue);
        passedTest = true;
        reader.Start();
        started = true;

    }

    void powerChecker(IEmotivState e)
    {
        if(e.Power >= currentPower)
        {
            passedTest = true;
        }
        lastPower = e.Power;

        if(e.Power > highscore)
        {
            highscore = e.Power;
        }
    }

    void colorRange(float power)
    {
        if (power < .01)
        {
            updateButtonColor(Color.white);
        }
        else
        {
            updateButtonColor(new Color(power * 2, 1 - ((power - .5f) * 2), 0, 1));
        }
    }


    public override void reset()
    {
        currentPoint = 0;
        currentPower = 0;
        lastPower = 0;
        passedTest = false;
        updateButtonColor(Color.white);
        setButtonText("Starting");
        slider.value = 0;
        secondRun = true;
        mainMenu.SetActive(true);
        currentPanel.SetActive(false);
    }

    void setup()
    {
        currentPoint = 0;
        highscore = 0;
        passedTest = false;
        started = false;
        complete = false;

        slider.maxValue = Start_Screen.sliderLength;

        reader = new BasicEmotivReader(device, false);

        reader.OnRead += (e) => powerChecker(e.State);

        activate();
    }
}
