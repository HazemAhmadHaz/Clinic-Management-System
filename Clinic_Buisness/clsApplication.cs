using System;
using System.Data;
using Clinic_DataAccess;

namespace Clinic_Buisness
{
    public class clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 }

        public enum enApplicationStatus
        {
            New = 1,
            Cancelled = 2,
            Completed = 3
        }

        public enMode Mode = enMode.AddNew;

        public int ApplicationID { get; set; }
        public int PersonID { get; set; }
        public int ApplicationTypeID { get; set; }
        public int ApplicationSubTypeID { get; set; }
        public int DoctorID { get; set; }
        public int CreatedByUserID { get; set; }

        public DateTime ApplicationDate { get; set; }
        public DateTime? AppointmentDate { get; set; }

        public string ResourceName { get; set; }

        public enApplicationStatus ApplicationStatus { get; set; }

        public decimal Fees { get; set; }
        public bool IsLocked { get; set; }
        public decimal PaidFees { get; set; }

        public string Outcome { get; set; }

        public clsUser CreatedByUserInfo
        {
            get
            {
                return clsUser.FindByUserID(CreatedByUserID);
            }
        }

        public string ApplicantFullName
        {
            get
            {
                clsPerson Person = clsPerson.Find(PersonID);
                return Person != null ? Person.FullName : "";
            }
        }

        public clsApplication()
        {
            ApplicationID = -1;
            PersonID = -1;
            ApplicationTypeID = -1;
            ApplicationSubTypeID = -1;
            DoctorID = -1;
            CreatedByUserID = -1;

            ApplicationDate = DateTime.Now;
            AppointmentDate = null;
            ResourceName = "";

            ApplicationStatus = enApplicationStatus.New;

            Fees = 0;
            IsLocked = false;
            PaidFees = 0;
            Outcome = "";

            Mode = enMode.AddNew;
        }

        private clsApplication(
            int ApplicationID,
            int PersonID,
            int ApplicationTypeID,
            int ApplicationSubTypeID,
            int DoctorID,
            int CreatedByUserID,
            DateTime ApplicationDate,
            DateTime? AppointmentDate,
            string ResourceName,
            enApplicationStatus ApplicationStatus,
            decimal Fees,
            bool IsLocked,
            decimal PaidFees,
            string Outcome)
        {
            this.ApplicationID = ApplicationID;
            this.PersonID = PersonID;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationSubTypeID = ApplicationSubTypeID;
            this.DoctorID = DoctorID;
            this.CreatedByUserID = CreatedByUserID;
            this.ApplicationDate = ApplicationDate;
            this.AppointmentDate = AppointmentDate;
            this.ResourceName = ResourceName;
            this.ApplicationStatus = ApplicationStatus;
            this.Fees = Fees;
            this.IsLocked = IsLocked;
            this.PaidFees = PaidFees;
            this.Outcome = Outcome;

            Mode = enMode.Update;
        }

        private bool _AddNewApplication()
        {
            ApplicationID = clsApplicationData.AddNewApplication(
                PersonID,
                ApplicationTypeID,
                ApplicationSubTypeID,
                DoctorID,
                CreatedByUserID,
                ApplicationDate,
                AppointmentDate,
                ResourceName,
                (byte)ApplicationStatus,
                Fees,
                IsLocked,
                PaidFees,
                Outcome);

            return (ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(
                ApplicationID,
                PersonID,
                ApplicationTypeID,
                ApplicationSubTypeID,
                DoctorID,
                CreatedByUserID,
                ApplicationDate,
                AppointmentDate,
                ResourceName,
                (byte)ApplicationStatus,
                Fees,
                IsLocked,
                PaidFees,
                Outcome);
        }

        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            int PersonID = -1;
            int ApplicationTypeID = -1;
            int ApplicationSubTypeID = -1;
            int DoctorID = -1;
            int CreatedByUserID = -1;

            DateTime ApplicationDate = DateTime.Now;
            DateTime? AppointmentDate = null;
            string ResourceName = "";

            byte ApplicationStatus = 1;
            decimal Fees = 0;
            bool IsLocked = false;
            decimal PaidFees = 0;
            string Outcome = "";

            if (clsApplicationData.GetApplicationInfoByID(
                ApplicationID,
                ref PersonID,
                ref ApplicationTypeID,
                ref ApplicationSubTypeID,
                ref DoctorID,
                ref CreatedByUserID,
                ref ApplicationDate,
                ref AppointmentDate,
                ref ResourceName,
                ref ApplicationStatus,
                ref Fees,
                ref IsLocked,
                ref PaidFees,
                ref Outcome))
            {
                return new clsApplication(
                    ApplicationID,
                    PersonID,
                    ApplicationTypeID,
                    ApplicationSubTypeID,
                    DoctorID,
                    CreatedByUserID,
                    ApplicationDate,
                    AppointmentDate,
                    ResourceName,
                    (enApplicationStatus)ApplicationStatus,
                    Fees,
                    IsLocked,
                    PaidFees,
                    Outcome);
            }

            return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }

                    return false;

                case enMode.Update:
                    return _UpdateApplication();
            }

            return false;
        }

        public static DataTable GetAllApplications()
        {
            return clsApplicationData.GetAllApplications();
        }

        public bool Delete()
        {
            return clsApplicationData.DeleteApplication(ApplicationID);
        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationID);
        }

        public static int GetActiveApplicationIDForApplicationTypeAndSubType(
            int PersonID,
            int ApplicationTypeID,
            int ApplicationSubTypeID)
        {
            return clsApplicationData.GetActiveApplicationIDForApplicationTypeAndSubType(
                PersonID,
                ApplicationTypeID,
                ApplicationSubTypeID);
        }
    }
}