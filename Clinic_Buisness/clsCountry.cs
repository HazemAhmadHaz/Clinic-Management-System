using System;
using System.Data;
using Clinic_DataAccess;

namespace Clinic_Buisness
{
    public class clsCountry
    { 

            // =============================================
            // Properties
            // =============================================
            public int CountryID { get; private set; }
            public string CountryName { get; set; }

            // =============================================
            // Constructor
            // =============================================
            public clsCountry()
            {
                CountryID = -1;
                CountryName = string.Empty;
            }

            private clsCountry(int countryID, string countryName)
            {
                CountryID = countryID;
                CountryName = countryName;
            }

            // =============================================
            // Find By ID
            // =============================================
            public static clsCountry Find(int countryID)
            {
                string countryName = string.Empty;

                bool isFound = clsCountryData.GetCountryInfoByID(countryID, ref countryName);

                if (isFound)
                    return new clsCountry(countryID, countryName);

                return null;
            }

            // =============================================
            // Find By Name
            // =============================================
            public static clsCountry Find(string countryName)
            {
                int countryID = -1;

                bool isFound = clsCountryData.GetCountryInfoByName(countryName, ref countryID);

                if (isFound)
                    return new clsCountry(countryID, countryName);

                return null;
            }

            // =============================================
            // Get All Countries
            // =============================================
            public static DataTable GetAllCountries()
            {
                return clsCountryData.GetAllCountries();
            }
        }
    }
