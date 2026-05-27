using DVLDTrainin_BusinessLogic;
using DVLDtraining.Global;
using DVLDtraining.Properties;
using DVLDtraining_BusinessLogic;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLDtraining.People.Forms
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandler(object sender, int PersonID);

        public event DataBackEventHandler DataBack;

        enum enGender { Male=0,Female =1};

        enum enMode { Add=0, Update=1 }

        enMode _Mode;

        clsPerson _Person = new clsPerson();

        public frmAddUpdatePerson()
        {
            InitializeComponent();

            lblAddUpdatePerson.Text = "Add new person";

            _Mode = enMode.Add;
        }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _Mode = enMode.Update;

            _Person.PersonID = PersonID;

            _Person = clsPerson.Find(_Person.PersonID);

            SwitchToUpdateMode();
        }

        bool CheckFields()
        {
            string Message = "This field is required!";

            if (string.IsNullOrEmpty(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, Message);

                return false;
            }


            if (string.IsNullOrEmpty(txtFirstName.Text))
            {
                errorProvider1.SetError(txtFirstName, Message);

                return false;
            }


            if (string.IsNullOrEmpty(txtSecondName.Text))
            {
                errorProvider1.SetError(txtSecondName, Message);

                return false;
            }


            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                errorProvider1.SetError(txtLastName, Message);

                return false;
            }

            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, Message);

                return false;
            }

            return true;
        }

        void MouseHover(Button button1)
        {
            button1.BackColor = Color.DimGray;
        }

        void MouseLeave(Button button1)
        {
            button1.BackColor = Color.Black;
        }

        void SwitchToUpdateMode()
        {
            lblAddUpdatePerson.Text = "Edit person info";

            lblPersonID.Text = _Person.PersonID.ToString();

            txtNationalNo.Text = _Person.NationalNo;

            txtFirstName.Text = _Person.FirstName;

            txtSecondName.Text = _Person.SecondName;

            txtThirdName.Text = _Person.ThirdName;

            txtLastName.Text = _Person.LastName;

            txtPhone.Text = _Person.Phone;

            txtEmail.Text = _Person.Email;

            txtAddress.Text = _Person.Address;

            dtpDateOfBirth.Value = _Person.DateOfBirth;

            cmbCountry.SelectedValue = _Person.NationalityCountryID;

            if (_Person.Gender == (int)enGender.Male)
            {
                rbMale.Checked = true;
            }

            else
            {
                rbFemale.Checked = true;
            }

            if (_Person.ImagePath != "")
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;

                llRemoveImage.Visible = true;
            }
        }

        bool _HandlePersonImage()
        {
            if(_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }

                    catch(IOException)
                    {

                    }
                }

                if (pbPersonImage.ImageLocation != null)
                {
                    string ImagePath = pbPersonImage.ImageLocation.ToString();

                    if(clsUtil.CopyImageToProjectImagesFolder(ref ImagePath))
                    {
                        pbPersonImage.ImageLocation = ImagePath;

                        return true;
                    }

                    else
                    {
                        MessageBox.Show("Error copying image file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return false;
                    }
                }
            }

            return true;
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);

            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            cmbCountry.DataSource = clsCountry.GetAllCountries();

            cmbCountry.DisplayMember = "CountryName";

            cmbCountry.ValueMember = "CountryID";

            cmbCountry.SelectedIndex = cmbCountry.FindString("Syria");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckFields())
            {
                _Person.NationalNo = txtNationalNo.Text;

                _Person.FirstName = txtFirstName.Text;

                _Person.SecondName = txtSecondName.Text;

                _Person.ThirdName = txtThirdName.Text;

                _Person.LastName = txtLastName.Text;

                _Person.Email = txtEmail.Text;

                _Person.Phone = txtPhone.Text;

                if (rbMale.Checked)
                {
                    _Person.Gender = (int)enGender.Male;
                }

                if (rbFemale.Checked)
                {
                    _Person.Gender = (int)enGender.Female;
                }

                _Person.DateOfBirth = dtpDateOfBirth.Value;

                _Person.ImagePath = pbPersonImage.ImageLocation;

                _Person.Address = txtAddress.Text;

                _Person.NationalityCountryID = (int)cmbCountry.SelectedValue;

                try
                {
                    if (_Person.Save())
                    {
                        MessageBox.Show("Data saved successfully! New person ID is " + _Person.PersonID, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        SwitchToUpdateMode();
                    }

                    else
                    {
                        MessageBox.Show("Data is not saved!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                }

                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            if(pbPersonImage.ImageLocation == null)
                llRemoveImage.Visible = false;

            if (!_HandlePersonImage())
                return;

            DataBack?.Invoke(this, _Person.PersonID);
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (_Person.ImagePath == "" || _Person.ImagePath == null)
                pbPersonImage.Image = Resources._6837225;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (_Person.ImagePath == "" || _Person.ImagePath == null)
                pbPersonImage.Image = Resources._6833591;
        }

        private void Buttons_MouseHover(object sender, EventArgs e)
        {
            MouseHover((Button)sender);
        }

        private void Buttons_MouseLeave(object sender, EventArgs e)
        {
            MouseLeave((Button)sender);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llEditImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            openFileDialog1.FilterIndex = 1;

            openFileDialog1.RestoreDirectory = true;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.Load(openFileDialog1.FileName);

                _Person.ImagePath = openFileDialog1.FileName;

                llRemoveImage.Visible = true;
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            if(rbMale.Checked)
            {
                pbPersonImage.Image = Resources._6837225;
            }

            if(rbFemale.Checked)
            {
                pbPersonImage.Image = Resources._6833591;
            }

            llRemoveImage.Visible = false;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}