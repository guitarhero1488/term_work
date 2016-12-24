using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.IO;
using System.Linq;

namespace Mia_Record
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LineCount();
        }

        static int LineCount() {
            string[] flines = File.ReadAllLines("CommandLines.ini");
            return flines.Length/2;
        }

        static int N = LineCount();
        int wcount = 3;
        SStruct[] SList = new SStruct[N];

        WaveIn waveIn;
        WaveFileWriter writer;
        string outputFilename1 = "Rec1.wav";

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
                Process();

            }
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

        public Double[] prepare(String wavePath)
        {
            double[] data;
            double sum = 0;
            int precision = 100;
            byte[] wave;
            FileStream WaveFile = File.OpenRead(wavePath);
            wave = new byte[WaveFile.Length];
            data = new double[(wave.Length - 44) / 4];
            WaveFile.Read(wave, 0, Convert.ToInt32(WaveFile.Length));
            double[] envelope = new double[data.Length / 100];

            for (int i = 44, j = 0; i < data.Length; i++)
            {
                data[i] = Math.Abs((BitConverter.ToInt32(wave, (1 + i) * 4)) / 65536.0);

                if (i != precision)
                {
                    sum = sum + data[i];
                }
                else
                {
                    envelope[j] = sum / 100;
                    j++;
                    sum = 0;
                    precision = precision + 100;
                }
            }

            WaveFile.Close();
            WaveFile.Dispose();
            Record.Enabled = true;

            return envelope;
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

                if (Tmp > threshold) { 
                    Tr = true;
                }
            }

            Sum2 /= Count;
            if (Tr || Sum2 > threshold) {
                result = true;
            } else {
                result = false;
            }

            return result;
        }

        private void stop_Click(object sender, EventArgs e)
        {
            if (waveIn != null)
            {
                waveIn.StopRecording();
            }
        }

        //private void record2_Click(object sender, EventArgs e)
        //{
        //    //waveIn = new WaveIn();
        //    //waveIn.DeviceNumber = 0;
        //    //waveIn.DataAvailable += waveIn_DataAvailable;
        //    //waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(RecordingStopped);
        //    //waveIn.WaveFormat = new WaveFormat(8000, 1);
        //    //writer = new WaveFileWriter(outputFilename2, waveIn.WaveFormat);
        //    //waveIn.StartRecording();
        //    //timer1.Enabled = !timer1.Enabled;
        //}

        //private void start_Click(object sender, EventArgs e)
        //{
        //    double corel = 0;
        //    int a = 65536;
        //    int chart_threshold = 500;
        //    double[] data1 = prepare("Rec1.wav");
        //    double[] data2 = prepare("Rec2.wav");

        //    if (data1.Length < data2.Length)
        //    {
        //        double[] swap = new double[data2.Length];
        //        for (int i = 0; i < data1.Length; i++)
        //        {
        //            swap[i] = data1[i];
        //        }
        //        corel = corr(swap, data2);
        //    }
        //    else if (data1.Length > data2.Length)
        //    {
        //        double[] swap = new double[data1.Length];
        //        for (int i = 0; i < data2.Length; i++)
        //        {
        //            swap[i] = data2[i];
        //        }
        //        corel = corr(data1, swap);
        //    }
        //    else
        //    {
        //        corel = corr(data1, data2);
        //    }

        //    foreach (double m in data1)
        //    {
        //        bool flag = false;
        //        if (m > chart_threshold)
        //        {
        //            chart1.Series[0].Points.Add(m);
        //            flag = true;
        //        }
        //        chart1.Series[0].Points.Add(m);
        //    }

        //    foreach (double m in data2)
        //    {
        //        bool flag = false;
        //        if (m > chart_threshold)
        //        {
        //            chart2.Series[0].Points.Add(m);
        //            flag = true;
        //        }
        //        chart2.Series[0].Points.Add(m);
        //    }

        //    label1.Text = corel.ToString();
        //    start.Enabled = false;
        //}

        public double corr(double[] data1, double[] data2)
        {
            try
            {
                double sumX = 0, sumY = 0, sumX2 = 0, sumY2 = 0, sumXY = 0;
                int N = data1.Length;
                for (int i = 0; i < N; i++)
                {
                    sumXY = sumXY + (data1[i] * data2[i]);
                }

                for (int i = 0; i < N; i++)
                {
                    sumX = sumX + data1[i];
                }

                for (int i = 0; i < N; i++)
                {
                    sumY = sumY + data2[i];
                }

                for (int i = 0; i < N; i++)
                {
                    sumX2 = sumX2 + Math.Pow(data1[i], 2);
                }

                for (int i = 0; i < N; i++)
                {
                    sumY2 = sumY2 + Math.Pow(data2[i], 2);
                }

                return (sumXY - (sumX * sumY) / N) / (Math.Sqrt(sumX2 - (Math.Pow(sumX, 2) / N)) * Math.Sqrt(sumY2 - (Math.Pow(sumY, 2) / N)));
            }
            catch
            {
                return 0;
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    chart2.Series[0].Points.Clear();
        //    chart1.Series[0].Points.Clear();

        //    double corel = 0;
        //    double[] data1 = prepare("Rec1.wav");
        //    data1 = Normalize(data1);
            
        //    for (int l = 0; l <= N - 1; l++) 
        //    {
        //        double[] data2 = prepare(SList[l].Audio_URL);
                
        //        data2 = Normalize(data2);

        //        if (data1.Length < data2.Length)
        //        {
        //            double[] swap = new double[data2.Length];
        //            for (int i = 0; i < data1.Length; i++)
        //            {
        //                swap[i] = data1[i];
        //            }
        //            corel = corr(swap, data2);
        //        }
        //        else if (data1.Length > data2.Length)
        //        {
        //            double[] swap = new double[data1.Length];
        //            for (int i = 0; i < data2.Length; i++)
        //            {
        //                swap[i] = data2[i];
        //            }
        //            corel = corr(data1, swap);
        //        }
        //        else
        //        {
        //            corel = corr(data1, data2);
        //        }

        //        foreach (double m in data1)
        //        {
        //            chart2.Series[0].Points.Add(m);
        //        }

        //        foreach (double m in data2)
        //        {
        //            chart1.Series[0].Points.Add(m);
        //            listBox1.Items.Insert(0, m);
        //        }

        //        MessageBox.Show(corel.ToString());

        //        if (corel >= 0.5)
        //        {
        //            System.Diagnostics.Process.Start(SList[l].Program_URL);
        //            break;
        //        }
        //    }
        //}

        private void offset(ref double[] data, int ind)
        {
            int offset = ind;
            for (int i = 0; i < data.Length - offset; i++, ind++)
            {
                data[i] = data[ind];
            }
            Array.Resize(ref data, (data.Length - offset));
        }

        public void Process()
        {
            double corel = 0;
            double[] corel_array = new double[N];
            double[] data1 = prepare(outputFilename1);
            data1 = Normalize(data1);

            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] > 0.2)
                {
                    offset(ref data1, i);
                    break;
                }
            }

            for (int l = 0; l < N; l++)
            {
                corel = 0;
                double[] data2 = prepare(SList[l].Audio_URL);
                data2 = Normalize(data2);

                for (int i = 0; i < data2.Length; i++)
                {
                    if (data2[i] > 0.2)
                    {
                        offset(ref data2, i);
                        break;
                    }
                }

                if (data1.Length < data2.Length)
                {
                    Array.Resize(ref data2, data1.Length);
                    corel = corr(data1, data2);
                }
                else if (data1.Length > data2.Length)
                {
                    Array.Resize(ref data1, data2.Length);
                    corel = corr(data1, data2);
                }
                else
                {
                    corel = corr(data1, data2);
                }

                corel_array[l] = corel;
            }

            double swapon = 0;
            int maxInd = 0;
            for (int k = 0; k < corel_array.Length; k++)
            {
                if (corel_array[k] > swapon)
                {
                    swapon = corel_array[k];
                    maxInd = k;
                }
            }

            if (swapon > 0.6)
            {
                System.Diagnostics.Process.Start(SList[maxInd].Program_URL);
            }
            else MessageBox.Show("Please, record your commad again!");

            //return corel_array[maxInd];
        }

        public double[] Normalize(double[] array)
        {
            for (int index = 0; index < array.Length; index++)
            {
                array[index] = (array[index] - array.Min()) / (array.Max() - array.Min());
            }

            return array;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Tray.ShowBalloonTip(500);

            string[] flines = File.ReadAllLines("CommandLines.ini");
            int j = 0;
            for (int i = 0; i < flines.Length; i += 2)
            {
                SList[j].Audio_URL = flines[i];
                SList[j].Program_URL = flines[i + 1];
                CommandList.Items.Insert(0, SList[j].Audio_URL + " # " + SList[j].Program_URL);
                j++;
            }

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            Show();
        }

        private void Record_Click(object sender, EventArgs e)
        {
            waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(RecordingStopped);
            waveIn.WaveFormat = new WaveFormat(8000, 1);
            writer = new WaveFileWriter(outputFilename1, waveIn.WaveFormat);
            waveIn.StartRecording();
            timer1.Enabled = !timer1.Enabled;
            Record.Enabled = false;
        }
    }
}
