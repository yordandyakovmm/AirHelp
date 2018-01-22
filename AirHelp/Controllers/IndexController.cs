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

namespace AirHelp.Controllers
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

        [Route("{item}/{category}")]
        public ActionResult Spliter(string category, string item)
        {
            //var contex = new DAL.AirHelpDBContext();
            //var count = contex.Users.Count();
            return View("RegisterClaim");


        }

        [HttpGet]
        [Route("обезщетение-при-полет/{category}")]
        public ActionResult Spliter1(string category)
        {
            ViewBag.category = category;
            return View("RegisterClaim");
        }

        [HttpPost]
        [Route("обезщетение-при-полет/{category}")]
        public ActionResult Spliter5(string category)
        {
            ViewBag.category = category;

            string BordCardUrl = "";
            string BookConfirmationUrl = "";

            if (Request.Files["BordCard"].ContentLength > 0)
            {
                var file = Request.Files["BordCard"];
                var name = Guid.NewGuid() + "." + file.FileName.Split('.')[1];
                BordCardUrl = $"/UserDocuments/{name}";
                file.SaveAs(Server.MapPath("~/UserDocuments/" + name ));
            }
            if (Request.Files["BookConfirmation"].ContentLength > 0)
            {
                var file = Request.Files["BordCard"];
                var name = Guid.NewGuid() + "." + file.FileName.Split('.')[1];
                BookConfirmationUrl = $"/UserDocuments/{name}";
                file.SaveAs(Server.MapPath("~/UserDocuments/" + name ));
            }

            Claim claim = new Claim
            {
                ClaimId = Guid.NewGuid(),
                State = "приета",

                UserId = null,
                DateCreated = DateTime.Now,

                BordCardUrl = BordCardUrl,
                BookConfirmationUrl = BookConfirmationUrl,
                Type = Request.Form["Type"],
                ConnectionAriports = Request.Form["ConnectionAriports"],
                FirstName = Request.Form["FirstName"],
                LastName = Request.Form["LastName"],
                City = Request.Form["City"],
                Country = Request.Form["Country"],
                Adress = Request.Form["Adress"],
                Email = Request.Form["Email"],
                Tel = Request.Form["Tel"],
                FlightNumber = Request.Form["FlightNumber"],
                Date = Request.Form["Date"],
                DepartureAirport = Request.Form["DepartureAirport"],
                DestinationAirports = Request.Form["DestinationAirports"],
                HasConnection = Request.Form["HasConnection"],
                ConnectionAirports = Request.Form["ConnectionAirports"],
                Reason = Request.Form["Reason"],
                HowMuch = Request.Form["HowMuch"],
                Annonsment = Request.Form["Annonsment"],
                BookCode = Request.Form["BookCode"],
                AirCompany = Request.Form["AirCompany"],
                AdditionalInfo = Request.Form["AdditionalInfo"],
                Confirm = Request.Form["Confirm"],
                Arival = Request.Form["Arival"],
                DocumentSecurity = Request.Form["DocumentSecurity"],
                Willness = Request.Form["Willness"],
                Delay = Request.Form["Delay"]
            };

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                dc.Claims.Add(claim);
                dc.SaveChanges();
            }

            return Redirect($"обезщетение-списък/{claim.ClaimId}");
            
        }

        [Route("обезщетение-при-полет")]
        public ActionResult Spliter2(string category)
        {
            return View("Claim");
        }


        [Route("обезщетение-списък/{id}")]
        public ActionResult Spliter8(Guid id)
        {

            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Where(c => c.ClaimId == id).SingleOrDefault();
            }

            var model = new VMClaim()
            {
                ClaimId = claim.ClaimId,
                State = claim.State,

                User = null,
                DateCreated = claim.DateCreated,

                BordCardUrl = claim.BordCardUrl,
                BookConfirmationUrl = claim.BookConfirmationUrl,
                Type = claim.Type,
                ConnectionAriports = claim.ConnectionAriports,
                FirstName = claim.FirstName,
                LastName = claim.LastName,
                City = claim.City,
                Country = claim.Country,
                Adress = claim.Adress,
                Email = claim.Email,
                Tel = claim.Tel,
                FlightNumber = claim.FlightNumber,
                Date = claim.Date,
                DepartureAirport = claim.DepartureAirport,
                DestinationAirports = claim.DestinationAirports,
                HasConnection = claim.HasConnection,
                ConnectionAirports = claim.ConnectionAirports,
                Reason = claim.Reason,
                HowMuch = claim.HowMuch,
                Annonsment = claim.Annonsment,
                BookCode = claim.BookCode,
                AirCompany = claim.AirCompany,
                AdditionalInfo = claim.AdditionalInfo,
                Confirm = claim.Confirm,
                Arival = claim. Arival,
                DocumentSecurity = claim.DocumentSecurity,
                Willness = claim.Willness,
                Delay = claim.Delay
            };

            return View("ViewClaim", model);
        }


        [Route("обезщетение-списък")]
        public ActionResult Spliter7(string category)
        {
            var list = new List<VMClaim>();
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                list = dc.Claims.Where(l => true).Select(claim => new VMClaim() {
                    ClaimId = claim.ClaimId,
                    State = claim.State,
                    User = claim.User,
                    DateCreated = claim.DateCreated,
                    BordCardUrl = claim.BordCardUrl,
                    BookConfirmationUrl = claim.BookConfirmationUrl,
                    Type = claim.Type,
                    ConnectionAriports = claim.ConnectionAriports,
                    FirstName = claim.FirstName,
                    LastName = claim.LastName,
                    City = claim.City,
                    Country = claim.Country,
                    Adress = claim.Adress,
                    Email = claim.Email,
                    Tel = claim.Tel,
                    FlightNumber = claim.FlightNumber,
                    Date = claim.Date,
                    DepartureAirport = claim.DepartureAirport,
                    DestinationAirports = claim.DestinationAirports,
                    HasConnection = claim.HasConnection,
                    ConnectionAirports = claim.ConnectionAirports,
                    Reason = claim.Reason,
                    HowMuch = claim.HowMuch,
                    Annonsment = claim.Annonsment,
                    BookCode = claim.BookCode,
                    AirCompany = claim.AirCompany,
                    AdditionalInfo = claim.AdditionalInfo,
                    Confirm = claim.Confirm,
                    Arival = claim.Arival,
                    DocumentSecurity = claim.DocumentSecurity,
                    Willness = claim.Willness,
                    Delay = claim.Delay
                }).ToList();
            }
                return View("ClaimList", list);
        }

        [Route("пpолитика-на-поверителност")]
        public ActionResult Spliter3(string category)
        {
            return View("PrivatePolice");
        }

        [Route("attorney/{id}")]
        public ActionResult Spliter11(Guid id)
        {
            return PartialView("Attorney");
        }


        [Route("проблеми-с-полета/често-задавани-въпроси")]
        public ActionResult Spliter4(string category)
        {
            return View("faq");
        }

        [Route("общи-условия")]
        public ActionResult Spliter6(string category)
        {
            return View("CommonRules");
        }


    }
}