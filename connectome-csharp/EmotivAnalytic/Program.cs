using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using System;
using System.Diagnostics;
using System.Timers;

namespace EmotivAnalytic
{
    class Program
    {
        /// <summary>
        /// Reader from random device and report states read from EmotivAnalyticReader
        /// </summary>
        static void Main(string[] args)
        {
            IEmotivDevice device = new EPOCEmotivDevice("emotiv123","Emotivbci123","KLD_Blink");

            device.OnConnectAttempted += (d, m) => Debug.WriteLine("de " + m);

            IEmotivReader readerPlug = new BasicEmotivReader(device, false);

            int waitTimeSecond = 1000;
            IEmotivReader reader = new TimedEmotivReader(readerPlug, waitTimeSecond);
            int count = 0;

            AppDomain.CurrentDomain.ProcessExit += (e, o) => { reader.Stop(); };

            Timer timer = new Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += (a, b) => { Console.WriteLine(count); };

            reader.OnRead += (e) =>
            {
                count++; 
                //Debug.WriteLine(e.State);
            };

            reader.OnStart += timer.Start; 
            reader.Start();

            while (reader.IsReading)
            {
              
            }
            timer.Stop();
            Debug.WriteLine("[END] " + count);
            Console.ReadLine();
        }

    }//end class 
}