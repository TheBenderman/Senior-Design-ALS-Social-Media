using Connectome.Core.Interface;
using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public class TimelineDisplayProcesee : MonoBehaviour , IProcessable<ITimeline<IEmotivState>>
{
    public Slider slider; 


    public bool Process(ITimeline<IEmotivState> timeline)
    {
        IEmotivState lastRecorded = timeline.Latest();

        if (lastRecorded == null)
            return false;

        IEnumerable<IEmotivState> dataSet = timeline[lastRecorded.Time - 200, lastRecorded.Time].ToArray();

        if (dataSet == null || dataSet.Count() == 0)
            return false;

        Debug.Log(lastRecorded.Time);

        //Debug.Log(dataSet.Count());


        float targetRate = ((float)dataSet.Where(s => s.Command == Connectome.Emotiv.Enum.EmotivCommandType.PUSH).Count()) / dataSet.Count();


       
        slider.value = targetRate;

        //Debug.Log(targetRate);


        return false; 
    }

    public float rate; 
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rate = slider.value; 

    }
}
