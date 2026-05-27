using DVLDtraining_BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDtraining.Users.Controls
{
    public partial class ctrlLoginInformation : UserControl
    {
        public ctrlLoginInformation()
        {
            InitializeComponent();
        }

        public int Load(int UserID)
        {
            clsUser user = clsUser.Find(UserID);

            if(user != null)
            {
                lblUserID.Text = user.UserID.ToString();

                txtCurrentPassword.Text = user.Password;

                lblUserName.Text = user.UserName;

                lblIsActive.Text = (user.IsActive) ? "Yes" : "No";

                return user.UserID;
            }

            return -1;
        }
    }
}
