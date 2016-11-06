using System;
using AppKit;
using Foundation;
using SciterSharp;

namespace Connectome
{
	class AppOSX
	{
		static void Main(string[] args)
		{
			NSApplication.Init();

			using(var p = new NSAutoreleasePool())
			{
				var application = NSApplication.SharedApplication;
				application.Delegate = new AppDelegate();
				application.Run();
			}
		}
	}

	[Register("AppDelegate")]// needed?
	class AppDelegate : NSApplicationDelegate
	{
		static readonly SciterMessages sm = new SciterMessages();
		public static Window AppWindow { get; private set; }
		public static Host AppHost { get; private set; }

		public override void DidFinishLaunching(NSNotification notification)
		{
			Mono.Setup();

			// Create the window
			AppWindow = new Window();

			// Prepares SciterHost and then load the page
			AppHost = new Host(AppWindow);
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
		{
			return false;
		}

		public override void WillTerminate(NSNotification notification)
		{
			// Insert code here to tear down your application
		}
	}

	// In OSX/Xamarin Studio, make Sciter messages be shown at 'Application Output' panel
    class SciterMessages : SciterDebugOutputHandler
    {
        protected override void OnOutput(SciterSharp.Interop.SciterXDef.OUTPUT_SUBSYTEM subsystem, SciterSharp.Interop.SciterXDef.OUTPUT_SEVERITY severity, string text)
        {
            Console.WriteLine(text);
        }
    }
}