using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDtraining.Licenses.Forms
{
    public partial class frmShowPersonLicensesHistory : Form
    {
        public frmShowPersonLicensesHistory()
        {
            InitializeComponent();
        }

        public frmShowPersonLicensesHistory(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
        }

        int _PersonID = -1;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPersonLicensesHistory_Load(object sender, EventArgs e)
        {
            if(_PersonID != -1)
            {
                ctrlPersonInfoWithFilter1.Load(_PersonID);

                ctrlPersonInfoWithFilter1.FilterEnabled = false;

                ctrlDriverLicenses1.LoadInfoByPersonID(_PersonID);
            }

            else
            {
                ctrlPersonInfoWithFilter1.Enabled = true;

                ctrlPersonInfoWithFilter1.txtFocus();
            }
        }

        private void ctrlPersonInfoWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;

            if (_PersonID == -1)
            {
                ctrlDriverLicenses1.Clear();
            }

            else
                ctrlDriverLicenses1.LoadInfoByPersonID(_PersonID);
        }

        private void btnClose_MouseHover(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.DimGray;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.Transparent;
        }
    }
}
