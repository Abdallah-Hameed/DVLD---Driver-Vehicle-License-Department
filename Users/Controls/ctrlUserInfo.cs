using DVLDtraining.Properties;
using DVLDtraining_BusinessLogic;
using Microsoft.VisualBasic.ApplicationServices;
using System.Windows.Forms;

namespace DVLDtraining.Users.Controls
{
    public partial class ctrlUserInfo : UserControl
    {
        public ctrlUserInfo()
        {
            InitializeComponent();
        }

        clsUser _User;

        public void Load(int UserID)
        {
            _User = clsUser.Find(UserID);

            if (_User != null)
            {
                ctrlPersonInformation1.Load(_User.PersonID);

                lblPersonID.Text = _User.PersonID.ToString();

                lblUserID.Text = _User.UserID.ToString();

                lblUserName.Text = _User.UserName;

                pbIsActive.Image = _User.IsActive ? Resources.check_mark : Resources.error;
            }
        }

        private void llEditUserInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_User != null)
            {
                frmAddUpdateUser frm = new frmAddUpdateUser(_User.UserID);

                frm.DataBack += AddUpdateUser_DataBack;

                frm.ShowDialog();
            }
        }

        private void AddUpdateUser_DataBack(object sender,int UserID)
        {
            _User = clsUser.Find(UserID);

            ctrlPersonInformation1.Load(_User.PersonID);

            lblUserID.Text = _User.UserID.ToString();

            lblUserName.Text = _User.UserName;

            pbIsActive.Image = (_User.IsActive) ? Resources.check_mark : Resources.error;
        }
    }
}