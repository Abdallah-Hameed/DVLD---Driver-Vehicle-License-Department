using DVLDTraining_DataAccess;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsLicenseClassData
    {
        static SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

        public static bool GetLicenseClassInfoByID(int LicenseClassID, ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ClassName = (string)reader["ClassName"];

                    ClassDescription = (string)reader["ClassDescription"];

                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];

                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];

                    ClassFees = Convert.ToSingle(reader["ClassFees"]);
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetLicenseClassInfoByID", $"License Class ID {LicenseClassID} not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetLicenseClassInfoByID", LicenseClassID, ex);
                isFound = false;
                throw new Exception(ex.Message, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetLicenseClassInfoByID", $"Error getting license class ID {LicenseClassID}", ex);
                isFound = false;
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID, ref string ClassDescription, ref byte MinimumAllowedAge, ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            string query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", ClassName);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    LicenseClassID = (int)reader["LicenseClassID"];

                    ClassDescription = (string)reader["ClassDescription"];

                    MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];

                    DefaultValidityLength = (byte)reader["DefaultValidityLength"];

                    ClassFees = Convert.ToSingle(reader["ClassFees"]);
                }
                else
                {
                    isFound = false;
                    EventLogger.LogWarning("GetLicenseClassInfoByClassName", $"License Class Name '{ClassName}' not found");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetLicenseClassInfoByClassName", 0, ex);
                isFound = false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetLicenseClassInfoByClassName", $"Error getting license class by name '{ClassName}'", ex);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllLicenseClasses()
        {
            DataTable dt = new DataTable();

            string query = "SELECT * FROM LicenseClasses order by ClassName";

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

                EventLogger.LogInformation("GetAllLicenseClasses", $"Retrieved {dt.Rows.Count} license classes");
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("GetAllLicenseClasses", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("GetAllLicenseClasses", "Error getting all license classes", ex);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static int AddNewLicenseClass(string ClassName, string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;

            // ملاحظة: هناك خطأ في الـ query الأصلي - وجود WHERE بعد VALUES
            string query = @"Insert Into LicenseClasses  (ClassName,ClassDescription,MinimumAllowedAge, DefaultValidityLength,ClassFees)
                            Values ( @ClassName,@ClassDescription,@MinimumAllowedAge, @DefaultValidityLength,@ClassFees);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClassName", ClassName);

            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);

            command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);

            command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);

            command.Parameters.AddWithValue("@ClassFees", ClassFees);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseClassID = insertedID;
                    EventLogger.LogDataOperation("INSERT", "LicenseClasses", LicenseClassID);
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("AddNewLicenseClass", 0, ex);
            }
            catch (Exception ex)
            {
                EventLogger.LogError("AddNewLicenseClass", $"Error adding license class '{ClassName}'", ex);
            }
            finally
            {
                connection.Close();
            }

            return LicenseClassID;
        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, string ClassDescription, byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int rowsAffected = 0;

            string query = @"Update  LicenseClasses  
                            set ClassName = @ClassName,
                                ClassDescription = @ClassDescription,
                                MinimumAllowedAge = @MinimumAllowedAge,
                                DefaultValidityLength = @DefaultValidityLength,
                                ClassFees = @ClassFees
                                where LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            command.Parameters.AddWithValue("@ClassName", ClassName);

            command.Parameters.AddWithValue("@ClassDescription", ClassDescription);

            command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);

            command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);

            command.Parameters.AddWithValue("@ClassFees", ClassFees);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    EventLogger.LogDataOperation("UPDATE", "LicenseClasses", LicenseClassID);
                }
                else
                {
                    EventLogger.LogWarning("UpdateLicenseClass", $"License Class ID {LicenseClassID} not found for update");
                }
            }
            catch (SqlException ex)
            {
                EventLogger.LogSqlError("UpdateLicenseClass", LicenseClassID, ex);
                return false;
            }
            catch (Exception ex)
            {
                EventLogger.LogError("UpdateLicenseClass", $"Error updating license class ID {LicenseClassID}", ex);
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