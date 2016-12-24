using System;
using System.Windows.Forms;
using MathNet.Numerics.IntegralTransforms;
using NAudio.Wave;
using MathNet.Numerics;
using NAudio.Dsp;

namespace Mia_Record
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        WaveIn waveIn;
        WaveFileWriter writer;
        float sample32;

        string outputFilename = "Rec.wav";


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
        int count = 0;
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //for (int index = 0; index < e.BytesRecorded; index += 2)
            //{
            //    short sample = (short)((e.Buffer[index + 1] << 8) |
            //                            e.Buffer[index + 0]);
            //    sample32 = sample / 32768f;
            //}

            //    writer.Write(e.Buffer, 0, e.BytesRecorded);
            //    writer.Flush();

            bool result = ProcessData(e);
            if (result == true)
            {
                listBox1.Items.Insert(0,count++);
                writer.Write(e.Buffer, 0, e.BytesRecorded);
                writer.Flush();
            }
            else
                {
                    
                }
        }

        // Обработчка речи - вычисляем, есть ли сама речь на звуковом отрезке
        private bool ProcessData(WaveInEventArgs e)
        {
            double porog = 0.05;
            bool result = false;
            bool Tr = false;
            double Sum2 = 0;
            int Count = e.BytesRecorded / 2;

            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                double Tmp = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
                Tmp /= 32768.0;
                Sum2 += Tmp * Tmp;

                if (Tmp > porog) { 
                    Tr = true;
                }
            }

            Sum2 /= Count;

            if (Tr || Sum2 > porog) {
                result = true;
            } else {
                result = false;
            }

            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(RecordingStopped);
            waveIn.WaveFormat = new WaveFormat(8000, 1);
            writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
            waveIn.StartRecording();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
            }
        }
    }

}
