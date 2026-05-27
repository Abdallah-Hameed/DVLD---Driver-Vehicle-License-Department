using DVLD_Buisness;
using DVLDTrainin_BusinessLogic;
using DVLDtraining_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDtraining_BusinessLogic
{
    public class clsLocalLicenseApplication : clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enMode Mode = enMode.AddNew;

        public int LocalLicenseApplicationID { get; set; }

        public int LicenseClassID { get; set; }

        public clsLicenseClass LicenseClassInfo { get; set; }

        public string PersonFullName
        {
            get
            {
                this.PersonInfo = clsPerson.Find(ApplicantPersonID); 

                return base.PersonInfo.FullName;
            }
        }

        public clsLocalLicenseApplication()
        {
            this.LocalLicenseApplicationID = -1;

            this.LicenseClassID = -1;

            Mode = enMode.AddNew;
        }

        public clsLocalLicenseApplication(int LocalLicenseApplicationID, int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
                                enApplicationStatus ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID, int LicenseClassID)
        {
            this.LocalLicenseApplicationID = LocalLicenseApplicationID;

            this.ApplicationID = ApplicationID;

            this.ApplicantPersonID = ApplicantPersonID;

            this.ApplicationDate = ApplicationDate;

            this.ApplicationTypeID = ApplicationTypeID;

            this.ApplicationStatus = ApplicationStatus;

            this.LastStatusDate = LastStatusDate;

            this.PaidFees = PaidFees;

            this.CreatedByUserID = CreatedByUserID;

            this.LicenseClassID = LicenseClassID;

            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);

            Mode = enMode.Update;
        }

        private bool _AddNew()
        {
            this.LocalLicenseApplicationID = clsLocalLicenseApplicationData.AddNewLocalLicenseApplication(this.ApplicationID,this.LicenseClassID);

            return (this.LocalLicenseApplicationID != -1);
        }

        private bool _Update()
        {
            return clsLocalLicenseApplicationData.UpdateLocalLicenseApplication(LocalLicenseApplicationID, ApplicationID, LicenseClassID);
        }

        static public clsLocalLicenseApplication FindByLocalLicenseApplicationID(int LocalLicenseID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            bool isFound = clsLocalLicenseApplicationData.GetLocalLicenseApplicationByID(LocalLicenseID, ref ApplicationID, ref LicenseClassID);

            if(isFound )
            {
                clsApplication Application = FindBaseApplication(ApplicationID);

                return new clsLocalLicenseApplication(LocalLicenseID, Application.ApplicationID, Application.ApplicantPersonID, Application.ApplicationDate,
                    Application.ApplicationTypeID, (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate, Application.PaidFees,
                    Application.CreatedByUserID, LicenseClassID);

            }

            else
                return null;
        }

        static public clsLocalLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LocalLicenseID = -1, LicenseClassID = -1;

            bool isFound = clsLocalLicenseApplicationData.GetLocalLicenseApplicationByApplicationID(ref LocalLicenseID,ApplicationID, ref LicenseClassID);

            if (isFound)
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalLicenseApplication(LocalLicenseID, Application.ApplicationID, Application.ApplicantPersonID,
                       Application.ApplicationDate, Application.ApplicationTypeID, (enApplicationStatus)Application.ApplicationStatus,
                        Application.LastStatusDate, Application.PaidFees, Application.CreatedByUserID, Application.LicenseClassID);
            }

            else
                return null;
        }

        public bool Save()
        {
            base.Mode = (clsApplication.enMode)Mode;

            if(!base.Save())
                return false;

            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNew())
                    {
                        Mode = enMode.Update;

                        return true;
                    }

                    else
                        return false;

                case enMode.Update:
                    return _Update();
            }

            return false;
        }

        public static DataTable GetAllLocalLicenseApplications()
        {
            return clsLocalLicenseApplicationData.GetAllLocalLicenseApplications();
        }

        public bool Delete()
        {
            bool IsLocalDrivingApplicationDeleted = false;

            bool IsBaseApplicationDeleted = false;

            IsLocalDrivingApplicationDeleted = clsLocalLicenseApplicationData.DeleteLocalLicenseApplication(this.LocalLicenseApplicationID);

            if (!IsLocalDrivingApplicationDeleted)
                return false;

            IsBaseApplicationDeleted = base.Delete();

            return IsBaseApplicationDeleted;
        }

        public bool DoesAttendTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalLicenseApplicationData.DoesAttendTestType(this.LocalLicenseApplicationID, (int)TestTypeID);
        }

        public byte TotalTrialsPerTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalLicenseApplicationData.TotalTrialsPerTest(this.LocalLicenseApplicationID, (int)TestTypeID);
        }

        public static byte TotalTrialsPerTest(int LocalLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalLicenseApplicationData.TotalTrialsPerTest(LocalLicenseApplicationID, (int)TestTypeID);
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public bool DoesPassTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalLicenseApplicationData.DoesPassTestType(this.LocalLicenseApplicationID, (int)TestTypeID);
        }

        public int GetActiveLicenseID()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }

        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1);
        }

        public bool IsThereAnActiveScheduledTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalLicenseApplicationData.IsThereAnActiveScheduledTest(this.LocalLicenseApplicationID, (int)TestTypeID);
        }

        public static bool AttendedTest(int LocalLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            return clsLocalLicenseApplicationData.TotalTrialsPerTest(LocalLicenseApplicationID, (int)TestTypeID) > 0;
        }

        public bool AttendedTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalLicenseApplicationData.TotalTrialsPerTest(this.LocalLicenseApplicationID, (int)TestTypeID) > 0;
        }

        public clsTest GetLastTestPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }

        public byte GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LocalLicenseApplicationID);
        }

        public bool PassedAllTests()
        {
            return clsTest.PassedAllTests(this.LocalLicenseApplicationID);
        }

        public static bool PassedAllTests(int LocalLicenseApplicationID)
        {
            return clsTest.PassedAllTests(LocalLicenseApplicationID);
        }

        public int IssueLicenseForTheFirstTime(string Notes ,int CreatedByUserID)
        {
            int DriverID = -1;

            clsDriver driver = clsDriver.FindByPersonID(this.ApplicantPersonID);

            if (driver == null) 
            {
                driver = new clsDriver();

                driver.PersonID = this.ApplicantPersonID;

                driver.CreatedByUserID = CreatedByUserID;

                if (driver.Save())
                {
                    DriverID = driver.DriverID;
                }

                else
                    return -1;
            }

            else // The person is already a driver
            {
                DriverID = driver.DriverID;
            }

            clsLicense license = new clsLicense();

            license.ApplicationID = this.ApplicationID;

            license.DriverID = DriverID;

            license.LicenseClass = this.LicenseClassID;

            license.IssueDate = DateTime.Now;

            license.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);

            license.Notes = Notes;

            license.PaidFees = this.LicenseClassInfo.ClassFees;

            license.IsActive = true;

            license.IssueReason = clsLicense.enIssueReason.FirstTime;

            license.CreatedByUserID = CreatedByUserID;

            if (license.Save())
            {
                this.SetComplete();

                return license.LicenseID;
            }

            else
                return -1;
        }
    }
}