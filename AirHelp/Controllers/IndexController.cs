using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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