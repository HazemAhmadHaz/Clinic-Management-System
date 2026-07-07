using System;
using System.Data;
using Clinic_DataAccess;

namespace Clinic_Buisness
{
    public class clsDoctor
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int DoctorID { set; get; }
        public int PersonID { set; get; }

        public clsPerson PersonInfo { set; get; } // Composition link to access linked Person properties
        public string Specialisation { set; get; }
        public bool IsAvailable { set; get; }

        // Default Constructor (Mode: Add New)
        public clsDoctor()
        {
            this.DoctorID = -1;
            this.PersonID = -1;
            this.Specialisation = "";
            this.IsAvailable = true;

            Mode = enMode.AddNew;
        }

        // Private Constructor (Mode: Update)
        private clsDoctor(int DoctorID, int PersonID, string Specialisation, bool IsAvailable)
        {
            this.DoctorID = DoctorID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID); // Automatically load underlying person data
            this.Specialisation = Specialisation;
            this.IsAvailable = IsAvailable;

            Mode = enMode.Update;
        }

        private bool _AddNewDoctor()
        {
            // Call DataAccess Layer to execute SP_AddNewDoctor
            this.DoctorID = clsDoctorData.AddNewDoctor(this.PersonID, this.Specialisation, this.IsAvailable);
            return (this.DoctorID != -1);
        }

        private bool _UpdateDoctor()
        {
            // Call DataAccess Layer to execute SP_UpdateDoctor
            return clsDoctorData.UpdateDoctor(this.DoctorID, this.PersonID, this.Specialisation, this.IsAvailable);
        }

        public static clsDoctor FindByDoctorID(int DoctorID)
        {
            int PersonID = -1;
            string Specialisation = "";
            bool IsAvailable = false;

            bool isFound = clsDoctorData.GetDoctorInfoByDoctorID(DoctorID, ref PersonID, ref Specialisation, ref IsAvailable);

            if (isFound)
                return new clsDoctor(DoctorID, PersonID, Specialisation, IsAvailable);
            else
                return null;
        }

        public static clsDoctor FindByPersonID(int PersonID)
        {
            int DoctorID = -1;
            string Specialisation = "";
            bool IsAvailable = false;

            bool isFound = clsDoctorData.GetDoctorInfoByPersonID(PersonID, ref DoctorID, ref Specialisation, ref IsAvailable);

            if (isFound)
                return new clsDoctor(DoctorID, PersonID, Specialisation, IsAvailable);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDoctor())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateDoctor();
            }

            return false;
        }

        public static DataTable GetAllDoctors()
        {
            return clsDoctorData.GetAllDoctors();
        }

        public static bool DeleteDoctor(int DoctorID)
        {
            return clsDoctorData.DeleteDoctor(DoctorID);
        }

        public static bool IsDoctorExist(int DoctorID)
        {
            return clsDoctorData.IsDoctorExist(DoctorID);
        }

        public static bool IsDoctorExist(string Specialisation)
        {
            return clsDoctorData.IsDoctorExist(Specialisation);
        }

        public static bool IsDoctorExistForPersonID(int PersonID)
        {
            return clsDoctorData.IsDoctorExistForPersonID(PersonID);
        }
    }
}