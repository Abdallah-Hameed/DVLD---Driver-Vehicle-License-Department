using DVLDtraining.Global;
using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Tests
{
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        void Refresh()
        {
            dgvTestTypes.DataSource = clsTestType.GetAllTestTypes();

            lblRecords.Text = dgvTestTypes.Rows.Count.ToString();
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

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            lblCurrentUser.Text = clsUtil.CurrentUser == null ? "" : clsUtil.CurrentUser.UserName;;

            Refresh();

            if (dgvTestTypes.Rows.Count > 0)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width = 30;

                dgvTestTypes.Columns[1].HeaderText = " Title";
                dgvTestTypes.Columns[1].Width = 140;

                dgvTestTypes.Columns[2].HeaderText = "                                            Description";
                dgvTestTypes.Columns[2].Width = 673;

                dgvTestTypes.Columns[3].HeaderText = " Fees";
                dgvTestTypes.Columns[3].Width = 70;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditTestType frm = new frmEditTestType((clsTestType.enTestType)dgvTestTypes.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            Refresh();
        }
    }
}
