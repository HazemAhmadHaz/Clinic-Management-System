using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
    {
        public class clsPersonData
        {
            // =============================================
            // Get Person By ID
            // =============================================
            public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
                ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
                ref short Gendor, ref string Address, ref string Phone, ref string Email,
                ref int NationalityCountryID, ref string ImagePath)
            {
                bool isFound = false;

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("sp_GetPersonByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            FirstName = (string)reader["FirstName"];
                            SecondName = (string)reader["SecondName"];
                            ThirdName = reader["ThirdName"] == DBNull.Value ? "" : (string)reader["ThirdName"];
                            LastName = (string)reader["LastName"];
                            NationalNo = (string)reader["NationalNo"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            Gendor = (byte)reader["Gendor"];
                            Address = (string)reader["Address"];
                            Phone = (string)reader["Phone"];
                            Email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"];
                            NationalityCountryID = (int)reader["NationalityCountryID"];
                            ImagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"];
                        }
                    }
                }

                return isFound;
            }

            // =============================================
            // Get Person By National No
            // =============================================
            public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
                ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
                ref short Gendor, ref string Address, ref string Phone, ref string Email,
                ref int NationalityCountryID, ref string ImagePath)
            {
                bool isFound = false;

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("sp_GetPersonByNationalNo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            PersonID = (int)reader["PersonID"];
                            FirstName = (string)reader["FirstName"];
                            SecondName = (string)reader["SecondName"];
                            ThirdName = reader["ThirdName"] == DBNull.Value ? "" : (string)reader["ThirdName"];
                            LastName = (string)reader["LastName"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            Gendor = (byte)reader["Gendor"];
                            Address = (string)reader["Address"];
                            Phone = (string)reader["Phone"];
                            Email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"];
                            NationalityCountryID = (int)reader["NationalityCountryID"];
                            ImagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"];
                        }
                    }
                }

                return isFound;
            }

            // =============================================
            // Add New Person
            // =============================================
            public static int AddNewPerson(string FirstName, string SecondName,
                string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
                short Gendor, string Address, string Phone, string Email,
                int NationalityCountryID, string ImagePath)
            {
                int PersonID = -1;

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("sp_AddNewPerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@ThirdName", (string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName));
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Gendor);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", (string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email));
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    command.Parameters.AddWithValue("@ImagePath", (string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath));

                    SqlParameter outputID = new SqlParameter("@NewPersonID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputID);

                    connection.Open();
                    command.ExecuteNonQuery();
                    PersonID = (int)outputID.Value;
                }

                return PersonID;
            }

            // =============================================
            // Update Person
            // =============================================
            public static bool UpdatePerson(int PersonID, string FirstName, string SecondName,
                string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
                short Gendor, string Address, string Phone, string Email,
                int NationalityCountryID, string ImagePath)
            {
                int rowsAffected = 0;

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("sp_UpdatePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@SecondName", SecondName);
                    command.Parameters.AddWithValue("@ThirdName", (string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName));
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Gendor);
                    command.Parameters.AddWithValue("@Address", Address);
                    command.Parameters.AddWithValue("@Phone", Phone);
                    command.Parameters.AddWithValue("@Email", (string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email));
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    command.Parameters.AddWithValue("@ImagePath", (string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath));

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }

                return rowsAffected > 0;
            }

            // =============================================
            // Get All People
            // =============================================
            public static DataTable GetAllPeople()
            {
                DataTable dt = new DataTable();

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("sp_GetAllPeople", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            dt.Load(reader);
                    }
                }

                return dt;
            }

            // =============================================
            // Delete Person
            // =============================================
            public static bool DeletePerson(int PersonID)
            {
                int rowsAffected = 0;

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("sp_DeletePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }

                return rowsAffected > 0;
            }

            // =============================================
            // Is Person Exist By ID
            // =============================================
            public static bool IsPersonExist(int PersonID)
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("sp_IsPersonExistByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }

            // =============================================
            // Is Person Exist By National No
            // =============================================
            public static bool IsPersonExist(string NationalNo)
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand("sp_IsPersonExistByNationalNo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    connection.Open();
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }
    }