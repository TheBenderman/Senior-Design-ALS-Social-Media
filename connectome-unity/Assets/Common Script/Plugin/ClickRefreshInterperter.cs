using System;
using System.Collections;
using System.Collections.Generic;
using Connectome.Emotiv.Interface;
using UnityEngine;
using Connectome.Core.Common;
using UnityEngine.Events;
using Connectome.Emotiv.Enum;
using System.Linq;
using UnityEngine.UI;
using Connectome.Unity.Template;

namespace Connectome.Unity.Plugin
{
    /// <summary>
    /// An interpreter that triggers refresh and click events based on a float interperation of a state interval.
    /// </summary>
    public abstract class ClickRefreshInterperter : EmotivInterpreterPlugin
    {
        #region Private Attributes
        private ITimeline<IEmotivState> Timeline;
        #endregion
        #region Public Inspector Attributes
        [Header("Settings")]
        public long Interval;
        public EmotivCommandType TargetCommand;

        [Header("Threshholds")]
        public float RefreshThreshhold;
        public float ClickThreshhold;

        [Header("Events")]
        public UnityEvent OnRefresh;
        public UnityEvent OnClick;

        /// <summary>
        /// If set, interpreted values is set to the slider. 
        /// </summary>
        [Header("Optional")]
        public Slider Slider;

        #endregion
        #region EmotivInterpreterPlugin Overrides
        /// <summary>
        /// Calculate and trigger events based on calculation. 
        /// <see cref="GetRate(IEnumerable{IEmotivState})"/>
        /// </summary>
        public override void Interpret()
        {
            IEmotivState lastRecorded = Timeline.Latest();

            if (lastRecorded == null)
                return;

            IEnumerable<IEmotivState> dataSet = Timeline[lastRecorded.Time - Interval, lastRecorded.Time].ToArray();

            if (dataSet == null || dataSet.Count() == 0)
                return;

            float targetRate = GetRate(dataSet);

            //Update slider
            if (Slider != null)
            {
                Slider.value = targetRate;
            }

            //Refresh
            if (targetRate >= RefreshThreshhold)
            {
                if (OnRefresh != null)
                    OnRefresh.Invoke();
            }

            //Click
			if (targetRate >= ClickThreshhold)
            {
				Timeline = new Timeline<IEmotivState>();
                if (OnClick != null)
                    OnClick.Invoke();
            }
        }

        /// <summary>
        /// Creates and attaches Timemline to reader 
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="Reader"></param>
        public override void Setup(IEmotivDevice Device, IEmotivReader Reader)
        {
            Timeline = new Timeline<IEmotivState>();
            Reader.OnRead += e => Timeline.Register(e.State);
        }
        #endregion
        #region Abstract Methods
        /// <summary>
        /// Calculate rate to trigger events 
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        protected abstract float GetRate(IEnumerable<IEmotivState> states);
        #endregion
    }
}