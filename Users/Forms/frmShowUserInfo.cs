using DVLDtraining.Global;
using DVLDtraining.People.UserControls;
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

namespace DVLDtraining.Users.Forms
{
    public partial class frmShowUserInfo : Form
    {
        public frmShowUserInfo(int UserID)
        {
            InitializeComponent();

            ctrlUserInfo1.Load(UserID);
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowUserInfo_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_MouseHover(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.DimGray;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.ForeColor = Color.Transparent;
        }
    }
}
