using System;
using System.Windows.Forms;
using NAudio.Wave;
using System.Runtime.InteropServices;

namespace Mia_Record
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            RegisterHotKey(Handle, 1, MOD_CONTROL + MOD_SHIFT, (int)Keys.R);
            myCommands.LoadCommands();
            CommandList.Items.Clear();
            for (int i = 0; i < myCommands.CommandList.Length; i++)
            {
                CommandList.Items.Add(myCommands.CommandList[i].Audio_URL);
            }
        }
        
        Commands myCommands = new Commands();
        Signal mySignal = new Signal();
        WaveIn waveIn;
        WaveFileWriter writer;
        DimScreen DScreen = new DimScreen();
        Settings DSettings = new Settings();
        int wcount = 3;
        bool activeRecord = false;
        const int MOD_CONTROL = 0x0002;
        const int MOD_SHIFT = 0x0004;
        const int WM_HOTKEY = 0x0312;

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            writer.Write(e.Buffer, 0, e.BytesRecorded);
            writer.Flush();
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
                mySignal.Process(myCommands.CommandsCount, myCommands);
                activeRecord = false;
            }
            DScreen.Close();
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        
        protected override void WndProc(ref Message m)
        {   
            if (m.Msg == WM_HOTKEY && (int)m.WParam == 1 && !activeRecord)
            {
                waveIn = new WaveIn();
                waveIn.DeviceNumber = 0;
                waveIn.DataAvailable += waveIn_DataAvailable;
                waveIn.RecordingStopped += new EventHandler<StoppedEventArgs>(RecordingStopped);
                waveIn.WaveFormat = new WaveFormat(8000, 1);
                writer = new WaveFileWriter(mySignal.outputFilename1, waveIn.WaveFormat);
                waveIn.StartRecording();
                timer1.Enabled = !timer1.Enabled;
                activeRecord = true;
                DScreen.ShowDialog();
            }
            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DSettings.ShowDialog();

            CommandList.Items.Clear();
            for (int i = 0; i < myCommands.CommandList.Length; i++)
            {
                CommandList.Items.Add(myCommands.CommandList[i].Audio_URL);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (CommandList.SelectedIndex.ToString() != "-1")
            {
                myCommands.RemoveCommand(CommandList.SelectedIndex);
                CommandList.Items.Clear();
                for (int i = 0; i < myCommands.CommandList.Length; i++)
                {
                    CommandList.Items.Add(myCommands.CommandList[i].Audio_URL);
                }
            }
        }

        private void Git_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/guitarhero1488/term_work");
        }

        //---------------------------------------------------------------------------------------------

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

        private void Exit_Click(object sender, EventArgs e)
        {
            UnregisterHotKey(Handle, 1);
            Environment.Exit(0);
        }
    }
}
