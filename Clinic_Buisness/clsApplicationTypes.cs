using System;
using System.Data;
using Clinic_DataAccess;

namespace Clinic_Buisness
{
    public class clsApplicationType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Fees { get; set; }

        public clsApplicationType()
        {
            this.ID = -1;
            this.Name = "";
            this.Fees = 0;

            Mode = enMode.AddNew;
        }

        public clsApplicationType(int ID, string Name, decimal Fees)
        {
            this.ID = ID;
            this.Name = Name;
            this.Fees = Fees;

            Mode = enMode.Update;
        }

        private bool _AddNew()
        {
            this.ID = clsApplicationTypeData.AddNew(this.Name, this.Fees);
            return (this.ID != -1);
        }

        private bool _Update()
        {
            return clsApplicationTypeData.Update(this.ID, this.Name, this.Fees);
        }

        public static clsApplicationType Find(int ID)
        {
            string Name = "";
            decimal Fees = 0;

            if (clsApplicationTypeData.GetByID(ID, ref Name, ref Fees))
                return new clsApplicationType(ID, Name, Fees);
            else
                return null;
        }

        public static DataTable GetAll()
        {
            return clsApplicationTypeData.GetAll();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _Update();

                default:
                    return false;
            }
        }

        public static bool Delete(int ID)
        {
            return clsApplicationTypeData.Delete(ID);
        }

        public static clsApplicationType FindByTitle(string ApplicationTypeTitle)
        {
            int ID = -1;
            decimal ApplicationFees = 0;

            if (clsApplicationTypeData.GetApplicationTypeInfoByTitle(ApplicationTypeTitle, ref ID, ref ApplicationFees))
            {
                return new clsApplicationType(ID, ApplicationTypeTitle, ApplicationFees);
            }
            else
            {
                return null;
            }
        }
        public static clsApplicationType FindByID(int ID)
        {
            string ApplicationTypeTitle = "";
            decimal ApplicationFees = 0;

            if (clsApplicationTypeData.GetByID(ID, ref ApplicationTypeTitle, ref ApplicationFees))
            {
                return new clsApplicationType(ID, ApplicationTypeTitle, ApplicationFees);
            }
            else
            {
                return null;
            }
        }
    }
}