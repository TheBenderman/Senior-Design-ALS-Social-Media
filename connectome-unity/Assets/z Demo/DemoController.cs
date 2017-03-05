using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using Connectome.Emotiv.Common;

namespace Connectome.Unity.Demo
{
    /// <summary>
    /// Main controler of the scene. Reads and displays readings of connected device. 
    /// Allows alteration of target power and thresh hold. 
    /// There are two types of data display each in a slider: pass rate, and average power. 
    /// </summary>
    public class DemoController : MonoBehaviour
    {
        #region Public attributes 
        public DeviceConnectionSetup deviceHolder;
        /// <summary>
        /// Pass rates display 
        /// </summary>
        public Slider PassRateSlider;
         
        /// <summary>
        /// Average  power 
        /// </summary>
        public Slider AvgPowerSlider;

        /// <summary>
        /// Holds text representing number of passed states. 
        /// </summary>
        public Text PassRateText;
       
        /// <summary>
        /// Holds text representing number of taregted command states. 
        /// </summary>
        public Text AvgPowerText;

        /// <summary>
        /// Contains instence of input of yes and no's 
        /// </summary>
        public InputContainer InputContainer;

        /// <summary>
        /// holds target command 
        /// </summary>
        public EmotivCommandType TargetCommand;

        /// <summary>
        /// Holds pass threshold
        /// </summary>
        [Range(0f, 1f)]
        public float PassThreshhold;

        /// <summary>
        /// holds interval (not really)
        /// </summary>
        [Tooltip("Flicking interval in millisecond")]
        [Range(1, 6000)]
        public int Interval = 1;
        #endregion
        #region Private attributes 
        /// <summary>
        /// temporately holds read values. 
        /// </summary>
        private IEmotivState[] ValuesRead;

        /// <summary>
        /// Used to track next value  to be aded to the list 'ValuesRead'
        /// </summary>
        private int IntervalOffset;

        /// <summary>
        /// holds genrator of yes' and no's for forcing a yes. 
        /// </summary>
        private InputGenerator inputGenerator;

        /// <summary>
        /// holds a reader that reads into a device. 
        /// </summary>
        private IEmotivReader reader;

        /// <summary>
        /// meh
        /// </summary>
        private bool forcedYes = false;
        #endregion
        #region Public methods
        /// <summary>
        /// Intilizes and defines functions for the read to happen and be reported on sliders. 
        /// </summary>
        public void Activate()
        {
            ValuesRead = Enumerable.Repeat(new EmotivState(EmotivCommandType.NULL, 0, 0), Interval).ToArray();

            inputGenerator = InputContainer.Instence;

            IntervalOffset = 0;

            inputGenerator.OnYes = () => forcedYes = true;
            inputGenerator.OnNo = () => forcedYes = false;

            reader = new BasicEmotivReader(deviceHolder.Device);

            //reader.OnStop = () => Debug.Log("Stopped");

            reader.OnRead += (e) =>
            {
                if (e.State.Command == EmotivCommandType.NULL)
                {
                    return;
                }
            //Debug.Log(e.ToString());

            if (forcedYes)
                {
                    ValuesRead[IntervalOffset = (IntervalOffset + 1 % Interval) % Interval] = new EmotivState(TargetCommand, PassThreshhold, 0);
                }
                else
                {
                    ValuesRead[IntervalOffset = (IntervalOffset + 1 % Interval) % Interval] = e.State;
                }
            };

            reader.OnStop += (s) => { Debug.Log("I stoped: " + s); };

           // deviceHolder.Device.OnDisconnectAttempted += (suc,s) => Debug.Log("OnDisconnectAttempted: "+ s);
            deviceHolder.Device.OnDisconnectAttempted += (suc, s) => Debug.Log("OnDisconnectAttempted: " + s);
            reader.ExceptionHandler += (e) => {Debug.Log("Exp"); throw e; };

            reader.Start();
            StartCoroutine(SliderUpdate());
        }
        #endregion
        #region Coroutines
        /// <summary>
        /// Updates both sliders to display visual ananlasys of the read data 
        /// </summary>
        /// <returns></returns>
        private IEnumerator SliderUpdate()
        {
            while (true)
            {
                yield return new WaitForSeconds(.01f);

                int totalState = ValuesRead.Count();

                var targetStates = ValuesRead.Where((e) => e.Command == TargetCommand && e.Power >= PassThreshhold);

                //var maxTime = valuesRead.Max(e => e.time);
                //var minTime = valuesRead.Min(e => e.time);

                //not used
                //int duration = (int) (maxTime - minTime); 

                //pass rate
                PassRateSlider.value = (float)targetStates.Count() / totalState;
                PassRateText.text = PassRateSlider.value.ToString("0.00");

                //average power
                AvgPowerSlider.value = (float)targetStates.Select(e => e.Power).Sum() / totalState;
                AvgPowerText.text = AvgPowerSlider.value.ToString("0.00");

            }
        }
        #endregion
        #region Application events
        /// <summary>
        /// Prevents zoombie thread. 
        /// </summary>
        public void OnApplicationQuit()
        {
            //TODO KLD PLEASE! 
            if (reader != null)
            {
                reader.Stop(); 
            }
        }
        #endregion
        #region UI events 
        /// <summary>
        /// updates threshold values after changing from slider 
        /// </summary>
        /// <param name="s"></param>
        public void ThreshHoldChange(Slider s)
        {
            PassThreshhold = s.value;
        }

        /// <summary>
        /// Updates Traget command after being selected from dropdown
        /// </summary>
        /// <param name="dd"></param>
        public void TargetCommandChange(Dropdown dd)
        {
            TargetCommand = (EmotivCommandType)dd.value;
        }

        /// <summary>
        /// Updates taregt command text based on passed Threshhold value 
        /// </summary>
        /// <param name="t"></param>
        public void TargetCommandTextUpdate(Text t)
        {
            t.text = PassThreshhold.ToString("0.00");
        }

        /// <summary>
        /// sets interval and reset 'ReadValues' 
        /// </summary>
        /// <param name="s"></param>
        public void IntervalChanged(Slider s)
        {
            Interval = (int)s.value;

            ValuesRead = Enumerable.Repeat(new EmotivState(EmotivCommandType.NULL, 0f, 0), Interval).ToArray();
        }

        /// <summary>
        /// updates interval text based on interval value. 
        /// </summary>
        /// <param name="t"></param>
        public void IntervalTextUpdate(Text t)
        {
            t.text = Interval.ToString();
        }
        #endregion 
    }
}
