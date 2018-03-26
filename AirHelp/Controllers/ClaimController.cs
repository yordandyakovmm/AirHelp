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
using AirHelp.Hellpers;

namespace AirHelp.Controllers
{


    public class ClaimController : BaseController
    {

        [HttpGet]
        [Route("калкулиране-на-обезщетение")]
        public ActionResult ColectFlightDataFlight()
        {
            var model = new VMDirectFlight();
            return View("ColectFlightDataFlight", model);
        }

        [HttpPost]
        [Route("калкулиране-на-обезщетение")]
        public ActionResult ColectFlightDataFlightPost()
        {
            

            Guid newGuid = Guid.NewGuid();
            
            var jsonString = Request.Form["jsonAirport"];
            var issueDepartureCode = Request.Form["Flight"];
            var nubmer = Request.Form["FlightNumber"];
            var nubmers = Request.Form["FlightNumbers"];
            var date = Request.Form["Date"];
            var dates = Request.Form["Dates"];

            string[] flightNumbers = nubmers.Split(',');
            string[] flightDates = dates.Split(',');

            var json = new JavaScriptSerializer();
            Airport[] airports = json.Deserialize<Airport[]>(jsonString);
            
            Claim claim = new Claim
            {
                ClaimId = Guid.NewGuid(),
                State = ClaimStatus.WaitForDocument,
                UserId = null,
                DateCreated = DateTime.Now,
                Type = ProblemType.Pending,
             };
                        

            int number = 1;
            airports.ToList().ForEach(a => {
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
                    y = double.Parse(a.y),
                    FlightNumber = flightNumbers[number - 1],
                    FlightDate = flightDates[number - 1],
                    startIssue = (number == 1 && airports.Length == 2) || (a.iata == issueDepartureCode)
                };
                number++;
                claim.AirPorts.Add(airport);
            });

            double allDistance = 0;
            double issuDistance = 0;
            bool flag = false;
            var claimAirPorts = claim.AirPorts.ToArray();
            for (int i = 0; i < claimAirPorts.Length - 1; i++)
            {
                var sCoord = new GeoCoordinate(claimAirPorts[i].y, claimAirPorts[i].x);
                var eCoord = new GeoCoordinate(claimAirPorts[i + 1].y, claimAirPorts[i + 1].x);

                var d  = sCoord.GetDistanceTo(eCoord);
                claimAirPorts[i].distanceToNext = d;
                allDistance += d;
                flag = flag || (claimAirPorts[i].iata == issueDepartureCode);
                if (flag)
                {
                    issuDistance += d;
                }

            }

            claim.issueDistance = issuDistance;
            claim.allDistance = allDistance;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                dc.Claims.Add(claim);
                dc.SaveChanges();
            }

            return View("ColectData", claim);
            
        }

        [HttpGet]
        [Route("проверка-полет")]
        public ActionResult DirctFlight()
        {
            var model = new VMDirectFlight();
            return View("DirectFlight", model);
        }
                
        [HttpPost]
        [Route("проверка-полет")]
        public ActionResult CheckDirctFlightPost(string FlightNumber, string Date)
        {
            // testing
            Claim c = null;
            Guid g = new Guid("ecbc3e69-71c0-4fab-971d-3acc3bd98480");
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                c = dc.Claims.Where(cl => cl.ClaimId == g).SingleOrDefault();


                ViewBag.IsEu = c.AirPorts.Any(a => CommonHeppler.IsEuCountry(a.countryCode));
                ViewBag.delayMessage = (c.issueDistance < 1500000 ? "Полета ви закъсня ли с повече от 2 часа?" :
                    (c.issueDistance < 3500000 || ViewBag.IsEu ? "Полета ви закъсня ли с повече от 3 часа?" : "Полета ви закъсня ли с повече от 3 часа ?"));
            }
                return View("ColectData", c);

            var model = new VMDirectFlight()
            {
                date = Date,
                number = FlightNumber,
                numberError = ""
            };
            var arr = Date.Split('.');
            var day = int.Parse(arr[0]);
            var mont = int.Parse(arr[1]);
            var year = int.Parse(arr[2]);

            FlightStatus fligth = CommonHeppler.GetFlight(FlightNumber,  Date);

            if (fligth.flightStatuses.Length == 0)
            {
                model.commonError = "Невалидна комбинция от номер и дата на полета";
                return View("DirectFlight", model);
            }

            var AirLine = fligth.appendix.airlines.ToList().Find(a => a.iata == fligth.flightStatuses[0].primaryCarrierFsCode);

            Claim claim = new Claim
            {
                ClaimId = Guid.NewGuid(),
                Date = new DateTime( year, mont, day),
                State = ClaimStatus.Accepted,
                UserId = User.Identity.IsAuthenticated ? User.Identity.Name: null,
                DateCreated = DateTime.Now,
                Type = ProblemType.Pending,
                FlightNumber = FlightNumber,
                AirCompany = AirLine.name,
                AirCompanyCountry = ""
            };

            int number = 0;
            fligth.appendix.airports.ToList().ForEach(a => {
                double distance = 0;
                if (number > 0)
                {
                    var sCoord = new GeoCoordinate(fligth.appendix.airports[number-1].longitude, fligth.appendix.airports[number-1].latitude);
                    var eCoord = new GeoCoordinate(fligth.appendix.airports[number].longitude, fligth.appendix.airports[number].latitude);

                    distance = sCoord.GetDistanceTo(eCoord);
                }
                AirPort airport = new AirPort
                {
                    Id = Guid.NewGuid(),
                    ap_name = a.name,
                    city = a.city,
                    cityCode = a.cityCode,
                    country = a.countryName,
                    countryCode = a.countryCode,
                    elevation = a.elevationFeet,
                    iata = a.iata,
                    number = number,
                    name = a.name,
                    timezone = 0,
                    icao = a.icao,
                    type =  "",
                    x = a.latitude,
                    y = a.longitude,
                    distanceToNext = distance
                };
                number++;
                claim.AirPorts.Add(airport);
            });

            claim.allDistance = claim.AirPorts.Sum(a => a.distanceToNext);

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                dc.Claims.Add(claim);
                dc.SaveChanges();
            }

            return View("ColectData", claim);
        }

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
                State = ClaimStatus.Accepted,

                UserId = null,
                DateCreated = DateTime.Now,

                
                Type = ProblemType.Cancel,
                FlightNumber = Request.Form["FlightNumber"],
                
               
                AirCompany = data.airline.al_name,
                AirCompanyCountry = data.airline.country,
                AdditionalInfo = Request.Form["AdditionalInfo"],
                Confirm = Request.Form["Confirm"],
              
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
                    UserId = claim.User.Email,
                    FirstName = claim.User.FirstName,
                    LastName = claim.User.LastName,
                    Email = claim.User.Email,
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