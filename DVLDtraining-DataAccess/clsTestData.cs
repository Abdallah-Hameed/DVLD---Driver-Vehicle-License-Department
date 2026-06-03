using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsTestData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        public static bool GetTestInfoByID(int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];

                    TestResult = (bool)reader["TestResult"];

                    if (reader["Notes"] == DBNull.Value)
                        Notes = "";

                    else
                        Notes = (string)reader["Notes"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetTestInfoByID", $"Test ID {TestID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetTestInfoByID", TestID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetTestInfoByID", $"Error getting test ID {TestID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLastTestByPersonIDAndLicenseClassIDAndTestTypeID(int PersonID, int LicenseClassID, int TestTypeID, ref int TestID,
                                ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            string query = @"SELECT  top 1 Tests.TestID, 
                Tests.TestAppointmentID, Tests.TestResult, 
			    Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                FROM            LocalDrivingLicenseApplications INNER JOIN
                                         Tests INNER JOIN
                                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                            ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
                            INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                WHERE        (Applications.ApplicantPersonID = @PersonID) 
                        AND (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                        AND ( TestAppointments.TestTypeID=@TestTypeID)
                ORDER BY Tests.TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestID = (int)reader["TestID"];

                    TestAppointmentID = (int)reader["TestAppointmentID"];

                    TestResult = (bool)reader["TestResult"];

                    if (reader["Notes"] == DBNull.Value)
                        Notes = "";

                    else
                        Notes = (string)reader["Notes"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetLastTestByPersonIDAndLicenseClassIDAndTestTypeID",
                        $"No test found for Person ID {PersonID}, License Class {LicenseClassID}, Test Type {TestTypeID}");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetLastTestByPersonIDAndLicenseClassIDAndTestTypeID", PersonID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetLastTestByPersonIDAndLicenseClassIDAndTestTypeID",
                    $"Error getting last test for Person ID {PersonID}, License Class {LicenseClassID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM Tests order by TestID";

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

                EventLogger.LogInformation("GetAllTests", $"Retrieved {dt.Rows.Count} tests");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllTests", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllTests", "Error getting all tests", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            string query = @"Insert Into Tests (TestAppointmentID,TestResult, Notes,   CreatedByUserID)
                            Values (@TestAppointmentID,@TestResult, @Notes,   @CreatedByUserID);
                            
                                UPDATE TestAppointments  SET IsLocked = 1 where TestAppointmentID = @TestAppointmentID;

                                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            command.Parameters.AddWithValue("@TestResult", TestResult);

            if (Notes != "" && Notes != null)
                command.Parameters.AddWithValue("@Notes", Notes);

            else
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "Tests", TestID);
                    EventLogger.LogInformation("AddNewTest", $"Test added for Appointment ID {TestAppointmentID}, Result: {TestResult}");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewTest", TestAppointmentID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewTest", $"Error adding test for Appointment ID {TestAppointmentID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return TestID;
        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"Update  Tests   set TestAppointmentID = @TestAppointmentID,TestResult=@TestResult,
                                Notes = @Notes, CreatedByUserID = @CreatedByUserID where TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestID", TestID);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            command.Parameters.AddWithValue("@TestResult", TestResult);

            command.Parameters.AddWithValue("@Notes", Notes);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "Tests", TestID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateTest", $"Test ID {TestID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateTest", TestID, ex);
                return false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateTest", $"Error updating test ID {TestID}", ex);
                return false;
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static byte GetPassedTestCount(int LocalLicenseApplicationID)
        {
            byte PassedTestCount = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"SELECT PassedTestCount = count(TestTypeID) FROM Tests INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                                where LocalDrivingLicenseApplicationID =@LocalLicenseApplicationID and TestResult=1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                {
                    PassedTestCount = ptCount;
                }
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetPassedTestCount", LocalLicenseApplicationID, ex);
            }

            catch (Exception ex)
            {
                EventLogger.LogError("GetPassedTestCount", $"Error getting passed test count for Local License Application ID {LocalLicenseApplicationID}", ex);
            }

            finally
            {
                connection.Close();
            }

            return PassedTestCount;
        }
    }
}