using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetTimeStamp
{
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateTimestamp();
        }

        private void UpdateTimestamp()
        {
            DateTime now = DateTime.Now;
            timestamp.Text = now.ToString("yyyy-MM-dd HH:mm:ss.fff zz");
            timestampUtc.Text = now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
            timestamp.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateTimestamp();
        }
    }
}
