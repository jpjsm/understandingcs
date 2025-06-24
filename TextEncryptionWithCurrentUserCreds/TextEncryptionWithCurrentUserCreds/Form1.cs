using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEncryptionWithCurrentUserCreds
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string Base64EncryptedWithDataProtection(string text, byte[] entropy)
        {
            return Convert.ToBase64String(
                ProtectedData.Protect(Encoding.Unicode.GetBytes(text), entropy, DataProtectionScope.CurrentUser));
        }

        public static string StringDecryptedWithDataProtectionFromBase64(string base64EncryptedValue, byte[] entropy)
        {
            return new string(Encoding.Unicode.GetChars(
                ProtectedData.Unprotect(Convert.FromBase64String(base64EncryptedValue), entropy, DataProtectionScope.CurrentUser)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EncryptedValueBase64.Text = Base64EncryptedWithDataProtection(
                PlainText.Text,
                null);

            DecryptedValue.Text = StringDecryptedWithDataProtectionFromBase64(
                EncryptedValueBase64.Text,
                null);

            if (PlainText.Text == DecryptedValue.Text)
            {
                MessageBox.Show("Successfull encryption.", "Encryption Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("FAILED encryption.", "Encryption Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
