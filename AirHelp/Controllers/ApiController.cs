using System.Net.Http;
using System.Web.Mvc;
using AirHelp.Hellpers;

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

    }
}