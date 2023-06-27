using System;
using System.Text;
using System.Windows.Forms;

namespace NationalRail
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void logged()
        {
            Menu menuForm = new Menu();
            menuForm.FormClosed += (sender, e) => this.Close(); // Close the login window when the menu window is closed
            menuForm.Show();
            this.Hide(); // Hide the login window instead of closing it immediately
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TODO add your handling code here:
            string user = jTextField1.Text;
            char[] pass = jPasswordField1.Text.ToCharArray();
            string passwordString = new string(pass);

            string hexStringPass = "6e6174696f6e616c7261696c";
            byte[] bytes = new byte[hexStringPass.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int intValue = int.Parse(hexStringPass.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                bytes[i] = (byte)intValue;
            }
            string decodedStringPass = Encoding.UTF8.GetString(bytes);

            string hexStringUser = "61646d696e";
            byte[] bytes2 = new byte[hexStringUser.Length / 2];
            for (int i = 0; i < bytes2.Length; i++)
            {
                int intValue = int.Parse(hexStringUser.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                bytes2[i] = (byte)intValue;
            }
            string decodedStringUser = Encoding.UTF8.GetString(bytes2);

            if (decodedStringUser.Equals(user) && decodedStringPass.Equals(passwordString))
            {
                jLabel_status.Text = "- Autentificare reusita -";
                logged();
            }
            else
            {
                jLabel_status.Text = "- Autentificare esuata -";
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
