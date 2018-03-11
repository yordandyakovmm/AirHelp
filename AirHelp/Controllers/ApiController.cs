using Facebook;
using Newtonsoft.Json.Linq;
using AirHelp.DAL;
using AirHelp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Device.Location;
using Newtonsoft.Json;

namespace AirHelp.Controllers
{


    public class ApiController : BaseController
    {

        private static readonly HttpClient client = new HttpClient();



        [HttpGet]
        [Route("api/getFlight")]
        async public Task<JsonResult> GetAirport(string number, string date)
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
            
            var response = await client.GetAsync(url);

            json = await response.Content.ReadAsStringAsync();
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            FlightStatus flight = JsonConvert.DeserializeObject<FlightStatus>(json);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        [Route("api/airports")]
        async public Task<string> GetAirport(string id)
        {
            string result = "";
            var url = "https://openflights.org/php/apsearch.php";
            var values = new Dictionary<string, string>
                {
                      {"name" , id},
                      {"country", "ALL"},
                      {"action", "SEARCH"},
                      {"offset", "0"}
                };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(url, content);

            result = await response.Content.ReadAsStringAsync();

            return result;
        }

        [HttpGet]
        [Route("api/airline")]
        async public Task<string> GetAirlines(string id)
        {
            string result = "";
            var url = "https://openflights.org/php/alsearch.php";
            var values = new Dictionary<string, string>
                {
                      {"name" , id},
                      {"country", "ALL"},
                      {"action", "SEARCH"},
                      {"mode", "F" },
                      {"iatafilter", "true" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(url, content);

            result = await response.Content.ReadAsStringAsync();
            result = result.Substring(result.IndexOf('{')).Replace("\n", ",");
            result = "{\"status\": 1, \"airports\": [" + result + "]}";

            return result;
        }

    }
}