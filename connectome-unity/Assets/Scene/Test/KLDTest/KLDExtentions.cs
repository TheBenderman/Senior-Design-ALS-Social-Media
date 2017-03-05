using Connectome.Emotiv.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Connectome.Unity.KLD.Extentions
{
    public static class KLDExtentions
    {
        /// <summary>
        /// Logs message into Unity's debug logs. 
        /// </summary>
        /// <param name="msg"></param>
        public static void Log(this string msg)
        {
            Debug.Log(msg); 
        } 

        /// <summary>
        /// Attaches a traker to a reader
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="tracker"></param>
        public static void Attach(this IEmotivReader reader, IEmotivReaderTracker tracker)
        {
            reader.OnRead += e => tracker.Track(e); 
        }

    }
}
