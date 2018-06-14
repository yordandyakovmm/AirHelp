using System.Net.Http;
using System.Web.Mvc;
using AirHelp.Hellpers;
using AirHelp.Models;
using AirHelp.DAL;
using System;

namespace AirHelp.Controllers
{


    public class ApiController : BaseController
    {

        private static readonly HttpClient client = new HttpClient();


        [HttpGet]
        [Route(".well-known/acme-challenge/htOKzuTvKzAtuvrWzfwjo1IEys5JdK-cLGNZ_mKjCTo")]
        public string ser(string number, string date)
        {
            return "htOKzuTvKzAtuvrWzfwjo1IEys5JdK-cLGNZ_mKjCTo.u683fKQxYVE_cg0O94ll8EBznr4Xt-M3PVdwgg_lHPs";


        }

        [HttpGet]
        [Route(".well-known/acme-challenge/j7cORfyihIOIQHjgAc4juv_dMEyzQNRA33N8cQqlZdg")]
        public string ser1(string number, string date)
        {
            return "j7cORfyihIOIQHjgAc4juv_dMEyzQNRA33N8cQqlZdg.u683fKQxYVE_cg0O94ll8EBznr4Xt-M3PVdwgg_lHPs";


        }

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


        [HttpGet]
        [Route("api/data")]
        public string data()
        {
            string result = "";

            string[] lines = System.IO.File.ReadAllLines("D:\\AirHelp\\Data\\airports1.csv");

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                int i = 1;
                foreach (string r in lines)
                {

                    var arr = r.Split(',');
                    DAL.AirPortData air = dc.AirPortsData.Create();
                    if (r.Contains("medium_airport") /*|| r.Contains("large_airport")*/)
                    {
                        try
                        {
                            air.name = arr[3].Trim('"').Trim('\\').Trim('"');
                            air.city = arr[10].Trim('"').Trim('\\').Trim('"');
                            air.country = arr[8].Trim('"').Trim('\\').Trim('"');
                            air.countryCode = arr[8].Trim('"').Trim('\\').Trim('"');
                            air.x = double.Parse(arr[5]);
                            air.y = double.Parse(arr[4]);
                            double e = 0;
                            double.TryParse(arr[6], out e);
                            air.elevation = e;
                            air.iata = arr[13].Trim('"').Trim('\\').Trim('"');
                            air.timezone = 0;
                            air.url = arr[15].Trim('"').Trim('\\').Trim('"');
                            if (air.name.Length > 90 || air.iata.Length > 4)
                            {
                                continue;
                            }
                            dc.AirPortsData.Add(air);
                        }
                        catch (Exception ex)
                        {

                            continue;
                        }


                        try
                        {
                            dc.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                    }

                   

                }

            }
            return result;
        }

    }
}