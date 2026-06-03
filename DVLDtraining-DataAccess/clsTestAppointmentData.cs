using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        public static bool GetTestAppointmentInfoByTestAppointmentID(int TestAppointmentID, ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestTypeID = (int)reader["TestTypeID"];

                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];

                    AppointmentDate = (DateTime)reader["AppointmentDate"];

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    PaidFees = Convert.ToSingle(reader["PaidFees"]);

                    IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                        RetakeTestApplicationID = -1;

                    else
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetTestAppointmentInfoByTestAppointmentID", $"Test Appointment ID {TestAppointmentID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetTestAppointmentInfoByTestAppointmentID", TestAppointmentID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetTestAppointmentInfoByTestAppointmentID", $"Error getting test appointment ID {TestAppointmentID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLastTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID, ref int TestAppointmentID, ref DateTime AppointmentDate,
            ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            string query = @"SELECT top 1 * FROM TestAppointments WHERE (TestTypeID = @TestTypeID) and (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                                order by TestAppointmentID Desc";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestAppointmentID = (int)reader["TestAppointmentID"];

                    AppointmentDate = (DateTime)reader["AppointmentDate"];

                    PaidFees = Convert.ToSingle(reader["PaidFees"]);

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    IsLocked = (bool)reader["IsLocked"];

                    if (reader["RetakeTestApplicationID"] == DBNull.Value)
                        RetakeTestApplicationID = -1;

                    else
                        RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetLastTestAppointment", $"No test appointment found for Local License Application ID {LocalDrivingLicenseApplicationID}, Test Type {TestTypeID}");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetLastTestAppointment", LocalDrivingLicenseApplicationID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetLastTestAppointment", $"Error getting last test appointment for Local License Application ID {LocalDrivingLicenseApplicationID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select * from TestAppointments_View
                                  order by AppointmentDate Desc";

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

                EventLogger.LogInformation("GetAllTestAppointments", $"Retrieved {dt.Rows.Count} test appointments");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllTestAppointments", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllTestAppointments", "Error getting all test appointments", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT TestAppointmentID, AppointmentDate,PaidFees, IsLocked FROM TestAppointments WHERE (TestTypeID = @TestTypeID) 
                                AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                        order by TestAppointmentID desc;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();

                EventLogger.LogInformation("GetApplicationTestAppointmentsPerTestType", $"Retrieved {dt.Rows.Count} test appointments for Local License Application ID {LocalDrivingLicenseApplicationID}, Test Type {TestTypeID}");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetApplicationTestAppointmentsPerTestType", LocalDrivingLicenseApplicationID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetApplicationTestAppointmentsPerTestType", $"Error getting test appointments for Local License Application ID {LocalDrivingLicenseApplicationID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
            float PaidFees, int CreatedByUserID, int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;

            string query = @"Insert Into TestAppointments (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID)
                            Values (@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,0,@RetakeTestApplicationID);
                
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);

            command.Parameters.AddWithValue("@PaidFees", PaidFees);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            if (RetakeTestApplicationID == -1)
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestAppointmentID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "TestAppointments", TestAppointmentID);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewTestAppointment", LocalDrivingLicenseApplicationID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewTestAppointment", $"Error adding test appointment for Local License Application ID {LocalDrivingLicenseApplicationID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
            float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int rowsAffected = 0;

            string query = @"Update  TestAppointments  
                            set TestTypeID = @TestTypeID,
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                AppointmentDate = @AppointmentDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID,
                                IsLocked=@IsLocked,
                                RetakeTestApplicationID=@RetakeTestApplicationID
                                where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);

            command.Parameters.AddWithValue("@PaidFees", PaidFees);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            command.Parameters.AddWithValue("@IsLocked", IsLocked);

            if (RetakeTestApplicationID == -1)
                command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);

            else
                command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "TestAppointments", TestAppointmentID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateTestAppointment", $"Test Appointment ID {TestAppointmentID} not found for update");
                }
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateTestAppointment", TestAppointmentID, ex);

                return false;
            }

            catch (Exception ex)
            {
                EventLogger.LogError("UpdateTestAppointment", $"Error updating test appointment ID {TestAppointmentID}", ex);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool LockTestAppointment(int TestAppointmentID)
        {
            int rowsAffected = 0;

            string query = @"Update TestAppointments set IsLocked = 1 where TestAppointmentID = @TestAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogInformation("LockTestAppointment", $"Test Appointment ID {TestAppointmentID} locked successfully");
                }
                else
                {
                    EventLogger.LogWarning("LockTestAppointment", $"Test Appointment ID {TestAppointmentID} not found for locking");
                }
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("LockTestAppointment", TestAppointmentID, ex);

                return false;
            }

            catch (Exception ex)
            {
                EventLogger.LogError("LockTestAppointment", $"Error locking test appointment ID {TestAppointmentID}", ex);

                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static int GetTestIDByTestAppointmentID(int TestAppointmentID)
        {
            int TestID = -1;

            string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
                else
                {
                    EventLogger.LogWarning("GetTestIDByTestAppointmentID", $"No test found for Test Appointment ID {TestAppointmentID}");
                }
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetTestIDByTestAppointmentID", TestAppointmentID, ex);
            }

            catch (Exception ex)
            {
                EventLogger.LogError("GetTestIDByTestAppointmentID", $"Error getting test ID for Test Appointment ID {TestAppointmentID}", ex);
            }

            finally
            {
                connection.Close();
            }

            return TestID;
        }
    }
}