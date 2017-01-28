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
    /// Processee to check when the user has pushed enough to trigger a click
    /// </summary>
    public class ClickProcessee : Processee<ITimeline<IEmotivState>>
    {
        #region Override Methods
        /// <summary>
        /// Use the timeline to determine if the user has enough pushes registered to trigger a click.
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
