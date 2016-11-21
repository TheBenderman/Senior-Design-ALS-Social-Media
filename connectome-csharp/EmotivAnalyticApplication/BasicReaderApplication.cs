using EmotivImpl;
using EmotivImpl.Device;
using EmotivWrapper;
using EmotivWrapper.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmotivAnalyticApplication
{
    public partial class BasicReaderApplication : Form
    {

        private EmotivDevice device;
        private List<EmotivState> list; 

        public BasicReaderApplication()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //connects or starts collecting data 
            if(device == null)
            {
                MessageBox.Show("Connect device first.", "Missing device",
                             MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int seconds;

                if (int.TryParse(textBox1.Text, out seconds))
                {
                    StartCollecting(seconds);
                }
                else
                {
                    MessageBox.Show("Unable to parse inputted time.", "Input Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
         
        }


        private void ToggleButton(string text, bool enable = true)
        {
            button1.Text = text;
            button1.Enabled = enable; 
        }

        private void StartCollecting(int sec)
        {
            //collect date for time 
            EmotivReader reader = new TimedEmotivReader(device, sec);
            list  = new List<EmotivState>(); 

            reader.OnRead = (state) => list.Add(state);
           
            reader.Start();
            ToggleButton("Reading...",false);


            //TODO learn threading 
            while (reader.isRunning) ;

            ToggleButton("Exporting...");

            //spit into excel 
            exportToExcl(list);
            ToggleButton("Start");

        }

        private void exportToExcl(IEnumerable<EmotivState> data)
        {
            //pop up window to get new file path 
            var popup = new SaveFileDialog();
            popup.DefaultExt = "csv";
            popup.FileName = "data"; 
            popup.ShowDialog();

            data = data.Where(e => e.command != EmotivStateType.NULL); 

            //pop up for path 
            string path = popup.FileName;

            int maxRow = 1048575; 

            //split into more than one file if too big 
            for (int i=0; i< (1+(data.Count()/ maxRow)); i++)
            {
                var subSet = data.Skip(i * maxRow).Take(maxRow); 

                // Writing the list to the .csv file
                try
                {
                    using (StreamWriter writer = new StreamWriter(path.Split('.')[0] + (i==0? "" : i.ToString()) + ".csv", false))
                    {
                        writer.WriteLine("Time (ms), Command, Power");

                        foreach (var state in subSet)
                        {
                            string line = state.time + "," + state.command + "," + state.power;

                            writer.WriteLine(line);
                        };
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "I suck",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            string user = usernameTextBox.Text.Trim();
            string pass = passwordTextBox.Text.Trim();
            string profile = profileTextBox.Text.Trim();

            connectButton.Text = "Connecting..."; 
            device = new EPOCEmotivDevice(user, pass, profile);
            string error; 
            if (device.Connect(out error))
            {
                connectButton.Text = "Connected!";
            }
            else
            {
                MessageBox.Show("Unable to connect device: " + error, "Connection Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                device = null;

                connectButton.Text = "Connect";
            }
        }
    }
}
