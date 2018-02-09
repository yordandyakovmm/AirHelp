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

namespace AirHelp.Controllers
{


    public class ApiController : BaseController
    {

        private static readonly HttpClient client = new HttpClient();

        
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