using System;
using SciterSharp;
using System.Threading;
using Connectome.Src;

namespace Connectome
{
	public class Window : SciterWindow
	{
        public Window()
        {
            EmotivSphero p = new EmotivSphero();

            EmotivSphero.Change = (action, power) =>
            {
                // calls UI layer TIScript function with the font data
                Program.HostInstance.InvokePost(() =>
                {
                    Program.HostInstance.CallFunction("UpdateText", new SciterValue(action.Split('_')[1] + " " + ((power > 0.00) ? "YES" : "NO")));
                });
            };

            CreateMainWindow(1200, 800);
            CenterTopLevelWindow();
            Title = "Sciter Bootstrap Hello";

            #if WINDOWS
            Icon = Properties.Resources.IconMain;
            #endif

            new Thread(p.Run).Start();

            new Thread(ButtonTimer.startTimer).Start();
        }
	}
}