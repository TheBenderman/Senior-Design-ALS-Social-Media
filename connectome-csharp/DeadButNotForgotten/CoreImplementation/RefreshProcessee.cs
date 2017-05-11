using Connectome.Core.Interface;
using Connectome.Core.Template;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Connectome.Core.Implementation
{
    /// <summary>
    /// The processee that checks when to refresh the timer
    /// </summary>
    public class RefreshProcessee : Processee<ITimeline<IEmotivState>>
    {
        public long RefreshInterval;

        public EmotivCommandType TargetCommand;

        public float ThreashHold;

        #region Override Methods
        /// <summary>
        /// Use the timeline to determine if the user has "pushed" enough to indicate they want to try and click
        /// </summary>
        /// <param name="timeline"></param>
        /// <returns></returns>
        protected override bool IsFulfilled(ITimeline<IEmotivState> timeline)
        {
            var lastRecorded = timeline.Latest();

            if (lastRecorded == null)
                return false;

            IEnumerable<IEmotivState> dataSet = timeline[lastRecorded.Time - RefreshInterval, lastRecorded.Time].ToArray();

            if (dataSet == null ||  dataSet.Count() == 0)
                return false;

            float targetRate = ((float)dataSet.Where(s => s.Command == TargetCommand).Count()) / dataSet.Count(); 

            if (targetRate >= ThreashHold)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
