﻿using Facebook;
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
using AirHelp.Hellpers;

namespace AirHelp.Controllers
{


    public class ApiController : BaseController
    {

        private static readonly HttpClient client = new HttpClient();



        [HttpGet]
        [Route("api/getFlight")]
        public string GetFlight(string number, string date)
        {
            var flight = CommonHeppler.GetFlight(number, date);
            return flight.ToString();


        }

        
        [HttpGet]
        [Route("api/airports")]
        public string GetAirport(string text)
        {
            string result = "";

            result = CommonHeppler.GetAirport(text);

            return result;
        }

        [HttpGet]
        [Route("api/airline")]
        public string GetAirlines(string text)
        {
            string result = "";

            result = CommonHeppler.GetAirlines(text);

            return result;
        }

    }
}