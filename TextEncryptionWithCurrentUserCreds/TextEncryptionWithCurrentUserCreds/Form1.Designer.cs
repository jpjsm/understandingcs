namespace TextEncryptionWithCurrentUserCreds
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.PlainText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EncryptedValueBase64 = new System.Windows.Forms.TextBox();
            this.DecryptedValue = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Plain Text";
            // 
            // PlainText
            // 
            this.PlainText.Location = new System.Drawing.Point(12, 29);
            this.PlainText.Name = "PlainText";
            this.PlainText.Size = new System.Drawing.Size(807, 22);
            this.PlainText.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(267, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Encrypted value (base64 representation)";
            // 
            // EncryptedValueBase64
            // 
            this.EncryptedValueBase64.Location = new System.Drawing.Point(9, 86);
            this.EncryptedValueBase64.Name = "EncryptedValueBase64";
            this.EncryptedValueBase64.ReadOnly = true;
            this.EncryptedValueBase64.Size = new System.Drawing.Size(807, 22);
            this.EncryptedValueBase64.TabIndex = 5;
            // 
            // DecryptedValue
            // 
            this.DecryptedValue.Location = new System.Drawing.Point(9, 155);
            this.DecryptedValue.Name = "DecryptedValue";
            this.DecryptedValue.ReadOnly = true;
            this.DecryptedValue.Size = new System.Drawing.Size(807, 22);
            this.DecryptedValue.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 135);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "Decrypted value";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(350, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 39);
            this.button1.TabIndex = 8;
            this.button1.Text = "Encrypt";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 300);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DecryptedValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EncryptedValueBase64);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PlainText);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Text Encryption With Current User Credentials";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PlainText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox EncryptedValueBase64;
        private System.Windows.Forms.TextBox DecryptedValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}

