using Connectome.Core.Common;
using Connectome.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Common
{
    public class TimelineCurser<L, T> where T : class, ITimeline<L>  where L : ITime
    {
        private long Current;
        private long Duration;
        private long Shift;

        private T Timeline; 

        public TimelineCurser(long Duration, long ShiftSpeed)
        {
            this.Duration = Duration;
            Shift = ShiftSpeed; 
        }

        public TimelineCurser(long Duration, long ShiftSpeed, T Timeline) : this(Duration, ShiftSpeed)
        {
            this.Timeline = Timeline; 
        }

        public long Seek(long to)
        {
            Current = to; //TODO can validate to closest 

            if (Timeline == null)
                return -1; 

            return Current; 
        }

        public long SeekStart()
        {
            return Seek(0); 
        }

        public long SeekEnd()
        {
            return Seek(long.MaxValue);
        }

        public IEnumerable<L> ShiftGetInterval(T tl = null)
        {
            tl = tl ?? this.Timeline; 

            var set = tl[Current, Current + Duration];

            Current += Shift; //TODO Validat current 

            return set; 
        }

        public void SetDuration(long d)
        {
            Duration = d; 
        }

        public void SetShift(long s)
        {
            Shift = s; 
        }
    }
}
