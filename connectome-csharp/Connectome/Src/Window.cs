using System;
using SciterSharp;
using System.Threading;

namespace Connectome
{
	public class Window : SciterWindow
	{
		public Window()
		{
            EmotivSphero p = new EmotivSphero();

            EmotivSphero.Change = (action, power) =>
            {
                //Title = action + " " + power;
                LoadHtml("<html><body><a>Brain Status: "+ (action.Split('_')[1]+"") + " " + ((power > 0.00)? "YES" : "NO")  +"</a><br></body></hmtl>");
                UpdateWindow();
            };

            
            CreateMainWindow(800, 600);
			CenterTopLevelWindow();
			Title = "Sciter Bootstrap Hello";

            #if WINDOWS
            Icon = Properties.Resources.IconMain;
            #endif

            new Thread(p.Run).Start() ;
            
		}
	}
}