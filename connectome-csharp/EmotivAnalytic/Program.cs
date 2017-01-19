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
            IEmotivDevice device = new RandomEmotivDevice();

            int interval = 500; //ms 
            float thresh = .5f;
            EmotivCommandType targetCmd = EmotivCommandType.NEUTRAL; 

            IEmotivReader readerPlug = new EmotivAnalyticReader(device,targetCmd, interval, thresh);

            int waitTimeSecond = 5;
            IEmotivReader reader = new TimedEmotivReader(readerPlug, waitTimeSecond);

            reader.OnRead += (e) =>
            {
                Debug.WriteLine(e.State);
            }; 
           
            reader.Start();
   
            while (reader.IsReading) ;

            Debug.WriteLine("[END]");
        }

    }//end class 
}