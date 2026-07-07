using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_DataAccess
{
    public class clsDoctorData
    {
        public static bool GetDoctorInfoByDoctorID(int DoctorID, ref int PersonID, ref string Specialisation, ref bool IsAvailable)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetDoctorInfoByDoctorID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DoctorID", DoctorID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            PersonID = (int)reader["PersonID"];
                            Specialisation = (string)reader["Specialisation"];
                            IsAvailable = (bool)reader["IsAvailable"];
                        }
                    }
                }
                catch (Exception)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetDoctorInfoByPersonID(int PersonID, ref int DoctorID, ref string Specialisation, ref bool IsAvailable)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetDoctorInfoByPersonID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            DoctorID = (int)reader["DoctorID"];
                            Specialisation = (string)reader["Specialisation"];
                            IsAvailable = (bool)reader["IsAvailable"];
                        }
                    }
                }
                catch (Exception)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static int AddNewDoctor(int PersonID, string Specialisation, bool IsAvailable)
        {
            int DoctorID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_AddNewDoctor", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@Specialisation", Specialisation);
                command.Parameters.AddWithValue("@IsAvailable", IsAvailable);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        DoctorID = insertedID;
                    }
                }
                catch (Exception)
                {
                    // Managed Exception
                }
            }

            return DoctorID;
        }

        public static bool UpdateDoctor(int DoctorID, int PersonID, string Specialisation, bool IsAvailable)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_UpdateDoctor", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DoctorID", DoctorID);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@Specialisation", Specialisation);
                command.Parameters.AddWithValue("@IsAvailable", IsAvailable);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int affected))
                    {
                        rowsAffected = affected;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllDoctors()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetAllDoctors", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception)
                {
                    // Managed Exception
                }
            }

            return dt;
        }

        public static bool DeleteDoctor(int DoctorID)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_DeleteDoctor", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DoctorID", DoctorID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int affected))
                    {
                        rowsAffected = affected;
                    }
                }
                catch (Exception)
                {
                    // Managed Exception
                }
            }

            return (rowsAffected > 0);
        }

        public static bool IsDoctorExist(int DoctorID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_IsDoctorExistByDoctorID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DoctorID", DoctorID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int check))
                    {
                        isFound = (check == 1);
                    }
                }
                catch (Exception)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool IsDoctorExist(string Specialisation)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_IsDoctorExistBySpecialisation", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Specialisation", Specialisation);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int check))
                    {
                        isFound = (check == 1);
                    }
                }
                catch (Exception)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool IsDoctorExistForPersonID(int PersonID)
        {
            return DoesPersonHaveDoctor44(PersonID);
        }

        public static bool DoesPersonHaveDoctor44(int PersonID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_IsDoctorExistByPersonID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PersonID", PersonID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int check))
                    {
                        isFound = (check == 1);
                    }
                }
                catch (Exception)
                {
                    isFound = false;
                }
            }

            return isFound;
        }
    }
}