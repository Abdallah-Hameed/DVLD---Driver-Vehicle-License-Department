using DVLDtraining.Global;
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

namespace DVLDtraining.Tests
{
    public partial class frmEditTestType : Form
    {
        public frmEditTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();

            _Test = clsTestType.Find(TestTypeID);
        }

        clsTestType _Test;

        bool CheckFields()
        {
            if (string.IsNullOrEmpty(txtTestTitle.Text))
            {
                errorProvider1.SetError(txtTestTitle, "This field is required!");

                return false;
            }

            if (string.IsNullOrEmpty(txtTestFees.Text))
            {
                errorProvider1.SetError(txtTestFees, "This field is required!");

                return false;
            }

            if (string.IsNullOrEmpty(txtDescription.Text))
            {
                errorProvider1.SetError(txtDescription, "This field is required!");

                return false;
            }

            if (int.Parse(txtTestFees.Text) <= 0)
            {
                errorProvider1.SetError(txtTestFees, "Test fees must be positive!");

                return false;
            }

            return true;
        }

        private void btnClose_MouseHover(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor = Color.DimGray;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            b.BackColor = Color.Transparent;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTestFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            if (_Test != null)
            {
                lblTestID.Text = _Test.ID.ToString();

                txtTestTitle.Text = _Test.Title;

                txtDescription.Text = _Test.Description;

                txtTestFees.Text = _Test.Fees.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckFields())
                return;

            if (_Test.Save())
            {
                MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.Close();

                return;
            }

            MessageBox.Show("Error saving new data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
