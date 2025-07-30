namespace WindowsMediaPlayer
{
    partial class Player
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Player));
            this.axplayer = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this.axplayer)).BeginInit();
            this.SuspendLayout();
            // 
            // axplayer
            // 
            this.axplayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axplayer.Enabled = true;
            this.axplayer.Location = new System.Drawing.Point(0, 0);
            this.axplayer.Name = "axplayer";
            this.axplayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axplayer.OcxState")));
            this.axplayer.Size = new System.Drawing.Size(576, 350);
            this.axplayer.TabIndex = 9;
            // 
            // Player
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 350);
            this.Controls.Add(this.axplayer);
            this.Name = "Player";
            this.Text = "Player";
            ((System.ComponentModel.ISupportInitialize)(this.axplayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer axplayer;
    }
}