using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace eCustoms
{
    public partial class LoginForm : Form
    {
        private DataTable userInfoList = new DataTable();
        private int numberOfTry = 0;

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

        private void LoginForm_Load(object sender, EventArgs e)
        {
            String version1 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            SqlConnection SqlConn = new SqlConnection(SqlLib.StrSqlConnection);
            if (SqlConn.State == ConnectionState.Closed) { SqlConn.Open(); }

            SqlCommand SqlComm = new SqlCommand();
            SqlComm.Connection = SqlConn;


            SqlComm.CommandText = @"SELECT * FROM " + FUVs.tableOfUsers;
            SqlDataAdapter oneAdapter = new SqlDataAdapter();
            oneAdapter.SelectCommand = SqlComm;

            oneAdapter.Fill(userInfoList);
            SqlComm.Dispose();
            oneAdapter.Dispose();

            if (SqlConn.State == ConnectionState.Open) {SqlConn.Close();  }
            SqlConn.Dispose();

            String SSOnum = Environment.UserName;
            DataRow[] dtUsers = userInfoList.Select("[SSO] = '" + SSOnum + "'");
            
            if (dtUsers.Length > 0 )
            {
                this.txtLoginName.Text = dtUsers[0]["LoginName"].ToString();
                this.txtPassword.Text = RijndaelAlgorithm.Decrypt(dtUsers[0]["Password"].ToString());
                       
            }
            else
            {
                this.txtLoginName.Text = "";
 
            }

            numberOfTry = 3;
            
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtLoginName.Text)) //help to locate input box based on user's name
            {
                this.txtLoginName.Focus();
            }
            else
            {
                this.txtPassword.Focus();
                this.btnLogin.Focus();
            }
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
       

            String passWord = RijndaelAlgorithm.Encrypt(this.txtPassword.Text.Trim());
            String userName = this.txtLoginName.Text.Trim().ToLower();

            DataRow[] dtUsers = userInfoList.Select("[LoginName] = '" + userName + "' And [Password] = '" + passWord + "' AND Approved = 'True'");



            if (dtUsers.Length  > 0)
            {
                var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                DateTime buildDateTime = new DateTime(2000, 1, 1).Add(new TimeSpan(
                                TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
                                TimeSpan.TicksPerSecond * 2 * version.Revision)); /* seconds since midnight,
                                                     (multiply by 2 to get original) */
                if (buildDateTime < Convert.ToDateTime(dtUsers[0]["VersionTime"])) //Compare with allowed version build time
                {
                    MessageBox.Show("The version of this program is out dated, please install the latest version. Application is closing......", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }

                StrPublicUserName = userName;
                MainForm MF = new MainForm();
                MF.WindowState = FormWindowState.Maximized;
                MF.Show();
                this.Hide();
            }
            else
            {
                numberOfTry -= 1;
                               
                if (numberOfTry == 0)
                {
                    MessageBox.Show("Failed 3 times. Application is closing......", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                    this.Close();
                }

                MessageBox.Show("Either Login name or password is wrong, please input again", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtPassword.Focus();
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


       
    }
}
