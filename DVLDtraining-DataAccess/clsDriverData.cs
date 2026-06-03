using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsDriverData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        public static bool GetDriverInfoByDriverID(int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = (int)reader["PersonID"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    CreatedDate = (DateTime)reader["CreatedDate"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetDriverInfoByDriverID", $"Driver ID {DriverID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetDriverInfoByDriverID", DriverID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetDriverInfoByDriverID", $"Error getting driver ID {DriverID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    DriverID = (int)reader["DriverID"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    CreatedDate = (DateTime)reader["CreatedDate"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetDriverInfoByPersonID", $"Person ID {PersonID} not found as a driver");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetDriverInfoByPersonID", PersonID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetDriverInfoByPersonID", $"Error getting driver for Person ID {PersonID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM Drivers_View order by FullName";

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

                EventLogger.LogInformation("GetAllDrivers", $"Retrieved {dt.Rows.Count} drivers");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllDrivers", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllDrivers", "Error getting all drivers", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;

            string query = @"Insert Into Drivers (PersonID,CreatedByUserID,CreatedDate)
                            Values (@PersonID,@CreatedByUserID,@CreatedDate);
                          
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DriverID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "Drivers", DriverID);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewDriver", PersonID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewDriver", $"Error adding driver for Person ID {PersonID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return DriverID;
        }

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"Update Drivers set PersonID = @PersonID, CreatedByUserID = @CreatedByUserID where DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "Drivers", DriverID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateDriver", $"Driver ID {DriverID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateDriver", DriverID, ex);
                return false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateDriver", $"Error updating driver ID {DriverID}", ex);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
    }
}