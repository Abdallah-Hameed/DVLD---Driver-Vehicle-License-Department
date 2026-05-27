using DVLDtraining_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDtraining_BusinessLogic
{
    public class clsApplicationType
    {
        public int ID { get; private set; }

        public string Title { get; set; }

        public float Fees { get; set; }

        public clsApplicationType()
        {
            ID = -1;

            Title = "";

            Fees = 0;
        }

        public clsApplicationType(int iD, string title, float fees)
        {
            ID = iD;
            Title = title;
            Fees = fees;
        }

        static public clsApplicationType Find(int ID)
        {
            string Title = "";

            float Fees = 0;

            if (clsApplicationTypesData.GetApplicationTypeByID(ID, ref Title, ref Fees))
                return new clsApplicationType(ID, Title, Fees);

            return null;
        }

        public bool Update(int ID,string Title,float Fees)
        {
            return clsApplicationTypesData.UpdateApplicationType(ID, Title, Fees);
        }

        static public DataTable GetAll()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }
    }
}
