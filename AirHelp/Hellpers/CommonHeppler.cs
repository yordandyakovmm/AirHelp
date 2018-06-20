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

        public static object GetAirport(string text)
        {
            var result = "";

            if (ConfigurationManager.AppSettings["useLocalData"].ToString() == "true")
            {

                using (AirHelpDBContext dc = new AirHelpDBContext())
                {
                    var airport = dc.AirPortsData.Where(a => a.name.StartsWith(text) || a.city.StartsWith(text) || a.country.StartsWith(text) || a.iata.StartsWith(text)).Select(a => new Airportss {
                        apid = a.Id.ToString(),
                        iata = a .iata,
                        city = a.city,
                        country = a.country,
                        name = a.name,
                        ap_name = a.name,
                        x = a.x.ToString(),
                        y = a.y.ToString(),
                        elevation = a.elevation.ToString()
                    });
                    var resultData = new ResultAirportData() {
                        status = 0
                    };

                    resultData.airports = airport.ToArray();
                    resultData.status = airport.Count() > 0 ? 1 : 0;


                    return resultData;

                }

            }
            else
            {



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
                case ClaimStatus.WaitForAttorny:
                    result = "Изчакване на пълномощно";
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

        public static string Metatags(string path)
        {
            var result = "";

            result = @"<title>Закъснение при полет - Обезщетение | Отменен полет</title>
                           <meta name='robots' content='index, follow' />
                           <meta name='description' content='Правата ми при закъснял, отеменен полет или отказан достъп до борда. Правата ми при полет съгласно регламент 261/2004.' />
                           <meta name='keywords' content='закъснение, отмяна, отмяна полет, отменен полет,закъснял полет, закъснение полет, обезщетение при полет,анулиране на полет,отказан достъп, 
                            претенция към авиокомпания,правата ми при полет, обезщетение за закъснял полет,обезщетение отмяна полет, отменен полет, овърбукинг' />
                           <meta property='og:type' content='website' />

                           <meta property='og:description' content = 'Правата ми при закъснял, отеменен полет или отказан достъп до борда. Правата ми при полет съгласно регламент 261/2004.' />

                           <meta property='og:title' content='Проблеми с полета | Обезщетение' />      
                           <meta property='og:url' content='https://helpclaim.eu/' />
                           <meta property='article:published_time' content='2018-05-04' />
                           <meta property='article:modified_time' content='2018-06-14' />
                           <meta property='og:site_name' content = 'Проблеми с полета | Обезщетение | HELPCLAIMS' />
                           
                           <meta property='og:image' content = 'https://helpclaim.eu/Content/Assets/Logo/link.png' />
                           <meta property='article:tag' content = 'отменен' />
                           <meta property='article:tag' content = 'полет' />
                           <meta property='article:tag' content = 'закъснение' />
                           <meta property='article:tag' content = 'закъснение полет' />
                           <meta property='article:tag' content = 'отменен полет' />
                           <meta property='article:tag' content = 'обезщетение' />
                           <meta property='article:tag' content = 'закъснял полет' />
                           <meta property='article:tag' content = 'пропусната връзка' />
                           <meta property='article:tag' content = 'правата ми' />
                           <meta property='article:tag' content = 'обезщетение' />
                           <meta property='article:tag' content = 'закъснял полет' />
                           <meta property='article:tag' content = 'регламент 261' />
                           <meta property='article:tag' content = 'пропусната връзка полет' />";
                           //<meta property='article:publisher' content='https://www.facebook.com/' />";

                if (path.Contains("вход")) {
                result = @"<title>обезщетение при поле | Вход за потребители</title>
                           <meta name='robots' content='index, follow' />
                           <meta name='description' content='Правата ми при проблеми с полета. Вход за потребители.' />
                           <meta name='keywords' content='вход, потребители, отмяна полет, отменен полет,закъснял полет, закъснение полет, обезщетение при полет, потребителски профил ' />
                           <meta property='og:type' content='article' />
                           <meta property='og:description' content = 'Правата ми при закъснял, отеменен полет или отказан достъп до борда. Правата ми при полет съгласно регламент 261/2004.' />
                           <meta property='og:title' content='Проблеми с полета | Обезщетение' />      
                           <meta property='og:url' content='https://helpclaim.eu/вход' />
                           <meta property='article:published_time' content='2018-05-04' />
                           <meta property='article:modified_time' content='2018-06-14' />
                           <meta property='og:site_name' content = 'Проблеми с полета | Обезщетение | HELPCLAIMS' />
                           <meta property='og:image' content = 'https://helpclaim.eu/Content/Assets/Logo/link.png' />
                           <meta property='article:tag' content = 'отменен' />
                           <meta property='article:tag' content = 'полет' />
                           <meta property='article:tag' content = 'закъснение' />
                           <meta property='article:tag' content = 'вход' />";
                           //<meta property='article:publisher' content='https://www.facebook.com/' />";
                }
                else if (path.Contains("често-задавани-въпроси"))
                {
                result = @"<title>обезщетение при полет | често задавани въпроси</title>
                           <meta name='robots' content='index, follow' />
                           <meta name='description' content='Правата ми при проблеми с полета. Често задавани въпроси.' />
                           <meta name='keywords' content='вход, потребители, отмяна полет, отменен полет,закъснял полет, закъснение полет,faq ,обезщетение при полет, потребителски профил ' />
                           <meta property='og:type' content='article' />
                           <meta property='og:description' content = 'Правата ми при закъснял, отеменен полет или отказан достъп до борда. Правата ми при полет съгласно регламент 261/2004.' />
                           <meta property='og:title' content='Проблеми с полета | Обезщетение' />      
                           <meta property='og:url' content='https://helpclaim.eu/проблеми-с-полета/често-задавани-въпроси' />
                           <meta property='article:published_time' content='2018-05-04' />
                           <meta property='article:modified_time' content='2018-06-14' />
                           <meta property='og:site_name' content = 'Проблеми с полета | често задавани въпроси | HELPCLAIMS' />
                           <meta property='og:image' content = 'https://helpclaim.eu/Content/Assets/Logo/link.png' />
                           <meta property='article:tag' content = 'отменен' />
                           <meta property='article:tag' content = 'полет' />
                           <meta property='article:tag' content = 'закъснение' />
                           <meta property='article:tag' content = 'въпроси' />
                           <meta property='article:tag' content = 'въпроси обезщетение' />";
                           //<meta property='article:publisher' content='https://www.facebook.com/' />";
                }
                else if (path.Contains("за-нас"))
                {
                    result = @"<title>обезщетение при полет | често задавани въпроси</title>
                           <meta name='robots' content='index, follow' />
                           <meta name='description' content='Правата ми при проблеми с полета. Често задавани въпроси.' />
                           <meta name='keywords' content='вход, потребители, отмяна полет, отменен полет,закъснял полет, закъснение полет,faq ,обезщетение при полет, потребителски профил ' />
                           <meta property='og:type' content='article' />
                           <meta property='og:description' content = 'Правата ми при закъснял, отеменен полет или отказан достъп до борда. Правата ми при полет съгласно регламент 261/2004.' />
                           <meta property='og:title' content='Проблеми с полета | Обезщетение' />      
                           <meta property='og:url' content='https://helpclaim.eu/за-нас' />
                           <meta property='article:published_time' content='2018-05-04' />
                           <meta property='article:modified_time' content='2018-06-14' />
                           <meta property='og:site_name' content = 'Проблеми с полета | често задавани въпроси | HELPCLAIMS' />
                           <meta property='og:image' content = 'https://helpclaim.eu/Content/Assets/Logo/link.png' />
                           <meta property='article:tag' content = 'отменен' />
                           <meta property='article:tag' content = 'полет' />
                           <meta property='article:tag' content = 'закъснение' />
                           <meta property='article:tag' content = 'въпроси' />
                           <meta property='article:tag' content = 'въпроси обезщетение' />";
                           
                }
                else if (path.Contains("контакти"))
                {
                    result = @"<title>обезщетение при полет | често задавани въпроси</title>
                           <meta name='robots' content='index, follow' />
                           <meta name='description' content='Правата ми при проблеми с полета. Често задавани въпроси.' />
                           <meta name='keywords' content='вход, потребители, отмяна полет, отменен полет,закъснял полет, закъснение полет,faq ,обезщетение при полет, потребителски профил ' />
                           <meta property='og:type' content='article' />
                           <meta property='og:description' content = 'Контакти с нас. Правата ми при полет съгласно регламент 261/2004.' />
                           <meta property='og:title' content='Проблеми с полета | Обезщетение' />      
                           <meta property='og:url' content='https://helpclaim.eu/контакти' />
                           <meta property='article:published_time' content='2018-05-04' />
                           <meta property='article:modified_time' content='2018-06-14' />
                           <meta property='og:site_name' content = 'Проблеми с полета | контакти с ХЕЛПКЛЕЙМС' />
                           <meta property='og:image' content = 'https://helpclaim.eu/Content/Assets/Logo/link.png' />
                           <meta property='article:tag' content = 'отменен' />
                           <meta property='article:tag' content = 'полет' />
                           <meta property='article:tag' content = 'закъснение' />
                           <meta property='article:tag' content = 'въпроси' />
                           <meta property='article:tag' content = 'контакти с нас' />";
                           
            }
            else if (path.Contains("контакти"))
            {
                result = @"<title>обезщетение при полет | общи условия</title>
                           <meta name='robots' content='index, follow' />
                           <meta name='description' content='Правата ми при проблеми с полета. Често задавани въпроси.' />
                           <meta name='keywords' content='вход, потребители, отмяна полет, отменен полет,закъснял полет, закъснение полет,faq ,обезщетение при полет, потребителски профил ' />
                           <meta property='og:type' content='article' />
                           <meta property='og:description' content = 'Правата ми при закъснял, отеменен полет или отказан достъп до борда. Правата ми при полет съгласно регламент 261/2004.' />
                           <meta property='og:title' content='HELPCLAIMS | общи условия' />      
                           <meta property='og:url' content='https://helpclaim.eu/общи-условия' />
                           <meta property='article:published_time' content='2018-05-01' />
                           <meta property='article:modified_time' content='2018-06-04' />
                           <meta property='og:site_name' content = 'Проблеми с полета | общи условия ХЕЛПКЛЕЙМС' />
                           <meta property='og:image' content = 'https://helpclaim.eu/Content/Assets/Logo/link.png' />
                           <meta property='article:tag' content = 'искове' />
                           <meta property='article:tag' content = 'претенции' />
                           <meta property='article:tag' content = 'полети' />
                           <meta property='article:tag' content = 'общи условия' />";
                           

            }
            else if (path.Contains("контакти"))
            {
                result = @"<title>обезщетение при полет | подаване  на иск</title>
                           <meta name='robots' content='index, follow' />
                           <meta name='description' content='Подаване на иск за обезщетение при полет.' />
                           <meta name='keywords' content='закъснял полет, отменен полет, Подаване на иск за, отменен полет, отказан достъп до борда или. ' />
                           <meta property='og:type' content='article' />
                           <meta property='og:description' content = 'Правата ми при закъснял, отеменен полет или отказан достъп до борда. Правата ми при полет съгласно регламент 261/2004.' />
                           <meta property='og:title' content='HELPCLAIMS | общи условия' />      
                           <meta property='og:url' content='https://helpclaim.eu/обезщетение-при-полет' />
                           <meta property='article:published_time' content='2018-05-01' />
                           <meta property='article:modified_time' content='2018-06-04' />
                           <meta property='og:site_name' content = 'Проблеми с полета | общи условия ХЕЛПКЛЕЙМС' />
                           <meta property='og:image' content = 'https://helpclaim.eu/Content/Assets/Logo/link.png' />
                           <meta property='article:tag' content = 'иск за обезщетени' />
                           <meta property='article:tag' content = 'претенция' />
                           <meta property='article:tag' content = 'полет' />
                           <meta property='article:tag' content = 'закъснял полет' />
                           <meta property='article:tag' content = 'отменен полет' />
                           <meta property='article:tag' content = 'отказан досъп до борда' />";
                          

            }
            else if (path.Contains("калкулиране-на-обезщетение"))
            {
                result = @"<title>Калкулиране на обезщетение при полет </title>
                           <meta name='robots' content='index, follow' />
                           <meta name='description' content='Калкулиране на обезщетение при полет.' />
                           <meta name='keywords' content='калкулиране на обезщетение, обезщетение закъснял полет,обезщетение отменен полет, размер, обезщетение, право на обезщетение' />
                           <meta property='og:type' content='article' />
                           <meta property='og:description' content = 'Правата ми при закъснял, отеменен полет или отказан достъп до борда. Правата ми при полет съгласно регламент 261/2004.' />
                           <meta property='og:title' content='HELPCLAIMS | общи условия' />      
                           <meta property='og:url' content='https://helpclaim.eu/калкулиране-на-обезщетение' />
                           <meta property='article:published_time' content='2018-05-01' />
                           <meta property='article:modified_time' content='2018-06-04' />
                           <meta property='og:site_name' content = 'Проблеми с полета | общи условия ХЕЛПКЛЕЙМС' />
                           <meta property='og:image' content = 'https://helpclaim.eu/Content/Assets/Logo/link.png' />
                           <meta property='article:tag' content = 'право' />
                           <meta property='article:tag' content = 'право на обезщетение' />
                           <meta property='article:tag' content = 'размер' />
                           <meta property='article:tag' content = 'размер на обезщетение' />
                           <meta property='article:tag' content = 'рамер на обезщетение' />
                           <meta property='article:tag' content = 'калулиране на обезщетение' />
                           <meta property='article:tag' content = 'калулиране на обезщетение голямо закъснение' />
                          <meta property='article:tag' content = 'калулиране на обезщетение отмене н полет' />";

            }

            return result.Replace('\'', '"');
        }
    }

}
