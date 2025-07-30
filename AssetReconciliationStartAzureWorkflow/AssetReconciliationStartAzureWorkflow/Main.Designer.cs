namespace AssetReconciliationStartAzureWorkflow
{
    partial class Main
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.defaultEndpointProtocols = new System.Windows.Forms.ComboBox();
            this.accountNames = new System.Windows.Forms.ComboBox();
            this.accountKeys = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.storageConnectionString = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.storageHierarchy = new System.Windows.Forms.TreeView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(314, 317);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 121);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Message";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "DCM"});
            this.comboBox2.Location = new System.Drawing.Point(58, 87);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(192, 21);
            this.comboBox2.TabIndex = 5;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(58, 52);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(192, 20);
            this.textBox1.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "HydrationStart"});
            this.comboBox1.Location = new System.Drawing.Point(58, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(192, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "service";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "path";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "type";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(372, 445);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(191, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Send Message";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Default Endpoints Protocol";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Account Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Account Key";
            // 
            // defaultEndpointProtocols
            // 
            this.defaultEndpointProtocols.FormattingEnabled = true;
            this.defaultEndpointProtocols.Items.AddRange(new object[] {
            "https"});
            this.defaultEndpointProtocols.Location = new System.Drawing.Point(146, 27);
            this.defaultEndpointProtocols.Name = "defaultEndpointProtocols";
            this.defaultEndpointProtocols.Size = new System.Drawing.Size(207, 21);
            this.defaultEndpointProtocols.TabIndex = 5;
            // 
            // accountNames
            // 
            this.accountNames.FormattingEnabled = true;
            this.accountNames.Items.AddRange(new object[] {
            "assetreconciliationstore"});
            this.accountNames.Location = new System.Drawing.Point(146, 60);
            this.accountNames.Name = "accountNames";
            this.accountNames.Size = new System.Drawing.Size(207, 21);
            this.accountNames.TabIndex = 6;
            // 
            // accountKeys
            // 
            this.accountKeys.FormattingEnabled = true;
            this.accountKeys.Items.AddRange(new object[] {
            "oDXZ1PIW/zDaiajhFYWs6frhk3ae7enItX1razln0qk0Oj3SSg6mbnfWzoBbn9D8FBSmwsseqK3QHtGag" +
                "9KlvA=="});
            this.accountKeys.Location = new System.Drawing.Point(146, 93);
            this.accountKeys.Name = "accountKeys";
            this.accountKeys.Size = new System.Drawing.Size(207, 21);
            this.accountKeys.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Connection String";
            // 
            // storageConnectionString
            // 
            this.storageConnectionString.Location = new System.Drawing.Point(146, 130);
            this.storageConnectionString.Name = "storageConnectionString";
            this.storageConnectionString.Size = new System.Drawing.Size(469, 20);
            this.storageConnectionString.TabIndex = 9;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.storageConnectionString);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.accountKeys);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.accountNames);
            this.groupBox2.Controls.Add(this.defaultEndpointProtocols);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(621, 197);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Storage options";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(146, 168);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Test connection";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // storageHierarchy
            // 
            this.storageHierarchy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.storageHierarchy.Location = new System.Drawing.Point(658, 23);
            this.storageHierarchy.Name = "storageHierarchy";
            this.storageHierarchy.Size = new System.Drawing.Size(263, 186);
            this.storageHierarchy.TabIndex = 11;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 492);
            this.Controls.Add(this.storageHierarchy);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Main";
            this.Text = "InventoryReconciliation Workflow Test";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox defaultEndpointProtocols;
        private System.Windows.Forms.ComboBox accountNames;
        private System.Windows.Forms.ComboBox accountKeys;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox storageConnectionString;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TreeView storageHierarchy;
    }
}

