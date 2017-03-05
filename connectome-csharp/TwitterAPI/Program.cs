using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Connectome.Twitter.API
{
    class Program
    {
        
        static int Main (string[] args)
        {
			TwitterAuthenticator authenticator = TwitterAuthenticator.Instance;
			Console.WriteLine("Please visit the url here: " + authenticator.getAuthorizationURL());
            Console.WriteLine("Enter your pin code: ");
            String pin = Console.ReadLine();
			authenticator.enterPinCode(pin);

			TwitterInteractor interactor = new TwitterInteractor(authenticator);
			interactor.getTop5HomeTimeLineTweets();

            while (true) { };
            return 0;
        }
    }
}
