using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsInternationalLicenseData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID, ref int ApplicationID, ref int DriverID, ref int IssuedUsingLocalLicenseID,
            ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];

                    DriverID = (int)reader["DriverID"];

                    IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];

                    IssueDate = (DateTime)reader["IssueDate"];

                    ExpirationDate = (DateTime)reader["ExpirationDate"];

                    IsActive = (bool)reader["IsActive"];

                    CreatedByUserID = (int)reader["DriverID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetInternationalLicenseInfoByID", $"International License ID {InternationalLicenseID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetInternationalLicenseInfoByID", InternationalLicenseID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetInternationalLicenseInfoByID", $"Error getting international license ID {InternationalLicenseID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();

            string query = @" SELECT InternationalLicenseID, ApplicationID,DriverID, IssuedUsingLocalLicenseID , IssueDate, ExpirationDate, IsActive from InternationalLicenses
                            order by IsActive, ExpirationDate desc";

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

                EventLogger.LogInformation("GetAllInternationalLicenses", $"Retrieved {dt.Rows.Count} international licenses");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllInternationalLicenses", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllInternationalLicenses", "Error getting all international licenses", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable dt = new DataTable();

            string query = @" SELECT InternationalLicenseID, ApplicationID,IssuedUsingLocalLicenseID , IssueDate,  ExpirationDate, IsActive from InternationalLicenses
                            where DriverID=@DriverID order by ExpirationDate desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();

                EventLogger.LogInformation("GetDriverInternationalLicenses", $"Retrieved {dt.Rows.Count} international licenses for Driver ID {DriverID}");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetDriverInternationalLicenses", DriverID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetDriverInternationalLicenses", $"Error getting international licenses for Driver ID {DriverID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int InternationalLicenseID = -1;

            string query = @"Update InternationalLicenses set IsActive=0 where DriverID=@DriverID;

                             INSERT INTO InternationalLicenses (ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive,CreatedByUserID)
                         VALUES(@ApplicationID,@DriverID,@IssuedUsingLocalLicenseID,@IssueDate,@ExpirationDate,@IsActive,@CreatedByUserID);

                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);

            command.Parameters.AddWithValue("@IssueDate", IssueDate);

            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            command.Parameters.AddWithValue("@IsActive", IsActive);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "InternationalLicenses", InternationalLicenseID);
                    EventLogger.LogInformation("AddNewInternationalLicense", $"Deactivated previous licenses for Driver ID {DriverID}");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewInternationalLicense", DriverID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewInternationalLicense", $"Error adding international license for Driver ID {DriverID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return InternationalLicenseID;
        }

        public static bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
             DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE InternationalLicenses
                           SET 
                              ApplicationID=@ApplicationID,
                              DriverID = @DriverID,
                              IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                              IssueDate = @IssueDate,
                              ExpirationDate = @ExpirationDate,
                              IsActive = @IsActive,
                              CreatedByUserID = @CreatedByUserID
                         WHERE InternationalLicenseID=@InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);

            command.Parameters.AddWithValue("@IssueDate", IssueDate);

            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            command.Parameters.AddWithValue("@IsActive", IsActive);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "InternationalLicenses", InternationalLicenseID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateInternationalLicense", $"International License ID {InternationalLicenseID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateInternationalLicense", InternationalLicenseID, ex);
                return false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateInternationalLicense", $"Error updating international license ID {InternationalLicenseID}", ex);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;

            string query = @"SELECT Top 1 InternationalLicenseID FROM InternationalLicenses 
                            where DriverID=@DriverID and GetDate() between IssueDate and ExpirationDate 
                            order by ExpirationDate Desc;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    InternationalLicenseID = insertedID;
                }
                else
                {
                    EventLogger.LogWarning("GetActiveInternationalLicenseIDByDriverID", $"No active international license found for Driver ID {DriverID}");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetActiveInternationalLicenseIDByDriverID", DriverID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetActiveInternationalLicenseIDByDriverID", $"Error getting active international license for Driver ID {DriverID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return InternationalLicenseID;
        }
    }
}