using DVLD_Buisness;
using DVLDtraining.Global;
using DVLDtraining.Licenses.Forms;
using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Applications.Forms.Renew_License_Application
{
    public partial class frmRenewLicenseApplication : Form
    {
        public frmRenewLicenseApplication()
        {
            InitializeComponent();
        }

        int _NewLicenseID = -1;

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.AutoValidate = AutoValidate.Disable;

            this.Close();
        }

        private void frmRenewLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseWithFilter1.txtSearchFocus();

            lblApplicationDate.Text = DateTime.Now.ToShortDateString();

            lblIssueDate.Text = lblApplicationDate.Text;

            lblExpirationDate.Text = "[????]";

            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees.ToString();

            lblCreatedByUser.Text = clsUtil.CurrentUser.UserName;
        }

        private void ctrlDriverLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            lblOldLicenseID.Text = SelectedLicenseID.ToString();

            llShowLicensesHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)
                return;

            if (ctrlDriverLicenseWithFilter1.SelectedLicenseInfo == null)
                return;

            int DefaultValidityLength = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.LicenseClassIfo.DefaultValidityLength;

            lblExpirationDate.Text = DateTime.Now.AddYears(DefaultValidityLength).ToShortDateString();

            lblLicenseFees.Text = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.LicenseClassIfo.ClassFees.ToString();

            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();

            txtNotes.Text = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.Notes;

            if (!ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected license is not yet expiared, it will expire on: " + ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.ExpirationDate.ToShortDateString(),
                    "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSave.Enabled = false;

                gbNewLicenseInfo.Visible = false;

                llShowLicenseInfo.Visible = false;

                return;
            }

            if (!ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected license is not NOT active, choose an active license.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSave.Enabled = false;

                gbNewLicenseInfo.Visible = false;

                llShowLicenseInfo.Visible = false;

                return;
            }

            gbNewLicenseInfo.Visible = true;

            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to renew the license?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
                    return;

            clsLicense NewLicense = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text.Trim(), clsUtil.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Failed to renew the license!", "Falied", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();

            _NewLicenseID = NewLicense.LicenseID;

            lblRenewedLicenseID.Text = _NewLicenseID.ToString();

            MessageBox.Show("License renewed successfully with ID = " + _NewLicenseID.ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            btnSave.Enabled = false;

            ctrlDriverLicenseWithFilter1.FilterEnabled = false;

            llShowLicenseInfo.Enabled = true;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_NewLicenseID == -1)
                return;

            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID);

            frm.ShowDialog();
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);

            frm.ShowDialog();
        }
    }
}