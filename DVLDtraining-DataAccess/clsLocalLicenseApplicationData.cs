using DVLD_DataAccess;
using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDtraining_DataAccess
{
    public class clsLocalLicenseApplicationData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        static public bool GetLocalLicenseApplicationByID(int ID, ref int ApplicationID, ref int ClassID)
        {
            bool isFound = false;

            string query = "select * from LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];

                    ClassID = (int)reader["LicenseClassID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetLocalLicenseApplicationByID", $"Local License Application ID {ID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetLocalLicenseApplicationByID", ID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetLocalLicenseApplicationByID", $"Error getting local license application ID {ID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLocalLicenseApplicationByApplicationID(ref int ID, int ApplicationID, ref int ClassID)
        {
            bool isFound = false;

            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ID = (int)reader["LocalDrivingLicenseApplicationID"];

                    ClassID = (int)reader["LicenseClassID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetLocalLicenseApplicationByApplicationID", $"Application ID {ApplicationID} not found in Local Driving License Applications");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetLocalLicenseApplicationByApplicationID", ApplicationID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetLocalLicenseApplicationByApplicationID", $"Error getting local license application for Application ID {ApplicationID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllLocalLicenseApplications()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT * FROM LocalDrivingLicenseApplications_View order by ApplicationDate Desc";

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

                EventLogger.LogInformation("GetAllLocalLicenseApplications", $"Retrieved {dt.Rows.Count} local license applications");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllLocalLicenseApplications", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllLocalLicenseApplications", "Error getting all local license applications", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewLocalLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            string query = @"INSERT INTO LocalDrivingLicenseApplications ( ApplicationID,LicenseClassID)
                             VALUES (@ApplicationID,@LicenseClassID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "LocalDrivingLicenseApplications", LocalDrivingLicenseApplicationID);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewLocalLicenseApplication", ApplicationID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewLocalLicenseApplication", $"Error adding local license application for Application ID {ApplicationID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return LocalDrivingLicenseApplicationID;
        }

        public static bool UpdateLocalLicenseApplication(int ID, int ApplicationID, int ClassID)
        {
            int rowsAffected = 0;

            string query = @"Update  LocalDrivingLicenseApplications  
                            set ApplicationID = @ApplicationID,
                                LicenseClassID = @LicenseClassID
                            where LocalDrivingLicenseApplicationID=@ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            command.Parameters.AddWithValue("@LicenseClassID", ClassID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "LocalDrivingLicenseApplications", ID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateLocalLicenseApplication", $"Local License Application ID {ID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateLocalLicenseApplication", ID, ex);
                return false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateLocalLicenseApplication", $"Error updating local license application ID {ID}", ex);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteLocalLicenseApplication(int ID)
        {
            int rowsAffected = 0;

            string query = @"Delete LocalDrivingLicenseApplications 
                                where LocalDrivingLicenseApplicationID = @ID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("DELETE", "LocalDrivingLicenseApplications", ID);
                }
                else
                {
                    EventLogger.LogWarning("DeleteLocalLicenseApplication", $"Local License Application ID {ID} not found for deletion");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("DeleteLocalLicenseApplication", ID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("DeleteLocalLicenseApplication", $"Error deleting local license application ID {ID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool DoesAttendTestType(int LocalLicenseApplicationID, int TestTypeID)
        {
            bool IsFound = false;

            string query = @" SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    IsFound = true;
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("DoesAttendTestType", LocalLicenseApplicationID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("DoesAttendTestType", $"Error checking test attendance for Local License Application ID {LocalLicenseApplicationID}, Test Type {TestTypeID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static byte TotalTrialsPerTest(int LocalLicenseApplicationID, int TestTypeID)
        {
            byte TotalTrialsPerTest = 0;

            string query = @" SELECT TotalTrialsPerTest = count(TestID)
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte Trials))
                {
                    TotalTrialsPerTest = Trials;
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("TotalTrialsPerTest", LocalLicenseApplicationID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("TotalTrialsPerTest", $"Error getting total trials for Local License Application ID {LocalLicenseApplicationID}, Test Type {TestTypeID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return TotalTrialsPerTest;
        }

        public static bool IsThereAnActiveScheduledTest(int LocalLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;

            string query = @" SELECT top 1 Found=1
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID)  
                            AND(TestAppointments.TestTypeID = @TestTypeID) and isLocked=0
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                {
                    Result = true;
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsThereAnActiveScheduledTest", LocalLicenseApplicationID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsThereAnActiveScheduledTest", $"Error checking active scheduled test for Local License Application ID {LocalLicenseApplicationID}, Test Type {TestTypeID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return Result;
        }

        public static bool DoesPassTestType(int LocalLicenseApplicationID, int TestTypeID)
        {
            bool Result = false;

            string query = @" SELECT top 1 TestResult
                            FROM LocalDrivingLicenseApplications INNER JOIN
                                 TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                 Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE
                            (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalLicenseApplicationID) 
                            AND(TestAppointments.TestTypeID = @TestTypeID)
                            ORDER BY TestAppointments.TestAppointmentID desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && bool.TryParse(result.ToString(), out bool returnedResult))
                {
                    Result = returnedResult;
                }
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("DoesPassTestType", LocalLicenseApplicationID, ex);
            }

            catch (Exception ex)
            {
                EventLogger.LogError("DoesPassTestType", $"Error checking if passed test for Local License Application ID {LocalLicenseApplicationID}, Test Type {TestTypeID}", ex);
            }

            finally
            {
                connection.Close();
            }

            return Result;
        }
    }
}