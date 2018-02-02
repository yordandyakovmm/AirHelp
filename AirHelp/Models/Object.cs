using AirHelp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHelp.Models
{

    public class Rootobject
    {
        public Airport[] airports { get; set; }
        public Airline airline { get; set; }
    }

    public class Airline
    {
        public string name { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public string callsign { get; set; }
        public string country { get; set; }
        public string alid { get; set; }
        public object alias { get; set; }
        public string mode { get; set; }
        public string active { get; set; }
        public object al_uid { get; set; }
        public string al_name { get; set; }
    }

    public class Airport
    {
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string elevation { get; set; }
        public string apid { get; set; }
        public string timezone { get; set; }
        public string dst { get; set; }
        public string tz_id { get; set; }
        public string type { get; set; }
        public string source { get; set; }
        public object ap_uid { get; set; }
        public string ap_name { get; set; }
    }


}