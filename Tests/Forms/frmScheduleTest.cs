using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Tests.Forms
{
    public partial class frmScheduleTest : Form
    {
        int _LocalLicenseApplicationID = -1;

        clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        int _AppointmentID = -1;

        public frmScheduleTest(int LocalLicenseApplicationID, clsTestType.enTestType TestTypeID, int AppointmentID = -1)
        {
            InitializeComponent();

            _LocalLicenseApplicationID = LocalLicenseApplicationID;

            _TestTypeID = TestTypeID;

            _AppointmentID = AppointmentID;
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

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestTypeID; //To change icon and text 

            ctrlScheduleTest1.Load(_LocalLicenseApplicationID, _AppointmentID);
        }
    }
}