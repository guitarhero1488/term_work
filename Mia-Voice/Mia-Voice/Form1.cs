using System;
using System.Windows.Forms;
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;

namespace Mia_Voice
{
    public partial class Form1 : Form
    {
        
        WaveIn waveIn;
        WaveFileWriter writer;
        string outputFilename = "Rec.wav";
        IWavePlayer waveOutDevice = new WaveOut();

        public Form1()
        {
            InitializeComponent();
        }
        
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<WaveInEventArgs>(waveIn_DataAvailable), sender, e);
            }
            else
            {
                //Записываем данные из буфера в файл
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            }
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
            this.Hide();
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

        void StopRecording()
        {
            MessageBox.Show("StopRecording");
            waveIn.StopRecording();
        }

        private void MenuRecord_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (MenuRecord.Checked)
                {
                    MessageBox.Show("Start Recording");
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
                    //Начало записи
                    waveIn.StartRecording();
                    MenuRecord.Text = "Остановить запись";
                }
                else
                {
                    if (waveIn != null)
                    {
                        StopRecording();
                        MenuRecord.Text = "Начать запись";
                    }
                }

            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
