using DVLDtraining.Global;
using DVLDtraining.Users.Forms;
using DVLDtraining_BusinessLogic;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.Users
{
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();
        }

        clsUser _User;

        DataTable dt = clsUser.GetAllUsers();

        void Refresh()
        {
            dt = clsUser.GetAllUsers();

            dgvUsers.DataSource = dt;

            cmbActive.SelectedIndex = 0;

            txtSearch.Visible = false;

            cmbFilter.SelectedIndex = 0;

            lblRecords.Text = dgvUsers.RowCount.ToString();
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

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword((int)dgvUsers.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            frmManageUsers_Load(null,null);
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            Refresh();

            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "                                 User ID";
                dgvUsers.Columns[0].Width = 270;

                dgvUsers.Columns[1].HeaderText = "                                 Person ID";
                dgvUsers.Columns[1].Width = 270;

                dgvUsers.Columns[2].HeaderText = "                                 Full name";
                dgvUsers.Columns[2].Width = 280;

                dgvUsers.Columns[3].HeaderText = "                                 Username";
                dgvUsers.Columns[3].Width = 280;

                dgvUsers.Columns[4].HeaderText = "                                  Active";
                dgvUsers.Columns[4].Width = 253;
            }

            lblCurrentUser.Text = clsUtil.CurrentUser == null ? "" : clsUtil.CurrentUser.UserName;
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser();

            frm.ShowDialog();

            Refresh();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frm = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            Refresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this user?", "Delete user", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                _User = clsUser.Find((int)dgvUsers.CurrentRow.Cells[0].Value);

                if (_User != null)
                {
                    if (_User != clsUtil.CurrentUser)
                    {
                        if (clsUser.DeleteUser(_User.UserID))
                        {
                            MessageBox.Show("User is deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            Refresh();

                            return;
                        }

                        else
                        {
                            MessageBox.Show("Error deleting data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    else
                    {
                        MessageBox.Show("This account is loged in!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void showInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowUserInfo frm = new frmShowUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            Refresh();
        }

        private void callToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This service will be implemented later on!", "disabled service", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return;
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This service will be implemented later on!", "disabled service", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string Filter = "";

            switch (cmbFilter.Text)
            {
                case "User ID":
                    Filter = "UserID";
                    break;

                case "User name":
                    Filter = "UserName";
                    break;

                case "Person ID":
                    Filter = "PersonID";
                    break;


                case "Full name":
                    Filter = "FullName";
                    break;

                default:
                    Filter = "None";
                    break;

            }

            if (txtSearch.Text.Trim() == "" || Filter == "None")
            {
                dt.DefaultView.RowFilter = "";

                lblRecords.Text = dgvUsers.Rows.Count.ToString();

                return;
            }


            if (Filter != "FullName" && Filter != "UserName")
                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", Filter, txtSearch.Text.Trim());

            else
                dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", Filter, txtSearch.Text.Trim());

            lblRecords.Text = dt.Rows.Count.ToString();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "Active")
            {
                txtSearch.Visible = false;

                cmbActive.Visible = true;

                cmbActive.Focus();

                cmbActive.SelectedIndex = 0;
            }

            else
            {
                txtSearch.Visible = (cmbFilter.Text != "None");

                cmbActive.Visible = false;

                if (cmbFilter.Text == "None")
                {
                    txtSearch.Enabled = false;
                }

                else
                    txtSearch.Enabled = true;

                txtSearch.Text = "";

                txtSearch.Focus();
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "Person ID" || cmbFilter.Text == "User ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cmbActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";

            string FilterValue = cmbActive.Text;

            switch (FilterValue)
            {
                case "All":
                    break;

                case "Yes":
                    FilterValue = "1";
                    break;

                case "No":
                    FilterValue = "0";
                    break;
            }


            if (FilterValue == "All")
                dt.DefaultView.RowFilter = "";

            else
                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecords.Text = dt.Rows.Count.ToString();
        }
    }
}
