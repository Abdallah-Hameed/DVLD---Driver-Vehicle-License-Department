using DVLDtraining.People.Forms;
using DVLDtraining_BusinessLogic;
using System.Windows.Forms;

namespace DVLDtraining.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        clsApplication _Application;

        int _ApplicationID = -1;

        public int ApplicationID
        {
            get
            {
                return _ApplicationID;
            }
        }

        void FillApplcationInfo()
        {
            lblApplicationID.Text = _Application.ApplicationID.ToString();

            lblApplicationStatus.Text = _Application.ApplicationStatus.ToString();

            lblApplicationFees.Text = _Application.PaidFees.ToString();

            lblApplicationType.Text = _Application.ApplicationTypeInfo.Title;

            lblApplicant.Text = _Application.ApplicantFullName;

            lblApplicationDate.Text = _Application.ApplicationDate.ToShortDateString();

            lblApplicationStatusDate.Text = _Application.LastStatusDate.ToShortDateString();

            lblCreatedBy.Text = clsUser.Find(_Application.CreatedByUserID).UserName;
        }

        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[????]";

            lblApplicationStatus.Text = "[????]";

            lblApplicationFees.Text = "[????]";

            lblApplicationType.Text = "[????]";

            lblApplicant.Text = "[????]";

            lblApplicationDate.Text = "[????]";

            lblApplicationStatusDate.Text = "[????]";

            lblCreatedBy.Text = "[????]";
        }

        public void Load(int ApplicationID)
        {
            _Application = clsApplication.FindBaseApplication(ApplicationID);

            if(_Application != null)
            {
                FillApplcationInfo();
            }

            else
            {
                MessageBox.Show("Application is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                ResetApplicationInfo();
            }
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Application != null)
            {
                frmShowPersonInfo frm = new frmShowPersonInfo(_Application.PersonInfo.PersonID);

                frm.ShowDialog();
            }

            else
            {
                MessageBox.Show("Person is not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                ResetApplicationInfo();
            }
        }
    }
}
