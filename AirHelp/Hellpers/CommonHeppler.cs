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
    public static class CommonHeppler
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

        public static string GetAirport(string text)
        {
            var result = "";
            var url = "https://openflights.org/php/apsearch.php";
            var values = new Dictionary<string, string>
                {
                      {"name" , text},
                      {"country", "ALL"},
                      {"action", "SEARCH"},
                      {"offset", "0"}
                };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;

                result = responseContent.ReadAsStringAsync().Result;

            }

            return result;
        }

        public static string GetAirlines(string text)
        {

            string result = "";
            var url = "https://openflights.org/php/alsearch.php";
            var values = new Dictionary<string, string>
                {
                      {"name" , text},
                      {"country", "ALL"},
                      {"action", "SEARCH"},
                      {"mode", "F" },
                      {"iatafilter", "true" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content;

                result = responseContent.ReadAsStringAsync().Result;

            }

            try
            {
                result = result.Substring(result.IndexOf('{')).Replace("\n", ",");
                result = "{\"status\": 1, \"airports\": [" + result + "]}";
            }
            catch (Exception ex)
            {
                result = "{\"status\": 0, \"airports\": []}";
            }
            return result;
        }
        public static bool IsEuCountry(string countryCode)
        {
            countryCode = countryCode.ToUpper();
            return CountryCodeArr.ToList().Any(c => c == countryCode);
        }


        public static bool IsEuCountryByName(string CountryName)
        {
            CountryName = CountryName.ToLower();
            return CountryNameArr.ToList().Any(c => c.ToLower() == CountryName);
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

        private static string[] CountryNameArr = {
            "Austria",
            "Belgium",
            "Bulgaria",
            "Croatia",
            "Cyprus ",
            "Czech Republic",
            "Denmark",
            "Estonia",
            "Finland",
            "France",
            "Germany",
            "Greece",
            "Hungary",
            "Ireland",
            "Italy",
            "Latvia",
            "Lithuania",
            "Luxembourg",
            "Malta",
            "Netherland",
            "Poland",
            "Portugal",
            "Romania",
            "Slovakia",
            "Slovenia ",
            "Spain",
            "Sweden",
            "United Kingdom"
        };

        public static string ViewString(this ProblemType type)
        {
            string result = "";
            switch (type)
            {
                case ProblemType.Pending:
                    result = "Грешка";
                    break;
                case ProblemType.Delay:
                    result = "Закъснение при полет";
                    break;
                case ProblemType.Cancel:
                    result = "Отменен полет";
                    break;
                default:
                    result = "Отказан достъп до борда";
                    break;
            }

            return result;
        }

        public static string ViewString(this ClaimStatus type)
        {
            string result = "";
            switch (type)
            {
                case ClaimStatus.WaitForDocument:
                    result = "Изчакване на документи";
                    break;
                case ClaimStatus.Accepted:
                    result = "Приета";
                    break;
                case ClaimStatus.InProgress:
                    result = "В прогрес";
                    break;
                case ClaimStatus.Compleeted:
                    result = "Приключена успешно";
                    break;
                case ClaimStatus.Rejected:
                    result = "Отхвърлена";
                    break;
                default:
                    result = "";
                    break;
            }

            return result;
        }


        public static string ViewString(this Reason type)
        {
            string result = "";
            switch (type)
            {
                case Reason.AirportFault:
                    result = "По вина на летището";
                    break;
                case Reason.BadWeather:
                    result = "Лошо време";
                    break;
                case Reason.InfuenceOtherFlight:
                    result = "Повлиян от други полети";
                    break;
                case Reason.Strike:
                    result = "Стачка";
                    break;
                case Reason.TechnicalIssue:
                    result = "Технически проблем";
                    break;
                case Reason.WithoutReason:
                    result = "Не беше дадена причина";
                    break;
            }

            return result;
        }

        public static string ViewString(this CancelOverbokingDelay type)
        {
            string result = "";
            switch (type)
            {
                case CancelOverbokingDelay.Beetwen0_2:
                    result = "Под 2 часа";
                    break;
                case CancelOverbokingDelay.Beetwen2_3:
                    result = "Между 2 и 3 часа";
                    break;
                case CancelOverbokingDelay.Beetwen3_4:
                    result = "Между 3 и 4 часа";
                    break;
                case CancelOverbokingDelay.MoreThan4:
                    result = "Повече от 4 часа";
                    break;
                case CancelOverbokingDelay.NotArrive:
                    result = "Не пристигнах";
                    break;

            }

            return result;
        }

        public static string ViewString(this CancelAnnonsment type)
        {
            string result = "";
            switch (type)
            {
                case CancelAnnonsment.MoreThan14:
                    result = "Повече от 14 дени";
                    break;
                case CancelAnnonsment.Beetwen7_14:
                    result = "Между 7 и 14 дни ";
                    break;
                case CancelAnnonsment.LessThat7:
                    result = "По-малко от 7 дни";
                    break;
            }

            return result;
        }

        public static string ViewString(this DenayArival type)
        {
            string result = "";
            switch (type)
            {
                case DenayArival.Before30:
                    result = "Повече 30 мин преди началото на полета";
                    break;
                case DenayArival.After30:
                    result = "по-малко 30 мин преди началото на полета";
                    break;
            }

            return result;
        }

        public static string ViewString(this DocumentSecurity type)
        {
            string result = "";
            switch (type)
            {
                case DocumentSecurity.MyFault:
                    result = "Да";
                    break;
                case DocumentSecurity.NotMyFault:
                    result = "Не";
                    break;
            }

            return result;
        }

        public static string ViewString(this Willness type)
        {
            string result = "";
            switch (type)
            {
                case Willness.Voluntary:
                    result = "Да";
                    break;
                case Willness.NotVoluntary:
                    result = "Не";
                    break;
            }

            return result;
        }

        public static string ViewString(this DelayDelay type, double distance)
        {
            string result = "";
            if (distance <= 1500000)
            {
                if (type == DelayDelay.Beetwen0_2)
                {
                    result = "по-малко от 2 часа";
                }
                else if (type == DelayDelay.Beetwen2_3)
                {
                    result = "между 2 и 3 часа";
                }
                else if (type > DelayDelay.Beetwen2_3)
                {
                    result = "повече от 3 часа";
                }
            }
            else if (1500000 < distance && distance <= 3500000)
            {
                if (type < DelayDelay.Beetwen3_4)
                {
                    result = "по-малко от 3 часа";
                }
                else
                {
                    result = "повече от 3 часа";
                }

            }
            else if (3500000 < distance)
            {
                if (type < DelayDelay.Beetwen3_4)
                {
                    result = "по-малко от 3 часа";
                }
                else if (type == DelayDelay.Beetwen3_4)
                {
                    result = "между 3 и 4 часа";
                }
                else if (type == DelayDelay.MoreThan4)
                {
                    result = "повече от 4 часа";
                }
            }
            return result;
        }

    }

}
