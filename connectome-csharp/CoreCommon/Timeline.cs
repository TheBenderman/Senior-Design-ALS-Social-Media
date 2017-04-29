using Connectome.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Connectome.Core.Common
{
    //TODO Unit Test thsi! -KLD
    /// <summary>
    /// Where to begin if time has no beginig...
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Timeline<T> : ITimeline<T> where T : class, ITime
    {
        #region Private Attributes
        /// <summary>
        /// Elements
        /// </summary>
        private T[] E;

        /// <summary>
        /// Pointer
        /// </summary>
        private int P;

        /// <summary>
        /// Holds Max Possible size for the array 
        /// <see cref="E"/>
        /// </summary>
        private int MaxSize; 

        private object Locker; 

        #endregion
        #region Constructors
        public Timeline()
        {
            Locker = new object();
            MaxSize = int.MaxValue; 
            E = new T[0];
            P = -1; 
        }

        /// <summary>
        /// Sets max size for the data structore. Newest element will replace oldest when full. 
        /// </summary>
        /// <param name="MaxSize"></param>
        public Timeline(int MaxSize) : this()
        {
            this.MaxSize = MaxSize;
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
        public long Length
        {
            get
            {
                return Math.Min(E.Length,P); 
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
            for (int i = 0; i < Length; i++)
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
            for (int i = 0; i < Length; i++)
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

            for (int i = 0; i < Length; i++)
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
                for (int i = 0; i < Length; i++)
                {
                    if(E[i].Time == t.Time)
                    {
                        E[i] = t;
                        return; 
                    }
                }
           
                InsureSpace();

                E[(P+1) % MaxSize] = t;
                P++; 
            }
        }
        #endregion
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
        #region Private Method
        private void InsureSpace()
        {
            if(P+1 == E.Length && P+1 != MaxSize)
            {
                T[] newList = new T[(E.Length + 1) * 2];

                E.CopyTo(newList, 0);

                E = newList;
            }
        }
        #endregion
    }
}
