using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
            ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];

                    DriverID = (int)reader["DriverID"];

                    LicenseClass = (int)reader["LicenseClass"];

                    IssueDate = (DateTime)reader["IssueDate"];

                    ExpirationDate = (DateTime)reader["ExpirationDate"];

                    if (reader["Notes"] == DBNull.Value)
                        Notes = "";

                    else
                        Notes = (string)reader["Notes"];

                    PaidFees = Convert.ToSingle(reader["PaidFees"]);

                    IsActive = (bool)reader["IsActive"];

                    IssueReason = (byte)reader["IssueReason"];

                    CreatedByUserID = (int)reader["DriverID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetLicenseInfoByID", $"License ID {LicenseID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetLicenseInfoByID", LicenseID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetLicenseInfoByID", $"Error getting license ID {LicenseID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM Licenses";

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

                EventLogger.LogInformation("GetAllLicenses", $"Retrieved {dt.Rows.Count} licenses");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllLicenses", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllLicenses", "Error getting all licenses", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT Licenses.LicenseID, ApplicationID,LicenseClasses.ClassName, Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive
                           FROM Licenses INNER JOIN LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID=@DriverID
                            Order By IsActive Desc, ExpirationDate Desc";

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

                EventLogger.LogInformation("GetDriverLicenses", $"Retrieved {dt.Rows.Count} licenses for Driver ID {DriverID}");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetDriverLicenses", DriverID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetDriverLicenses", $"Error getting licenses for Driver ID {DriverID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
            string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;

            string query = @"INSERT INTO Licenses (ApplicationID, DriverID,LicenseClass,IssueDate,ExpirationDate, Notes, PaidFees,IsActive,IssueReason, CreatedByUserID)
                         VALUES (@ApplicationID, @DriverID, @LicenseClass, @IssueDate,@ExpirationDate,@Notes, @PaidFees,@IsActive,@IssueReason,  @CreatedByUserID);

                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);

            command.Parameters.AddWithValue("@IssueDate", IssueDate);

            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            if (Notes == "")
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            else
                command.Parameters.AddWithValue("@Notes", Notes);

            command.Parameters.AddWithValue("@PaidFees", PaidFees);

            command.Parameters.AddWithValue("@IsActive", IsActive);

            command.Parameters.AddWithValue("@IssueReason", IssueReason);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "Licenses", LicenseID);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewLicense", DriverID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewLicense", $"Error adding license for Driver ID {DriverID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
            string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {

            int rowsAffected = 0;

            string query = @"UPDATE Licenses
                           SET ApplicationID=@ApplicationID, DriverID = @DriverID,
                              LicenseClass = @LicenseClass,
                              IssueDate = @IssueDate,
                              ExpirationDate = @ExpirationDate,
                              Notes = @Notes,
                              PaidFees = @PaidFees,
                              IsActive = @IsActive,IssueReason=@IssueReason,
                              CreatedByUserID = @CreatedByUserID
                         WHERE LicenseID=@LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            command.Parameters.AddWithValue("@DriverID", DriverID);

            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);

            command.Parameters.AddWithValue("@IssueDate", IssueDate);

            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            if (Notes == "")
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            else
                command.Parameters.AddWithValue("@Notes", Notes);

            command.Parameters.AddWithValue("@PaidFees", PaidFees);

            command.Parameters.AddWithValue("@IsActive", IsActive);

            command.Parameters.AddWithValue("@IssueReason", IssueReason);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "Licenses", LicenseID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateLicense", $"License ID {LicenseID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateLicense", LicenseID, ex);
                return false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateLicense", $"Error updating license ID {LicenseID}", ex);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            string query = @"SELECT Licenses.LicenseID
                            FROM Licenses INNER JOIN Drivers ON Licenses.DriverID = Drivers.DriverID
                            WHERE Licenses.LicenseClass = @LicenseClass AND Drivers.PersonID = @PersonID And IsActive=1;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }
                else
                {
                    EventLogger.LogWarning("GetActiveLicenseIDByPersonID", $"No active license found for Person ID {PersonID}, License Class {LicenseClassID}");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetActiveLicenseIDByPersonID", PersonID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetActiveLicenseIDByPersonID", $"Error getting active license for Person ID {PersonID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE Licenses SET IsActive = 0 WHERE LicenseID=@LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogInformation("DeactivateLicense", $"License ID {LicenseID} deactivated successfully");
                }
                else
                {
                    EventLogger.LogWarning("DeactivateLicense", $"License ID {LicenseID} not found for deactivation");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("DeactivateLicense", LicenseID, ex);
                return false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("DeactivateLicense", $"Error deactivating license ID {LicenseID}", ex);
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