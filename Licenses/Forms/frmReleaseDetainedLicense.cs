using DVLDtraining.Global;
using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Licenses.Forms
{
    public partial class frmReleaseDetainedLicense : Form
    {
        int _SelectedLicenseID = -1;

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();

            _SelectedLicenseID = LicenseID;

            ctrlDriverLicenseWithFilter1.Load(_SelectedLicenseID);

            ctrlDriverLicenseWithFilter1.FilterEnabled = false;
        }

        void ClearDetainInfo()
        {
            lblApplicationFees.Text = "[????]";

            lblCreatedByUser.Text = "[????]";

            lblDetainID.Text = "[????]";

            lblLicenseID.Text = "[????]";

            lblCreatedByUser.Text = "[????]";

            lblDetainDate.Text = "[????]";

            lblFineFees.Text = "[????]";

            lblTotalFees.Text = "[????]";
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;

            lblLicenseID.Text = _SelectedLicenseID.ToString();

            llShowLicensesHistory.Enabled = (_SelectedLicenseID != -1);

            if (_SelectedLicenseID == -1)
            {
                return;
            }

            if (!ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected license is not detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                ClearDetainInfo();

                btnSave.Enabled = false;

                return;
            }

            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees.ToString();

            lblCreatedByUser.Text = clsUtil.CurrentUser.UserName;

            lblDetainID.Text = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainID.ToString();

            lblLicenseID.Text = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.LicenseID.ToString();

            lblCreatedByUser.Text = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DetainedInfo.CreatedByUserInfo.UserName;

            lblDetainDate.Text = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainDate.ToShortDateString();

            lblFineFees.Text = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DetainedInfo.FineFees.ToString();

            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblFineFees.Text)).ToString();

            btnSave.Enabled = true;
        }

        private void frmReleaseDetainedLicense_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseWithFilter1.txtSearchFocus();
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);

            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseID);

            frm.ShowDialog();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this detained  license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int ApplicationID = -1;


            bool IsReleased = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.ReleaseDetainedLicense(clsUtil.CurrentUser.UserID, ref ApplicationID); ;

            lblApplicationID.Text = ApplicationID.ToString();

            if (!IsReleased)
            {
                MessageBox.Show("Faild to to release the detain license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            MessageBox.Show("Detained license released successfully ", "Detained license released", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnSave.Enabled = false;

            ctrlDriverLicenseWithFilter1.FilterEnabled = false;

            llShowLicenseInfo.Enabled = true;
        }
    }
}
