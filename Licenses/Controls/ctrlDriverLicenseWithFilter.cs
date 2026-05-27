using DVLD_Buisness;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Licenses.Controls
{
    public partial class ctrlDriverLicenseWithFilter : UserControl
    {
        public ctrlDriverLicenseWithFilter()
        {
            InitializeComponent();
        }

        public event Action<int> OnLicenseSelected;

        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;

            if (handler != null)
            {
                handler(LicenseID);
            }
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }

            set
            {
                _FilterEnabled = value;

                btnShowInfo.Enabled = _FilterEnabled;
            }
        }

        int _LicenseID = -1;

        public int LicenseID
        {
            get { return ctrlDriverLicenseInfo1.LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        {
            get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; }
        }

        public void Load(int LicenseID)
        {
            txtSearch.Text = LicenseID.ToString();

            ctrlDriverLicenseInfo1.LoadInfo(LicenseID);

            _LicenseID = ctrlDriverLicenseInfo1.LicenseID;

            if (OnLicenseSelected != null && _FilterEnabled)
                OnLicenseSelected(_LicenseID);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if (e.KeyChar == (char)13)
            {
                btnShowInfo.PerformClick();
            }
        }

        private void btnShowInfo_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show("Some fields are not valide!", "Invalidated", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txtSearch.Focus();

                return;
            }

            _LicenseID = int.Parse(txtSearch.Text.Trim());

            Load(_LicenseID);
        }

        public void txtSearchFocus()
        {
            txtSearch.Focus();
        }

        private void txtSearch_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                e.Cancel = true;

                errorProvider1.SetError(txtSearch, "This field is required!");
            }

            else
            {
                errorProvider1.SetError(txtSearch, null);
            }
        }

        private void btnShowInfo_MouseHover(object sender, EventArgs e)
        {
            btnShowInfo.BackColor = Color.DimGray;
        }

        private void btnShowInfo_MouseLeave(object sender, EventArgs e)
        {
            btnShowInfo.BackColor = Color.Transparent;
        }
    }
}