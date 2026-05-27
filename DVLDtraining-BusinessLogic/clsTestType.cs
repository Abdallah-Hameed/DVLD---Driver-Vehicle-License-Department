using DVLDtraining_DataAccess;
using System.Data;

namespace DVLDtraining_BusinessLogic
{
    public class clsTestType
    {

        public enum enMode { AddNew = 0, Update = 1 };

        public enMode _Mode = enMode.AddNew;

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };

        public enTestType ID { set; get; }

        public string Title { set; get; }

        public string Description { set; get; }

        public float Fees { set; get; }

        public clsTestType()
        {
            this.ID = enTestType.VisionTest;

            this.Title = "";

            this.Description = "";

            this.Fees = 0;

            _Mode = enMode.AddNew;
        }

        public clsTestType(enTestType ID, string TestTypeTitel, string Description, float TestTypeFees)
        {
            this.ID = ID;

            this.Title = TestTypeTitel;

            this.Description = Description;

            this.Fees = TestTypeFees;

            _Mode = enMode.Update;
        }

        private bool _AddNew()
        {
            this.ID = (enTestType)clsTestTypeData.AddNewTestType(this.Title, this.Description, this.Fees);

            return (this.Title != "");
        }

        private bool _Update()
        {
            return clsTestTypeData.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees);
        }

        public static clsTestType Find(enTestType TestTypeID)
        {
            string Title = "", Description = "";
            
            float Fees = 0;

            if (clsTestTypeData.GetTestTypeByTestTypeID((int)TestTypeID, ref Title, ref Description, ref Fees))
                return new clsTestType(TestTypeID, Title, Description, Fees);

            else
                return null;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();

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
    }
}