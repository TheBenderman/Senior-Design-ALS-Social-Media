using System;
using SciterSharp;
using System.Threading;
using Connectome.Src;
using EmotivWrapper.Core;
using EmotivImpl.Device;
using EmotivImpl.Reader;
using EmotivImpl;

namespace Connectome
{
	public class Window : SciterWindow
	{
        public Window()
        {
            CreateMainWindow(1200, 800);
            CenterTopLevelWindow();
            Title = "Sciter Bootstrap Hello";

            #if WINDOWS
            Icon = Properties.Resources.IconMain;
            #endif

            new Thread(ButtonTimer.startTimer).Start();
        }
	}
}