using DVLDtraining_DataAccess;
using System.Data;
using System;
using DVLDTrainin_BusinessLogic;

namespace DVLDtraining_BusinessLogic
{
    public class clsUser
    {
        enum enMode { AddNew = 0, Update = 1 }

        enMode _Mode = enMode.AddNew;

        public int UserID { get; set; }

        public int PersonID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public clsPerson PersonInfo;

        public clsUser()
        {
            UserID = -1;

            PersonID = -1;

            UserName = "";

            Password = "";

            IsActive = true;

            _Mode = enMode.AddNew;
        }

        public clsUser(int UserID,int PersonID, string UserName, string Password,bool IsActive)
        {
            this.UserID = UserID;

            this.PersonID = PersonID;

            this.UserName = UserName;

            this.Password = Password;

            this.IsActive = IsActive;

            this.PersonInfo = clsPerson.Find(PersonID);

            _Mode = enMode.Update;
        }

        static public clsUser Find(int UserID)
        {
            string UserName = "", Password = "";

            int PersonID = -1;

            bool IsActive = false;

            if (clsUserData.GetUserInfoByUserID(UserID, ref PersonID, ref UserName, ref Password, ref IsActive))
            {
                return new clsUser(UserID,PersonID, UserName, Password, IsActive);
            }

            else
            {
                return null;
            }
        }

        static public clsUser FindByPersonID(int PersonID)
        {
            string UserName = "", Password = "";

            int UserID = -1;

            bool IsActive = false;

            if (clsUserData.GetUserInfoByPersonID(ref UserID, PersonID, ref UserName, ref Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            else
            {
                return null;
            }
        }

        static public clsUser Find(string UserName)
        {
            int UserID = -1;

            string Password = "";

            int PersonID = -1;

            bool IsActive = false;

            if (clsUserData.GetUserInfoByUserName(ref UserID, ref PersonID, UserName, ref Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            else
            {
                return null;
            }
        }

        static public clsUser Find(string UserName,string Password)
        {
            int UserID = -1;

            int PersonID = -1;

            bool IsActive = false;

            if (clsUserData.GetUserInfoByUserNameAndPassword(ref UserID, ref PersonID, UserName,Password, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            else
            {
                return null;
            }
        }

        private bool _AddNewUser()
        {

            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName, this.Password, this.IsActive);

            return (UserID != -1);
        }

        private bool _UpateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.UserName, this.Password, this.IsActive);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        _Mode = enMode.Update;

                        return true;
                    }

                    return false;

                case enMode.Update:
                    return _UpateUser();

            }

            return false;
        }

        static public bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }

        static public DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }

        static public bool IsUserExists(string UserName, string Password)
        {
            return clsUserData.IsUserExistsByUserNameAndPassword(UserName, Password);
        }

        static public bool IsUserExists(int UserID)
        {
            return clsUserData.IsUserExistsByUserID(UserID);
        }

        static public bool IsUserExists(string UserName)
        {
            return clsUserData.IsUserExistsByUserName(UserName);
        }

        static public bool IsUserActive(string UserName)
        {
            return clsUserData.IsUserActiveByUsername(UserName);
        }

        static public bool IsUserActive(string UserName, string Password)
        {
            return clsUserData.IsUserActiveByUsernameAndPassword(UserName, Password);
        }

        static public bool IsUserActive(int PersonID)
        {
            return clsUserData.IsUserActiveByPersonID(PersonID);
        }
    }
}