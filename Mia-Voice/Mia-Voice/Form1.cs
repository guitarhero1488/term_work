using System;
using System.Windows.Forms;
using MathNet.Numerics.IntegralTransforms;
using System.Numerics;
using NAudio.Wave;

namespace Mia_Voice
{
    public partial class Form1 : Form
    {

        WaveIn waveIn;
        WaveIn stream;
        WaveFileWriter wstream;
        WaveFileWriter writer;
        string outputFilename = "Rec.wav";
        IWavePlayer waveOutDevice = new WaveOut();
        static double Fs = 42000; // Частота дискретизвции !В данной программе ТОЛЬКО целые числа
        static double T = 1.0 / Fs; // Шаг дискретизации
        static double Fn = Fs / 2;// Частота Найквиста

        public Form1()
        {
            InitializeComponent();
        }



        bool startRecording = false;
        float sample32;
        void Stream_DataAvailable(object sender, WaveInEventArgs e)
        {
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((e.Buffer[index + 1] << 8) |
                                        e.Buffer[index + 0]);
                sample32 = sample / 32768f;
                if (sample32 > 0.2)
                {
                    startRecording = true;
                }
                else if (startRecording && count_timer == 3)
                {
                    startRecording = false;
                    timer1.Enabled = false;
                    waveIn.StopRecording();
                    count_timer = 0;
                }
            }

            if (startRecording)
            {
                waveIn.StartRecording();
                timer1.Enabled = true;
                listBox1.Items.Insert(0, sample32);
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            }
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            //for (int index = 0; index < e.BytesRecorded; index += 2)
            //{
            //    short sample = (short)((e.Buffer[index + 1] << 8) |
            //                            e.Buffer[index + 0]);
            //    sample32 = sample / 32768f;
            //    if (sample32 > 0.2)
            //    {
            //        startRecording = true;
            //    }
            //    else if (startRecording && count_timer == 3)
            //    {
            //        startRecording = false;
            //        timer1.Enabled = false;
            //        waveIn.StopRecording();
            //        count_timer = 0;
            //    }
            //}

            //if (startRecording)
            //{
            //    timer1.Enabled = true;
            //    listBox1.Items.Insert(0, sample32);
            //    writer.Write(e.Buffer, 0, e.BytesRecorded);
            //    writer.Flush();
            //}
        }

        void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(waveIn_RecordingStopped), sender, e);
            }
            else
            {
                waveIn.Dispose();
                waveIn = null;
                writer.Close();
                writer = null;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.Hide();

            stream = new WaveIn();
            stream.DeviceNumber = 0;
            stream.DataAvailable += Stream_DataAvailable;
            waveIn = new WaveIn();
            waveIn.DeviceNumber = 0;
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(waveIn_RecordingStopped);
            waveIn.WaveFormat = new WaveFormat(8000, 1);
            writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
            stream.StartRecording();
        }
        
        private void MenuExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MenuSettings_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        
        private void MenuRecord_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (MenuRecord.Checked)
                {
                    waveIn = new WaveIn();
                    //Дефолтное устройство для записи (если оно имеется)
                    //встроенный микрофон ноутбука имеет номер 0
                    waveIn.DeviceNumber = 0;
                    //Прикрепляем к событию DataAvailable обработчик, возникающий при наличии записываемых данных
                    waveIn.DataAvailable += waveIn_DataAvailable;
                    //Прикрепляем обработчик завершения записи
                    waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(waveIn_RecordingStopped);
                    //Формат wav-файла - принимает параметры - частоту дискретизации и количество каналов(здесь mono)
                    waveIn.WaveFormat = new WaveFormat(8000, 1);
                    //Инициализируем объект WaveFileWriter
                    writer = new WaveFileWriter(outputFilename, waveIn.WaveFormat);
                    MenuRecord.Text = "Остановить запись";
                    waveIn.StartRecording();
                }
                else
                {
                    if (waveIn != null)
                    {
                        waveIn.StopRecording();
                        MenuRecord.Text = "Начать запись";
                    }
                }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
        int count_timer = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            count_timer += 1;
        }
    }
}
