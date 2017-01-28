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
    public class TimelineProcessor<T> : Processor<T, Timeline<IEmotivState>>
    {
        private ITimeline<IEmotivState> Timeline;
        public override IProcessable<Timeline<IEmotivState>>[] Children { get; set; }
        

        public TimelineProcessor(Timeline<IEmotivState> timeline, 
            params IProcessable<Timeline<IEmotivState>>[] children) : base(timeline)
        {
            Children = children;
        }
    }
}
