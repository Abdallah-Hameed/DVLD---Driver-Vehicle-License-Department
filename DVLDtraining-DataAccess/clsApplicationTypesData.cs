using DVLD_DataAccess;
using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDtraining_DataAccess
{
    public class clsApplicationTypesData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        static public bool GetApplicationTypeByID(int ID, ref string Title, ref float Fees)
        {
            bool isFound = false;

            string Query = "select * from ApplicationTypes where ApplicationTypeID = @ID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Title = (string)reader["ApplicationTypeTitle"];

                    Fees = Convert.ToSingle(reader["ApplicationFees"]);

                    isFound = true;
                }
                else
                {
                    EventLogger.LogWarning("GetApplicationTypeByID", $"Application Type ID {ID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetApplicationTypeByID", ID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetApplicationTypeByID", $"Error getting application type ID {ID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool UpdateApplicationType(int ID, string Title, float Fees)
        {
            int rowsAffected = 0;

            string Query = @"Update ApplicationTypes 
                                Set ApplicationTypeTitle = @Title , ApplicationFees = @Fees
                                    where ApplicationTypeID = @ID;";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            command.Parameters.AddWithValue("@Title", Title);

            command.Parameters.AddWithValue("@Fees", Fees);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "ApplicationTypes", ID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateApplicationType", $"Application Type ID {ID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateApplicationType", ID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateApplicationType", $"Error updating application type ID {ID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        static public DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();

            string Query = "Select * from ApplicationTypes";

            SqlCommand command = new SqlCommand(@Query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();

                EventLogger.LogInformation("GetAllApplicationTypes", $"Retrieved {dt.Rows.Count} application types");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllApplicationTypes", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllApplicationTypes", "Error getting all application types", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }
    }
}