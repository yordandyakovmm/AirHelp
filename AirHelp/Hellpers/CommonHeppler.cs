using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHelp.DAL;
using AirHelp.Models;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Net.Http;

namespace AirHelp.Hellpers
{
    public class CommonHeppler
    {
        private static readonly HttpClient client = new HttpClient();

        public static FlightStatus GetFlight(string number, string date)
        {
            number = number.Trim().Replace(" ", "").Replace("-", "");
            string airLineCode = number.Substring(0, 2).ToUpper();
            string flightNumber = number.Substring(2);
            string year = date.Split('.')[2];
            string month = date.Split('.')[1];
            string day = date.Split('.')[0];

            string appID = ConfigurationManager.AppSettings["appId"];
            string appKey = ConfigurationManager.AppSettings["appKey"];

            string json = "";
            var url = $"https://api.flightstats.com/flex/flightstatus/historical/rest/v3/json/flight/status/{airLineCode}/{flightNumber}/dep/{year}/{month}/{day}?appId={appID}&appKey={appKey}";
            
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;

                json = responseContent.ReadAsStringAsync().Result;

            }
                   
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            FlightStatus flight = JsonConvert.DeserializeObject<FlightStatus>(json);

            return flight;
        }

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