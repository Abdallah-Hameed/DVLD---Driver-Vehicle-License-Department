using DVLDTrainin_BusinessLogic;
using DVLDtraining.People.Forms;
using DVLDtraining.Properties;
using System.IO;
using System.Windows.Forms;

namespace DVLDtraining.People.UserControls
{
    public partial class ctrlPersonInformation : UserControl
    {
        public ctrlPersonInformation()
        {
            InitializeComponent();
        }

        clsPerson _Person;

        public int PersonID
        {
            get
            {
                return _Person.PersonID;
            }
        }

        public void Load(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);

            if (_Person != null)
            {
                // Reset previous image
                pbPersonImage.Image = null;

                pbPersonImage.ImageLocation = null;

                lblPersonID.Text = _Person.PersonID.ToString();

                lblNationalNo.Text = _Person.NationalNo;

                lblFirstName.Text = _Person.FirstName;

                lblSecondName.Text = _Person.SecondName;

                lblThirdName.Text = _Person.ThirdName;

                lblLastName.Text = _Person.LastName;

                lblPhone.Text = _Person.Phone;

                lblEmail.Text = _Person.Email;

                lblAddress.Text = _Person.Address;

                if (_Person.Gender == 0)
                {
                    lblGender.Text = "Male";
                }
                else
                {
                    lblGender.Text = "Female";
                }

                lblCountry.Text = _Person.CountryInfo.CountryName;

                lblDateOfBirth.Text = _Person.DateOfBirth.ToString("yyyy-MM-dd");

                if (!string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
                {
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                }
                else
                {
                    if (_Person.Gender == 0)
                        pbPersonImage.Image = Resources._6837225;
                    else
                        pbPersonImage.Image = Resources._6833591;
                }
            }

            else
            {
                MessageBox.Show("Person is not found!", "Wrong person ID", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Load(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);

            if (_Person != null)
            {
                // Reset previous image
                pbPersonImage.Image = null;

                pbPersonImage.ImageLocation = null;

                lblPersonID.Text = _Person.PersonID.ToString();

                lblNationalNo.Text = _Person.NationalNo;

                lblFirstName.Text = _Person.FirstName;

                lblSecondName.Text = _Person.SecondName;

                lblThirdName.Text = _Person.ThirdName;

                lblLastName.Text = _Person.LastName;

                lblPhone.Text = _Person.Phone;

                lblEmail.Text = _Person.Email;

                lblAddress.Text = _Person.Address;

                if (_Person.Gender == 0)
                {
                    lblGender.Text = "Male";
                }
                else
                {
                    lblGender.Text = "Female";
                }

                lblCountry.Text = _Person.CountryInfo.CountryName;

                lblDateOfBirth.Text = _Person.DateOfBirth.ToString("yyyy-MM-dd");

                if (!string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
                {
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                }
                else
                {
                    if (_Person.Gender == 0)
                        pbPersonImage.Image = Resources._6837225;

                    else
                        pbPersonImage.Image = Resources._6833591;
                }
            }
            else
            {
                MessageBox.Show("Person is not found!", "Wrong national number", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Person != null)
            {
                frmAddUpdatePerson frm = new frmAddUpdatePerson(_Person.PersonID);

                frm.DataBack += AddUpdatePerson_DataBack;

                frm.ShowDialog();
            }

            else
            {
                MessageBox.Show("Please add person informaion first!", "Person is not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddUpdatePerson_DataBack(object sender , int PersonID)
        {
            _Person.PersonID = PersonID;

            Load(PersonID);
        }
    }
}