using DVLDtraining.Applications.New_License.Local_License;
using DVLDtraining.Global;
using DVLDtraining.Licenses;
using DVLDtraining.Licenses.Forms;
using DVLDtraining.Tests.Forms;
using DVLDtraining_BusinessLogic;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Applications.Forms.Local_License
{
    public partial class frmManageLocalLicenseApplications : Form
    {
        public frmManageLocalLicenseApplications()
        {
            InitializeComponent();
        }

        DataTable dt;

        void _Refresh()
        {
            dt = clsLocalLicenseApplication.GetAllLocalLicenseApplications();

            dgvLocalLicenseApplications.DataSource = dt;

            lblRecords.Text = dgvLocalLicenseApplications.Rows.Count.ToString();

            cmbFilter.SelectedIndex = 0;
        }

        private void _ScheduleTest(clsTestType.enTestType TestType)
        {            
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value, TestType);

            frm.ShowDialog();

            frmManageLocalLicenseApplications_Load(null,null);
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

        private void frmManageLocalLicenseApplications_Load(object sender, EventArgs e)
        {
            lblCurrentUser.Text = clsUtil.CurrentUser == null ? "" : clsUtil.CurrentUser.UserName;;

            _Refresh();

            if (dgvLocalLicenseApplications.Rows.Count > 0)
            {
                dgvLocalLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalLicenseApplications.Columns[0].Width = 120;

                dgvLocalLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalLicenseApplications.Columns[1].Width = 300;

                dgvLocalLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLocalLicenseApplications.Columns[2].Width = 150;

                dgvLocalLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalLicenseApplications.Columns[3].Width = 350;

                dgvLocalLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalLicenseApplications.Columns[4].Width = 170;

                dgvLocalLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalLicenseApplications.Columns[5].Width = 150;
            }
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdateNewLocalLicenseApplication frm = new frmAddUpdateNewLocalLicenseApplication();

            frm.ShowDialog();

            _Refresh();
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to cancel this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalLicenseApplication LocalDrivingLicenseApplication =
                clsLocalLicenseApplication.FindByLocalLicenseApplicationID(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _Refresh();
                }
                else
                {
                    MessageBox.Show("Could not cancel applicatoin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLocalLicenseApplicationInfo frm = new frmShowLocalLicenseApplicationInfo((int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            frmManageLocalLicenseApplications_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateNewLocalLicenseApplication frm = new frmAddUpdateNewLocalLicenseApplication((int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            _Refresh();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cmbFilter.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No":
                    FilterColumn = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                    
                case "Status":
                    FilterColumn = "Status";
                    break;


                default:
                    FilterColumn = "None";
                    break;

            }

            if (txtSearch.Text.Trim() == "" || FilterColumn == "None")
            {
                dt.DefaultView.RowFilter = "";

                lblRecords.Text = dgvLocalLicenseApplications.Rows.Count.ToString();

                return;
            }


            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtSearch.Text.Trim());

            else
                dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtSearch.Text.Trim());

            lblRecords.Text = dgvLocalLicenseApplications.Rows.Count.ToString();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Visible = (cmbFilter.Text != "None");

            if (txtSearch.Visible)
            {
                txtSearch.Text = "";

                txtSearch.Focus();
            }

            dt.DefaultView.RowFilter = "";

            lblRecords.Text = dgvLocalLicenseApplications.Rows.Count.ToString();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        //private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if(MessageBox.Show("Are you sure you want to delete this application?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation) == DialogResult.No)
        //    {
        //        return;
        //    }

        //    int LocalLicenseApplication = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;

        //    clsLocalLicenseApplication localLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(LocalLicenseApplication);

        //    if(localLicenseApplication != null)
        //    {
        //        if(localLicenseApplication.Delete())
        //        {
        //            MessageBox.Show("Application deleted successfully!","Deleted",MessageBoxButtons.OK,MessageBoxIcon.Information);

        //            _Refresh();
        //        }

        //        else
        //        {
        //            MessageBox.Show("Error deleting application!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }

        //    else
        //    {
        //        MessageBox.Show("Application is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (dgvLocalLicenseApplications.CurrentRow == null)
            {
                e.Cancel = true;

                return;
            }

            int LocalLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalLicenseApplication LocalLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(LocalLicenseApplicationID);

            if (LocalLicenseApplication == null)
            {
                MessageBox.Show("Application not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                e.Cancel = true;

                return;
            }

            int TotalPassedTests = (int)dgvLocalLicenseApplications.CurrentRow.Cells[5].Value;

            bool LicenseExists = LocalLicenseApplication.IsLicenseIssued();

            bool PassedVisionTest = LocalLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest);

            bool PassedWrittenTest = LocalLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);

            bool PassedStreetTest = LocalLicenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);

            bool isNew = LocalLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New;

            issueDrivingToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;

            showInfoToolStripMenuItem.Enabled = LicenseExists;

            editToolStripMenuItem.Enabled = !LicenseExists && isNew;

            CancelToolStripMenuItem.Enabled = isNew;

            scheduleTestToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && isNew;

            if (scheduleTestToolStripMenuItem.Enabled)
            {
                visionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                writtenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                streetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.VisionTest);
        }

        private void streetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.StreetTest);
        }

        private void writtenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.WrittenTest);
        }

        private void issueDrivingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueLicenseForTheFirstTime frm = new frmIssueLicenseForTheFirstTime((int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            _Refresh();
        }

        private void lToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value;

            int LicenseID = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(LocalDrivingLicenseApplicationID).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);

                frm.ShowDialog();
            }

            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalLicenseApplication localDrivingLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID((int)dgvLocalLicenseApplications.CurrentRow.Cells[0].Value);

            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(localDrivingLicenseApplication.ApplicantPersonID);

            frm.ShowDialog();
        }
    }
}