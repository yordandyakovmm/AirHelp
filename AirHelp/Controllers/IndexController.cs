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

namespace AirHelp.Controllers
{


    public class IndexController : BaseController
    {

        private static readonly HttpClient client = new HttpClient();

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
        async public Task<string> GetAirline(string id)
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
        

        [Route("{item}/{category}")]
        public ActionResult Spliter(string category, string item)
        {
            //var contex = new DAL.AirHelpDBContext();
            //var count = contex.Users.Count();
            return View("Index");


        }

        [HttpGet]
        [Route("обезщетение-при-полет/{category}")]
        public ActionResult Spliter1(string category)
        {
            ViewBag.category = category;
            return View("RegisterClaim");
        }

        [HttpGet]
        [Route("контакти")]
        public ActionResult Spliter14()
        {
            return View("ContactForm");
        }

        [HttpGet]
        [Route("вход")]
        public ActionResult Login(string ReturnUrl)
        {
            return View("Login");
        }

        [HttpPost]
        [Route("вход")]
        public ActionResult Login(string Email, string Password, string ReturnUrl)
        {

            string hashPassword = GetHash(Password);

            User user = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                user = dc.Users.Where(u => u.Email == Email && u.password == hashPassword).SingleOrDefault();
            }

            if (user == null)
            {
                ViewBag.error = "Грешно потребителско име или парола";
                return View("Login");
            }

            var VMuser = new VMUser()
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PictureUrl = user.PictureUrl,
                Role = user.Role
            };

            Session["user"] = user;

            FormsAuthenticationTicket authTicket =
                new FormsAuthenticationTicket(1, user.UserId, DateTime.Now, DateTime.Now.AddMinutes(200), true, user.Role, "/");
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                               FormsAuthentication.Encrypt(authTicket));
            Response.Cookies.Add(cookie);
            return Redirect(ReturnUrl);

        }

        [Route("изход")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Remove("user");
            return Redirect("/");
        }

        [HttpGet]
        [Route("за-нас")]
        public ActionResult Spliter15()
        {
            return View("ForUs");
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
                file.SaveAs(Server.MapPath("~/UserDocuments/" + name));
            }
            if (Request.Files["BookConfirmation"].ContentLength > 0)
            {
                var file = Request.Files["BookConfirmation"];
                var name = Guid.NewGuid() + "." + file.FileName.Split('.')[1];
                BookConfirmationUrl = $"/UserDocuments/{name}";
                file.SaveAs(Server.MapPath("~/UserDocuments/" + name));
            }

            Guid newGuid = Guid.NewGuid();
            string AttorneyUrl = $"/UserDocuments/{newGuid}.pdf";

            var jsonString = Request.Form["json"];

            var json = new JavaScriptSerializer();
            Rootobject data = json.Deserialize<Rootobject>(jsonString);
                       

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
                AirCompany = data.airline.al_name,
                AirCompanyCountry = data.airline.country,
                AdditionalInfo = Request.Form["AdditionalInfo"],
                Confirm = Request.Form["Confirm"],
                Arival = Request.Form["Arival"],
                DocumentSecurity = Request.Form["DocumentSecurity"],
                Willness = Request.Form["Willness"],
                Delay = Request.Form["Delay"],
                SignitureImage = Request.Form["SignitureImage"],
                AttorneyUrl = AttorneyUrl
            };

            data.airports.ToList().ForEach(a => {
                AirPort airport = new AirPort {
                    Id = Guid.NewGuid(),
                    ap_name = a.ap_name,
                    city = a.city,
                    country = a.country,
                    elevation = int.Parse(a.elevation),
                    iata = a.iata,
                    name = a.name,
                    timezone = double.Parse(a.timezone),
                    icao = a.icao,
                    type = a.type,
                    x = double.Parse(a.x),
                    y = double.Parse(a.y)
                };

                claim.AirPorts.Add(airport);
            });
            
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                dc.Claims.Add(claim);
                dc.SaveChanges();
            }
            
            string port = Request.Url.Port == 80 ? string.Empty : $":{Request.Url.Port.ToString()}";

            String url = $"{Request.Url.Scheme}://{Request.Url.Host}{port}/attorneyPdf/{claim.ClaimId}";

            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            SelectPdf.PdfDocument doc = converter.ConvertUrl(url);
            doc.Save(Server.MapPath($"~/UserDocuments/{newGuid}.pdf"));
            doc.Close();


            return Redirect($"/обезщетение-списък/{claim.ClaimId}");

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

            var model = new VMClaim(claim);

            return View("ViewClaim", model);
        }

        [Authorize(Roles = "admin,user")]
        [Route("обезщетение-списък")]
        public ActionResult Spliter7(string category)
        {
            var list = new List<VMClaim>();
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                list = dc.Claims.Where(l => true).Select(claim => claim)
                    .ToList()
                    .Select(claim => new VMClaim(claim))
                    .ToList();
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
            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Where(c => c.ClaimId == id).SingleOrDefault();
            }

            var model = new VMClaim(claim);

            return PartialView("Attorney", model);
        }

        [Route("attorneyPdf/{id}")]
        public ActionResult Spliter12(Guid id)
        {

            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Where(c => c.ClaimId == id).SingleOrDefault();
            }

            var model = new VMClaim(claim);

            return PartialView("AttorneyPdf", model);

        }

        [Route("пълномощно/{id}")]
        public ActionResult Spliter14(Guid id)
        {
            string port = Request.Url.Port == 80 ? string.Empty : $":{Request.Url.Port.ToString()}";

            String url = $"{Request.Url.Scheme}://{Request.Url.Host}{port}/attorneyPdf/{id}";

            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            SelectPdf.PdfDocument doc = converter.ConvertUrl(url);
            Response.ContentType = "application/pdf";
            //doc.Save(Server.MapPath($"~/UserDocuments/{newGuid}.pdf"));
            doc.Save(Response.OutputStream);
            doc.Close();
            Response.End();
            return null;
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