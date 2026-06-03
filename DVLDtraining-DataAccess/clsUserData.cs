using DVLD_DataAccess;
using DVLDTraining_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDtraining_DataAccess
{
    public class clsUserData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        static public bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            string query = "select top 1 * from Users where UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];

                    UserName = (string)reader["UserName"];

                    Password = (string)reader["Password"];

                    IsActive = (bool)reader["IsActive"];

                    isFound = true;
                }
                else
                {
                    EventLogger.LogWarning("GetUserInfoByUserID", $"User ID {UserID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetUserInfoByUserID", UserID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetUserInfoByUserID", $"Error getting user ID {UserID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool GetUserInfoByUserName(ref int UserID, ref int PersonID, string UserName, ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            string query = "select top 1 * from Users where UserName = @UserName;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserID = (int)reader["UserID"];

                    PersonID = (int)reader["PersonID"];

                    Password = (string)reader["Password"];

                    IsActive = (bool)reader["IsActive"];

                    isFound = true;
                }
                else
                {
                    EventLogger.LogWarning("GetUserInfoByUserName", $"Username '{UserName}' not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetUserInfoByUserName", 0, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetUserInfoByUserName", $"Error getting user by username '{UserName}'", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool GetUserInfoByUserNameAndPassword(ref int UserID, ref int PersonID, string UserName, string Password, ref bool IsActive)
        {
            bool isFound = false;

            string query = "select top 1 * from Users where UserName = @UserName and Password = @Password;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            command.Parameters.AddWithValue("@Password", Password);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserID = (int)reader["UserID"];

                    PersonID = (int)reader["PersonID"];

                    IsActive = (bool)reader["IsActive"];

                    isFound = true;
                }
                else
                {
                    EventLogger.LogWarning("GetUserInfoByUserNameAndPassword", $"Username '{UserName}' not found or password incorrect");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetUserInfoByUserNameAndPassword", 0, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetUserInfoByUserNameAndPassword", $"Error getting user by username and password", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool GetUserInfoByPersonID(ref int UserID, int PersonID, ref string UserName, ref string Password, ref bool IsActive)
        {
            bool isFound = false;

            string query = "select top 1 * from Users where PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    UserID = (int)reader["UserID"];

                    UserName = (string)reader["UserName"];

                    Password = (string)reader["Password"];

                    IsActive = (bool)reader["IsActive"];

                    isFound = true;
                }
                else
                {
                    EventLogger.LogWarning("GetUserInfoByPersonID", $"No user found for Person ID {PersonID}");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetUserInfoByPersonID", PersonID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetUserInfoByPersonID", $"Error getting user for Person ID {PersonID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;

            string query = @"insert into Users (PersonID,UserName,Password,IsActive) values(@PersonID,@UserName,@Password,@IsActive);
                                Select Scope_Identity();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@UserName", UserName);

            command.Parameters.AddWithValue("@Password", Password);

            command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                connection.Open();

                object RowsAffected = command.ExecuteScalar();

                if (RowsAffected != null && int.TryParse(RowsAffected.ToString(), out int insertedID))
                {
                    UserID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "Users", UserID);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewUser", PersonID, ex);
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewUser", $"Error adding user for Person ID {PersonID}", ex);
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }

            return UserID;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            int RowsAffected = 0;

            string query = "update Users set PersonID = @PersonID,UserName = @UserName,Password = @password,IsActive = @IsActive where UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            command.Parameters.AddWithValue("@UserName", UserName);

            command.Parameters.AddWithValue("@Password", Password);

            command.Parameters.AddWithValue("@IsActive", IsActive);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                RowsAffected = command.ExecuteNonQuery();

                if (RowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "Users", UserID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateUser", $"User ID {UserID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateUser", UserID, ex);
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateUser", $"Error updating user ID {UserID}", ex);
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
        }

        static public bool DeleteUser(int UserID)
        {
            int RowsAffected = 0;

            string query = "Delete from Users where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                RowsAffected = command.ExecuteNonQuery();

                if (RowsAffected > 0)
                {
                    EventLogger.LogDataOperation("DELETE", "Users", UserID);
                }
                else
                {
                    EventLogger.LogWarning("DeleteUser", $"User ID {UserID} not found for deletion");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("DeleteUser", UserID, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("DeleteUser", $"Error deleting user ID {UserID}", ex);
            }
            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
        }

        static public DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();

            string query = @"Select Users.UserID,Users.PersonID,
                                FullName = People.FirstName + ' ' + People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName,
                                Users.UserName,Users.IsActive
                                From Users inner join People
                                On Users.PersonID = People.PersonID";

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

                EventLogger.LogInformation("GetAllUsers", $"Retrieved {dt.Rows.Count} users");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllUsers", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllUsers", "Error getting all users", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        static public DataTable GetActiveUsers()
        {
            DataTable dt = new DataTable();

            string query = "Select * from Users where IsActive = 1";

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

                EventLogger.LogInformation("GetActiveUsers", $"Retrieved {dt.Rows.Count} active users");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetActiveUsers", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetActiveUsers", "Error getting active users", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        static public bool IsUserExistsByUserNameAndPassword(string UserName, string Password)
        {
            string query = "select Found = 1 from Users where UserName = @UserName and Password = @Password";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            command.Parameters.AddWithValue("@Password", Password);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                if (!isFound)
                {
                    EventLogger.LogWarning("IsUserExistsByUserNameAndPassword", $"User '{UserName}' not found or password incorrect");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsUserExistsByUserNameAndPassword", 0, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsUserExistsByUserNameAndPassword", $"Error checking user by username and password", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool IsUserExistsByUserID(int UserID)
        {
            string query = "select Found = 1 from Users where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsUserExistsByUserID", UserID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsUserExistsByUserID", $"Error checking if user ID {UserID} exists", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool IsUserExistsByPeronID(int PersonID)
        {
            string query = "select Found = 1 from Users where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsUserExistsByPeronID", PersonID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsUserExistsByPeronID", $"Error checking if user exists for Person ID {PersonID}", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool IsUserExistsByUserName(string UserName)
        {
            string query = "select Found = 1 from Users where UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsUserExistsByUserName", 0, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsUserExistsByUserName", $"Error checking if username '{UserName}' exists", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool IsUserActiveByUsername(string UserName)
        {
            string query = "select Found = 1 from Users where UserName = @UserName and IsActive = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsUserActiveByUsername", 0, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsUserActiveByUsername", $"Error checking if user '{UserName}' is active", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool IsUserActiveByUserID(int UserID)
        {
            string query = "select Found = 1 from Users where UserID = @UserID and IsActive = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsUserActiveByUserID", UserID, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsUserActiveByUserID", $"Error checking if user ID {UserID} is active", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool IsUserActiveByUsernameAndPassword(string UserName, string Password)
        {
            string query = "select Found = 1 from Users where UserName = @UserName and Password = @Password and IsActive = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            command.Parameters.AddWithValue("@Password", Password);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsUserActiveByUsernameAndPassword", 0, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("IsUserActiveByUsernameAndPassword", $"Error checking if user '{UserName}' is active", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        static public bool IsUserActiveByPersonID(int PersonID)
        {
            string query = "select Found = 1 from Users where PersonID = @PersonID and IsActive = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            bool isFound = false;

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("IsUserActiveByPersonID", PersonID, ex);

                isFound = false;
            }

            catch (Exception ex)
            {
                EventLogger.LogError("IsUserActiveByPersonID", $"Error checking if user for Person ID {PersonID} is active", ex);

                isFound = false;
            }

            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
            int RowsAffected = 0;

            string query = "update Users set Password = @Password where UserID = @UserID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            command.Parameters.AddWithValue("@Password", NewPassword);

            try
            {
                connection.Open();

                RowsAffected = command.ExecuteNonQuery();

                if (RowsAffected > 0)
                {
                    EventLogger.LogInformation("ChangePassword", $"Password changed for User ID {UserID}");
                }

                else
                {
                    EventLogger.LogWarning("ChangePassword", $"User ID {UserID} not found for password change");
                }
            }

            catch (SqlException ex)
            {
                EventLogger.LogSqlError("ChangePassword", UserID, ex);

                throw new Exception(ex.Message, ex);
            }

            catch (Exception ex)
            {
                EventLogger.LogError("ChangePassword", $"Error changing password for User ID {UserID}", ex);

                throw new Exception(ex.Message, ex);
            }

            finally
            {
                connection.Close();
            }

            return (RowsAffected > 0);
        }
    }
}