using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Applications.Forms.Local_License
{
    public partial class frmShowLocalLicenseApplicationInfo : Form
    {
        private int _LocalLicenseApplicationID = -1;

        public frmShowLocalLicenseApplicationInfo(int LocalLicenseApplicationID)
        {
            InitializeComponent();

            _LocalLicenseApplicationID = LocalLicenseApplicationID;
        }

        private void frmShowLocalLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            if (_LocalLicenseApplicationID != -1)
                ctrlLicenseApplicationInfo1.LoadApplicationInfoByLocalLicenseApplicationID(_LocalLicenseApplicationID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_MouseHover(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.DimGray;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.Transparent;
        }
    }
}