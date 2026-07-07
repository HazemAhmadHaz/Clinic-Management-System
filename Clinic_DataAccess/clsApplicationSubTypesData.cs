using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsApplicationSubTypeData
    {
        public static bool GetByID(int ApplicationSubTypeID, ref string Name, ref decimal Fees, ref int ApplicationTypeID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetApplicationSubTypeByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationSubTypeID", ApplicationSubTypeID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Name = reader["Name"].ToString();
                        Fees = Convert.ToDecimal(reader["Fees"]);
                        ApplicationTypeID = Convert.ToInt32(reader["ApplicationTypeID"]);
                        return true;
                    }
                }
            }

            return false;
        }
        
        public static bool GetAppSubTypeByID(int ApplicationSubTypeID, ref string Name, ref decimal Fees)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetApplicationSubTypeByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationSubTypeID", ApplicationSubTypeID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Name = reader["Name"].ToString();
                        Fees = Convert.ToDecimal(reader["Fees"]);
                        return true;
                    }
                }
            }

            return false;
        }

        public static DataTable GetAll()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetAllApplicationSubTypes", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            return dt;
        }

        public static int AddNew(string Name, decimal Fees, int ApplicationSubTypeID)
        {
            int newID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_AddNewApplicationSubType", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Fees", Fees);
                command.Parameters.AddWithValue("@ApplicationSubTypeID", ApplicationSubTypeID);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    newID = id;
                }
            }

            return newID;
        }

        public static bool Update(int ApplicationSubTypeID, string Name, decimal Fees, int ApplicationTypeID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_UpdateApplicationSubType", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ApplicationSubTypeID", ApplicationSubTypeID);
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Fees", Fees);
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public static bool Delete(int ApplicationSubTypeID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_DeleteApplicationSubType", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ApplicationSubTypeID", ApplicationSubTypeID);

                connection.Open();

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public static bool GetApplicationSubTypeInfoByTitle(string Name, ref int ID, ref decimal ApplicationFees, ref int ApplicationSubTypeID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("SP_GetApplicationSubTypeByTitle", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", Name);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;

                            // Match these exact column string keys to your SELECT statement keys!
                            ID = (int)reader["ApplicationSubTypeID"];
                            ApplicationFees = Convert.ToDecimal(reader["Fees"]);
                            ApplicationSubTypeID = (int)reader["ApplicationSubTypeID"];
                        }
                    }
                }
            }

            return isFound;
        }
        public static DataTable GetApplicationSubTypesByApplicationTypeID(int applicationTypeID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command =
                new SqlCommand("SP_GetApplicationSubTypes", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())

                {
                    if (reader.HasRows)
                        dt.Load(reader);
                }
            }

            return dt;
        }
    }
} 
