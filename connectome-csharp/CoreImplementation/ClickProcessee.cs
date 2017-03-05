using Connectome.Core.Interface;
using Connectome.Core.Template;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Implementation
{
    /// <summary>
    /// Processee to check when the user has pushed enough to trigger a click
    /// </summary>
    public class ClickProcessee : RefreshProcessee
    {
        #region Override Methods
        /// <summary>
        /// Use the timeline to determine if the user has "pushed" enough to indicate they want to try and click
        /// </summary>
        /// <param name="timeline"></param>
        /// <returns></returns>
        //protected override bool IsFulfilled(ITimeline<IEmotivState> timeline)
        //{
        //hehe    
        //}
        #endregion
    }
}
