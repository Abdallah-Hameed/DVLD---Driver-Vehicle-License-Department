using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDtraining_DataAccess
{
    public class clsTestTypeData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        static public bool GetTestTypeByTestTypeID(int TestTypeID, ref string Title,ref string Description, ref float Fees)
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
            }

            catch
            {
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

            string query = @"Insert Into TestTypes (TestTypeTitle,TestTypeTitle,TestTypeFees)
                            Values (@TestTypeTitle,@TestTypeDescription,@ApplicationFees)
                            where TestTypeID = @TestTypeID;

                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeTitle", Title);

            command.Parameters.AddWithValue("@TestTypeDescription", Description);

            command.Parameters.AddWithValue("@ApplicationFees", Fees);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestTypeID = insertedID;
                }
            }

            catch
            {
            }

            finally
            {
                connection.Close();
            }

            return TestTypeID;
        }

        static public bool UpdateTestType(int ID, string Title,string Description, float Fees)
        {
            int rowsAffected = 0;

            string Query = @"Update TestTypes 
                                Set TestTypeTitle = @Title ,TestTypeDescription = @Description, TestTypeFees = @Fees where TestTypeID = @ID;";

            SqlCommand command = new SqlCommand(Query, connection);

            command.Parameters.AddWithValue("@ID", ID);

            command.Parameters.AddWithValue("@Title", Title);

            command.Parameters.AddWithValue("@Description",Description);

            command.Parameters.AddWithValue("@Fees", Fees);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch
            {

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
            }

            catch
            {

            }

            finally
            {
                connection.Close();
            }

            return dt;
        }
    }
}