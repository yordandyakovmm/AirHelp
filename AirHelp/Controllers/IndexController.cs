﻿using Facebook;
using Newtonsoft.Json.Linq;
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

namespace Recipes.Controllers
{


    public class IndexController : BaseController
    {


        public async System.Threading.Tasks.Task<ActionResult> Index(string studentId, string seond)
        {
            //string fxmlUrl = "http://flightxml.flightaware.com/json/FlightXML3/AirportInfo?airport_code=KIAH";
            //string username = "yordandyakov";
            //string apiKey = ConfigurationManager.AppSettings["flightaware"];
            //var uriBuilder = new UriBuilder(fxmlUrl);
            //var requestUrl = fxmlUrl;

            //           var client = new HttpClient();
            //var credentials = Encoding.ASCII.GetBytes(username + ":" + apiKey);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(credentials));
            //byte[] result = await client.GetByteArrayAsync(new Uri(requestUrl));
            //string sResult = System.Text.Encoding.Default.GetString(result);



            return View();
        }

        public ActionResult Edit(string studentId, string seond)
        {
            return View();
        }

        [HttpGet]
        [Route("api/airports")]
        public string GetAirport(string id)
        {
            string result = "";
            var url = "https://www.save70.com/components/autocompleteJson.php?type=airport&term=" + id;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        [Route("{category}/{item}")]
        [Route("recipes/{category}/{item}")]
        public ActionResult Spliter(string category, string item)
        {
            //var contex = new DAL.AirHelpDBContext();
            //var count = contex.Users.Count();
            return View("RegisterClaim");


        }
    }
}