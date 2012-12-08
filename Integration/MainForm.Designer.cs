namespace Integration {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.apiKeyTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.apiSecretTextBox = new System.Windows.Forms.TextBox();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.startStopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "API Key";
            // 
            // apiKeyTextBox
            // 
            this.apiKeyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.apiKeyTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Integration.Properties.Settings.Default, "apiKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.apiKeyTextBox.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apiKeyTextBox.Location = new System.Drawing.Point(97, 17);
            this.apiKeyTextBox.Margin = new System.Windows.Forms.Padding(8);
            this.apiKeyTextBox.Name = "apiKeyTextBox";
            this.apiKeyTextBox.Size = new System.Drawing.Size(574, 24);
            this.apiKeyTextBox.TabIndex = 0;
            this.apiKeyTextBox.Text = global::Integration.Properties.Settings.Default.apiKey;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "API Secret";
            // 
            // apiSecretTextBox
            // 
            this.apiSecretTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.apiSecretTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Integration.Properties.Settings.Default, "apiSecret", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.apiSecretTextBox.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apiSecretTextBox.Location = new System.Drawing.Point(97, 57);
            this.apiSecretTextBox.Margin = new System.Windows.Forms.Padding(8);
            this.apiSecretTextBox.Name = "apiSecretTextBox";
            this.apiSecretTextBox.Size = new System.Drawing.Size(574, 24);
            this.apiSecretTextBox.TabIndex = 3;
            this.apiSecretTextBox.Text = global::Integration.Properties.Settings.Default.apiSecret;
            // 
            // outputTextBox
            // 
            this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.outputTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputTextBox.Location = new System.Drawing.Point(20, 145);
            this.outputTextBox.Margin = new System.Windows.Forms.Padding(8);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputTextBox.Size = new System.Drawing.Size(651, 295);
            this.outputTextBox.TabIndex = 5;
            // 
            // startStopButton
            // 
            this.startStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startStopButton.AutoSize = true;
            this.startStopButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.startStopButton.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startStopButton.Location = new System.Drawing.Point(599, 97);
            this.startStopButton.Margin = new System.Windows.Forms.Padding(8);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Padding = new System.Windows.Forms.Padding(12, 3, 12, 3);
            this.startStopButton.Size = new System.Drawing.Size(72, 32);
            this.startStopButton.TabIndex = 6;
            this.startStopButton.Text = "Start";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 457);
            this.Controls.Add(this.startStopButton);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.apiSecretTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.apiKeyTextBox);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "MainForm";
            this.Text = "App55 .NET Integration Tests";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox apiKeyTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox apiSecretTextBox;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button startStopButton;
    }
}

