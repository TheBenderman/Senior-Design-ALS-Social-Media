using Connectome.Core.Interface;
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
            IEmotivDevice device = new RandomEmotivDevice(.2f, 1f);

            //int interval = 500; //ms 
            //float thresh = .5f;
            //EmotivCommandType targetCmd = EmotivCommandType.NEUTRAL; 

            IConnectomeReader<IEmotivState> readerPlug = new BasicEmotivReader(device);

            int waitTimeSecond = 5;
            IConnectomeReader<IEmotivState> reader = new TimedEmotivReader(readerPlug, waitTimeSecond);

            reader.OnRead += (state) =>
            {
                Debug.WriteLine(state);
            };
           
            reader.StartReading();
   
            while (reader.IsReading)
            {
               //timelineProc.Process(null);
            }
           
            Debug.WriteLine("[END]");
        }

    }//end class 
}