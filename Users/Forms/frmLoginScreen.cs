using DVLDtraining.Global;
using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLDtraining
{
    public partial class frmLoginScreen : Form
    {
        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";

            if(clsUtil.GetStoredCredential(ref Username,ref Password))
            {
                txtUserName.Text = Username;

                txtPassword.Text = Password; 

                chkRememberme.Checked = true;
            }

            else
            {
                chkRememberme.Checked = false;
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            clsUser User = clsUser.Find(txtUserName.Text.Trim(), txtPassword.Text.Trim());

            if (User != null)
            {
                if (User.IsActive)
                {
                    if (chkRememberme.Checked)
                    {
                        clsUtil.RememberUsernameAndPassword(txtUserName.Text, txtPassword.Text);
                    }

                    else
                    {
                        clsUtil.RememberUsernameAndPassword("", "");
                    }

                    clsUtil.CurrentUser = clsUser.Find(txtUserName.Text.Trim(), txtPassword.Text.Trim());

                    this.Hide();

                    frmMain frm = new frmMain(this);

                    frm.ShowDialog();
                }

                else
                {
                    txtUserName.Focus();

                    MessageBox.Show("This account is inactive!", "Inactive account", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            else
            {
                MessageBox.Show("Username or password is wrong!", "Wrong credetial", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
        }

        private void llForgetPassword_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This service will be enabled later on!", "Disabled service", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtUserName_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (txtUserName.Text == "Username")
                txtUserName.Text = "";
        }

        private void txtPassword_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (txtPassword.Text == "Password")
                txtPassword.Text = "";
        }

        private void pbClose_MouseHover(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.DimGray;
        }

        private void pbClose_MouseLeave(object sender, EventArgs e)
        {
            pbClose.BackColor = Color.Transparent;
        }
    }
}
