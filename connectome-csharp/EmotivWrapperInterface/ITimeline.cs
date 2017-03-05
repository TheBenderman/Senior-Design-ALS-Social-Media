using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Emotiv.Interface
{
    /// <summary>
    /// Epic data structore 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITimeline<T> : IEnumerable<T> where T : ITime
    {
        #region Property
        long Length { get; }
        #endregion
        #region Indexer
        T this[long time] {get;}
        IEnumerable<T> this[long time, long tme] {get;}
        #endregion
        #region Methods
        void Register(T t);
        void Register(IEnumerable<T> t);
        T Get(long i);
        IEnumerable<T> GetInterval(long begin, long end);
        T Earliest();
        T Latest();
        void NormalizeTime();
        #endregion
    }
}
