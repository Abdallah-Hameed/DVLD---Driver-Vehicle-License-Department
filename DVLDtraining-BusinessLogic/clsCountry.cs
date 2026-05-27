using DVLDTraining_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDtraining_BusinessLogic
{
    public class clsCountry
    {
        public int ID { get; set; }

        public string CountryName { get; set; }

        public clsCountry()
        {
            ID = -1;

            CountryName = "";
        }

        private clsCountry(int ID, string CountryName)
        {
            this.ID = ID;

            this.CountryName = CountryName;
        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();
        }

        public static clsCountry Find(int ID)
        {
            string CountryName = "";

            if (clsCountryData.GetCountryInfoByID(ID, ref CountryName))
            {
                return new clsCountry(ID, CountryName);
            }

            else
            {
                return null;
            }
        }

        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;

            if (clsCountryData.GetCountryInfoByName(CountryName, ref CountryID))
            {
                return new clsCountry(CountryID, CountryName);
            }

            else
            {
                return null;
            }
        }
    }
}
