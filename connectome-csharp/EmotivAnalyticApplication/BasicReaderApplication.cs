using Connectome.Emotiv.Enum;
using Connectome.Emotiv.Implementation;
using Connectome.Emotiv.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EmotivAnalyticApplication
{
    public partial class BasicReaderApplication : Form
    {
        private IEmotivDevice device;
        private List<IEmotivState> list; 

        public BasicReaderApplication()
        {
            InitializeComponent();
        }

        //fix this pile of disgust 
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

                if (int.TryParse(TimeTextBox.Text, out seconds))
                {
                    IEmotivReader reader = new BasicEmotivReader(device); 
                    StartCollecting(reader, seconds);
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
            ReadStatus.Text = text;
            ReadButton.Enabled = enable; 
        }

        private void StartCollecting(IEmotivReader readerPlug, int seconds)
        {
            //collect date for time 
            list  = new List<IEmotivState>();

            IEmotivReader reader = new TimedEmotivReader(readerPlug, seconds); 

            reader.OnRead += (e) => list.Add(e.State);

            ToggleButton("Reading...",false);
            reader.Start();

            //TODO learn threading with UI
            while (reader.IsReading) ;

            ToggleButton("Exporting...", false);

            //spit into excel 
            exportToExcl(list);
            ToggleButton("Exported!");
        }

        private void exportToExcl(IEnumerable<IEmotivState> data)
        {
            //pop up window to get new file path 
            var popup = new SaveFileDialog();
            popup.DefaultExt = "csv";
            popup.FileName = "data"; 
            popup.ShowDialog();

            data = data.Where(e => e.Command != EmotivCommandType.NULL); 

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
                            string line = state.Time + "," + state.Command + "," + state.Power;

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
            string user = UsernameTextBox.Text.Trim();
            string pass = PasswordTextBox.Text.Trim();
            string profile = ProfileTextBox.Text.Trim();

            ConnectionLabel.Text = "Connecting..."; 
            device =  new EPOCEmotivDevice(user, pass, profile);

            bool connected = true;
            string error = "" ;

            device.OnConnectAttempted += (c, em) => { connected = c; error = em;  };
            device.Connect();

            if (connected)
            {
                ConnectionLabel.Text = "Device connected"; 
            }
            else
            {
                MessageBox.Show("Unable to connect device: " + error, "Connection Error",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                device = null;

                ConnectionLabel.Text = "Failed to connect device. Try again";
            }
        }

        private void ConnectionLabel_Click(object sender, EventArgs e)
        {
            device = new RandomEmotivDevice();

            ConnectionLabel.Text = "Random Device Connected!"; 
        }
    }
}
