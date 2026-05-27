using DVLD_Buisness;
using DVLDtraining.Global;
using DVLDtraining_BusinessLogic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Applications.New_License.Local_License
{
    public partial class frmAddUpdateNewLocalLicenseApplication : Form
    {
        public frmAddUpdateNewLocalLicenseApplication()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateNewLocalLicenseApplication(int LocalLicenseApplicaionID)
        {
            InitializeComponent();

            _Mode = enMode.Update;

            _LocalLicenseApplicationID = LocalLicenseApplicaionID;
        }

        enum enMode { AddNew=0, Update=1 }

        enMode _Mode = enMode.AddNew;

        clsLocalLicenseApplication _LocalLicenseApplication;

        int _LocalLicenseApplicationID = -1;

        int _SelectedPersonID = -1;

        private void _FillLicenseClassesInComoboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow row in dtLicenseClasses.Rows)
            {
                cmbLicenseClass.Items.Add(row["ClassName"]);
            }
        }

        private void _ResetDefualtValues()
        {
            _FillLicenseClassesInComoboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add new local License application";

                _LocalLicenseApplication = new clsLocalLicenseApplication();

                ctrlPersonInfoWithFilter1.FilterFocus();

                //tpApplicationInfo.Enabled = false;

                cmbLicenseClass.SelectedIndex = 0;

                 lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();

                lblApplicationDate.Text = DateTime.Now.ToShortDateString();

                lblCreatedBy.Text = clsUtil.CurrentUser == null ? "" : clsUtil.CurrentUser.UserName;
            }

            else
            {
                lblTitle.Text = "Update local license application";

                tpApplicationInfo.Enabled = true;

                btnSave.Enabled = true;
            }
        }

        void _LoadData()
        {
            ctrlPersonInfoWithFilter1.FilterEnable(false);

            _LocalLicenseApplication = clsLocalLicenseApplication.FindByLocalLicenseApplicationID(_LocalLicenseApplicationID);

            if (_LocalLicenseApplication == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.Close();

                return;
            }

            ctrlPersonInfoWithFilter1.Load(_LocalLicenseApplication.ApplicantPersonID);

            lblApplicationID.Text = _LocalLicenseApplication.LocalLicenseApplicationID.ToString();

            lblApplicationDate.Text = _LocalLicenseApplication.ApplicationDate.ToShortDateString();

            try
            {
                clsLicenseClass licenseClass = clsLicenseClass.Find(_LocalLicenseApplication.LicenseClassID);

                if (licenseClass != null)
                    cmbLicenseClass.SelectedIndex = cmbLicenseClass.FindString(licenseClass.ClassName);

                else
                    MessageBox.Show("License class not found! ID = " + _LocalLicenseApplication.LicenseClassID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            lblApplicationFees.Text = _LocalLicenseApplication.PaidFees.ToString();

            lblCreatedBy.Text = _LocalLicenseApplication.CreatedByUserInfo == null ? "" : _LocalLicenseApplication.CreatedByUserInfo.UserName;
        }

        private void Buutons_MouseHover(object sender, EventArgs e)
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

        private void frmAddUpdateNewLocalLicense_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if(_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (ctrlPersonInfoWithFilter1.PersonID == -1)
            {
                MessageBox.Show("Please select a person first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int LicenseClassID = clsLicenseClass.Find(cmbLicenseClass.Text).LicenseClassID;

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                cmbLicenseClass.Focus();

                return;
            }

            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonInfoWithFilter1.PersonID, LicenseClassID))
            {
                MessageBox.Show("Person already have a license with the same applied driving class, Choose different driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _LocalLicenseApplication.ApplicantPersonID = ctrlPersonInfoWithFilter1.PersonID;

            _LocalLicenseApplication.ApplicationDate = DateTime.Now;

            _LocalLicenseApplication.ApplicationTypeID = 1;

            _LocalLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;

            _LocalLicenseApplication.LastStatusDate = DateTime.Now;

            _LocalLicenseApplication.PaidFees = Convert.ToSingle(lblApplicationFees.Text);

            _LocalLicenseApplication.CreatedByUserID = clsUtil.CurrentUser.UserID;

            _LocalLicenseApplication.LicenseClassID = LicenseClassID;

            ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_LocalLicenseApplication.ApplicantPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                cmbLicenseClass.Focus();

                return;
            }

            if (_LocalLicenseApplication.Save())
            {
                lblApplicationID.Text = _LocalLicenseApplication.LocalLicenseApplicationID.ToString();

                _Mode = enMode.Update;

                lblTitle.Text = "Update Local License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ctrlPersonInfoWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void frmAddUpdateNewLocalLicense_Activated(object sender, EventArgs e)
        {
            ctrlPersonInfoWithFilter1.FilterFocus();
        }
    }
}