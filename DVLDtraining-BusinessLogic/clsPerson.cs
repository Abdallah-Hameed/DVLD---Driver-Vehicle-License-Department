using System;
using System.Data;
using DVLDtraining_BusinessLogic;
using DVLDTraining_DataAccess;

namespace DVLDTrainin_BusinessLogic
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };

        public enMode Mode = enMode.AddNew;

        public clsCountry CountryInfo;        

        public int PersonID { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string LastName { get; set; }

        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }

        public string NationalNo { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short Gender { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int NationalityCountryID { get; set; }

        private string _ImagePath;

        public string ImagePath
        {
            get { return _ImagePath; }

            set { _ImagePath = value; }
        }

        public clsPerson()
        {
            this.PersonID = -1;

            this.FirstName = "";

            this.SecondName = "";

            this.ThirdName = "";

            this.LastName = "";

            this.Phone = "";

            this.Email = "";

            this.NationalNo = "";

            this.Address = "";

            this.Gender = 0;

            this.DateOfBirth = DateTime.Now;

            this.ImagePath = "";

            this.NationalityCountryID = -1;

            Mode = enMode.AddNew;
        }

        clsPerson(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName, string NationalNo,
                     DateTime DateOfBirth, short Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;

            this.FirstName = FirstName;

            this.SecondName = SecondName;

            this.ThirdName = ThirdName;

            this.LastName = LastName;

            this.Phone = Phone;

            this.Email = Email;

            this.NationalNo = NationalNo;

            this.Address = Address;

            this.Gender = Gender;

            this.DateOfBirth = DateOfBirth;

            this.ImagePath = ImagePath;

            this.CountryInfo = clsCountry.Find(NationalityCountryID);

            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.NationalNo, this.DateOfBirth, this.Gender,
                this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.FirstName, this.SecondName, this.ThirdName, this.LastName, this.NationalNo, this.DateOfBirth, this.Gender,
                this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
        }

        public static clsPerson Find(int PersonID)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "";

            string NationalNo = "", Address = "", Phone = "", Email = "", ImagePath = "";

            DateTime DateOfBirth = DateTime.Now;

            int NationalityCountryID = -1;

            short Gender = 0;

            bool isFound = clsPersonData.GetPersonInfoByID(PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref NationalNo,
                ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);

            if (isFound)
            {
                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName, NationalNo, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            }

            else
            {
                return null;
            }
        }

        public static clsPerson Find(string NationalNo)
        {
            int PersonID = -1;

            string FirstName = "", SecondName = "", ThirdName = "", LastName = "";

            string Address = "", Phone = "", Email = "", ImagePath = "";

            DateTime DateOfBirth = DateTime.Now;

            int NationalityCountryID = -1;

            short Gender = 0;

            bool isFound = clsPersonData.GetPersonInfoByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);

            if (isFound)
            {
                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName, NationalNo, DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            }

            else
            {
                return null;
            }
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;

                        return true;
                    }

                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();
            }

            return false;
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static bool isPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }

        public static bool isPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }
    }
}