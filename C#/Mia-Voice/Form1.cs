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

        bool record = false;

        public Form1()
        {
            InitializeComponent();
        }

        // Сюда код по обработке звука NAudio
        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
        }

        void waveIn_RecordingStopped(object sender, EventArgs e)
        {
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

        private void MenuRecord_Click(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
   
    }
}
