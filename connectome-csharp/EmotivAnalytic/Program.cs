using Connectome.Core.Interface;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using System;
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
            IEmotivDevice device = new EPOCEmotivDevice("emotiv123", "Emotivbci123", "KLD_Blink");

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
                Console.WriteLine(device.BatteryLevel); 
            }
           
            Debug.WriteLine("[END]");
        }

    }//end class 
}