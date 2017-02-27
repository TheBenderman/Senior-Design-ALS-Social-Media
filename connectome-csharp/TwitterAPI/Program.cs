using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Twitter.API
{
    class Program
    {
        
        public static void Main ()
        {
            TwitterAPI api = TwitterAPI.Instance;
            Console.WriteLine("Please visit the url here: " + api.getAuthorizationURL());
            Console.WriteLine("Enter your pin code: ");
            String pin = Console.ReadLine();
            api.enterPinCode(pin);


            Console.WriteLine("Write a tweeet: ");
            api.publishTweet(Console.ReadLine());

            Console.WriteLine("[End]");
            Console.ReadLine();     
           }
    }
}
