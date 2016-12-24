using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.IO;
using System.Linq;

namespace Mia_Record
{
    public partial class Settings : Form
    {

        public bool SListOtion()
        {
            string[] flines = File.ReadAllLines("CommandLines.ini");
            int j = 0;
            for (int i = 0; i < flines.Length; i += 2)
            {
                comboBox1.Items.Add(flines[i] + " || " + flines[i+1]);
                j++;
            }
            return true;
        }

        public Settings()
        {
            InitializeComponent();
            LineCount();
        }

        static int LineCount()
        {
            string[] flines = File.ReadAllLines("CommandLines.ini");
            return flines.Length / 2;
        }

        static int N = LineCount();
        int wcount = 3;
        SStruct[] SList = new SStruct[N];

        WaveIn waveIn;
        WaveFileWriter writer;

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
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            wcount--;
            if (wcount == 0 && waveIn != null)
            {
                waveIn.StopRecording();
                button1.Enabled = false;
                button1.Text = "Назначение...";
                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                {
                    button1.Enabled = true;
                    return;
                }
                else {
                    using (StreamWriter file = new StreamWriter("CommandLines.ini", true))
                    {
                        file.WriteLine();
                        file.WriteLine("SoundCollection/" + textBox1.Text + ".wav");
                        file.WriteLine(openFileDialog1.FileName);
                        button1.Enabled = true;
                    }
                } 
                
                timer1.Stop();
                wcount = 4;
            }
        }

        // Обработка речи - вычисляем, есть ли сама речь на звуковом отрезке
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
            else {
                result = false;
            }

            return result;
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //bool result = ProcessData(e);
            //if (result == true)
            //{
            byte[] buffer = e.Buffer;
            writer.Write(e.Buffer, 0, e.BytesRecorded);
            writer.Flush();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(RecordingStopped);
            waveIn.WaveFormat = new WaveFormat(8000, 1);
            writer = new WaveFileWriter("SoundCollection/" + textBox1.Text+".wav", waveIn.WaveFormat);
            waveIn.StartRecording();
            timer1.Enabled = !timer1.Enabled;
            button1.Enabled = false;
            button1.Text = "Запись...";
            //record1.Enabled = false;
            //запись звука

        }

        private void Settings_Load(object sender, EventArgs e)
        {
            SListOtion();
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private string[] Remove(string[] array, int item)
        {
            int remInd = item;

            string[] retVal = new string[array.Length - 1];

            for (int i = 0, j = 0; i < retVal.Length; ++i, ++j)
            {
                if (j == remInd)
                    ++j;

                retVal[i] = array[j];
            }

            return retVal;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //
            int it = comboBox1.SelectedIndex;
            it *= 2;
            string[] flines = File.ReadAllLines("CommandLines.ini");
            for (int i = it; i < (LineCount() * 2) - 1; i++)
            {
                flines[i] = flines[i + 1];
            }
            //flines[flines.Length - 1] = null;
            //flines[flines.Length - 2] = null;
            //flines = Remove(flines, flines.Length - 1);
            //flines = Remove(flines, flines.Length - 1);
            Array.Resize(ref flines, flines.Length - 2);
            File.WriteAllLines("CommandLines.ini", flines);
            //for (int i = 0; i < flines.Length - 2; i++)
            //{
                //using (StreamWriter file = new StreamWriter("CommandLines.ini", false))
                //{
                  //  file.WriteLine(flines);
                //}

            //}
        }
    }
}
