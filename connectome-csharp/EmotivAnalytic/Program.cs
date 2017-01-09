using EmotivImpl;
using EmotivImpl.Device;
using EmotivImpl.Reader;
using EmotivWrapperInterface;
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
            EmotivStateType targetCmd = EmotivStateType.NEUTRAL; 

            IEmotivReader readerPlug = new EmotivAnalyticReader(device,targetCmd, interval, thresh);

            int waitTimeSecond = 5;
            IEmotivReader reader = new TimedEmotivReader(readerPlug, waitTimeSecond);

            reader.OnRead = (state) =>
            {
                Debug.WriteLine(state);
            }; 
           
            reader.Start();
   
            while (reader.isRunning) ;

            Debug.WriteLine("[END]");
        }

    }//end class 
}