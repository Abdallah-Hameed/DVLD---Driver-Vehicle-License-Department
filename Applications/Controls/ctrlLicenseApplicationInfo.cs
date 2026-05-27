using DVLD_Buisness;
using DVLDtraining.Licenses.Forms;
using DVLDtraining_BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDtraining.Applications.Controls
{
    public partial class ctrlLicenseApplicationInfo : UserControl
    {
        public ctrlLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private clsLocalLicenseApplication _LocalLicenseApplication;

        private int _LocalLicenseApplicationID = -1;

        private int _LicenseID = -1;

        public int LocalLicenseApplicationID
        {
            get
            {
                return _LocalLicenseApplicationID;
            }
        }

        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            _LocalLicenseApplicationID = -1;

            ctrlApplicationBasicInfo1.ResetApplicationInfo();

            lblLocalDrivingApplicationID.Text = "[????]";

            lblAppliedForLicense.Text = "[????]";
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _LicenseID = _LocalLicenseApplication.GetActiveLicenseID();

            llShowLicenseInfo.Enabled = (_LicenseID != -1);

            lblLocalDrivingApplicationID.Text = _LocalLicenseApplication.LocalLicenseApplicationID.ToString();

            lblAppliedForLicense.Text = clsLicenseClass.Find(_LocalLicenseApplication.LicenseClassID).ClassName;

            lblPassedTests.Text = _LocalLicenseApplication.GetPassedTestCount().ToString() + "/3";

            ctrlApplicationBasicInfo1.Load(_LocalLicenseApplication.ApplicationID);
        }

        public void LoadApplicationInfoByLocalLicenseApplicationID(int LocalLicenseApplicationID)
        {
            _LocalLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(LocalLicenseApplicationID);

            if(_LocalLicenseApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();

                MessageBox.Show("Application is not found!", "Not found", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        public void LoadApplicationInfoByApplicationID(int ApplicationID)
        {
            _LocalLicenseApplication = clsLocalLicenseApplication.FindByApplicationID(ApplicationID);
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_LocalLicenseApplication.GetActiveLicenseID());

            frm.ShowDialog();
        }
    }
}
