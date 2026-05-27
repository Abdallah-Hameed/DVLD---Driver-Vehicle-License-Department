using DVLD_DataAccess;
using DVLDtraining_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDtraining_BusinessLogic
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 };

        enMode _Mode = enMode.AddNew;

        public int TestAppointmentID { get; set; }

        public clsTestType.enTestType TestTypeID { get; set; }

        public int LocalLicensApplicationID { get; set; }

        public DateTime AppointmentDate { get; set; }

        public float PaidFees { get; set; }

        public int CreatedByUserID { get; set; }

        public bool IsLocked { get; set; }

        public int RetakeTestApplicationID { get; set; }

        public clsApplication RetakeTestApplicationInfo { get; set; }

        public int TestID
        {
            get
            {
                return _GetTestID();
            }
        }

        public clsTestAppointment()
        {
            _Mode = enMode.AddNew;

            TestAppointmentID = -1;

            TestTypeID = clsTestType.enTestType.VisionTest;

            LocalLicensApplicationID = -1;

            AppointmentDate = DateTime.Now;

            PaidFees = 0;

            CreatedByUserID = -1;

            IsLocked = false;

            RetakeTestApplicationID = -1;
        }

        public clsTestAppointment(int testAppointmentID, clsTestType.enTestType testTypeID, int localLicensApplicationID, DateTime appointmentDate,
            float paidFees, int createdByUserID, bool isLocked, int retakeTestApplicationID)
        {
            TestAppointmentID = testAppointmentID;

            TestTypeID = testTypeID;

            LocalLicensApplicationID = localLicensApplicationID;

            AppointmentDate = appointmentDate;

            PaidFees = paidFees;

            CreatedByUserID = createdByUserID;

            IsLocked = isLocked;

            RetakeTestApplicationID = retakeTestApplicationID;

            RetakeTestApplicationInfo = clsApplication.FindBaseApplication(retakeTestApplicationID);
        }

        private bool _AddNew()
        {
            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment((int)this.TestTypeID, this.LocalLicensApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.RetakeTestApplicationID);

            return (this.TestAppointmentID != -1);
        }

        private bool _Update()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, (int)this.TestTypeID, this.LocalLicensApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID); 
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = 1;

            int LocalLicenseApplicationID = -1;

            DateTime AppointmentDate = DateTime.Now; 

            float PaidFees = 0;

            int CreatedByUserID = -1;

            bool IsLocked = false;

            int RetakeTestApplicationID = -1;

            if (clsTestAppointmentData.GetTestAppointmentInfoByTestAppointmentID(TestAppointmentID, ref TestTypeID, ref LocalLicenseApplicationID,
                        ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)TestTypeID, LocalLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);

            }


            else
                return null;

        }

        public static clsTestAppointment GetLastTestAppointment(int LocalLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            int TestAppointmentID = -1;

            DateTime AppointmentDate = DateTime.Now;
            
            float PaidFees = 0;

            int CreatedByUserID = -1;
            
            bool IsLocked = false;
            
            int RetakeTestApplicationID = -1;

            if (clsTestAppointmentData.GetLastTestAppointment(LocalLicenseApplicationID, (int)TestTypeID,
                            ref TestAppointmentID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalLicenseApplicationID, AppointmentDate,
                    PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);

            else
                return null;

        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();

        }

        public DataTable GetApplicationTestAppointmentsPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(this.LocalLicensApplicationID, (int)TestTypeID);

        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);

        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {

                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _Update();

            }

            return false;
        }

        private int _GetTestID()
        {
            return clsTestAppointmentData.GetTestIDByTestAppointmentID(TestAppointmentID);
        }

        public void LockTestAppointment()
        {
            clsTestAppointmentData.LockTestAppointment(this.TestAppointmentID);
        }

        static public void LockTestAppointment(int TestAppointmentID)
        {
            clsTestAppointmentData.LockTestAppointment(TestAppointmentID);
        }
    }
}
