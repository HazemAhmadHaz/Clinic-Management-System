using System;
using System.Data;
using System.Data.SqlClient;

namespace Clinic_DataAccess
{
    public class clsApplicationData
    {
        public static bool GetApplicationInfoByID(
            int ApplicationID,
            ref int PersonID,
            ref int ApplicationTypeID,
            ref int ApplicationSubTypeID,
            ref int DoctorID,
            ref int CreatedByUserID,
            ref DateTime ApplicationDate,
            ref DateTime? AppointmentDate,
            ref string ResourceName,
            ref byte ApplicationStatus,
            ref decimal Fees,
            ref bool IsLocked,
            ref decimal PaidFees,
            ref string Outcome)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetApplicationByID", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        isFound = true;

                        PersonID = (int)reader["PersonID"];
                        ApplicationTypeID = (int)reader["ApplicationTypeID"];
                        ApplicationSubTypeID = (int)reader["ApplicationSubTypeID"];
                        DoctorID = (int)reader["DoctorID"];
                        CreatedByUserID = (int)reader["CreatedByUserID"];
                        ApplicationDate = (DateTime)reader["ApplicationDate"];

                        AppointmentDate = reader["AppointmentDate"] == DBNull.Value
                            ? (DateTime?)null
                            : Convert.ToDateTime(reader["AppointmentDate"]);

                        ResourceName = reader["ResourceName"].ToString();
                        ApplicationStatus = Convert.ToByte(reader["ApplicationStatus"]);
                        Fees = Convert.ToDecimal(reader["Fees"]);
                        IsLocked = Convert.ToBoolean(reader["IsLocked"]);
                        PaidFees = Convert.ToDecimal(reader["PaidFees"]);
                        Outcome = reader["Outcome"] == DBNull.Value
                            ? ""
                            : reader["Outcome"].ToString();
                    }
                }
            }

            return isFound;
        }

        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_GetAllApplications", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if(reader.HasRows)
                    dt.Load(reader);
                }
            }

            return dt;
        }

        public static int AddNewApplication(
            int PersonID,
            int ApplicationTypeID,
            int ApplicationSubTypeID,
            int DoctorID,
            int CreatedByUserID,
            DateTime ApplicationDate,
            DateTime? AppointmentDate,
            string ResourceName,
            byte ApplicationStatus,
            decimal Fees,
            bool IsLocked,
            decimal PaidFees,
            string Outcome)
        {
            int ApplicationID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_AddNewApplication", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@ApplicationSubTypeID", ApplicationSubTypeID);
                command.Parameters.AddWithValue("@DoctorID", DoctorID);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                command.Parameters.AddWithValue("@AppointmentDate", (object)AppointmentDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@ResourceName", ResourceName);
                command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                command.Parameters.AddWithValue("@Fees", Fees);
                command.Parameters.AddWithValue("@IsLocked", IsLocked);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@Outcome", (object)Outcome ?? DBNull.Value);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int ID))
                    ApplicationID = ID;
            }

            return ApplicationID;
        }

        public static bool UpdateApplication(
            int ApplicationID,
            int PersonID,
            int ApplicationTypeID,
            int ApplicationSubTypeID,
            int DoctorID,
            int CreatedByUserID,
            DateTime ApplicationDate,
            DateTime? AppointmentDate,
            string ResourceName,
            byte ApplicationStatus,
            decimal Fees,
            bool IsLocked,
            decimal PaidFees,
            string Outcome)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_UpdateApplication", connection))
            {

                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                command.Parameters.AddWithValue("@ApplicationSubTypeID", ApplicationSubTypeID);
                command.Parameters.AddWithValue("@DoctorID", DoctorID);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                command.Parameters.AddWithValue("@AppointmentDate", (object)AppointmentDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@ResourceName", ResourceName);
                command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                command.Parameters.AddWithValue("@Fees", Fees);
                command.Parameters.AddWithValue("@IsLocked", IsLocked);
                command.Parameters.AddWithValue("@PaidFees", PaidFees);
                command.Parameters.AddWithValue("@Outcome", (object)Outcome ?? DBNull.Value);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected > 0;
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_DeleteApplication", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand("SP_IsApplicationExist", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int value))
                    isFound = (value == 1);
            }

            return isFound;
        }

        public static int GetActiveApplicationIDForApplicationTypeAndSubType(
            int PersonID,
            int ApplicationSubTypeID,
            int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;

            using (SqlConnection connection =
                new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command =
                new SqlCommand("SP_GetActiveApplicationIDForApplicationTypeAndSubType", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@PersonID", PersonID);
                command.Parameters.AddWithValue("@ApplicationSubTypeID", ApplicationSubTypeID);
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int AppID))
                    ActiveApplicationID = AppID;
            }

            return ActiveApplicationID;
        }
    }
}