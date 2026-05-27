using DVLD_Buisness;
using DVLDtraining.Global;
using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Licenses.Forms
{
    public partial class frmAddNewInternationalLicense : Form
    {
        public frmAddNewInternationalLicense()
        {
            InitializeComponent();
        }

        int _InternationalLicenseID = -1;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            lblLocalLicenseID.Text = SelectedLicenseID.ToString();

            llShowLicensesHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)
            {
                return;
            }

            //check the license class, person could not issue international license without having
            //normal license of class 3.

            if (ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.LicenseClass != 3)
            {
                MessageBox.Show("Selected license should be Class 3, select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            int ActiveInternaionalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DriverID);

            if (ActiveInternaionalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternaionalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                llShowNewLicenseInfo.Enabled = true;

                _InternationalLicenseID = ActiveInternaionalLicenseID;

                btnSave.Enabled = false;

                return;
            }

            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsInternationalLicense InternationalLicense = new clsInternationalLicense();

            InternationalLicense.ApplicantPersonID = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;

            InternationalLicense.ApplicationDate = DateTime.Now;

            InternationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed;

            InternationalLicense.LastStatusDate = DateTime.Now;

            InternationalLicense.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees;

            InternationalLicense.CreatedByUserID = clsUtil.CurrentUser.UserID;

            InternationalLicense.DriverID = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DriverID;

            InternationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.LicenseID;

            InternationalLicense.IssueDate = DateTime.Now;

            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);

            InternationalLicense.CreatedByUserID = clsUtil.CurrentUser.UserID;

            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to issue international license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = InternationalLicense.ApplicationID.ToString();

            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;

            lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();

            MessageBox.Show("International license issued successfully with ID=" + InternationalLicense.InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnSave.Enabled = false;

            ctrlDriverLicenseWithFilter1.FilterEnabled = false;

            llShowNewLicenseInfo.Enabled = true;
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);

            frm.ShowDialog();
        }

        private void llShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);

            frm.ShowDialog();
        }

        private void frmAddNewInternationalLicense_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseWithFilter1.txtSearchFocus();
        }

        private void frmAddNewInternationalLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();

            lblIssueDate.Text = lblApplicationDate.Text;

            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();

            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();

            lblCreatedByUser.Text = clsUtil.CurrentUser.UserName;
        }

        private void Buttons_MouseHover(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor = Color.DimGray;
        }

        private void Buttons_MouseLeave(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor =Color.Transparent;
        }
    }
}
