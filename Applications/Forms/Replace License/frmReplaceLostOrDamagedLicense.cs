using DVLD_Buisness;
using DVLDtraining.Global;
using DVLDtraining.Licenses.Forms;
using DVLDtraining_BusinessLogic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static DVLD_Buisness.clsLicense;

namespace DVLDtraining.Applications.License_Replacement
{
    public partial class frmReplaceLostOrDamagedLicense : Form
    {
        public frmReplaceLostOrDamagedLicense()
        {
            InitializeComponent();
        }

        private void Buttons_MouseHover(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor = Color.DimGray;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor = Color.Transparent;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.AutoValidate = AutoValidate.Disable;

            this.Close();
        }

        int _NewLicenseID = -1;

        int _GetApplicationTypeID()
        {
            if (rbDamagedLicense.Checked)
                return (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;

            else
                return (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;
        }

        private enIssueReason _GetIssueReason()
        {
            if (rbDamagedLicense.Checked)

                return enIssueReason.DamagedReplacement;
            else
                return enIssueReason.LostReplacement;
        }

        private void frmReplaceLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();

            lblCreatedByUser.Text = clsUtil.CurrentUser.UserName;

            rbDamagedLicense.Checked = true;
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replace for damaged license";

            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replace for lost license";

            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void ctrlDriverLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            lblOldLicenseID.Text = SelectedLicenseID.ToString();

            if (SelectedLicenseID == -1)
                return;

            if(!ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected license is NOT active!", "Inactive license", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSave.Enabled = false;

                return;
            }

            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue a replacement for the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            clsLicense NewLicense = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.Replace(_GetIssueReason(), clsUtil.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to issue a replacemnet for this license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();

            _NewLicenseID = NewLicense.LicenseID;

            lblRreplacedLicenseID.Text = _NewLicenseID.ToString();

            MessageBox.Show("Licensed replaced successfully with ID=" + _NewLicenseID.ToString(), "License issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnSave.Enabled = false;

            gbReplaceFor.Visible = false;

            ctrlDriverLicenseWithFilter1.FilterEnabled = false;

        }

        private void frmReplaceLostOrDamagedLicense_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseWithFilter1.txtSearchFocus();
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
