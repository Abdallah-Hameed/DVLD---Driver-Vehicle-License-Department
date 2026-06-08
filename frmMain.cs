using DVLDtraining.Applications.Forms.International_License;
using DVLDtraining.Applications.Forms.Local_License;
using DVLDtraining.Applications.Forms.Renew_License_Application;
using DVLDtraining.Applications.License_Replacement;
using DVLDtraining.Applications.New_License.Local_License;
using DVLDtraining.Drivers;
using DVLDtraining.Global;
using DVLDtraining.Licenses.Forms;
using DVLDtraining.People.Forms;
using DVLDtraining.Tests;
using DVLDtraining.Users;
using DVLDtraining.Users.Forms;
using DVLDtraining_BusinessLogic;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining
{
    public partial class frmMain : Form
    {
        frmLoginScreen _frmLogin;

        public frmMain()
        {
            InitializeComponent();
        }

        public frmMain(frmLoginScreen frm)
        {
            InitializeComponent();

            _frmLogin = frm;
        }

        void ShowMainButtons(bool Visible)
        {
            btnApplications.Visible = Visible;

            btnManagePeople.Visible = Visible;

            btnManageDrivers.Visible = Visible;

            btnManageUsers.Visible = Visible;

            btnAccountSettings.Visible = Visible;

            btnClose.Visible = Visible;
        }

        void SetPanelLocation(Panel panel1)
        {
            panel1.Location = new Point((this.ClientSize.Width - panel1.Width) / 2, (this.ClientSize.Height - panel1.Height) / 2);
        }

        private void btnApplications_Click(object sender, EventArgs e)
        {
            pnApplication.Visible = true;

            SetPanelLocation(pnApplication);

            pbArrow1.Visible = true;

            lblTheLabel1.Visible = true;

            lblTheLabel1.Text = "Applications";

            ShowMainButtons(false);
        }

        private void btnDrivingLicensesServices_Click(object sender, EventArgs e)
        {
            pnApplication.Visible = false;

            pnLicensesServices.Visible = true;

            SetPanelLocation(pnLicensesServices);

            pbArrow2.Visible = true;

            lblTheLabel2.Visible = true;

            lblTheLabel2.Text = "Licenses Services";
        }

        private void btnManageApplications_Click(object sender, EventArgs e)
        {
            pnApplication.Visible = false;

            pnManageApplications.Visible = true;

            SetPanelLocation(pnManageApplications);

            pbArrow2.Visible = true;

            lblTheLabel2.Visible = true;

            lblTheLabel2.Text = "Manage Applications";
        }

        private void btnNewLicense_Click(object sender, EventArgs e)
        {
            pnLicensesServices.Visible = false;

            pnNewLicense.Visible = true;

            SetPanelLocation(pnNewLicense);

            pbArrow3.Visible = true;

            lblTheLabel3.Visible = true;

            lblTheLabel3.Text = "New License";
        }

        private void btnBackToMain_Click(object sender, EventArgs e)
        {
            pnApplication.Visible = false;

            ShowMainButtons(true);

            pbArrow1.Visible = false;

            lblTheLabel1.Visible = false;
        }

        private void btnBackFromLicensesServicesToApplications_Click(object sender, EventArgs e)
        {
            pnLicensesServices.Visible = false;

            pnApplication.Visible = true;

            pbArrow2.Visible = false;

            lblTheLabel2.Visible = false;
        }

        private void btnBackToLicensesServices_Click(object sender, EventArgs e)
        {
            pnNewLicense.Visible = false;

            pnLicensesServices.Visible = true;

            pbArrow3.Visible = false;

            lblTheLabel3.Visible = false;
        }

        private void btnBackFromManageApplicationsToApplications_Click(object sender, EventArgs e)
        {
            pnApplication.Visible = true;

            pnManageApplications.Visible = false;

            pbArrow2.Visible = false;

            lblTheLabel2.Visible = false;
        }

        private void btnDetainLicenses_Click(object sender, EventArgs e)
        {
            pnDetainLicense.Visible = true;

            pnApplication.Visible = false;

            SetPanelLocation(pnDetainLicense);

            pbArrow2.Visible = true;

            lblTheLabel2.Visible = true;

            lblTheLabel2.Text = "Detain License";
        }

        private void btnBackFromDetainLicenseToApplications_Click(object sender, EventArgs e)
        {
            pnApplication.Visible = true;

            pnDetainLicense.Visible = false;

            pbArrow2.Visible = false;

            lblTheLabel2.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ShowMainButtons(false);

            SetPanelLocation(pnLeave);

            pnLeave.Visible = true;
        }

        private void btnAccountSettings_Click(object sender, EventArgs e)
        {
            ShowMainButtons(false);

            pnApplication.Visible = false;

            pnAccountInfo.Visible = true;

            SetPanelLocation(pnAccountInfo);

            pbArrow1.Visible = true;

            lblTheLabel1.Visible = true;

            lblTheLabel1.Text = "Account Settings";
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            pbArrow1.Visible = false;

            pbArrow2.Visible = false;

            pbArrow3.Visible = false;

            lblTheLabel1.Visible = false;

            lblTheLabel2.Visible = false;

            lblTheLabel3.Visible = false;

            SetPanelLocation(pnApplication);

            if (clsUtil.CurrentUser != null)
                lblCurrentUser.Text = clsUtil.CurrentUser.UserName;

            else
            {
                MessageBox.Show("No user logged in!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Application.Exit();
            }

            timer1.Interval = 1000;

            timer1.Tick += timer1_Tick;

            timer1.Start();

            lblTimer.Text = DateTime.Now.ToString("hh:mm tt");
        }

        private void btnBackFromAccountSettingsToMain_Click(object sender, EventArgs e)
        {
            ShowMainButtons(true);

            pnAccountInfo.Visible = false;

            pbArrow1.Visible = false;

            lblTheLabel1.Visible = false;
        }

        private void btnYesLeave_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnNoLeave_Click(object sender, EventArgs e)
        {
            ShowMainButtons(true);

            pnLeave.Visible = false;
        }

        private void btnManagePeople_Click(object sender, EventArgs e)
        {
            frmManagePeople frm = new frmManagePeople();

            frm.ShowDialog();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            frmManageUsers frm = new frmManageUsers();

            frm.ShowDialog();
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are ypu sure you want to sign out?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (clsUtil.CurrentUser.UserID != -1)
            {
                clsUtil.CurrentUser = null;

                if (_frmLogin != null)
                    _frmLogin.Show();

                this.Close();
            }

            else
                MessageBox.Show("There is no user logged in!", "User is not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (clsUtil.CurrentUser.UserID != -1)
            {
                frmChangePassword frm = new frmChangePassword(clsUtil.CurrentUser.UserID);

                frm.ShowDialog();
            }

            else
                MessageBox.Show("There is no user logged in!", "User is not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnCurrentUserInfo_Click(object sender, EventArgs e)
        {
            if (clsUtil.CurrentUser.UserID != -1)
            {
                frmShowUserInfo frm = new frmShowUserInfo(clsUtil.CurrentUser.UserID);

                frm.ShowDialog();
            }

            else
                MessageBox.Show("There is no user logged in!", "User is not found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = DateTime.Now.ToString("hh:mm tt");
        }

        private void lblMyName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Developer info");
        }

        private void btnManageApplicationTypes_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frm = new frmManageApplicationTypes();

            frm.ShowDialog();
        }

        private void btnManageTestTypes_Click(object sender, EventArgs e)
        {
            frmManageTestTypes frm = new frmManageTestTypes();

            frm.ShowDialog();
        }

        private void btnLocalLicense_Click(object sender, EventArgs e)
        {
            frmAddUpdateNewLocalLicenseApplication frm = new frmAddUpdateNewLocalLicenseApplication();

            frm.ShowDialog();
        }

        private void btnLocalLicenseApplications_Click(object sender, EventArgs e)
        {
            frmManageLocalLicenseApplications frm = new frmManageLocalLicenseApplications();

            frm.ShowDialog();
        }

        private void btnRetakeTest_Click(object sender, EventArgs e)
        {
            frmManageLocalLicenseApplications frm = new frmManageLocalLicenseApplications();

            frm.ShowDialog();
        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            frmRenewLicenseApplication frm = new frmRenewLicenseApplication();

            frm.ShowDialog();
        }

        private void btnReplacement_Click(object sender, EventArgs e)
        {
            frmReplaceLostOrDamagedLicense frm = new frmReplaceLostOrDamagedLicense();

            frm.ShowDialog();
        }

        private void btnManageDrivers_Click(object sender, EventArgs e)
        {
            frmManageDrivers frm = new frmManageDrivers();

            frm.ShowDialog();
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

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicenseApplication frm = new frmDetainLicenseApplication();

            frm.ShowDialog();
        }

        private void btnReleaseDetainedLicense2_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();

            frm.ShowDialog();
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();

            frm.ShowDialog();
        }

        private void btnManageDetainedLicenses_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicenses frm = new frmManageDetainedLicenses();

            frm.ShowDialog();
        }

        private void btnNewInternationalLicense_Click(object sender, EventArgs e)
        {
            frmAddNewInternationalLicense frm = new frmAddNewInternationalLicense();

            frm.ShowDialog();
        }

        private void btnInternationalLicenseApplications_Click(object sender, EventArgs e)
        {
            frmManageInternationalLicenses frm = new frmManageInternationalLicenses();

            frm.ShowDialog();
        }
    }
}