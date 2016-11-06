using SciterSharp;
using SciterSharp.Interop;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connectome
{
	class Program
	{
		public static Window AppWindow { get; private set; }// must keep a reference to survive GC
		public static Host AppHost { get; private set; }
		
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

            var wnd = new SciterWindow();
            wnd.CreateMainWindow(800, 600);
            wnd.CenterTopLevelWindow();
            wnd.Title = "Connectome";

            // Prepares SciterHost and then load the page
            var host = new Host(wnd);
        }
	}
}