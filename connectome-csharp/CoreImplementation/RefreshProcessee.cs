using Connectome.Core.Int;
using Connectome.Core.Interface;
using Connectome.Core.Template;
using Connectome.Emotiv.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Implementation
{
    /// <summary>
    /// The processee that checks when to refresh the timer
    /// </summary>
    public class RefreshProcessee : Processee<ITimeline<IEmotivState>>
    {
        #region Override Methods
        /// <summary>
        /// Use the timeline to determine if the user has "pushed" enough to indicate they want to try and click
        /// </summary>
        /// <param name="timeline"></param>
        /// <returns></returns>
        protected override bool IsFulfilled(ITimeline<IEmotivState> timeline)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
