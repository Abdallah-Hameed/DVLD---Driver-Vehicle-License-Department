using DVLDTrainin_BusinessLogic;
using DVLDtraining.Global;
using DVLDtraining.People.Forms;
using DVLDtraining.Users.Forms;
using DVLDtraining_BusinessLogic;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Users
{
    public partial class frmAddUpdateUser : Form
    {
        enum enMode { Add = 0, Update = 1 };

        enMode _Mode = enMode.Add;

        clsUser _User;

        clsPerson _Person;

        public frmAddUpdateUser()
        {
            InitializeComponent();

            _Mode = enMode.Add;

            ctrlPersonInfoWithFilter1.evShowInfo += ctrlPersonInfoWithFilter_ShowPersonInfo;

            _User = new clsUser();

            ctrlPersonInfoWithFilter1.FilterEnable(true);
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _User = clsUser.Find(UserID);

            lblUserID.Text = _User.UserID.ToString();

            txtUserName.Text = _User.UserName;

            chkIsActive.Checked = _User.IsActive;

            ShowChangePassword(true);

            lblAddUpdateUser.Text = "Update user info";

            if (_User == null)
            {
                MessageBox.Show("User is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Mode = enMode.Update;

            ctrlPersonInfoWithFilter1.evShowInfo += ctrlPersonInfoWithFilter_ShowPersonInfo;

            ctrlPersonInfoWithFilter1.Load(_User.PersonID);
        }

        public delegate void DataBackEventHandler(object sender, int UserID);

        public event DataBackEventHandler DataBack;

        bool CheckFields()
        {
            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                errorProvider1.SetError(txtUserName, "This field is required!");

                return false;
            }

            if (txtPassword.Visible)
            {
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    errorProvider1.SetError(txtPassword, "This field is required!");

                    return false;
                }

                if (txtConfirmPassword.Visible)
                {
                    if (string.IsNullOrEmpty(txtConfirmPassword.Text))
                    {
                        errorProvider1.SetError(txtConfirmPassword, "This field is required!");

                        return false;
                    }

                    if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
                    {
                        MessageBox.Show("Password and confirm password do not match!", "Wrong", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return false;
                    }
                }
            }

            return true;
        }

        void ShowChangePassword(bool IsVisible)
        {
            llChangePassword.Visible = IsVisible;

            pnPassword.Visible = !IsVisible;
        }

        private void ChangePassword_DataBack(object sender, int UserID)
        {
            _User = clsUser.Find(_User.UserID);

            txtPassword.Text = _User.Password;
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            if(_User != null)
            {
                ctrlPersonInfoWithFilter1.FilterEnable(true);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckFields())
            {
                return;
            }

            _User.UserName = txtUserName.Text;

            _User.Password = txtPassword.Text;

            _User.IsActive = chkIsActive.Checked;

            if (_Mode == enMode.Add && clsUser.IsUserExists(_User.UserName))
            {
                errorProvider1.SetError(txtUserName, "Username is already used");

                return;
            }

            else
            {
                if (_User.Save())
                {
                    MessageBox.Show("User saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lblUserID.Text = _User.UserID.ToString();

                    lblAddUpdateUser.Text = "Update user info";

                    ShowChangePassword(true);

                    DataBack?.Invoke(this, _User.UserID);
                }

                else
                {
                    MessageBox.Show("Error saving data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void llChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(_User.UserID);

            frm.DataBack += ChangePassword_DataBack;

            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Buttons_MouseHover(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            button.BackColor = Color.DimGray;
        }

        private void Buttons_MouseLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            button.BackColor = Color.Transparent;
        }

        private void ctrlPersonInfoWithFilter_ShowPersonInfo(object sender, clsPerson Person)
        {
            if (Person == null) return;

            if (_Mode == enMode.Update) return;

            _Person = Person;

            _User = clsUser.FindByPersonID(Person.PersonID);

            if (_User != null)
            {
                lblUserID.Text = _User.UserID.ToString();

                txtUserName.Text = _User.UserName;

                chkIsActive.Checked = _User.IsActive;

                ShowChangePassword(true);

                lblAddUpdateUser.Text = "Update user info";

                _Mode = enMode.Update;
            }
            else
            {
                _User = new clsUser();

                _User.PersonID = Person.PersonID;

                lblUserID.Text = "-1";

                ShowChangePassword(false);

                _Mode = enMode.Add;
            }
        }
    }
}