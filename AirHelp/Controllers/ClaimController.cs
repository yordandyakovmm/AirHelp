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


    public class ClaimController : BaseController
    {

        [HttpGet]
        [Route("обезщетение-при-полет/{category}")]
        public ActionResult RegisterClaim(string category)
        {
            ViewBag.category = category;
            return View("RegisterClaim");
        }

        [HttpPost]
        [Route("обезщетение-при-полет/{category}")]
        public ActionResult RegisterClaimSave(string category)
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

            int number = 1;
            data.airports.ToList().ForEach(a => {
                AirPort airport = new AirPort
                {
                    Id = Guid.NewGuid(),
                    ap_name = a.ap_name,
                    city = a.city,
                    country = a.country,
                    elevation = int.Parse(a.elevation),
                    iata = a.iata,
                    number = number,
                    name = a.name,
                    timezone = double.Parse(a.timezone),
                    icao = a.icao,
                    type = a.type,
                    x = double.Parse(a.x),
                    y = double.Parse(a.y)
                };
                number++;

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

            LogWithUser("temp", "temp");

            return Redirect($"/обезщетение-списък/{claim.ClaimId}");

        }

        [Route("обезщетение-при-полет")]
        public ActionResult ClaimSpliter(string category)
        {
            return View("ClaimSpliter");
        }

        [HttpGet]
        [Route("обезщетение-списък/{id}")]
        [Authorize]
        public ActionResult ClaimDetail(Guid id)
        {

            Claim claim = null;
            List<AirPort> airPorts = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Where(c => c.ClaimId == id).SingleOrDefault();

                airPorts = claim.AirPorts.OrderBy(a => a.number).ToList();

            }
            var model = new VMClaim(claim);


            model.totalDistance = 0;

            for (int i = 0; i < airPorts.Count - 1; i++)
            {
                var sCoord = new GeoCoordinate(airPorts[i].y, airPorts[i].x);
                var eCoord = new GeoCoordinate(airPorts[i + 1].y, airPorts[i + 1].x);

                var distance = sCoord.GetDistanceTo(eCoord);

                var AirportDistance = new AirportDistance
                {
                    From = $"{airPorts[i].name} ({airPorts[i].iata})",
                    To = $"{airPorts[i + 1].name} ({airPorts[i + 1].iata})",
                    distance = distance / 1000
                };
                model.totalDistance = model.totalDistance + distance / 1000;
                model.AirporstDistance.Add(AirportDistance);
            }

            model.rightOfCompensation = true;
            model.CompensationAmount = 250;
            if (model.totalDistance >= 3500)
            {
                model.CompensationAmount = 600;
            }
            else if (model.totalDistance >= 1500)
            {
                model.CompensationAmount = 450;
            }
            return View("ViewClaim", model);
        }

        [HttpPost]
        [Authorize(Roles = "admin,temp")]
        [Route("обезщетение-списък/{id}")]
        public ActionResult ClaimUpdate(Guid id)
        {
            Claim claim = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Where(c => c.ClaimId == id).SingleOrDefault();

                var password = Request.Form["password"];
                var newUserBD = new User()
                {
                    UserId = claim.Email,
                    FirstName = claim.FirstName,
                    LastName = claim.LastName,
                    Email = claim.Email,
                    password = GetHash(password),
                    PictureUrl = "",
                    CreateDate = DateTime.Now,
                    Role = "user"
                };

                dc.Users.Add(newUserBD);
                claim.UserId = newUserBD.UserId;
                dc.SaveChanges();

            }

            return Redirect($"/обезщетение-списък/{claim.ClaimId}");

        }

        [Authorize(Roles = "admin,user")]
        [Route("обезщетение-списък")]
        public ActionResult ClaimList(string category)
        {
            var list = new List<VMClaim>();
            var isAdmin = User.IsInRole("admin");
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                list = dc.Claims.Where(c => c.UserId == User.Identity.Name || isAdmin).Select(claim => claim)
                    .ToList()
                    .Select(claim => new VMClaim(claim))
                    .ToList();
            }
            return View("ClaimList", list);
        }



    }
}