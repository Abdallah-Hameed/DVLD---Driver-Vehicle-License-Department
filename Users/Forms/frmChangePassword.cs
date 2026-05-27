using DVLDtraining.Properties;
using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Users.Forms
{
    public partial class frmChangePassword : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);

        public event DataBackEventHandler DataBack;

        clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();

            _User = clsUser.Find(UserID);

            if( _User != null )
            {
                ctrlPersonInformation1.Load(_User.PersonInfo.PersonID);

                Load();
            }
        }

        void Load()
        {
            if (_User != null)
            {
                lblUserID.Text = _User.UserID.ToString();

                lblUserName.Text = _User.UserName;

                if (_User.IsActive)
                {
                    pbIsActive.Image = Resources.check_mark;
                }

                else
                {
                    pbIsActive.Image = Resources.error;
                }
            }
        }

        bool CheckFields()
        {
            string Message = "This field is required!";

            if (string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                errorProvider1.SetError(txtCurrentPassword, Message);

                return false;
            }

            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                errorProvider1.SetError(txtNewPassword, Message);

                return false;
            }


            if (string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                errorProvider1.SetError(txtConfirmPassword, Message);

                return false;
            }

            if (txtNewPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                MessageBox.Show("Password and confirm password do not match!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Buttons_MouseHover(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor = Color.DimGray;
        }

        private void Buttons_MouseLeave(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor = Color.Transparent;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckFields())
                return;

            if (txtCurrentPassword.Text.Trim() != _User.Password.Trim())
            {
                MessageBox.Show("Current password is wrong!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _User.Password = txtNewPassword.Text.Trim();

            if (_User.Save())
            {
                MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                DataBack?.Invoke(this, _User.UserID);

                this.Close();

                return;
            }

            else
            {
                MessageBox.Show("Error changing password!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
        }
    }
}
