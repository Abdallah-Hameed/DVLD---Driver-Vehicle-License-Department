using DVLD_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDTraining_DataAccess
{
    public class clsCountryData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool isFound = false;

            string query = "Select * from Countries where CountryID = @CountryID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryID", ID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    CountryName = (string)reader["CountryName"];
                }
                else
                {
                    EventLogger.LogWarning("GetCountryInfoByID", $"Country ID {ID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetCountryInfoByID", ID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetCountryInfoByID", $"Error getting country ID {ID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetCountryInfoByName(string CountryName, ref int CountryID)
        {
            bool isFound = false;

            string query = "Select * from Countries where CountryName = @CountryName;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    CountryID = (int)reader["CountryID"];
                }
                else
                {
                    EventLogger.LogWarning("GetCountryInfoByName", $"Country Name '{CountryName}' not found");
                }

                reader.Close();
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetCountryInfoByName", 0, ex);
                isFound = false;
            }

            catch (Exception ex)
            {
                EventLogger.LogError("GetCountryInfoByName", $"Error getting country by name '{CountryName}'", ex);
                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            string query = @"select * from Countries order by CountryName";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();

                EventLogger.LogInformation("GetAllCountries", $"Retrieved {dt.Rows.Count} countries");
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllCountries", 0, ex);
            }

            catch (Exception ex)
            {
                EventLogger.LogError("GetAllCountries", "Error getting all countries", ex);
            }

            finally
            {
                connection.Close();
            }

            return dt;
        }
    }
}