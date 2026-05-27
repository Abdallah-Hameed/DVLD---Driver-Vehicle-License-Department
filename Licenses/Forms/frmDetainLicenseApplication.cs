using DVLDtraining.Global;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Licenses.Forms
{
    public partial class frmDetainLicenseApplication : Form
    {
        public frmDetainLicenseApplication()
        {
            InitializeComponent();
        }

        int _DetainID = -1;

        int _SelectedLicenseID = -1;

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
           
            _DetainID = ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.Detain(Convert.ToSingle(txtFineFees.Text), clsUtil.CurrentUser.UserID);

            if (_DetainID == -1)
            {
                MessageBox.Show("Faild to detain license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblDetainID.Text = _DetainID.ToString();

            MessageBox.Show("License detained successfully with ID = " + _DetainID.ToString(), "License detained", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnSave.Enabled = false;

            ctrlDriverLicenseWithFilter1.FilterEnabled = false;

            txtFineFees.Enabled = false;

            llShowLicenseInfo.Enabled = true;
        }

        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = DateTime.Now.ToShortDateString();

            lblCreatedByUser.Text = clsUtil.CurrentUser.UserName;
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

            if (ctrlDriverLicenseWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected license is already detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSave.Enabled = false;

                return;
            }

            txtFineFees.Focus();

            btnSave.Enabled = true;
        }

        private void frmDetainLicenseApplication_Activated(object sender, EventArgs e)
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

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtFineFees, "Fees cannot be empty!");

                return;
            }

            else
            {
                errorProvider1.SetError(txtFineFees, null);
            }

            if (!clsValidation.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtFineFees, "Invalid Number.");
            }

            else
            {
                errorProvider1.SetError(txtFineFees, null);
            }
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
    }
}