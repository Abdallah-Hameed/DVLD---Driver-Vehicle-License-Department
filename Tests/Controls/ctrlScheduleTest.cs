using DVLDtraining.Global;
using DVLDtraining.Properties;
using DVLDtraining_BusinessLogic;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace DVLDtraining.Tests.Controls
{
    public partial class ctrlScheduleTest : UserControl
    {
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        public enum enMode { AddNew = 0, Update = 1 }

        enMode _Mode = enMode.AddNew;

        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 }

        enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        float _Fees = 0;

        float _RetakeFees = 0;

        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }

            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        gbTestType.Text = "Vision test";
                        pbScheduleTestType.Image = Resources.view;
                        break;

                    case clsTestType.enTestType.WrittenTest:
                        gbTestType.Text = "Written test";
                        pbScheduleTestType.Image = Resources.edit__1_;
                        break;

                    default:
                        gbTestType.Text = "Street test";
                        pbScheduleTestType.Image = Resources.street_racing__1_;
                        break;
                }
            }
        }

        clsLocalLicenseApplication _LocalLicenseApplication;

        int _LocalLicenseApplicationID = -1;

        clsTestAppointment _TestAppointment;

        int _TestAppointmentID = -1;

        bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Test appointment is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSave.Enabled = false;

                return false;
            }

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtpDate.MinDate = DateTime.Now;

            else
                dtpDate.MinDate = _TestAppointment.AppointmentDate;

            dtpDate.Value = _TestAppointment.AppointmentDate;

            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                _RetakeFees = 0;

                lblRetakeAppFees.Text = "0";

                lblRetakeTestAppID.Text = "N/A";
            }

            else
            {
                _RetakeFees = _TestAppointment.RetakeTestApplicationInfo.PaidFees;

                lblRetakeAppFees.Text = _RetakeFees.ToString();

                gbRetakeTestInfo.Visible = true;

                lblTitle.Text = "Schedule retake test";

                lblRetakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
            }

            return true;
        }

        bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && clsLocalLicenseApplication.IsThereAnActiveScheduledTest(_LocalLicenseApplicationID, TestTypeID))
            {
                lblUserMessage.Text = "Person already has an active appointment for this test";

                btnSave.Enabled = false;

                dtpDate.Enabled = false;

                return false;
            }
            return true;
        }

        bool _HandleAppointmentLockedConstraint()
        {
            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible = true;

                lblUserMessage.Text = "Person already sat for the test!";

                dtpDate.Enabled = false;

                btnSave.Enabled = false;

                return false;
            }

            else
                lblUserMessage.Visible = false;

            return true;
        }

        bool _HandlePreviousTestConstraint()
        {
            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    lblUserMessage.Visible = false;
                    return true;

                case clsTestType.enTestType.WrittenTest:
                    if (!_LocalLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot schedule, vision test should be passed first";

                        lblUserMessage.Visible = true;

                        btnSave.Enabled = false;

                        dtpDate.Enabled = false;

                        return false;
                    }

                    else
                    {
                        lblUserMessage.Visible = false;

                        btnSave.Enabled = true;

                        dtpDate.Enabled = true;
                    }

                    return true;

                case clsTestType.enTestType.StreetTest:
                    if (!_LocalLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot schedule, written test should be passed first";

                        lblUserMessage.Visible = true;

                        btnSave.Enabled = false;

                        dtpDate.Enabled = false;

                        return false;
                    }

                    else
                    {
                        lblUserMessage.Visible = false;

                        btnSave.Enabled = true;

                        dtpDate.Enabled = true;
                    }

                    return true;
            }

            return true;
        }

        public void Load(int LocalLicenseApplicationID, int AppointmentID = -1)
        {
            _Mode = (AppointmentID == -1) ? enMode.AddNew : enMode.Update;

            _LocalLicenseApplicationID = LocalLicenseApplicationID;

            _TestAppointmentID = AppointmentID;

            _LocalLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(_LocalLicenseApplicationID);

            if (_LocalLicenseApplication == null)
            {
                MessageBox.Show("Local license is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                btnSave.Enabled = false;

                return;
            }

            _CreationMode = _LocalLicenseApplication.DoesAttendTestType(_TestTypeID) ? enCreationMode.RetakeTestSchedule : enCreationMode.FirstTimeSchedule;

            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                _RetakeFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;

                lblRetakeAppFees.Text = _RetakeFees.ToString();

                gbRetakeTestInfo.Visible = true;

                lblTitle.Text = "Schedule retake test";

                lblRetakeTestAppID.Text = "0";
            }

            else
            {
                _RetakeFees = 0;

                gbRetakeTestInfo.Visible = false;

                lblTitle.Text = "Schedule test";

                lblRetakeAppFees.Text = "0";

                lblRetakeTestAppID.Text = "N/A";
            }

            lblLocalLicenseApplicationID.Text = _LocalLicenseApplicationID.ToString();

            lblClassID.Text = _LocalLicenseApplication.LicenseClassID.ToString();

            lblName.Text = _LocalLicenseApplication.ApplicantFullName;

            lblTrial.Text = _LocalLicenseApplication.TotalTrialsPerTest(TestTypeID).ToString();

            if (_Mode == enMode.AddNew)
            {
                _Fees = clsTestType.Find(TestTypeID).Fees;

                lblFees.Text = _Fees.ToString();

                dtpDate.MinDate = DateTime.Now;

                lblRetakeTestAppID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }

            else
            {
                if (!_LoadTestAppointmentData())
                    return;

                _Fees = _TestAppointment.PaidFees;

                lblFees.Text = _Fees.ToString();
            }

            float TotalFees = _Fees + _RetakeFees;

            lblTotalFees.Text = TotalFees.ToString();

            if (!_HandleActiveTestAppointmentConstraint())
                return;

            if (_HandleAppointmentLockedConstraint())
                return;

            if (_HandlePreviousTestConstraint())
                return;
        }

        bool _HandleRetakeApplication()
        {
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = _LocalLicenseApplication.ApplicantPersonID;

                Application.ApplicationDate = DateTime.Now;

                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;

                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;

                Application.LastStatusDate = DateTime.Now;

                Application.PaidFees = _RetakeFees;

                Application.CreatedByUserID = clsUtil.CurrentUser.UserID;

                if (!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;

                    MessageBox.Show("Failed to create application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return false;
                }

                _TestAppointment.RetakeTestApplicationID = Application.ApplicationID;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;

            _TestAppointment.LocalLicensApplicationID = _LocalLicenseApplicationID;

            _TestAppointment.AppointmentDate = dtpDate.Value;

            _TestAppointment.PaidFees = _Fees;

            _TestAppointment.CreatedByUserID = clsUtil.CurrentUser.UserID;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;

                MessageBox.Show("Data saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
                MessageBox.Show("Error saving data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}