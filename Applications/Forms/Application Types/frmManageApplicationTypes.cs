using DVLDtraining.Applications.Application_Types;
using DVLDtraining.Global;
using DVLDtraining_BusinessLogic;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining
{
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        void Refresh()
        {
            dgvApplicationTypes.DataSource = clsApplicationType.GetAll();

            lblRecords.Text = dgvApplicationTypes.Rows.Count.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_MouseHover(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.DimGray;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackColor = Color.Transparent;
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            lblCurrentUser.Text = clsUtil.CurrentUser == null ? "" : clsUtil.CurrentUser.UserName;;

            Refresh();

            if (dgvApplicationTypes.Rows.Count > 0)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "                          ID";
                dgvApplicationTypes.Columns[0].Width = 250;

                dgvApplicationTypes.Columns[1].HeaderText = "                        Title";
                dgvApplicationTypes.Columns[1].Width = 460;

                dgvApplicationTypes.Columns[2].HeaderText = "                         Fees";
                dgvApplicationTypes.Columns[2].Width = 200;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditApplicationType frm = new frmEditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            Refresh();
        }
    }
}
