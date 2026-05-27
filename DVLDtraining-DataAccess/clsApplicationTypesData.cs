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

        static public bool UpdateApplicationType(int ID,string Title,float Fees)
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
