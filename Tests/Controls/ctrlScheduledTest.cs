using DVLDtraining.Properties;
using DVLDtraining_BusinessLogic;
using System.Windows.Forms;

namespace DVLDtraining.Tests.Controls
{
    public partial class ctrlScheduledTest : UserControl
    {
        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        private clsTestType.enTestType _TestTypeID;

        private int _TestID = -1;

        private clsLocalLicenseApplication _LocalLicenseApplication;

        public int TestAppointmentID
        {
            get
            {
                return _TestAppointmentID;
            }
        }

        public int TestID
        {
            get
            {
                return _TestID;
            }
        }

        private int _TestAppointmentID = -1;

        private int _LocalLicenseApplicationID = -1;

        private clsTestAppointment _TestAppointment;

        public clsTestType.enTestType TestTypeID
        {
            set
            {
                _TestTypeID = value;

                switch(_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        pbScheduleTestType.Image = Resources.view;
                        gbTestType.Text = "Vision test";
                        break;

                    case clsTestType.enTestType.WrittenTest:
                        pbScheduleTestType.Image = Resources.edit__1_;
                        gbTestType.Text = "Written test";
                        break;

                    default:
                        pbScheduleTestType.Image = Resources.street_racing__1_;
                        gbTestType.Text = "Street test";
                        break;
                }
            }

            get
            {
                return _TestTypeID;
            }
        }

        public void Load(int TestAppointmentID)
        {
            _TestAppointmentID = TestAppointmentID;

            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Appointment is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                _TestAppointmentID = -1;

                return;
            }

            _TestID = _TestAppointment.TestID;

            _LocalLicenseApplicationID = _TestAppointment.LocalLicensApplicationID;

            _LocalLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(_LocalLicenseApplicationID);

            if (_LocalLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblLocalLicenseApplicationID.Text = _LocalLicenseApplication.LocalLicenseApplicationID.ToString();

            lblClassID.Text = _LocalLicenseApplication.LicenseClassInfo.ClassName;

            lblName.Text = _LocalLicenseApplication.PersonFullName;

            lblTrial.Text = _LocalLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();

            lblDate.Text = (_TestAppointment.AppointmentDate).ToShortDateString();

            lblFees.Text = _TestAppointment.PaidFees.ToString();

            lblTestID.Text = (_TestAppointment.TestID == -1) ? "Not Taken Yet" : _TestAppointment.TestID.ToString();
        }
    }
}