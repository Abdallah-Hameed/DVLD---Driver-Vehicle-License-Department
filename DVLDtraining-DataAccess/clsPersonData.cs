using DVLD_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDTraining_DataAccess
{
    public class clsPersonData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName, ref string NationalNo,
           ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            string query = "select * from People where PersonID=@PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    FirstName = (string)reader["FirstName"];

                    SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }

                    else
                    {
                        ThirdName = string.Empty;
                    }

                    LastName = (string)reader["LastName"];

                    NationalNo = (string)reader["NationalNo"];

                    DateOfBirth = (DateTime)reader["DateOfBirth"];

                    Phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }

                    else
                    {
                        Email = string.Empty;
                    }

                    Address = (string)reader["Address"];

                    Gendor = (byte)reader["Gendor"];

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }

                    else
                    {
                        ImagePath = string.Empty;
                    }
                }
                else
                {
                    EventLogger.LogWarning("GetPersonInfoByID", $"Person ID {PersonID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetPersonInfoByID", PersonID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetPersonInfoByID", $"Error getting person ID {PersonID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
           ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            string query = "select * from People where NationalNo=@NationalNo;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = (int)reader["PersonID"];

                    FirstName = (string)reader["FirstName"];

                    SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"];
                    }

                    else
                    {
                        ThirdName = string.Empty;
                    }

                    LastName = (string)reader["LastName"];

                    DateOfBirth = (DateTime)reader["DateOfBirth"];

                    Phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"];
                    }

                    else
                    {
                        Email = string.Empty;
                    }

                    Address = (string)reader["Address"];

                    Gendor = (byte)reader["Gendor"];

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"];
                    }

                    else
                    {
                        ImagePath = string.Empty;
                    }
                }
                else
                {
                    EventLogger.LogWarning("GetPersonInfoByNationalNo", $"National No '{NationalNo}' not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetPersonInfoByNationalNo", 0, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetPersonInfoByNationalNo", $"Error getting person by National No '{NationalNo}'", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewPerson(string FirstName, string SecondName, string ThirdName, string LastName, string NationalNo,
           DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;

            string query = "insert into People (FirstName,SecondName,ThirdName,LastName,NationalNo,DateOfBirth,Gendor,Address,Phone,Email,NationalityCountryID,ImagePath) " +
                "values(@FirstName,@SecondName,@ThirdName,@LastName,@NationalNo,@DateOfBirth,@Gendor,@Address,@Phone,@Email,@NationalityCountryID,@ImagePath);" +
                "select scope_identity();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FirstName", FirstName);

            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "" && ThirdName != null)
            {
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);

            command.Parameters.AddWithValue("@Gendor", Gendor);

            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
            {
                command.Parameters.AddWithValue("@Email", Email);
            }
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@Address", Address);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath != "" && ImagePath != null)
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "People", PersonID);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewPerson", 0, ex);
                throw new Exception("Error: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewPerson", $"Error adding person with National No '{NationalNo}'", ex);
                throw new Exception("Error: " + ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName, string NationalNo,
           DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int RowsAffected = 0;

            string query = "Update People Set " +
                "FirstName=@FirstName," +
                "SecondName = @SecondName," +
                "ThirdName = @ThirdName," +
                "LastName = @LastName," +
                "NationalNo = @NationalNo," +
                "DateOfBirth = @DateOfBirth," +
                "Gendor=@Gendor," +
                "Address = @Address," +
                "Phone = @Phone," +
                "Email = @Email," +
                "NationalityCountryID = @NationalityCountryID," +
                "ImagePath =@ImagePath " +
                "where PersonID=@PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@FirstName", FirstName);

            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "" && ThirdName != null)
            {
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);

            command.Parameters.AddWithValue("@Gendor", Gendor);

            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
            {
                command.Parameters.AddWithValue("@Email", Email);
            }
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@Address", Address);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath != "" && ImagePath != null)
            {
                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

            try
            {
                connection.Open();

                RowsAffected = command.ExecuteNonQuery();

                if (RowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "People", PersonID);
                }
                else
                {
                    EventLogger.LogWarning("UpdatePerson", $"Person ID {PersonID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdatePerson", PersonID, ex);
                throw new Exception("Error: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdatePerson", $"Error updating person ID {PersonID}", ex);
                throw new Exception("Error: " + ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
        }

        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT People.PersonID, People.NationalNo,
              People.FirstName, People.SecondName, People.ThirdName, People.LastName,
			  People.DateOfBirth, People.Gendor,  
				  CASE
                  WHEN People.Gendor = 0 THEN 'Male'
                  ELSE 'Female'
                  END as GendorCaption ,
			  People.Address, People.Phone, People.Email, 
              People.NationalityCountryID, Countries.CountryName, People.ImagePath
              FROM            People INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                ORDER BY People.PersonID";

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

                EventLogger.LogInformation("GetAllPeople", $"Retrieved {dt.Rows.Count} people");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllPeople", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllPeople", "Error getting all people", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool DeletePerson(int PersonID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete People where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("DELETE", "People", PersonID);
                }
                else
                {
                    EventLogger.LogWarning("DeletePerson", $"Person ID {PersonID} not found for deletion");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("DeletePerson", PersonID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("DeletePerson", $"Error deleting person ID {PersonID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found = 1 FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsPersonExist", PersonID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsPersonExist", $"Error checking if person ID {PersonID} exists", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsPersonExist", 0, ex);

                isFound = false;
            }

            catch (Exception ex)
            {
                EventLogger.LogError("IsPersonExist", $"Error checking if National No '{NationalNo}' exists", ex);

                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}