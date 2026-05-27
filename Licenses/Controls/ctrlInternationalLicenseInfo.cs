using DVLD_Buisness;
using DVLDtraining.Properties;
using System.IO;
using System.Windows.Forms;

namespace DVLDtraining.Licenses.Controls
{
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        private int _InternationalLicenseID;

        private clsInternationalLicense _InternationalLicense;

        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        public int InternationalLicenseID
        {
            get { return _InternationalLicenseID; }
        }

        private void _LoadPersonImage()
        {
            if (_InternationalLicense.DriverInfo.PersonInfo.Gender == 0)
                pbDriverImage.Image = Resources._6837225;

            else
                pbDriverImage.Image = Resources._6833591;

            string ImagePath = _InternationalLicense.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbDriverImage.Load(ImagePath);

                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void LoadInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;

            _InternationalLicense = clsInternationalLicense.Find(_InternationalLicenseID);

            if (_InternationalLicense == null)
            {
                MessageBox.Show("Could not find Internationa License ID = " + _InternationalLicenseID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                _InternationalLicenseID = -1;

                return;
            }

            lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();

            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();

            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";

            lblLicenseID.Text = _InternationalLicense.IssuedUsingLocalLicenseID.ToString();

            lblFullName.Text = _InternationalLicense.DriverInfo.PersonInfo.FullName;

            lblNationalNo.Text = _InternationalLicense.DriverInfo.PersonInfo.NationalNo;

            lblGendor.Text = _InternationalLicense.DriverInfo.PersonInfo.Gender == 0 ? "Male" : "Female";

            lblDateOfBirth.Text = _InternationalLicense.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();

            lblDriverID.Text = _InternationalLicense.DriverID.ToString();

            lblIssueDate.Text = _InternationalLicense.IssueDate.ToShortDateString();

            lblExpirationDate.Text = _InternationalLicense.ExpirationDate.ToShortDateString();

            _LoadPersonImage();
        }
    }
}