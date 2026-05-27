using DVLDTrainin_BusinessLogic;
using DVLDtraining.People.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDtraining.People.Controls
{
    public partial class ctrlPersonInfoWithFilter : UserControl
    {
        clsPerson _Person;

        DataTable dt = clsPerson.GetAllPeople();

        public event EventHandler<clsPerson> evShowInfo;

        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelected(object sender , int PersonID)
        {
            Action<int> handler = OnPersonSelected;

            if(handler != null)
            {
                handler(PersonID);
            }
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }

            set
            {
                _FilterEnabled = value;

                gbFilter.Enabled = false;
            }
        }

        public int PersonID
        {
            get
            {
                return (_Person == null) ? -1 : _Person.PersonID;
            }
        }

        public ctrlPersonInfoWithFilter()
        {
            InitializeComponent();

            cmbFilter.SelectedIndex = 0;

            FilterEnable(true);
        }

        private void Buttons_MouseHover(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor = Color.DimGray;
        }

        private void Buttons_MouseLeave(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor = Color.Transparent;
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();

            frm.DataBack += AddUpdatePerson_DataBack;

            frm.ShowDialog();

            evShowInfo?.Invoke(this, _Person);
        }

        private void AddUpdatePerson_DataBack(object sender,int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person != null)
            {
                ctrlPersonInformation1.Load(_Person.PersonID);

                txtSearch.Text = _Person.PersonID.ToString();
            }

            else
            {
                MessageBox.Show("Error saving data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowInfo_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() == "")
            {
                MessageBox.Show("Please enter person ID/national number!", "Enter information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            if (cmbFilter.SelectedItem.ToString() == "Person ID")
            {
                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", "PersonID", txtSearch.Text.Trim());

                _Person = clsPerson.Find(int.Parse(txtSearch.Text.Trim()));
            }
            else
            {
                dt.DefaultView.RowFilter = string.Format("[{0}] like '{1}%'", "NationalNo", txtSearch.Text.Trim());

                _Person = clsPerson.Find(txtSearch.Text.Trim());
            }

            if (_Person == null)
            {
                MessageBox.Show("Person is not found!", "Invalid ID/National No", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            ctrlPersonInformation1.Load(_Person.PersonID);

            txtSearch.Text = _Person.PersonID.ToString();

            OnPersonSelected?.Invoke(_Person.PersonID);

            evShowInfo?.Invoke(this, _Person);
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbFilter.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        public void Load(int PersonID)
        {
            cmbFilter.SelectedIndex = 0;

            txtSearch.Text = PersonID.ToString();

            ctrlPersonInformation1.Load(PersonID);

            _Person = clsPerson.Find(PersonID);
        }

        public void FilterFocus()
        {
            txtSearch.Focus();
        }

        public void FilterEnable(bool Enable)
        {
            gbFilter.Enabled = Enable;
        }

        public void txtFocus()
        {
            txtSearch.Focus();
        }

        private void ctrlPersonInfoWithFilter_Load(object sender, EventArgs e)
        {
        }
    }
}
