using Connectome.Emotiv.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Connectome.Emotiv.Common
{
    public class EmotivDeviceConnector
    {
        private IEmotivDevice Device;
        private Thread ConnectingThread; 

        /// <summary>
        /// Connect a device on a thread
        /// </summary>
        /// <param name="device"></param>
        public EmotivDeviceConnector(IEmotivDevice device)
        {
            Device = device;
           
            ConnectingThread = new Thread(device.Connect); 
        }

        public void Connect()
        {
            ConnectingThread.Start(); 
        }

        public bool IsAlive()
        {
            return ConnectingThread.IsAlive; 
        }
    }
}
