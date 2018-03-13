using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHelp.DAL;
using AirHelp.Models;

namespace AirHelp.Hellpers
{
    public class CommonHeppler
    {

       
        
        public static bool IsEuCountry(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            return CountryCodeArr.ToList().Any(c => c == countryCode);
        }

        private static string[] CountryCodeArr = {
            "BE",
            "BG",
            "CZ",
            "DK",
            "DE",
            "EE",
            "IE",
            "EL",
            "ES",
            "FR",
            "HR",
            "IT",
            "CY",
            "LV",
            "LT",
            "LU",
            "HU",
            "MT",
            "NL",
            "AT",
            "PL",
            "PT",
            "RO",
            "SI",
            "SK",
            "FI",
            "SE",
            "UK",
        };

    }
}