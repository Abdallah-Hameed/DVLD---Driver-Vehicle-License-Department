using DVLDtraining.Global;
using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Licenses
{
    public partial class frmIssueLicenseForTheFirstTime : Form
    {
        public frmIssueLicenseForTheFirstTime(int LocalLicenseApplicationID)
        {
            InitializeComponent();

            _LocalLicenseApplicationID = LocalLicenseApplicationID;
        }

        int _LocalLicenseApplicationID;

        clsLocalLicenseApplication _LocalLicenseApplication;

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

        private void frmIssueLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
            txtNotes.Focus();

            _LocalLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(_LocalLicenseApplicationID);

            if (_LocalLicenseApplication == null)
            {

                MessageBox.Show("No applicaiton with ID = " + _LocalLicenseApplicationID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();

                return;
            }


            if (!_LocalLicenseApplication.PassedAllTests())
            {

                MessageBox.Show("Person should pass all tests first.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();

                return;
            }

            int LicenseID = _LocalLicenseApplication.GetActiveLicenseID();

            if (LicenseID != -1) // The person already has a license of the same type
            {
                MessageBox.Show("Person already has License before with License ID = " + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();

                return;

            }

            ctrlLicenseApplicationInfo1.LoadApplicationInfoByLocalLicenseApplicationID(_LocalLicenseApplicationID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseID = _LocalLicenseApplication.IssueLicenseForTheFirstTime(txtNotes.Text.Trim(), clsUtil.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show("License issued successfully with license ID = " + LicenseID.ToString(), "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }

            else
            {
                MessageBox.Show("License was not issued! ", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
