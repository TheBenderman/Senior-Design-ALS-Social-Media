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

            int waitTimeSecond = 40;
            IConnectomeReader<IEmotivState> reader = new TimedEmotivReader(readerPlug, waitTimeSecond);

            int p = -1; 
            reader.OnRead += (state) =>
            {
                if (device.WirelessSignalStrength != p)
                {
                    p = device.WirelessSignalStrength; 
                    //Debug.WriteLine(p);
                }
            };
           
            reader.StartReading();
   
            while (reader.IsReading)
            {
               
                //Debug.WriteLine(device.BatteryLevel);
            }

            Console.ReadLine();
            Console.WriteLine("[END]");
        }

    }//end class 
}