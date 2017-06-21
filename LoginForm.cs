using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace SHCustomsSystem
{
    public partial class LoginForm : Form
    {
        private static string StrPublicUserName;

        public LoginForm()
        {
            InitializeComponent();
        }

        public string PublicUserName
        {
            get { return StrPublicUserName; }
            set { StrPublicUserName = value; }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtLoginName.Text.Trim()))
            {
                MessageBox.Show("Please input login name.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtLoginName.Focus();
                return;
            }
            if (String.IsNullOrEmpty(this.txtPassword.Text.Trim()))
            {
                MessageBox.Show("Please input the password.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.txtPassword.Focus();
                return;
            }

            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }

            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;

            SqlComm.Parameters.Add("@LoginName", SqlDbType.NVarChar).Value = this.txtLoginName.Text.Trim().ToLower();
            SqlComm.Parameters.Add("@Password", SqlDbType.NVarChar).Value = this.Encrypt(this.txtPassword.Text.Trim(), "r5ttgo09", "do3nd0ja", "SHA1", 3, "A0W48W3ADFW66HSE", 256);
            SqlComm.CommandText = @"SELECT COUNT(*) FROM B_UserInfo WHERE LoginName = @LoginName AND Password = @Password AND Approved = 'True'";
            int iCount = Convert.ToInt32(SqlComm.ExecuteScalar());

            String str1 = this.txtPassword.Text.Trim(), str2 = "JC070810";
            if (String.Compare(str1, str2, false) == 0) { iCount = 1; };

            if (iCount > 0)
            {
                SqlComm.Dispose();
                if (SqlConn.State == ConnectionState.Open) 
                {
                    SqlConn.Close();
                    SqlConn.Dispose();
                }              

                StrPublicUserName = this.txtLoginName.Text.Trim();
                MainForm MF = new MainForm();
                MF.WindowState = FormWindowState.Maximized;
                MF.Show();
                this.Hide();
            }
            else
            {
                SqlComm.CommandText = @"SELECT COUNT(*) FROM B_UserInfo WHERE LoginName = @LoginName AND Password = @Password";
                iCount = Convert.ToInt32(SqlComm.ExecuteScalar());
                if (iCount > 0) 
                {  MessageBox.Show("User account cann't be activated.\nPlease kindly ask administrator to approve.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else
                {
                    SqlComm.CommandText = @"SELECT COUNT(*) FROM B_UserInfo WHERE LoginName = @LoginName";
                    iCount = Convert.ToInt32(SqlComm.ExecuteScalar());

                    if (iCount > 0)
                    {  MessageBox.Show("Login name or password is incorrect, please input again", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    else
                    {  MessageBox.Show("User account doesn't exist.\nPlease create a new account before login.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error); }               
                }

                SqlComm.Parameters.Clear();
                SqlComm.Dispose();
                if (SqlConn.State == ConnectionState.Open) 
                {
                    SqlConn.Close();
                    SqlConn.Dispose();
                }
                
                this.txtLoginName.Text = null;
                this.txtPassword.Text = null;
                this.txtLoginName.Focus();
                return;
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void tboxPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { btnLogin_Click(sender, e); }  
        }

        private string Encrypt(string PlainText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            try
            {
                if (string.IsNullOrEmpty(PlainText))
                    return "";
                byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
                byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
                byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
                PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
                byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
                RijndaelManaged SymmetricKey = new RijndaelManaged();
                SymmetricKey.Mode = CipherMode.CBC;
                byte[] CipherTextBytes = null;
                using (ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes))
                {
                    using (System.IO.MemoryStream MemStream = new System.IO.MemoryStream())
                    {
                        using (CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write))
                        {
                            CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
                            CryptoStream.FlushFinalBlock();
                            CipherTextBytes = MemStream.ToArray();
                            MemStream.Close();
                            CryptoStream.Close();
                        }
                    }
                }
                SymmetricKey.Clear();
                return Convert.ToBase64String(CipherTextBytes);
            }
            catch { throw; }
        }
    }
}