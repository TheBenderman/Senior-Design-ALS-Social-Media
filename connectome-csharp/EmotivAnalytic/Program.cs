using Connectome.Core.Common;
using Connectome.Core.Implementation;
using Connectome.Core.Interface;
using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using System.Diagnostics;

namespace EmotivAnalytic
{
    class Program
    {
        /// <summary>
        /// Reader from random device and report states read from EmotivAnalyticReader
        /// </summary>
        static void Main(string[] args)
        {
            TimelineProcessor<object> timelineProc = new TimelineProcessor<object>(new Timeline<IEmotivState>());

            var r = new RefreshProcessee();
            r.TargetCommand = EmotivCommandType.PUSH;
            r.RefreshInterval = 200;
            r.ThreashHold = .25f; 
            
            timelineProc.Children = new IProcessable<ITimeline<IEmotivState>>[] { r };

            IEmotivDevice device = new RandomEmotivDevice(.2f, 1f);

            //int interval = 500; //ms 
            //float thresh = .5f;
            //EmotivCommandType targetCmd = EmotivCommandType.NEUTRAL; 

            IEmotivReader readerPlug = new BasicEmotivReader(device);

            int waitTimeSecond = 5;
            IEmotivReader reader = new TimedEmotivReader(readerPlug, waitTimeSecond);

            reader.OnRead += (e) =>
            {
                Debug.WriteLine(e.State);
            };
            reader.OnRead += timelineProc.Track; 

            reader.Start();
   
            while (reader.IsReading)
            {
                timelineProc.Process(null);
            }
           
            Debug.WriteLine("[END]");
        }

    }//end class 
}