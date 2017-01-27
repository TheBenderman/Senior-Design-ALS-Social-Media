using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Core.Interface
{
    /// <summary>
    /// Epic data structore 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITimeline<T> where T : ITime 
    {
        long Duration { get; }

        T this[long time, long tme] {set;get;}
        T this[long time] {set;get;}

        void Register(T t);
        void Register(IEnumerable<T> t);

        T Get(int i);
        IEnumerable<T> GetInterval(int begin, int end);

        T Earliest();
        T Latest();

        void NormalizeTime(); 
    }
}
