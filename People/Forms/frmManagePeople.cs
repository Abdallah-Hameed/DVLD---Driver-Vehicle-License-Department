using DVLDTrainin_BusinessLogic;
using DVLDtraining.Global;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.People.Forms
{
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();
        }

        static DataTable dtAllPeople = clsPerson.GetAllPeople();

        DataTable dt = dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo", "FirstName", "SecondName", "ThirdName", "LastName",
            "GendorCaption", "DateOfBirth", "CountryName", "Phone", "Email");

        void MouseHover(Button button1)
        {
            button1.BackColor = Color.DimGray;
        }

        void MouseLeave(Button button1)
        {
            button1.BackColor = Color.Black;
        }
        
        void _SetHeadersAndWidth()
        {
            dgvPeople.Columns[0].HeaderText = "Person ID";
            dgvPeople.Columns[0].Width = 80;

            dgvPeople.Columns[1].HeaderText = "National No.";
            dgvPeople.Columns[1].Width = 120;


            dgvPeople.Columns[2].HeaderText = "First Name";
            dgvPeople.Columns[2].Width = 120;

            dgvPeople.Columns[3].HeaderText = "Second Name";
            dgvPeople.Columns[3].Width = 120;


            dgvPeople.Columns[4].HeaderText = "Third Name";
            dgvPeople.Columns[4].Width = 120;

            dgvPeople.Columns[5].HeaderText = "Last Name";
            dgvPeople.Columns[5].Width = 120;

            dgvPeople.Columns[6].HeaderText = "Gender";
            dgvPeople.Columns[6].Width = 120;

            dgvPeople.Columns[7].HeaderText = "Date Of Birth";
            dgvPeople.Columns[7].DefaultCellStyle.Format = "yyyy/MM/dd";
            dgvPeople.Columns[7].Width = 140;

            dgvPeople.Columns[8].HeaderText = "Nationality";
            dgvPeople.Columns[8].Width = 120;


            dgvPeople.Columns[9].HeaderText = "Phone";
            dgvPeople.Columns[9].Width = 120;


            dgvPeople.Columns[10].HeaderText = "Email";
            dgvPeople.Columns[10].Width = 170;
        }

        void _Refresh()
        {
            dtAllPeople = clsPerson.GetAllPeople();

            dt = dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo", "FirstName", "SecondName", "ThirdName", "LastName",
                "GendorCaption", "DateOfBirth", "CountryName", "Phone", "Email");

            dgvPeople.DataSource = dt;

            _RecordsCount();

            if (dgvPeople.ColumnCount > 0)
            {
                _SetHeadersAndWidth();
            }

            lblCurrentUser.Text = clsUtil.CurrentUser == null ? "" : clsUtil.CurrentUser.UserName;

            txtSearch.Text = "";

            cmbFilter.SelectedIndex = 0;
        }

        void _RecordsCount()
        {
            lblRecords.Text = dgvPeople.Rows.Count.ToString();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();

            frm.ShowDialog();

            _Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            panel1.Left = (this.ClientSize.Width - panel1.Width) / 2;

            panel1.Top = (this.ClientSize.Height - panel1.Height) / 2;

            panel1.Size = this.Size;

            cmbFilter.SelectedIndex = 0;

            _Refresh();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Visible = (cmbFilter.Text != "None");

            if (txtSearch.Visible)
            {
                txtSearch.Text = "";

                txtSearch.Focus();
            }
        }

        private void Buttons_MouseHover(object sender, EventArgs e)
        {
            MouseHover((Button)sender);
        }

        private void Buttons_MouseLeave(object sender, EventArgs e)
        {
            MouseLeave((Button)sender);
        }

        private void showInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvPeople.CurrentRow.Cells[0].Value);

            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            _Refresh();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string Filter = "";

            switch (cmbFilter.SelectedItem.ToString())
            {
                case "Person ID":
                    Filter = "PersonID";
                    break;

                case "National No":
                    Filter = "NationalNo";
                    break;

                case "First name":
                    Filter = "FirstName";
                    break;

                case "Second name":
                    Filter = "SecondName";
                    break;

                case "Third name":
                    Filter = "ThirdName";
                    break;

                case "Last name":
                    Filter = "LastName";
                    break;

                case "Email":
                    Filter = "Email";
                    break;

                case "Phone":
                    Filter = "Phone";
                    break;

                case "Gender":
                    Filter = "GendorCaption";
                    break;

                case "Country":
                    Filter = "CountryName";
                    break;

                default:
                    Filter = "None";
                    break;
            }

            txtSearch.Visible = (Filter != "None");

            if (Filter == "None" || txtSearch.Text.Trim() == "")
            {
                dt.DefaultView.RowFilter = "";

                _RecordsCount();

                return;
            }

            if (Filter == "PersonID")
            {
                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", Filter, txtSearch.Text.Trim());
            }

            else
            {
                dt.DefaultView.RowFilter = string.Format("[{0}] like '{1}%'", Filter, txtSearch.Text.Trim());
            }

            _RecordsCount();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete this person ?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person deleted successfuly!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _Refresh();

                    return;
                }

                else
                {
                    MessageBox.Show("Error:Person is not deleted!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            else
            {
                return;
            }
        }

        private void callToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This service is currently unavailable!", "Unavailabe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This service is currently unavailable!", "Unavailabe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}