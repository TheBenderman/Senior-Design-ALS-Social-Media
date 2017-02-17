using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using Connectome.Unity.Common;
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

    public Text maxPower;

    // Use this for initialization
    void Start () {
        currentPoint = 0;
        highscore = 0;
        passedTest = false;
        started = false;

        slider.maxValue = Start_Screen.sliderLength;

        deviceSetup(Start_Screen.deviceValue);

        reader = new BasicEmotivReader(device, false);

        reader.OnRead += (e) => powerChecker(e.State);

        activate();
    }

    private void OnEnable()
    {

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

        if (currentPoint >= ect.Length || (slider.value == slider.maxValue && started))
        {
            reader.Stop();
            passedTest = false;
            setButtonText("Complete!");


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
    }
}
