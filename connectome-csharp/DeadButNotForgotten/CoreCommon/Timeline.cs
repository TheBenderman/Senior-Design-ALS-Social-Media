using Connectome.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Connectome.Core.Common
{
    public class Timeline<T> : ITimeline<T> where T : class, ITime
    {
        #region Private Fields

        /// <summary>
        /// Elements
        /// </summary>
        private T[] E;

        /// <summary>
        /// Pointer
        /// </summary>
        private int P;

        private object Locker; 

        #endregion
        #region Constructors
        public Timeline()
        {
            Locker = new object(); 
            E = new T[0];
            P = -1; 
        }

        public Timeline(ITimeline<T> tl) : this()
        {
            //TODO o somethng
        }
        #endregion
        #region ITimeline Indexer
        public T this[long time]
        {
            get
            {
                return Get(time); 
            }
        }

        public IEnumerable<T> this[long from, long to]
        {
            get
            {
                return GetInterval(from, to); 
            }
        }
        #endregion
        #region ITimeline Properties
        public long Duration
        {
            get
            {
                return E.Length; 
            }
        }
        #endregion
        #region ITimeline Methods
        public T Earliest()
        {
            if(E.Length == 0)
            {
                return null;
            }
            T min = E[0];
            for (int i = 0; i < P; i++)
            {
                if (E[i].Time < min.Time)
                    min = E[i];
            }
            return min;
        }

        public T Latest()
        {
            if (E.Length == 0)
            {
                return null;
            }

            T max = E[0]; 
            for (int i = 0; i < P; i++)
            {
                if (E[i].Time > max.Time)
                    max = E[i]; 
            }
            return max; 
        }

        public T Get(long i)
        {
            return E.Where(s => s.Time == i).SingleOrDefault(); 
        }
    
        public IEnumerable<T> GetInterval(long begin, long end)
        {
            List<T> dickbutt = new List<T>();

            for (int i = 0; i < P; i++)
            {
                if(E[i].Time >= begin && E[i].Time < end)
                {
                    dickbutt.Add(E[i]); 
                }
            }

            return dickbutt; 
        }
        public void NormalizeTime()
        {
            //NOPE! 
            throw new NotImplementedException();
        }

        public void Register(IEnumerable<T> t)
        {
            foreach (var item in t)
            {
                Register(t); 
            }
        }

        public void Register(T t)
        {
            lock (Locker)
            {
                for (int i = 0; i < P; i++)
                {
                    if(E[i].Time == t.Time)
                    {
                        E[i] = t;
                        return; 
                    }
                }
           
                InsureSpace();

                E[P+1] = t;
                P++; 
            }
        }
        #endregion

        private void InsureSpace()
        {
            if(P+1 == E.Length)
            {
                T[] newList = new T[(E.Length + 1) * 2];

                E.CopyTo(newList, 0);

                E = newList;
            }
        }

        #region IEnumerator
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < P; i++)
            {
                yield return E[i]; 
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < P; i++)
            {
                yield return E[i];
            }
        }
        #endregion
    }
}
