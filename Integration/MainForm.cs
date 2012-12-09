using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Integration {

    public partial class MainForm : Form {
        private App55.Integration integration;

        public MainForm() {
            InitializeComponent();
            integration = new App55.Integration(this);
            integration.OnStop += new EventHandler<EventArgs>(MainForm_OnStop);
            integration.OnWrite += new EventHandler<App55.WriteEventArgs>(MainForm_OnWrite);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            Properties.Settings.Default.Save();
        }

        private void startStopButton_Click(object sender, EventArgs e) {
            if(integration.IsStarted) {
                startStopButton.Text = "Stop";
                integration.Stop();
            } else integration.Start(Properties.Settings.Default.apiKey, Properties.Settings.Default.apiSecret);
        }

        private void MainForm_OnWrite(object sender, App55.WriteEventArgs e) {
            this.outputTextBox.Text += e.Message;
            this.outputTextBox.SelectionStart = this.outputTextBox.Text.Length;
            this.outputTextBox.ScrollToCaret();
        }

        private void MainForm_OnStop(object sender, EventArgs e) {
            startStopButton.Text = "Start";
        }
        
    }

    
}
