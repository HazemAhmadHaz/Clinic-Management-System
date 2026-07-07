using System;
using System.Data;
using Clinic_DataAccess;

namespace Clinic_Buisness
{
    public class clsApplicationSubType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Fees { get; set; }
        public int ApplicationSubTypeID { get; set; }
        public clsApplicationSubType()
        {
            this.ID = -1;
            this.Name = "";
            this.Fees = 0;
            this.ApplicationSubTypeID = -1;
            Mode = enMode.AddNew;
        }

        public clsApplicationSubType(int ID, string Name, decimal Fees, int ApplicationSubTypeID)
        {
            this.ID = ID;
            this.Name = Name;
            this.Fees = Fees;
            this.ApplicationSubTypeID = ApplicationSubTypeID;
            Mode = enMode.Update;
        }

        private bool _AddNew()
        {
            this.ID = clsApplicationSubTypeData.AddNew(this.Name, this.Fees, this.ApplicationSubTypeID);
            return (this.ID != -1);
        }

        private bool _Update()
        {
            return clsApplicationSubTypeData.Update(this.ID, this.Name, this.Fees, this.ApplicationSubTypeID);
        }

        public static clsApplicationSubType Find(int ID)
        {
            string Name = "";
            decimal Fees = 0;
            int ApplicationSubTypeID = -1;

            if (clsApplicationSubTypeData.GetByID(ID, ref Name, ref Fees, ref ApplicationSubTypeID))
                return new clsApplicationSubType(ID, Name, Fees, ApplicationSubTypeID);
            else
                return null;
        }

        public static DataTable GetAll()
        {
            return clsApplicationSubTypeData.GetAll();
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
            return clsApplicationSubTypeData.Delete(ID);
        }
        public static clsApplicationSubType FindByTitle(string ApplicationSubTypeTitle)
        {
            int ApplicationSubTypeID = -1;
            int ID = -1;
            decimal ApplicationFees = 0;

            // Call the Data Access Layer to fill the values by reference
            if (clsApplicationSubTypeData.GetApplicationSubTypeInfoByTitle(ApplicationSubTypeTitle, ref ID, ref ApplicationFees, ref ApplicationSubTypeID))
            {
                return new clsApplicationSubType(ID, ApplicationSubTypeTitle, ApplicationFees, ApplicationSubTypeID);
            }
            else
            {
                return null;
            }
     
        }
        public static DataTable GetApplicationSubTypesByApplicationTypeID(int applicationTypeID)
        {
            return clsApplicationSubTypeData.GetApplicationSubTypesByApplicationTypeID(applicationTypeID);
        }
    }
}