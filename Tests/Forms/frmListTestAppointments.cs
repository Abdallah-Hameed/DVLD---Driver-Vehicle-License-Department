using DVLD_Buisness;
using DVLDtraining.Properties;
using DVLDtraining_BusinessLogic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Tests.Forms
{
    public partial class frmListTestAppointments : Form
    {
        DataTable _dt;

        int _LocalLicenseApplicationID = -1;

        clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;

        clsTestAppointment _TestAppointment;

        public frmListTestAppointments(int LocalLicenseApplicationID, clsTestType.enTestType TestType)
        {
            InitializeComponent();

            _LocalLicenseApplicationID = LocalLicenseApplicationID;

            _TestType = TestType;
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

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            if (_TestType == clsTestType.enTestType.VisionTest)
            {
                pbScheduleTestType.Image = Resources.view;

                lblTitle.Text = "Vision test appointment";
            }

            else if (_TestType == clsTestType.enTestType.WrittenTest)
            {
                pbScheduleTestType.Image = Resources.edit__1_;

                lblTitle.Text = "Written test appointment";
            }

            else
            {
                pbScheduleTestType.Image = Resources.street_racing__1_;

                lblTitle.Text = "Street test appointment";
            }

            ctrlLicenseApplicationInfo1.LoadApplicationInfoByLocalLicenseApplicationID(_LocalLicenseApplicationID);

            _dt = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalLicenseApplicationID, _TestType);

            dgvTestAppointments.DataSource = _dt;

            lblRecords.Text = dgvTestAppointments.Rows.Count.ToString();

            if(dgvTestAppointments.Rows.Count > 0 )
            {
                dgvTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvTestAppointments.Columns[0].Width = 200;

                dgvTestAppointments.Columns[1].HeaderText = "Appointment date";
                dgvTestAppointments.Columns[1].Width = 350;

                dgvTestAppointments.Columns[2].HeaderText = "Paid fees";
                dgvTestAppointments.Columns[2].Width = 350;

                dgvTestAppointments.Columns[3].HeaderText = "Is locked";
                dgvTestAppointments.Columns[3].Width = 160;
            }
        }

        private void btnAddTestAppointment_Click(object sender, EventArgs e)
        {
            clsLocalLicenseApplication localDrivingLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(_LocalLicenseApplicationID);

            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestType);

            if (LastTest == null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_LocalLicenseApplicationID, _TestType);

                frm1.ShowDialog();

                frmListTestAppointments_Load(null, null);

                return;
            }

            //if person already passed the test s/he cannot retak it.
            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest(LastTest.TestAppointmentInfo.LocalLicensApplicationID, _TestType);

            frm2.ShowDialog();

            frmListTestAppointments_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvTestAppointments.Rows.Count > 0)
            {
                int TestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;

                int Records = (int)dgvTestAppointments.Rows.Count;

                frmScheduleTest frm = new frmScheduleTest(_LocalLicenseApplicationID, _TestType, TestAppointmentID);

                frm.ShowDialog();

                frmListTestAppointments_Load(null, null);

                if (Records < (int)dgvTestAppointments.Rows.Count)
                {
                    clsTestAppointment.LockTestAppointment(TestAppointmentID);

                    frmListTestAppointments_Load(null, null);
                }
            }

            else
            {
                MessageBox.Show("Please add a test appointment first!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dgvTestAppointments.Rows.Count > 0)
            {             
                int TestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;

                frmTakeTest frm = new frmTakeTest(TestAppointmentID, _TestType);

                frm.ShowDialog();

                frmListTestAppointments_Load(null, null);
            }

            else
            {
                MessageBox.Show("Please add a test appointment first!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(dgvTestAppointments.SelectedRows != null)
            {
                clsTestAppointment testapp = clsTestAppointment.Find((int)dgvTestAppointments.CurrentRow.Cells[0].Value);

                takeTestToolStripMenuItem.Enabled = !testapp.IsLocked;

                editToolStripMenuItem.Enabled = !testapp.IsLocked;

                cancelToolStripMenuItem.Enabled = !testapp.IsLocked;
            }
        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel the test appointment with ID = " + dgvTestAppointments.CurrentRow.Cells[0].Value.ToString() + " ?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                clsTestAppointment.LockTestAppointment((int)dgvTestAppointments.CurrentRow.Cells[0].Value);

                frmListTestAppointments_Load(null, null);
            }
        }
    }
}