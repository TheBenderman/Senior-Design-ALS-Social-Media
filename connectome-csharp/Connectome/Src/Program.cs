using SciterSharp.Interop;
using System;
using System.Diagnostics;
using EmotivImpl.Device;
using EmotivImpl.Reader;
using EmotivWrapperInterface;

namespace Connectome
{
    class Program
	{
		public static Window AppWindow { get; private set; }// must keep a reference to survive GC
		public static Host AppHost { get; private set; }

        public static Host HostInstance { get; private set; }

        [STAThread]
		static void Main(string[] args)
		{

		#if WINDOWS
			// Sciter needs this for drag'n'drop support; STAThread is required for OleInitialize succeess
			int oleres = PInvokeWindows.OleInitialize(IntPtr.Zero);
			Debug.Assert(oleres == 0);
		#endif

		#if GTKMONO
			PInvokeGTK.gtk_init(IntPtr.Zero, IntPtr.Zero);
			Mono.Setup();
		#endif
			
			/*
				NOTE:
				In Linux, if you are getting a System.TypeInitializationException below, it is because you don't have 'libsciter-gtk-64.so' in your LD_LIBRARY_PATH.
				Run 'sudo bash install-libsciter.sh' contained in this package to install it in your system.
			*/
			// Create the window
			AppWindow = new Window();

			// Prepares SciterHost and then load the page
			AppHost = new Host(AppWindow);
            HostInstance = AppHost;

            IEmotivDevice device = new RandomEmotivDevice();

            long timeInterval = 3000;
            float threshHold = 0f;

            IEmotivReader reader = new EmotivAnalyticReader(device, EmotivStateType.NEUTRAL, timeInterval, threshHold); 

            reader.OnRead = (state) =>
            {
                // calls UI layer TIScript function with the font data
                    HostInstance.InvokePost(() =>
                    {
                        HostInstance.CallFunction("update");
                    });
            };

            // Run message loop
            reader.Start();
            PInvokeUtils.RunMsgLoop();

        }
	}
}