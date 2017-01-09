using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using EmotivWrapperInterface;
using EmotivWrapper.Core;
using EmotivImpl;
using EmotivWrapper;

public class InputController : MonoBehaviour
{
    public DeviceConnectionSetup deviceHolder;

    public Slider slider;
    public Slider slider2;

    public Text sliderValueText;
    public Text slider2ValueText;

    public InputContainer inputContainer;

    public EmotivStateType targetState;

    [Range(0f, 1f)]
    public float targetPower;

    [Tooltip("Flicking interval in millisecond")]
    [Range(1, 6000)]
    public int interval = 1;

    private IEmotivState[] valuesRead;
    private int intervalOffset;
    private InputGenerator inputGenerator;

    private IEmotivReader reader;

    private bool forcedYes = false;

    public void Activate()
    {
        valuesRead = Enumerable.Repeat(new EmotivState() { power=0f, time=0, command=EmotivStateType.NULL}, interval).ToArray();
       
        inputGenerator = inputContainer.inputGenerator;

        intervalOffset = 0;

        inputGenerator.OnYes = () => forcedYes = true;
        inputGenerator.OnNo = () => forcedYes = false;

        reader = new EmotivReader(deviceHolder.device);

        //reader.OnStop = () => Debug.Log("Stopped");

        reader.OnRead = (e) =>
        {
            if (e.command == EmotivStateType.NULL)
            {
                return; 
            }
            //Debug.Log(e.ToString());

            if (forcedYes)
            {
                valuesRead[intervalOffset = (intervalOffset + 1 % interval) % interval] = new EmotivState() { power = 1f, time = 0, command = targetState }; 
            }
            else
            {
                valuesRead[intervalOffset = (intervalOffset + 1 % interval) % interval] = e;
            }
        };

        reader.Start();
        StartCoroutine(SliderUpdate());
    }

    private IEnumerator SliderUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(.01f);

            int totalState = valuesRead.Count();

            var targetStates = valuesRead.Where((e) => e.command == targetState && e.power >= targetPower);

            //var maxTime = valuesRead.Max(e => e.time);
            //var minTime = valuesRead.Min(e => e.time);

            //not used
            //int duration = (int) (maxTime - minTime); 

            //pass rate
            slider.value =(float) targetStates.Count() / totalState;
            sliderValueText.text = slider.value.ToString("0.00");

            //average power
            slider2.value = (float)targetStates.Select(e => e.power).Sum() / totalState; 
            slider2ValueText.text = slider2.value.ToString("0.00");

        }
    }

    public void OnApplicationQuit()
    {
        //TODO KLD PLEASE! 
        reader.isRunning = false; 
    }

    public void ThreshHoldChange(Slider s)
    {
        targetPower = s.value; 
    }

    public void TargetCommandChange(Dropdown dd)
    {
        targetState = (EmotivStateType)dd.value;
    }

    public void TargetCommandTextUpdate(Text t)
    {
        t.text = targetPower.ToString("0.00");
    }

    public void IntervalChanged(Slider s)
    {
        interval = (int) s.value;

        valuesRead = Enumerable.Repeat(new EmotivState() { power = 0f, time = 0, command = EmotivStateType.NULL }, interval).ToArray();
    }

    public void IntervalTextUpdate(Text t)
    {
        t.text = interval.ToString(); 
    }

}
