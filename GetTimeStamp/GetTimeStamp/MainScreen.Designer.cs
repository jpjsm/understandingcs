namespace GetTimeStamp
{
    partial class MainScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.timestamp = new System.Windows.Forms.TextBox();
            this.timestampUtc = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button1.Location = new System.Drawing.Point(105, 27);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // timestamp
            // 
            this.timestamp.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.timestamp.Location = new System.Drawing.Point(16, 81);
            this.timestamp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.timestamp.Name = "timestamp";
            this.timestamp.ReadOnly = true;
            this.timestamp.Size = new System.Drawing.Size(320, 22);
            this.timestamp.TabIndex = 0;
            this.timestamp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // timestampUtc
            // 
            this.timestampUtc.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.timestampUtc.Location = new System.Drawing.Point(16, 111);
            this.timestampUtc.Margin = new System.Windows.Forms.Padding(4);
            this.timestampUtc.Name = "timestampUtc";
            this.timestampUtc.ReadOnly = true;
            this.timestampUtc.Size = new System.Drawing.Size(320, 22);
            this.timestampUtc.TabIndex = 2;
            this.timestampUtc.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 143);
            this.Controls.Add(this.timestampUtc);
            this.Controls.Add(this.timestamp);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainScreen";
            this.Text = "TimeStamp";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox timestamp;
        private System.Windows.Forms.TextBox timestampUtc;
    }
}

