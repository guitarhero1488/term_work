namespace Mia_Voice
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuGit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tray
            // 
            this.Tray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.Tray.BalloonTipText = "Работает в фоновом режиме";
            this.Tray.BalloonTipTitle = "Mia-Voice v0.1";
            this.Tray.ContextMenuStrip = this.TrayMenu;
            this.Tray.Icon = ((System.Drawing.Icon)(resources.GetObject("Tray.Icon")));
            this.Tray.Visible = true;
            // 
            // TrayMenu
            // 
            this.TrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuRecord,
            this.MenuSettings,
            this.toolStripSeparator1,
            this.MenuGit,
            this.toolStripSeparator2,
            this.MenuExit});
            this.TrayMenu.Name = "contextMenuStrip1";
            this.TrayMenu.ShowImageMargin = false;
            this.TrayMenu.Size = new System.Drawing.Size(135, 104);
            // 
            // MenuRecord
            // 
            this.MenuRecord.Name = "MenuRecord";
            this.MenuRecord.Size = new System.Drawing.Size(134, 22);
            this.MenuRecord.Text = "Начать запись";
            this.MenuRecord.Click += new System.EventHandler(this.MenuRecord_Click);
            // 
            // MenuSettings
            // 
            this.MenuSettings.Name = "MenuSettings";
            this.MenuSettings.Size = new System.Drawing.Size(134, 22);
            this.MenuSettings.Text = "Настройки";
            this.MenuSettings.Click += new System.EventHandler(this.MenuSettings_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // MenuGit
            // 
            this.MenuGit.Name = "MenuGit";
            this.MenuGit.Size = new System.Drawing.Size(134, 22);
            this.MenuGit.Text = "Git Source Code";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(131, 6);
            // 
            // MenuExit
            // 
            this.MenuExit.Name = "MenuExit";
            this.MenuExit.Size = new System.Drawing.Size(134, 22);
            this.MenuExit.Text = "Выход";
            this.MenuExit.Click += new System.EventHandler(this.MenuExit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 405);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mia-Voice : Настройки";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TrayMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon Tray;
        private System.Windows.Forms.ContextMenuStrip TrayMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuRecord;
        private System.Windows.Forms.ToolStripMenuItem MenuSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem MenuGit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem MenuExit;
    }
}

