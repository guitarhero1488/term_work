using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.IO;
using System.Linq;

namespace Mia_Record
{
    public partial class Settings : Form
    {

        public Settings()
        {
            InitializeComponent();
        }

        DimScreen DScreen = new DimScreen();
        int wcount = 3;
        WaveIn waveIn;
        WaveFileWriter writer;
        Commands myCommands = new Commands();

        void RecordingStopped(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(RecordingStopped), sender, e);
            }
            else
            {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
                DScreen.Hide();
                myCommands.AddCommand("SoundCollection/" + textBox1.Text + ".wav", textBox2.Text);
                Hide();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            wcount--;
            if (wcount == 0 && waveIn != null)
            {
                waveIn.StopRecording();
                timer1.Stop();
                wcount = 4;
            }
        }
        
        private bool ProcessData(WaveInEventArgs e)
        {
            double threshold = 0.05;
            bool result = false;
            bool Tr = false;
            double Sum2 = 0;
            int Count = e.BytesRecorded / 2;

            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                double Tmp = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
                Tmp /= 32768.0;
                Sum2 += Tmp * Tmp;

                if (Tmp > threshold)
                {
                    Tr = true;
                }
            }

            Sum2 /= Count;
            if (Tr || Sum2 > threshold)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            writer.Write(e.Buffer, 0, e.BytesRecorded);
            writer.Flush();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                waveIn = new WaveIn();
                waveIn.DeviceNumber = 0;
                waveIn.DataAvailable += waveIn_DataAvailable;
                waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(RecordingStopped);
                waveIn.WaveFormat = new WaveFormat(8000, 1);
                writer = new WaveFileWriter("SoundCollection/" + textBox1.Text + ".wav", waveIn.WaveFormat);
                waveIn.StartRecording();
                timer1.Enabled = !timer1.Enabled;
                DScreen.ShowDialog();
            }
            else
            {
                MessageBox.Show("Заполните все поля.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }        
    }
}
