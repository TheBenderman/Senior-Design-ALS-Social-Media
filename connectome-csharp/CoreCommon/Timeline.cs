using Connectome.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Int
{
    public class Timeline<T> : ITimeline<T> where T : ITime
    {
        private IDictionary<long, T> Dict;
       
        public Timeline()
        {
            Dict = new Dictionary<long, T>();
        }

        public T this[long time]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public T this[long time, long tme]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public long Duration
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public T Earliest()
        {
            throw new NotImplementedException();
        }

        public T Get(int i)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetInterval(int begin, int end)
        {
            throw new NotImplementedException();
        }

        public T Latest()
        {
            throw new NotImplementedException();
        }

        public void NormalizeTime()
        {
            throw new NotImplementedException();
        }

        public void Register(IEnumerable<T> t)
        {
            throw new NotImplementedException();
        }

        public void Register(T t)
        {
            throw new NotImplementedException();
        }
    }
}
