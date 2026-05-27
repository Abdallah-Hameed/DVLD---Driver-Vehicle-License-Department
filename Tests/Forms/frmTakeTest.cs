using DVLD_Buisness;
using DVLDtraining.Global;
using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Tests.Forms
{
    public partial class frmTakeTest : Form
    {
        public frmTakeTest(int AppointmentID, clsTestType.enTestType TestType)
        {
            InitializeComponent();

            _AppointmentID = AppointmentID;

            _TestType = TestType;
        }

        int _AppointmentID = -1;

        clsTestType.enTestType _TestType;

        clsTest _Test;

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestTypeID = _TestType;

            ctrlScheduledTest1.Load(_AppointmentID);

            if (ctrlScheduledTest1.TestAppointmentID == -1) //there is no appointment
            {
                btnSave.Enabled = false;
            }

            else
                btnSave.Enabled = true;

            int TestID = ctrlScheduledTest1.TestID;

            if (TestID != -1)
            {
                _Test = clsTest.Find(TestID);

                if(_Test.TestResult)
                {
                    rbPass.Checked = true;
                }

                else
                {
                    rbFail.Checked = true;
                }

                txtNotes.Text = _Test.Notes;

                lblUserMessage.Visible = true;

                rbFail.Enabled = false; //we cannot change the result

                rbPass.Enabled = false;
            }

            else
            {
                _Test = new clsTest();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            _Test.TestAppointmentID = _AppointmentID;

            _Test.TestResult = rbPass.Checked;

            _Test.Notes = txtNotes.Text.Trim();

            _Test.CreatedByUserID = clsUtil.CurrentUser.UserID;

            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnSave.Enabled = false;

                rbPass.Enabled = false;

                rbFail.Enabled = false;
            }

            else
                MessageBox.Show("Error saving data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}