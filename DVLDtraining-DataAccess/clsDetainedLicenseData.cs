using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsDetainedLicenseData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        public static bool GetDetainedLicenseInfoByDetainID(int DetainID, ref int LicenseID, ref DateTime DetainDate, ref float FineFees, ref int CreatedByUserID,
            ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    LicenseID = (int)reader["LicenseID"];

                    DetainDate = (DateTime)reader["DetainDate"];

                    FineFees = Convert.ToSingle(reader["FineFees"]);

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    IsReleased = (bool)reader["IsReleased"];

                    if (reader["ReleaseDate"] == DBNull.Value)
                        ReleaseDate = DateTime.MaxValue;

                    else
                        ReleaseDate = (DateTime)reader["ReleaseDate"];

                    if (reader["ReleasedByUserID"] == DBNull.Value)
                        ReleasedByUserID = -1;

                    else
                        ReleasedByUserID = (int)reader["ReleasedByUserID"];

                    if (reader["ReleaseApplicationID"] == DBNull.Value)
                        ReleaseApplicationID = -1;

                    else
                        ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetDetainedLicenseInfoByDetainID", $"Detain ID {DetainID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetDetainedLicenseInfoByDetainID", DetainID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetDetainedLicenseInfoByDetainID", $"Error getting detain ID {DetainID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, ref int DetainID, ref DateTime DetainDate, ref float FineFees, ref int CreatedByUserID,
         ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            string query = "SELECT top 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID order by DetainID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    DetainID = (int)reader["DetainID"];

                    DetainDate = (DateTime)reader["DetainDate"];

                    FineFees = Convert.ToSingle(reader["FineFees"]);

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    IsReleased = (bool)reader["IsReleased"];

                    if (reader["ReleaseDate"] == DBNull.Value)
                        ReleaseDate = DateTime.MaxValue;

                    else
                        ReleaseDate = (DateTime)reader["ReleaseDate"];

                    if (reader["ReleasedByUserID"] == DBNull.Value)
                        ReleasedByUserID = -1;

                    else
                        ReleasedByUserID = (int)reader["ReleasedByUserID"];

                    if (reader["ReleaseApplicationID"] == DBNull.Value)
                        ReleaseApplicationID = -1;

                    else
                        ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetDetainedLicenseInfoByLicenseID", $"License ID {LicenseID} not found or not detained");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetDetainedLicenseInfoByLicenseID", LicenseID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetDetainedLicenseInfoByLicenseID", $"Error getting detain info for License ID {LicenseID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();

            string query = "select * from detainedLicenses_View order by IsReleased ,DetainID;";

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

                EventLogger.LogInformation("GetAllDetainedLicenses", $"Retrieved {dt.Rows.Count} detained licenses");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllDetainedLicenses", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllDetainedLicenses", "Error getting all detained licenses", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID)
        {
            int DetainID = -1;

            string query = @"INSERT INTO dbo.DetainedLicenses(LicenseID, DetainDate, FineFees,CreatedByUserID,IsReleased  )
                            VALUES(@LicenseID, @DetainDate,  @FineFees,  @CreatedByUserID, 0);
                            
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            command.Parameters.AddWithValue("@DetainDate", DetainDate);

            command.Parameters.AddWithValue("@FineFees", FineFees);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DetainID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "DetainedLicenses", DetainID);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewDetainedLicense", LicenseID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewDetainedLicense", $"Error adding detained license for License ID {LicenseID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return DetainID;
        }

        public static bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET LicenseID = @LicenseID, 
                              DetainDate = @DetainDate, 
                              FineFees = @FineFees,
                              CreatedByUserID = @CreatedByUserID   
                              WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            command.Parameters.AddWithValue("@DetainDate", DetainDate);

            command.Parameters.AddWithValue("@FineFees", FineFees);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "DetainedLicenses", DetainID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateDetainedLicense", $"Detain ID {DetainID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateDetainedLicense", DetainID, ex);
                return false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateDetainedLicense", $"Error updating detain ID {DetainID}", ex);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleasedByUserID = @ReleasedByUserID,
                              ReleaseApplicationID = @ReleaseApplicationID   
                              WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);

            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);

            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogInformation("ReleaseDetainedLicense", $"Detain ID {DetainID} released successfully");
                }
                else
                {
                    EventLogger.LogWarning("ReleaseDetainedLicense", $"Detain ID {DetainID} not found for release");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("ReleaseDetainedLicense", DetainID, ex);
                return false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("ReleaseDetainedLicense", $"Error releasing detain ID {DetainID}", ex);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select IsDetained = 1 from detainedLicenses  where  LicenseID = @LicenseID and IsReleased = 0;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    IsDetained = Convert.ToBoolean(result);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsLicenseDetained", LicenseID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsLicenseDetained", $"Error checking if License ID {LicenseID} is detained", ex);
            }
            finally
            {
                connection.Close();
            }

            return IsDetained;
        }
    }
}