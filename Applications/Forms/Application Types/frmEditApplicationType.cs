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

namespace DVLDtraining.Applications.Application_Types
{
    public partial class frmEditApplicationType : Form
    {
        public frmEditApplicationType(int ID)
        {
            InitializeComponent();

            _App = clsApplicationType.Find(ID);
        }

        clsApplicationType _App;

        bool CheckFields()
        {
            if (string.IsNullOrEmpty(txtApplicationTitle.Text))
            {
                errorProvider1.SetError(txtApplicationTitle, "This field is required!");

                return false;
            }

            if (string.IsNullOrEmpty(txtApplicationFees.Text))
            {
                errorProvider1.SetError(txtApplicationFees, "This field is required!");

                return false;
            }

            if (int.Parse(txtApplicationFees.Text) <= 0)
            {
                errorProvider1.SetError(txtApplicationFees, "Application fees must be positive!");

                return false;
            }

            return true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckFields())
                return;

            if(_App.Update(int.Parse (lblApplicationID.Text),txtApplicationTitle.Text,Convert.ToSingle(txtApplicationFees.Text)))
            {
                MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.Close();

                return;
            }

            MessageBox.Show("Error saving new data!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            if (_App != null)
            {
                lblApplicationID.Text = _App.ID.ToString();

                txtApplicationTitle.Text = _App.Title;

                txtApplicationFees.Text = _App.Fees.ToString();
            }
        }

        private void txtApplicationFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
