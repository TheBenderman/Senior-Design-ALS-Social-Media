using EmotivImpl;
using EmotivImpl.Device;
using EmotivImpl.Reader;
using EmotivWrapper;
using EmotivWrapper.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmotivAnalytic
{
    class Program
    {
        static void Main(string[] args)
        {
            EmotivDevice device = new RandomEmotivDevice();

            EmotivReader reader = new EmotivAnalyticReader(device, EmotivStateType.NEUTRAL, 2, 0.5f);

            reader.OnRead = (state) => Debug.WriteLine(state);

            reader.Start(); 
        }


        static void oldPrimitive()
        {
           EmotivSphero emo = new EmotivSphero();
           float threshHold = .01f;

           double duration = 30;

           double interval = 1;  ///seconds 

           string targetCmd = "NEUTRAL";


           //Timer timer =  Timer
           var time = DateTime.Now;

           List<MentalRecord> list = new List<MentalRecord>();

           Random r = new Random();

           int i = 0;
           int smapleSize = (int)((list.Count() * interval) / duration);

           bool isTargergatted = true;

           List<bool> succ = new List<bool>();

           var watch = Stopwatch.StartNew();



           EmotivSphero.Update = (cmd, str) =>
           {

               //Debug.WriteLine(cmd + " " + str );
               //  cmd = (r.Next() % 2 == 0) ? targetCmd : "Buh";
               // str = (float)(r.NextDouble()); 

               //list.Add(new MentalRecord(){ strength = str, cmd = cmd  });

               succ.Add(((isTargergatted) ? cmd == targetCmd : cmd != targetCmd) && str >= threshHold);

               if (watch.Elapsed.Seconds > 3)
               {
                   List<bool> subList = succ.Where(f => f == isTargergatted).ToList();
                   float rate = ((float)subList.Count()) / succ.Count();

                   Debug.WriteLine(rate);
                   //print 
                   isTargergatted = !isTargergatted;
                   succ = new List<bool>();
                   i = 0;

                   watch.Restart();
                   watch.Start(); 
               }
           };
           Debug.WriteLine("----------------");

           Thread thread = new Thread(emo.Run);

           watch.Start();
           //thread.Start();

           DateTime endingTime = DateTime.Now.AddSeconds(duration);


           while (DateTime.Now < endingTime)
           {
              EmotivSphero.Update(r.Next(2)==1? targetCmd : "Nope", (float)r.NextDouble()); 
           }


           //print result 
           thread.Abort();




          /*
           foreach(var state in list)
           {

               succ.Add(((isTargergatted)? state.cmd == targetCmd : state.cmd != targetCmd) && state.strength >= threshHold);

               if(i/smapleSize == 1)
               {
                   List<bool> subList = succ.Where(f => f == isTargergatted).ToList(); 
                   float rate = ((float)subList.Count())/smapleSize;

                   Debug.WriteLine(rate);
                   //print 
                   isTargergatted = !isTargergatted;
                   succ = new List<bool>();
                   i = 0; 
               }
               i++; 
           }*/


        }
    }//end class 

    public class MentalRecord
    {
       public float strength;
       public  string cmd;
        public double time; 

        //public string GetValidComd(float thresh)
       // {

        //}
    }

}
