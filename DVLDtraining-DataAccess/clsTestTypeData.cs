using DVLD_DataAccess;
using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDtraining_DataAccess
{
    public class clsTestTypeData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        static public bool GetTestTypeByTestTypeID(int TestTypeID, ref string Title, ref string Description, ref float Fees)
        {
            bool isFound = false;

            string Query = "select * from TestTypes where TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Title = (string)reader["TestTypeTitle"];

                    Fees = Convert.ToSingle(reader["TestTypeFees"]);

                    Description = (string)reader["TestTypeDescription"];

                    isFound = true;
                }
                else
                {
                    EventLogger.LogWarning("GetTestTypeByTestTypeID", $"Test Type ID {TestTypeID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetTestTypeByTestTypeID", TestTypeID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetTestTypeByTestTypeID", $"Error getting test type ID {TestTypeID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            int TestTypeID = -1;

            // ملاحظة: هناك خطأ في الـ query الأصلي (كتابة خاطئة للأعمدة)
            string query = @"Insert Into TestTypes (TestTypeTitle,TestTypeDescription,TestTypeFees)
                            Values (@TestTypeTitle,@TestTypeDescription,@TestTypeFees);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeTitle", Title);

            command.Parameters.AddWithValue("@TestTypeDescription", Description);

            command.Parameters.AddWithValue("@TestTypeFees", Fees);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestTypeID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "TestTypes", TestTypeID);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewTestType", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewTestType", $"Error adding test type '{Title}'", ex);
            }
            finally
            {
                connection.Close();
            }

            return TestTypeID;
        }

        static public bool UpdateTestType(int ID, string Title, string Description, float Fees)
        {
            int rowsAffected = 0;

            string Query = @"Update TestTypes 
                                Set TestTypeTitle = @Title ,TestTypeDescription = @Description, TestTypeFees = @Fees where TestTypeID = @ID;";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            command.Parameters.AddWithValue("@Title", Title);

            command.Parameters.AddWithValue("@Description", Description);

            command.Parameters.AddWithValue("@Fees", Fees);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "TestTypes", ID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateTestType", $"Test Type ID {ID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateTestType", ID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateTestType", $"Error updating test type ID {ID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        static public DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();

            string Query = "Select * from TestTypes";

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

                EventLogger.LogInformation("GetAllTestTypes", $"Retrieved {dt.Rows.Count} test types");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllTestTypes", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllTestTypes", "Error getting all test types", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }
    }
}